using System;
using System.Collections.Generic;
using TEntS.ClientBLL.Interfaces;
using TEntS.Types;
using TEntS.Types.Assembly;
using TEntS.Types.BOM;
using TEntS.Types.Markups;

namespace TEntS.ClientBLL.ClientBLL
					{
					public class BillOfMaterials : IBillOfMaterials
										{
										private readonly IBillOfMaterials _bll;
										public BillOfMaterials(IBillOfMaterials bomBll)
															{
															_bll = bomBll;
															}

										public bool AssignMarkupToBom(int bomId, int markupCode, UserDetails userDetails)
															{
															try
																				{
																				return _bll.AssignMarkupToBom(bomId, markupCode, userDetails);
																				}
															catch (Exception ex)
																				{
																				throw new Types.Exception.TEntSException(ex.Message);
																				}
															}

										public bool Create(BomTypes bomItem, UserDetails userDetails)
															{
															try
																				{
																				return _bll.Create(bomItem, userDetails);
																				}
															catch (Exception ex)
																				{

																				throw new TEntS.Types.Exception.TEntSException(ex.Message);
																				}
															}

										public List<MarkupTypes> GetMarkups(int bomId)
															{
															return _bll.GetMarkups(bomId);
															}

										public bool RemoveMarkupFromBom(int bomId, int markupId, UserDetails userDetails)
															{
															try
																				{
																				return _bll.RemoveMarkupFromBom(bomId, markupId, userDetails);
																				}
															catch (Exception ex)
																				{

																				throw new TEntS.Types.Exception.TEntSException(ex.Message);
																				}
															}

										public bool Retire(int bomId, UserDetails userDetails)
															{
															throw new NotImplementedException();
															}

										public List<BomTypes> RetrieveAllActiveBomItems()
															{
															return _bll.RetrieveAllActiveBomItems();
															}

										public List<Assembly> RetrieveAllBomAssemblyDetails()
															{
															return _bll.RetrieveAllBomAssemblyDetails();
															}

										public List<BomTypes> RetrieveAllBomItems()
															{
															return _bll.RetrieveAllBomItems();
															}

										public bool Update(BomTypes bomItem, UserDetails userDetails)
															{
															try
																				{
																				return _bll.Update(bomItem, userDetails);
																				}
															catch (Exception ex)
																				{

																				throw new TEntS.Types.Exception.TEntSException(ex.Message);
																				}
															}
										}
					}
