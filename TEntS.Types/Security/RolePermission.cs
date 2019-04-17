using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEntS.Types.Security;

namespace TEntS.Types
{
    /*
    ID		INT IDENTITY CONSTRAINT role_perm_id_pk PRIMARY KEY,
    ROLEID	INT NOT NULL,
    PERMID	INT NOT NULL
     */
    public class RolePermission
    {
        public int Id { get; set; }
        public int ComponentId { get; set; }
        public int SequenceNo { get; set; }
        public ulong PermissionMask { get; set; }
        public bool IsActive { get; set; }
        public List<RoleDetails> RoleList { get; set; }
    }
}
