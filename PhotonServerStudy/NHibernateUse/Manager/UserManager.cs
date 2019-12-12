using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sikiedu.Model;
using NHibernate;
using NHibernate.Criterion;

namespace Sikiedu.Manager
{
    class UserManager : IUserManager
    {
        public void Add(User user)
        {
            //ISession session = NHibernateHelper.OpenSession()
            //session.Save(user);
            //session.Close();

            using (ISession session = NHibernateHelper.OpenSession())
            {
                session.Save(user);
            }
        }

        public ICollection<User> GetAllUsers()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(User));
                IList<User> users = criteria.List<User>();
                return users;
            }
        }

        public User GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                User user = session.Get<User>(id);
                return user;
            }
        }

        public User GetByName(string username)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(User));
                criteria.Add(Restrictions.Eq("Username", username));
                User user = criteria.UniqueResult<User>();
                return user;
            }
        }

        public void Remove(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                session.Delete(user);
                session.Flush();
            }
        }

        public void Update(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                session.Update(user);
                session.Flush();
            }
        }

        public bool VerifyUser(string username, string password)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(User));
                criteria.Add(Restrictions.Eq("Username", username));
                criteria.Add(Restrictions.Eq("Password", password));
                User user = criteria.UniqueResult<User>();
                if (user == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
