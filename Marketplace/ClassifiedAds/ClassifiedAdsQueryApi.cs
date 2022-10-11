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

[Route("/ad")]
public class ClassifiedAdsQueryApi : Controller {
    private static Serilog.ILogger _log = Log.ForContext<ClassifiedAdsQueryApi>();
    private readonly IAsyncDocumentSession _session;

    public ClassifiedAdsQueryApi(IAsyncDocumentSession session) => _session = session;

    [HttpGet]
    [Route("list")]
    public Task<IActionResult> Get(QueryModels.GetPublishedClassifiedAds request)
        => RequestHandler.HandleQuery(() => _session.Query(request), _log);

    [HttpGet]
    [Route("myads")]
    public Task<IActionResult> Get(QueryModels.GetOwnersClassifiedAd request)
        => RequestHandler.HandleQuery(() => _session.Query(request), _log);

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public Task<IActionResult> Get(QueryModels.GetPublicClassifiedAd request) 
        => RequestHandler.HandleQuery(() => _session.Query(request), _log);
}