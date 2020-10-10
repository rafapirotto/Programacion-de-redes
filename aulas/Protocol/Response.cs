using System;
using System.Collections.Generic;

namespace Protocol
{
    public class Response
    {
        private readonly string[] responsePackage;
        
        private string MyResponseCode => responsePackage[0];

        private int Code {
            get{return Int32.Parse(MyResponseCode);}
            set{}
        }
        
        public Response(string[] response)
        {
            responsePackage = response;
        }

        public string GetClientToken()
        {
            return responsePackage[1];
        }

        public string GetUsername()
        {
            return responsePackage[2];
        }

        public string ErrorMessage()
        {
            return responsePackage[1];
        }
  
        public List<string> UserList()
        {
            var users = new List<string>();
            for (var i = 1; i < responsePackage.Length; i++)
                users.Add(responsePackage[i]);
            return users;
        }

        public bool HadSuccess()
        {
            return HasCode(ResponseCode.Ok) || HasCode(ResponseCode.Created);
        } 
        

        private bool HasCode(int responseCode)
        {
            if(MyResponseCode != null)
            {
                return Code == responseCode;
            }else
            {
                return false;
            }
        }

    }
}