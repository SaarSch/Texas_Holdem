using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Data
{
    public class TupleModel<T, R>
    {
        public T m_Item1 { get; set; } 
        public R m_Item2 { get; set; }
    }
}
