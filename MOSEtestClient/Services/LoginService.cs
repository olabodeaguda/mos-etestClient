using MOSEtestClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MOSEtestClient.Services
{
    public class LoginService : AbstractService
    {

        public async Task<bool> ValidateUser(LoginModel loginModel)
        {
            HttpClient ht = this.httpClient;
            HttpResponseMessage response = await ht.PostAsJsonAsync("api/profile/validate", loginModel);
            Response result = await response.Content.ReadAsAsync<Response>();
            if (response.IsSuccessStatusCode)
            {
                if (result.code == "00")
                {
                    JObject jb = (JObject)result.data;
                    Profile p = jb.ToObject<Profile>();
                    appConfigDao.updateConfig(p);
                    return true;
                }
                else
                {
                    throw new Exception(result.msg);
                }
            }
            else 
            {
                throw new Exception(result.msg);
            }
        }

    }
}
