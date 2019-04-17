using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEntS.Types;

namespace TEntS.BLL.Interfaces
{
    public interface IUserBase
    {
        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="userObj"></param>
        /// <param name="currentUser"></param>
        /// <param name="userId"></param>
        void Add(UserDetails userObj, int currentUser, ref int userId);
        /// <summary>
        /// Edit user
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="userObj"></param>
        /// <param name="currentUser"></param>
        void Edit(ulong sessionId, UserDetails userObj, int currentUser);
        /// <summary>
        /// Retrieve for update
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="userId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        UserDetails RetrieveForUpdate(ulong sessionId, int userId, int currentUser);
        /// <summary>
        /// Retrieve user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserDetails Retrieve(int userId);
        /// <summary>
        /// Delete user 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="userId"></param>
        /// <param name="currentUser"></param>
        void Delete(ulong sessionId, int userId, int currentUser);
        /// <summary>
        /// Search user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        List<UserDetails> Search(string userName, string firstName, string lastName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="userId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="currentUser"></param>
        void ChangePassword(ulong sessionId, int userId, string oldPassword, string newPassword, int currentUser);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        string RetrieveUserPassword(string userName);
    }
}
