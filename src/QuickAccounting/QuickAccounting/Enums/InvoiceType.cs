namespace QuickAccounting.Enums
{
    public enum InvoiceType
    {
        /// <summary>
        /// Invoices related to purchases made by the company.
        /// </summary>
        Purchase,

        /// <summary>
        /// Invoices issued for sales made to customers.
        /// </summary>
        Sales,

        /// <summary>
        /// Credit notes issued to customers to reduce the amount owed.
        /// </summary>
        CreditNote,

        /// <summary>
        /// Debit notes issued to suppliers to request additional payments.
        /// </summary>
        DebitNote,

        /// <summary>
        /// Proforma invoices sent to buyers in advance of shipment or delivery.
        /// </summary>
        Proforma
    }
}
