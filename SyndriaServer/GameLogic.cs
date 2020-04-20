using SyndriaServer.Models;
using SyndriaServer.Utils;
using System.Collections.Generic;

namespace SyndriaServer
{
    public class GameLogic
    {
        public static Dictionary<int, Fight> fights = new Dictionary<int, Fight>();

        public static void Update()
        {
            ThreadManager.UpdateMain();

            /*foreach (var fight in fights)
            {
                fight.Value.Update();
            }*/
        }
    }
}
