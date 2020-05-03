using SyndriaServer.Utils;
using SyndriaServer.Utils.Network;
using System;
using System.Threading;

namespace SyndriaServer
{
    class Program
    {
        private static bool isRunning = false;

        static void Main(string[] args)
        {
            Console.Title = "Syndria - Game Server";

            if (!DatabaseManager.InitConnection())
            {
                Console.ReadKey();
                return;
            }

            GameLogic.LoadSpellPatterns();
            GameLogic.LoadHeroBase();
            GameLogic.LoadAllMaps();

            isRunning = true;

            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();

            Server.Start(50, Constants.SERVER_PORT);
        }


        private static void MainThread()
        {
            Logger.Write($"Main thread started. Running at {Constants.TICKS_PER_SEC} ticks per second.");
            DateTime _nextLoop = DateTime.Now;

            while (isRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    GameLogic.Update();

                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);

                    if (_nextLoop > DateTime.Now)
                    {
                        Thread.Sleep(_nextLoop - DateTime.Now);
                    }
                }
            }
        }
    }
}
