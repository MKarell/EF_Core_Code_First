using System;
using System.Collections.Generic;

namespace EF_Core_Code_First.Models
{
    public class ComponentManufacturers
    {
        public int Id { get; set; }
        public string Abbreviation { get; set; }
        public string FullName { get; set; }
        public DateTime FoundationDate { get; set; }

        public ICollection<Components> Components { get; set; } = new List<Components>();
    }
}
