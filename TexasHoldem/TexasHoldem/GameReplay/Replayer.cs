using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TexasHoldem.GameReplay
{
    public class Replayer
    {
        private static int _counter = 1;

        private Replayer() { }

        public static string CreateReplay()
        {
            //TODO: add error checking

            var filename = "gameReplay#" + _counter + ".csv";
            File.AppendAllText(filename, "round no, seat0, seat1, seat2, seat3, seat4, seat5, seat6, seat7, seat8, pot, community,,," + Environment.NewLine);
            _counter++;

            return filename;
        }

        public static void Save(string filename, int round, List<Player> players, int pot, Card[] community, string comment)
        {
            //TODO: add error checking
            string entry = round + ",";
            foreach (Player p in players)
            {
                entry += p.CurrentBet + ",";
            }
            for (int i = 0; i <= 8-players.Count; i++)
            {
                entry += "undef,";
            }
            entry += pot + ",";
            if (community != null)
            {
                for (int i = 0; i < community.Length; i++)
                {
                    entry += community[i].value + community[i].type.ToString().Substring(0,1) + ";";
                }
            }
            else
            {
                entry += "-";
            }
            entry += ",,," + comment;
            File.AppendAllText(Directory.GetCurrentDirectory() + "\\" + filename, entry + Environment.NewLine);
        }
    }
}
