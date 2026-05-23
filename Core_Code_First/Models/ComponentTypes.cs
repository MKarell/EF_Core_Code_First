using System.Collections.Generic;

namespace EF_Core_Code_First.Models
{
    public class ComponentTypes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public ICollection<Components> Components { get; set; } = new List<Components>();
    }
}
