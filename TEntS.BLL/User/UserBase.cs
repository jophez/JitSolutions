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

namespace TEntS.BLL.User
{
    public class UserBase : IUserBase
    {
        public void Add(Types.UserDetails userObj, int currentUser, ref int userId)
        {
            try
            {
                /*
	            @username VARCHAR(32),
	            @password NVARCHAR(32),
	            @firstname VARCHAR(32),
	            @middlename VARCHAR(32),
	            @lastname VARCHAR(32),
	            @currentuser INT                
                 */
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_AddUser", con))
                    {
                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = userObj.UserName;
                        cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = SecurityToolSet.EncrypUser(userObj.Password);
                        cmd.Parameters.Add("@firstname", SqlDbType.VarChar).Value = userObj.FirsName;
                        cmd.Parameters.Add("@middlename", SqlDbType.VarChar).Value = userObj.MiddleName;
                        cmd.Parameters.Add("@lastname", SqlDbType.VarChar).Value = userObj.LastName;
                        cmd.Parameters.Add("@currentuser", SqlDbType.Int).Value = currentUser;
                        cmd.Parameters.Add("@returnVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        if (cmd.ExecuteNonQuery() <= 0)
                        {
                            if (int.Parse(cmd.Parameters["@returnVal"].Value.ToString()) == -1)
                                throw new Exception("TRANSACTION_ROLLED_BACK");
                        }

                        userId = int.Parse(cmd.Parameters["@returnVal"].Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
        }

        public void Edit(ulong sessionId, Types.UserDetails userObj, int currentUser)
        {
            try
            {
                /*
                @userid INT,
	            @firstname VARCHAR(32),
	            @middlename VARCHAR(32),
	            @lastname VARCHAR(32),
	            @sessionid BIGINT,
	            @cur_user INT,
	            @lockduration INT,
	            @lockstatus BIT OUT              
                 */
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_EditUser", con))
                    {
                        cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userObj.Id;
                        cmd.Parameters.Add("@firstname", SqlDbType.VarChar).Value = userObj.FirsName;
                        cmd.Parameters.Add("@middlename", SqlDbType.VarChar).Value = userObj.MiddleName;
                        cmd.Parameters.Add("@lastname", SqlDbType.VarChar).Value = userObj.LastName;
                        cmd.Parameters.Add("@sessionid", SqlDbType.BigInt).Value = sessionId;
                        cmd.Parameters.Add("@currentuser", SqlDbType.Int).Value = currentUser;
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
                            if (int.Parse(cmd.Parameters["@returnVal"].Value.ToString()) == 99)
                                throw new Exception("USER_DOES_NOT_EXIST");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
        }

        public Types.UserDetails RetrieveForUpdate(ulong sessionId, int userId, int currentUser)
        {
            UserDetails details = new UserDetails();
            try
            {
                /*
                @userid INT,
	            @cur_user INT,
	            @sessionid BIGINT,
	            @lockduration INT,
	            @lockstatus BIT OUT             
                 */
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_RetrieveUserForUpdate", con))
                    {
                        cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userId;
                        cmd.Parameters.Add("@sessionid", SqlDbType.BigInt).Value = sessionId;
                        cmd.Parameters.Add("@currentuser", SqlDbType.Int).Value = currentUser;
                        cmd.Parameters.Add("@lockduration", SqlDbType.Int).Value = Utility.LockDuration;
                        cmd.Parameters.Add("@lockstatus", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        if (reader.Read())
                        {
                            //u.username, ud.lastname, ud.firstname, ud.middlename
                            details.UserName = reader["username"] != DBNull.Value ? reader["username"].ToString() : string.Empty;
                            details.LastName = reader["lastname"] != DBNull.Value ? reader["lastname"].ToString() : string.Empty;
                            details.FirsName = reader["firstname"] != DBNull.Value ? reader["firstname"].ToString() : string.Empty;
                            details.MiddleName = reader["middlename"] != DBNull.Value ? reader["middlename"].ToString() : string.Empty;
                        }
                        else if ((bool)cmd.Parameters["@lockstatus"].Value)
                        {
                            throw new Exception("RECORD_LOCKED");
                        }
                        else
                        {
                            throw new Exception("INVALID_USER");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
            return details;
        }

        public Types.UserDetails Retrieve(int userId)
        {
            UserDetails details = new UserDetails();
            try
            {
                /*
                @userid INT,
	            @cur_user INT,
	            @sessionid BIGINT,
	            @lockduration INT,
	            @lockstatus BIT OUT             
                 */
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_RetrieveUserDetails", con))
                    {
                        cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userId;
                        cmd.Parameters.Add("@returnVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        if (reader.Read())
                        {
                            //u.username, ud.lastname, ud.firstname, ud.middlename
                            details.UserName = reader["username"] != DBNull.Value ? reader["username"].ToString() : string.Empty;
                            details.LastName = reader["lastname"] != DBNull.Value ? reader["lastname"].ToString() : string.Empty;
                            details.FirsName = reader["firstname"] != DBNull.Value ? reader["firstname"].ToString() : string.Empty;
                            details.MiddleName = reader["middlename"] != DBNull.Value ? reader["middlename"].ToString() : string.Empty;
                        }
                        else
                        {
                            if (int.Parse(cmd.Parameters["@returnVal"].Value.ToString()) == 100)
                                throw new Exception("USER_DOES_NOT_EXIST");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
            return details;
        }

        public void Delete(ulong sessionId, int userId, int currentUser)
        {
            try
            {
                /*
                @userid INT,
	            @cur_user INT,
	            @sessionid BIGINT,
	            @lockduration INT,
	            @lockstatus BIT OUT             
                 */
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_DeleteUser", con))
                    {
                        cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userId;
                        cmd.Parameters.Add("@sessionid", SqlDbType.BigInt).Value = sessionId;
                        cmd.Parameters.Add("@currentuser", SqlDbType.Int).Value = currentUser;
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
                            else if (int.Parse(cmd.Parameters["@returnVal"].Value.ToString()) == 99)
                                throw new Exception("ALREADY_DELETED");
                            else if (int.Parse(cmd.Parameters["@returnVal"].Value.ToString()) == 100)
                                throw new Exception("ALREADY_LOGGED_IN");
                            else if ((bool)cmd.Parameters["@lockstatus"].Value)
                                throw new Exception("RECORD_LOCKED");
                            else
                                throw new Exception("INVALID_USER");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
        }

        public List<Types.UserDetails> Search(string userName, string firstName, string lastName)
        {
            List<UserDetails> listDetails = new List<UserDetails>();
            try
            {
                /*
                @username VARCHAR(32),
	            @firstname VARCHAR(32),
	            @lastname VARCHAR(32)          
                 */
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_SearchUser", con))
                    {
                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = userName;
                        cmd.Parameters.Add("@firstname", SqlDbType.VarChar).Value = firstName;
                        cmd.Parameters.Add("@lastname", SqlDbType.VarChar).Value = lastName;
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        if (reader.HasRows)
                        {
                            //SELECT	u.userid, u.username, ud.firstname, ud.middlename, ud.lastname, u.active
                            while (reader.Read())
                            {
                                UserDetails details = new UserDetails();
                                details.Id = (Guid)reader["userid"];
                                details.UserName = reader["username"] != DBNull.Value ? reader["username"].ToString() : string.Empty;
                                details.LastName = reader["lastname"] != DBNull.Value ? reader["lastname"].ToString() : string.Empty;
                                details.FirsName = reader["firstname"] != DBNull.Value ? reader["firstname"].ToString() : string.Empty;
                                details.MiddleName = reader["middlename"] != DBNull.Value ? reader["middlename"].ToString() : string.Empty;
                                details.IsActive = reader["active"] != DBNull.Value ? (bool)reader["active"] : false;
                                listDetails.Add(details);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
            return listDetails;
        }

        public void ChangePassword(ulong sessionId, int userId, string oldPassword, string newPassword, int currentUser)
        {
            try
            {
                /*
	            @userid INT,
	            @oldpassword NVARCHAR(32),
	            @newpassword NVARCHAR(32),
	            @sessionid BIGINT,
	            @lockduration INT,
	            @cur_user INT,
	            @lockstatus BIT OUT            
                 */
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_ChangePassword", con))
                    {
                        cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userId;
                        cmd.Parameters.Add("@sessionid", SqlDbType.BigInt).Value = sessionId;
                        cmd.Parameters.Add("@currentuser", SqlDbType.Int).Value = currentUser;
                        cmd.Parameters.Add("@oldpassword", SqlDbType.NVarChar).Value = oldPassword;
                        cmd.Parameters.Add("@newpassword", SqlDbType.NVarChar).Value = newPassword;
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
                            else if ((bool)cmd.Parameters["@lockstatus"].Value)
                                throw new Exception("RECORD_LOCKED");
                            else
                                throw new Exception("INVALID_USER");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }
        }

        public string RetrieveUserPassword(string userName)
        {
            string password = string.Empty;
            try
            {
                /*
                 @username VARCHAR(32)        
                 */
                using (SqlConnection con = new SqlConnection(Utility.ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_RetrievePassword", con))
                    {
                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = userName;
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        if (reader.Read())
                        {
                            //SELECT	[password]
                            password = reader["password"] != DBNull.Value ? reader["password"].ToString() : string.Empty;
                        }
                        else
                        {
                            throw new Exception("USER_DOES_NOT_EXIST");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TEntSException(ex.Message);
            }

            return SecurityToolSet.DecryptString(password);
        }
    }
}
