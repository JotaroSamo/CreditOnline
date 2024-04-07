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
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetInt32("UserId", GetCurrentUserId());
            var creditapp = await _creditAplication.GetListByID(GetCurrentUserId());
            return View(creditapp);
        }
        [HttpGet]
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
        [HttpPost]
        public async Task<IActionResult> PayCredit(decimal paymentAmount, int applicationId)
        {
            var creditapp = await _creditAplication.GetByIdAsync(applicationId);
            if (creditapp.AmountPaid == null)
            {
                creditapp.AmountPaid = paymentAmount;
            }
            else
            {
                creditapp.AmountPaid += paymentAmount;
            }
            creditapp.LastDayPaid = DateTime.Now;
            await _creditAplication.UpdateAsync(creditapp);
            return RedirectToAction("Index");
        }
      
    }
}
