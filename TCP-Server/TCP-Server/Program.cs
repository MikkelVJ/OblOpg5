using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FootballPlayer;

namespace TCP_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TCP-Server");

            TcpListener listener = new TcpListener(IPAddress.Loopback, 2121);
            listener.Start();
            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Console.WriteLine("Client Connecting...");
                Task.Run(() => { HandleClient(socket); });
            }
        }

        public static void HandleClient(TcpClient socket)
        {
            bool connection = true;
            while (connection)
            {
                NetworkStream ns = socket.GetStream();
                StreamReader reader = new StreamReader(ns);
                StreamWriter writer = new StreamWriter(ns);
                string message = reader.ReadLine();
                string id = reader.ReadLine();
                Console.WriteLine($"Message: {message}\nID/Object: {id}");
                writer.Write(RequestHandler(message, id));
                writer.Flush();
                if (message == "-1")
                {
                    socket.Close();
                    connection = false;
                }
            }
        }

        private static readonly FBPlayersManager _manager = new FBPlayersManager();

        public static string RequestHandler(string message, string id)
        {
            if (message.ToLower().Trim() == "hentalle")
            {
                string data = JsonSerializer.Serialize(_manager.GetAll());
                return data;
            }

            if (message.ToLower().Trim() == "hent")
            {
                int result = int.Parse(id);
                string data = JsonSerializer.Serialize(_manager.GetById(result));
                return data;
            }

            if (message.ToLower().Trim() == "gem")
            {
                FBPlayer player = JsonSerializer.Deserialize<FBPlayer>(id);
                _manager.Add(player);
                string data = ""; //JsonSerializer.Serialize(_manager.GetById(player.Id));
                return data;
            }

            return "Command not recognized";
        }

    }
}
