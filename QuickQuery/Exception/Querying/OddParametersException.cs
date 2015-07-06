namespace QckQuery.Exception.Querying
{
    /// <summary>
    /// Exception throw when an odd number of command parameters is found.
    /// </summary>
    public class OddParametersException : QuickQueryException
    {
        internal OddParametersException(int n)
            : base(string.Format("And odd number of parameters ({0}) was passed.", n)) { }
    }
}
