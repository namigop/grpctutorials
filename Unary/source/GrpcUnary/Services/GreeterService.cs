using Grpc.Core;

using GrpcUnary;

namespace GrpcUnary.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private List<Customer> _customers = new();

        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
            for (int i = 1; i <= 100; i++)
            {
                var customer = new Customer() { Age = i * 5, FirstName = $"First{i}", LastName=$"Last{i}" };
                _customers.Add(customer);
            }
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override async Task<GetCustomerResponse> GetCustomerByName(GetCustomerRequest request, ServerCallContext context)
        {
            //Simulate a Db call to the Customers database
            var matched = _customers.Where(c => c.FirstName.StartsWith(request.NamePrefix));
            if (matched.Any())
            {
                var response = new GetCustomerResponse();
                response.Customers.AddRange(matched);
                return response;
            }

            throw new RpcException(new Status(StatusCode.NotFound, $"Unable to find a customer with a first name starting with {request.NamePrefix}"));
        }

    }
}
