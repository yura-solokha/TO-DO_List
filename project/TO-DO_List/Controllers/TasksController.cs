using AutoMapper;
using BusinessLogicLayer.Service;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TO_DO_List.Models.Tasks;
using Task = DataAccessLayer.Model.Task;

namespace TO_DO_List.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<TasksController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public TasksController(IMapper mapper, ITaskService taskService, ICategoryService categoryService,
            ILogger<TasksController> logger, UserManager<User> userManager)
        {
            _mapper = mapper;
            _taskService = taskService;
            _categoryService = categoryService;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId, bool? isDone, int? priority)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return View();
            _logger.LogInformation("View all tasks for user with id={}.", currentUser.Id);
            var tasks = _taskService.FindForUser(currentUser.Id);
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
        public async Task<IActionResult> CreateTask(TaskCategoryViewModel model)
        {
            _logger.LogInformation("Creating task");
            var currentUser = await _userManager.GetUserAsync(User);
            var task = _mapper.Map<Task>(model.Task);
            task.UserId = currentUser!.Id;
            _taskService.Create(task);
            _logger.LogInformation("Task successfully created.");
            return RedirectToAction("Index", "Tasks");
        }

        [HttpGet]
        public IActionResult CreateSubTask(int parentId)
        {
            var model = new SubTaskViewModel
            {
                ParentId = parentId
            };
            _logger.LogInformation("View create subtask form.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubTask(SubTaskViewModel model)
        {
            _logger.LogInformation("Creating subtask");
            var currentUser = await _userManager.GetUserAsync(User);
            var task = _mapper.Map<Task>(model);
            task.UserId = currentUser!.Id;
            _taskService.Create(task);
            _logger.LogInformation("Subtask successfully created.");
            return RedirectToAction("Index", "Tasks");
        }

        [HttpPost]
        public IActionResult MarkTask(int taskId, bool isDone)
        {
            _logger.LogInformation("Marking task");
            var task = _taskService.FindById(taskId);
            task.IsDone = isDone;
            _taskService.Update(task);
            _logger.LogInformation("Task was successfully marked.");
            return RedirectToAction("Index", "Tasks");
        }
    }
}