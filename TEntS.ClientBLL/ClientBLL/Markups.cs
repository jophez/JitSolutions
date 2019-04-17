using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEntS.ClientBLL.Interfaces;
using TEntS.Types;
using TEntS.Types.Markups;

namespace TEntS.ClientBLL.ClientBLL
     {
     public class Markups : IMarkups
          {

          private readonly IMarkups bll;
          public Markups(IMarkups markupBll)
               {
               bll = markupBll;
               }
          public bool AssignMarkupToBOM(int markupId, int bomId)
               {
               throw new NotImplementedException();
               }

          public bool Create(MarkupTypes markup, UserDetails userDetails)
               {
               try
                    {
                    return bll.Create(markup, userDetails);
                    }
               catch (Exception ex)
                    {
                    throw new TEntS.Types.Exception.TEntSException(ex.Message);
                    }
               }

          public bool Retire(int markupId, UserDetails userDetails)
               {
               try
                    {
                    return bll.Retire(markupId, userDetails);
                    }
               catch (Exception ex)
                    {

                    throw new TEntS.Types.Exception.TEntSException(ex.Message);
                    }
               }

          public List<MarkupTypes> RetrieveAllActiveMarkups()
               {
               return bll.RetrieveAllActiveMarkups();
               }

          public List<MarkupTypes> RetrieveAllMarkups()
               {
               try
                    {
                    return bll.RetrieveAllMarkups();
                    }
               catch (Exception ex)
                    {
                    throw new TEntS.Types.Exception.TEntSException(ex.Message);
                    }
               }

          public List<MarkupTypes> RetrieveMarkupsByCode(string code)
               {
               throw new NotImplementedException();
               }

          public List<MarkupTypes> RetrieveMarkupsById(int markupId)
               {
               throw new NotImplementedException();
               }

          public bool Update(MarkupTypes markup, UserDetails userDetails)
               {
               try
                    {
                    return bll.Update(markup, userDetails);
                    }
               catch (Exception ex)
                    {

                    throw new TEntS.Types.Exception.TEntSException(ex.Message);
                    }
               }
          }
     }
