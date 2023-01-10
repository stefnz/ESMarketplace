using System.Net;
using ES.Framework;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;

namespace Marketplace.ClassifiedAds;

public interface IClassifiedAdQueryService_Params {
    Task<IEnumerable<ReadModels.ClassifiedAdListItem>> GetPublishedAds(int page, int pageSize);
    Task<ReadModels.ClassifiedAdDetails> GetPublicClassifiedAd(Guid classifiedAdId);
    Task<IEnumerable<ReadModels.ClassifiedAdListItem>> GetClassifiedAdsOwnedBy(Guid userId, int page, int pageSize);
}

public interface IClassifiedAdQueryService {
    Task<IEnumerable<ReadModels.ClassifiedAdListItem>> Query(QueryModels.GetPublishedClassifiedAds query);
    Task<ReadModels.ClassifiedAdDetails> Query(QueryModels.GetPublicClassifiedAd query);
    Task<IEnumerable<ReadModels.ClassifiedAdListItem>> Query(QueryModels.GetOwnersClassifiedAd query);
}

[Route("/ad-raven")]
public class ClassifiedAdsQueryApi_RavenDb : Controller {
    private static Serilog.ILogger logger = Log.ForContext<ClassifiedAdsQueryApi_RavenDb>();
    private readonly IAsyncDocumentSession session;

    public ClassifiedAdsQueryApi_RavenDb(IAsyncDocumentSession session) => this.session = session;

    [HttpGet]
    [Route("list")]
    public Task<IActionResult> Get(QueryModels.GetPublishedClassifiedAds request)
        => RequestHandler.HandleQuery(() => session.Query(request), logger);

    [HttpGet]
    [Route("myads")]
    public Task<IActionResult> Get(QueryModels.GetOwnersClassifiedAd request)
        => RequestHandler.HandleQuery(() => session.Query(request), logger);

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public Task<IActionResult> Get(QueryModels.GetPublicClassifiedAd request) 
        => RequestHandler.HandleQuery(() => session.Query(request), logger);
}