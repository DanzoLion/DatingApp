using System.Net;                               // HttpStatusCode
using System;                                      // Exception
using System.Threading.Tasks;          //Task
using Microsoft.AspNetCore.Http; // RequestDelegate
using Microsoft.Extensions.Hosting; // IHostEnvironment
using Microsoft.Extensions.Logging; // ILogger<ExceptionMiddleware>
using API.Errors;                                // ApiException()
using System.Text.Json;                     // JsonSerializerOptions

namespace API.Middleware {
    public class ExceptionMiddleware { //logger is the name of our ILogger Interface Parameter            //IHostEnvironment is the type of environment we are running .. ie production, dev. 
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;                                                           //ILogger allows us to display on the terminal instead of logging out //Exc.Middl. -name of our class
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware (RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env) //RequestDelegate specifies what comes next in the middleware pipeline    
        {
            _env = env;                                                                                                                             // initialise these fields from parameters
            _logger = logger;                                                                                                                   // these fields/parameters are all the things we need
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)                                                  // when we add middleware we have access to HttpContext request that comes in ie the Http Request
        {                                                                                                                                   // exception middleware as at the top of the tree, we then need to catch the exception once the exception is passed back up the stack
            try{                                                                                                                            // here we get context in parameters and passt this onto _next(context)
                await _next(context);                                                                                            // context is the http context
            }
            catch(Exception ex)
            { _logger.LogError(ex, ex.Message);                                                                       //ex.Message logs what actually happened // otherwise our message will return silent, ie we won't see the error returned
            context.Response.ContentType = "application/json";                                          // write out our exception to the response here
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;         // ie 500 error response

            var response = _env.IsDevelopment()                                                                                                // to check what mode we are running in here ie dev. mode
            ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())              // ? in case of null //     // ternary operator to say what we will do if this is dev. mode
            : new ApiException(context.Response.StatusCode, "Internal Server Error");                                    // for the case where we are not in dev. mode, ie we are in production mode
            // next we create a few options becuase we want to send our data back in json format // default, json responses should be returned in camelCase // we serialise the response into a json response
            var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};     // ensures our response goes back as a normal json response
            var json = JsonSerializer.Serialize(response, options);                                                                             // serialize response we created earlier and pass in some options .. option is we want CamelCase

            await context.Response.WriteAsync(json);
        }}
    }
}