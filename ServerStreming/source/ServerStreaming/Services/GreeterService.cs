using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using ServerStreaming;

namespace ServerStreaming.Services;

public class GreeterService : Greeter.GreeterBase {
    private readonly ILogger<GreeterService> _logger;
    private readonly List<GetNotificationsResponse> _notifications;
    public GreeterService(ILogger<GreeterService> logger) {
        _logger = logger;
        
        //Sample data -  simulate DB
        this._notifications = new List<GetNotificationsResponse>();
        for (int i = 0; i < 100; i++) {
            this._notifications.Add(new GetNotificationsResponse() {
                Name = i % 2 == 0 ? "Joe" : "Mary",
                Message = $"Notification {i}",
                Sent = Timestamp.FromDateTimeOffset(DateTimeOffset.Now)
            });
        }
    }

    public override async Task GetNotifications(GetNotificationsRequest request, IServerStreamWriter<GetNotificationsResponse> responseStream, ServerCallContext context) {
        //simulate a db call
        var notifications = this._notifications.Where(m => m.Name == request.Name);

        foreach (var notif in notifications) {
            //write to the response stream
            await responseStream.WriteAsync(notif);
        }
    }
}