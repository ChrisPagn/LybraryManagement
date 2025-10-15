using LybraryManagement.Services;
using LybraryManagement.Shared.Library.DTOs;
using MudBlazor;
using System.Net.Http.Json;

namespace LybraryManagement.Pages.Library
{
    public partial class Items
    {
        private enum ItemFilter { All, Books, Games, Movies }

        private List<ItemDto> items = new();
        private HashSet<int> bookItemIds = new();
        private HashSet<int> gameItemIds = new();
        private HashSet<int> movieItemIds = new();
        private List<CategoryDto> categories = new();
        private List<SubcategoryDto> subcategories = new();
        private List<GenreDto> genres = new();
        private ItemFilter currentFilter = ItemFilter.All;

        private bool dialogVisible;
        private string dialogTitle = string.Empty;
        private MudForm? form;

        private string title = string.Empty;
        private string? creator;
        private string? publisher;
        private int? year;
        private string? description;
        private int? subcategoryId;
        private string? imageUrl;
        private int? selectedCategoryId;
        private int? editingId;

        private string itemType = "Book";

        private int? genreId;
        private string? isbn;

        private string? platform;
        private string? ageRange;

        private string? durationText;
        private string? rating;

        private IEnumerable<ItemDto> FilteredItems => currentFilter switch
        {
            ItemFilter.Books => items.Where(i => bookItemIds.Contains(i.ItemId)),
            ItemFilter.Games => items.Where(i => gameItemIds.Contains(i.ItemId)),
            ItemFilter.Movies => items.Where(i => movieItemIds.Contains(i.ItemId)),
            _ => items
        };

        private IEnumerable<SubcategoryDto> FilteredSubcategories => selectedCategoryId.HasValue
            ? subcategories.Where(s => s.CategoryId == selectedCategoryId)
            : subcategories;

        private string GetItemType(int itemId)
        {
            if (bookItemIds.Contains(itemId)) return "Livre";
            if (gameItemIds.Contains(itemId)) return "Jeu";
            if (movieItemIds.Contains(itemId)) return "Film";
            return "Non défini";
        }

        private Color GetTypeColor(int itemId)
        {
            if (bookItemIds.Contains(itemId)) return Color.Primary;
            if (gameItemIds.Contains(itemId)) return Color.Tertiary;
            if (movieItemIds.Contains(itemId)) return Color.Secondary;
            return Color.Default;
        }

        protected override async Task OnInitializedAsync() => await LoadAsync();

        private async Task LoadAsync()
        {
            var allItems = await ApiClient.GetAsync<List<ItemDto>>(HttpFactory, "api/items");
            var books = await ApiClient.GetAsync<List<BookDto>>(HttpFactory, "api/books");
            var games = await ApiClient.GetAsync<List<GameDto>>(HttpFactory, "api/games");
            var movies = await ApiClient.GetAsync<List<MovieDto>>(HttpFactory, "api/movies");
            var categoryList = await ApiClient.GetAsync<List<CategoryDto>>(HttpFactory, "api/categories");
            var subcategoryList = await ApiClient.GetAsync<List<SubcategoryDto>>(HttpFactory, "api/subcategories");
            var genreList = await ApiClient.GetAsync<List<GenreDto>>(HttpFactory, "api/genres");

            items = allItems ?? new List<ItemDto>();
            bookItemIds = books?.Select(b => b.ItemId).ToHashSet() ?? new HashSet<int>();
            gameItemIds = games?.Select(g => g.ItemId).ToHashSet() ?? new HashSet<int>();
            movieItemIds = movies?.Select(m => m.ItemId).ToHashSet() ?? new HashSet<int>();
            categories = categoryList ?? new List<CategoryDto>();
            subcategories = subcategoryList ?? new List<SubcategoryDto>();
            genres = genreList ?? new List<GenreDto>();
        }

        private void OpenCreate()
        {
            dialogTitle = "Nouvel item";
            editingId = null;
            itemType = "Book";
            title = string.Empty;
            creator = null;
            publisher = null;
            year = null;
            description = null;
            subcategoryId = null;
            imageUrl = null;
            selectedCategoryId = null;

            genreId = null;
            isbn = null;
            platform = null;
            ageRange = null;
            durationText = null;
            rating = null;

            dialogVisible = true;
        }

        private async Task OpenEdit(ItemDto dto)
        {
            dialogTitle = $"Modifier l'item #{dto.ItemId}";
            editingId = dto.ItemId;
            title = dto.Title;
            creator = dto.Creator;
            publisher = dto.Publisher;
            year = dto.Year;
            description = dto.Description;
            subcategoryId = dto.SubcategoryId;
            imageUrl = dto.ImageUrl;

            if (subcategoryId.HasValue)
                selectedCategoryId = subcategories.FirstOrDefault(s => s.SubcategoryId == subcategoryId)?.CategoryId;
            else
                selectedCategoryId = null;

            if (bookItemIds.Contains(dto.ItemId))
            {
                itemType = "Book";
                await LoadBookData(dto.ItemId);
            }
            else if (gameItemIds.Contains(dto.ItemId))
            {
                itemType = "Game";
                await LoadGameData(dto.ItemId);
            }
            else if (movieItemIds.Contains(dto.ItemId))
            {
                itemType = "Movie";
                await LoadMovieData(dto.ItemId);
            }

            dialogVisible = true;
        }

        private async Task LoadBookData(int itemId)
        {
            var books = await ApiClient.GetAsync<List<BookDto>>(HttpFactory, "api/books");
            var book = books?.FirstOrDefault(b => b.ItemId == itemId);
            if (book != null)
            {
                genreId = book.GenreId;
                isbn = book.Isbn;
            }
        }

        private async Task LoadGameData(int itemId)
        {
            var games = await ApiClient.GetAsync<List<GameDto>>(HttpFactory, "api/games");
            var game = games?.FirstOrDefault(g => g.ItemId == itemId);
            if (game != null)
            {
                platform = game.Platform;
                ageRange = game.AgeRange;
            }
        }

        private async Task LoadMovieData(int itemId)
        {
            var movies = await ApiClient.GetAsync<List<MovieDto>>(HttpFactory, "api/movies");
            var movie = movies?.FirstOrDefault(m => m.ItemId == itemId);
            if (movie != null)
            {
                durationText = movie.Duration?.ToString();
                rating = movie.Rating;
            }
        }

        private async Task Save()
        {
            await form!.Validate();
            if (!form.IsValid) return;

            var trimmedTitle = title?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(trimmedTitle))
            {
                Snackbar.Add("Le titre est requis", Severity.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(itemType))
            {
                Snackbar.Add("Le type d'item est requis", Severity.Error);
                return;
            }

            int itemId;
            if (editingId is null)
            {
                var itemResponse = await ApiClient.PostAsync(HttpFactory, "api/items", new CreateItemDto(
                    trimmedTitle,
                    string.IsNullOrWhiteSpace(creator) ? null : creator.Trim(),
                    string.IsNullOrWhiteSpace(publisher) ? null : publisher.Trim(),
                    year,
                    string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
                    subcategoryId,
                    string.IsNullOrWhiteSpace(imageUrl) ? null : imageUrl.Trim()
                ));

                if (!itemResponse.IsSuccessStatusCode)
                {
                    Snackbar.Add("Erreur lors de la création de l'item", Severity.Error);
                    return;
                }

                var createdItem = await itemResponse.Content.ReadFromJsonAsync<ItemDto>();
                itemId = createdItem!.ItemId;
            }
            else
            {
                itemId = editingId.Value;
                var itemResponse = await ApiClient.PutAsync(HttpFactory, $"api/items/{itemId}", new UpdateItemDto(
                    trimmedTitle,
                    string.IsNullOrWhiteSpace(creator) ? null : creator.Trim(),
                    string.IsNullOrWhiteSpace(publisher) ? null : publisher.Trim(),
                    year,
                    string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
                    subcategoryId,
                    string.IsNullOrWhiteSpace(imageUrl) ? null : imageUrl.Trim()
                ));

                if (!itemResponse.IsSuccessStatusCode)
                {
                    Snackbar.Add("Erreur lors de la mise à jour de l'item", Severity.Error);
                    return;
                }
            }

            bool specificSuccess = false;
            if (itemType == "Book")
            {
                specificSuccess = await SaveBookData(itemId);
            }
            else if (itemType == "Game")
            {
                specificSuccess = await SaveGameData(itemId);
            }
            else if (itemType == "Movie")
            {
                specificSuccess = await SaveMovieData(itemId);
            }

            if (!specificSuccess) return;

            dialogVisible = false;
            await LoadAsync();
            Snackbar.Add("Item enregistré avec succès", Severity.Success);
        }

        private async Task<bool> SaveBookData(int itemId)
        {
            if (editingId is null)
            {
                var response = await ApiClient.PostAsync(HttpFactory, "api/books", new CreateBookDto(itemId, genreId, isbn));
                if (!response.IsSuccessStatusCode)
                {
                    Snackbar.Add("Erreur lors de la création du livre", Severity.Error);
                    return false;
                }
            }
            else
            {
                var books = await ApiClient.GetAsync<List<BookDto>>(HttpFactory, "api/books");
                var book = books?.FirstOrDefault(b => b.ItemId == itemId);
                if (book != null)
                {
                    var response = await ApiClient.PutAsync(HttpFactory, $"api/books/{book.BookId}", new UpdateBookDto(itemId, genreId, isbn));
                    if (!response.IsSuccessStatusCode)
                    {
                        Snackbar.Add("Erreur lors de la mise à jour du livre", Severity.Error);
                        return false;
                    }
                }
            }
            return true;
        }

        private async Task<bool> SaveGameData(int itemId)
        {
            if (editingId is null)
            {
                var response = await ApiClient.PostAsync(HttpFactory, "api/games", new CreateGameDto(itemId, platform, ageRange));
                if (!response.IsSuccessStatusCode)
                {
                    Snackbar.Add("Erreur lors de la création du jeu", Severity.Error);
                    return false;
                }
            }
            else
            {
                var games = await ApiClient.GetAsync<List<GameDto>>(HttpFactory, "api/games");
                var game = games?.FirstOrDefault(g => g.ItemId == itemId);
                if (game != null)
                {
                    var response = await ApiClient.PutAsync(HttpFactory, $"api/games/{game.GameId}", new UpdateGameDto(itemId, platform, ageRange));
                    if (!response.IsSuccessStatusCode)
                    {
                        Snackbar.Add("Erreur lors de la mise à jour du jeu", Severity.Error);
                        return false;
                    }
                }
            }
            return true;
        }

        private async Task<bool> SaveMovieData(int itemId)
        {
            var duration = !string.IsNullOrWhiteSpace(durationText) && TimeSpan.TryParse(durationText, out var ts)
                ? ts
                : null as TimeSpan?;

            if (editingId is null)
            {
                var response = await ApiClient.PostAsync(HttpFactory, "api/movies", new CreateMovieDto(itemId, duration, rating));
                if (!response.IsSuccessStatusCode)
                {
                    Snackbar.Add("Erreur lors de la création du film", Severity.Error);
                    return false;
                }
            }
            else
            {
                var movies = await ApiClient.GetAsync<List<MovieDto>>(HttpFactory, "api/movies");
                var movie = movies?.FirstOrDefault(m => m.ItemId == itemId);
                if (movie != null)
                {
                    var response = await ApiClient.PutAsync(HttpFactory, $"api/movies/{movie.MovieId}", new UpdateMovieDto(itemId, duration, rating));
                    if (!response.IsSuccessStatusCode)
                    {
                        Snackbar.Add("Erreur lors de la mise à jour du film", Severity.Error);
                        return false;
                    }
                }
            }
            return true;
        }

        private async Task Delete(ItemDto dto)
        {
            var response = await ApiClient.DeleteAsync(HttpFactory, $"api/items/{dto.ItemId}");
            if (!response.IsSuccessStatusCode)
            {
                Snackbar.Add("Suppression échouée", Severity.Error);
                return;
            }

            await LoadAsync();
            Snackbar.Add("Item supprimé avec succès", Severity.Success);
        }

        private void SetFilter(ItemFilter filter)
            => currentFilter = currentFilter == filter ? ItemFilter.All : filter;

        private Variant GetVariant(ItemFilter filter)
            => currentFilter == filter ? Variant.Filled : Variant.Outlined;
    }
}
