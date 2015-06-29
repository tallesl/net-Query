namespace QckQuery.Exception.Querying
{
    /// <summary>
    /// Exception throw when an odd number of command parameters is found.
    /// </summary>
    public class OddParametersException : QuickQueryException
    {
        internal OddParametersException(int n) : base("And odd number of parameters (" + n + " ) was passed.") { }
    }
}
