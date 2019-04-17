using System.Collections.Generic;
using TEntS.Types;
using TEntS.Types.Assembly;
using TEntS.Types.BOM;
using TEntS.Types.Markups;

namespace TEntS.ClientBLL.Interfaces
     {
     public interface IBillOfMaterials
          {
          List<BomTypes> RetrieveAllBomItems();
          List<BomTypes> RetrieveAllActiveBomItems();
          bool Create(BomTypes bomItem, UserDetails userDetails);
          bool Update(BomTypes bomItem, UserDetails userDetails);
          bool Retire(int bomId, UserDetails userDetails);
          bool AssignMarkupToBom(int bomId, int markupCode, UserDetails userDetails);
          List<MarkupTypes> GetMarkups(int bomId);
          bool RemoveMarkupFromBom(int bomId, int markupId, UserDetails userDetails);

										List<Assembly> RetrieveAllBomAssemblyDetails();
          }
     }
