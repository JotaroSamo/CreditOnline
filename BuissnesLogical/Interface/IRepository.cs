using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnesLogical.Interface
{
  
        public interface IRepository<T>
        {
            // Создание объекта
            Task CreateAsync(T entity);

            // Получение всех объектов
            Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetListByID(int id);
        // Получение объекта по идентификатору
        Task<T> GetByIdAsync(int id);

            // Обновление объекта
            Task UpdateAsync(T entity);

            // Удаление объекта
            Task DeleteAsync(int id);
        }
    

}
