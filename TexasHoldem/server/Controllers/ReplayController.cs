using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web.Http;
using Server.Models;

namespace Server.Controllers
{
    public class ReplayController : ApiController
    {
        // GET: api/Replay?RoomName=="KAKI"&&Player="moshe"
        public IEnumerable<RoomState> Get(string roomName, string player, string token)
        {
            Server.CheckToken(token);
            List<RoomState> ans = null;
            if (RoomController.Replays.ContainsKey(roomName) && RoomController.Replays[roomName].ContainsKey(player))
                ans = RoomController.Replays[roomName][player];
            return ans;
        }

        // GET: api/Replay?user=="sean"&&token="asdadasdasda"
        //get all replay Files by player
        public List<Tuple<string, string>> Get(string user, string token)
        {
            var ansLists = new List<Tuple<string, string>>();
            var AppDataPath = AppDomain.CurrentDomain.GetData("DataDirectory") != null
                ? AppDomain.CurrentDomain.GetData("DataDirectory").ToString()
                : AppDomain.CurrentDomain.BaseDirectory;
            foreach (var f in Directory.GetFiles(AppDataPath))
                if (f.EndsWith(".json") && f.Contains("User_Name" + Crypto.Decrypt(user)))
                {
                    var split = f.Split('#');
                    var date = split[1];
                    var roomName = split[2].Substring(0, split[2].Length - 5);
                    ansLists.Add(new Tuple<string, string>(date, roomName));
                }
            return ansLists;
        }

        // GET: api/Replay?user=="sean"&&roomName="sdfhj"&&date=21_06_2017 07_10_42"&&token="asdadasdasda"
        //get replay    
        public List<RoomState> Get(string user, string roomName, string date, string token)
        {
            var ans = new List<RoomState>();
            var AppDataPath = AppDomain.CurrentDomain.GetData("DataDirectory") != null
                ? AppDomain.CurrentDomain.GetData("DataDirectory").ToString()
                : AppDomain.CurrentDomain.BaseDirectory;
            foreach (var f in Directory.GetFiles(AppDataPath))
                if (f.EndsWith(".json") && f.Contains("User_Name" + Crypto.Decrypt(user)) && f.Contains(roomName) &&
                    f.Contains(date))
                {
                    var stream1 = new MemoryStream();
                    using (var fs = File.OpenRead(f))
                    {
                        fs.CopyTo(stream1);
                    }
                    stream1.Position = 0;
                    var ser = new DataContractJsonSerializer(typeof(List<RoomState>));
                    ans = (List<RoomState>) ser.ReadObject(stream1);
                }
            return ans;
        }
    }
}