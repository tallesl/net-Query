namespace QckQuery.Exception.Querying
{
    /// <summary>
    /// Exception throw when an parameter name that isn't a string is found.
    /// </summary>
    public class ParameterNameNotString : QuickQueryException
    {
        internal ParameterNameNotString(object parameter)
            : base("An parameter name that isn't a string was found: \"" + parameter + "\".") { }
    }
}
