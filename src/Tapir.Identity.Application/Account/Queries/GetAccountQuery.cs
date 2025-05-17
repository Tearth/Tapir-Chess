using Microsoft.AspNetCore.Http;
using Tapir.Core.Commands;
using Tapir.Core.Identity;
using Tapir.Identity.Infrastructure.Commands;

namespace Tapir.Identity.Application.Account.Commands
{
    public class GetAccountQuery
    {

    }

    public class GetAccountQueryResult : CommandResultBase<GetAccountQueryResult>
    {
        public Guid? Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
    }

    public interface IGetAccountQueryHandler : ICommandHandler<GetAccountQuery, GetAccountQueryResult>
    {

    }

    public class GetAccountQueryHandler : IGetAccountQueryHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAccountQueryHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetAccountQueryResult> Process(GetAccountQuery request)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            return new GetAccountQueryResult
            {
                Success = true,
                Id = user?.GetId(),
                Username = user?.GetName(),
                Email = user?.GetEmail(),
            };
        }
    }
}
