using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcGreeter
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override async Task GetStockPrices(GetPriceRequest request, IServerStreamWriter<GetPriceReply> responseStream, ServerCallContext context)
        {
            for (var i = 0; i < 5; ++i)
            {
                await responseStream.WriteAsync(new GetPriceReply()
                {
                    StockId = request.StockId,
                    Price = 100 + i
                });
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        public override async Task<UpdatePriceReply> UpdateStockPrices(IAsyncStreamReader<UpdatePriceRequest> requestStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                var message = requestStream.Current;
                _logger.Log(LogLevel.Information, $"Update Stock: {message.StockId}; Price: {message.Price}");
            }

            return new UpdatePriceReply() { Success = true };
        }

        public override async Task Echo(IAsyncStreamReader<EchoRequest> requestStream, IServerStreamWriter<EchoReply> responseStream, ServerCallContext context)
        {
            // Read requests in a background task.
            var readTask = Task.Run(async () =>
            {
                await foreach (var message in requestStream.ReadAllAsync())
                {
                    // Process request.
                    _logger.Log(LogLevel.Information, $"Client Said: {message.Message}");
                    await responseStream.WriteAsync(new EchoReply() 
                    {
                        Message = message.Message
                    });
                }
            });
    
            // Send responses until the client signals that it is complete.
            while (!readTask.IsCompleted)
            {
                await responseStream.WriteAsync(new EchoReply()
                {
                    Message = "Are you there?"
                });
                await Task.Delay(TimeSpan.FromSeconds(5), context.CancellationToken);
            }
        }
    }
}