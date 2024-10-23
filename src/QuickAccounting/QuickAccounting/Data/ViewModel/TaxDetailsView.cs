namespace QuickAccounting.Data.ViewModel
{
    public class TaxDetailsView
    {
        public int Id { get; set; }
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
        public int TaxNameId { get; set; }
        public bool IsActive { get; set; }
        public IList<TaxView> listModel { get; set; }
      
    }

   
}
