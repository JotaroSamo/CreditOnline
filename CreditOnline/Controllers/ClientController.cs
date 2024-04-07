using BuissnesLogical.Interface;
using LogicalModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CreditOnline.Controllers
{
    public class ClientController : Controller
    {
        private readonly IRepository<CreditAplication> _creditAplication;
        private readonly IRepository<CreditCard> _creditCard;
        private readonly IRepository<User> _user;
        public ClientController(IRepository<CreditAplication> creditAplication, IRepository<CreditCard> creditCard, IRepository<User> user)
        {
            _creditAplication = creditAplication;
            _creditCard = creditCard;
            _user = user;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
        public IActionResult Index()
        {
            HttpContext.Session.SetInt32("UserID", GetCurrentUserId());
            var creditapp = _creditAplication.GetListByID(GetCurrentUserId());
            return View(creditapp);
        }
        [HttpPost]
        public async Task<IActionResult> EditUsers()
        {
            int UserID = GetCurrentUserId();
            var users = await _user.GetByIdAsync(UserID);
            return View(users);
        }
        [HttpPost]
        public async Task<IActionResult> SaveEditUsers(User user)
        {
            await _user.UpdateAsync(user);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> CreditCard()
        { 
            var creditcard = await _creditCard.GetAllAsync();
            return View(creditcard);
        }
        [HttpPost]
        public async Task<IActionResult> ApplyForCredit(int cardId, decimal loanAmount, decimal InterestRate)
        {
            CreditAplication creditAplication = new CreditAplication();
            creditAplication.UserID = GetCurrentUserId();
            creditAplication.CardID = cardId;
            creditAplication.LoanAmount = loanAmount + (loanAmount * (InterestRate/100));
            await _creditAplication.CreateAsync(creditAplication);
            return RedirectToAction("Index");
            
        }
    }
}
