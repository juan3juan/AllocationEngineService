using System;

namespace AllocationEngineService.Model
{
    /// <summary>
    /// Class used to store Security Pricng
    /// </summary>
    public class PricingData
    {
        #region Property
        /// <summary>
        /// Date of the Prcing
        /// </summary>
        public DateTime Date;
        /// <summary>
        /// Market Close Price
        /// </summary>
        public double ClosePrice;
        #endregion Property

        public PricingData(DateTime date, double closePrice)
        {
            Date = date;
            ClosePrice = closePrice;
        }
    }
}
