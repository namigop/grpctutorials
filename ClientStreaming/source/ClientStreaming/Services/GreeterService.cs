using ClientStreaming;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

namespace ClientStreaming.Services;
public class GreeterService : Greeter.GreeterBase {
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger) {
        _logger = logger;
    }

    public override async  Task<NotificationsResponse> SendNotifications(IAsyncStreamReader<NotificationsRequest> requestStream, ServerCallContext context) {
        int total = 0;

        List<NotificationsRequest> messages = new();
        while (await requestStream.MoveNext()) {
            var request = requestStream.Current;
            messages.Add(request);
            Console.WriteLine($"Message \"{request.Message}\" sent to {request.To}");
        }

        var recipients = messages.Select(c => c.To).Distinct().ToList();
        return new NotificationsResponse() { 
            Message = $"{string.Join(",", recipients)} received messages", 
            ReceivedAt = Timestamp.FromDateTimeOffset( DateTimeOffset.UtcNow ),
            Total = total
        };
    }
}
