using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEntS.Types
{
	/*
	ID		INT IDENTITY CONSTRAINT user_role_pk PRIMARY KEY,
	USERID	INT NOT NULL,
	ROLEID	INT NOT NULL
	 */
	public class UserRole
	{
		public UserDetails UserId { get; set; }
		public RoleDetails RoleId { get; set; }
	}
}
