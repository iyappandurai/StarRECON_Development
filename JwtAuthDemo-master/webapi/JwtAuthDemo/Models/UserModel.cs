using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthDemo.Models
{
    public class UserModel
    {
        public string USER_NAME { get; set; }
        public string USER_PWD { get; set; }
        public string USER_MAILID { get; set; }
        public string USER_PWD_EXPIRY { get; set; }
        public string USER_GROUP { get; set; }
        public string FIRST_LOGIN_FLAG { get; set; }
        public string USER_LOGIN_RETRY { get; set; }
        public string LDAP_FLAG { get; set; }
        public string FIN_ID { get; set; }
        public DateTime LAST_LOGIN { get; set; }

        public string Token { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
