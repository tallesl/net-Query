# QuickQuery

[![][build-img]][build]
[![][nuget-img]][nuget]

A simplistic ADO.NET wrapper.

[build]:     https://ci.appveyor.com/project/TallesL/QuickQuery
[build-img]: https://ci.appveyor.com/api/projects/status/github/tallesl/QuickQuery

[nuget]:     http://badge.fury.io/nu/QuickQuery
[nuget-img]: https://badge.fury.io/nu/QuickQuery.png

## Instantiating

```cs
var qckQuery = new QuickQuery("YourConnectionStringName");
```

The constructor uses [ConfigurationManager.ConnectionStrings] underneath. May throw [NoSuchConnectionStringException], [EmptyConnectionStringException] or even [EmptyProviderNameException].

[ConfigurationManager.ConnectionStrings]: http://msdn.microsoft.com/library/system.configuration.configurationmanager.connectionstrings.aspx

[NoSuchConnectionStringException]: https://github.com/tallesl/ConnectionStringReader/tree/master/ConnectionStringReader/Exceptions/NoSuchConnectionStringException.cs
[EmptyConnectionStringException]:  https://github.com/tallesl/ConnectionStringReader/tree/master/ConnectionStringReader/Exceptions/EmptyConnectionStringException.cs
[EmptyProviderNameException]:      https://github.com/tallesl/ConnectionStringReader/tree/master/ConnectionStringReader/Exceptions/EmptyProviderNameException.cs

## Querying without return

```cs
qckQuery.NoReturn("DELETE FROM Users WHERE Name LIKE @NameToDelete", "NameToDelete", "John");
```

or

```cs
qckQuery.NoReturn("DELETE FROM Users WHERE Name LIKE @NameToDelete", new { NameToDelete = "John" });
```

You can also make sure how many rows will be affected with:

* `ChangeExactly(n, sql, parameters)`;
* `ChangeNoMoreThan(n, sql, parameters)`.

[UnexpectedNumberOfRowsAffected] is thrown and the transaction is rolled back if the amount of affected rows is different from the expected.

It uses [SqlCommand.ExecuteNonQuery] underneath.

[UnexpectedNumberOfRowsAffected]: QuickQuery/Exception/Querying/UnexpectedNumberOfRowsAffected.cs
[SqlCommand.ExecuteNonQuery]:     http://msdn.microsoft.com/library/system.data.sqlclient.sqlcommand.executenonquery.aspx

## Querying with return

```cs
int userCount = qckQuery.SelectSingle<int>("SELECT COUNT(0) FROM Users");
```

```cs
DataTable user1337 = qckQuery.Select("SELECT * FROM Users WHERE Id = @UserId", "UserId", 1337);
```

or

```cs
DataTable user1337 = qckQuery.Select("SELECT * FROM Users WHERE Id = @UserId", new { UserId = 1333 });
```

You can also make sure how many rows will be selected with:

* `SelectExactly(n, sql, parameters)`;
* `SelectNoMoreThan(n, sql, parameters)`.

[UnexpectedNumberOfRowsSelected] is thrown if the amount of selected rows is different from the expected.

`SelectSingle` uses [SqlCommand.ExecuteScalar] while the others uses [DbDataAdapter.Fill] underneath.

[UnexpectedNumberOfRowsSelected]: QuickQuery/Exception/Querying/UnexpectedNumberOfRowsSelected.cs
[SqlCommand.ExecuteScalar]:       http://msdn.microsoft.com/library/system.data.sqlclient.sqlcommand.executescalar.aspx
[DbDataAdapter.Fill]:             http://msdn.microsoft.com/library/system.data.common.dbdataadapter.fill.aspx

## IN clauses

It automatically prepares collections ([IEnumerable]) for [IN] clauses ([taking that burden off you][so]).

So this:

```cs
qckQuery.NoReturn("DELETE FROM Users WHERE Id = (@Ids)", "Ids", new[] { 1, 123, 44 });
```

Becomes this:

```sql
DELETE FROM Users WHERE Id = (@Ids0, @Ids1, @Ids2)
```

Note that to do this the library concatenates SQL on its own.
**This gives opening for [SQL injection], never use this with unsanitized user input.**

[IN]:          https://msdn.microsoft.com/library/ms177682.aspx
[IEnumerable]: https://msdn.microsoft.com/library/system.collections.ienumerable.aspx
[so]:          http://stackoverflow.com/q/337704/1316620
[SQL injection]:   https://en.wikipedia.org/wiki/SQL_injection

## Automatically parsing the DataTable

You can pass a type T to any of the library methods instead of parsing the DataTable on your own:

```cs
IEnumerable<User> users = qckQuery.Select<User>("SELECT * FROM Users");
```

The types and properties names should match between the DataTable and the type T.
