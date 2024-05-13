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
        private readonly IRepository<User> _user;
        public AdminController(IRepository<CreditAplication> creditAplication, IRepository<CreditCard> creditCard, IRepository<User> user)
        {
            _creditAplication = creditAplication;
            _creditCard = creditCard;
            _user = user;
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
        public async Task<IActionResult> HistoryPage(int UserID)
        {
            var creditHistory = await _user.GetByIdAsync(UserID);
            var a = creditHistory.Applications.ToList();
            return View(a);
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
        [HttpGet]
        public async Task<IActionResult> UsersPage()
        {
            var users = await _user.GetAllAsync();
            return View(users);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int UserID)
        {
            await _user.DeleteAsync(UserID);
            return RedirectToAction("UsersPage");
        }
        [HttpPost]
        public async Task<IActionResult> EditUsers(int UserID)
        {
            var users = await _user.GetByIdAsync(UserID);
            return View(users);
        }
        [HttpPost]
        public async Task<IActionResult> SaveEditUsers(User user)
        {
            await _user.UpdateAsync(user);
            return RedirectToAction("UsersPage");
        }
        public async Task<IActionResult> CreditApplication()
        {
            var creditAplications = await _creditAplication.GetAllAsync();
            return View(creditAplications);
        }
        [HttpPost]
        public async Task<IActionResult> CreditApplicationStatus(CreditAplication creditAplication, ApplicationStatus newStatus)
        {
            var creditapp = await _creditAplication.GetByIdAsync(creditAplication.ApplicationID);
            creditapp.Status = newStatus;
            await _creditAplication.UpdateAsync(creditapp);
            return RedirectToAction("CreditApplication");
        }
    }
}
