using Microsoft.AspNetCore.Components;

namespace LybraryManagement.Layout
{
    public partial class NavMenu
    {
        [Parameter] public EventCallback OnNavigation { get; set; }

        private async Task NavigateAndClose()
        {
            // Notifie le parent (MainLayout) pour fermer le drawer après navigation
            if (OnNavigation.HasDelegate)
                await OnNavigation.InvokeAsync();
        }
    }
}
