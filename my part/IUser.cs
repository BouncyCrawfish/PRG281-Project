using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_281_Project
{
    public interface IUser
    {
        int UserID { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        bool Login(string username, string password);
        void Logout();
        bool HasPermission(string action);
    }

}
