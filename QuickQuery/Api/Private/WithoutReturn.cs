namespace QckQuery
{
    using QckQuery.DataAccess;
    using QckQuery.Exceptions.Querying;
    using System.Collections.Generic;
    using System.Transactions;

    public partial class QuickQuery
    {
        private void WithoutReturn(string sql, IDictionary<string, object> parameters)
        {
            using (var connection = _connectionProvider.Provide())
            using (var command = connection.GetCommandWithParametersSet(sql, parameters))
            {
                command.ExecuteNonQuery();
            }
        }

        private void WithoutReturn(int n, string sql, bool acceptsLess, IDictionary<string, object> parameters)
        {
            using (var connection = _connectionProvider.Provide())
            using (var transaction = new TransactionScope())
            using (var command = connection.GetCommandWithParametersSet(sql, parameters))
            {
                var affected = command.ExecuteNonQuery();
                if (affected == n || (acceptsLess && affected < n)) transaction.Complete();
                else throw new UnexpectedNumberOfRowsAffected(command, affected);
            }
        }
    }
}
