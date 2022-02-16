using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Suframa.Sciex.DataAccess.RestService
{
	public enum HttpVerb
	{
		GET,
		POST,
		PUT,
		DELETE
	}

	public class RestClientBase
	{
		public static string _user { get; set; }
		public static string _pass { get; set; }
		public String encoded { get; set; }

		public RestClientBase()
		{

		}

		public RestClientBase(string user, string pass)
		{
			_user = user;
			_pass = pass;

			encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(user + ":" + pass));
		}
	}
	public class RestClientApi : RestClientBase
	{
		public string EndPoint { get; set; }
		public HttpVerb Method { get; set; }
		public string ContentType { get; set; }
		public string PostData { get; set; }



		public RestClientApi(string endpoint)
		{
			EndPoint = endpoint;
			Method = HttpVerb.GET;
			ContentType = "application/json";
			PostData = "";
		}

		public RestClientApi(string endpoint, HttpVerb method)

		{
			EndPoint = endpoint;
			Method = method;
			ContentType = "application/json";
			PostData = "";
		}

		public RestClientApi(string endpoint, HttpVerb method, string user, string pass) : base(user, pass)

		{
			EndPoint = endpoint;
			Method = method;
			ContentType = "application/json";
			PostData = "";
		}

		public RestClientApi(string endpoint, HttpVerb method, string postData, string user, string pass) : base(user, pass)
		{
			EndPoint = endpoint;
			Method = method;
			ContentType = "application/json";
			PostData = postData;
		}

		public string MakeRequest()
		{
			return MakeRequest("");
		}

		public string MakeRequest(string parameters)
		{
			var request = (HttpWebRequest)WebRequest.Create(EndPoint + parameters);

			if (_user != null && _pass != null && encoded != null)
			{

				request.Headers.Add("Authorization", "Basic " + encoded);
			}

			if (EndPoint.Contains("https"))
			{
				request.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
			}

			request.Method = Method.ToString();
			request.ContentLength = 0;
			request.ContentType = ContentType;

			if (!string.IsNullOrEmpty(PostData) && (Method == HttpVerb.POST || Method == HttpVerb.PUT))
			{
				var encoding = new UTF8Encoding();
				var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(PostData);
				request.ContentLength = bytes.Length;

				using (var writeStream = request.GetRequestStream())
				{
					writeStream.Write(bytes, 0, bytes.Length);
				}
			}

			using (var response = (HttpWebResponse)request.GetResponse())
			{
				var responseValue = string.Empty;

				if (response.StatusCode != HttpStatusCode.OK)
				{
					var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
					throw new ApplicationException(message);
				}

				// grab the response
				using (var responseStream = response.GetResponseStream())
				{
					if (responseStream != null)
						using (var reader = new StreamReader(responseStream))
						{
							responseValue = reader.ReadToEnd();
						}
				}

				return responseValue;
			}
		}

		public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		public string MakePostRequest(Object data)
		{
			var request = (HttpWebRequest)WebRequest.Create(EndPoint);

			if (_user != null && _pass != null && encoded != null)
			{

				request.Headers.Add("Authorization", "Basic " + encoded);
			}

			if (EndPoint.Contains("https"))
			{
				request.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
			}

			request.Method = HttpVerb.POST.ToString();
			request.ContentLength = 0;
			request.ContentType = "application/json";

			if (data != null)
			{
				request.ContentType = "application/json";
				using (var stream = new StreamWriter(request.GetRequestStream()))
				{
					var serialized = JsonConvert.SerializeObject(data);
					stream.Write(serialized);
				}
			}

			using (var response = (HttpWebResponse)request.GetResponse())
			{

				if (response.StatusCode != HttpStatusCode.OK)
				{
					var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
					throw new ApplicationException(message);
				}

				// grab the response
				using (var responseStream = response.GetResponseStream())
				{
					if (responseStream != null)
						using (var reader = new StreamReader(responseStream))
						{
							return reader.ReadToEnd();
						}
				}

				return null;
			}
		}
	}
}
