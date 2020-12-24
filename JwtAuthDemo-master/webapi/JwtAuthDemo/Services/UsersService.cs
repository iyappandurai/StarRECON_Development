using System.Collections.Generic;
using System.Data;
using JwtAuthDemo.Models;
using Microsoft.Extensions.Logging;

namespace JwtAuthDemo.Services
{
    public interface IUserService
    {
        bool IsAnExistingUser(string userName);
        UserModel IsValidUserCredentials(string userName, string password,string financial);
        string GetUserRole(string userName);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IDapper _dapper;


        private readonly IDictionary<string, string> _users = new Dictionary<string, string>
        {
            { "test1", "password1" },
            { "test2", "password2" },
            { "admin", "securePassword" }
        };
        // inject your database here for user validation
        public UserService(ILogger<UserService> logger, IDapper dapper)
        {
            _logger = logger;
            _dapper = dapper;
        }

        public UserModel IsValidUserCredentials(string userName, string password,string financila)
        {
            _logger.LogInformation($"Validating user [{userName}]");
            EncryptDecrypt.EncryptDecrypt ed = new EncryptDecrypt.EncryptDecrypt();
            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            //return _users.TryGetValue(userName, out var p) && p == password;
            var result = _dapper.Get<UserModel>($"SELECT DISTINCT USER_NAME,USER_PWD,USER_MAILID,USER_PWD_EXPIRY,USER_GROUP,FIRST_LOGIN_FLAG, USER_LOGIN_RETRY, LDAP_FLAG, B.FIN_ID, (SELECT MAX(USER_LOGIN_TIME) FROM RECON_USER_LOG_INFORMATION WHERE USER_ID = A.USER_ID) AS LAST_LOGIN FROM RECON_USER_INFORMATION A INNER JOIN RECON_USER_FIN B ON A.USER_ID = B.USER_ID WHERE A.USER_NAME = '" + userName + "'", null, commandType: CommandType.Text);

            if (result == null)
                return null;
            if (result.USER_PWD != ed.EncryptText(password))
            {
                return null;
            }
            result.USER_PWD = "";
            return result;
        }

        public bool IsAnExistingUser(string userName)
        {
            return _users.ContainsKey(userName);
        }

        public string GetUserRole(string userName)
        {
            if (!IsAnExistingUser(userName))
            {
                return string.Empty;
            }

            if (userName == "admin")
            {
                return UserRoles.Admin;
            }

            return UserRoles.BasicUser;
        }
    }

    public static class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string BasicUser = nameof(BasicUser);
    }
}
