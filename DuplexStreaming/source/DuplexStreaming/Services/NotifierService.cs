using System.ComponentModel;
using System.Diagnostics.Metrics;

using DuplexStreaming;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

namespace DuplexStreaming.Services;
public class NotifierService : Notifier.NotifierBase {
    private readonly ILogger<NotifierService> _logger;
    public NotifierService(ILogger<NotifierService> logger) {
        _logger = logger;
    }

    public override async Task ChatNotification(IAsyncStreamReader<NotificationsRequest> requestStream, IServerStreamWriter<NotificationsResponse> responseStream, ServerCallContext context) {

        while (await requestStream.MoveNext()) {
            var request = requestStream.Current;
          
            var now = Timestamp.FromDateTimeOffset(DateTimeOffset.UtcNow);
            var reply = new NotificationsResponse() {
                Message = $"Hi {request.From}!, You have sent the message \"{request.Message}\" to {request.To}",
                ReceivedAt = now
            };
         
            await responseStream.WriteAsync(reply);
          
        }
    }
}
