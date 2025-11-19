using LybraryManagement.Services;
using LybraryManagement.Shared.Library.DTOs;
using MudBlazor;

namespace LybraryManagement.Pages.Library
{
    public partial class Games
    {
        private List<GameDto> games = new();
        private bool dialogVisible;
        private string dialogTitle = string.Empty;
        private MudForm? form;
        private int itemId;
        private string? platform;
        private string? ageRange;
        private int? editingId;

        protected override async Task OnInitializedAsync() => await LoadAsync();

        private async Task LoadAsync() => games = (await ApiClient.GetAsync<List<GameDto>>(HttpFactory, "api/games")) ?? new();

        private void OpenCreate()
        {
            dialogTitle = "Nouveau jeu";
            editingId = null;
            itemId = 0;
            platform = null;
            ageRange = null;
            dialogVisible = true;
        }

        private void OpenEdit(GameDto dto)
        {
            dialogTitle = $"Modifier le jeu #{dto.GameId}";
            editingId = dto.GameId;
            itemId = dto.ItemId;
            platform = dto.Platform;
            ageRange = dto.AgeRange;
            dialogVisible = true;
        }

        private async Task Save()
        {
            await form!.Validate();
            if (!form.IsValid) return;

            if (editingId is null)
            {
                var resp = await ApiClient.PostAsync(HttpFactory, "api/games", new CreateGameDto(itemId, platform, ageRange));
                if (!resp.IsSuccessStatusCode) { Snackbar.Add("Erreur de création", Severity.Error); return; }
                Snackbar.Add("Jeu créé avec succès", Severity.Success);
            }
            else
            {
                var resp = await ApiClient.PutAsync(HttpFactory, $"api/games/{editingId}", new UpdateGameDto(itemId, platform, ageRange));
                if (!resp.IsSuccessStatusCode) { Snackbar.Add("Erreur de mise à jour", Severity.Error); return; }
                Snackbar.Add("Jeu modifié avec succès", Severity.Success);
            }

            dialogVisible = false;
            await LoadAsync();
        }

        private async Task Delete(GameDto dto)
        {
            var resp = await ApiClient.DeleteAsync(HttpFactory, $"api/games/{dto.GameId}");
            if (!resp.IsSuccessStatusCode) { Snackbar.Add("Suppression échouée", Severity.Error); return; }
            await LoadAsync();
            Snackbar.Add("Jeu supprimé", Severity.Success);
        }
    }
}
