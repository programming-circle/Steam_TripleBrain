using Xunit;
using Moq;
using Steam_TripleBrain.CQRS.Handler.Game;
using Steam_TripleBrain.CQRS.Query.Game;
using Steam_TripleBrain.Models;
using Microsoft.Extensions.Logging;
using GameModel = Steam_TripleBrain.Models.Game;
using TestProject1.Helpers;

namespace TestProject1.Games
{
    public class GetGameByIdTests
    {
        [Fact]
        public async Task Should_Return_Game_By_Id()
        {
            var context = TestDbContextFactory.Create();

            GameModel game = new GameModel
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Genres = new List<Genre>()
            };

            context.Games.Add(game);
            await context.SaveChangesAsync();

            var logger = new Mock<ILogger<GetGameByIdQueryHandler>>();
            var handler = new GetGameByIdQueryHandler(context, logger.Object);

            var result = await handler.Handle(
                new GetGameByIdQueryRequest { gameId = game.Id },
                CancellationToken.None);

            Assert.True(result.IsSuccess);
        }
    }
}
