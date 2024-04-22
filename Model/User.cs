using System.ComponentModel.DataAnnotations;

namespace LogicalModel
{
    public class User
    {
        [Key]
        public int UserID { get; set; } // Уникальный идентификатор пользователя

        public string Name { get; set; } // Имя пользователя
        public string Password { get; set; }
        public UserType UserTypes { get; set; } // Тип пользователя (админ или клиент)

        // Другие данные о пользователе
        public string? WorkPlace { get; set; } // Место работы
        public decimal? Salary { get; set; } // Зарплата пользователя

        // Список заявок пользователя
        public ICollection<CreditAplication>? Applications { get; set; }
    }
}
