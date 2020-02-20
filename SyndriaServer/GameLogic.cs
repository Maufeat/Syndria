using SyndriaServer.Models;
using SyndriaServer.Utils;
using System.Collections.Generic;

namespace SyndriaServer
{
    public class GameLogic
    {
        public static Dictionary<int, TutorialFight> fights = new Dictionary<int, TutorialFight>();

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
