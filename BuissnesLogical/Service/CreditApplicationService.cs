using BuissnesLogical.Interface;
using Microsoft.EntityFrameworkCore;
using LogicalModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnesLogical.Service
{
    public class CreditApplicationService : IRepository<CreditAplication>
    {
        BankDbContext _bankDbContext;
        public CreditApplicationService(BankDbContext bankDbContext)
        {
            _bankDbContext = bankDbContext;
        }
        public async Task CreateAsync(CreditAplication entity)
        {
            if (entity != null)
            {
                entity.Status = ApplicationStatus.Ожидание;
                entity.ApplicationDate = DateTime.Now;
                entity.DueDate= DateTime.Now;
                entity.DueDate = entity.DueDate.AddMonths(entity.Term);
                entity.CreditCard = await _bankDbContext.CreditCards.FindAsync(entity.CardID);
                entity.Term = entity.CreditCard.Term;
                entity.User = await _bankDbContext.Users.FindAsync(entity.UserID);
                // Получаем все заявки пользователя
                var userApplications = await _bankDbContext.CreditApplications
                    .Where(c => c.UserID == entity.UserID)
                    .ToListAsync();

                // Вычисляем уровень доверия
                entity.TrustLevel = await CalculateTrustLevelAsync(userApplications);
                await _bankDbContext.CreditApplications.AddAsync(entity);
                await _bankDbContext.SaveChangesAsync();
            }
        }
        public async Task<int> CalculateTrustLevelAsync(List<CreditAplication> userApplications)
        {
            if (userApplications == null || userApplications.Count == 0)
            {
                return 0; // Если нет заявок, уровень доверия равен 0
            }

            decimal totalLoanAmount = userApplications.Sum(c => c.LoanAmount);
            decimal totalTrustLevel = 0;

            foreach (var userApplication in userApplications)
            {
                decimal totalAmountPaid = userApplication.AmountPaid ?? 0;
                decimal paymentPercentage = (totalAmountPaid / totalLoanAmount) * 100;
                int trustLevel = (int)Math.Floor(paymentPercentage / 10);

                if (userApplication.LastDayPaid.HasValue)
                {
                    var daysDifference = (int)(DateTime.Now - userApplication.LastDayPaid.Value).TotalDays;

                    if (daysDifference > 30)
                    {
                        trustLevel -= 5;
                    }
                }

                // Накапливаем значение уровня доверия для всех заявок
                totalTrustLevel += trustLevel;
            }

            // Вычисляем среднее значение уровня доверия
            int averageTrustLevel = (int)Math.Round(totalTrustLevel / userApplications.Count);

            return averageTrustLevel;
        }


        public async Task DeleteAsync(int id)
        {
                 var creditapp = await _bankDbContext.CreditApplications.FindAsync(id);
               _bankDbContext.CreditApplications.Remove(creditapp);
                await _bankDbContext.SaveChangesAsync();
            
        }

        public async Task<IEnumerable<CreditAplication>> GetAllAsync()
        {
          return await _bankDbContext.CreditApplications.Include(u => u.User).Include(c => c.CreditCard).ToListAsync();
        }

        public async Task<CreditAplication> GetByIdAsync(int id)
        {
            return await _bankDbContext.CreditApplications.Include(u => u.User).Include(c => c.CreditCard).FirstOrDefaultAsync(i=>i.ApplicationID==id);
        }

        public async Task<IEnumerable<CreditAplication>> GetListByID(int id)
        {
           return await _bankDbContext.CreditApplications.Where(i=>i.UserID==id).Include(u=>u.User).Include(c=>c.CreditCard).ToListAsync();
        }

        public async Task UpdateAsync(CreditAplication entity)
        {

            if (entity != null)
            {
                if (entity.LoanAmount <= entity.AmountPaid)
                {
                    entity.Status = ApplicationStatus.Оплачен;
                }
                _bankDbContext.CreditApplications.Update(entity);
                await _bankDbContext.SaveChangesAsync();
            }
        }
    }
}
