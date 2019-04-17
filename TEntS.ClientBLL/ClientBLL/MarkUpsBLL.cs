using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEntS.BLL.Utilities;
using TEntS.ClientBLL.Interfaces;
using TEntS.Types;
using TEntS.Types.Markups;

namespace TEntS.ClientBLL.ClientBLL
     {
     public class MarkUpsBLL : IMarkups
          {
          public bool AssignMarkupToBOM(int markupId, int bomId)
               {
               throw new NotImplementedException();
               }

          public bool Create(MarkupTypes markup, UserDetails userDetails)
               {
               try
                    {
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_AddMarkup", con))
                              {
                              cmd.Parameters.Add("@code", SqlDbType.VarChar).Value = markup.Code;
                              cmd.Parameters.Add("@markup", SqlDbType.Float).Value = markup.Markup;
                              cmd.Parameters.Add("@VAT_EWT", SqlDbType.Float).Value = markup.Vat_Ewt;
                              cmd.Parameters.Add("@Administrative", SqlDbType.Float).Value = markup.Administrative;
                              cmd.Parameters.Add("@Equipment", SqlDbType.Float).Value = markup.Equipment;
                              cmd.Parameters.Add("@Marketing", SqlDbType.Float).Value = markup.Marketing;
                              cmd.Parameters.Add("@Contingency", SqlDbType.Float).Value = markup.Contingency;
                              cmd.Parameters.Add("@Representation", SqlDbType.Float).Value = markup.Representation;
                              cmd.Parameters.Add("@DirectLabor", SqlDbType.Float).Value = markup.DirectLabor;
                              cmd.Parameters.Add("@cur_user", SqlDbType.NVarChar).Value = userDetails.Email;
                              cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                              cmd.CommandType = CommandType.StoredProcedure;

                              if (con.State != ConnectionState.Open)
                                   con.Open();

                              cmd.ExecuteNonQuery();

                              return int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) > 0 ? true : false;

                              }
                         }
                    }
               catch (Exception ex)
                    {
                    throw new TEntS.Types.Exception.TEntSException(ex.Message);
                    }
               }

          public List<MarkupTypes> RetrieveAllActiveMarkups()
               {
               var localMarkupList = new List<MarkupTypes>();
               try
                    {
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_RetrieveAllActiveMarkups", con))
                              {
                              cmd.CommandType = CommandType.StoredProcedure;

                              if (con.State != ConnectionState.Open)
                                   con.Open();

                              var da = new SqlDataAdapter(cmd);
                              var dSet = new DataSet();
                              da.Fill(dSet);

                              localMarkupList = ConvertDataSetToMarkupList(dSet);
                              }
                         }

                    return localMarkupList;
                    }
               catch (Exception ex)
                    {
                    throw new TEntS.Types.Exception.TEntSException(ex.Message);
                    }
               }

          public List<MarkupTypes> RetrieveAllMarkups()
               {
               var localMarkupList = new List<MarkupTypes>();
               try
                    {
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_RetrieveAllMarkups", con))
                              {
                              cmd.CommandType = CommandType.StoredProcedure;

                              if (con.State != ConnectionState.Open)
                                   con.Open();

                              var da = new SqlDataAdapter(cmd);
                              var dSet = new DataSet();
                              da.Fill(dSet);

                              localMarkupList = ConvertDataSetToMarkupList(dSet);
                              }
                         }

                    return localMarkupList;
                    }
               catch (Exception ex)
                    {
                    throw new TEntS.Types.Exception.TEntSException(ex.Message);
                    }
               }

          private List<MarkupTypes> ConvertDataSetToMarkupList(DataSet dSet)
               {
               var list = new List<MarkupTypes>();
               if (dSet.Tables.Count > 0)
                    {
                    foreach (DataRow row in dSet.Tables[0].Rows)
                         {
                         var markupItem = new MarkupTypes
                              {
                              Id = int.Parse(row["Id"].ToString())
                              ,
                              Administrative = float.Parse(row["Administrative"].ToString())
                              ,
                              Code = row["Code"].ToString()
                              ,
                              Contingency = float.Parse(row["Contingency"].ToString())
                              ,
                              DirectLabor = float.Parse(row["DirectLabor"].ToString())
                              ,
                              Equipment = float.Parse(row["Equipment"].ToString())
                              ,
                              Generic_1 = row["Generic_1"] != null ? (float?)float.Parse(row["Generic_1"].ToString()) : null
                              ,
                              Generic_2 = row["Generic_2"] != null ? (float?)float.Parse(row["Generic_2"].ToString()) : null
                              ,
                              Generic_3 = row["Generic_3"] != null ? (float?)float.Parse(row["Generic_3"].ToString()) : null
                              ,
                              Marketing = float.Parse(row["Marketing"].ToString())
                              ,
                              Markup = float.Parse(row["Markup"].ToString())
                              ,
                              Representation = float.Parse(row["Representation"].ToString())
                              ,
                              Vat_Ewt = float.Parse(row["Vat_Ewt"].ToString())
                              };

                         list.Add(markupItem);
                         }

                    }
               return list;
               }

          public List<MarkupTypes> RetrieveMarkupsByCode(string code)
               {
               throw new NotImplementedException();
               }

          public List<MarkupTypes> RetrieveMarkupsById(int markupId)
               {
               throw new NotImplementedException();
               }

          public bool Update(MarkupTypes markup, UserDetails userDetails)
               {
               try
                    {
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_EditMarkup", con))
                              {
                              cmd.Parameters.Add("@code", SqlDbType.VarChar).Value = markup.Code;
                              cmd.Parameters.Add("@markup", SqlDbType.Float).Value = markup.Markup;
                              cmd.Parameters.Add("@VAT_EWT", SqlDbType.Float).Value = markup.Vat_Ewt;
                              cmd.Parameters.Add("@Administrative", SqlDbType.Float).Value = markup.Administrative;
                              cmd.Parameters.Add("@Equipment", SqlDbType.Float).Value = markup.Equipment;
                              cmd.Parameters.Add("@Marketing", SqlDbType.Float).Value = markup.Marketing;
                              cmd.Parameters.Add("@Contingency", SqlDbType.Float).Value = markup.Contingency;
                              cmd.Parameters.Add("@Representation", SqlDbType.Float).Value = markup.Representation;
                              cmd.Parameters.Add("@DirectLabor", SqlDbType.Float).Value = markup.DirectLabor;
                              cmd.Parameters.Add("@cur_user", SqlDbType.NVarChar).Value = userDetails.Email;
                              cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                              cmd.CommandType = CommandType.StoredProcedure;

                              if (con.State != ConnectionState.Open)
                                   con.Open();

                              cmd.ExecuteNonQuery();

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == -1)
                                   throw new Exception("TRANSACTION_ROLLED_BACK");

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == 100)
                                   throw new Exception("RECORD_DOES_NOT_EXIST");

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == 101)
                                   throw new Exception("RECORD_IS_CURRENTLY_INACTIVE");

                              return int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) > 0 ? true : false;
                              }
                         }
                    }
               catch (Exception ex)
                    {

                    throw ex;
                    }
               }

          public bool Retire(int markupId, UserDetails userDetails)
               {
               try
                    {
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_RetireMarkup", con))
                              {
                              cmd.Parameters.Add("@markupId", SqlDbType.VarChar).Value = markupId;
                              cmd.Parameters.Add("@cur_user", SqlDbType.NVarChar).Value = userDetails.Email;
                              cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                              cmd.CommandType = CommandType.StoredProcedure;

                              if (con.State != ConnectionState.Open)
                                   con.Open();

                              cmd.ExecuteNonQuery();

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == -1)
                                   throw new Exception("TRANSACTION_ROLLED_BACK");

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == 100)
                                   throw new Exception("RECORD_DOES_NOT_EXIST");

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == 101)
                                   throw new Exception("RECORD_IS_CURRENTLY_INACTIVE");

                              return int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == 0 ? true : false;
                              }
                         }
                    }
               catch (Exception)
                    {

                    throw;
                    }
               }
          }
     }
