using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.ViewModel
{
    public class UnitView
    {
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public int NoOfDecimalplaces { get; set; }
        public int CompanyId { get; set; }
    }
}
