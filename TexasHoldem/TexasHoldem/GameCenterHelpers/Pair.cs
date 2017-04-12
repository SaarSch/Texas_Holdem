using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.GameCenterHelpers
{
    public class Pair<T, U>
    {
        public Pair()
        {
        }

        public Pair(T First, U Second)
        {
            this.First = First;
            this.Second = Second;
        }

        public T First { get; set; }
        public U Second { get; set; }
    }
}
