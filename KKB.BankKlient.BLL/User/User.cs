using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KKB.BankKlient.BLL.User
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; }
        public string Iin { get; set; }
        public Doc Doc { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public List<KKB.BankKlient.BLL.Account.Account> Accounts = new List<BLL.Account.Account>();
    }

    public class Doc
    {
        public int Number { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpDate { get; set; }
    }
}
