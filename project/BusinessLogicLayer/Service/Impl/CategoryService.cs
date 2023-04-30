using DataAccessLayer.Model;
using DataAccessLayer.Repository;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Service.Impl;

public class CategoryService : ICategoryService
{
    private readonly ILogger<CategoryService> _logger;
    private readonly IEntityRepository<Category> _categoryRepository;

    public CategoryService(IEntityRepository<Category> categoryRepository, ILogger<CategoryService> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    public List<Category> FindAll()
    {
        _logger.LogInformation("Found all categories.");
        return _categoryRepository.FindAll().ToList();
    }

    public Category FindById(int id)
    {
        _logger.LogInformation($"Get category with id={id}.");
        return _categoryRepository.GetById(id);
    }

    public void Delete(int id)
    {
        _logger.LogInformation($"Delete category with id={id}.");
        _categoryRepository.Delete(id);
    }
}