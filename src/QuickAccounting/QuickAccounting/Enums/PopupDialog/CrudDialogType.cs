namespace QuickAccounting.Enums.PopupDialog
{
    /// <summary>
    /// Represents the type of CRUD (Create, Read, Update, Delete) operations 
    /// associated with a dialog. This enum is used to define the purpose of 
    /// a dialog, such as viewing, adding, editing, or deleting items.
    /// </summary>
    public enum CrudDialogType
    {
        /// <summary>
        /// Dialog is in view-only mode, no actions can be performed.
        /// </summary>
        View,

        /// <summary>
        /// Dialog is in add mode, used for creating new items.
        /// </summary>
        Add,

        /// <summary>
        /// Dialog is in edit mode, used for modifying existing items.
        /// </summary>
        Edit,

        /// <summary>
        /// Dialog is in delete mode, used for confirming deletions.
        /// </summary>
        Delete
    }
}
