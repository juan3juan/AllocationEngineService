using AllocationEngineService.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AllocationEngineService.Controllers
{
    [Route("api/[Controller]")]
    public class AllocationEngineController : Controller
    {
        //[HttpPost("[action]")]
        //public async Task<IActionResult> Strategy([FromBody] DataContract dataContract)
        //{
        //    double Cash = dataContract.CurrentCash;
        //    return Ok("Cash remail :" + Cash / 3);
        //}
        [HttpPost("[action]")]
        public async Task<IActionResult> Strategy([FromBody] DataContract dataContract)
        {
            //DataContract dataContract = JsonConvert.DeserializeObject<DataContract>(dataContractstring);
            Dictionary<string, int> result = new Dictionary<string, int>();

            SecPosition secPosition = dataContract.SecPositions.First();
            string SecurityID = secPosition.SecurityID;
            double cash = dataContract.CurrentCash;
            List<double> priceHistory = secPosition.CurrentPrice;
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
    }
}
