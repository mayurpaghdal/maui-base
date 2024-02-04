namespace MauiBase.Helpers.Navigation;

public class NavigationParameters : ParametersBase, INavigationParameters, INavigationParametersInternal
{
    private readonly Dictionary<string, object> _internalParameters = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationParameters"/> class.
    /// </summary>
    public NavigationParameters()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationParameters"/> class with a query string.
    /// </summary>
    /// <param name="query">The query string.</param>
    public NavigationParameters(string query)
        : base(query)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationParameters"/> class with a query string.
    /// </summary>
    /// <param name="query">The query string.</param>
    public NavigationParameters(IDictionary<string, object> query)
        : base(query)
    {
    }

    #region INavigationParametersInternal
    void INavigationParametersInternal.Add(string key, object value)
    {
        _internalParameters.Add(key, value);
    }

    void INavigationParametersInternal.AddRange(IDictionary<string, object> parameters)
    {
        if (parameters is null)
            return;

        foreach (var p in parameters.AsEnumerable())
            _internalParameters.Add(p.Key, p.Value);
    }

    bool INavigationParametersInternal.ContainsKey(string key)
    {
        return _internalParameters.ContainsKey(key);
    }

    T INavigationParametersInternal.GetValue<T>(string key)
    {
        return _internalParameters.GetValue<T>(key);
    }
    #endregion
}
