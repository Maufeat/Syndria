using SyndriaServer.Models;
using SyndriaServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndriaServer
{
    public class Player
    {
        public int connectionId;
        public DebugToken fbToken;

        //Fields
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime lastLogin { get; set; }
        public DateTime lastDaily { get; set; }
        public int dailyCount { get; set; }
        public int level { get; set; }
        public int exp { get; set; }
        public int energy { get; set; }
        public DateTime lastUsedEnergy { get; set; }
        public int gold { get; set; }
        public int diamonds { get; set; }
        public int tutorialDone { get; set; }

        public List<PlayerHero> heroes { get; set; }

        public Player()
        {
        }

        public void GetUserData()
        {
        }

        public bool UpdateHeroes()
        {
            return DatabaseManager.getHeroes(this);
        }

        public void OnDataRecieve(byte[] data)
        {
            /*LittleEndianReader reader = new LittleEndianReader(data);
            PacketCmd cmd = (PacketCmd)reader.ReadShort();

            switch (cmd)
            {
                case PacketCmd.C2S_FBLogin:
                    
                    var facebookClient = new FacebookClient();
                    var facebookService = new FacebookService(facebookClient);
                    var verifyTask = facebookService.VerifyAccessToken(reader.ReadSizedString());
                    Task.WaitAll(verifyTask);
                    fbToken = verifyTask.Result;

                    Console.WriteLine($"[>>][{connectionId}][{cmd.ToString()}] Token is {fbToken.IsValid} - Logging in.");

                    LittleEndianWriter writer = new LittleEndianWriter();

                    if (DatabaseManager.getAccountByFacebookId(this))
                    {
                        writer.WriteShort((short)PacketCmd.S2C_UserDataToVillage);
                        writer.WriteSizedString(Username);
                        writer.WriteInt(level);
                        writer.WriteInt(exp);
                        writer.WriteInt(gold);
                        writer.WriteInt(diamonds);

                        server.Send(connectionId, writer.Data);
                        Console.WriteLine($"[<<][{connectionId}][{Username}] Logged In.");

                        writer.Clear();
                        if (tutorialDone == 0)
                        {
                            writer.WriteShort((short)PacketCmd.S2C_GoToTutorial);
                            Console.WriteLine($"[>>][{connectionId}][{PacketCmd.S2C_GoToTutorial.ToString()}] Send User To Tutorial");
                        } else
                        {
                            Console.WriteLine($"[>>][{connectionId}][{PacketCmd.S2C_GoToTutorial.ToString()}] Send User To Village");
                        }
                        server.Send(connectionId, writer.Data);
                    } else
                    {
                        writer.WriteShort((short)PacketCmd.S2C_CreateCharacter);
                        server.Send(connectionId, writer.Data);
                        Console.WriteLine($"[<<][{connectionId}][{PacketCmd.S2C_CreateCharacter.ToString()}] Take to Create Character Screen...");
                    }

                    break;
                case PacketCmd.C2S_StartTutorial:
                    LittleEndianWriter writeTutorial = new LittleEndianWriter();
                    writeTutorial.WriteShort((short)PacketCmd.S2C_GoToTutorial);
                    server.Send(connectionId, writeTutorial.Data);
                    break;
                case PacketCmd.C2S_CreateCharacter:

                    var nickname = reader.ReadSizedString();
                    var heroId = reader.ReadInt();

                    Console.WriteLine(heroId);

                    DatabaseManager.createUser(nickname, this);
                    DatabaseManager.getAccountByFacebookId(this);
                    DatabaseManager.addNinjaToPlayer(heroId, this);

                    LittleEndianWriter writer2 = new LittleEndianWriter();
                    writer2.WriteShort((short)PacketCmd.S2C_UserDataToVillage);
                    writer2.WriteSizedString(Username);
                    writer2.WriteInt(level);
                    writer2.WriteInt(exp);
                    writer2.WriteInt(gold);
                    writer2.WriteInt(diamonds);
                    server.Send(connectionId, writer2.Data);

                    Console.WriteLine($"[>>][{connectionId}][{cmd.ToString()}] Created User: " + nickname);

                    writer2.Clear();
                    writer2.WriteShort((short)PacketCmd.S2C_GoToTutorial);
                    server.Send(connectionId, writer2.Data);
                    Console.WriteLine($"[>>][{connectionId}][{PacketCmd.S2C_GoToTutorial.ToString()}] Send User To Tutorial");
                    break;
                default:
                    Console.WriteLine($"[>>][{connectionId}] Unknown PacketCmd: {cmd} Recieved.");
                    break;
            }*/
        }
    }
}