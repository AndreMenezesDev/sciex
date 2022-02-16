using System;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using NLog;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.CrossCutting.Security
{
	public static class JwtManager
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public static TokenDto CreateNew(string cpfCnpj)
		{
			TokenDto usuarioLogado = new TokenDto();
			usuarioLogado.CpfCnpj = cpfCnpj;
			return usuarioLogado;
		}

		/// <summary>
		/// Use the below code to generate symmetric Secret Key
		///     var hmac = new HMACSHA256();
		///     var key = Convert.ToBase64String(hmac.Key);
		/// </summary>
		public static string GenerateToken(UsuarioPssVM payload, int expireMinutes)
		{
			IDateTimeProvider provider = new UtcDateTimeProvider();
			var now = provider.GetNow();

			var unixEpoch = JwtValidator.UnixEpoch; // 1970-01-01 00:00:00 UTC
			var secondsSinceEpoch = Math.Round((now - unixEpoch).TotalSeconds);

			secondsSinceEpoch += expireMinutes * 60;

			//var payload = new Dictionary<string, object>
			//{
			//	{ "CpfCnpj", cpfCnpj },
			//	{ "exp", secondsSinceEpoch }
			//};
			payload.exp = secondsSinceEpoch;

			IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
			IJsonSerializer serializer = new JsonNetSerializer();
			IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
			IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

			var token = encoder.Encode(payload, PrivateSettings.TOKEN_SECRET);
			return token;
		}

		/// <summary>
		/// Obter todas as claims do token
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static UsuarioPssVM GetPrincipal(string token)
		{
			try
			{
				IJsonSerializer serializer = new JsonNetSerializer();
				IDateTimeProvider provider = new UtcDateTimeProvider();
				IJwtValidator validator = new JwtValidator(serializer, provider);
				IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
				IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

				var obj = decoder.DecodeToObject<UsuarioPssVM>(token, PrivateSettings.TOKEN_SECRET, verify: true);
				return obj;
			}
			catch (TokenExpiredException ex)
			{
				logger.Warn(ex, "Token expirado");
				throw new Exception(Resources.Resources.SESSAO_EXPIRADA);
			}
			catch (SignatureVerificationException ex)
			{
				logger.Warn(ex, "Assintatura do Token inválida");
			}

			return null;
		}

		/// <summary>
		/// Obtem o token do Header
		/// </summary>
		/// <returns></returns>
		public static string GetTokenFromHeader()
		{
			try
			{
				if (System.Web.HttpContext.Current.Request != null)
				{
					var req = System.Web.HttpContext.Current.Request;
					string bearerToken = req.Headers["Authorization"];

					if (bearerToken != null)
					{
						// https://tools.ietf.org/html/rfc6750
						var token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
						return token;
					}
				}
			}
			catch
			{
				return null;
			}

			return null;
		}

		/// <summary>
		/// Verifica se o token do Header está válido
		/// </summary>
		/// <returns></returns>
		public static bool IsValid()
		{
			return IsValid(GetTokenFromHeader());
		}

		/// <summary>
		/// Verifica se o token do Header está válido
		/// </summary>
		/// <returns></returns>
		public static bool IsValid(string token)
		{
			if (string.IsNullOrEmpty(token))
				return false;

			var clain = JwtManager.GetPrincipal(token);
			if (clain == null)
				return false;

			return true;
		}
	}
}