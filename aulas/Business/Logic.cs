using System.Collections.Generic;
using Business.Exceptions;
using Entities;
using Persistence;

namespace Business
{
    public class Logic
    {

        public Store Store { get; set; }
        private Server Server { get; set; }
        private StudentLogic StudentLogic { get; set; }

        private readonly object loginLock = new object();

        public Logic(Store store)
        {
            Store = store;
            Server = new Server();
            StudentLogic = new StudentLogic(Store);
        }

        public string Login(Client client)
        {
            lock (loginLock)
            {
                if (!Store.ClientExists(client))
                    Store.AddClient(client);
                Client storedClient = Store.GetClient(client.Username);
                bool isValidPassword = storedClient.ValidatePassword(client.Password);
                bool isClientConnected = Server.IsClientConnected(client);
                if (isValidPassword && isClientConnected)
                    throw new ClientAlreadyConnectedException();
                return isValidPassword ? Server.ConnectClient(storedClient) : "";
            }
        }

        public Client GetLoggedClient(string userToken)
        {
            lock (loginLock)
            {
                Client loggedUser = Server.GetLoggedClient(userToken);
                if (loggedUser == null)
                    throw new ClientNotConnectedException();
                return loggedUser;
            }
        }

        public List<Client> GetLoggedClients()
        {
            lock (loginLock)
            {
                return Server.GetLoggedClients();
            }
        }

        public List<Subject> GetSubjects()
        {
            lock (loginLock)
            {
                return Store.GetSubjects();
            }
        }

        public void DisconnectClient(string token)
        {
            lock (loginLock)
            {
                Server.DisconnectClient(token);
            }
        }

    }
}