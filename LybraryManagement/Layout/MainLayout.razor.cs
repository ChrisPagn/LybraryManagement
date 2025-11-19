using MudBlazor;

namespace LybraryManagement.Layout
{
    public partial class MainLayout
    {
        private bool _drawerOpen = true;
        private MudTheme _theme = new();

        // Petit délai pour éviter la fermeture "trop nerveuse"
        private System.Threading.CancellationTokenSource? _leaveCts;

        protected override void OnInitialized()
        {
            // Par défaut fermé ; s'ouvrira via le bouton
            _drawerOpen = false;
        }

        private void DrawerToggle() => _drawerOpen = !_drawerOpen;

        private void HandleNavigation()
        {
            // Ferme après navigation (utile sur mobile)
            if (_drawerOpen)
            {
                _drawerOpen = false;
                StateHasChanged();
            }
        }

        private async void OnDrawerMouseLeave(Microsoft.AspNetCore.Components.Web.MouseEventArgs _)
        {
            // Debounce: si la souris revient vite, on annule
            _leaveCts?.Cancel();
            _leaveCts = new System.Threading.CancellationTokenSource();
            try
            {
                await Task.Delay(150, _leaveCts.Token);
                if (_drawerOpen)
                {
                    _drawerOpen = false;
                    StateHasChanged();
                }
            }
            catch (TaskCanceledException)
            {
                // Ignorer si annulé
            }
        }
    }
}
