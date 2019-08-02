using System;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using static tci.common.Enums.Enums;

namespace NA.Common.Extentions
{
    public static class Captcha
    {
        public static bool IsCaptchaNeeded(string sid)
        {
            try
            {
                if (!sid.HasValue()) throw new ArgumentNullException(nameof(sid));

                var response = Network.MakeRequest(Method.GetValue, ServiceStatus.KomunikatKod.ToString(), sid);
                dynamic dane = JsonConvert.DeserializeObject(response);

                return dane.d == KomunikatKod.NeedCaptcha;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetCaptcha(string sid)
        {
            try
            {
                if (!sid.HasValue()) throw new ArgumentNullException(nameof(sid));

                var response = Network.MakeRequest(Method.PobierzCaptcha, null, sid);
                dynamic dane = JsonConvert.DeserializeObject(response);
                return dane.d.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Image Base64ToImage(string base64String)
        {
            try
            {
                if (!base64String.HasValue()) return null;

                var imageBytes = Convert.FromBase64String(base64String);
                using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    var image = Image.FromStream(ms, true);
                    return image;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool ValidateCaptcha(string sid, string input)
        {
            try
            {
                if (!sid.HasValue()) throw new ArgumentNullException(nameof(sid));
                if (!input.HasValue()) throw new ArgumentNullException(nameof(input));

                var response = Network.MakeRequest(Method.SprawdzCaptcha, input, sid);
                dynamic dane = JsonConvert.DeserializeObject(response);
                return Convert.ToBoolean(dane.d.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
