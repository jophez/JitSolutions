using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEntS.Types;

namespace TEntS.BLL.Interfaces
{
    public interface ISessionBase
    {
        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <param name="message"></param>
        /// <param name="forceLogout"></param>
        /// <param name="sessionId"></param>
        bool Login(string userName, string passWord, ref string message);
        /// <summary>
        /// Disconnect User
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="userName"></param>
        void DisconnectUser(ulong sessionId);
        /// <summary>
        /// Validate User Who is Currently Logged On
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        UserRole IsUserCurrentlyLoggedOn(ulong sessionId);
        /// <summary>
        /// Enable the disabled user
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
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool SetFailedLogCount(string userName);
        /// <summary>
        /// Add session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="userName"></param>
        /// <param name="ipAddress"></param>
        void Add(ulong sessionId, string userName, string ipAddress);
    }
}
