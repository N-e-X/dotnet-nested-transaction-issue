using ConsoleApp.Database;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace ConsoleApp.Examples;

public sealed class WhenReturnsWithoutCallingCompleteExample : IExample
{
    private readonly AppDbContext _db;
    private readonly ILogger<WhenReturnsWithoutCallingCompleteExample> _logger;

    public WhenReturnsWithoutCallingCompleteExample(AppDbContext db, ILogger<WhenReturnsWithoutCallingCompleteExample> logger)
    {
        _db = db;
        _logger = logger;
    }

    public void Outer()
    {
        // outerScope opens a transaction that is ambient for Inner()
        using var outerScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);

        Inner(); // ambient transaction is expected to be aborted here

        _db.Add(new UserEntity { Name = "Artem" });
        _db.SaveChanges(); // throws the exception that transaction has been already aborted

        // code below is never invoked since the exception was already thrown

        var addedUser = _db.Users.First();
        _logger.LogInformation("User {UserName} was added", addedUser.Name);

        outerScope.Complete();
    }

    public void Inner()
    {
        // innerScope uses the ambient transaction by TransactionScopeOption.Required
        using var innerScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);

        // work with db omitted for brevity

        // in a real-world scenario innerScope.Complete() more likely is present in the end of the method but might be missed in 'if' block
        var someCondition = true;
        if (someCondition)
            return; // forces innerScope to call Dispose() in hidden 'finally' block. This causes the ambient transaction to be aborted

        innerScope.Complete(); // never invoked
    }
}
