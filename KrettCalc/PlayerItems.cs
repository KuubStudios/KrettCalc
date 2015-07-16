using System.Collections;
using System.Collections.Generic;

namespace KrettCalc {
    public class PlayerItems : IEnumerable<ItemStat> {
        public ItemStat First { get; set; }
        public ItemStat Second { get; set; }
        public ItemStat Third { get; set; }
        public ItemStat Fourth { get; set; }
        public ItemStat Fifth { get; set; }
        public ItemStat Sixth { get; set; }

        public IEnumerator<ItemStat> GetEnumerator() {
            yield return First;
            yield return Second;
            yield return Third;
            yield return Fourth;
            yield return Fifth;
            yield return Sixth;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}