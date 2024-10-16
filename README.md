# .NET nested transaction issue repository
The repository contains some examples that show how nested transaction scope can close an ambient transaction in .NET. This behavior shortly described by Microsoft in [Voting inside a nested scope](https://learn.microsoft.com/en-us/dotnet/framework/data/transactions/implementing-an-implicit-transaction-using-transaction-scope#voting-inside-a-nested-scope) section.

## How to run

1. Start **PostgreSQL** by running the following command:
    ```
    docker compose up -d postgres
    ```
1. Run the **ConsoleApp** in `Visual Studio`/`Rider` or by the command:
    ```
    cd ConsoleApp | dotnet run
    ```

## Examples

All examples are placed in `/ConsoleApp/Examples/` folder. **ConsoleApp** runs them sequentially. The result of each example is a thrown exception with the message:
```
An exception occurred in the database while saving changes for context type 'ConsoleApp.Database.AppDbContext'.
System.Transactions.TransactionException: The operation is not valid for the state of the transaction.
```

This message points out that ambient transaction was aborted. You can also check a transaction state in Debug mode.

Additionally, in the source code you can find explanatory comments that will help you understand the flow.
