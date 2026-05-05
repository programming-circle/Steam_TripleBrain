using Xunit;
using Moq;
using Steam_TripleBrain.CQRS.Handler.Game;
using Steam_TripleBrain.CQRS.Query.Game;
using Steam_TripleBrain.Models;
using Microsoft.Extensions.Logging;
using GameModel = Steam_TripleBrain.Models.Game;
using TestProject1.Helpers;

namespace TestProject1.Game
{
    public class GetAllGamesTests
    {
        [Fact]
        public async Task Should_Return_All_Games()
        {
            var context = TestDbContextFactory.Create();

            context.Games.AddRange(
                new GameModel { Id = Guid.NewGuid(), Name = "Game1", Genres = new List<Genre>() },
                new GameModel { Id = Guid.NewGuid(), Name = "Game2", Genres = new List<Genre>() }
            );

            await context.SaveChangesAsync();

            var logger = new Mock<ILogger<GetAllGamesHandler>>();
            var handler = new GetAllGamesHandler(context, logger.Object);

            var result = await handler.Handle(new GetAllGamesQueryRequest(), CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count);
        }
    }
}
