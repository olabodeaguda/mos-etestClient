using MOSEtestClient.Dao;
using MOSEtestClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MOSEtestClient.Services
{
    public class AbstractService
    {
        //protected static HttpClient httpClient = new HttpClient();
        protected AppConfigDao appConfigDao;
        public AbstractService()
        {
            this.appConfigDao = new AppConfigDao();
            profile = appConfigDao.read();
            // RemoteContext();
        }

        private static Profile _profile = new Profile();

        public static Profile profile
        {
            get { return _profile; }
            set
            {
                _profile = value;
            }
        }

        protected HttpClient httpClient
        {
            get
            {

                HttpClient hc = new HttpClient();
                hc.BaseAddress = new Uri(profile.remoteUrl);
                hc.DefaultRequestHeaders.Accept.Clear();
                hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return hc;
            }
        }

        public async Task<bool> PingConnection()
        {
            HttpClient ht = this.httpClient;
            HttpResponseMessage response = await ht.GetAsync("api/health");
            if (response.IsSuccessStatusCode)
            {
                string res = await response.Content.ReadAsStringAsync();
                return bool.Parse(res);
            }

            return false;
        }
    }
}
