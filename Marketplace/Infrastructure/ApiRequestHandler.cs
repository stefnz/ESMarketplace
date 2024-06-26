﻿using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace ES.Framework;

public static class RequestHandler {
    public static async Task<IActionResult> HandleCommand<T>(T request, Func<T, Task> handler, ILogger log) {
        try {
            log.Debug("Handling HTTP request of type {type}", typeof(T).Name);
            await handler(request);
            return new OkResult();
        }
        catch (Exception e) {
            log.Error(e, "Error handling the request");
            return new BadRequestObjectResult(new {
                error = e.Message,
                stackTrace = e.StackTrace
            });
        }
    }

    /*public static IActionResult HandleQuery<TModel>(Func<TModel> query, ILogger log)
    {
        try
        {
            return new OkObjectResult(query());
        }
        catch (Exception e)
        {
            log.Error(e, "Error handling the query");
            return new BadRequestObjectResult(new {
                error = e.Message, 
                stackTrace = e.StackTrace
            });
        }
    }*/
    
    public static async Task<IActionResult> HandleQuery<TModel>(Func<Task<TModel>> query, ILogger log) {
        try {
            return new OkObjectResult(await query());
        }
        catch (Exception e) {
            log.Error(e, "Error handling the query");
            return new BadRequestObjectResult(new {
                error = e.Message,
                stackTrace = e.StackTrace
            });
        }
    }
}