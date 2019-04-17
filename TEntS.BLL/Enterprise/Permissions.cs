using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessSolutionFramework
{
    public class EnterprisePermission
    {
        public static bool HasPermission(long permissionMask, Permissions permission)
        {
            string binary = "";
            try
            {                
                while (permissionMask > 1)
                {
                    long remainder = 0;
                    permissionMask = Math.DivRem(permissionMask, 2, out remainder);
                    binary += remainder.ToString();
                }
                if (permissionMask == 1)
                    binary += "1";
                else
                    binary += "0";
            }
            catch (ApplicationException)
            {
                throw;
            }
            return Convert.ToBoolean(int.Parse(binary[((int)permission)].ToString()));
        }

        public static long GetPermissionMask(Permissions[] permissions)
        {
            long pMask = 0L;
            try
            {
                foreach (Permissions permission in permissions)
                {
                    pMask += long.Parse(Math.Pow(2, Convert.ToDouble((int)permission)).ToString());
                }
            }
            catch (ApplicationException)
            {
                throw;
            }
            return pMask;
        }
    }

    public enum Permissions : uint
    {        
        HasPermissionToInitialize = 0,
        HasPermissionToListManagement,
        HasPermissionToSecurity,
        HasPermissionToReports,
        HasPermissionToManageMaterials,
        HasPermissionToManageAssembly,
        HasPermissionToManageQuotation,
        HasPermissionToApproveQuotation
    }
}
