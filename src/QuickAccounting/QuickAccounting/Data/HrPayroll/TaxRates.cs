using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.HrPayroll
{
    public class TaxRates
    {

        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public int TaxNameId { get; set; }
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a valid numeric or decimal number.")]
        public decimal Rate { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

    }
}
