using Core.SAML.Library;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace Web.SSO
{
    public class SSOLoginService
    {
        public void ValidateSSO()
        {
            try
            {
                //Sau khi call SSO
                //Goi method tao session

                string identityProvider = ConfigurationManager.AppSettings["identityProvider"];
                string assertionConsumerServiceUrl = ConfigurationManager.AppSettings["assertionConsumerServiceUrl"];
                string serviceProviderIdentity = ConfigurationManager.AppSettings["serviceProviderIdentity"];
                string serviceProviderName = ConfigurationManager.AppSettings["serviceProviderName"];
                string certificateLocation = ConfigurationManager.AppSettings["certificateLocation"];
                string returnLogOffUrl = ConfigurationManager.AppSettings["returnLogOffUrl"];
                X509Certificate2 signingCert = CertificateUtility.GetCertificateForSigning(@certificateLocation, ConfigurationManager.AppSettings["CertPass"]);
                string SAMLRequest = SAML20Assertion.CreateSAML20AuthRequest(serviceProviderIdentity, "Recipient", assertionConsumerServiceUrl, serviceProviderName, "", "", signingCert);
                string returnUserUrl = HttpContext.Current.Request.Url.ToString();
                HttpContext.Current.Response.Clear();
                StringBuilder sb = new StringBuilder();
                sb.Append("<html>");
                sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
                sb.AppendFormat("<form name='form' action='{0}' method='post'>", identityProvider);
                sb.AppendFormat("<input type='hidden' name='SAMLRequest' value='{0}'>", HttpContext.Current.Server.UrlEncode(SAMLRequest));
                sb.AppendFormat("<input type='hidden' name='returnUserUrl' value='{0}'>", returnUserUrl);
                sb.AppendFormat("<input type='hidden' name='returnLogOffUrl' value='{0}'>", returnLogOffUrl);
                sb.Append("</form>");
                sb.Append("</body>");
                sb.Append("</html>");
                HttpContext.Current.Response.Write(sb.ToString());
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
                // do nothing
            }
        }
    }
}