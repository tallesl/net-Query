# QuickQuery

[![][build-img]][build]
[![][nuget-img]][nuget]

[build]:     https://ci.appveyor.com/project/TallesL/QuickQuery
[build-img]: https://ci.appveyor.com/api/projects/status/github/tallesl/QuickQuery

[nuget]:     http://badge.fury.io/nu/QuickQuery
[nuget-img]: https://badge.fury.io/nu/QuickQuery.png

A simplistic ADO.NET wrapper.

## Instantiating

```cs
var qckQuery = new QuickQuery("YourConnectionStringName");
```

The constructor uses [ConfigurationManager.ConnectionStrings] underneath. May throw [NoSuchConnectionStringException], [EmptyConnectionStringException] or even [EmptyProviderNameException].

[ConfigurationManager.ConnectionStrings]: http://msdn.microsoft.com/library/system.configuration.configurationmanager.connectionstrings.aspx

[NoSuchConnectionStringException]: QuickQuery/Exception/NoSuchConnectionStringException.cs
[EmptyConnectionStringException]:  QuickQuery/Exception/EmptyConnectionStringException.cs
[EmptyProviderNameException]:      QuickQuery/Exception/EmptyProviderNameException.cs

## Querying without return

```cs
qckQuery.NoReturn("DELETE FROM Users WHERE Name LIKE @NameToDelete", "NameToDelete", "John");
```

You can also make sure how many rows will be affected with:

* `ChangeExactly(int n, string sql, params string[] parameters)`;
* `ChangeNoMoreThan(int n, string sql, params string[] parameters)`.

`UnexpectedNumberOfRowsAffected` is throw and the transaction is rolled back if the amount of affected rows is different from the expected.

It uses [SqlCommand.ExecuteNonQuery][SqlCommand.ExecuteNonQuery] underneath.

[SqlCommand.ExecuteNonQuery]: http://msdn.microsoft.com/library/system.data.sqlclient.sqlcommand.executenonquery.aspx

## Querying with return

```cs
int userCount = qckQuery.SelectSingle<int>("SELECT COUNT(0) FROM Users");
DataTable user1337 = qckQuery.Select("SELECT * FROM Users WHERE Id = @UserId", "UserId", 1337);
```

You can also make sure how many rows will be selected with:

* `SelectExactly(int n, string sql, params string[] parameters)`;
* `SelectNoMoreThan(int n, string sql, params string[] parameters)`.

`UnexpectedNumberOfRowsSelected` is throw if the amount of selected rows is different from the expected.

`SelectSingle` uses [SqlCommand.ExecuteScalar] while the others uses [DbDataAdapter.Fill] underneath.

[SqlCommand.ExecuteScalar]: http://msdn.microsoft.com/library/system.data.sqlclient.sqlcommand.executescalar.aspx
[DbDataAdapter.Fill]:       http://msdn.microsoft.com/library/system.data.common.dbdataadapter.fill.aspx

## `IN` clauses

It automatically prepares collections ([IEnumerable]) for [`IN`][IN] clauses ([taking that burden off you][so]).

So this:

```cs
var ids = new List<int> { 1, 123, 44 };
qckQuery.NoReturn("DELETE FROM Users WHERE Id = (@Ids)", "Ids", ids);
```

Becomes this:

```sql
DELETE FROM Users WHERE Id = (@Ids0, @Ids1, @Ids2)
```

Note that to do this the library concatenates SQL on its own.
**This gives opening for [SQL injection][injection], never use this with user input.**

[IN]:          https://msdn.microsoft.com/library/ms177682.aspx
[IEnumerable]: https://msdn.microsoft.com/library/system.collections.ienumerable.aspx
[so]:          http://stackoverflow.com/q/337704/1316620
[injection]:   https://en.wikipedia.org/wiki/SQL_injection

## Automatically parsing the DataTable

You can pass a type T to any of the library methods instead of parsing the DataTable on your own:

```cs
IEnumerable<User> users = qckQuery.Select<User>("SELECT * FROM Users");
```

Note that the types and properties names should match between the DataTable and the type T.
