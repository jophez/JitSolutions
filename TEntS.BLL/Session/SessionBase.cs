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

namespace TEntS.BLL.Session
{
    public class SessionBase : ISessionBase
    {
        public bool Login(string userName, string passWord, ref string message)
        {
            try
            {
                //Authenticate the User
                return AuthenticateUser(userName, passWord, ref message);
            }
            catch (TEntSException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
        }

        public bool SetFailedLogCount(string userName)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_SetFailedLogCount", con))
                    {
                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = userName;
                        cmd.Parameters.Add("@maxfailedlogcount", SqlDbType.Int).Value = Utility.FailedLogCount;
                        cmd.Parameters.Add("@disabled", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@returnVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (con.State != ConnectionState.Open)
                            con.Open();

                        if (cmd.ExecuteNonQuery() <= 0)
                        {
                            if (int.Parse(cmd.Parameters["@returnVal"].Value.ToString()) == -1)
                                throw new Exception("TRANSACTION_ROLLED_BACK");
                        }

                        return ((bool)cmd.Parameters["@disabled"].Value);
                    }
                }
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
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_ResetFailedLogCount", con))
                    {
                        cmd.Parameters.Add("@username", SqlDbType.VarChar);
                        cmd.Parameters.Add("@returnVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (con.State != ConnectionState.Open)
                            con.Open();

                        if (cmd.ExecuteNonQuery() <= 0)
                        {
                            if (int.Parse(cmd.Parameters["@returnVal"].Value.ToString()) == -1)
                                throw new Exception("TRANSACTION_ROLLED_BACK");
                        }
                    }
                }
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
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_EnableDisableUser", con))
                    {
                        /*
                        @userid INT,
                        @enabled BIT,
                        @sessionid BIGINT,
                        @lockduration INT,
                        @cur_user INT,
                        @lockstatus BIT OUT
                         */

                        cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userId;
                        cmd.Parameters.Add("@enabled", SqlDbType.Bit).Value = 1;
                        cmd.Parameters.Add("@sessionid", SqlDbType.BigInt).Value = sessionId;
                        cmd.Parameters.Add("@lockduration", SqlDbType.Int).Value = Utility.LockDuration;
                        cmd.Parameters.Add("@cur_user", SqlDbType.Int).Value = currentUser;
                        cmd.Parameters.Add("@lockstatus", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@returnVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (con.State != ConnectionState.Open)
                            con.Open();

                        if (cmd.ExecuteNonQuery() <= 0)
                        {
                            if (int.Parse(cmd.Parameters["@returnVal"].Value.ToString()) == -1)
                                throw new Exception("TRANSACTION_ROLLED_BACK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
        }

        public void DisconnectUser(ulong sessionId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_RemoveApplicationSession", con))
                    {
                        cmd.Parameters.Add("@sessionid", SqlDbType.BigInt).Value = sessionId;
                        cmd.Parameters.Add("@returnVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        if (cmd.ExecuteNonQuery() <= 0)
                        {
                            if (int.Parse(cmd.Parameters["@returnVal"].ToString()) == -1)
                                throw new Exception("TRANSACTION_ROLLED_BACK");

                        }
                       // return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
        }

        public UserRole IsUserCurrentlyLoggedOn(ulong sessionId)
        {
            UserRole retrievedUser = new UserRole();
            try
            {
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_IsUserCurrentlyLogin", con))
                    {
                        cmd.Parameters.Add("@sessionid", SqlDbType.BigInt).Value = sessionId;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                //	SELECT	userid, roleid, login_time
                                retrievedUser = new UserRole();
                                retrievedUser.UserId.Id = (Guid)reader["userid"];
                                retrievedUser.RoleId.Id = int.Parse(reader["roleid"].ToString());
                                retrievedUser.UserId.LastLoginDate = DateTime.Parse(reader["login_time"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }

            return retrievedUser;
        }

        public void Add(ulong sessionId, string userName, string ipAddress)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_AddApplicationSession", con))
                    {
                        /*
                        	@username INT,
	                        @sessionid BIGINT,
	                        @client_ipaddress VARCHAR(32)
                         */
                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = userName;
                        cmd.Parameters.Add("@sessionid", SqlDbType.BigInt).Value = sessionId;
                        cmd.Parameters.Add("@client_ipaddress", SqlDbType.VarChar).Value = ipAddress;
                        cmd.Parameters.Add("@returnVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        if (cmd.ExecuteNonQuery() <= 0)
                        {
                            if (int.Parse(cmd.Parameters["@returnVal"].Value.ToString()) == -1)
                                throw new Exception("TRANSACTION_ROLLED_BACK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
        }

        #region Private Methods
        private bool AuthenticateUser(string userName, string passWord, ref string message)
        {
            bool isAuthenticated = true;
            try
            {
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_AuthenticateUser", con))
                    {
                        /*
                            @username VARCHAR(32),
                            @password NVARCHAR(32)
                         */
                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = userName;
                        cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = passWord;
                        cmd.Parameters.Add("@returnVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        if (cmd.ExecuteNonQuery() <= 0)
                        {
                            switch (int.Parse(cmd.Parameters["@returnVal"].Value.ToString()))
                            {
                                case -1: { message = "TRANSACTION_ROLLED_BACK"; isAuthenticated = false; } break;
                                case 100: { message = "USER_CURRENTLY_DISABLED"; isAuthenticated = false; } break;
                                case 101: { message = "INVALID_USERNAME_OR_PASSWORD"; isAuthenticated = false; } break;
                            }
                        }
                        return isAuthenticated;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
        }
        #endregion
    }
}
