namespace QueryLibrary
{
    using System.Data;

    public sealed partial class Query
    {
        private T WithReturn<T>(string sql, object parameters)
        {
            using (var command = _parameterSetter.GetCommand(OpenConnection, sql, parameters))
            {
                try
                {
                    // No need to open a transaction, only to use an open one if exists
                    // That's why _currentTransaction and not OpenTransaction (unlike the on writes)
                    command.Transaction = _currentTransaction;

                    var t = (T)command.ExecuteScalar();
                    CloseIfNeeded();
                    return t;
                }
                catch
                {
                    CloseRegardless(true);
                    throw;
                }
            }
        }

        private DataTable WithReturn(string sql, object parameters, int? n = null, bool acceptsLess = false)
        {
            using (var command = _parameterSetter.GetCommand(OpenConnection, sql, parameters))
            {
                try
                {
                    // No need to open a transaction, only to use an open one if exists
                    // That's why _currentTransaction and not OpenTransaction (unlike the on writes)
                    command.Transaction = _currentTransaction;

                    var dt = _dataTableFiller.Fill(command);
                    var selected = dt.Rows.Count;

                    if (n.HasValue && (selected > n || (!acceptsLess && selected != n)))
                        throw new UnexpectedNumberOfRowsSelectedException(command, selected);

                    CloseIfNeeded();
                    return dt;
                }
                catch
                {
                    CloseRegardless(true);
                    throw;
                }
            }
        }
    }
}