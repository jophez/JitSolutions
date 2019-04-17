using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEntS.ClientBLL.ClientBLL;
using TEntS.Types;
using TEntS.Types.Materials;

namespace TEntS.ClientBLL.Interfaces
     {
     public interface IMaterialBase
          {
          Types.Materials.Material RetrieveActiveMaterialsById(int materialId);

          Types.Materials.Material RetrieveActiveMaterialsByCode(string materialCode);

          List<Types.Materials.Material> RetrieveAllActiveMaterials();
          List<Types.Materials.Material> RetrieveAllMaterials();
          bool Create(Types.Materials.Material materialObj, UserDetails userDetails);
          bool Update(Types.Materials.Material materialObj, UserDetails userDetails);
          bool Retire(int materialId, UserDetails userDetails);
          bool Activate(int materialId, UserDetails userDetails);
          List<Types.Materials.Material> RetrieveMaterialDetailsByAssemblyId(int assemblyId);
          }
     }
