using KKB.BankKlient.BLL.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KKB.BankKlient.BLL.User.Account;
using System.Threading;
using KKB.BankKlient.BLL.Account;
using n = NLog;

namespace KKB.BankKlient.Web.Model
{

    public class ServiceMenu
    {
        private static User AuthorUser = null;
        private static ServiceUser service = null;
        private static n.Logger logger = n.LogManager.GetCurrentClassLogger();
        
        static ServiceMenu()
        {
            service = new ServiceUser();
        }

        public static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Добро пожаловать в KKB!\n");

            Console.WriteLine("1. Регистрация");
            Console.WriteLine("2. Вход");

            Console.Write(": ");
            int menu = Int32.Parse(Console.ReadLine());

            if (menu == 1)
            {
                RegisterMenu();
            }
            else if (menu == 2)
            {
                LogOnMenu();
            }
        }

        public static void RegisterMenu()
        {
            Console.Clear();
            Console.WriteLine("Форма регистрации пользователя\n");

            User user = new User();

            Console.Write("FirstName: ");
            user.FirstName = Console.ReadLine();

            Console.Write("LastName: ");
            user.LastName = Console.ReadLine();

            Console.Write("Login: ");
            user.Login = Console.ReadLine();

            Console.Write("Password: ");
            user.Password = Console.ReadLine();

            string message = "";
            if (service.RegisterUser(user, out message))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;

                Thread.Sleep(3000);
                MainMenu();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void LogOnMenu()
        {
            Console.Clear();
            Console.WriteLine("");

            Console.Write("Login: ");
            string login = Console.ReadLine();
            logger.Info("Login: " + login);

            Console.Write("Password: ");
            string password = Console.ReadLine();
            logger.Info("Password: " + password);
            string message = "";
            User user = service.LogOn(login, password,
                                       out message);

            if (user != null)
            {
                AuthorUser = user;
                AuthorUser.Accounts = ServiceAccount.GetAccountsByUserId(AuthorUser.Id);
                AuthorizeUserMenu();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;

                Thread.Sleep(3000);
                LogOnMenu();
            }
        }

        public static void AuthorizeUserMenu()
        {
            Console.Clear();

            Console.WriteLine("Приветствуем Вас, {0} {1}\n",
                AuthorUser.FirstName, AuthorUser.LastName);
            if (AuthorUser.Accounts != null && AuthorUser.Accounts.Count > 0)
            {
                Console.WriteLine("1. Вывод баланса на экран");
                Console.WriteLine("2. Пополнение счёта");
                Console.WriteLine("3. Снять деньги со счёта");
                Console.WriteLine("4. Создать дополнительный счет");
            }
            else
            {
                Console.WriteLine("6. Создать счет");

            }
            Console.WriteLine("5. Выход");

            Console.Write(": ");
            int menu = Int32.Parse(Console.ReadLine());
            if (menu == 6)
            {
                ServiceAccount serviceAcc = new ServiceAccount();
                Account acc = serviceAcc.CreateAccount(AuthorUser, currency.kzt);
                string message = "";
                if (serviceAcc.CreateAccountDb(acc, out message))
                {
                    Console.ForegroundColor = ConsoleColor.Green;

                    Console.WriteLine(message);
                    Console.ForegroundColor = ConsoleColor.White;

                    AuthorUser.Accounts = ServiceAccount.GetAccountsByUserId(AuthorUser.Id);

                    Thread.Sleep(3000);
                    AuthorizeUserMenu();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if (menu == 1)
            {
                Console.Clear();
                PrintBalanseOnScreen();
                Console.ReadLine();
                AuthorizeUserMenu();
            }
            else if (menu == 4)
            {
                ServiceAccount serviceAcc = new ServiceAccount();
                Account acc = serviceAcc.CreateAccount(AuthorUser, currency.kzt);
                string message = "";
                if (serviceAcc.CreateAccountDb(acc, out message))
                {
                    Console.ForegroundColor = ConsoleColor.Green;

                    Console.WriteLine(message);
                    Console.ForegroundColor = ConsoleColor.White;

                    AuthorUser.Accounts = ServiceAccount.GetAccountsByUserId(AuthorUser.Id);

                    Thread.Sleep(3000);
                    AuthorizeUserMenu();
                }
            }
            else if (menu == 2)
            {
                ServiceAccount serviceAcc = new ServiceAccount();
                Console.Clear();
                Console.WriteLine("Выберите счет для пополнения: ");
                PrintBalanseOnScreen();
                int accountId = int.Parse(Console.ReadLine());

                if (AuthorUser.Accounts.Count>=accountId)
                {
                    Console.WriteLine("Введите сумму: ");
                    decimal sum = decimal.Parse(Console.ReadLine());
                    string message = serviceAcc.IncreaseAccount(AuthorUser.Accounts,accountId, sum);
                    Console.WriteLine(message);
                }
                else
                {
                    Console.WriteLine("Выберите счет из имеющихся!");
                }
               
                Thread.Sleep(3000);
                AuthorizeUserMenu();
            }
            else if (menu == 3)
            {
                ServiceAccount serviceAcc = new ServiceAccount();
                Console.Clear();
                Console.WriteLine("Выберите счет для снятия: ");
                PrintBalanseOnScreen();
                int accountId = int.Parse(Console.ReadLine());

                if (AuthorUser.Accounts.Count >= accountId)
                {
                    Console.WriteLine("Введите сумму: ");
                    decimal sum = decimal.Parse(Console.ReadLine());
                    string message = serviceAcc.DecreaseAccount(AuthorUser.Accounts, accountId, sum);
                    Console.WriteLine(message);
                }
                else
                {
                    Console.WriteLine("Выберите счет из имеющихся!");
                }

                Thread.Sleep(3000);
                AuthorizeUserMenu();
            }
            else if (menu == 5)
            {
                MainMenu();
            }

        }

        public static void PrintBalanseOnScreen()
        {
            if (AuthorUser.Accounts.Count() > 1)
            {
                for (int i = 0; i < AuthorUser.Accounts.Count(); i++)
                {
                    Console.WriteLine("{0} На вашем счету номер {1}: {2} тенге",
                    AuthorUser.Accounts[i].Id, AuthorUser.Accounts[i].Number, AuthorUser.Accounts[i].Balance);
                }
            }
            else
            {
                Console.WriteLine("{0} На вашем счету {1}: номер {2} тенге",
                AuthorUser.Accounts[0].Id, AuthorUser.Accounts[0].Number, AuthorUser.Accounts[0].Balance);
            }
        }
        
    }
}
