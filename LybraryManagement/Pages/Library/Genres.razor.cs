using LybraryManagement.Services;
using LybraryManagement.Shared.Library.DTOs;
using MudBlazor;

namespace LybraryManagement.Pages.Library
{
    public partial class Genres
    {
        private List<GenreDto> genres = new();
        private bool dialogVisible;
        private string dialogTitle = string.Empty;
        private MudForm? form;
        private string name = string.Empty;
        private int? editingId;

        protected override async Task OnInitializedAsync() => await LoadAsync();

        private async Task LoadAsync() => genres = (await ApiClient.GetAsync<List<GenreDto>>(HttpFactory, "api/genres")) ?? new();

        private void OpenCreate()
        {
            dialogTitle = "Nouveau genre";
            editingId = null;
            name = string.Empty;
            dialogVisible = true;
        }

        private void OpenEdit(GenreDto dto)
        {
            dialogTitle = $"Modifier le genre #{dto.GenreId}";
            editingId = dto.GenreId;
            name = dto.Name;
            dialogVisible = true;
        }

        private async Task Save()
        {
            await form!.Validate();
            if (!form.IsValid) return;

            if (editingId is null)
            {
                var resp = await ApiClient.PostAsync(HttpFactory, "api/genres", new CreateGenreDto(name));
                if (!resp.IsSuccessStatusCode) { Snackbar.Add("Erreur de création", Severity.Error); return; }
                Snackbar.Add("Genre créé avec succès", Severity.Success);
            }
            else
            {
                var resp = await ApiClient.PutAsync(HttpFactory, $"api/genres/{editingId}", new UpdateGenreDto(name));
                if (!resp.IsSuccessStatusCode) { Snackbar.Add("Erreur de mise à jour", Severity.Error); return; }
                Snackbar.Add("Genre modifié avec succès", Severity.Success);
            }

            dialogVisible = false;
            await LoadAsync();
        }

        private async Task Delete(GenreDto dto)
        {
            var resp = await ApiClient.DeleteAsync(HttpFactory, $"api/genres/{dto.GenreId}");
            if (!resp.IsSuccessStatusCode) { Snackbar.Add("Suppression échouée", Severity.Error); return; }
            await LoadAsync();
            Snackbar.Add("Genre supprimé", Severity.Success);
        }
    }
}
