using Xunit;
using Moq;
using Steam_TripleBrain.CQRS.Handler.Game;
using Steam_TripleBrain.CQRS.Command.Game;
using Steam_TripleBrain.Services;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Models;
using Microsoft.Extensions.Logging;
using TestProject1.Helpers;

namespace TestProject1.Game
{
    public class CreateGameTests
    {
        [Fact]
        public async Task Should_Create_Game()
        {
            var context = TestDbContextFactory.Create();

            var logger = new Mock<ILogger<CreateGameHandler>>();
            var fileStorage = new Mock<IFileStorageService>();

            fileStorage
                .Setup(x => x.SaveProductImageFromUriOrPathAsync(It.IsAny<string>(), default))
                .ReturnsAsync("/uploads/test.jpg");

            var handler = new CreateGameHandler(context, logger.Object, fileStorage.Object);

            var command = new CreateGameCommand
            {
                Name = "Test Game",
                Poster = "http://img.com/test.jpg",
                Rating = 8,
                Description = "Test",
                Genres = new List<GenreViewProfile> { new() { Name = "Action" } },
                Price = 100,
                Discount = 0,
                Developer = Guid.NewGuid()
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }
    }
}
