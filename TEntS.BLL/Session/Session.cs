using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEntS.BLL.Interfaces;
using TEntS.BLL.Utilities;
using TEntS.Types;
using TEntS.Types.Exception;

namespace TEntS.BLL.Session
{
    public class Session : MarshalByRefObject, ISession
    {
        ISessionBase _base;
        private string _lockHash = string.Empty;
        private Utility SessionData = new Utility();

        public Session(ISessionBase sessionBase)
        {
            _base = sessionBase;
        }

        public void Login(string userName, string passWord, string ipAddress, bool forceLogout, ref ulong sessionId)
        {
            if (!(Utility.ValidateInput(userName)))
                throw new TEntSInvalidUserException("INVALID_USERNAME_OR_PASSWORD");

            try
            {
                string message = string.Empty;
                //Authenticate the user
                if (!_base.Login(userName, SecurityToolSet.EncrypUser(passWord), ref message))
                {
                    if (_base.SetFailedLogCount(userName))
                        throw new TEntSInvalidUserException("USER_DISABLED");
                    else
                        throw new TEntSInvalidUserException(message);
                }
                //Create the session detail object
                SessionDetails details = new SessionDetails() { IpAddress = ipAddress, LogIn = DateTime.Now, UserId = new UserDetails() { UserName = userName, LastLoginDate = DateTime.Now, Password = SecurityToolSet.EncrypUser(passWord) } };
                //once authenticated create a session for the user
                CreateSession(details, forceLogout, ref sessionId);
            }
            catch (TEntSInvalidUserException ex)
            {
                throw new TEntSInvalidUserException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
        }

        public void DisconnectUser(string userName)
        {
            try
            {
                //lock the operation
                lock (_lockHash)
                {
                    //retrieve all user sessions first
                    ArrayList userSessions = (ArrayList)SessionData._UserHashSessions[userName];//list of sessions for the current user
                    //remove all entries of user's sessions in the sessions hashtable
                    if (userSessions != null)
                    {
                        foreach (ulong sessionId in userSessions)
                        {
                            //remove all records locked by this user
                            _base.DisconnectUser(sessionId);

                            SessionData.SessionData.Remove(sessionId);
                        }
                        //remove the user in the usersessions hashtable
                        SessionData._UserHashSessions.Remove(userName);
                    }
                }
            }
            catch (TEntSInvalidUserException ex)
            {
                throw new TEntSInvalidUserException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
        }

        public bool IsUserCurrentlyLoggedOn(string userName)
        {
            bool loggedOn = false;
            ArrayList invalidSessions = new ArrayList();
            //if user is logged in
            try
            {
                if (SessionData._UserHashSessions.ContainsKey(userName))
                {
                    foreach (ulong sessionID in (ArrayList)SessionData._UserHashSessions[userName])
                    {
                        SessionDetails sessData = (SessionDetails)SessionData._UserHashSessions[sessionID];
                        DateTime lastAccessTime = sessData.LogIn;
                        //if session is expired
                        if (lastAccessTime.AddMinutes((double)Utility.TimeOut) < DateTime.UtcNow)
                        {
                            //add this session to the list of invalid sessions
                            invalidSessions.Add(sessionID);
                        }
                        //user's session is valid
                        else
                            return true;
                    }
                }
            }
            finally
            {
                //delete all invalid sessions
                foreach (ulong sessionID in invalidSessions)
                {
                    DeleteSession(sessionID);
                }
            }
            return loggedOn;
        }

        public UserRole IsUserCurrentlyLoggedOn(ulong sessionId)
        {
            try
            {
                return _base.IsUserCurrentlyLoggedOn(sessionId);
            }
            catch (TEntSInvalidUserException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
        }

        public void EnableDisabledUser(ulong sessionId, int userId, int currentUser)
        {
            try
            {
                _base.EnableDisabledUser(sessionId, userId, currentUser);
            }
            catch (TEntSInvalidUserException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
        }

        public void ResetFailedLogCount(string userName)
        {
            try
            {
                _base.ResetFailedLogCount(userName);
            }
            catch (TEntSInvalidUserException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
        }

        public void Add(ulong sessionId, string userName, string ipAddress)
        {
            try
            {
                _base.Add(sessionId, userName, ipAddress);
            }
            catch (TEntSInvalidUserException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
        }

        #region Private Methods
        private void CreateSession(SessionDetails detailObj, bool forceLogout, ref ulong sessionId)
        {
            ulong tmpSession = 0;
            lock (_lockHash)
            {
                if (SessionData._UserHashSessions.ContainsKey(detailObj.UserId.UserName))
                {
                    if (forceLogout)
                        _base.DisconnectUser(sessionId);
                }
                else
                {
                    if (!IsUserCurrentlyLoggedOn(detailObj.UserId.UserName))
                    {
                        do
                        {
                            tmpSession = Utility.GenerateSessionId();//Generate a random session
                        }
                        while (SessionData.SessionData.ContainsKey(sessionId)); //there should be a retrieval for the session
                        //save the session
                        sessionId = tmpSession;
                        //Insert the session to the hash table
                        InsertSessionToHash(sessionId, detailObj);
                        Add(sessionId, detailObj.UserId.UserName, detailObj.IpAddress); //Add to db
                    }
                    else
                        throw new TEntSInvalidUserException("USER_ALREADY_LOGGED_IN");
                }
            }
        }

        private void InsertSessionToHash(ulong newSessionId, SessionDetails sessionDetails)
        {
            Hashtable tble = new Hashtable();
            lock (_lockHash)
            {
                if (SessionData._UserHashSessions.ContainsKey(sessionDetails.UserId.UserName))
                {
                    ((ArrayList)SessionData._UserHashSessions[sessionDetails.UserId.UserName]).Add(newSessionId);
                }
                else
                {
                    ArrayList items = new ArrayList();
                    items.Add(newSessionId);
                    SessionData._UserHashSessions.Add(sessionDetails.UserId.UserName, items);
                }
                SessionData.SessionData.Add(newSessionId, sessionDetails);
            }
        }

        private SessionDetails ConvertToSessionData(string userName, string ipAddress, ulong sessionId)
        {
            SessionDetails detail = new SessionDetails() { Id = sessionId, IpAddress = ipAddress, UserId = new UserDetails() { UserName = userName }, LogIn = DateTime.Now };
            return detail;
        }

        private void DeleteSession(ulong sessionId)
        {
            //get session data first to retrieve the username of the session to be deleted
            SessionDetails data;//session data for the current session
            ArrayList userSessions = new ArrayList();//list of sessions for the current user
            //lock the operation
            lock (_lockHash)
            {
                if (SessionData.SessionData.ContainsKey(sessionId))
                {
                    //remove all records locked by this user
                    _base.DisconnectUser(sessionId);

                    data = (SessionDetails)SessionData.SessionData[sessionId];
                    //get all user's session
                    userSessions = (ArrayList)SessionData._UserHashSessions[data.UserId.UserName];
                    //remove the entry of this sessionID at the second hashtable(table of user sessions) first
                    userSessions.Remove(sessionId);
                    //if there are no more session's in the list remove the user from the hashtable and remove the IP address entry for the current user
                    if (userSessions.Count < 1)
                    {
                        SessionData._UserHashSessions.Remove(data.UserId.UserName);
                    }

                    //now remove the session on the first table(table of all existing sessions)
                    SessionData.SessionData.Remove(sessionId);
                }
            }
        }
        #endregion
    }
}
