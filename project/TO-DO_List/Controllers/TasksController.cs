using System.Reflection.Metadata;
using AutoMapper;
using BusinessLogicLayer.Service;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Mvc;
using TO_DO_List.Models.Tasks;
using Task = DataAccessLayer.Model.Task;

namespace TO_DO_List.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public TasksController(IMapper mapper, ITaskService taskService, ICategoryService categoryService)
        {
            _mapper = mapper;
            _taskService = taskService;
            _categoryService = categoryService;
        }

        [HttpGet("Tasks/Index/{userId:int}")]
        public IActionResult Index(int userId)
        {
            var tasks = _taskService.FindForUser(userId);
            var taskViewModels = _mapper.Map<IEnumerable<TaskViewModel>>(tasks);
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

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateTask(TaskCategoryViewModel model)
        {
            var task = _mapper.Map<Task>(model.Task);
            _taskService.Create(task);
            return RedirectToAction("Index", "Tasks", new { userId = 1 });
        }

        [HttpGet]
        public IActionResult CreateSubTask(int parentId)
        {
            Console.WriteLine(parentId);
            var model = new SubTaskViewModel()
            {
                ParentId = parentId
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateSubTask(SubTaskViewModel model)
        {
            var task = _mapper.Map<Task>(model);
            Console.WriteLine(task);
            _taskService.Create(task);
            return RedirectToAction("Index", "Tasks",new { userId = 1 });
        }
    }
}