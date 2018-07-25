using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllocationEngineService.Model
{
    public class DataContract
    {
        public double CurrentCash;
        public List<SecPosition> SecPositions;

        public DataContract()
        {
            SecPositions = new List<SecPosition>();
        }
    }
}
