using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicalModel
{
    public class CreditCard
    {
        [Key]
        public int CardID { get; set; } // Уникальный идентификатор карточки

        public string? Name { get; set; }

        public string? Description { get; set; }
        public int Term { get; set; } // Срок кредита в месяцах

        public decimal InterestRate { get; set; } // Процентная ставка

        public decimal MaxAmount { get; set; } // Максимальная сумма кредита
    }
}
