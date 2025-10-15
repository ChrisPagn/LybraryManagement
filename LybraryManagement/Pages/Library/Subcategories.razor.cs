using LybraryManagement.Services;
using LybraryManagement.Shared.Library.DTOs;
using MudBlazor;

namespace LybraryManagement.Pages.Library
{
    public partial class Subcategories
    {
        private List<SubcategoryDto> subcategories = new();
        private List<CategoryDto> categories = new();
        private bool dialogVisible;
        private string dialogTitle = string.Empty;
        private MudForm? form;
        private int? categoryId;
        private string name = string.Empty;
        private string? description;
        private int? editingId;

        protected override async Task OnInitializedAsync() => await LoadAsync();

        private async Task LoadAsync()
        {
            subcategories = (await ApiClient.GetAsync<List<SubcategoryDto>>(HttpFactory, "api/subcategories")) ?? new();
            categories = (await ApiClient.GetAsync<List<CategoryDto>>(HttpFactory, "api/categories")) ?? new();
        }

        private void OpenCreate()
        {
            dialogTitle = "Nouvelle sous-catégorie";
            editingId = null;
            categoryId = null;
            name = string.Empty;
            description = null;
            dialogVisible = true;
        }

        private void OpenEdit(SubcategoryDto dto)
        {
            dialogTitle = $"Modifier la sous-catégorie #{dto.SubcategoryId}";
            editingId = dto.SubcategoryId;
            categoryId = dto.CategoryId;
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
                var resp = await ApiClient.PostAsync(HttpFactory, "api/subcategories", new CreateSubcategoryDto(categoryId, name, description));
                if (!resp.IsSuccessStatusCode) { Snackbar.Add("Erreur de création", Severity.Error); return; }
                Snackbar.Add("Sous-catégorie créée avec succès", Severity.Success);
            }
            else
            {
                var resp = await ApiClient.PutAsync(HttpFactory, $"api/subcategories/{editingId}", new UpdateSubcategoryDto(categoryId, name, description));
                if (!resp.IsSuccessStatusCode) { Snackbar.Add("Erreur de mise à jour", Severity.Error); return; }
                Snackbar.Add("Sous-catégorie modifiée avec succès", Severity.Success);
            }

            dialogVisible = false;
            await LoadAsync();
        }

        private async Task Delete(SubcategoryDto dto)
        {
            var resp = await ApiClient.DeleteAsync(HttpFactory, $"api/subcategories/{dto.SubcategoryId}");
            if (!resp.IsSuccessStatusCode) { Snackbar.Add("Suppression échouée", Severity.Error); return; }
            await LoadAsync();
            Snackbar.Add("Sous-catégorie supprimée", Severity.Success);
        }

        private string GetCategoryName(int categoryId)
        {
            var category = categories.FirstOrDefault(c => c.CategoryId == categoryId);
            return category?.Name ?? "Inconnue";
        }
    }
}
