<p align="center">
    <a href="#query">
        <img alt="logo" src="Assets/logo-200x200.png">
    </a>
</p>

# Query

[![][build-img]][build]
[![][nuget-img]][nuget]

A simplistic ADO.NET wrapper.

* [Instantiating](#instantiating)
* [Modifying data](#modifying-data)
* [Retrieving data](#retrieving-data)
* [Behind the covers](#behind-the-covers)
* [Options](#options)
* [IN clauses](#in-clauses)
* [Connections and Transactions](#connections-and-transactions)
* [Thread safety](#thread-safety)
* [Building `SELECT` clauses](#building-select-clauses)

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

DataTable dataTable = query.Select("SELECT * FROM Users");

IEnumerable<User> users = query.Select<User>("SELECT * FROM Users");
User user = query.SelectExactlyOne<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = 1337 });
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
var query = new Query("YourConnectionStringName", new QueryOptions { ManualClosing = true });

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
using (var query = new Query("YourConnectionStringName", new QueryOptions { ManualClosing = true }))
{
    query.Change("INSERT INTO Foo VALUES ('Bar')");
    query.Select("some syntax error");
}
```

[Dispose]: https://msdn.microsoft.com/library/System.IDisposable.Dispose

## Thread safety

Query isn't thread safe, but it should be lightweight enough to be instantiated as needed during the lifetime of your
application (such as one per request).

## Building `SELECT` clauses

Running handmade SQL queries instead of battling an ORM to find out what it's generating is one of the reasons to use a
so called *micro ORM*.

It's all fun and games until you have to build a more complex query, one that can take multiple forms accordingly to
some set of parameters.
Those are usually `SELECT` queries in which the `WHERE` clause can take different combinations of parameters (or even be
abscent).

To aid in such scenarios, there is a built-in `Select` type.
You can instantiate one yourself with `new Select("ColumnA", "ColumnB", "ColumnC", ...)` and then, on the constructed
object, you will be able to call:

* `From`
* `Join`
* `LeftOuterJoin`
* `RightOuterJoin`
* `FullOuterJoin`
* `CrossJoin`
* `Where`
* `WhereAnd`
* `WhereOr`
* `GroupBy`
* `Having`
* `OrderBy`

A `ToString` call gives you the resulting SQL (but you don't have to call it when passing as parameter, it implicitly
casts to string).

To illustrate the `Select` class being helpful, such page:

![](Assets/screen.png)

Could be powered by the following code:

```cs
// A class representing a role.
public class Role
{
    public int Id { get; set; }

    public string Name { get; set; }
}

// A class representing an user.
// The role property here require a JOIN.
public class User
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public Role Role { get; set; }
}

// It's often useful to expose the Query object through a property,
// specially when reusing the object (manual closing) or when QueryOptions is used.
Query Query
{
    get
    {
        return new Query("MyConnectionString");
    }
}

// It's also useful to create a property with a 'vanilla' SELECT of an entity of yours (DRY).
Select UserSelect
{
    get
    {
        return new Select(
            // The names of the selected columns and the class properties must match if you want
            // the library to give you the instantiated object (and not a DataTable).
            "User.Id",
            "User.Name",
            "User.Email",

            // The library is also able to construct complex properties
            // (non primitive types) as long the names and the types matches;
            // it can work out the whole property tree, in other words, it can go more than one level deeper
            // ("PropertyA.PropertyAB.PropertyABC.PropertyABCD...").
            "Role.Id AS 'Role.Id'",
            "Role.Name AS 'Role.Name'")
            .From("User")
            .Join("Role", "RoleId = Role.Id");
    }
}

// A search method in which all the parameters are optional.
IEnumerable<User> Search(string name, string email, int? role)
{
    // Getting a Select for the user table that we can work with.
    // You can call ToString at any time to check its SQL.
    var select = UserSelect;

    // Using a ExpandoObject as parameters helps a lot when dealing with optional conditions.
    dynamic parameters = new ExpandoObject();

    // If there's a name a to filter.
    if (!string.IsNullOrWhiteSpace(name))
    {
        // We add a WHERE clause to our Select object.
        select.WhereAnd("User.Name LIKE %@Name%");

        // We also add the used parameter to our ExpandoObject object.
        parameters.Name = name;
    }

    // Same here
    if (!string.IsNullOrWhiteSpace(email))
    {
        select.WhereAnd("User.Email LIKE %@Email%");
        parameters.Email = email;
    }

    // Same here too
    if (role.HasValue)
    {
        select.WhereAnd("User.RoleId = @RoleId");
        parameters.Role = role;
    }

    // At this point the Select object is complete, with or without the necessary WHERE clause.
    // Note that if we were building the SQL on our own, by concatenating strings,
    // making this query would be much more cumbersome.

    // Finally, we query for the users by handling the objects we built to Query,
    // the Select is the query and the ExpandoObject are its parameters.
    return Query.Select<User>(select, parameters);
}
```
