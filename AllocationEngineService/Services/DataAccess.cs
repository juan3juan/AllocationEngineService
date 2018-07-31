using AllocationEngineService.Model;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;

namespace AllocationEngineService.Services
{
    public class DataAccess: IDataAccess
    {    
        public Dictionary<string, Security> GetData()
        {
            return DataFromServices();
        }

        private Dictionary<string, Security> DataFromServices()
        {
            var client = new RestClient("https://localhost:44350/api/DataAccess");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("DataFromFile", Method.GET);

            //request.AddJsonBody(JsonConvert.SerializeObject(dataContract));
            //request.AddJsonBody(dataContract);

            Dictionary<string, Security> result = new Dictionary<string, Security>();
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result = JsonConvert.DeserializeObject<Dictionary<string, Security>>(response.Content);
            }

            return result;
        }
    }
}
