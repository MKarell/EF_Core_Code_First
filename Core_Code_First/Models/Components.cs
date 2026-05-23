using System.Collections.Generic;

namespace EF_Core_Code_First.Models
{
    public class Components
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ComponentManufacturersId { get; set; }
        public int ComponentTypesId { get; set; }

        public ComponentManufacturers Manufacturers { get; set; }
        public ComponentTypes Type { get; set; }
        public ICollection<PCComponents> PCComponents { get; set; } = new List<PCComponents>();
    }
}
