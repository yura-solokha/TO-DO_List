using BusinessLogicLayer.Service.Impl;
using DataAccessLayer.Model;
using DataAccessLayer.Repository;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests;

public class CategoryServiceTests
{
    private readonly Mock<IEntityRepository<Category>> _categoryRepositoryMock;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _categoryRepositoryMock = new Mock<IEntityRepository<Category>>();
        var loggerMock = new Mock<ILogger<CategoryService>>();
        _categoryService = new CategoryService(_categoryRepositoryMock.Object, loggerMock.Object);
    }

    [Fact]
    public void FindAll_ReturnsListOfCategories()
    {
        var categories = new List<Category> { new(), new() };
        _categoryRepositoryMock.Setup(repo => repo.FindAll()).Returns(categories.AsQueryable());

        var result = _categoryService.FindAll();

        Assert.Equal(categories, result);
    }

    [Fact]
    public void FindById_ReturnsCategory()
    {
        const int categoryId = 1;
        var category = new Category { Id = categoryId };
        _categoryRepositoryMock.Setup(repo => repo.GetById(categoryId)).Returns(category);

        var result = _categoryService.FindById(categoryId);

        Assert.Equal(category, result);
    }

    [Fact]
    public void Remove_DeletesCategory()
    {
        const int categoryId = 1;

        _categoryService.Delete(categoryId);

        _categoryRepositoryMock.Verify(repo => repo.Delete(categoryId), Times.Once());
    }
}