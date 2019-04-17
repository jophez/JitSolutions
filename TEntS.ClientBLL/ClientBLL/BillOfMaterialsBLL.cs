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
using TEntS.Types.BOM;
using TEntS.Types.Markups;
using TEntS.Types.Pole;
using TEntS.Types.ProjectInfo;

namespace TEntS.ClientBLL.ClientBLL
					{
					public class BillOfMaterialsBLL : IBillOfMaterials
										{
										public bool AssignMarkupToBom(int bomId, int markupCode, UserDetails userDetails)
															{
															try
																				{
																				using (var con = new SqlConnection(Utility.ConStr))
																									{
																									using (var cmd = new SqlCommand("USP_AssignMarkupToBom", con))
																														{
																														cmd.Parameters.Add("@bomId", SqlDbType.Int).Value = bomId;
																														cmd.Parameters.Add("@cur_user", SqlDbType.NVarChar).Value = userDetails.Email;
																														cmd.Parameters.Add("@markupId", SqlDbType.Int).Value = markupCode;
																														cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
																														cmd.CommandType = CommandType.StoredProcedure;

																														if (con.State != ConnectionState.Open)
																																			con.Open();

																														cmd.ExecuteNonQuery();

																														return int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) == 0 ? true : false;

																														}
																									}
																				}
															catch (Exception)
																				{

																				throw;
																				}
															}

										public bool Create(BomTypes bomItem, UserDetails userDetails)
															{
															/*
																@code				VARCHAR(10),
																@poleXml			XML,
																@markupId			INT,
																@ControlNumber		UNIQUEIDENTIFIER,
																@Name				VARCHAR(150),
																@Location			VARCHAR(MAX),
																@Owner				VARCHAR(150),
																@Description		VARCHAR(MAX),
																@Approved			BIT,
																@cur_user			NVARCHAR(128)
               */
															try
																				{
																				var poleXml = ConvertPoleListToXml(bomItem.PoleList);
																				using (var con = new SqlConnection(Utility.ConStr))
																									{
																									using (var cmd = new SqlCommand("USP_AddBoM", con))
																														{
																														cmd.Parameters.Add("@code", SqlDbType.NVarChar).Value = bomItem.Code;
																														cmd.Parameters.Add("@cur_user", SqlDbType.NVarChar).Value = userDetails.Email;
																														cmd.Parameters.Add("@poleXml", SqlDbType.Xml).Value = poleXml;
																														cmd.Parameters.Add("@markupId", SqlDbType.Int).Value = bomItem.Markup.Id;
																														cmd.Parameters.Add("@ControlNumber", SqlDbType.UniqueIdentifier).Value = bomItem.ProjectInfo.ControlNumber;
																														cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = bomItem.ProjectInfo.Name;
																														cmd.Parameters.Add("@Location", SqlDbType.VarChar).Value = bomItem.ProjectInfo.Location;
																														cmd.Parameters.Add("@Owner", SqlDbType.VarChar).Value = bomItem.ProjectInfo.Owner;
																														cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = bomItem.ProjectInfo.Description;
																														cmd.Parameters.Add("@Approved", SqlDbType.Bit).Value = bomItem.ProjectInfo.IsApproved;
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
															catch (Exception)
																				{

																				throw;
																				}
															}

										private string ConvertPoleListToXml(List<PoleTypes> poleList)
															{
															var xmlDoc = new XmlDocument();
															var root = xmlDoc.CreateElement("POLES");
															xmlDoc.AppendChild(root);

															foreach (var pole in poleList)
																				{
																				var poleItem = xmlDoc.CreateElement("POLE");
																				var assemblyRoot = xmlDoc.CreateElement("ASSEMBLIES");

																				var idNode = xmlDoc.CreateElement("NUMBER");
																				var idText = xmlDoc.CreateTextNode(pole.Number.ToString());
																				idNode.AppendChild(idText);

																				var relBomNode = xmlDoc.CreateElement("RELBOMCODE");
																				var relBomNodeText = xmlDoc.CreateTextNode(pole.RelBomCode);
																				relBomNode.AppendChild(relBomNodeText);

																				var cNode = xmlDoc.CreateElement("CODE");
																				var cText = xmlDoc.CreateTextNode(pole.Code);
																				cNode.AppendChild(cText);

																				var spanNode = xmlDoc.CreateElement("BSPAN");
																				var spanText = xmlDoc.CreateTextNode(pole.BSpan.ToString());
																				spanNode.AppendChild(spanText);

																				var wNode = xmlDoc.CreateElement("WIRES");
																				var wText = xmlDoc.CreateTextNode(pole.Wires.ToString());
																				wNode.AppendChild(wText);

																				var raIdNode = xmlDoc.CreateElement("RELATEDASSEMBLYID");

																				foreach (var assembly in pole.PoleAssemblyList)
																									{
																									var assemblyItem = xmlDoc.CreateElement("ASSEMBLY");

																									var arelBomNode = xmlDoc.CreateElement("RELBOMCODE");
																									var arelBomNodeText = xmlDoc.CreateTextNode(pole.RelBomCode);
																									arelBomNode.AppendChild(arelBomNodeText);

																									var relPoleNode = xmlDoc.CreateElement("POLECODE");
																									var relPoleNodeText = xmlDoc.CreateTextNode(assembly.Code.ToString());
																									relPoleNode.AppendChild(relPoleNodeText);

																									var aIdNode = xmlDoc.CreateElement("ID");
																									var aIdText = xmlDoc.CreateTextNode(assembly.AssemblyId.ToString());
																									aIdNode.AppendChild(aIdText);

																									var raIdText = xmlDoc.CreateTextNode(string.Format("{0}, ", assembly.AssemblyId.ToString()));
																									raIdNode.AppendChild(raIdText);

																									var qNode = xmlDoc.CreateElement("QUANTITY");
																									var qText = xmlDoc.CreateTextNode(assembly.Quantity.ToString());
																									qNode.AppendChild(qText);

																									assemblyItem.AppendChild(arelBomNode);
																									assemblyItem.AppendChild(relPoleNode);
																									assemblyItem.AppendChild(aIdNode);
																									assemblyItem.AppendChild(qNode);

																									assemblyRoot.AppendChild(assemblyItem);
																									}

																				poleItem.AppendChild(raIdNode);
																				poleItem.AppendChild(idNode);
																				poleItem.AppendChild(relBomNode);
																				poleItem.AppendChild(cNode);
																				poleItem.AppendChild(spanNode);
																				poleItem.AppendChild(wNode);
																				poleItem.AppendChild(assemblyRoot);

																				root.AppendChild(poleItem);
																				}

															return xmlDoc.InnerXml;

															}

										public List<MarkupTypes> GetMarkups(int bomId)
															{
															var markupList = new List<MarkupTypes>();

															using (var con = new SqlConnection(Utility.ConStr))
																				{
																				using (var cmd = new SqlCommand("USP_GetMarkups", con))
																									{
																									cmd.Parameters.Add("@bomId", SqlDbType.Int).Value = bomId;
																									cmd.CommandType = CommandType.StoredProcedure;

																									if (con.State != ConnectionState.Open)
																														con.Open();

																									var da = new SqlDataAdapter(cmd);
																									DataTable dt = new DataTable();
																									da.Fill(dt);

																									foreach (DataRow row in dt.Rows)
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
																																			Generic_1 = row["Generic1"] != null ? (float?)float.Parse(row["Generic1"].ToString()) : null
																																																											,
																																			Generic_2 = row["Generic2"] != null ? (float?)float.Parse(row["Generic2"].ToString()) : null
																																																											,
																																			Generic_3 = row["Generic3"] != null ? (float?)float.Parse(row["Generic3"].ToString()) : null
																																																											,
																																			Marketing = float.Parse(row["Marketing"].ToString())
																																																											,
																																			Markup = float.Parse(row["Markup"].ToString())
																																																											,
																																			Representation = float.Parse(row["Representation"].ToString())
																																																											,
																																			Vat_Ewt = float.Parse(row["Vat_Ewt"].ToString())
																																			};

																														markupList.Add(markupItem);
																														}
																									}
																				}

															return markupList;

															}

										public bool Retire(int bomId, UserDetails userDetails)
															{
															throw new NotImplementedException();
															}

										public List<BomTypes> RetrieveAllActiveBomItems()
															{
															var bomList = new List<BomTypes>();
															using (var con = new SqlConnection(Utility.ConStr))
																				{
																				using (var cmd = new SqlCommand("USP_RetrieveActiveBOMDetails", con))
																									{
																									cmd.CommandType = CommandType.StoredProcedure;
																									if (con.State != ConnectionState.Open)
																														con.Open();

																									var da = new SqlDataAdapter(cmd);
																									var dSet = new DataSet();
																									da.Fill(dSet);

																									bomList = dSet.Tables.Count > 0 ? ConvertDataSetToBom(dSet) : new List<BomTypes>();
																									}
																				}
															return bomList;
															}

										public List<Assembly> RetrieveAllBomAssemblyDetails()
															{
															var assemblyList = new List<Assembly>();
															using (var con = new SqlConnection(Utility.ConStr))
																				{
																				using (var cmd = new SqlCommand("USP_RetrieveAllBOMAssemblyDetails", con))
																									{
																									cmd.CommandType = CommandType.StoredProcedure;
																									if (con.State != ConnectionState.Open)
																														con.Open();

																									var da = new SqlDataAdapter(cmd);
																									var dSet = new DataSet();
																									da.Fill(dSet);

																									assemblyList = dSet.Tables.Count > 0 ? ConvertToAssemblyList(dSet) : new List<Assembly>();
																									}
																				}
															return assemblyList;
															}

										private List<Assembly> ConvertToAssemblyList(DataSet dSet)
															{
															var list = new List<Assembly>();
															foreach (DataRow row in dSet.Tables[0].Rows)
																				{
																				var assemblyItem = new Assembly
																									{
																									Code = row["PoleCode"].ToString()
																																		,
																									Classification = row["Classification"].ToString().Length > 0 && row["Classification"] != null ? row["Classification"].ToString() : string.Empty
																																		,
																									Name = row["Name"] != null && row["Name"].ToString().Length > 0 ? row["Name"].ToString() : string.Empty
																																		,
																									Quantity = row["Quantity"] != null ? int.Parse(row["Quantity"].ToString()) : 0
																									,
																									Id = int.Parse(row["AssemblyId"].ToString())
																									,
																									RelBomCode = new Guid(row["RELBOMCODE"].ToString())
																									};
																				list.Add(assemblyItem);
																				}

															return list;
															}

										private List<BomTypes> ConvertDataSetToBom(DataSet dSet)
															{
															var list = new List<BomTypes>();

															var dataSetItems = dSet.Tables[0].Rows.Count > 0 ? dSet.Tables[0].AsEnumerable().GroupBy(r => r["Code"]).Select(g => g.OrderBy(r => r["Code"]).First()).CopyToDataTable() : new DataTable();
															foreach (DataRow row in dataSetItems.Rows)
																				{
																				var bomItem = new BomTypes()
																									{
																									Id = int.Parse(row["Id"].ToString()),
																									IsActive = bool.Parse(row["Active"].ToString()),
																									Code = row["Code"].ToString(),
																									DateCreated = DateTime.Parse(row["DateCreated"].ToString()),
																									ProjectInfo = new ProjectInfoType
																														{
																														ControlNumber = new Guid(row["ControlNumber"].ToString()),
																														Description = row["Description"].ToString(),
																														Location = row["Location"].ToString(),
																														Name = row["CustomerName"].ToString(),
																														Owner = row["Owner"].ToString(),
																														IsApproved = bool.Parse(row["Approved"].ToString())
																														},
																									Markup = GetMarkups(int.Parse(row["Id"].ToString())).Where(m => m.Code.ToLower().Equals(row["Markup"].ToString().ToLower())).FirstOrDefault()
																									//PoleList = GetPolesAndAssembly(row)
																									};
																				list.Add(bomItem);

																				}

															foreach (BomTypes row in list)
																				{
																				var poleTables = dSet.Tables[0].Select("RelBomCode ='" + row.Code + "'");

																				row.PoleList = GetPolesAndAssembly(poleTables);

																				}
															return list;
															}

										private List<PoleTypes> GetPolesAndAssembly(DataRow[] rows)
															{
															var poles = new List<PoleTypes>();
															var assemblyList = new List<Assembly>();
															foreach (DataRow row in rows)
																				{
																				var poleItem = new PoleTypes
																									{
																									Number = row["PoleNumber"].ToString()
																									,
																									Code = row["PoleCode"].ToString()
																									,
																									BSpan = int.Parse(row["BSpan"].ToString())
																									,
																									Wires = int.Parse(row["Wires"].ToString())
																									,
																									RelBomCode = row["RelBomCode"].ToString()
																									};
																				poles.Add(poleItem);
																				}

															//foreach (DataRow row in rows)
															//					{
															//					var assemblyItem = new Assembly
															//										{
															//										Code = row["PoleCode"].ToString()
															//									,
															//										Classification = row["Classification"].ToString().Length > 0 && row["Classification"] != null ? row["Classification"].ToString() : string.Empty
															//									,
															//										Name = row["AssemblyName"] != null && row["AssemblyName"].ToString().Length > 0 ? row["AssemblyName"].ToString() : string.Empty
															//									,
															//										Quantity = row["Quantity"] != null ? int.Parse(row["Quantity"].ToString()) : 0
															//										};
															//					assemblyList.Add(assemblyItem);
															//					}

															//foreach (var pole in poles)
															//					{
															//					foreach (var aItem in assemblyList)
															//										{
															//										if (aItem.Code.Equals(pole.Code))
															//															{
															//															pole.AssemblyList = new List<Assembly>();
															//															pole.AssemblyList.Add(aItem);
															//															}
															//										}
															//					}

															return poles;
															}

										public List<BomTypes> RetrieveAllBomItems()
															{
															var bomList = new List<BomTypes>();
															using (var con = new SqlConnection(Utility.ConStr))
																				{
																				using (var cmd = new SqlCommand("USP_RetrieveAllBOMDetails", con))
																									{
																									cmd.CommandType = CommandType.StoredProcedure;
																									if (con.State != ConnectionState.Open)
																														con.Open();

																									var da = new SqlDataAdapter(cmd);
																									var dSet = new DataSet();
																									da.Fill(dSet);

																									bomList = dSet.Tables.Count > 0 ? ConvertDataSetToBom(dSet) : new List<BomTypes>();
																									}
																				}
															return bomList;
															}

										public bool Update(BomTypes bomItem, UserDetails userDetails)
															{
															try
																				{
																				//TODO : Create the Update stored procedure
																				var poleXml = ConvertPoleListToXml(bomItem.PoleList);
																				using (var con = new SqlConnection(Utility.ConStr))
																									{
																									using (var cmd = new SqlCommand("USP_UpdateBoM", con))
																														{
																														cmd.Parameters.Add("@code", SqlDbType.NVarChar).Value = bomItem.Code;
																														cmd.Parameters.Add("@cur_user", SqlDbType.NVarChar).Value = userDetails.Email;
																														cmd.Parameters.Add("@poleXml", SqlDbType.Xml).Value = poleXml;
																														cmd.Parameters.Add("@markupId", SqlDbType.Int).Value = bomItem.Markup.Id;
																														cmd.Parameters.Add("@ControlNumber", SqlDbType.UniqueIdentifier).Value = bomItem.ProjectInfo.ControlNumber;
																														cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = bomItem.ProjectInfo.Name;
																														cmd.Parameters.Add("@Location", SqlDbType.VarChar).Value = bomItem.ProjectInfo.Location;
																														cmd.Parameters.Add("@Owner", SqlDbType.VarChar).Value = bomItem.ProjectInfo.Owner;
																														cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = bomItem.ProjectInfo.Description;
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
															catch (Exception)
																				{

																				throw;
																				}
															}

										public bool RemoveMarkupFromBom(int bomId, int markupId, UserDetails userDetails)
															{
															try
																				{
																				using (var con = new SqlConnection(Utility.ConStr))
																									{
																									using (var cmd = new SqlCommand("USP_RemoveMarkupFromBom", con))
																														{
																														cmd.Parameters.Add("@bomId", SqlDbType.Int).Value = bomId;
																														cmd.Parameters.Add("@markupId", SqlDbType.Int).Value = markupId;
																														cmd.Parameters.Add("@cur_user", SqlDbType.NVarChar).Value = userDetails.Email;
																														cmd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
																														cmd.CommandType = CommandType.StoredProcedure;

																														if (con.State != ConnectionState.Open)
																																			con.Open();

																														cmd.ExecuteNonQuery();
																														string message = string.Empty;
																														switch (int.Parse(cmd.Parameters["@returnValue"].Value.ToString()))
																																			{
																																			case -1: { message = "TRANSACTION_ROLLED_BACK"; break; }
																																			case 99: { message = "BOM_NOT_EXISTING"; break; }
																																			case 100: { message = "BOM_NOT_ACTIVE"; break; }
																																			case 101: { message = "MARKUP_NOT_EXISTING"; break; }
																																			case 102: { message = "MARKUP_NOT_ACTIVE"; break; }
																																			};

																														if (message.Length > 0) throw new Exception(message);

																														return int.Parse(cmd.Parameters["@returnValue"].Value.ToString()) >= 0 ? true : false;

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
