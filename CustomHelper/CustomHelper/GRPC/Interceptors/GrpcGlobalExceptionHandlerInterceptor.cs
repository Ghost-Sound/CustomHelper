using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace CustomHelper.GRPC.Interceptors
{
    //https://learn.microsoft.com/en-us/aspnet/core/grpc/interceptors?view=aspnetcore-8.0

    public class GrpcGlobalExceptionHandlerInterceptor(
        ILogger<GrpcGlobalExceptionHandlerInterceptor> logger) : Interceptor
    {
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var call = continuation(request, context);

            return new AsyncUnaryCall<TResponse>(
                HandleResponse(call.ResponseAsync),
                call.ResponseHeadersAsync,
                call.GetStatus,
                call.GetTrailers,
                call.Dispose);
        }

        private async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> inner)
        {
            try
            {
                return await inner;
            }
            catch (RpcException ex)
            {
                //Status codes different from normal http https://grpc.github.io/grpc/core/md_doc_statuscodes.html

                logger.LogError(message: ex.Message, ex.StatusCode);
                throw;
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Custom error", ex);
            }
        }
    }
}
