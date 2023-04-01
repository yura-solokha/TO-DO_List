using BusinessLogicLayer.Service.Impl;
using DataAccessLayer.Repository;
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
        _taskService = new TaskService(_taskRepositoryMock.Object, _iTaskRepositoryMock.Object);
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
}