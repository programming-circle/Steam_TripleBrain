using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Steam_TripleBrain.CQRS.Command.Game;
using Steam_TripleBrain.CQRS.Handler.Game;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Services;
using TestProject1.Helpers;
using Xunit;
using GameModel = Steam_TripleBrain.Models.Game;

namespace TestProject1.Game
{
    public class UpdateGameTests
    {
        [Fact]
        public async Task Should_Update_Game()
        {
            var context = TestDbContextFactory.Create();

            var gameId = Guid.NewGuid();

            var game = new GameModel
            {
                Id = gameId,
                Name = "Old",
                Description = "Old desc",
                Poster = "/old.jpg",
                Genres = new List<Genre>
                {
                    new Genre
                    {
                        Id = Guid.NewGuid(),
                        Name = "Action"
                    }
                }
            };

            context.Games.Add(game);
            await context.SaveChangesAsync();

            // 🔥 важно: очищаем tracking EF
            context.ChangeTracker.Clear();

            var logger = new Mock<ILogger<UpdateGameHandler>>();

            var fileStorage = new Mock<IFileStorageService>();

            fileStorage
                .Setup(x => x.SaveProductImageFromUriOrPathAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync("/uploads/new.jpg");

            fileStorage
                .Setup(x => x.DeleteAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new UpdateGameHandler(context, logger.Object, fileStorage.Object);

            var command = new UpdateGameCommand
            {
                Id = gameId,
                Name = "Updated",
                Poster = "new.jpg",
                Rating = 9,
                Description = "Updated desc",
                Genres = new List<GenreViewProfile>
                {
                    new GenreViewProfile { Name = "RPG" }
                },
                Price = 200,
                Discount = 5,
                Developer = Guid.NewGuid()
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);

            var updated = await context.Games
                .AsNoTracking()
                .FirstAsync(x => x.Id == gameId);

            Assert.Equal("Updated", updated.Name);
        }
    }
}