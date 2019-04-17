using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEntS.Types.Exception;

namespace TEntS.Types
{
    public class UserDetails
    {
        private DateTime _lastLoginDate = new DateTime(1800, 01, 01);
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirsName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int FailedLogCount { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastLoginDate
        {
            get { return _lastLoginDate; }
            set
            {
                if (value.Year < 1800 || value.Year > 9999)
                    throw new TEntSInternalException("INVALID_DATE_RANGE");
                _lastLoginDate = value;
            }
        }
    }
}
