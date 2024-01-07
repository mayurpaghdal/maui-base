using System.Collections;

namespace MauiAppDemo.Helpers.Navigation;

public interface INavigationParameters : IParameters, IEnumerable<KeyValuePair<string, object>>, IEnumerable
{
}