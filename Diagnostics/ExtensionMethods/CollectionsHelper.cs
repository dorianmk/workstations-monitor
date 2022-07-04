using System;
using System.Collections.Generic;
using System.Linq;

namespace Diagnostics.ExtensionMethods
{
    internal static class CollectionsHelper
    {
        internal static void Update<TKey, TValue, TItem>(
            this Dictionary<TKey, TValue> target, 
            List<TItem> actualItems, 
            Func<TItem, TKey> key,
            Func<TItem, TValue> newValue,
            Action<TValue, TItem> onUpdate)
        {
            foreach (var item in actualItems)
            {
                if (target.TryGetValue(key(item), out var existingValue))
                    onUpdate(existingValue, item);
                else
                    target.Add(key(item), newValue(item));
            }
            var toRemove = target.Keys.Except(actualItems.Select(x => key(x))).ToArray();
            foreach (var notExistingKey in toRemove)
                target.Remove(notExistingKey);
        }
    }
}
