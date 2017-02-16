using System.Collections.Generic;
using System.DirectoryServices;

namespace Arc.ActiveDirectory.Extensions
{
    public static class ResultPropertyValueCollectionExtensions
    {
        public static IEnumerable<TCast> Cast<TCast>(this ResultPropertyValueCollection collection)
        {
            for (int i = 0; i < collection.Count; i++)
                yield return (TCast)collection[i];
        }

        public static IEnumerable<TCast> Cast<TCast>(this PropertyValueCollection collection)
        {
            for (int i = 0; i < collection.Count; i++)
                yield return (TCast)collection[i];
        }
    }
}
