namespace MauiBase.Helpers.Navigation;

//
// Summary:
//     Used to set internal parameters used by Prism's Prism.Navigation.INavigationService
public interface INavigationParametersInternal
{
    //
    // Summary:
    //     Adds the key and value to the parameters Collection
    //
    // Parameters:
    //   key:
    //     The key to reference this value in the parameters collection.
    //
    //   value:
    //     The value of the parameter to store
    void Add(string key, object value);

    //
    // Summary:
    //     Checks collection for presence of key
    //
    // Parameters:
    //   key:
    //     The key to check in the Collection
    //
    // Returns:
    //     true if key exists; else returns false.
    bool ContainsKey(string key);

    //
    // Summary:
    //     Returns the value of the member referenced by key
    //
    // Parameters:
    //   key:
    //     The key for the value to be returned
    //
    // Type parameters:
    //   T:
    //     The type of object to be returned
    //
    // Returns:
    //     Returns a matching parameter of T if one exists in the Collection
    T GetValue<T>(string key);
}