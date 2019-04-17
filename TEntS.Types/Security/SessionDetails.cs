using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEntS.Types.Exception;

namespace TEntS.Types
{
    /*
    APPSESSIONID		INT IDENTITY CONSTRAINT app_session_id_pk PRIMARY KEY,
    SESSIONID			BIGINT NOT NULL CONSTRAINT session_id UNIQUE,
    USERID				INT NOT NULL,
    ROLEID				INT NOT NULL,
    LOGIN_TIME			DATETIME NOT NULL,
    CLIENT_IPADDRESS	VARCHAR(32) NOT NULL
     */
    public class SessionDetails
    {
        private DateTime _login = new DateTime(1800, 01, 01);
        public ulong Id { get; set; }
        public UserDetails UserId { get; set; }
        public RoleDetails RoleId { get; set; }
        public string IpAddress { get; set; }
        public DateTime LogIn
        {
            get { return _login; }
            set
            {
                if (value.Year < 1800 || value.Year > 9999)
                    throw new TEntSInternalException("INVALID_DATE_RANGE");
                _login = value;
            }
        }

    }
}
