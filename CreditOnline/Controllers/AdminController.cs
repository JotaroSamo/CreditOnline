using BuissnesLogical.Interface;
using LogicalModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CreditOnline.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRepository<CreditAplication> _creditAplication;
        private readonly IRepository<CreditCard> _creditCard;
        public AdminController(IRepository<CreditAplication> creditAplication, IRepository<CreditCard> creditCard)
        {
            _creditAplication = creditAplication;
            _creditCard = creditCard;
        }
        public async Task<IActionResult> Index()
        {
            var creditcard = await _creditCard.GetAllAsync();
            return View(creditcard);
        }
        public async Task<IActionResult> NewCreditCard()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewCreditCard(CreditCard creditCard)
        {
            await _creditCard.CreateAsync(creditCard);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int CardID)
        {
            await _creditCard.DeleteAsync(CardID);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditCreditCard(int CardID) 
        {
            var creditcard = await _creditCard.GetByIdAsync(CardID);
            return View(creditcard);
        }
        [HttpPost]
        public async Task<IActionResult> SaveEditCreditCard(CreditCard creditCard)
        {
            await _creditCard.UpdateAsync(creditCard);
            return RedirectToAction("Index");
        }
    }
}
