using BusinessLogicLayer.Service.Impl;
using DataAccessLayer.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using Task = DataAccessLayer.Model.Task;

namespace UnitTests;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _iTaskRepositoryMock;
    private readonly Mock<IEntityRepository<Task>> _taskRepositoryMock;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        _taskRepositoryMock = new Mock<IEntityRepository<Task>>();
        _iTaskRepositoryMock = new Mock<ITaskRepository>();
        var loggerMock = new Mock<ILogger<TaskService>>();
        _taskService = new TaskService(_taskRepositoryMock.Object, _iTaskRepositoryMock.Object, loggerMock.Object);
    }

    [Fact]
    public void FindAll_ReturnsListOfTasks()
    {
        var tasks = new List<Task> { new(), new() };
        _taskRepositoryMock.Setup(repo => repo.FindAll()).Returns(tasks.AsQueryable());

        var result = _taskService.FindAll();

        Assert.Equal(tasks, result);
    }

    [Fact]
    public void FindById_ReturnsTaskById()
    {
        var task = new Task { Id = 1 };
        _taskRepositoryMock.Setup(repo => repo.GetById(1)).Returns(task);

        var result = _taskService.FindById(1);

        Assert.Equal(task, result);
    }

    [Fact]
    public void Create_CallsTaskRepositoryCreate()
    {
        var task = new Task();

        _taskService.Create(task);

        _taskRepositoryMock.Verify(repo => repo.Create(task), Times.Once);
    }

    [Fact]
    public void Delete_CallsTaskRepositoryDelete()
    {
        const int taskId = 1;

        _taskService.Delete(taskId);

        _taskRepositoryMock.Verify(repo => repo.Delete(taskId), Times.Once);
    }

    [Fact]
    public void FindForUser_ReturnsListOfTasks()
    {
        const int userId = 1;
        var tasks = new List<Task> { new(), new() };
        _iTaskRepositoryMock.Setup(repo => repo.GetByUserId(userId)).Returns(tasks.AsQueryable());

        var result = _taskService.FindForUser(userId);

        Assert.Equal(tasks, result);
    }
    
    [Fact]
    public void MarkSubtasks_MarksAllSubtasksAsDone()
    {
        var task = new Task
        {
            Id = 1,
            IsDone = true,
            Subtasks = new List<Task>
            {
                new() { Id = 2, IsDone = false, ParentId = 1},
                new() { Id = 3, IsDone = false, ParentId = 1 }
            }
        };

        _taskService.MarkSubtasks(task);

        Assert.True(task.Subtasks.All(s => s.IsDone));
    }
    
    [Fact]
    public void FilterTasks_FiltersByCategoryId()
    {
        const int categoryId = 1;
        var tasks = new List<Task>
        {
            new() { Id = 1, CategoryId = categoryId },
            new() { Id = 2, CategoryId = 2 }
        };

        var result = _taskService.FilterTasks(tasks, categoryId, null, null);

        Assert.Single(result);
        Assert.Equal(categoryId, result[0].CategoryId);
    }

    [Fact]
    public void FilterTasks_FiltersByIsDone()
    {
        var tasks = new List<Task>
        {
            new() { Id = 1, IsDone = true },
            new() { Id = 2, IsDone = false }
        };

        var result = _taskService.FilterTasks(tasks, null, true, null);

        Assert.Single(result);
        Assert.True(result[0].IsDone);
    }

    [Fact]
    public void FilterTasks_FiltersByPriority()
    {
        const int priority = 2;
        var tasks = new List<Task>
        {
            new() { Id = 1, Priority = priority },
            new() { Id = 2, Priority = 1 }
        };

        var result = _taskService.FilterTasks(tasks, null, null, priority);

        Assert.Single(result);
        Assert.Equal(priority, result[0].Priority);
    }
    
    [Fact]
    public void FilterTasks_FiltersByCategoryIdAndIsDone()
    {
        const int categoryId = 1;
        var tasks = new List<Task>
        {
            new() { Id = 1, CategoryId = categoryId, IsDone = true },
            new() { Id = 2, CategoryId = 2, IsDone = true },
            new() { Id = 3, CategoryId = categoryId, IsDone = false },
            new() { Id = 4, CategoryId = 2, IsDone = false }
        };

        var result = _taskService.FilterTasks(tasks, categoryId, true, null);

        Assert.Single(result);
        Assert.Equal(categoryId, result[0].CategoryId);
        Assert.True(result[0].IsDone);
    }
    [Fact]
    public void FilterTasks_FiltersByCategoryIdAndPriority()
    {
        const int categoryId = 1;
        const int priority = 2;
        var tasks = new List<Task>
        {
            new() { Id = 1, CategoryId = categoryId, Priority = priority },
            new() { Id = 2, CategoryId = 2, Priority = priority },
            new() { Id = 3, CategoryId = categoryId, Priority = 1 },
            new() { Id = 4, CategoryId = 2, Priority = 1 }
        };

        var result = _taskService.FilterTasks(tasks, categoryId, null, priority);

        Assert.Single(result);
        Assert.Equal(categoryId, result[0].CategoryId);
        Assert.Equal(priority, result[0].Priority);
    }

    [Fact]
    public void FilterTasks_FiltersByIsDoneAndPriority()
    {
        const int priority = 2;
        var tasks = new List<Task>
        {
            new() { Id = 1, IsDone = true, Priority = priority },
            new() { Id = 2, IsDone = true, Priority = 1 },
            new() { Id = 3, IsDone = false, Priority = priority },
            new() { Id = 4, IsDone = false, Priority = 1 }
        };

        var result = _taskService.FilterTasks(tasks, null, true, priority);

        Assert.Single(result);
        Assert.True(result[0].IsDone);
        Assert.Equal(priority, result[0].Priority);
    }

    [Fact]
    public void FilterTasks_FiltersByCategoryIdAndIsDoneAndPriority()
    {
        const int categoryId = 1;
        const int priority = 2;
        var tasks = new List<Task>
        {
            new() { Id = 1, CategoryId = categoryId, IsDone = true, Priority = priority },
            new() { Id = 2, CategoryId = 2, IsDone = true, Priority = priority },
            new() { Id = 3, CategoryId = categoryId, IsDone = false, Priority = priority },
            new() { Id = 4, CategoryId = 2, IsDone = false, Priority = priority },
            new() { Id = 5, CategoryId = categoryId, IsDone = true, Priority = 1 },
            new() { Id = 6, CategoryId = 2, IsDone = true, Priority = 1 },
            new() { Id = 7, CategoryId = categoryId, IsDone = false, Priority = 1 },
            new() { Id = 8, CategoryId = 2, IsDone = false, Priority = 1 }
        };

        var result = _taskService.FilterTasks(tasks, categoryId, true, priority);

        Assert.Single(result);
        Assert.Equal(categoryId, result[0].CategoryId);
        Assert.True(result[0].IsDone);
        Assert.Equal(priority, result[0].Priority);
    }
}