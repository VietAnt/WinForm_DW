using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System_WareHouse
{
    class ListAccount
    {
        private static ListAccount instance;

        internal static ListAccount Instance
        {
            get
            {
                if (instance == null)
                    instance = new ListAccount();
                return instance;
            }
            set => instance = value;
        }

        List<Account> listAccounts;
        public List<Account> ListAccounts { get => listAccounts; set => listAccounts = value; }


        //
        ListAccount()
        {
            ListAccounts = new List<Account>();
            ListAccounts.Add(new Account("tranviet11", "12345"));
            ListAccounts.Add(new Account("admin", "admin"));
            ListAccounts.Add(new Account("viethau", "12345"));
        }
    }
}
