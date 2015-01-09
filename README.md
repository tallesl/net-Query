# QuickQuery

[![nuget package](https://badge.fury.io/nu/QuickQuery.png)](http://badge.fury.io/nu/QuickQuery)

A simplistic ADO.NET wrapper that queries a SQL Server database. 

## Usage

Instantiating:

```cs
var qckQuery = new QuickQuery("Server=myServerAddress;Database=myDataBase;UserId=myUsername;Password=myPassword;");
```

Querying without return ([SqlCommand.ExecuteNonQuery](http://msdn.microsoft.com/library/system.data.sqlclient.sqlcommand.executenonquery.aspx)):

```cs
qckQuery.WithoutReturn("DELETE FROM Users WHERE Name LIKE @NameToDelete", "NameToDelete", "John");
```

Querying a single value ([SqlCommand.ExecuteScalar](http://msdn.microsoft.com/library/system.data.sqlclient.sqlcommand.executescalar.aspx)):

```cs
int userCount = qckQuery.SingleValue<int>("SELECT COUNT(0) FROM Users");
```

Querying with multiple value ([DbDataAdapter.Fill](http://msdn.microsoft.com/library/system.data.common.dbdataadapter.fill.aspx)):

```cs
DataTable user1337 = qckQuery.WithReturn("SELECT * FROM Users WHERE Id = @UserId", "UserId", "1337");
```

## .NET version

4.


