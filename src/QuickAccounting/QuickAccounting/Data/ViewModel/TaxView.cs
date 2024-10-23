using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.ViewModel
{
	public class TaxView
	{
        public int TaxId { get; set; }
        public string TaxName { get; set; }
        public decimal Rate { get; set; }
        public bool IsActive { get; set; }
        public bool Checked { get; set; }
        public int CompanyId { get; set; }
    }
}
