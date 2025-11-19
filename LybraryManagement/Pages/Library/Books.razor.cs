using LybraryManagement.Services;
using LybraryManagement.Shared.Library.DTOs;
using MudBlazor;

namespace LybraryManagement.Pages.Library
{
    public partial class Books
    {
        private List<BookDto> books = new();
        private bool dialogVisible;
        private string dialogTitle = string.Empty;
        private MudForm? form;
        private int itemId;
        private int? genreId;
        private string? isbn;
        private int? editingId;

        protected override async Task OnInitializedAsync() => await LoadAsync();

        private async Task LoadAsync() => books = (await ApiClient.GetAsync<List<BookDto>>(HttpFactory, "api/books")) ?? new();

        private void OpenCreate()
        {
            dialogTitle = "Nouveau livre";
            editingId = null;
            itemId = 0;
            genreId = null;
            isbn = null;
            dialogVisible = true;
        }

        private void OpenEdit(BookDto dto)
        {
            dialogTitle = $"Modifier #" + dto.BookId;
            editingId = dto.BookId;
            itemId = dto.ItemId;
            genreId = dto.GenreId;
            isbn = dto.Isbn;
            dialogVisible = true;
        }

        private async Task Save()
        {
            await form!.Validate();
            if (!form.IsValid) return;

            if (editingId is null)
            {
                var resp = await ApiClient.PostAsync(HttpFactory, "api/books", new CreateBookDto(itemId, genreId, isbn));
                if (!resp.IsSuccessStatusCode) { Snackbar.Add("Erreur de création", Severity.Error); return; }
            }
            else
            {
                var resp = await ApiClient.PutAsync(HttpFactory, $"api/books/{editingId}", new UpdateBookDto(itemId, genreId, isbn));
                if (!resp.IsSuccessStatusCode) { Snackbar.Add("Erreur de mise à jour", Severity.Error); return; }
            }

            dialogVisible = false;
            await LoadAsync();
            Snackbar.Add("Enregistré", Severity.Success);
        }

        private async Task Delete(BookDto dto)
        {
            var resp = await ApiClient.DeleteAsync(HttpFactory, $"api/books/{dto.BookId}");
            if (!resp.IsSuccessStatusCode) { Snackbar.Add("Suppression échouée", Severity.Error); return; }
            await LoadAsync();
            Snackbar.Add("Supprimé", Severity.Success);
        }
    }
}
