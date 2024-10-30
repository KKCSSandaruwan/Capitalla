namespace QuickAccounting.Enums
{
    /*
    * Note: 
    * - Do not change the values in this enum as they map directly to the database.
    * - Modifying these values may cause issues with data consistency.
    * - If changes are necessary, consult with the project leader and obtain approval.
    */
    public enum InvoiceStatus
    {
        /// <summary>
        /// Invoice is still being prepared and not yet finalized.
        /// </summary>
        Draft = 1,

        /// <summary>
        /// Invoice has been reviewed and authorized for processing.
        /// </summary>
        Approved = 2,

        /// <summary>
        /// Invoice has been rejected and not approved.
        /// </summary>
        Rejected = 3,

        /// <summary>
        /// Invoice has been canceled and is no longer valid.
        /// </summary>
        Canceled = 4,

        /// <summary>
        /// Invoice has been sent to the customer.
        /// </summary>
        Sent = 5
    }
}
