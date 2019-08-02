using System;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static tci.common.Enums.Enums;

namespace NA.Common.Extentions
{
    internal static class Raport
    {
        public static string PrepareDataString(Method type, params string[] param)
        {
            try
            {
                JObject json;
                switch (type)
                {
                    case Method.Zaloguj:
                        json = JObject.Parse(Jsons.LoginParams);
                        json["pKluczUzytkownika"] = param[0];
                        break;
                    case Method.Wyloguj:
                        json = JObject.Parse(Jsons.LogoutParams);
                        json["pIdentyfikatorSesji"] = param[0];
                        break;
                    case Method.PobierzCaptcha:
                        json = JObject.Parse(Jsons.CaptchaGetParams);
                        break;
                    case Method.SprawdzCaptcha:
                        json = JObject.Parse(Jsons.CaptchaCheckParams);
                        json["pCaptcha"] = param[0];
                        break;
                    case Method.GetValue:
                        json = JObject.Parse(Jsons.GetValueParams);
                        json["pNazwaParametru"] = param[0];
                        break;
                    case Method.DaneKomunikat:
                        json = JObject.Parse(Jsons.DaneKomunikatParams);
                        break;
                    case Method.DanePobierzPelnyRaport:
                        json = JObject.Parse(Jsons.DaneRaportParams);
                        json["pRegon"] = param[0];
                        json["pNazwaRaportu"] = param[1];
                        json["pSilosID"] = param[2];
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type));
                }
                return JsonConvert.SerializeObject(json);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string PrepareSearchString(SearchBy type, string[] param)
        {
            try
            {
                var json = JObject.Parse(Jsons.DaneSzukajParams);
                var prepParam = new StringBuilder();

                if (type == SearchBy.Regony9zn || type == SearchBy.Rregony14zn || type == SearchBy.Krsy || type == SearchBy.Nipy)
                {
                    foreach (var s in param)
                        prepParam.AppendFormat("{0},", s);

                    prepParam = prepParam.Remove(prepParam.Length - 1, 1);
                    var regexp = new Regex(@"^([0-9]+)(,\s*[0-9]+)*$");
                    if (!regexp.IsMatch(prepParam.ToString())) return null;
                }
                else
                    prepParam.Append(param[0]);

                json["pParametryWyszukiwania"][type.ToString()] = prepParam.ToString();
                var paramsString = JsonConvert.SerializeObject(json);

                return paramsString;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetStatusDataString(ServiceStatus type)
        {
            try
            {
                return PrepareDataString(Method.GetValue, type.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetReportDataString(string regon, RaportFiz type, string silosId)
        {
            try
            {
                return PrepareDataString(Method.DanePobierzPelnyRaport, regon, type.ToString(), silosId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetReportDataString(string regon, RaportPraw type, string silosId)
        {
            try
            {
                return PrepareDataString(Method.DanePobierzPelnyRaport, regon, type.ToString(), silosId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public static class Jsons
    {
        public const string LoginParams = "{'pKluczUzytkownika': 'aaaaهbbbbbccccccdd|ه'}";

        public const string LogoutParams = "{'pIdentyfikatorSesji': ''}";

        public const string DaneSzukajParams = @"{
                                                    'pParametryWyszukiwania': {'NazwaPodmiotu': '','AdsSymbolGminy': '','AdsSymbolMiejscowosci': '','AdsSymbolPowiatu': '','AdsSymbolUlicy': '','AdsSymbolWojewodztwa': '','Dzialalnosci': '','PrzewazajacePKD': false,'Regon': '','Krs': '','Nip': '','Regony9zn': '','Regony14zn': '','Krsy': '','Nipy': '','NumerwRejestrzeLubEwidencji': '','OrganRejestrowy': '','RodzajRejestru': '','FormaPrawna': ''},
                                                    'jestWojPowGmn': true
                                                }";

        public const string CaptchaCheckParams = "{'pCaptcha': ''}";

        public const string CaptchaGetParams = "{'': ''}";

        public const string DaneKomunikatParams = "{'': ''}";

        public const string DaneRaportParams = @"{  
                                                    'pRegon': '', 
                                                    'pNazwaRaportu': '', 
                                                    'pSilosID': ''
                                                }";

        public const string GetValueParams = "{'pNazwaParametru': ''}";
    }
}
