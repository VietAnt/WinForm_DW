using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System_WareHouse
{
    class Account
    {
        string userName;
        string passWorld;

        public string UserName { get => userName; set => userName = value; }
        public string PassWorld { get => passWorld; set => passWorld = value; }

        public Account(string username, string password)
        {
            this.userName = username;
            this.passWorld = password;
        }
    }
}
