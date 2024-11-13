using QuickAccounting.Enums;

namespace QuickAccounting.Utilities
{
    /// <summary>
    /// Utility class for generating unique transaction IDs based on transaction type and last counter value.
    /// </summary>
    public class IDGenerator
    {
        #region Public Methods
        /// <summary>
        /// Generates a unique transaction ID based on the specified transaction type and the last counter value.
        /// The format of the generated ID is: <prefix> + <incremented counter> + <suffix>.
        /// </summary>
        /// <param name="transactionType">The type of the transaction for which the ID is being generated. 
        /// Valid types include: SupplierPurchase, CustomerSales, TransferIn, TransferOut.</param>
        /// <param name="lastCounter">The last generated counter value. The counter is incremented by 1 for the new ID.</param>
        /// <returns>A string representing the unique transaction ID in the format: <prefix><counter><suffix>.</returns>
        /// <exception cref="ArgumentException">Thrown when an invalid transaction type is provided.</exception>
        public static string GenerateVoucherNo(TransactionType transactionType, int lastCounter)
        {
            string prefix = string.Empty;
            string suffix = string.Empty;
            int nextCounter = lastCounter++; // Increment the counter by 1 for the new ID

            // Map the TransactionType to its corresponding prefix and suffix
            switch (transactionType)
            {
                // Purchase-related transactions
                case TransactionType.SupplierPurchase:  // Purchase Order
                    prefix = "PUR";
                    suffix = "ORD";
                    break;

                case TransactionType.SupplierInvoice:    // Purchase Invoice
                    prefix = "PUR";
                    suffix = "INV";
                    break;

                case TransactionType.SupplierReturn:    // Purchase Return
                    prefix = "PUR";
                    suffix = "RET";
                    break;

                case TransactionType.SupplierPayment:   // Supplier Payment for Order
                    prefix = "PUR";
                    suffix = "PAY";
                    break;

                case TransactionType.SupplierReversal:  // Supplier Payment Reversal
                    prefix = "PUR";
                    suffix = "REV";
                    break;

                // Sales-related transactions
                case TransactionType.CustomerSale:     // Sales Order
                    prefix = "SAL";
                    suffix = "ORD";
                    break;

                case TransactionType.CustomerInvoice:     // Sales Invoice
                    prefix = "SAL";
                    suffix = "INV";
                    break;

                case TransactionType.CustomerReturn:    // Sales Return
                    prefix = "SAL";
                    suffix = "RET";
                    break;

                case TransactionType.CustomerPayment:   // Customer Payment for Sales
                    prefix = "SAL";
                    suffix = "PAY";
                    break;

                case TransactionType.CustomerReversal:  // Customer Payment Reversal
                    prefix = "SAL";
                    suffix = "REV";
                    break;

                default:
                    throw new ArgumentException($"Invalid transaction type: {transactionType}");
            }

            // Format the counter to 8 digits with leading zeros
            string formattedCounter = nextCounter.ToString("D8");

            // Return the generated ID in the format: <prefix><counter><suffix>
            return $"{prefix}{formattedCounter}{suffix}";
        }
        #endregion
    }
}