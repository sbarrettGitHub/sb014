using Moq;
using SB014.API.BAL;
using Xunit;

namespace SB014.UnitTests.Api
{
    public class GameLogic_BuildGameShould
    {
        [Fact]
        public void ReturnAGameWithACollectionOfAnagrams()
        {
            // Arrange
            var wordRepositoryFake = new Mock<IWordRepository>();
            IGameLogic gameLogic = new GameLogic();

            // Act
            //GameModel game = gameLogic.BuildGame(new System.Guid());

            // Assert
            //Assert.NotEmpty(game.Anagrams);

        }
    }
}