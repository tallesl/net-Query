namespace QueryLibrary
{
    using DbParameterSetting;
    using QueryLibrary.Exceptions;

    public partial class Query
    {
        private void WithoutReturn(string sql, object parameters, int? n = null, bool acceptsLess = false)
        {
            using (var command = OpenConnection.GetCommand(sql, parameters, _options.EnumAsString))
            {
                try
                {
                    command.Transaction = OpenTransaction;

                    var affected = command.ExecuteNonQuery();

                    if (n.HasValue && (affected > n || (!acceptsLess && affected != n)))
                        throw new UnexpectedNumberOfRowsAffected(command, affected);

                    CloseIfNeeded();
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
