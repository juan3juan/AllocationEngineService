using AllocationEngineService.Model;
using AllocationEngineService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllocationEngineService.Controllers
{
    [Route("api/[Controller]")]
    public class AllocationEngineController : Controller
    {
        private static IDataAccess _dataAccess;

        public AllocationEngineController(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        private static Dictionary<string, Security> securityMaster = null;
        public static Dictionary<string, Security> SecurityMaster
        {
            get
            {
                if (securityMaster == null)
                {
                    // if not new, then the method will become a dead loop with SecurityMaster add
                    securityMaster = _dataAccess.GetData();
                }
                return securityMaster;
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> FlushSecurityMaster()
        {
            return Ok("Flushed");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ExecuteSimpleStrategy([FromBody] DataContract dataContract)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();

            double cash = dataContract.CurrentCash;
            SecPosition secPosition = dataContract.SecPositions.First();
            string SecurityID = secPosition.SecurityID;  
            DateTime currentDate = secPosition.currentDate;
            List<double> priceHistory = GetPriceInrange(SecurityID, currentDate);   
            double currentPrice = priceHistory.Last();
            double previousPrice = priceHistory[priceHistory.Count - 2];
            bool flagBuy = secPosition.PositionQuantity > 0 ? true : false;

            #region logic to buy or sell stock
            if ((previousPrice > currentPrice * 1.03) && flagBuy == false)
            {
                #region Buy
                flagBuy = true;
                int quantity = (int)(cash / currentPrice);
                result.Add(SecurityID, quantity);
                #endregion Buy
            }
            else if (flagBuy == true)
            {
                #region Sell
                if (currentPrice > secPosition.BuyPrice * 1.05)
                {
                    result.Add(SecurityID, -1 * secPosition.PositionQuantity);
                }
                #endregion Sell
            }
            #endregion logic to buy or sell stock
            //string x = JsonConvert.SerializeObject(result);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ExecuteMultiStrategy([FromBody] DataContract dataContract)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();

            double initCash = dataContract.CurrentCash;
            int secNum = dataContract.SecPositions.Where(p => p.PositionQuantity == 0).Count();
            foreach (var sec in dataContract.SecPositions)
            {
                double cash = initCash / secNum;
                string securityID = sec.SecurityID;
                DateTime currentDate = dataContract.SecPositions.First().currentDate;
                List<double> priceHistory = GetPriceInrange(sec.SecurityID, currentDate);
                double currentPrice = priceHistory.Last();
                double previousPrice = priceHistory[priceHistory.Count - 2];
                bool flagBuy = sec.PositionQuantity > 0 ? true : false;

                #region logic to buy or sell stock
                if ((previousPrice > currentPrice * 1.03) && flagBuy == false)
                {
                    #region Buy
                    flagBuy = true;
                    int quantity = (int)(cash / currentPrice);
                    result.Add(sec.SecurityID, quantity);
                    #endregion Buy
                }
                else if (flagBuy == true)
                {
                    #region Sell
                    if (currentPrice > sec.BuyPrice * 1.05)
                    {
                        result.Add(sec.SecurityID, -1 * sec.PositionQuantity);
                    }
                    #endregion Sell
                }
                #endregion logic to buy or sell stock
            }

            return Ok(result);
        }

        private List<double> GetPriceInrange(string secID, DateTime currentDate)
        {
            return SecurityMaster[secID]
                    .SecurityPricingData
                    .Where(p => DateTime.Compare(p.Date, currentDate) <= 0)
                    .Select(p => p.ClosePrice)
                    .ToList();
        }
    }
}
