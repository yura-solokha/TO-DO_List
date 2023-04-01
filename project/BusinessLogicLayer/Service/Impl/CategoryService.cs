using DataAccessLayer.Model;
using DataAccessLayer.Repository;

namespace BusinessLogicLayer.Service.Impl;

public class CategoryService : ICategoryService
{
    private readonly IEntityRepository<Category> _categoryRepository;

    public CategoryService(IEntityRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public List<Category> FindAll()
    {
        return _categoryRepository.FindAll().ToList();
    }

    public Category FindById(int id)
    {
        return _categoryRepository.GetById(id);
    }

    public void Remove(int id)
    {
        _categoryRepository.Delete(id);
    }
}