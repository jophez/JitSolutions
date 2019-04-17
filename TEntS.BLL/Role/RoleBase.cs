using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TEntS.BLL.Interfaces;
using TEntS.BLL.Utilities;
using TEntS.Types;
using TEntS.Types.Exception;
using TEntS.Types.Security;

namespace TEntS.BLL.Role
{
    public class RoleBase : IRoleBase
    {
        public void Add(Types.RoleDetails roleObj, int currentUser, ref int roleId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_AddRole", con))
                    {
                        //	@rolename VARCHAR(32),@description VARCHAR(128),@cur_user INT
                        cmd.Parameters.Add("@rolename", SqlDbType.VarChar).Value = roleObj.Name;
                        cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = roleObj.Description;
                        cmd.Parameters.Add("@cur_user", SqlDbType.Int).Value = currentUser;
                        cmd.Parameters.Add("@returnVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        if (cmd.ExecuteNonQuery() <= 0)
                        {
                            if (int.Parse(cmd.Parameters["@returnVal"].Value.ToString()) == -1)
                                throw new Exception("TRANSACTION_ROLLED_BACK");
                        }

                        roleId = int.Parse(cmd.Parameters["@returnVal"].Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }

        public void Edit(ulong sessionId, Types.RoleDetails roleObj, int currentUser)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_AddRole", con))
                    {
                        //		@roleid INT,@rolename VARCHAR(32),@description VARCHAR(128),@sessionid BIGINT,@lockduration INT,@cur_user INT,@lockstatus BIT OUT
                        cmd.Parameters.Add("@roleid", SqlDbType.Int).Value = roleObj.Id;
                        cmd.Parameters.Add("@rolename", SqlDbType.VarChar).Value = roleObj.Name;
                        cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = roleObj.Description;
                        cmd.Parameters.Add("@sessionid", SqlDbType.BigInt).Value = sessionId;
                        cmd.Parameters.Add("@cur_user", SqlDbType.Int).Value = currentUser;
                        cmd.Parameters.Add("@lockduration", SqlDbType.Int).Value = Utility.LockDuration;
                        cmd.Parameters.Add("@lockstatus", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@returnVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        if (cmd.ExecuteNonQuery() <= 0)
                        {
                            if (int.Parse(cmd.Parameters["@returnVal"].Value.ToString()) == -1)
                                throw new Exception("TRANSACTION_ROLLED_BACK");
                            if (int.Parse(cmd.Parameters["@returnVal"].Value.ToString()) == 100)
                                throw new Exception("ROLE_DOES_NOT_EXIST");
                            if ((bool)cmd.Parameters["@lockstatus"].Value)
                                throw new Exception("ROLE_LOCKED");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }

        public void Delete(ulong sessionId, int roleId, int currentUser)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_DeleteRole", con))
                    {
                        /*
                        @roleid INT,
	                    @sessionid BIGINT,
	                    @lockduration INT,
	                    @cur_user INT,
	                    @lockstatus BIT OUT
                         */
                        cmd.Parameters.Add("@roleid", SqlDbType.Int).Value = roleId;
                        cmd.Parameters.Add("@sessionid", SqlDbType.BigInt).Value = sessionId;
                        cmd.Parameters.Add("@cur_user", SqlDbType.Int).Value = currentUser;
                        cmd.Parameters.Add("@lockduration", SqlDbType.Int).Value = Utility.LockDuration;
                        cmd.Parameters.Add("@lockstatus", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@returnVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        if (cmd.ExecuteNonQuery() <= 0)
                        {
                            if (int.Parse(cmd.Parameters["@returnVal"].Value.ToString()) == -1)
                                throw new Exception("TRANSACTION_ROLLED_BACK");
                            if (int.Parse(cmd.Parameters["@returnVal"].Value.ToString()) == 100)
                                throw new Exception("ROLE_DOES_NOT_EXIST"); //RETURN 100 -- roleid does not exist
                            if ((bool)cmd.Parameters["@lockstatus"].Value)
                                throw new Exception("ROLE_LOCKED");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
        }

        public Types.RolePermission RetrieveForUpdate(ulong sessionId, int roleId, int currentUser)
        {
            RolePermission rolePermission = new RolePermission();
            try
            {
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_RetrieveRoleForUpdate", con))
                    {
                        /*
                        @roleid INT,
	                    @sessionid BIGINT,
	                    @lockduration INT,
	                    @cur_user INT,
	                    @lockstatus BIT OUT
                         */
                        cmd.Parameters.Add("@roleid", SqlDbType.Int).Value = roleId;
                        cmd.Parameters.Add("@sessionid", SqlDbType.BigInt).Value = sessionId;
                        cmd.Parameters.Add("@cur_user", SqlDbType.Int).Value = currentUser;
                        cmd.Parameters.Add("@lockduration", SqlDbType.Int).Value = Utility.LockDuration;
                        cmd.Parameters.Add("@lockstatus", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet dset = new DataSet();
                        da.Fill(dset);

                        if (dset.Tables.Count > 0)
                        {
                            rolePermission = ConvertToObject(dset);
                        }
                        else if ((bool)cmd.Parameters["@lockstatus"].Value)
                            throw new Exception("RECORD_LOCKED");
                        else
                            throw new Exception("INVALID_ROLE");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
            return rolePermission;
        }

        public RolePermission Retrieve(int roleId)
        {
            RolePermission rolePermission = new RolePermission();
            try
            {
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_RetrieveRole", con))
                    {
                        /*
                        @roleid INT,
                         */
                        cmd.Parameters.Add("@roleid", SqlDbType.Int).Value = roleId;
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet dset = new DataSet();
                        da.Fill(dset);

                        rolePermission = ConvertToObject(dset);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
            return rolePermission;
        }

        public List<RoleDetails> Search(string roleName, string roleDescription)
        {
            List<RoleDetails> roleList = new List<RoleDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_SearchRole", con))
                    {
                        /*
                        @rolename VARCHAR(32),
	                    @roledesc VARCHAR(128)
                         */
                        cmd.Parameters.Add("@rolename", SqlDbType.VarChar).Value = roleName;
                        cmd.Parameters.Add("@roledesc", SqlDbType.VarChar).Value = roleDescription;
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet dset = new DataSet();
                        da.Fill(dset);

                        roleList = ConvertToRoleObject(dset);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSInternalException(ex.Message);
            }
            return roleList;
        }       

        #region Private Methods
        private RolePermission ConvertToObject(DataSet dset)
        {
            RolePermission rolePermission = new RolePermission();
            int roleId = 0;
            if (dset.Tables.Count > 0)
            {
                foreach (DataRow dRow in dset.Tables[0].Rows)
                {
                    //roleid, rolename, roledesc
                    RoleDetails rDetail = new RoleDetails();
                    rDetail.Id = int.Parse(dRow["roleid"].ToString());
                    rDetail.Name = dRow["rolename"] != DBNull.Value ? dRow["rolename"].ToString() : string.Empty;
                    rDetail.Description = dRow["roledesc"] != DBNull.Value ? dRow["roledesc"].ToString() : string.Empty;
                    roleId = rDetail.Id;
                    rolePermission.RoleList.Add(rDetail);
                }
                if (dset.Tables.Count > 1)
                {
                    foreach (DataRow dRow in dset.Tables[1].Select("roleid = " + roleId))
                    {
                        //r.roleid, rp.seqno, rp.pmask
                        rolePermission.Id = int.Parse(dRow["roleid"].ToString());
                        rolePermission.SequenceNo = int.Parse(dRow["seqno"].ToString());
                        rolePermission.PermissionMask = ulong.Parse(dRow["pmask"].ToString());
                    }
                }
            }
            return rolePermission;
        }
        private List<RoleDetails> ConvertToRoleObject(DataSet dset)
        {
            List<RoleDetails> rDetails = new List<RoleDetails>();
            if (dset.Tables.Count > 0)
            {
                foreach (DataRow dRow in dset.Tables[0].Rows)
                {
                    //roleid, rolename, roledesc
                    RoleDetails detail = new RoleDetails();
                    detail.Id = int.Parse(dRow["roleid"].ToString());
                    detail.Name = dRow["rolename"] != DBNull.Value ? dRow["rolename"].ToString() : string.Empty;
                    detail.Description = dRow["roledesc"] != DBNull.Value ? dRow["roledesc"].ToString() : string.Empty;
                    rDetails.Add(detail);
                }
            }
            return rDetails;
        }
        #endregion
    }
}
