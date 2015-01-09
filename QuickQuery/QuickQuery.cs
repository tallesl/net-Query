using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;
using QckQuery.Exception;

namespace QckQuery
{
    /// <summary>
    /// Performs a *quick query* to a SQL server database.
    /// </summary>
    public class QuickQuery
    {
        /// <summary>
        /// The database connection string.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="connectionStringName">
        /// The name of the connection string to be read from the .config file
        /// </param>
        public QuickQuery(string connectionStringName)
        {
            // Reading the connection string from .config file
            var config = ConfigurationManager.ConnectionStrings[connectionStringName];

            // If there's no connection string with the given name
            if (config == null)
            {
                throw new NoSuchConnectionStringException(connectionStringName);
            }
            else
            {
                var connectionString = config.ToString();

                // If the connection string value is an empty string
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new EmptyConnectionStringException(connectionStringName);
                }
                else
                {
                    // The connection string being OK we set the field and let the class be constructed
                    _connectionString = connectionString;
                }
            }
        }

        /// <summary>
        /// Runs the given query giving no return.
        /// It uses DbCommand.ExecuteNonQuery underneath. 
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        public void WithoutReturn(string sql, params string[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = SetUpCommand(connection, sql, parameters))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Runs the given query giving no return.
        /// Throws MoreThanOneRowAffectedException if the quantity of affected rows is different from the provided.
        /// It uses DbCommand.ExecuteNonQuery underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="affectedRows">Quantity of rows that the query should affect</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <exception cref="MoreThanOneRowAffectedException">
        /// If the quantity of affected rows is different form the provided
        /// </exception>
        public void WithoutReturnEnsuringAffected(string sql, int affectedRows, params string[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var transaction = new TransactionScope())
            using (var command = SetUpCommand(connection, sql, parameters))
            {
                connection.Open();
                if (affectedRows == command.ExecuteNonQuery())
                {
                    transaction.Complete();
                }
                else
                {
                    throw new MoreThanOneRowAffectedException(GetSql(command));
                }
            }
        }

        /// <summary>
        /// Runs the given query and returns the obtained value.
        /// Only a single value (first row of the first column) is returned.
        /// It uses DbCommand.ExecuteScalar underneath.
        /// </summary>
        /// <typeparam name="T">Type of the value to be returned</typeparam>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>The first column of the first row queried</returns>
        public T SingleValue<T>(string sql, params string[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = SetUpCommand(connection, sql, parameters))
            {
                connection.Open();
                return (T)command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Runs the given query and returns the a DataTable with the queried values.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        public DataTable WithReturn(string sql, params string[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = SetUpCommand(connection, sql, parameters))
            {
                using (var adapter = DbProviderFactories.GetFactory("System.Data.SqlClient").CreateDataAdapter())
                {
                    connection.Open();
                    var datatable = new DataTable();
                    adapter.SelectCommand = command;
                    adapter.Fill(datatable);
                    return datatable;
                }
            }
        }

        /// <summary>
        /// Formats underlying SQL of the given DbCommand.
        /// </summary>
        /// <param name="command">DBCommand to get the SQL from</param>
        /// <returns>The SQL query</returns>
        private static string GetSql(IDbCommand command)
        {
            var sql = new StringBuilder(command.CommandText);
            foreach (DbParameter parameter in command.Parameters)
            {
                sql.Replace(parameter.ParameterName, parameter.Value.ToString());
            }
            return sql.ToString();
        }

        /// <summary>
        /// Creates a new DBCommand from the given connection with the given SQL and parameters.
        /// </summary>
        /// <param name="connection">Connection to create the command from</param>
        /// <param name="sql">Command's query</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>The set up command</returns>
        private static DbCommand SetUpCommand(SqlConnection connection, string sql, string[] parameters)
        {
            var command = connection.CreateCommand();

            // Setting the command query
            command.CommandText = sql;

            // Reading the parameters pairs and setting in the command (if any)
            if (parameters != null)
            {
                for (var i = 0; i < parameters.Length; )
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = parameters[i++];
                    parameter.Value = (object)parameters[i++] ?? DBNull.Value;
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }
    }
}
