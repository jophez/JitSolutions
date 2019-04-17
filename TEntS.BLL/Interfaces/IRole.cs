using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEntS.Types;

namespace TEntS.BLL.Interfaces
{
    public interface IRole
    {
        /// <summary>
        /// Add Role
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="roleObj"></param>
        /// <param name="roleId"></param>
        void Add(ulong sessionId, RoleDetails roleObj, int currentUser, ref int roleId);
        /// <summary>
        /// Edit Role
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="roleObj"></param>
        /// <param name="currentUser"></param>
        void Edit(ulong sessionId, RoleDetails roleObj, int currentUser);
        /// <summary>
        /// Delete Role
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="roleId"></param>
        /// <param name="currentUser"></param>
        void Delete(ulong sessionId, int roleId, int currentUser);
        /// <summary>
        /// Retrieve to update
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="roleId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        RolePermission RetrieveForUpdate(ulong sessionId, int roleId, int currentUser);
        /// <summary>
        /// Retrieve role
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        RolePermission Retrieve(ulong sessionId, int roleId);
        /// <summary>
        /// Search role
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="roleName"></param>
        /// <param name="roleDescription"></param>
        /// <returns></returns>
        List<RoleDetails> Search(ulong sessionId, string roleName, string roleDescription);
        /// <summary>
        /// Assign role to user
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="userId"></param>
        /// <param name="currentUser"></param>
        /// <param name="rolesToAssign"></param>
        void AssignToUser(ulong sessionId, int userId, int currentUser, int[] rolesToAssign);
    }
}
