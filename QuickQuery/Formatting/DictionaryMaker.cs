namespace QckQuery.Formatting
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;

    internal static class DictionaryMaker
    {
        internal static IDictionary<string, object> Make(object parameters)
        {
            return parameters is ExpandoObject ?
                (IDictionary<string, object>)parameters :
                parameters.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(parameters, null));
        }
    }
}
