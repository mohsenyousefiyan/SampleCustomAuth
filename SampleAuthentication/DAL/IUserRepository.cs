using SampleAuthentication.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAuthentication.DAL
{
    public interface IUserRepository
    {
        User UserLogin(string username, string password);
        IEnumerable<UserClaims> GetUserClaims(int userid);
    }

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<UserClaims> GetUserClaims(int userid)
        {
            return dbContext.UserClaims.Where(x => x.UserId == userid).ToList();            
        }

        public User UserLogin(string username, string password)
        {
            var user = dbContext.Users.Where(x => x.UserName == username).FirstOrDefault();
            if (user != null && user.Password != password)
                user = null;

            return user;
        }
    }
}
