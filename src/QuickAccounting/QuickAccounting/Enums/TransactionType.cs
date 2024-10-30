namespace QuickAccounting.Enums
{
    public enum TransactionType
    {
        /// <summary>
        /// Represents purchases made from suppliers.
        /// </summary>
        SupplierPurchase,

        /// <summary>
        /// Represents returns made to suppliers.
        /// </summary>
        SupplierReturn,

        /// <summary>
        /// Represents sales made to customers.
        /// </summary>
        CustomerSales,

        /// <summary>
        /// Represents returns received from customers.
        /// </summary>
        CustomerReturn,

        /// <summary>
        /// Represents general expenses.
        /// </summary>
        Expense,

        /// <summary>
        /// Represents revenue generated from sales or services.
        /// </summary>
        Revenue,

        /// <summary>
        /// Represents income earned from interest.
        /// </summary>
        InterestIncome,

        /// <summary>
        /// Represents interest paid on debts.
        /// </summary>
        InterestExpense,

        /// <summary>
        /// Represents disbursements of loans.
        /// </summary>
        LoanDisbursement,

        /// <summary>
        /// Represents repayments made towards a loan.
        /// </summary>
        LoanRepayment,

        /// <summary>
        /// Represents purchases of investments.
        /// </summary>
        InvestmentPurchase,

        /// <summary>
        /// Represents sales of investments.
        /// </summary>
        InvestmentSale,

        /// <summary>
        /// Represents income received from dividends.
        /// </summary>
        DividendIncome,

        /// <summary>
        /// Represents contributions made by owners or shareholders.
        /// </summary>
        CapitalContribution,

        /// <summary>
        /// Represents incoming transfers between accounts.
        /// </summary>
        TransferIn,

        /// <summary>
        /// Represents outgoing transfers between accounts.
        /// </summary>
        TransferOut,

        /// <summary>
        /// Represents adjustments made to accounts.
        /// </summary>
        Adjustment
    }
}
