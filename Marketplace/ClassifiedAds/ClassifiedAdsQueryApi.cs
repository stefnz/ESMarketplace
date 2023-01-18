using ES.Framework;
using Marketplace.ClassifiedAds;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using Serilog;

namespace Marketplace.ClassifiedAd {
    [Route("/ad")]
    public class ClassifiedAdsQueryApi : Controller {
        private static Serilog.ILogger log = Log.ForContext<ClassifiedAdsQueryApi>();
        private readonly IAsyncDocumentSession session;
        
        public ClassifiedAdsQueryApi(IAsyncDocumentSession session) => this.session = session;
        
        [HttpGet]
        public Task<IActionResult> Get(QueryModels.GetPublicClassifiedAd request) 
            => RequestHandler.HandleQuery(() => session.Query(request), log); 
    }
}