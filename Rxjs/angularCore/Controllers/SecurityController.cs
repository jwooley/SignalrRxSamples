using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angularCore.Controllers
{
    public class SecurityController : Controller
    {
        [Route("api/Security/UserPermissions")]
        public async Task<IEnumerable<string>> UserPermissions()
        {
            await Task.Delay(2000);
            // Would come from a datastore or credentials. Sending hardcoded values for demo purposes
            return new string[]
            {
                "CanEditUser",
                "CanAddUser",
                "CanDoSomethingElse"
            };
        }

        [Route("api/Security/User/{id}")]
        public UserInfo GetUser(int id)
        {
            return new UserInfo
            {
                UserId = id,
                FirstName = "Jim",
                LastName = "Wooley",
                Password = "Supersecret",
                StatusId = 1
            };
        }

        [Route("api/Security/StatusCodes")]
        public IEnumerable<Lookup> StatusCodes()
        {
            return new Lookup[]
            {
                new Lookup{ Code = 1, Description = "Active"},
                new Lookup{ Code = 99, Description = "Terminated"}
            };
        }
    }

    public class Lookup
    {
        public int Code { get; set; }
        public string Description { get; set; }
    }

    public class UserInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public int StatusId { get; set; }
        public int UserId { get; internal set; }
    }
}
