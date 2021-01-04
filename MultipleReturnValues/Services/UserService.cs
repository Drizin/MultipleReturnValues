using MultipleReturnValues.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultipleReturnValues.Services
{
    public class UserService
    {
        public enum CreateUserError
        {
            USERNAME_NOT_AVAILABLE,
            WEAK_PASSWORD,
        }

        public class CreateUserDTO
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string UserName { get; set; }
        }
        List<string> existingUsers = new List<string>();
        public CommandResult<User, CreateUserError> CreateUser(CreateUserDTO newUserInfo)
        {
            if (newUserInfo != null && existingUsers.Contains(newUserInfo.UserName, StringComparer.OrdinalIgnoreCase))
                return (null, CreateUserError.USERNAME_NOT_AVAILABLE);
            var user = new User() { FirstName = newUserInfo.FirstName, LastName = newUserInfo.LastName, UserName = newUserInfo.UserName };
            existingUsers.Add(user.UserName);
            return (user, null);
        }
    }
}
