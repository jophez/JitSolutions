using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEntS.Types.Materials
{
    public class Costing
    {
        public int Id { get; set; }
        public double BasePrice { get; set; }
        public double ActualCost { get; set; }   //material cost
        public bool IsActive { get; set; }
    }
}
