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
    public class CreditCardService : IRepository<CreditCard>
    {
        BankDbContext _bankDbContext;
        public CreditCardService(BankDbContext bankDbContext)
        {
            _bankDbContext = bankDbContext;
        }
        public async Task CreateAsync(CreditCard entity)
        {
            if (entity != null)
            {
                await _bankDbContext.CreditCards.AddAsync(entity);
                await _bankDbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {

                var creditCard = await _bankDbContext.CreditCards.FindAsync(id);
                _bankDbContext.CreditCards.Remove(creditCard);
                await _bankDbContext.SaveChangesAsync();
            
        }

        public async Task<IEnumerable<CreditCard>> GetAllAsync()
        {
            return await _bankDbContext.CreditCards.ToListAsync();
        }

        public async Task<CreditCard> GetByIdAsync(int id)
        {
            return await _bankDbContext.CreditCards.FindAsync(id);
        }

        public async Task<IEnumerable<CreditCard>> GetListByID(int id)
        {
            // Этот метод вероятно не имеет смысла для CreditCard, так как id - это идентификатор конкретной карты, а не пользователя.
            // Если он нужен для других целей, то его можно реализовать по аналогии с другими методами.
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(CreditCard entity)
        {
            if (entity != null)
            {
                _bankDbContext.CreditCards.Update(entity);
                await _bankDbContext.SaveChangesAsync();
            }
        }
    }
}
