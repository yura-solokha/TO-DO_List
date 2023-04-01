using DataAccessLayer.DataContext;
using DataAccessLayer.Model;

namespace DataAccessLayer.Repository.Impl;

public class CategoryRepository : IEntityRepository<Category>
{
    private readonly TodoListContext _context;

    public CategoryRepository(TodoListContext context)
    {
        _context = context;
    }

    public IQueryable<Category> FindAll()
    {
        return _context.Categories;
    }

    public void Create(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var category = _context.Categories.Find(id);

        if (category == null) return;
        _context.Categories.Remove(category);
        _context.SaveChanges();
    }

    public Category GetById(int id)
    {
        return _context.Categories.Find(id)!;
    }
}