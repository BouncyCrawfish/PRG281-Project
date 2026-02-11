using PRG_281_Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_281_Project
{
    
    
        public class User : IUser
        {
            public int UserID { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }

            public bool Login(string username, string password)
            {
                if (ValidateCredentials(username, password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void Logout()
            {
                ClearSessionData();
            }

            public bool HasPermission(string action)
            {
                return CheckUserPermissions(UserID, action);
            }

            private bool ValidateCredentials(string username, string password)
            {
                return Username == username && Password == password;
            }

            private void ClearSessionData()
            {
                Console.WriteLine("Session data cleared");
            }

            private bool CheckUserPermissions(int userID, string action)
            {
                return Username == "Admin"; // Simple permission check
            }
        }
}

