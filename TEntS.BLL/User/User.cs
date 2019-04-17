using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEntS.BLL.Interfaces;
using TEntS.BLL.Utilities;
using TEntS.Types.Exception;

namespace TEntS.BLL.User
{
    public class User : MarshalByRefObject, IUser
    {
        IUserBase _base;

        public User(IUserBase userObj)
        {
            _base = userObj;
        }

        public void Add(ulong sessionId, Types.UserDetails userObj, int currentUser, ref int userId)
        {
            if (!ValidateUserObj(userObj))
                throw new TEntSInvalidUserException("INVALID_USER_DETAIL");

            try
            {
                //TODO : Validate Session
                _base.Add(userObj, currentUser, ref userId);
            }
            catch (TEntSInvalidUserException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (TEntSException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }


        public void Edit(ulong sessionId, Types.UserDetails userObj, int currentUser)
        {
            try
            {
                //TODO : Validate Session
                _base.Edit(sessionId, userObj, currentUser);
            }
            catch (TEntSException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }

        public Types.UserDetails RetrieveForUpdate(ulong sessionId, int userId, int currentUser)
        {
            try
            {
                //TODO : Validate Session
                return _base.RetrieveForUpdate(sessionId, userId, currentUser);
            }
            catch (TEntSException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }

        public Types.UserDetails Retrieve(ulong sessionId, int userId)
        {
            try
            {
                //TODO : Validate Session
                return _base.Retrieve(userId);
            }
            catch (TEntSException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }

        public void Delete(ulong sessionId, int userId, int currentUser)
        {
            try
            {
                //TODO : Validate Session
                _base.Delete(sessionId, userId, currentUser);
            }
            catch (TEntSException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }

        public List<Types.UserDetails> Search(ulong sessionId, string userName, string firstName, string lastName)
        {
            try
            {
                userName = userName.Replace("[", "\\[").Replace("]", "\\]").Replace("_", "\\_").Replace("%", "\\%");
                firstName = firstName.Replace("[", "\\[").Replace("]", "\\]").Replace("_", "\\_").Replace("%", "\\%");
                lastName = lastName.Replace("[", "\\[").Replace("]", "\\]").Replace("_", "\\_").Replace("%", "\\%");
                //TODO : Validate Session
                return _base.Search(userName, firstName, lastName);
            }
            catch (TEntSException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }

        public void ChangePassword(ulong sessionId, int userId, string oldPassword, string newPassword, int currentUser)
        {
            try
            {
                //TODO : Validate Session
                _base.ChangePassword(sessionId, userId, oldPassword, newPassword, currentUser);
            }
            catch (TEntSException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }

        public string RetrieveUserPassword(ulong sessionId, string userName)
        {
            try
            {
                //TODO : Validate Session
                return _base.RetrieveUserPassword(userName);
            }
            catch (TEntSException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }

        private bool ValidateUserObj(Types.UserDetails userObj)
        {
            bool isValidated = false;
            if (Utility.ValidateInput(userObj.UserName) && Utility.ValidateInput(userObj.FirsName) && Utility.ValidateInput(userObj.LastName) && Utility.ValidateInput(userObj.MiddleName))
                isValidated = true;
            return isValidated;
        }

    }
}
