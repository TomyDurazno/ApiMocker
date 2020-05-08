using System.Web.Http;
using System.Web.Http.Cors;

namespace APIMocker.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BillingController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post(BillingDTO dto) => Ok(string.Empty);

        public class BillingDTO
        {
            public int ProductId { get; set; }
            public string Reason { get; set; }
        }
    }
}
