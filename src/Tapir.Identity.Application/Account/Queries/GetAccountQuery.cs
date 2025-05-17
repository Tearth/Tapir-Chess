using Microsoft.AspNetCore.Http;
using System.Security.Claims;
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
        public GetAccountQueryHandler()
        {

        }

        public async Task<GetAccountQueryResult> Process(GetAccountQuery query, ClaimsPrincipal? user)
        {
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
