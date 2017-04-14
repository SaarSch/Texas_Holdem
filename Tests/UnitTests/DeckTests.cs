using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.UnitTests
{
    class DeckTests
    {
        [TestMethod]
        public void CreateDeck()
        {
            Deck d = new Deck();
            for (int i = 1; i < 14; i++) // init deck
            {
                for (int j = 1; j < 5; j++)
                {
                    Assert.IsTrue(d.Contains(i, j));
                }
            }
        }
    }
}
