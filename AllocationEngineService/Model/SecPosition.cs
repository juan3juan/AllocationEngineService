using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllocationEngineService.Model
{
    public class SecPosition
    {
        public string SecurityID;
        public int PositionQuantity;
        public double BuyPrice;
        public List<double> CurrentPrice;
    }
}
