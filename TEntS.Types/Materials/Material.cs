using System;

namespace TEntS.Types.Materials
{
    public class Material
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public Costing Cost { get; set; }
        public DateTime CreationDate { get; set; }
        public int? Quantity { get; set; } 
        public string Unit { get; set; }
    }
}
