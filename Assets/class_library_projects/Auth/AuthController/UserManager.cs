using AuthSystem;
using System;
using System.IO;

namespace AuthSystem
{
    public class UserManager
    {
        private const string UsersFilePath = "users.txt";

        public void Register(string username, string password)
        {
            if (UserExists(username))
            {
                throw new Exception("Пользователь с таким именем уже существует.");
            }

            string hashedPassword = HashHelper.Hash(password);
            string userEntry = $"{username}:{hashedPassword}";

            File.AppendAllText(UsersFilePath, userEntry + Environment.NewLine);
        }

        public string Login(string username, string password)
        {
            if (!UserExists(username))
            {
                return "Неверное имя пользователя";
            }

            string hashedPassword = HashHelper.Hash(password);
            string[] lines = File.ReadAllLines(UsersFilePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts[0] == username)
                {
                    if (parts[1] == hashedPassword)
                    {
                        return null;
                    }
                    else
                    {
                        return "Неверный пароль";
                    }
                }
            }

            return "Неверное имя пользователя";
        }

        private bool UserExists(string username)
        {
            if (!File.Exists(UsersFilePath))
            {
                return false;
            }

            string[] lines = File.ReadAllLines(UsersFilePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts[0] == username)
                {
                    return true;
                }
            }

            return false;
        }
    }
}