using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using Business;
using Business.Exceptions;
using Persistence;
using Protocol;
using UI;
using System.Runtime.InteropServices;

namespace Server
{

    class Program
    {

        private static bool endServer = false;
        private static List<Thread> threads = new List<Thread>();
        private static List<Connection> connections = new List<Connection>();
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static void Main(string[] args)
        {
            var server = new ServerProtocol();
            int port = GetServerPortFromConfigFile();
            string ip = GetServerIpFromConfigFile();
            server.Start(ip, port);
            var logic = new Logic(new Store());

            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);
            var thread = new Thread(() =>
            {
                var router = new Router(new ServerController(logic));
                while (!endServer)
                {
                    try
                    {
                        var clientSocket = server.Socket.Accept();
                        var clientThread = new Thread(() =>
                        {
                            try
                            {
                                Connection conn = new Connection(clientSocket);
                                connections.Add(conn);
                                router.Handle(conn);
                            }
                            catch (Exception)
                            {
                                endServer = true;
                            }
                        });
                        threads.Add(clientThread);
                        clientThread.Start();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(" -> The server has stopped listening for connections.");
                    }
                }
            });
            threads.Add(thread);
            thread.Start();
            bool exit = false;
            while (!exit)
            {
                int option = Menus.ServerMainMenu();

                GoToMenuOption(option, logic);
                if (option == 3)
                {
                    endServer = true;
                    exit = true;
                }

            }
            CloseServer(server, logic);
        }

        private static void CloseServer(ServerProtocol server, Logic logic)
        {
            try
            {
                server.Socket.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Closing the thread that is listening for connections.");
            }

            if (connections.Count > 0)
            {
                foreach (Connection connection in connections)
                {
                    try
                    {
                        connection.Close();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Forcing socket to close.");
                    }
                }
            }
            CloseThreads();
        }

        private static void CloseThreads()
        {
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            Console.WriteLine("Every thread has been closed. Good-bye.");
        }

        private static void GoToMenuOption(int option, Logic controller)
        {
            if (option == 1)
                Console.WriteLine("\n -> Opcion 1\n");
            else if (option == 2)
            {
                Console.WriteLine("\n -> Opcion2\n");
            }
        }

        private static string GetServerIpFromConfigFile()
        {
            return "192.168.1.4";
            //var appSettings = new AppSettingsReader();
            //return (string)appSettings.GetValue("ServerIp", typeof(string));
        }

        private static int GetServerPortFromConfigFile()
        {
            return 4013;
            //var appSettings = new AppSettingsReader();
            //return (int)appSettings.GetValue("ServerPort", typeof(int));
        }

    }

}