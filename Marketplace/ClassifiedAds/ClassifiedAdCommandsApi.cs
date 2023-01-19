using Marketplace.ClassifiedAds;
using Marketplace.Contracts;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Marketplace.Api; 

/// <summary>
/// Web API facade, accepts json requests and deserializes to a command.
/// The command is passed to the appropriate use case to be handled. 
/// </summary>
[Route("/ad")]
public class ClassifiedAdCommandsApi: Controller {
    private readonly ClassifiedAdsUseCases useCases;

    private static Serilog.ILogger Log = Serilog.Log.ForContext<ClassifiedAdCommandsApi>();
    //private readonly ICommandHandler<ClassifiedAdContract.V1.Create> createAdHandler;

    public ClassifiedAdCommandsApi(ClassifiedAdsUseCases useCases) => this.useCases = useCases;

    [HttpPost]
    public Task<IActionResult> Post(ClassifiedAdContract.V1.Create request) => HandleRequest(request, useCases.Handle);

    [Route("name")]
    [HttpPut]
    public Task<IActionResult> Put(ClassifiedAdContract.V1.SetTitle request) => HandleRequest(request, useCases.Handle);
    
    [Route("text")]
    [HttpPut]
    public Task<IActionResult> Put(ClassifiedAdContract.V1.UpdateText request) => HandleRequest(request, useCases.Handle);

    [Route("price")]
    [HttpPut]
    public Task<IActionResult> Put(ClassifiedAdContract.V1.UpdatePrice request) => HandleRequest(request, useCases.Handle); 

    [Route("publish")]
    [HttpPut]
    public Task<IActionResult> Put(ClassifiedAdContract.V1.RequestPublish request) => HandleRequest(request, useCases.Handle);
   
    private async Task<IActionResult> HandleRequest<T>(T request, Func<T, Task> handler) {
        try {
            //Log.Debug("Handling HTTP request of type {type}", typeof(T).Name);
            await handler(request);
            return Ok();
        }
        catch(Exception e) {
            //Log.Error("Error handling request.", e);
            return new BadRequestObjectResult(new {
                error = e.Message,
                stackTrace = e.StackTrace
            });
        }
    }
}