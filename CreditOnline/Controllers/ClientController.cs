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
    }
}
