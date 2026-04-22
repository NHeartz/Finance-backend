using Finance.Api.Models;

namespace Finance.Api.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetCategoriesAsync(int userId);
    Task<Category> CreateCategoryAsync(Category category, int userId);
    Task<Category?> UpdateCategoryAsync(int id, Category category, int userId);
    Task<bool> DeleteCategoryAsync(int id, int userId);
}