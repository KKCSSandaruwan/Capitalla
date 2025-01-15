namespace QuickAccounting.Enums
{
    public enum TransactionType
    {
        /// <summary>
        /// Represents a purchase order issued to a supplier for the procurement of goods or services.
        /// </summary>
        SupplierPurchase = 1,

        /// <summary>
        /// Represents a purchase invoice issued by a supplier for the goods or services provided under a purchase order.
        /// </summary>
        SupplierInvoice = 2,

        /// <summary>
        /// Represents a return of goods purchased from a supplier back to the supplier, typically due to defects or other issues.
        /// </summary>
        SupplierReturn = 3,

        /// <summary>
        /// Represents a payment made to a supplier in settlement of a purchase invoice.
        /// </summary>
        SupplierPayment = 4,

        /// <summary>
        /// Represents a reversal of a previously made payment to a supplier, often due to errors or disputes.
        /// </summary>
        SupplierReversal = 5,

        /// <summary>
        /// Represents a sales order issued to a customer for the sale of goods or services.
        /// </summary>
        CustomerSale = 6,

        /// <summary>
        /// Represents a sales invoice issued to a customer for goods or services provided under a sales order.
        /// </summary>
        CustomerInvoice = 7,

        /// <summary>
        /// Represents a return of goods sold to a customer, typically due to defects or other issues.
        /// </summary>
        CustomerReturn = 8,

        /// <summary>
        /// Represents a payment received from a customer in settlement of a sales invoice.
        /// </summary>
        CustomerPayment = 9,

        /// <summary>
        /// Represents a reversal of a previously received payment from a customer, often due to errors or disputes.
        /// </summary>
        CustomerReversal = 10,
    }
}
