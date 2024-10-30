﻿namespace QuickAccounting.Enums
{
    public enum LedgerType
    {
        /// <summary>
        /// Accounts representing assets owned by the business.
        /// </summary>
        Asset,

        /// <summary>
        /// Accounts representing liabilities or obligations of the business.
        /// </summary>
        Liability,

        /// <summary>
        /// Accounts representing the owner's equity in the business.
        /// </summary>
        Equity,

        /// <summary>
        /// Accounts representing revenue generated, often linked to sales or services.
        /// </summary>
        Revenue,

        /// <summary>
        /// Accounts representing income generated by the business outside of regular revenue, such as investment income.
        /// </summary>
        Income,

        /// <summary>
        /// Accounts representing expenses incurred by the business.
        /// </summary>
        Expense
    }
}
