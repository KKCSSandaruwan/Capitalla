using Microsoft.AspNetCore.Components;

namespace QuickAccounting.Models.UI
{
    public class CrudDialogButton
    {
        public Radzen.ButtonType ButtonType { get; set; } = Radzen.ButtonType.Button;

        public string? Icon { get; set; } = String.Empty;

        public string? Text { get; set; } = String.Empty;

        public bool HasBusyState { get; set; } = false;

        public string BusyText { get; set; } = String.Empty;

        public EventCallback Action { get; set; }
    }
}
