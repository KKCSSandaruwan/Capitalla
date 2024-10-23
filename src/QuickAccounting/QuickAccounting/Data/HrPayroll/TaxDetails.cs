using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.HrPayroll
{
    public class TaxDetails
    {
        [Key]
        public int Id { get; set; }
        public string TaxCode { get; set; }
        public int TaxNameId { get; set; }
        public bool IsActive { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }

  
}
