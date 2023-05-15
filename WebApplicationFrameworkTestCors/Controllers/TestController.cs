using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

// https://learn.microsoft.com/pt-br/aspnet/web-api/overview/security/enabling-cross-origin-requests-in-web-api
// https://idreesdotnet.blogspot.com/2022/05/configure-iis-for-cors-preflight.html
// https://stackoverflow.com/questions/22495240/iis-hijacks-cors-preflight-options-request
// https://stackoverflow.com/questions/49450854/how-to-authorize-cors-preflight-request-on-iis-with-windows-authentication

namespace WebApplicationFrameworkTestCors.Controllers
{
    [EnableCors(origins: "http://xwcteste.xgen.com.br,http://www.example.com", headers: "*", methods: "*")]
    public class TestController : ApiController 
    {
       //[AcceptVerbs("OPTIONS")]
       //public HttpResponseMessage Options()
       //{
       //    var resp = new HttpResponseMessage(HttpStatusCode.OK);
       //    resp.Headers.Add("Access-Control-Allow-Origin", "*");
       //    resp.Headers.Add("Access-Control-Allow-Methods", "GET,DELETE");
       //
       //    return resp;
       //}

        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent("GET: Test message")
            };
        }

        public HttpResponseMessage Post()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent("POST: Test message")
            };
        }

        public HttpResponseMessage Put()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent("PUT: Test message")
            };
        }
    }
}
