using System.Collections;

namespace MauiBase.Helpers.Navigation;

public interface INavigationParameters : IParameters, IEnumerable<KeyValuePair<string, object>>, IEnumerable
{
}