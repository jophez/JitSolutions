using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEntS.Types;
using TEntS.Types.Markups;

namespace TEntS.ClientBLL.Interfaces
     {
     public interface IMarkups
          {
          bool Create(MarkupTypes markup, UserDetails userDetails);
          bool AssignMarkupToBOM(int markupId, int bomId);
          List<MarkupTypes> RetrieveAllMarkups();
          List<MarkupTypes> RetrieveAllActiveMarkups();
          List<MarkupTypes> RetrieveMarkupsById(int markupId);
          List<MarkupTypes> RetrieveMarkupsByCode(string code);
          bool Update(MarkupTypes markup, UserDetails userDetails);

          bool Retire(int markupId, UserDetails userDetails);
          }
     }
