using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
                Logger.Log(Severity.Exception, "gameReplay file does not exists");
                throw new Exception("gameReplay file does not exists");
            }
            if (round < 1)
            {
                Logger.Log(Severity.Exception, "round no. is invalid");
                throw new Exception("round no. is invalid" +
                    "");
            }
            if (players == null)
            {
                Logger.Log(Severity.Exception, "player list is invalid");
                throw new Exception("player list is invalid");
            }
            if (players.Count == 0)
            {
                Logger.Log(Severity.Exception, "player list is empty");
                throw new Exception("player list is empty");
            }
            if (pot < 0)
            {
                Logger.Log(Severity.Exception, "pot value is invalid");
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

        public static string SaveTurn(string replay, int turnNum)
        {
            string path = Directory.GetCurrentDirectory() + "\\" + replay;
            string turn = "";
            if (File.Exists(path))
            {
                StreamReader file = new StreamReader(path);
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    if (Char.GetNumericValue(line[0]) == turnNum)
                    { 
                        turn += line + "\n";
                    }
                }

                file.Close();
            }
            else
            {
                Logger.Log(Severity.Exception, "game replay not found");
                throw new Exception("game replay not found");
            }
            if (turn == "")
            {
                Logger.Log(Severity.Exception, "turn not found in game replay");
                throw new Exception("turn not found in game replay");
            }
            return turn;
        }
    }
}
