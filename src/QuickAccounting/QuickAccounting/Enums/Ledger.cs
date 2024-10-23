namespace QuickAccounting.Enums
{
    public enum Ledger
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
