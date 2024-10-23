using System.ComponentModel.DataAnnotations;

namespace QuickAccounting.Data.Setting
{
    public class Batch
    {
        [Key]
        public int BatchId { get; set; }
        public string BatchNo { get; set; }
        public int ProductId { get; set; }
        public string Barcode { get; set; }
        public string batchNo { get; set; }
    }
}
