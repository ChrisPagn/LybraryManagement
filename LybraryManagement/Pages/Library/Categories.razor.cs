using LybraryManagement.Services;
using LybraryManagement.Shared.Library.DTOs;
using MudBlazor;

namespace LybraryManagement.Pages.Library
{
    public partial class Categories
    {
        private List<CategoryDto> categories = new();
        private bool dialogVisible;
        private string dialogTitle = string.Empty;
        private MudForm? form;
        private string name = string.Empty;
        private string? description;
        private int? editingId;

        protected override async Task OnInitializedAsync() => await LoadAsync();

        private async Task LoadAsync() => categories = (await ApiClient.GetAsync<List<CategoryDto>>(HttpFactory, "api/categories")) ?? new();

        private void OpenCreate()
        {
            dialogTitle = "Nouvelle catégorie";
            editingId = null;
            name = string.Empty;
            description = null;
            dialogVisible = true;
        }

        private void OpenEdit(CategoryDto dto)
        {
            dialogTitle = $"Modifier la catégorie #{dto.CategoryId}";
            editingId = dto.CategoryId;
            name = dto.Name;
            description = dto.Description;
            dialogVisible = true;
        }

        private async Task Save()
        {
            await form!.Validate();
            if (!form.IsValid) return;

            if (editingId is null)
            {
                var resp = await ApiClient.PostAsync(HttpFactory, "api/categories", new CreateCategoryDto(name, description));
                if (!resp.IsSuccessStatusCode) { Snackbar.Add("Erreur de création", Severity.Error); return; }
                Snackbar.Add("Catégorie créée avec succès", Severity.Success);
            }
            else
            {
                var resp = await ApiClient.PutAsync(HttpFactory, $"api/categories/{editingId}", new UpdateCategoryDto(name, description));
                if (!resp.IsSuccessStatusCode) { Snackbar.Add("Erreur de mise à jour", Severity.Error); return; }
                Snackbar.Add("Catégorie modifiée avec succès", Severity.Success);
            }

            dialogVisible = false;
            await LoadAsync();
        }

        private async Task Delete(CategoryDto dto)
        {
            var resp = await ApiClient.DeleteAsync(HttpFactory, $"api/categories/{dto.CategoryId}");
            if (!resp.IsSuccessStatusCode) { Snackbar.Add("Suppression échouée", Severity.Error); return; }
            await LoadAsync();
            Snackbar.Add("Catégorie supprimée", Severity.Success);
        }
    }
}
