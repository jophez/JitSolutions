using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TEntS.BLL.Utilities;
using TEntS.ClientBLL.Interfaces;
using TEntS.Types;
using TEntS.Types.Assembly;
using TEntS.Types.Materials;

namespace TEntS.ClientBLL.ClientBLL
     {
     public class AssemblyBase : IAssemblyBll
          {
          MaterialBase materialBase;
          public AssemblyBase()
               {
               materialBase = new ClientBLL.MaterialBase();
               }

          public bool Activate(int assemblyId, UserDetails userDetails)
               {
               throw new NotImplementedException();
               }

          public bool Create(Assembly assemblyObj, UserDetails userDetails)
               {
               try
                    {
                    var materialListXml = ConvertMaterialListToXml(assemblyObj.Materials);
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_AddAssembly", con))
                              {
                              cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = assemblyObj.Name;
                              cmd.Parameters.Add("@classification", SqlDbType.VarChar).Value = assemblyObj.Classification;
                              cmd.Parameters.Add("@cur_user", SqlDbType.NVarChar).Value = userDetails.Email;
                              cmd.Parameters.Add("@materials", SqlDbType.Xml).Value = materialListXml;
                              cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                              cmd.CommandType = CommandType.StoredProcedure;

                              if (con.State != ConnectionState.Open)
                                   con.Open();

                              cmd.ExecuteNonQuery();

                              //TODO : Add the return codes from the database here

                              return int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) > 0 ? true : false;
                              }
                         }
                    }
               catch (Exception ex)
                    {
                    throw new ApplicationException(ex.Message);
                    }
               }

          private string ConvertMaterialListToXml(List<MaterialForAssembly> materials)
               {
               var xmlDoc = new XmlDocument();
               var root = xmlDoc.CreateElement("MATERIALS");
               xmlDoc.AppendChild(root);

               foreach (var material in materials)
                    {
                    var materialItem = xmlDoc.CreateElement("MATERIAL");

                    var idNode = xmlDoc.CreateElement("ID");
                    var idText = xmlDoc.CreateTextNode(material.Id.ToString());
                    idNode.AppendChild(idText);

                    var qNode = xmlDoc.CreateElement("QUANTITY");
                    var qText = xmlDoc.CreateTextNode(material.Quantity.ToString());
                    qNode.AppendChild(qText);

                    var uNode = xmlDoc.CreateElement("UNIT");
                    var uText = xmlDoc.CreateTextNode(material.Unit);
                    uNode.AppendChild(uText);

                    materialItem.AppendChild(idNode);
                    materialItem.AppendChild(qNode);
                    materialItem.AppendChild(uNode);

                    root.AppendChild(materialItem);
                    }

               return xmlDoc.InnerXml;
               }

          public bool Retire(int assemblyId, UserDetails userDetails)
               {
               try
                    {
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_RetireAssembly", con))
                              {
                              cmd.Parameters.Add("@assemblyId", SqlDbType.Int).Value = assemblyId;
                              cmd.Parameters.Add("@cur_user", SqlDbType.NVarChar).Value = userDetails.Email;
                              cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                              cmd.CommandType = CommandType.StoredProcedure;

                              if (con.State != ConnectionState.Open)
                                   con.Open();

                              cmd.ExecuteNonQuery();

                              //TODO : Add the return codes from the database here

                              return int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) > 0 ? true : false;
                              }
                         }
                    }
               catch (Exception ex)
                    {
                    throw new ApplicationException(ex.Message);
                    }
               }

          public List<Assembly> RetrieveAssemblyDetails()
               {
               var localAssemblyList = new List<Assembly>();

               try
                    {
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_RetrieveAssemblyDetails", con))
                              {
                              cmd.CommandType = CommandType.StoredProcedure;
                              if (con.State != ConnectionState.Open)
                                   con.Open();

                              SqlDataAdapter da = new SqlDataAdapter(cmd);
                              var dSet = new DataSet();
                              da.Fill(dSet);

                              localAssemblyList = ConvertDataSetToAssemblyList(dSet);
                              }
                         }

                    return localAssemblyList;
                    }
               catch (Exception ex)
                    {
                    throw new ApplicationException(ex.Message);
                    }
               }

          private List<Assembly> ConvertDataSetToAssemblyList(DataSet dSet)
               {
               var list = new List<Types.Assembly.Assembly>();
               if (dSet.Tables.Count > 0)
                    {
                    var assemblyName = string.Empty;
                    var assembly = new Assembly();
                    assembly.Materials = new List<MaterialForAssembly>();
                    foreach (DataRow assemblyRow in dSet.Tables[0].Rows)
                         {
                         if (!assemblyName.Trim().ToLower().Equals(assemblyRow["Name"].ToString().ToLower().Trim()))
                              {
                              assembly = new Assembly();
                              assembly.Materials = new List<MaterialForAssembly>();

                              assembly.Id = int.Parse(assemblyRow["Id"].ToString());
                              assembly.Name = assemblyRow["Name"].ToString();
                              assembly.Classification = assemblyRow["Classification"].ToString();
                              assembly.IsActive = (bool?)assemblyRow["Status"];
                              assemblyName = assemblyRow["Name"].ToString();
                              assembly.UnitPrice = assemblyRow["AssemblyPrice"] != null ? Decimal.Parse(assemblyRow["AssemblyPrice"].ToString()) : 0;
                              }
                         assembly.Materials = MapMaterialObjectToMaterialForAssembly(materialBase.RetrieveMaterialDetailsByAssemblyId(int.Parse(assemblyRow["Id"].ToString())));

                         if (list.Find(a => a.Name.Equals(assembly.Name)) != null)
                              continue;

                         list.Add(assembly);
                         //list.Add(ConvertDataSetToAssemblyList(assemblyRow, true));
                         }
                    }

               return list;
               }

          private Assembly ConvertDataSetToAssemblyList(DataRow assemblyRow, bool includeAssemblyId)
               {
               return new Types.Assembly.Assembly
                    {
                    Id = includeAssemblyId ? int.Parse(assemblyRow["Id"].ToString()) : 0
                   ,
                    Classification = assemblyRow["Classification"].ToString()
                   ,
                    Name = assemblyRow["Name"].ToString()
                   ,
                    IsActive = assemblyRow["Status"] != null ? (bool?)assemblyRow["Status"] : null
                    // ,
                    //Materials = MapMaterialObjectToMaterialForAssembly(materialBase.RetrieveActiveMaterialsByCode(assemblyRow["MaterialId"].ToString()), int.Parse(assemblyRow["Quantity"].ToString()), assemblyRow["Unit"].ToString())
                    };
               }

          private List<MaterialForAssembly> MapMaterialObjectToMaterialForAssembly(List<Types.Materials.Material> materials)
               {
               var materialList = new List<MaterialForAssembly>();
               foreach (var material in materials)
                    {
                    MaterialForAssembly matAssembly = new MaterialForAssembly
                         {
                         Id = material.Id
                        ,
                         Code = material.Code
                        ,
                         Description = material.Description
                        ,
                         Quantity = (int)material.Quantity
                        ,
                         Unit = material.Unit
                        ,
                         Cost = material.Cost
                         };
                    materialList.Add(matAssembly);
                    }
               return materialList;
               }

          public List<Assembly> RetrieveActiveAssemblyDetails()
               {
               var localAssemblyList = new List<Assembly>();

               try
                    {
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_RetrieveActiveAssemblyDetails", con))
                              {
                              cmd.CommandType = CommandType.StoredProcedure;
                              if (con.State != ConnectionState.Open)
                                   con.Open();

                              SqlDataAdapter da = new SqlDataAdapter(cmd);
                              var dSet = new DataSet();
                              da.Fill(dSet);

                              localAssemblyList = ConvertDataSetToAssemblyList(dSet);
                              }
                         }

                    return localAssemblyList;
                    }
               catch (Exception ex)
                    {
                    throw new ApplicationException(ex.Message);
                    }
               }

          public Assembly RetrieveAssemblyDetailsById(int assemblyId)
               {
               var assemblyItem = new Assembly();
               using (var con = new SqlConnection(Utility.ConStr))
                    {
                    using (var cmd = new SqlCommand("USP_RetrieveAssemblyDetailsById", con))
                         {
                         cmd.Parameters.Add("@assemblyId", SqlDbType.Int).Value = assemblyId;
                         cmd.CommandType = CommandType.StoredProcedure;
                         if (con.State != ConnectionState.Open)
                              con.Open();

                         SqlDataAdapter da = new SqlDataAdapter(cmd);
                         var dSet = new DataSet();
                         da.Fill(dSet);

                         assemblyItem = ConvertDataSetToAssemblyList(dSet).FirstOrDefault();
                         }
                    }

               return assemblyItem;
               }

          public bool Update(Assembly assemblyObj, UserDetails userDetails)
               {
               try
                    {
                    var materialListXml = ConvertMaterialListToXml(assemblyObj.Materials);
                    using (var con = new SqlConnection(Utility.ConStr))
                         {
                         using (var cmd = new SqlCommand("USP_EditAssembly", con))
                              {
                              cmd.Parameters.Add("@assemblyId", SqlDbType.Int).Value = assemblyObj.Id;
                              cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = assemblyObj.Name;
                              cmd.Parameters.Add("@classification", SqlDbType.VarChar).Value = assemblyObj.Classification;
                              cmd.Parameters.Add("@cur_user", SqlDbType.NVarChar).Value = userDetails.Email;
                              cmd.Parameters.Add("@materials", SqlDbType.Xml).Value = materialListXml;
                              cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                              cmd.CommandType = CommandType.StoredProcedure;

                              if (con.State != ConnectionState.Open)
                                   con.Open();

                              cmd.ExecuteNonQuery();

                              //TODO : Add the return codes from the database here

                              return int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) > 0 ? true : false;
                              }
                         }
                    }
               catch (Exception ex)
                    {
                    throw new ApplicationException(ex.Message);
                    }
               }

          public List<Types.Materials.MaterialForAssembly> RetrieveAssemblyMaterialsByCode(string assemblyCode)
               {
               var materialList = new List<TEntS.Types.Materials.MaterialForAssembly>();
               using (var con = new SqlConnection(Utility.ConStr))
                    {
                    using (var cmd = new SqlCommand("USP_RetrieveAssemblyDetailsByCode", con))
                         {
                         cmd.Parameters.Add("@assemblyName", SqlDbType.VarChar).Value = assemblyCode;
                         cmd.CommandType = CommandType.StoredProcedure;
                         if (con.State != ConnectionState.Open)
                              con.Open();

                         var dataTable = new DataTable();
                         dataTable.Load(cmd.ExecuteReader());

                         foreach (DataRow dr in dataTable.Rows)
                              {
                              var materialAssembly = new MaterialForAssembly
                                   {
                                   Code = dr["Code"].ToString(),
                                   Description = dr["Description"].ToString(),
                                   Quantity = int.Parse(dr["Quantity"].ToString()),
                                   Cost = new Costing { BasePrice = double.Parse(dr["BasePrice"].ToString()) },
                                   Unit = dr["Unit"].ToString(),
                                   IsActive = dr["Status"] != null ? (bool?)bool.Parse(dr["Status"].ToString()) : null
                                   };

                              materialList.Add(materialAssembly);
                              }
                         }
                    }

               return materialList;
               }

          public List<Types.Materials.MaterialForAssembly> RetrieveAssemblyMaterialsByClassification(string classification)
               {
               var materialList = new List<TEntS.Types.Materials.MaterialForAssembly>();
               using (var con = new SqlConnection(Utility.ConStr))
                    {
                    using (var cmd = new SqlCommand("USP_RetrieveAssemblyDetailsByClassification", con))
                         {
                         cmd.Parameters.Add("@assemblyClassfication", SqlDbType.VarChar).Value = classification;
                         cmd.CommandType = CommandType.StoredProcedure;
                         if (con.State != ConnectionState.Open)
                              con.Open();

                         SqlDataAdapter da = new SqlDataAdapter(cmd);
                         var dSet = new DataSet();
                         da.Fill(dSet);

                         if (dSet.Tables.Count > 0)
                              {
                              foreach (DataRow dr in dSet.Tables[0].Rows)
                                   {
                                   var materialAssembly = new MaterialForAssembly
                                        {
                                        Code = dr["Code"].ToString(),
                                        Description = dr["Description"].ToString(),
                                        Quantity = int.Parse(dr["Quantity"].ToString()),
                                        Cost = new Costing { BasePrice = double.Parse(dr["BasePrice"].ToString()) },
                                        Unit = dr["Unit"].ToString(),
                                        IsActive = dr["Status"] != null ? (bool?)bool.Parse(dr["Status"].ToString()) : null
                                        };

                                   materialList.Add(materialAssembly);
                                   }
                              }
                         }
                    }

               return materialList;
               }
          }
     }
