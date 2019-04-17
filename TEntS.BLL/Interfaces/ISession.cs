using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEntS.Types;

namespace TEntS.BLL.Interfaces
{
    public interface ISession
    {
        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <param name="ipAddress"></param>
        /// <param name="forceLogout"></param>
        /// <param name="sessionId"></param>
        void Login(string userName, string passWord, string ipAddress, bool forceLogout, ref ulong sessionId);
        /// <summary>
        /// Disconnect User
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userName"></param>
        void DisconnectUser(string userName);
        /// <summary>
        /// Validate User Who is Currently Logged On
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        bool IsUserCurrentlyLoggedOn(string userName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        UserRole IsUserCurrentlyLoggedOn(ulong sessionId);
        /// <summary>
        /// Enable a disabled user
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="userId"></param>
        /// <param name="currentUser"></param>
        void EnableDisabledUser(ulong sessionId, int userId, int currentUser);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        void ResetFailedLogCount(string userName);
        /// <summary>
        /// Add session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="userName"></param>
        /// <param name="ipAddress"></param>
        void Add(ulong sessionId, string userName, string ipAddress);
    }
}
