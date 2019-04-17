using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEntS.Types.Materials
{
    public class MaterialForAssembly : Material
    {
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }
}
