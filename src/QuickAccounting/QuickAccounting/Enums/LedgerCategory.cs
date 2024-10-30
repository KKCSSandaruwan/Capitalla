namespace QuickAccounting.Enums
{
    /*
    * Note: 
    * - Do not change the values in this enum as they map directly to the database.
    * - Modifying these values may cause issues with data consistency.
    * - If changes are necessary, consult with the project leader and obtain approval.
    */
    public enum LedgerCategory
    {
        /// <summary>
        /// Cash transactions, including inflows and outflows.
        /// </summary>
        Cash = 1,

        /// <summary>
        /// Revenue from sales of products or services.
        /// </summary>
        Sales = 2,

        /// <summary>
        /// Transactions related to purchases of goods and services.
        /// </summary>
        Purchase = 3,

        /// <summary>
        /// Transactions associated with Value Added Tax (VAT).
        /// </summary>
        Vat = 4,

        /// <summary>
        /// Advance payments made for future goods or services.
        /// </summary>
        Advance = 5,

        /// <summary>
        /// Salary payments to employees, including wages and bonuses.
        /// </summary>
        Salary = 6
    }
}
