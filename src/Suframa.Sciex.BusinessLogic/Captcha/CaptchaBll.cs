using Newtonsoft.Json;
using Suframa.Sciex.CrossCutting.Configuration;
using System.Collections.Generic;
using System.Net;

namespace Suframa.Sciex.BusinessLogic
{
    public class CaptchaBll : ICaptchaBll
    {
        public bool IsValid(string token)
        {
            if (new PublicSettings().BYPASS_CAPTCHA)
                return true;

            try
            {
                using (var client = new WebClient())
                {
                    var url = string.Format("{0}?secret={1}&response={2}",
                        PrivateSettings.CAPTCHA_URL,
                        PrivateSettings.CAPTCHA_SECRET_KEY,
                        token);

                    var result = client.DownloadString(url);

                    var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(result);
                    return captchaResponse.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        private class CaptchaResponse
        {
            [JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }

            [JsonProperty("success")]
            public bool Success { get; set; }
        }
    }
}