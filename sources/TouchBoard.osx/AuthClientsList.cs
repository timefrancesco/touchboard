using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ninjaKeyserverosx
{
    

    public static class ClientUtil
    {
        static string _dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NinjaKey\\";
        static string _clientsFile = _dataFolder + "clients.ninjalist";
        static string _clientsFileTemp = _dataFolder + "clients.ninjalist.temp";

        public static bool AddClient(AuthClient client)
        {
            if (!Directory.Exists(_dataFolder))
                Directory.CreateDirectory(_dataFolder);

            List<AuthClient> clients = ReadClientsFromDisk(_clientsFile);
            if (clients == null)
                clients = new List<AuthClient>();

            clients.Add(client);

            return WriteClientsToDisk(clients);           

        }

        public static bool RemoveClient(AuthClient client)
        {
            if (!Directory.Exists(_dataFolder))
                return false;

            List<AuthClient> clients = ReadClientsFromDisk(_clientsFile);
            if (clients == null)
                return false;

            var lookd = clients.Where(ip => ip.IPaddress.Equals(client.IPaddress)).FirstOrDefault();
            clients.Remove(lookd);

            return WriteClientsToDisk(clients);
        }

        public static List<AuthClient> GetClients()
        {
            return ReadClientsFromDisk(_clientsFile);
        }

        private static bool WriteClientsToDisk(List<AuthClient> list)
        {
            FileStream fs2 = new FileStream(_clientsFileTemp, FileMode.Create);
            BinaryFormatter bf2 = new BinaryFormatter();

            bf2.Serialize(fs2, list);
            fs2.Close();

            var writtenClients = ReadClientsFromDisk(_clientsFileTemp);
            if (writtenClients != null)
            {
                File.Delete(_clientsFile);
                File.Copy(_clientsFileTemp, _clientsFile);
                File.Delete(_clientsFileTemp);
                return true;
            }

            return false;

        }

        private static List<AuthClient> ReadClientsFromDisk(string input)
        {
            List<AuthClient> clients = null;
            if (File.Exists(input))
            {
                try
                {
                    FileStream fs = new FileStream(input, FileMode.Open);
                    BinaryFormatter bf = new BinaryFormatter();
                    clients = (List<AuthClient>)bf.Deserialize(fs);
                    fs.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[IO] Error reading clients file: " + ex.Message);
                }
            }

            return clients;
        }
    }

    [Serializable]
    public class AuthClient
    {
        public string IPaddress { get; set; }
        public string HostName { get; set; }
    }
}
