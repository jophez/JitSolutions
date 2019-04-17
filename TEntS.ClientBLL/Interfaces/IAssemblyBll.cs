using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEntS.Types;
using TEntS.Types.Assembly;
using TEntS.Types.Materials;

namespace TEntS.ClientBLL.Interfaces
{
    public interface IAssemblyBll
    {
        List<Assembly> RetrieveAssemblyDetails();
        List<Assembly> RetrieveActiveAssemblyDetails();
        Assembly RetrieveAssemblyDetailsById(int assemblyId);
        bool Create(Assembly assemblyObj, UserDetails userDetails);
        bool Update(Assembly assemblyObj, UserDetails userDetails);
        bool Retire(int assemblyId, UserDetails userDetails);
        bool Activate(int assemblyId, UserDetails userDetails);
        List<MaterialForAssembly> RetrieveAssemblyMaterialsByCode(string assemblyCode);
        List<MaterialForAssembly> RetrieveAssemblyMaterialsByClassification(string classification);
    }
}
