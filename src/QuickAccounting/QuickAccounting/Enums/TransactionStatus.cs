namespace QuickAccounting.Enums
{
    public enum TransactionStatus
    {
        /// <summary>
        /// Transaction is in draft and has not been finalized.
        /// </summary>
        Draft,

        /// <summary>
        /// Transaction is pending and awaiting processing.
        /// </summary>
        Pending,

        /// <summary>
        /// Transaction has been reviewed and approved.
        /// </summary>
        Approved,

        /// <summary>
        /// Transaction has been successfully completed.
        /// </summary>
        Completed,

        /// <summary>
        /// Transaction failed to complete successfully.
        /// </summary>
        Failed,

        /// <summary>
        /// Transaction is under review or dispute.
        /// </summary>
        Disputed,

        /// <summary>
        /// Transaction has been refunded back to the payer.
        /// </summary>
        Refunded,

        /// <summary>
        /// Transaction has been reversed and is no longer valid.
        /// </summary>
        Reversed,

        /// <summary>
        /// Transaction has been cancelled and will not proceed.
        /// </summary>
        Cancelled
    }
}
