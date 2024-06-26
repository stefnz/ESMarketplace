﻿using ES.Framework;
using Marketplace.UserProfiles;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Marketplace.UserProfiles; 

[Route("/profile")]
public class UserProfileCommandsApi : Controller
{
    private readonly UserProfileUseCases useCases;
    private static readonly ILogger Log = Serilog.Log.ForContext<UserProfileCommandsApi>();

    public UserProfileCommandsApi(UserProfileUseCases useCases) => this.useCases = useCases;

    [HttpPost]
    public Task<IActionResult> Post(UserProfileContract.V1.RegisterUser request)
        => RequestHandler.HandleCommand(request, useCases.Handle, Log);
        
    [Route("fullname")]
    [HttpPut]
    public Task<IActionResult> Put(UserProfileContract.V1.UpdateUserFullName request)
        => RequestHandler.HandleCommand(request, useCases.Handle, Log);
        
    [Route("displayname")]
    [HttpPut]
    public Task<IActionResult> Put(UserProfileContract.V1.UpdateUserDisplayName request)
        => RequestHandler.HandleCommand(request, useCases.Handle, Log);
        
    [Route("photo")]
    [HttpPut]
    public Task<IActionResult> Put(UserProfileContract.V1.UpdateUserProfilePhoto request)
        => RequestHandler.HandleCommand(request, useCases.Handle, Log);
}