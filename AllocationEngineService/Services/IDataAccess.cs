using AllocationEngineService.Model;
using System.Collections.Generic;

namespace AllocationEngineService.Services
{
    public interface IDataAccess
    {
        Dictionary<string, Security> GetData();
    }
}
