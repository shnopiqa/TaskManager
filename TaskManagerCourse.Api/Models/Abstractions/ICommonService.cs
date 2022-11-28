using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerCourse.Api.Models.Abstractions
{
    // Интерфейс который надо обязательно создавать при создании сервисов
    public interface ICommonService<T>
    {
        bool Create(T model); // возвращает создался объект или не создался
        bool Update(int id, T model); // принимает id для изменения объекта
        bool Delete(int id);
        T Get(int id);
    }
}
