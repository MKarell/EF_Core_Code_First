namespace EF_Core_Code_First.Models
{
    public class PCComponents
    {
        public int PCId { get; set; }
        public string ComponentCode { get; set; }
        public int Amount { get; set; }

        public PCs PC { get; set; }
        public Components Component { get; set; }
    }
}
