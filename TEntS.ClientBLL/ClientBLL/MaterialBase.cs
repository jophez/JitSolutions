using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TEntS.BLL.Utilities;
using TEntS.ClientBLL.Interfaces;
using TEntS.Types;
using TEntS.Types.Materials;

namespace TEntS.ClientBLL.ClientBLL
     {
     public class MaterialBase : IMaterialBase
          {
          public MaterialBase()
               {
               }

          public bool Activate(int materialId, UserDetails userDetails)
               {
               try
                    {
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_ActivateMaterial", con))
                              {
                              cmd.Parameters.Add("@materialId", SqlDbType.Int).Value = materialId;
                              cmd.Parameters.Add("@cur_user", SqlDbType.NVarChar).Value = userDetails.Email;
                              cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                              cmd.CommandType = CommandType.StoredProcedure;
                              if (con.State != ConnectionState.Open)
                                   con.Open();
                              cmd.ExecuteNonQuery();

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == -1)
                                   throw new Exception("TRANSACTION_ROLLED_BACK");

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == 99)
                                   throw new Exception("RECORD_DOES_NOT_EXIST");

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == 100)
                                   throw new Exception("RECORD_ALREADY_ACTIVATED");

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == 101)
                                   throw new Exception("RECORD_CURRENTLY_RETIRED");

                              return int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) > 0 ? true : false;
                              }
                         }
                    }
               catch (Exception ex)
                    {
                    throw new ApplicationException(ex.Message);
                    }
               }

          public bool Create(Types.Materials.Material materialObj, UserDetails userDetails)
               {
               /*
                * 	@code VARCHAR(10),
    @unitPrice FLOAT,
    @description VARCHAR(255),
    @cur_user NVARCHAR(128)

                */
               try
                    {
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_AddMaterial", con))
                              {
                              cmd.Parameters.Add("@code", SqlDbType.VarChar).Value = materialObj.Code;
                              cmd.Parameters.Add("@unitPrice", SqlDbType.Float).Value = materialObj.Cost.BasePrice;
                              cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = materialObj.Description;
                              cmd.Parameters.Add("@cur_user", SqlDbType.NVarChar).Value = userDetails.Email;
                              cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                              cmd.CommandType = CommandType.StoredProcedure;
                              if (con.State != ConnectionState.Open)
                                   con.Open();
                              cmd.ExecuteNonQuery();

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == -1)
                                   throw new Exception("TRANSACTION_ROLLED_BACK");

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == 100)
                                   throw new Exception("RECORD_ALREADY_EXISTS");

                              return int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) > 0 ? true : false;
                              }
                         }
                    }
               catch (Exception ex)
                    {
                    throw new ApplicationException(ex.Message);
                    }

               }

          public bool Retire(int materialId, UserDetails userDetails)
               {
               try
                    {
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_RetireMaterial", con))
                              {
                              cmd.Parameters.Add("@materialId", SqlDbType.Int).Value = materialId;
                              cmd.Parameters.Add("@cur_user", SqlDbType.NVarChar).Value = userDetails.Email;
                              cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                              cmd.CommandType = CommandType.StoredProcedure;
                              if (con.State != ConnectionState.Open)
                                   con.Open();
                              cmd.ExecuteNonQuery();

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == -1)
                                   throw new Exception("TRANSACTION_ROLLED_BACK");

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == 99)
                                   throw new Exception("RECORD_DOES_NOT_EXIST");

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == 100)
                                   throw new Exception("RECORD_ALREADY_RETIRED");

                              return int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) > 0 ? true : false;
                              }
                         }
                    }
               catch (Exception ex)
                    {
                    throw new ApplicationException(ex.Message);
                    }
               }

          public Types.Materials.Material RetrieveActiveMaterialsById(int materialId)
               {
               var localMaterialItem = new Types.Materials.Material();

               try
                    {
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_RetrieveActiveMaterialDetailsById", con))
                              {
                              cmd.Parameters.Add("@materialId", SqlDbType.Int).Value = materialId;
                              cmd.CommandType = CommandType.StoredProcedure;
                              if (con.State != ConnectionState.Open)
                                   con.Open();

                              SqlDataAdapter da = new SqlDataAdapter(cmd);
                              var dSet = new DataSet();
                              da.Fill(dSet);

                              if (dSet.Tables.Count > 0)
                                   if (dSet.Tables[0].Rows.Count > 0)
                                        {
                                        foreach (DataRow row in dSet.Tables[0].Rows)
                                             localMaterialItem = ConvertDataSetToMaterial(row, false);
                                        }
                                   else
                                        {
                                        throw new ApplicationException("RECORD_NOT_FOUND_OR_NOT_ACTIVE");
                                        }
                              }
                         }

                    return localMaterialItem;
                    }
               catch (Exception ex)
                    {
                    throw new ApplicationException(ex.Message);
                    }
               }



          public List<Types.Materials.Material> RetrieveAllActiveMaterials()
               {
               var localMaterialList = new List<Types.Materials.Material>();

               try
                    {
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_RetrieveAllActiveMaterials", con))
                              {
                              cmd.CommandType = CommandType.StoredProcedure;
                              if (con.State != ConnectionState.Open)
                                   con.Open();

                              SqlDataAdapter da = new SqlDataAdapter(cmd);
                              var dSet = new DataSet();
                              da.Fill(dSet);

                              localMaterialList = ConvertDataSetToMaterialList(dSet, false);
                              }
                         }

                    return localMaterialList;
                    }
               catch (Exception ex)
                    {
                    throw new ApplicationException(ex.Message);
                    }
               }

          public List<Types.Materials.Material> RetrieveAllMaterials()
               {
               var localMaterialList = new List<Types.Materials.Material>();

               try
                    {
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_RetrieveAllMaterials", con))
                              {
                              cmd.CommandType = CommandType.StoredProcedure;
                              if (con.State != ConnectionState.Open)
                                   con.Open();

                              SqlDataAdapter da = new SqlDataAdapter(cmd);
                              var dSet = new DataSet();
                              da.Fill(dSet);

                              localMaterialList = ConvertDataSetToMaterialList(dSet, false);
                              }
                         }

                    return localMaterialList;
                    }
               catch (Exception ex)
                    {
                    throw new ApplicationException(ex.Message);
                    }
               }

          public bool Update(Types.Materials.Material materialObj, UserDetails userDetails)
               {
               /*
                * 	@materialId		INT,
    @code	VARCHAR(10),
    @description VARCHAR(255),
    @costId	INT,
    @cur_user NVARCHAR(128)
                */
               try
                    {
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_EditMaterial", con))
                              {
                              cmd.Parameters.Add("@materialId", SqlDbType.Int).Value = materialObj.Id;
                              cmd.Parameters.Add("@code", SqlDbType.VarChar).Value = materialObj.Code;
                              cmd.Parameters.Add("@unitCost", SqlDbType.Float).Value = materialObj.Cost.BasePrice;
                              cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = materialObj.Description;
                              cmd.Parameters.Add("@cur_user", SqlDbType.NVarChar).Value = userDetails.Email;
                              cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                              cmd.CommandType = CommandType.StoredProcedure;
                              if (con.State != ConnectionState.Open)
                                   con.Open();
                              cmd.ExecuteNonQuery();

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == -1)
                                   throw new Exception("TRANSACTION_ROLLED_BACK");

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == 99)
                                   throw new Exception("RECORD_DOES_NOT_EXIST");

                              if (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == 100)
                                   throw new Exception("COST_IS_NOT_RELATED_TO_MATERIAL");

                              return int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) > 0 ? true : false;
                              }
                         }
                    }
               catch (Exception ex)
                    {
                    throw new ApplicationException(ex.Message);
                    }
               }

          public Types.Materials.Material RetrieveActiveMaterialsByCode(string materialCode)
               {
               var localMaterialItem = new Types.Materials.Material();

               try
                    {
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_RetrieveActiveMaterialDetailsByCode", con))
                              {
                              cmd.Parameters.Add("@materialCode", SqlDbType.VarChar).Value = materialCode;
                              cmd.CommandType = CommandType.StoredProcedure;
                              if (con.State != ConnectionState.Open)
                                   con.Open();

                              SqlDataAdapter da = new SqlDataAdapter(cmd);
                              var dSet = new DataSet();
                              da.Fill(dSet);

                              if (dSet.Tables.Count > 0)
                                   if (dSet.Tables[0].Rows.Count > 0)
                                        {
                                        foreach (DataRow row in dSet.Tables[0].Rows)
                                             localMaterialItem = ConvertDataSetToMaterial(row, false);
                                        }
                                   else
                                        {
                                        throw new ApplicationException("RECORD_NOT_FOUND_OR_NOT_ACTIVE");
                                        }
                              }
                         }

                    return localMaterialItem;
                    }
               catch (Exception ex)
                    {
                    throw new ApplicationException(ex.Message);
                    }
               }

          public List<Types.Materials.Material> RetrieveMaterialDetailsByAssemblyId(int assemblyId)
               {
               var localMaterialItem = new List<Types.Materials.Material>();

               try
                    {
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_RetrieveMaterialDetailsByAssemblyId", con))
                              {
                              cmd.Parameters.Add("@assemblyId", SqlDbType.VarChar).Value = assemblyId;
                              cmd.CommandType = CommandType.StoredProcedure;
                              if (con.State != ConnectionState.Open)
                                   con.Open();

                              SqlDataAdapter da = new SqlDataAdapter(cmd);
                              var dSet = new DataSet();
                              da.Fill(dSet);

                             localMaterialItem = ConvertDataSetToMaterialList(dSet, true);
                              }
                         }
                    return localMaterialItem;
                    }
               catch (Exception ex)
                    {
                    throw new ApplicationException(ex.Message);
                    }
               }

          #region PRIVEATE METHODS
          private List<Types.Materials.Material> ConvertDataSetToMaterialList(DataSet dSet, bool isFromAssembly)
               {
               var list = new List<Types.Materials.Material>();
               if (dSet.Tables.Count > 0)
                    {
                    if (!isFromAssembly)
                         {
                         foreach (DataRow materialObject in dSet.Tables[0].Rows)
                              {
                              list.Add(ConvertDataSetToMaterial(materialObject, true));
                              }
                         }
                    else
                         {
                         foreach (DataRow materialObject in dSet.Tables[0].Rows)
                              {
                              list.Add(ConvertDataSetToMaterialFromAssembly(materialObject));
                              }
                         }
                    }
               return list;
               }

          private Types.Materials.Material ConvertDataSetToMaterialFromAssembly(DataRow rowSet)
               {
               return new Types.Materials.Material
                    {
                    Code = rowSet["MaterialId"].ToString()
                    ,
                    Description = rowSet["Description"].ToString()
                    ,
                    Quantity = int.Parse(rowSet["Quantity"].ToString())
                    ,
                    Unit = rowSet["Unit"].ToString()
                    ,
                    Cost = new Costing
                         {
                         BasePrice = double.Parse(rowSet["BasePrice"].ToString())
                         ,
                         ActualCost = double.Parse(rowSet["MaterialCost"].ToString())
                         }
                    };
               }

          private Types.Materials.Material ConvertDataSetToMaterial(DataRow rowSet, bool includeMaterialId)
               {
               return new Types.Materials.Material
                    {
                    Id = int.Parse(rowSet["Id"].ToString()),
                    Code = rowSet["Code"].ToString(),
                    Description = rowSet["Description"].ToString(),
                    CreationDate = DateTime.Parse(rowSet["DateCreated"].ToString()),
                    Cost = new Costing { BasePrice = double.Parse(rowSet["Cost"].ToString()) },
                    IsActive = rowSet["Status"] != null ? (bool?)bool.Parse(rowSet["Status"].ToString()) : null
                    };
               }
          #endregion
          }
     }
