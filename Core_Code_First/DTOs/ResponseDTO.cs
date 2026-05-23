using System;

namespace EF_Core_Code_First.DTOs
{
    public class ResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public int Warranty { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Stock { get; set; }
    }
}
