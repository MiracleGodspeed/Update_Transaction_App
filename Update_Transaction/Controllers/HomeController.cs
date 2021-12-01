using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Update_Transaction.Models;

namespace Update_Transaction.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly TransactionAppContext _context;


        public HomeController(ILogger<TransactionController> logger, TransactionAppContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string fullname, string username)
        {
            //using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                ResponseModel response = new ResponseModel();
                var generateWalletId = Convert.ToString(Guid.NewGuid());
                if (fullname == null || username == null)
                    throw new NullReferenceException("Enter fullname and username");
                var doesUserExist = await _context.USER.Where(x => x.Username == username).FirstOrDefaultAsync();
                if (doesUserExist != null)
                    throw new Exception("Username already taken.");
           
                    User user = new User()
                    {
                        Fullname = fullname,
                        Username = username,
                        UserBalance = 0,
                        WalletID = generateWalletId
                    };
                    _context.Add(user);


                    await _context.SaveChangesAsync();
                    response.NewWalletId = generateWalletId;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.StatusText = "success";
                
                
                
                return Json(response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(ViewModel vModel)
        {
            try
            {
                ViewModel model = new ViewModel();
                if (vModel.WalletAddress == null)
                {
                    ViewBag.ErrorMsg = "Wallet address could not be validated";
                    return View(vModel);
                }

                var doesWalletExist = await _context.USER.Where(x => x.WalletID == vModel.WalletAddress).FirstOrDefaultAsync();
                if(doesWalletExist != null)
                {
                    model.Fullname = doesWalletExist.Fullname;
                    model.Username = doesWalletExist.Username;
                    model.Balance = doesWalletExist.UserBalance;
                    model.WalletAddress = doesWalletExist.WalletID;
                }
                else
                {
                    ViewBag.ErrorMsg = "Wallet address could not be validated";
                    return View(vModel);
                }

                var transactions = await _context.TRANSACTION.Where(x => x.UserId == doesWalletExist.Id)
                  .Select(f => new ThirdPartyApiResponse()
                  {
                      TransactionDate = f.TransactionDate.ToLongDateString(),
                      TransactionType = f.TransactionType,
                      Amount = Convert.ToDecimal(f.TransactionAmount)
                  })
                  .ToListAsync();
                if (transactions != null && transactions.Count > 0)
                {
                    model.TransactionList = transactions;
                }
                TempData["walletAddress"] = vModel.WalletAddress;
                return View(model);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> InitiateTransaction(string amt,  string trans__type)
        {
            try
            {
                ResponseModel response = new ResponseModel();
                ViewModel tModel = new ViewModel();
                var walletAddress = Convert.ToString(TempData["walletAddress"]);
                TempData.Keep();
                if (walletAddress == null || trans__type == null || amt == null)
                    throw new Exception("Specify transaction type and amount");
                User isWalletValid = await _context.USER.Where(x => x.WalletID == walletAddress).FirstOrDefaultAsync();

                if (trans__type.Contains("Deposit") && isWalletValid != null)
                {
                    Transaction userTransaction = new Transaction()
                    {
                        UserId = isWalletValid.Id,
                        TransactionAmount = Convert.ToDecimal(amt),
                        TransactionDate = DateTime.Now,
                        TransactionType = "Deposit"

                    };
                    _context.Add(userTransaction);
                    await _context.SaveChangesAsync();


                   isWalletValid.UserBalance += Convert.ToDecimal(amt);

                   _context.Update(isWalletValid);

                   await _context.SaveChangesAsync();
                }
                else
                {
                    Transaction userTransaction = new Transaction()
                    {
                        User = isWalletValid,
                        TransactionAmount = Convert.ToDecimal(amt),
                        TransactionDate = DateTime.Now,
                        TransactionType = "Withdrawal"

                    };

                    if (Convert.ToDecimal(amt) > isWalletValid.UserBalance)
                    {
                        throw new Exception("Specified withdrawal amount exceeds available balance");
                    }
                    isWalletValid.UserBalance -= Convert.ToDecimal(amt);
                    _context.Add(userTransaction);
                    _context.Update(isWalletValid);
                    await _context.SaveChangesAsync();

                   
                }
                response.StatusCode = StatusCodes.Status200OK;
                response.StatusText = "success";
                response.NewWalletId = walletAddress;
                return Json(response);

               // return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> ThirdPartyApi(string walletAddress)
        {
            try
            {
                ResponseModel response = new ResponseModel();

                var doesWalletExist = await _context.USER.Where(x => x.WalletID == walletAddress).FirstOrDefaultAsync();
                if (doesWalletExist != null)
                {
                    var transactions = await _context.TRANSACTION.Where(x => x.UserId == doesWalletExist.Id)
                   .Select(f => new ThirdPartyApiResponse()
                   {
                       TransactionDate = f.TransactionDate.ToLongDateString(),
                       TransactionType = f.TransactionType,
                       Amount = Convert.ToDecimal(f.TransactionAmount)
                   })
                   //.OrderBy(x => x.TransactionDate)
                   .ToListAsync();

                    if (transactions != null && transactions.Count > 0)
                    {
                        response.TransactionList = transactions;
                        response.Amount = doesWalletExist.UserBalance;
                    }
                }

                return Json(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
