using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEntS.Types;
using TEntS.Types.Materials;

namespace TEntS.ClientBLL
     {
     public interface IMaterial
          {
          Material RetrieveActiveMaterialsById(int materialId);
          Material RetrieveActiveMaterialsByCode(string materialCode);
          List<Types.Materials.Material> RetrieveAllActiveMaterials();
          List<Types.Materials.Material> RetrieveAllMaterials();
          bool Create(Material materialObj, UserDetails userDetails);
          bool Update(Material materialObj, UserDetails userDetails);
          bool Retire(int materialId, UserDetails userDetails);
          bool Activate(int materialId, UserDetails userDetails);
          List<Types.Materials.Material> RetrieveMaterialDetailsByAssemblyId(int assemblyId);
          }
     }
