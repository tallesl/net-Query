namespace QckQuery.Exceptions.Formatting
{
    using System;

    /// <summary>
    /// Exception throw when an parameter name that isn't a string is found.
    /// </summary>
    [Serializable]
    public class ParameterNameNotString : QuickQueryException
    {
        internal ParameterNameNotString(object parameter)
            : base(string.Format("An parameter name that isn't a string was found: \"{0}\".", parameter)) { }
    }
}
