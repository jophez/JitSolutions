using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEntS.BLL.Interfaces;
using TEntS.BLL.Utilities;
using TEntS.Types.Exception;

namespace TEntS.BLL.Role
{
    public class Role : IRole
    {
        IRoleBase _base;

        public Role(IRoleBase roleObj)
        {
            _base = roleObj;
        }

        public void Add(ulong sessionId, Types.RoleDetails roleObj, int currentUser, ref int roleId)
        {
            if (!ValidateRole(roleObj))
                throw new TEntSInternalException("INVALID_ROLE");
            //TODO: Verify Session
            try
            {
                _base.Add(roleObj, currentUser, ref roleId);
            }
            catch (TEntSInternalException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }

        public void Edit(ulong sessionId, Types.RoleDetails roleObj, int currentUser)
        {
            if (!ValidateRole(roleObj))
                throw new TEntSInternalException("INVALID_ROLE");
            //TODO: Verify Session
            try
            {
                _base.Edit(sessionId, roleObj, currentUser);
            }
            catch (TEntSInternalException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }

        public void Delete(ulong sessionId, int roleId, int currentUser)
        {
            //TODO: Verify Session
            try
            {
                _base.Delete(sessionId, roleId, currentUser);
            }
            catch (TEntSInternalException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }

        public Types.RolePermission RetrieveForUpdate(ulong sessionId, int roleId, int currentUser)
        {
            //TODO: Verify Session
            try
            {
                return _base.RetrieveForUpdate(sessionId, roleId, currentUser);
            }
            catch (TEntSInternalException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }

        public Types.RolePermission Retrieve(ulong sessionId, int roleId)
        {
            //TODO: Verify Session
            try
            {
                return _base.Retrieve(roleId);
            }
            catch (TEntSInternalException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }

        public List<Types.RoleDetails> Search(ulong sessionId, string roleName, string roleDescription)
        {
            //TODO: Verify Session
            try
            {
                return _base.Search(roleName, roleDescription);
            }
            catch (TEntSInternalException ex)
            {
                throw new TEntSException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }

        public void AssignToUser(ulong sessionId, int userId, int currentUser, int[] rolesToAssign)
        {
            throw new NotImplementedException();
        }

        private bool ValidateRole(Types.RoleDetails roleObj)
        {
            bool isValidObj = false;
            if (Utility.ValidateInput(roleObj.Name) && Utility.ValidateInput(roleObj.Description))
                isValidObj = true;
            return isValidObj;
        }
    }
}
