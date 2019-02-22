using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using n=NLog;

namespace KKB.BankKlient.BLL.Account
{
    public class ServiceAccount
    {
        private static n.Logger logger = n.LogManager.GetCurrentClassLogger();
        
        Random rnd = new Random();
        public Account CreateAccount(User.User user, currency cur)
        {
            Account account = new Account();
            account.CreateDate = DateTime.Now.AddMonths(rnd.Next(1, 12) * -1);
            account.Number = cur.ToString().ToUpper() + rnd.Next();
            account.Balance = 0;
            account.UserId = user.Id;
            account.Currency = cur;
            return account;
        }
        public bool CreateAccountDb(Account account, out string message)
        {
            try
            {
                using (var db = new LiteDatabase(@"kkb.db"))
                {
                    var accounts = db.GetCollection<Account>("Account");

                    accounts.Insert(account);
                }

                message = "Создание счета прошло успешно";
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
        public bool UpdateAccountDb(Account account, out string message)
        {
            try
            {
                using (var db = new LiteDatabase(@"kkb.db"))
                {
                    var accounts = db.GetCollection<Account>("Account");

                    accounts.Update(account);
                }

                message = "Операция прошла успешно";
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }


        public static List<Account> GetAccountsByUserId(int id)
        {
            try
            {
                using (var db = new LiteDatabase(@"kkb.db"))
                {
                    var accounts = db.GetCollection<Account>("Account");

                    return accounts.FindAll().ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Get account by user Id has error: " + ex.Message);
                return null;
            }
        }

        public string IncreaseAccount(List<Account> accounts, int accountId, decimal sum)
        {
            string message = " ";
            try
            {
                if (accounts[accountId - 1].Id == accountId)
                {
                    accounts[accountId - 1].Balance += sum;
                    UpdateAccountDb(accounts[accountId - 1], out message);
                    return message;
                }
                else
                {
                    return message;
                }

            }
            catch (Exception ex)
            {
                message=ex.Message;
                return message;
            }
        }

        public string DecreaseAccount(List<Account> accounts, int accountId, decimal sum)
        {
            string message = " ";
            try
            {
                if (accounts[accountId - 1].Id == accountId)
                {
                    accounts[accountId - 1].Balance -= sum;
                    UpdateAccountDb(accounts[accountId - 1], out message);
                    return message;
                }
                else
                {
                    return message;
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
                return message;
            }
        }
    }
}
