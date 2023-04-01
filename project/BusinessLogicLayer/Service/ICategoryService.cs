using System.Collections.Generic;
using DataAccessLayer.Model;

namespace BusinessLogicLayer.Service;

public interface ICategoryService
{
    List<Category> FindAll();

    Category FindById(int id);

    void Remove(int id);
}