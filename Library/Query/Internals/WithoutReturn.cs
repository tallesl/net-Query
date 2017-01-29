namespace QueryLibrary
{

    public sealed partial class Query
    {
        private void WithoutReturn(string sql, object parameters, int? n = null, bool acceptsLess = false)
        {
            using (var command = _parameterSetter.GetCommand(OpenConnection, sql, parameters))
            {
                try
                {
                    command.Transaction = OpenTransaction;

                    var affected = command.ExecuteNonQuery();

                    if (n.HasValue && (affected > n || (!acceptsLess && affected != n)))
                        throw new UnexpectedNumberOfRowsAffectedException(command, affected);

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
