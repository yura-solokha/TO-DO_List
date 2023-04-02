using AutoMapper;
using DataAccessLayer.Model;
using TO_DO_List.Models.Tasks;
using TO_DO_List.Models.User;
using Task = DataAccessLayer.Model.Task;

namespace TO_DO_List.Mappers
{
    public class AppMappingProfile: Profile
    {
        public AppMappingProfile() {
            CreateMap<RegisterViewModel, User>();
            CreateMap<TaskViewModel, Task>().ReverseMap();
            CreateMap<CategoryViewModel, Category>().ReverseMap();
            CreateMap<SubTaskViewModel, Task>().ReverseMap();
        }
    }
}
