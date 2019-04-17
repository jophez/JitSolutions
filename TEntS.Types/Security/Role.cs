using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEntS.Types
{
    /*
    ROLEID		INT IDENTITY CONSTRAINT role_id_pk PRIMARY KEY,
    ROLENAME	VARCHAR(128) NOT NULL CONSTRAINT role_name_uk UNIQUE,
    ROLEDESC	VARCHAR(512) NULL,
    ACTIVE		BIT NOT NULL
     */
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
