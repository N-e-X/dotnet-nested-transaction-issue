namespace ConsoleApp.Database;

public sealed class ErrorEntity
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
}
