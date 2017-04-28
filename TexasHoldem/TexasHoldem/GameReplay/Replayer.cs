using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TexasHoldem.GameReplay
{
    public class Replayer
    {
        private static readonly string counterPath = Directory.GetCurrentDirectory() + "\\gameReplayCounter.txt";

        private Replayer() { }

        public static string CreateReplay()
        {
            int counter = 1;
            if (File.Exists(counterPath))
            {
                counter = int.Parse(File.ReadAllText(counterPath));
            }

            var filename = "gameReplay#" + counter + ".csv";
            File.AppendAllText(filename, "turn no., seat0, seat1, seat2, seat3, seat4, seat5, seat6, seat7, seat8, pot, community,,," + Environment.NewLine);
            File.WriteAllText(counterPath, "" + ++counter);

            return filename;
        }

        public static void Save(string filename, int round, List<Player> players, int pot, Card[] community, string comment)
        {
            string path = Directory.GetCurrentDirectory() + "\\" + filename;
            if (!File.Exists(path))
            {
                throw new Exception("gameReplay file does not exists");
            }
            if (round < 1)
            {
                throw new Exception("round no. is invalid" +
                    "");
            }
            if (players == null)
            {
                throw new Exception("player list is invalid");
            }
            if (players.Count == 0)
            {
                throw new Exception("player list is empty");
            }
            if (pot < 0)
            {
                throw new Exception("pot value is invalid");
            }

            string entry = round + ",";
            foreach (Player p in players)
            {
                if (!p.Folded)
                {
                    entry += p.CurrentBet + ",";
                }
                else
                {
                    entry += "fold,";
                }
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
                    if (community[i] != null)
                    {
                        entry += community[i].value + community[i].type.ToString().Substring(0, 1) + ";";
                    }
                }
            }
            else
            {
                entry += "-";
            }
            entry += ",,," + comment;

            File.AppendAllText(path, entry + Environment.NewLine);
        }
    }
}
