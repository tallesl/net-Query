<p align="center">
    <a href="#query">
        <img alt="logo" src="Assets/logo-200x200.png">
    </a>
</p>

# Query

[![][build-img]][build]
[![][nuget-img]][nuget]

A simplistic ADO.NET wrapper.

[build]:     https://ci.appveyor.com/project/TallesL/net-Query
[build-img]: https://ci.appveyor.com/api/projects/status/github/tallesl/net-Query?svg=true
[nuget]:     https://www.nuget.org/packages/Query/
[nuget-img]: https://badge.fury.io/nu/Query.svg

## Instantiating

```cs
var query = new Query("YourConnectionStringName");
```

May throw [NoSuchConnectionStringException], [EmptyConnectionStringException] or [EmptyProviderNameException].

[NoSuchConnectionStringException]: https://github.com/tallesl/ConnectionStringReader/tree/master/ConnectionStringReader/Exceptions/NoSuchConnectionStringException.cs
[EmptyConnectionStringException]:  https://github.com/tallesl/ConnectionStringReader/tree/master/ConnectionStringReader/Exceptions/EmptyConnectionStringException.cs
[EmptyProviderNameException]:      https://github.com/tallesl/ConnectionStringReader/tree/master/ConnectionStringReader/Exceptions/EmptyProviderNameException.cs

## Modifying data

```cs
query.Change("DELETE FROM Users WHERE Name LIKE @NameToDelete", new { NameToDelete = "John" });
```

You can also make sure how many rows will be affected with:

* `ChangeExactly(n, sql, parameters)`;
* `ChangeNoLessThan(n, sql, parameters)`;
* `ChangeNoMoreThan(n, sql, parameters)`.

[UnexpectedNumberOfRowsAffectedException] is thrown and the transaction is rolled back if the amount of affected rows is
different from the expected.

[UnexpectedNumberOfRowsAffectedException]: Library/Public/Exceptions/UnexpectedNumberOfRowsAffectedException.cs

## Retrieving data

```cs
int count = query.SelectSingle<int>("SELECT COUNT(0) FROM Users");
DataTable dt = query.Select("SELECT * FROM Users WHERE Id = @UserId", new { UserId = 1 });
User usr = query.Select<User>("SELECT * FROM Users WHERE Id = @UserId", new { UserId = 1337 });
```

You can also make sure how many rows will be selected with:

* `SelectExactly(n, sql, parameters)`;
* `SelectNoLessThan(n, sql, parameters)`;
* `SelectNoMoreThan(n, sql, parameters)`.

[UnexpectedNumberOfRowsSelectedException] is thrown if the amount of selected rows is different from the expected.

[UnexpectedNumberOfRowsSelectedException]: Library/Public/Exceptions/UnexpectedNumberOfRowsSelectedException.cs

## Behind the covers

The constructor uses [ConfigurationManager.ConnectionStrings], `Change` uses [SqlCommand.ExecuteNonQuery], `Select` uses
[DbDataAdapter.Fill]&nbsp;(except `SelectSingle` that uses [SqlCommand.ExecuteScalar]).

[ConfigurationManager.ConnectionStrings]: https://msdn.microsoft.com/library/System.Configuration.ConfigurationManager.ConnectionStrings
[SqlCommand.ExecuteNonQuery]:             https://msdn.microsoft.com/library/System.Data.SqlClient.SqlCommand.ExecuteNonQuery
[DbDataAdapter.Fill]:                     https://msdn.microsoft.com/library/System.Data.Common.DbDataAdapter.Fill
[SqlCommand.ExecuteScalar]:               https://msdn.microsoft.com/library/System.Data.SqlClient.SqlCommand.ExecuteScalar

## Options

There's a `QueryOptions` with the following flags:

* `ArrayAsInClause`: Arrays are expanded to IN clauses.
* `EnumAsString`: Treat enum values as strings (rather than as integers).
* `ManualClosing`: Connection/transaction closing should be done manually (see below).
* `Safe`: Throw if a selected property is not found in the given type.

They all default to `False`.

## IN clauses

If `ArrayAsInClause` is set to `True`, the library automatically prepares collections ([IEnumerable]) for [IN] clauses
([taking that burden off you]).

So this:

```cs
query.Change("DELETE FROM Users WHERE Id IN (@Ids)", new { Ids = new[] { 1, 123, 44 } });
```

Becomes this:

```sql
DELETE FROM Users WHERE Id IN (@Ids0, @Ids1, @Ids2)
```

Note that to do this the library concatenates SQL on its own.
**This gives opening for [SQL injection], never use this feature with unsanitized user input.**

[IN]:                         https://msdn.microsoft.com/library/ms177682
[IEnumerable]:                https://msdn.microsoft.com/library/System.Collections.IEnumerable
[taking that burden off you]: http://stackoverflow.com/q/337704/1316620
[SQL injection]:              https://en.wikipedia.org/wiki/SQL_injection

## Connections and Transactions

If `ManualClosing` is set to `False`, the library opens a connection (and a transaction for writes) and closes for every
operation.
There's no need to call [Dispose].

```cs
var query = new Query("YourConnectionStringName"); // false is the default for ManualClosing

// Opens and closes a connection and a transaction
query.Change("INSERT INTO Foo VALUES ('Bar')");

// Opens and closes a connection (again)
// 'Foo' will have 'Bar' in the database, despite the exception here
query.Select("some syntax error");
```

If `ManualClosing` is set to `True`, it automatically opens the connection and transaction and reuses it for each
consecutive command.
The open connection and eventual open transaction are closed/committed when you call `Close()` (hence "manual closing").

```cs
var query = new Query("YourConnectionStringName", new QueryConfiguration { ManualClosing = true });

// Opens a connection and a transaction
query.Change("INSERT INTO Foo VALUES ('Bar')");

// Reuses the connection (and transaction) opened above
// 'Foo' won't have 'Bar' in the database, the exception here rollbacks the transaction
query.Select("some syntax error");

// Commits the transaction and closes the connection
// (won't reach here in this particular example because the line
// above raised  an exception and rolled back, but you get the point)
query.Close();
```

If you don't plan to reuse the object you may shield its usage with `using`:

```cs
// This is equivalent to the example above
using (var query = new Query("YourConnectionStringName", new QueryConfiguration { ManualClosing = true }))
{
    query.Change("INSERT INTO Foo VALUES ('Bar')");
    query.Select("some syntax error");
}
```

[Dispose]: https://msdn.microsoft.com/library/System.IDisposable.Dispose

## Thread safety

Query isn't thread safe, but it should be lightweight enough to be instantiated as needed during the lifetime of your
application (such as one per request).
