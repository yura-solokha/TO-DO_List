using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Model;

namespace DataAccessLayer.Repository
{
    public interface IEntityRepository<T>
        where T : EntityBase
    {
        T GetById(int id);

        void Create(T entity);

        void Delete(int id);

        IQueryable<T> FindAll();
    }
}
