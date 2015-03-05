# QuickQuery

[![build](https://ci.appveyor.com/api/projects/status/github/tallesl/QuickQuery)](https://ci.appveyor.com/project/TallesL/QuickQuery)
[![nuget package](https://badge.fury.io/nu/QuickQuery.png)](http://badge.fury.io/nu/QuickQuery)

## Instantiating

```cs
var qckQuery = new QuickQuery("YourConnectionStringName");
```

It uses [ConfigurationManager.ConnectionStrings](http://msdn.microsoft.com/library/system.configuration.configurationmanager.connectionstrings.aspx) underlying. May throw [NoSuchConnectionStringException](/QuickQuery/Exception/NoSuchConnectionStringException.cs) or [EmptyConnectionStringException](/QuickQuery/Exception/EmptyConnectionStringException.cs).

## Querying without return

```cs
qckQuery.WithoutReturn("DELETE FROM Users WHERE Name LIKE @NameToDelete", "NameToDelete", "John");
```

It uses [SqlCommand.ExecuteNonQuery](http://msdn.microsoft.com/library/system.data.sqlclient.sqlcommand.executenonquery.aspx) underlying.

## Querying without return and ensuring affected rows

```cs
qckQuery.WithoutReturnEnsuringAffected("DELETE FROM Users WHERE Name = @NameToDelete", 1, "NameToDelete", "Joe Blows");
```

It uses [SqlCommand.ExecuteNonQuery](http://msdn.microsoft.com/library/system.data.sqlclient.sqlcommand.executenonquery.aspx) underlying. May throw [MoreThanOneRowAffectedException](/QuickQuery/Exception/MoreThanOneRowAffectedException.cs).

## Querying a single value

```cs
int userCount = qckQuery.SingleValue<int>("SELECT COUNT(0) FROM Users");
```

It uses [SqlCommand.ExecuteScalar](http://msdn.microsoft.com/library/system.data.sqlclient.sqlcommand.executescalar.aspx) underlying.

## Querying with multiple value

```cs
DataTable user1337 = qckQuery.WithReturn("SELECT * FROM Users WHERE Id = @UserId", "UserId", "1337");
```

It uses [DbDataAdapter.Fill](http://msdn.microsoft.com/library/system.data.common.dbdataadapter.fill.aspx) underlying.
