using System;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace C969
{
    class User
    {
        int userId;
        string userName;

        public int ID
        {
            get { return userId; }
        }
        private bool IsHash(string password)
        {
            string sha512Pattern = @"^[A-Fa-f0-9]{50}$"; //50 characters; must be hex characters (ie 0-9, A-F, hex strings are case insensitive so also a-f)
            Regex regex = new Regex(sha512Pattern);
            bool isMatch = regex.IsMatch(password);
            return isMatch;
        }
        private string HashPassword(string password)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(userId.ToString() + password + userName);
                byte[] hashBytes = sha512.ComputeHash(inputBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 50);
                return hash;
            }
        }
        public User(int userId1)
        {
            //lookup user from database.
            if (Database.UserGet(userId1, out string userName1))
            {
                userId = userId1;
                userName = userName1;
            }
        }
        internal User(string userName1, string password)
        {
            userId = Database.UserAdd(userName);
            userName = userName1;
            SetPassword(password);
        }
        internal User(int userId1, string userName1)
        {
            //no need to query database. 
            userId = userId1;
            userName = userName1;
        }
        public User(string userName1)
        {
            //lookup userId from database.
            if (Database.UserGet(userName1, out int userId1))
            {
                userName = userName1;
                userId = userId1;
            }
        }
        public string DisplayName()
        {
            return this.userName;
        }
        private bool SetPassword(string password)
        {
            if (IsHash(password))
            {
                return Database.UserPasswordSet(userId, password);
            }
            else
            {
                return Database.UserPasswordSet(userId, HashPassword(password));
            }
        }
        public bool ChangePassword(string oldPassword, string newPassword)
        {
            if (VerifyPassword(oldPassword))
            {
                return SetPassword(newPassword);
            }
            else
            {
                throw new Exception(Languages.LanguageFill("$wrongpassword"));
            }
        }
        public bool VerifyPassword(string password)
        {
            if (password == "")
            {
                return false;
            }
            if (!IsHash(password))
            {
                password = HashPassword(password);
            }
            if (Database.UserPasswordGet(userId, out string dbPassword))
            {
                if (!IsHash(dbPassword))
                {
                    SetPassword(dbPassword);
                    dbPassword = HashPassword(dbPassword);
                }
                if (dbPassword == password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new Exception(Languages.LanguageFill("Error: $usernamenotfound"));
            }
        }
    }
}
