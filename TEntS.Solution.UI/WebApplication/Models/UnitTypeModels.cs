using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Models
{
    public class UnitTypeModels
    {
        public int Id {get;set;}
        public string Name { get; set; }
    }

    public class UnitType
    {
        private readonly List<DefaultUnits> _unitTypes = new List<DefaultUnits>();

        public int SelectedId { get; set; }
        public IEnumerable<SelectListItem> Units { get { return new SelectList(_unitTypes); } }
    }

    public enum DefaultUnits
    {
        pc,
        meters,
        roll
    }
}