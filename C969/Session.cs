using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace C969
{
    static class Session
    {
        static bool authenticated = false;
        static Dictionary<string, string> _appVariables = new Dictionary<string, string>();

        public static string GetVariable(string variableName)
        {
            if (_appVariables.ContainsKey(variableName.Replace("$", "")))
            {
                return _appVariables[variableName.Replace("$", "")];
            }
            else
            {
                return "";
            }
        }
        static bool SetVariable(string variableName, string variableValue)
        {
            if (_appVariables.ContainsKey(variableName))
            {
                _appVariables[variableName] = variableValue;
            }
            else
            {
                _appVariables.Add(variableName, variableValue);
            }
            if (_appVariables.ContainsKey(variableName) && _appVariables[variableName] == variableValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //insert user (username,password,active,createdate,createdby,lastupdateby)
        public static MySqlConnection GetDBConnection()
        {
            if (!authenticated)
            {
                Session.SetVariable("username", "_unauthenticated (" + System.Environment.MachineName + ")");
            }
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            return connection;
        }
        public static void AddParameters(ref MySqlCommand command)
        {
            command.Parameters.AddWithValue("@username", GetVariable("username"));
        }
        public static bool Logoff()
        {
            try
            {
                authenticated = false;
                SetVariable("username", "");
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool Login(string username, string password)
        {
            User user = new User(username);
            if (!user.VerifyPassword(password))
            {
                throw new Exception(Languages.LanguageFill("$wrongpassword"));
            }
            else
            {
                if (SetVariable("username", username))
                {
                    authenticated = true;
                    return true;
                }
                else
                {
                    throw new Exception(Languages.LanguageFill("$cannotset $username"));
                }
            }
        }
        public static bool IsAuthenticated()
        {
            return authenticated;
        }
    }
}
