namespace QuickAccounting.Enums
{
    public enum PaymentMethod
    {
        /// <summary>
        /// Represents a payment made using a payment voucher.
        /// </summary>
        Voucher,

        /// <summary>
        /// Represents a payment made to a supplier.
        /// </summary>
        Supplier,

        /// <summary>
        /// Represents a payment made through bank transfer.
        /// </summary>
        BankTransfer,

        /// <summary>
        /// Represents a cash payment.
        /// </summary>
        Cash,

        /// <summary>
        /// Represents an online payment made through digital platforms.
        /// </summary>
        Online
    }

}
