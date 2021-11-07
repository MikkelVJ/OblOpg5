using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballPlayer;

namespace TCP_Server
{
    class FBPlayersManager
    {
        private static int _nextId = 1;

        private static readonly List<FBPlayer> Data = new List<FBPlayer>()
        {
            new FBPlayer("john", 13, _nextId++, 13),
            new FBPlayer("jen", 31, _nextId++, 31)
        };

        public List<FBPlayer> GetAll()
        {
            return new List<FBPlayer>(Data);
        }

        public FBPlayer GetById(int id)
        {
            return Data.Find(Item => Item.Id == id);
        }

        public FBPlayer Add(FBPlayer newFBPlayer)
        {
            newFBPlayer.Id = _nextId++;
            Data.Add(newFBPlayer);
            return newFBPlayer;
        }
    }
}
