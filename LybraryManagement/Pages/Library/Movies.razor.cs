using LybraryManagement.Services;
using LybraryManagement.Shared.Library.DTOs;
using MudBlazor;

namespace LybraryManagement.Pages.Library
{
    public partial class Movies
    {
        private List<MovieDto> movies = new();
        private bool dialogVisible;
        private string dialogTitle = string.Empty;
        private MudForm? form;
        private int itemId;
        private string? durationText;
        private string? rating;
        private int? editingId;

        protected override async Task OnInitializedAsync() => await LoadAsync();

        private async Task LoadAsync() => movies = (await ApiClient.GetAsync<List<MovieDto>>(HttpFactory, "api/movies")) ?? new();

        private void OpenCreate()
        {
            dialogTitle = "Nouveau film";
            editingId = null;
            itemId = 0;
            durationText = null;
            rating = null;
            dialogVisible = true;
        }

        private void OpenEdit(MovieDto dto)
        {
            dialogTitle = $"Modifier le film #{dto.MovieId}";
            editingId = dto.MovieId;
            itemId = dto.ItemId;
            durationText = dto.Duration?.ToString(@"hh\:mm\:ss");
            rating = dto.Rating;
            dialogVisible = true;
        }

        private async Task Save()
        {
            await form!.Validate();
            if (!form.IsValid) return;

            var duration = !string.IsNullOrWhiteSpace(durationText) && TimeSpan.TryParse(durationText, out var ts)
                ? ts
                : null as TimeSpan?;

            if (editingId is null)
            {
                var resp = await ApiClient.PostAsync(HttpFactory, "api/movies", new CreateMovieDto(itemId, duration, rating));
                if (!resp.IsSuccessStatusCode) { Snackbar.Add("Erreur de création", Severity.Error); return; }
                Snackbar.Add("Film créé avec succès", Severity.Success);
            }
            else
            {
                var resp = await ApiClient.PutAsync(HttpFactory, $"api/movies/{editingId}", new UpdateMovieDto(itemId, duration, rating));
                if (!resp.IsSuccessStatusCode) { Snackbar.Add("Erreur de mise à jour", Severity.Error); return; }
                Snackbar.Add("Film modifié avec succès", Severity.Success);
            }

            dialogVisible = false;
            await LoadAsync();
        }

        private async Task Delete(MovieDto dto)
        {
            var resp = await ApiClient.DeleteAsync(HttpFactory, $"api/movies/{dto.MovieId}");
            if (!resp.IsSuccessStatusCode) { Snackbar.Add("Suppression échouée", Severity.Error); return; }
            await LoadAsync();
            Snackbar.Add("Film supprimé", Severity.Success);
        }

        private string FormatDuration(TimeSpan duration)
        {
            if (duration.TotalHours >= 1)
                return $"{(int)duration.TotalHours}h{duration.Minutes:00}";
            else
                return $"{duration.Minutes}min";
        }
    }
}
