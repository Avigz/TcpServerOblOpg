using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace TcpServerOblOpg
{
    public class Program
    {
        private static List<Book> books = new List<Book>()
        {
            new Book("Book1", "Mr.Book1",11,"a123456789110"),
            new Book("Book2", "Mr.Book2",12,"a123456789111"),
            new Book("Book3", "Mr.Book3",13,"a123456789112"),

        };

        static void Main(string[] args)
        {

            int port = 4646;
            int clientNr = 0;

            Console.WriteLine("Hello Echo Server!");

            IPAddress ip = IPAddress.Loopback;
            TcpListener ServerListener = StartServer(ip, port);

            do
            {
                TcpClient ClientConnection = GetConnectionSocket(ServerListener, ref clientNr);
                Task.Run(() => ReadWriteStream(ClientConnection, ref clientNr));
             

            } while (clientNr != 0);

            StopServer(ServerListener);
        }
        private static void StopServer(TcpListener serverListener)
        {
            serverListener.Stop();
            Console.WriteLine("listener stopped");
        }

        private static TcpClient GetConnectionSocket(TcpListener serverListener, ref int clientNr)
        {

            TcpClient connectionSocket = serverListener.AcceptTcpClient();
            clientNr++;
            //Socket connectionSocket = serverSocket.AcceptSocket();
            Console.WriteLine("Client " + clientNr + " connected");
            return connectionSocket;
        }

        private static void ReadWriteStream(TcpClient connectionSocket, ref int clientNr)
        {
            Stream ns = connectionSocket.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true; // enable automatic flushing

            string message = sr.ReadLine();
            Thread.Sleep(1000);
            string answer = "";
            while (message != null && message != "")
            {
                Console.WriteLine("Client: " + clientNr + " " + message);
                string[] messageArray = message.Split(' ');

                
                for (int i = 0; i < messageArray.Length; i++)
                {
                    
                    if (messageArray[0] == "GetAll")
                    {
                        foreach (var v in books)
                        {
                            answer = JsonConvert.SerializeObject(v);
                            sw.WriteLine(answer);
                        }

                        message = "";
                    }

                    if (messageArray[0] == "Get" && messageArray[1] != null)
                    {
                        answer = JsonConvert.SerializeObject(books.Find(i => i.ISBN13 == messageArray[1]));
                        sw.WriteLine(answer);

                        message = "";
                    }

                    if (messageArray[0] == "Save" && messageArray[1] != null || messageArray[1] != "")
                    {
                        int bookCount = books.Count;
                        books.Add(JsonConvert.DeserializeObject<Book>(messageArray[1]));
                        if (bookCount < books.Count)
                        {
                            answer = "Saved Correctly";
                            sw.WriteLine(answer);
                        }
                    }
                  
                    Thread.Sleep(1000);
                }
            }

            Console.WriteLine("Empty message detected");
            ns.Close();
            connectionSocket.Close();
            clientNr--;
            Console.WriteLine("connection socket " + clientNr + " closed");

        }

      

        

        private static TcpListener StartServer(IPAddress ip, int port)
        {
            TcpListener serverSocket = new TcpListener(ip, port);
            serverSocket.Start();

            Console.WriteLine("server started waiting for connection!");
            Console.WriteLine("Ip: " + ip);
            Console.WriteLine("Port: " + port);

            return serverSocket;
        }


    }
}

