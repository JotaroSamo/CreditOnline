using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicalModel
{
    public class CreditAplication
    {
        [Key]
        public int ApplicationID { get; set; } // Уникальный идентификатор заявки

        // Внешний ключ на пользователя
        public int UserID { get; set; }

        // Внешний ключ на карточку кредитных ставок
        public int CardID { get; set; }

        public decimal LoanAmount { get; set; } // Сумма кредита

        public ApplicationStatus Status { get; set; } // Статус заявки

        public int TrustLevel { get; set; } // Уровень доверия пользователя

        // Дата подачи заявки
        public DateTime ApplicationDate { get; set; }

        public int Term { get; set; } // Срок кредита в месяцах

        // Дата окончания срока погашения кредита
        public DateTime DueDate { get; set; }


        // Сумма, которую клиент уже выплатил
        public decimal? AmountPaid { get; set; }

        public DateTime? LastDayPaid { get; set; }

        // Навигационные свойства
        public User User { get; set; }
        public CreditCard CreditCard { get; set; }
    }
}
