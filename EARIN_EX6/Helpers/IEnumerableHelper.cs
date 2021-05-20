using System;
using System.Collections.Generic;
using System.Linq;

namespace EARIN_EX6.Helpers {
    static class IEnumerableHelper {

        private readonly static Random random = new Random();

        public static T PickRandom<T>(this IEnumerable<T> enumerable) {
            return enumerable.ElementAt(random.Next(0, enumerable.Count()));
        }

        public static T RoulettePick<T>(this IEnumerable<T> sequence, Func<T, double> weightSelector) {
            var totalWeight = sequence.Sum(weightSelector);
            var itemWeightIndex = random.NextDouble() * totalWeight;
            var currentWeightIndex = 0.0;

            foreach (var item in from weightedItem in sequence select new { Value = weightedItem, Weight = weightSelector(weightedItem) }) {
                currentWeightIndex += item.Weight;

                if (currentWeightIndex >= itemWeightIndex)
                    return item.Value;
            }

            return default;
        }
    }
}
