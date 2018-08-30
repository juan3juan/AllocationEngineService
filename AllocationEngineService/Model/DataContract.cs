using System.Collections.Generic;

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
