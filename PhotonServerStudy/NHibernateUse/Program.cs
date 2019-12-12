using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using Sikiedu.Manager;
using Sikiedu.Model;

namespace Sikiedu
{
    class Program
    {
        static void Main(string[] args)
        {
            //var configuration = new Configuration();
            //configuration.Configure();//解析nhibernate.cfg.xml
            //configuration.AddAssembly("Sikiedu");//解析映射文件

            //ISessionFactory sessinoFactory = null;
            //ISession session = null;
            //ITransaction transaction = null;
            //try
            //{
            //    sessinoFactory = configuration.BuildSessionFactory();
            //    session = sessinoFactory.OpenSession();

            //    //User user = new User() {Username = "uuuuuu",Password = "222222"};
            //    //session.Save(user);

            //    //事物
            //    transaction = session.BeginTransaction();

            //    User user1 = new User() { Username = "eeeeee", Password = "1111111" };
            //    User user2 = new User() { Username = "uuuuuu", Password = "222222" };

            //    session.Save(user1);
            //    session.Save(user2);

            //    transaction.Commit();

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
            //finally
            //{
            //    if (transaction != null)
            //    {
            //        transaction.Dispose();
            //    }

            //    transaction = null;

            //    if (session != null)
            //    {
            //        session.Close();
            //    }

            //    session = null;

            //    if (sessinoFactory != null)
            //    {
            //        sessinoFactory.Close();
            //    }
            //    sessinoFactory = null;
            //}

            User user = new User() {Id = 11,Username = "yuyuyuyu",Password = "4444333"};
            IUserManager userManager = new UserManager();
            //userManager.Add(user);

            //User userFind = userManager.GetByName("wer2");
            //Console.WriteLine("userName = " + userFind.Username);

            //userFind.Username = userFind.Username + "yuyuyu";

            //userManager.Update(user);
            userManager.Remove(user);


            //ICollection<User> users = userManager.GetAllUsers();
            //foreach (var user in users)
            //{
            //    Console.WriteLine("userName = " + user.Username);
            //}

            //Console.WriteLine(userManager.VerifyUser("wer","wer"));


            Console.ReadKey();
        }
    }
}
