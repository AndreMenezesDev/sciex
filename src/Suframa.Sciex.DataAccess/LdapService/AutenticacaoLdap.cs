using NLog;
using Suframa.Sciex.CrossCutting.Configuration;
using System;
using System.DirectoryServices;

namespace Suframa.Sciex.DataAccess.LdapService
{
    public class AutenticacaoLdap : IAutenticacaoLdap
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Este método foi importado do sistema arrecadacao (frontend)
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="senha"></param>
        /// <returns></returns>
        public bool Autenticar(string usuario, string senha)
        {
            string AdUrl = PrivateSettings.IP_LDAP;
            bool authentic = false;
            DirectorySearcher deSearch;
            string dados = "";
            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://" + AdUrl,
                    usuario, senha);
                object nativeObject = entry.NativeObject;
                deSearch = new DirectorySearcher(entry);

                deSearch.PropertiesToLoad.Add("department");
                deSearch.PropertiesToLoad.Add("company");
                deSearch.SearchScope = SearchScope.Subtree;
                deSearch.Filter = "(&(objectClass=user)(sAMAccountName=" + usuario + "))";
                SearchResultCollection results = deSearch.FindAll();
                foreach (SearchResult result in results)
                {
                    ResultPropertyCollection props = result.Properties;
                    foreach (String propName in props.PropertyNames)
                    {
                        //Loop properties and pick out company,department
                        dados += (String)props[propName][0];
                    }
                }
                authentic = true;
                return authentic;
            }
            catch (DirectoryServicesCOMException ex)
            {
                logger.Error(ex, "Erro ao tentar acessar o LDAP");
            }

            //DirectorySearcher deSearch = new DirectorySearcher(entry);
            return authentic;
        }
    }
}