using System.Collections;

namespace MauiAppDemo.Helpers.Navigation;

//
// Summary:
//     Defines a contract for specifying values associated with a unique key.
public interface IParameters : IEnumerable<KeyValuePair<string, object>>, IEnumerable
{
    //
    // Summary:
    //     Gets the number of parameters contained in the Prism.Common.IParameters.
    int Count { get; }

    //
    // Summary:
    //     Gets a collection containing the keys in the Prism.Common.IParameters.
    IEnumerable<string> Keys { get; }

    //
    // Summary:
    //     Gets the parameter associated with the specified key (legacy).
    //
    // Parameters:
    //   key:
    //     The key of the parameter to get.
    //
    // Returns:
    //     A matching value if it exists.
    object this[string key] { get; }

    //
    // Summary:
    //     Adds the specified key and value to the parameter collection.
    //
    // Parameters:
    //   key:
    //     The key of the parameter to add.
    //
    //   value:
    //     The value of the parameter to add.
    void Add(string key, object value);

    //
    // Summary:
    //     Determines whether the Prism.Common.IParameters contains the specified key.
    //
    // Parameters:
    //   key:
    //     The key to search the parameters for existence.
    //
    // Returns:
    //     true if the Prism.Common.IParameters contains a parameter with the specified
    //     key; otherwise, false.
    bool ContainsKey(string key);

    //
    // Summary:
    //     Gets the parameter associated with the specified key.
    //
    // Parameters:
    //   key:
    //     The key of the parameter to find.
    //
    // Type parameters:
    //   T:
    //     The type of the parameter to get.
    //
    // Returns:
    //     A matching value of T if it exists.
    T GetValue<T>(string key);

    //
    // Summary:
    //     Gets the parameter associated with the specified key.
    //
    // Parameters:
    //   key:
    //     The key of the parameter to find.
    //
    // Type parameters:
    //   T:
    //     The type of the parameter to get.
    //
    // Returns:
    //     An System.Collections.Generic.IEnumerable`1 of all the values referenced by key.
    IEnumerable<T> GetValues<T>(string key);

    //
    // Summary:
    //     Gets the parameter associated with the specified key.
    //
    // Parameters:
    //   key:
    //     The key of the parameter to get.
    //
    //   value:
    //     When this method returns, contains the parameter associated with the specified
    //     key, if the key is found; otherwise, the default value for the type of the value
    //     parameter.
    //
    // Type parameters:
    //   T:
    //     The type of the parameter to get.
    //
    // Returns:
    //     true if the Prism.Common.IParameters contains a parameter with the specified
    //     key; otherwise, false.
    bool TryGetValue<T>(string key, out T value);
}