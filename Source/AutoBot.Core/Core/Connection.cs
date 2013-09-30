using System;
using System.Net.Security;
using System.Net.Sockets;
using System.IO;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace AutoBot.Core
{
    /// <summary>
    /// Handles a connection to an IRC server
    /// </summary>
    public class Connection : IConnection
    {
        private TcpClient client;
        private SslStream network;
        private StreamReader receive;
        private StreamWriter send;

        /// <summary>
        /// Gets or sets the console.
        /// </summary>
        /// <value>
        /// The console.
        /// </value>
        public IConsole Console { get; set; }

        /// <summary>
        /// Connects the specified server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public bool Connect(string server, int port)
        {
            try
            {
                client = new TcpClient(server, port);
                network = new SslStream(client.GetStream(), false, ValidateServerCert, null, EncryptionPolicy.RequireEncryption);
                network.AuthenticateAsClient(server);

                receive = new StreamReader(network);
                send = new StreamWriter(network);

                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.GetType() + ": " + ex.Message);
                System.Console.WriteLine(ex.StackTrace);

                return false;
            }
        }

        public void GetSocketStatus()
        {
            var s = client.Client;

            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);

            if (part1 & part2)
            {
                System.Console.WriteLine("Socket Disconnected");
            }
            else
            {
                System.Console.WriteLine("Socket Connected");
            }
        }

        /// <summary>
        /// Validates the server certificate.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="certificate">The certificate.</param>
        /// <param name="chain">The chain.</param>
        /// <param name="sslpolicyerrors">The sslpolicyerrors.</param>
        /// <returns></returns>
        private bool ValidateServerCert(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {

            if (sslpolicyerrors != SslPolicyErrors.None)
            {
                System.Console.WriteLine("SSL Cert Error: " + sslpolicyerrors);
            }

            // Allow any certificate
            // TODO: Make the user force the use of self signed certificates
            return true;
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Send(string data)
        {
            try
            {
                System.Console.WriteLine("<< " + data);



                send.WriteLine(data);
                send.Flush();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.GetType() + ": " + ex.Message);
                System.Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="args">The args.</param>
        public void Send(string data, params object[] args)
        {
            Send(string.Format(data, args));
        }

        /// <summary>
        /// Receives some data from the server.
        /// </summary>
        /// <returns></returns>
        public string Receive()
        {
            try
            {
                return receive.ReadLine();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.GetType() + ": " + ex.Message);
                System.Console.WriteLine(ex.StackTrace);

                return string.Empty;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            send.Close();
            receive.Close();
            network.Close();
            client.Close();
        }
    }
}
