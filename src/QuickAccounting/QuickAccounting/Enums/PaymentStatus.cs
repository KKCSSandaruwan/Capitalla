namespace QuickAccounting.Enums
{
    public enum PaymentStatus
    {
        /// <summary>
        /// Payment has been fully completed.
        /// </summary>
        Paid = 1,

        /// <summary>
        /// No payment has been made.
        /// </summary>
        Unpaid = 2,

        /// <summary>
        /// Part of the payment has been made.
        /// </summary>
        Partial = 3,

        /// <summary>
        /// Payment exceeds the due amount.
        /// </summary>
        Overpaid = 4,

        /// <summary>
        /// Payment is in process but not completed.
        /// </summary>
        Pending = 5,

        /// <summary>
        /// Payment attempt failed.
        /// </summary>
        Failed = 6,

        /// <summary>
        /// Payment has been refunded.
        /// </summary>
        Refunded = 7,

        /// <summary>
        /// Payment or transaction was cancelled.
        /// </summary>
        Cancelled = 8,

        /// <summary>
        /// Payment is under dispute.
        /// </summary>
        Disputed = 9
    }
}
