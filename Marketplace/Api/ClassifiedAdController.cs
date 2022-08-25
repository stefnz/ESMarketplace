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

    public ClassifiedAdController(ClassifiedAdUseCases useCases) => this.useCases = useCases;
    
    [HttpPost]
    public async Task<IActionResult> Post(ClassifiedAdContract.V1.Create request) {
        await useCases.Handle(request);
        return Ok();
    }
}