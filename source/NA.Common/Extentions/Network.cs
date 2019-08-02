using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using static tci.common.Enums.Enums;

namespace NA.Common.Extentions
{
    public static class Network
    {
        public static string MakeRequest(string regon, string silosId, RaportFiz type, string sid = null)
        {
            try
            {
                return MakeRequest(GetMethodUri(Method.DanePobierzPelnyRaport), Raport.PrepareDataString(Method.DanePobierzPelnyRaport, regon, type.ToString(), silosId), sid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string MakeRequest(string regon, string silosId, RaportPraw type, string sid = null)
        {
            try
            {
                return MakeRequest(GetMethodUri(Method.DanePobierzPelnyRaport), Raport.PrepareDataString(Method.DanePobierzPelnyRaport, regon, type.ToString(), silosId), sid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string MakeRequest(SearchBy search, string[] data, string sid = null)
        {
            try
            {
                return MakeRequest(GetMethodUri(Method.DaneSzukaj), Raport.PrepareSearchString(search, data), sid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string MakeRequest(Method type, string data, string sid = null)
        {
            try
            {
                if (type == Method.DaneSzukaj) throw new ArgumentOutOfRangeException(nameof(type));

                return MakeRequest(GetMethodUri(type), Raport.PrepareDataString(type, data), sid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string MakeRequest(string uri, string data, string sid = null)
        {
            try
            {
                if (!uri.HasValue()) throw new ArgumentNullException(nameof(uri));

                var req = (HttpWebRequest)WebRequest.Create(uri);
                req.Method = "POST";
                req.SendChunked = true;
                req.ContentType = "application/json";
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:45.0) Gecko/20100101 Firefox/45.0";
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req.Timeout = 7000;
                req.AutomaticDecompression = DecompressionMethods.GZip;
                req.ContentLength = !data.HasValue() ? 0 : data.Length;
                if (sid.HasValue())
                    req.Headers.Add("sid", sid);

                var os = new StreamWriter(req.GetRequestStream());
                os.Write(data);
                os.Flush();
                os.Close();
                var resp = (HttpWebResponse)req.GetResponse();
                var respStream = resp.GetResponseStream();
                if (respStream == null) return null;

                var sr = new StreamReader(respStream);
                return sr.ReadToEnd();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string GetMethodUri(Method type)
        {
            return Config.TestServerRequests ? $"{Config.ServiceTestUrl}{type}" : $"{Config.ServiceUrl}{type}";
        }

        public static string GetKluczUzytkownika()
        {
            try
            {
                string data;
                using (var c = new WebClient())
                {
                    data = c.DownloadString(Config.WebAppUrl);
                }

                var rx = new Regex(@"""pKluczUzytkownika"":""(.+)""");

                return rx.Match(data).Groups[1].Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public static class Config
    {
        public const string ServiceUrl = "https://wyszukiwarkaregon.stat.gov.pl/wsBIR/UslugaBIRzewnPubl.svc/ajaxEndpoint/";
        public const string ServiceTestUrl = "https://wyszukiwarkaregontest.stat.gov.pl/wsBIR/UslugaBIRzewnPubl.svc/ajaxEndpoint/";

        public const string WebAppUrl = "https://wyszukiwarkaregon.stat.gov.pl/appBIR/scripts/appbir.js";

        public static readonly Version UtilsMinVersion = new Version("1.3.1.0");
        public static bool TestServerRequests { get; set; }
    }
}
