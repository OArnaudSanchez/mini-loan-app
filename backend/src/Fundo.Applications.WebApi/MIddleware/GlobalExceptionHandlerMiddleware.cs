using Fundo.Applications.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.MIddleware
{
    public sealed class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogWarning(ex,
                    "Request validation failed. Path: {Path}",
                    context.Request.Path);

                await WriteResponseAsync(
                    context,
                    HttpStatusCode.BadRequest,
                    new
                    {
                        error = "Validation failed",
                        details = ex.Errors.Select(e => new
                        {
                            field = e.PropertyName,
                            message = e.ErrorMessage
                        })
                    });
            }
            catch (Application.Exceptions.ValidationException ex)
            {
                _logger.LogWarning(ex,
                    "Business validation error. Path: {Path}",
                    context.Request.Path);

                await WriteResponseAsync(
                    context,
                    HttpStatusCode.UnprocessableEntity,
                    new
                    {
                        error = ex.Message
                    });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex,
                    "Resource not found. Path: {Path}",
                    context.Request.Path);

                await WriteResponseAsync(
                    context,
                    HttpStatusCode.NotFound,
                    new
                    {
                        error = ex.Message
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled exception. TraceId: {TraceId}",
                    context.TraceIdentifier);

                await WriteResponseAsync(
                    context,
                    HttpStatusCode.InternalServerError,
                    new
                    {
                        error = "An unexpected error occurred",
                        traceId = context.TraceIdentifier
                    });
            }
        }

        private static async Task WriteResponseAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            object body)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(body));
        }
    }
}