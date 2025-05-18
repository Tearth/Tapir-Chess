using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Chess;
using Tapir.Providers.Chess.PaxChess.Chessboard;

namespace Tapir.Providers.Chess.PaxChess
{
    public static class Module
    {
        public static IServiceCollection AddPaxChess(this IServiceCollection services)
        {
            // Board
            services.AddScoped<IBoardFactory, BoardFactory>();
            services.AddScoped<IBoard, Board>();

            return services;
        }
    }
}
