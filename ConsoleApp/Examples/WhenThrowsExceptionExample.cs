using ConsoleApp.Database;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace ConsoleApp.Examples;

public sealed class WhenThrowsExceptionExample : IExample
{
    private readonly AppDbContext _db;
    private readonly ILogger<WhenThrowsExceptionExample> _logger;

    public WhenThrowsExceptionExample(AppDbContext db, ILogger<WhenThrowsExceptionExample> logger)
    {
        _db = db;
        _logger = logger;
    }

    public void Outer()
    {
        // outerScope opens a transaction that is ambient for Inner()
        using var outerScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);

        try
        {
            Inner(); // exception is expected to be thrown here

            _db.Add(new UserEntity { Name = "Artem" });
            _db.SaveChanges();

            var addedUser = _db.Users.First();
            _logger.LogInformation("User {UserName} was added", addedUser.Name);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Inner() threw and exception. Trying to save the error into db...");
            _db.Errors.Add(new() { Description = ex.Message });
            _db.SaveChanges(); // throws the exception that transaction has been already aborted
        }
        outerScope.Complete(); // never invoked since the exception is thrown in catch block
    }

    public void Inner()
    {
        // innerScope uses the ambient transaction by TransactionScopeOption.Required
        using var innerScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);

        // work with db omitted for brevity

        throw new Exception("Test message"); // forces innerScope to call Dispose() in hidden finally block. This causes the ambient transaction to be aborted

        innerScope.Complete(); // never invoked
    }
}
