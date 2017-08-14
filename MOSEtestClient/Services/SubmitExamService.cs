using MOSEtestClient.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MOSEtestClient.Services
{
    public class SubmitExamService:AbstractService
    {
        public async Task<bool> SubmitExam(SubmitModel submitModel)
        {
            HttpClient ht = this.httpClient;
            HttpResponseMessage response = await ht.PostAsJsonAsync("api/SubmitAnswer", submitModel);
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
