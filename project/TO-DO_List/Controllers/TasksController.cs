using AutoMapper;
using BusinessLogicLayer.Service;
using Microsoft.AspNetCore.Mvc;
using TO_DO_List.Models.Tasks;
using Task = DataAccessLayer.Model.Task;

namespace TO_DO_List.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<TasksController> _logger;
        private readonly IMapper _mapper;

        public TasksController(IMapper mapper, ITaskService taskService, ICategoryService categoryService,
            ILogger<TasksController> logger)
        {
            _mapper = mapper;
            _taskService = taskService;
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet("Tasks/Index/{userId:int}")]
        public IActionResult Index(int userId, int? categoryId, bool? isDone, int? priority)
        {
            _logger.LogInformation("View all tasks for user with id={}.", userId);
            var tasks = _taskService.FindForUser(userId);
            tasks = _taskService.FilterTasks(tasks, categoryId, isDone, priority);

            var taskViewModels = _mapper.Map<IEnumerable<TaskViewModel>>(tasks);
            var categories = _categoryService.FindAll();
            ViewBag.Categories = categories;
            return View(taskViewModels);
        }

        [HttpGet]
        public IActionResult CreateTask()
        {
            var categories = _categoryService.FindAll();

            var model = new TaskCategoryViewModel
            {
                Categories = categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id.ToString(),
                    Name = c.Name
                }).ToList()
            };

            _logger.LogInformation("View create task form.");
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateTask(TaskCategoryViewModel model)
        {
            _logger.LogInformation("Creating task");
            var task = _mapper.Map<Task>(model.Task);
            _taskService.Create(task);
            _logger.LogInformation("Task successfully created.");
            return RedirectToAction("Index", "Tasks", new { userId = 1 });
        }

        [HttpGet]
        public IActionResult CreateSubTask(int parentId)
        {
            var model = new SubTaskViewModel()
            {
                ParentId = parentId
            };
            _logger.LogInformation("View create subtask form.");
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateSubTask(SubTaskViewModel model)
        {
            _logger.LogInformation("Creating subtask");
            var task = _mapper.Map<Task>(model);
            Console.WriteLine(task);
            _taskService.Create(task);
            _logger.LogInformation("Subtask successfully created.");
            return RedirectToAction("Index", "Tasks", new { userId = 1 });
        }

        [HttpPost]
        public IActionResult MarkTask(int taskId, bool isDone)
        {
            _logger.LogInformation("Marking task");
            var task = _taskService.FindById(taskId);
            task.IsDone = isDone;
            _taskService.Update(task);
            _logger.LogInformation("Task was successfully marked.");
            return RedirectToAction("Index", "Tasks", new { userId = 1 });
        }
    }
}