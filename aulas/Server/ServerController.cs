using System.Collections.Generic;
using System.Linq;
using Business;
using Business.Exceptions;
using Entities;
using Protocol;

namespace Server
{

    public class ServerController
    {

        private readonly Logic logic;

        private string picture = string.Empty;

        private string picturesUsername;

        private string extension;

        public ServerController(Logic logic)
        {
            this.logic = logic;
        }

        public void InvalidCommand(Connection connection)
        {
            object[] response = BuildResponse(ResponseCode.BadRequest, "Unrecognizable command");
            connection.SendMessage(response);
        }

        public void ConnectClient(Connection connection, Request request)
        {
            try
            {
                var client = new Entities.Client(request.Username(), request.Password());
                string token = logic.Login(client);

                object[] response = string.IsNullOrEmpty(token)
                    ? BuildResponse(ResponseCode.NotFound, "Client not found. Wrong username or password.")
                    : BuildResponse(ResponseCode.Ok, token);
                connection.SendMessage(response);
            }
            catch (ClientAlreadyConnectedException e)
            {
                connection.SendMessage(BuildResponse(ResponseCode.Forbidden, e.Message));
            }
        }

         public void ListAllSubjects(Connection connection, Request request)
        {
            try
            {
                List<Subject> subjects = logic.GetSubjects();

                string[] subjectsNames = subjects.Select(c => c.Name).ToArray();

                string[] hola = new string[1];
                hola[0]="hola";

                connection.SendMessage(BuildResponse(ResponseCode.Ok, hola));
            }
         /*  catch (RecordNotFoundException e)
            {
                connection.SendMessage(BuildResponse(ResponseCode.NotFound, e.Message));
            } */ 
            catch (ClientNotConnectedException e)
            {
                connection.SendMessage(BuildResponse(ResponseCode.Unauthorized, e.Message));
            }
        }

        public void DisconnectClient(Connection connection, Request request)
        {
            try
            {
                logic.DisconnectClient(request.UserToken());
                connection.SendMessage(BuildResponse(ResponseCode.Ok, "Client disconnected"));
                connection.Close();
            }
          /*  catch (RecordNotFoundException e)
            {
                connection.SendMessage(BuildResponse(ResponseCode.NotFound, e.Message));
            }*/
            catch (ClientNotConnectedException e)
            {
                connection.SendMessage(BuildResponse(ResponseCode.Unauthorized, e.Message));
            }
        }

        private Entities.Client CurrentClient(Request request)
        {
            return logic.GetLoggedClient(request.UserToken());
        }

        private object[] BuildResponse(int responseCode, params object[] payload)
        {
            var responseList = new List<object>(payload);
            string code = responseCode.ToString();
            responseList.Insert(0, responseCode.ToString());

            return responseList.ToArray();
        }

    }
}