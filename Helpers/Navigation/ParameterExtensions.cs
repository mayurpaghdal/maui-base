
namespace MauiAppDemo.Helpers.Navigation
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ParametersExtensions
    {
        //
        // Summary:
        //     Searches parameters for key
        //
        // Parameters:
        //   parameters:
        //     A collection of parameters to search
        //
        //   key:
        //     The key of the parameter to find
        //
        // Type parameters:
        //   T:
        //     The type of the parameter to return
        //
        // Returns:
        //     A matching value of T if it exists
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static T GetValue<T>(this IEnumerable<KeyValuePair<string, object>> parameters, string key)
        {
            return (T)parameters.GetValue(key, typeof(T));
        }

        //
        // Summary:
        //     Searches parameters for value referenced by key
        //
        // Parameters:
        //   parameters:
        //     A collection of parameters to search
        //
        //   key:
        //     The key of the parameter to find
        //
        //   type:
        //     The type of the parameter to return
        //
        // Returns:
        //     A matching value of type if it exists
        //
        // Exceptions:
        //   T:System.InvalidCastException:
        //     Unable to convert the value of Type
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static object GetValue(this IEnumerable<KeyValuePair<string, object>> parameters, string key, Type type)
        {
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                if (string.Compare(parameter.Key, key, StringComparison.Ordinal) == 0)
                {
                    if (TryGetValueInternal(parameter, type, out var value))
                    {
                        return value;
                    }

                    throw new InvalidCastException("Unable to convert the value of Type '" + parameter.Value.GetType().FullName + "' to '" + type.FullName + "' for the key '" + key + "' ");
                }
            }

            return GetDefault(type);
        }

        //
        // Summary:
        //     Searches parameters for value referenced by key
        //
        // Parameters:
        //   parameters:
        //     A collection of parameters to search
        //
        //   key:
        //     The key of the parameter to find
        //
        //   value:
        //     The value of parameter to return
        //
        // Type parameters:
        //   T:
        //     The type of the parameter to return
        //
        // Returns:
        //     Success if value is found; otherwise returns false
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool TryGetValue<T>(this IEnumerable<KeyValuePair<string, object>> parameters, string key, out T value)
        {
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                if (string.Compare(parameter.Key, key, StringComparison.Ordinal) == 0)
                {
                    object value2;
                    bool result = TryGetValueInternal(parameter, typeof(T), out value2);
                    value = (T)value2;
                    return result;
                }
            }

            value = default(T)!;
            return false;
        }

        //
        // Summary:
        //     Searches parameters for value referenced by key
        //
        // Parameters:
        //   parameters:
        //     A collection of parameters to search
        //
        //   key:
        //     The key of the parameter to find
        //
        // Type parameters:
        //   T:
        //     The type of the parameter to return
        //
        // Returns:
        //     An IEnumerable{T} of all the values referenced by key
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<T> GetValues<T>(this IEnumerable<KeyValuePair<string, object>> parameters, string key)
        {
            List<T> list = new();
            Type typeFromHandle = typeof(T);
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                if (string.Compare(parameter.Key, key, StringComparison.Ordinal) == 0)
                {
                    TryGetValueInternal(parameter, typeFromHandle, out var value);
                    list.Add((T)value);
                }
            }

            return list.AsEnumerable();
        }

        private static bool TryGetValueInternal(KeyValuePair<string, object> kvp, Type type, out object value)
        {
            value = GetDefault(type);
            bool flag = false;
            if (kvp.Value == null)
            {
                flag = true;
            }
            else if (kvp.Value.GetType() == type)
            {
                flag = true;
                value = kvp.Value;
            }
            else if (type.IsAssignableFrom(kvp.Value.GetType()))
            {
                flag = true;
                value = kvp.Value;
            }
            else if (type.IsEnum)
            {
                string text = kvp.Value.ToString()!;
                int result;
                if (Enum.IsDefined(type, text))
                {
                    flag = true;
                    value = Enum.Parse(type, text);
                }
                else if (int.TryParse(text, out result))
                {
                    flag = true;
                    value = Enum.ToObject(type, result);
                }
            }

            if (!flag && type.GetInterface("System.IConvertible") != null)
            {
                flag = true;
                value = Convert.ChangeType(kvp.Value, type)!;
            }

            return flag;
        }

        //
        // Summary:
        //     Checks to see if key exists in parameter collection
        //
        // Parameters:
        //   parameters:
        //     IEnumerable to search
        //
        //   key:
        //     The key to search the parameters for existence
        //
        // Returns:
        //     true if key exists; false otherwise
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool ContainsKey(this IEnumerable<KeyValuePair<string, object>> parameters, string key)
        {
            return parameters.Any((KeyValuePair<string, object> x) => string.Compare(x.Key, key, StringComparison.Ordinal) == 0);
        }

        private static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type)!;
            }

            return default!;
        }
    }
}
