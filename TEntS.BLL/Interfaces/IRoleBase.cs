using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEntS.Types;

namespace TEntS.BLL.Interfaces
{
    public interface IRoleBase
    {
        /// <summary>
        /// Add role
        /// </summary>
        /// <param name="roleObj"></param>
        /// <param name="currentUser"></param>
        /// <param name="roleId"></param>
        void Add(RoleDetails roleObj, int currentUser, ref int roleId);
        /// <summary>
        /// Edit role
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="roleObj"></param>
        /// <param name="currentUser"></param>
        void Edit(ulong sessionId, Types.RoleDetails roleObj, int currentUser);
        /// <summary>
        /// Delete role
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
        /// <param name="roleId"></param>
        /// <returns></returns>
        RolePermission Retrieve(int roleId);
        /// <summary>
        /// Search role
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="roleDescription"></param>
        /// <returns></returns>
        List<RoleDetails> Search(string roleName, string roleDescription);
    }
}
