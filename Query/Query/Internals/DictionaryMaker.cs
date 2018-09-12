namespace QueryLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;

    public static class DictionaryMaker
    {
        public static IDictionary<string, Tuple<Type, object>> MakeWithType(object input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input is IDictionary<string, object>)
                return ((IDictionary<string, object>)input).ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value == null ?
                        new Tuple<Type, object>(typeof(object), kvp.Value) :
                        new Tuple<Type, object>(kvp.Value.GetType(), kvp.Value)
                );

            var dict = new Dictionary<string, Tuple<Type, object>>();

            foreach (var property in input.GetType().GetProperties())
                dict.Add(property.Name, new Tuple<Type, object>(property.PropertyType, property.GetValue(input, null)));

            foreach (var field in input.GetType().GetFields())
                dict.Add(field.Name, new Tuple<Type, object>(field.FieldType, field.GetValue(input)));

            return dict;
        }
    }
}