using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcpClient
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(100);
                // Adres IP serwera i port, na którym nasłuchuje
                string server = "192.168.31.196";
                int port = 11000;
                string password = "DensoServerFIOT";
                // Połączenie z serwerem

                using (System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient(server, port))
                using (NetworkStream stream = client.GetStream())
                {
                    // Wysłanie hasła
                    byte[] passwordData = Encoding.UTF8.GetBytes(password + "\n");
                    await stream.WriteAsync(passwordData, 0, passwordData.Length);
                    Console.WriteLine("Wysłano hasło: {0}", password);

                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string confirmationMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Serwer odpowiedział: {0}", confirmationMessage);

                    // Jeśli hasło zostało zaakceptowane, wysyłamy wiadomość
                    if (confirmationMessage.Trim() == "Password accepted")
                    {
                        string message = "Nazdar, Pepinko :)";
                        byte[] messageData = Encoding.UTF8.GetBytes(message);
                        await stream.WriteAsync(messageData, 0, messageData.Length);
                        Console.WriteLine("Wysłano: {0}", message);
                    }
                    else
                    {
                        Console.WriteLine("Błąd autoryzacji - hasło nieprawidłowe.");
                    }
                }
            }
        }
    }
}