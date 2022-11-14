using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

namespace NZWalks.API.Models.Domain
{
    public class User
    {

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public List <string> Roles { get; set; }

        //Navigation 
        public List<User_Role> UserRoles { get; set; }
    }
}
