namespace QckQuery.Formatting
{
    using QckQuery.Exception.Formatting;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;

    internal static class DictionaryMaker
    {
        internal static IDictionary<string, object> ToParameterDictionary(this object[] parameters)
        {
            var dictionary = new Dictionary<string, object>();
            if (parameters != null)
            {
                if (parameters.Length % 2 != 0) throw new OddParametersException(parameters.Length);
                for (var i = 0; i < parameters.Length; i += 2)
                {
                    var key = parameters[i++] as string;
                    var value = parameters[i++];

                    if (key == null) throw new ParameterNameNotString(parameters[i]);
                    else dictionary.Add(key, value);
                }
            }
            return dictionary;
        }

        internal static IDictionary<string, object> ToParameterDictionary(this object parameters)
        {
            return parameters is ExpandoObject ?
                (IDictionary<string, object>)parameters :
                parameters.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(parameters, null));
        }
    }
}
