using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class StaticUserRepository : IUserRepository
    {
        private readonly IUserRepository userRepository;

        private List<User> Users = new List<User>()
        {
            /*
            new User()
            {
                FirstName="Read Only",
                LastName="User",
                EmailAddress="readonly@user.com",
                Id=Guid.NewGuid(),
                UserName="readonly@user.com",
                Password="readonly@user",
                Roles= new List<string> {"reader"}
            },
            new User()
            {
                FirstName="Read Write",
                LastName="User",
                EmailAddress="readwrite@user.com",
                Id=Guid.NewGuid(),
                UserName="readwrite@user.com",
                Password="readwrite@user",
                Roles= new List<string> {"reader","writer"}
            }
            */
        };

        public async Task<User> AuthenticateUser(string UserName, string Password)
        {
            var user = Users.Find(x => x.UserName.Equals(UserName, StringComparison.InvariantCultureIgnoreCase) && x.Password == Password);
            if (user != null)
            {
                return user;
            }
            return user;
        }
    }
}
