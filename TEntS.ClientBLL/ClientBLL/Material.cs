using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEntS.ClientBLL.Interfaces;
using TEntS.Types;
using TEntS.Types.Materials;

namespace TEntS.ClientBLL.ClientBLL
     {
     public class Material : IMaterial
          {
          IMaterialBase _base;
          public Material(IMaterialBase materialBase)
               {
               _base = materialBase;
               }
          public bool Activate(int materialId, UserDetails userDetails)
               {
               try
                    {
                    return _base.Activate(materialId, userDetails);
                    }
               catch (Exception ex)
                    {
                    throw new TEntS.Types.Exception.TEntSException(ex.Message);
                    }
               }

          public bool Create(Types.Materials.Material materialObj, UserDetails userDetails)
               {
               try
                    {
                    return _base.Create(materialObj, userDetails);
                    }
               catch (Exception ex)
                    {
                    throw new TEntS.Types.Exception.TEntSException(ex.Message);
                    }
               }

          public bool Retire(int materialId, UserDetails userDetails)
               {
               try
                    {
                    return _base.Retire(materialId, userDetails);
                    }
               catch (Exception ex)
                    {
                    throw new TEntS.Types.Exception.TEntSException(ex.Message);
                    }
               }

          public Types.Materials.Material RetrieveActiveMaterialsByCode(string materialCode)
               {
               try
                    {
                    return _base.RetrieveActiveMaterialsByCode(materialCode);
                    }
               catch (Exception ex)
                    {
                    throw new TEntS.Types.Exception.TEntSException(ex.Message);
                    }
               }

          public Types.Materials.Material RetrieveActiveMaterialsById(int materialId)
               {
               try
                    {
                    return _base.RetrieveActiveMaterialsById(materialId);
                    }
               catch (Exception ex)
                    {
                    throw new TEntS.Types.Exception.TEntSException(ex.Message);
                    }
               }

          public List<Types.Materials.Material> RetrieveAllActiveMaterials()
               {
               try
                    {
                    return _base.RetrieveAllActiveMaterials();
                    }
               catch (Exception ex)
                    {
                    throw new TEntS.Types.Exception.TEntSException(ex.Message);
                    }
               }

          public List<Types.Materials.Material> RetrieveAllMaterials()
               {
               try
                    {
                    return _base.RetrieveAllMaterials();
                    }
               catch (Exception ex)
                    {
                    throw new TEntS.Types.Exception.TEntSException(ex.Message);
                    }
               }

          public List<Types.Materials.Material> RetrieveMaterialDetailsByAssemblyId(int assemblyId)
               {
               try
                    {
                    return _base.RetrieveMaterialDetailsByAssemblyId(assemblyId);
                    }
               catch (Exception ex)
                    {
                    throw new TEntS.Types.Exception.TEntSException(ex.Message);
                    }
               }

          public bool Update(Types.Materials.Material materialObj, UserDetails userDetails)
               {
               try
                    {
                    return _base.Update(materialObj, userDetails);
                    }
               catch (Exception ex)
                    {
                    throw new TEntS.Types.Exception.TEntSException(ex.Message);
                    }
               }
          }
     }
