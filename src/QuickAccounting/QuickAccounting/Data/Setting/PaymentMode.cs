using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Setting
{
    public class PaymentMode
    {
        [Key]
        public int PaymentmodeId { get; set; }
        public string Name { get; set; }
    }
}
