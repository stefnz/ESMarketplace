using ES.Framework;
using Marketplace.ClassifiedAds;

using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Marketplace.ClassifiedAd {
    [Route("/ad")]
    public class ClassifiedAdsQueryApi : Controller {
        private static Serilog.ILogger log = Log.ForContext<ClassifiedAdsQueryApi>();
        private readonly IEnumerable<ReadModels.ClassifiedAdDetails> items;
        
        public ClassifiedAdsQueryApi(IEnumerable <ReadModels.ClassifiedAdDetails> items) => this.items = items;
        
        [HttpGet]
        public IActionResult Get(QueryModels.GetPublicClassifiedAd request) 
            => RequestHandler.HandleQuery(() => items.Query(request), log);
    }
}