# QuickQuery

[![build](https://ci.appveyor.com/api/projects/status/github/tallesl/QuickQuery)](https://ci.appveyor.com/project/TallesL/QuickQuery)
[![nuget package](https://badge.fury.io/nu/QuickQuery.png)](http://badge.fury.io/nu/QuickQuery)

## Instantiating

```cs
var qckQuery = new QuickQuery("YourConnectionStringName");
```

The constructor uses [ConfigurationManager.ConnectionStrings](http://msdn.microsoft.com/library/system.configuration.configurationmanager.connectionstrings.aspx) underneath. May throw [NoSuchConnectionStringException](/QuickQuery/Exception/NoSuchConnectionStringException.cs) or [EmptyConnectionStringException](/QuickQuery/Exception/EmptyConnectionStringException.cs).

## Querying without return

```cs
qckQuery.WithoutReturn("DELETE FROM Users WHERE Name LIKE @NameToDelete", "NameToDelete", "John");
```

You can also make sure how many rows will be affected with:

* `WithoutReturnAffectingExactlyOneRow(string sql, params string[] parameters)`;
* `WithoutReturnAffectingOneRowOrLess(string sql, params string[] parameters)`;
* `WithoutReturnAffectingExactlyNRows(string sql, int n, params string[] parameters)`;
* `WithoutReturnAffectingNRowsOrLess(string sql, int n, params string[] parameters)`.

`UnexpectedNumberOfRowsAffected` is throw and the transaction is rolled back if the amount of affected rows is different from the expected.

`WithoutReturn` uses [SqlCommand.ExecuteNonQuery](http://msdn.microsoft.com/library/system.data.sqlclient.sqlcommand.executenonquery.aspx) underneath.

## Querying with return

```cs
int userCount = qckQuery.SingleValue<int>("SELECT COUNT(0) FROM Users");
DataTable user1337 = qckQuery.WithReturn("SELECT * FROM Users WHERE Id = @UserId", "UserId", "1337");
```

You can also make sure how many rows will be selected with:

* `WithReturnSelectingExactlyOneRow(string sql, params string[] parameters)`;
* `WithReturnSelectingOneRowOrLess(string sql, params string[] parameters)`;
* `WithReturnSelectingExactlyNRows(string sql, int n, params string[] parameters)`;
* `WithReturnSelectingNRowsOrLess(string sql, int n, params string[] parameters)`.

`UnexpectedNumberOfRowsSelected` is throw if the amount of selected rows is different from the expected.

`SingleValue` uses [SqlCommand.ExecuteScalar](http://msdn.microsoft.com/library/system.data.sqlclient.sqlcommand.executescalar.aspx) and `WithReturn` uses [DbDataAdapter.Fill](http://msdn.microsoft.com/library/system.data.common.dbdataadapter.fill.aspx) underneath.
