namespace QueryLibrary
{

    public sealed partial class Query
    {
        private void WithoutReturn(string sql, object parameters, CountValidationEnum countValidation, int n)
        {
            using (var command = _parameterSetter.GetCommand(OpenConnection, sql, parameters))
            {
                try
                {
                    command.Transaction = OpenTransaction;

                    var affected = command.ExecuteNonQuery();

                    switch (countValidation)
                    {
                        case CountValidationEnum.Exactly:
                            if (affected != n)
                                throw new UnexpectedNumberOfRowsAffectedException(command, affected);
                            break;

                        case CountValidationEnum.NoLessThan:
                            if (affected < n)
                                throw new UnexpectedNumberOfRowsAffectedException(command, affected);
                            break;

                        case CountValidationEnum.NoMoreThan:
                            if (affected > n)
                                throw new UnexpectedNumberOfRowsAffectedException(command, affected);
                            break;
                    }

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