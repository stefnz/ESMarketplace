using Marketplace.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api; 

/// <summary>
/// Web API facade, accepts HTTP/json quests and deserializes to a command.
/// The command is passed to the appropriate use case to be handled. 
/// </summary>
[Route("/ad")]
public class ClassifiedAdController: Controller {
    private readonly ClassifiedAdUseCases useCases;
    //private readonly ICommandHandler<ClassifiedAdContract.V1.Create> createAdHandler;

    public ClassifiedAdController(ClassifiedAdUseCases useCases) => this.useCases = useCases;

    [HttpPost]
    public async Task<IActionResult> Post(ClassifiedAdContract.V1.Create request) {
        await useCases.Handle(request);
        return Ok();
    }

    [Route("name")]
    [HttpPut]
    public async Task<IActionResult> Put(ClassifiedAdContract.V1.SetTitle request) {
        await useCases.Handle(request);
        return Ok();
    }
    
    [Route("text")]
    [HttpPut]
    public async Task<IActionResult> Put(ClassifiedAdContract.V1.UpdateText request)
    {
        await useCases.Handle(request);
        return Ok();
    }
    [Route("price")]
    [HttpPut]
    public async Task<IActionResult> Put(ClassifiedAdContract.V1.UpdatePrice request)
    {
        await useCases.Handle(request);
        return Ok();
    }
    [Route("publish")]
    [HttpPut]
    public async Task<IActionResult> Put(ClassifiedAdContract.V1.RequestPublish request)
    {
        await useCases.Handle(request);
        return Ok();
    }
}