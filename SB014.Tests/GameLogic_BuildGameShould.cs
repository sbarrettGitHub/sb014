using Moq;
using SB014.API.BAL;
using SB014.API.Domain;
using Xunit;
using System.Linq;
using SB014.API.DAL;
using System.Collections.Generic;

namespace SB014.UnitTests.Api
{
    public class GameLogic_BuildGameShould
    {
        [Fact]
        public void ReturnAGameWithACollectionOfAnagrams()
        {
            // Arrange
            var wordRepositoryFake = new Mock<IWordRepository>();
            wordRepositoryFake.Setup(p=>p.GetWords(It.IsAny<int>())).Returns(new List<string>{"one", "two"});
            IGameLogic gameLogic = new GameLogic(wordRepositoryFake.Object);

            // Act
            Game game = gameLogic.BuildGame(new System.Guid(), 2);

            // Assert
            Assert.NotEmpty(game.Clues);
        }
        [Fact]
        public void ReturnAGameWithaCollectionOfDistinctAnagrams()
        {
            // Arrange
            var wordRepositoryFake = new Mock<IWordRepository>();
            wordRepositoryFake.Setup(p=>p.GetWords(It.IsAny<int>())).Returns(new List<string>{"one", "two"});
            IGameLogic gameLogic = new GameLogic(wordRepositoryFake.Object);

            // Act
            Game game = gameLogic.BuildGame(new System.Guid(), 2);

            // Assert
            Assert.True(game.Clues.Count == game.Clues.Distinct().Count());
        }

        [Fact]
        public void DrawWordsFromAWordRepository()
        {
            // Arrange
            var wordRepositoryMock = new Mock<IWordRepository>();
            IGameLogic gameLogic = new GameLogic(wordRepositoryMock.Object);

            // Act 
            Game game = gameLogic.BuildGame(new System.Guid(), 2);

            // Assert
             wordRepositoryMock.Verify(mock => mock.GetWords(It.IsAny<int>()), Times.Once()); 
        }
        [Fact]
        public void ScrambleWordsReturnedFromWordRepository()
        {
            // Arrange
            string testWord = "Unscrambled";
            var wordRepositoryFake = new Mock<IWordRepository>();
            wordRepositoryFake.Setup(p=>p.GetWords(It.IsAny<int>())).Returns(new List<string>{testWord});
            IGameLogic gameLogic = new GameLogic(wordRepositoryFake.Object);

            // Act 
            // Build a game with a single anagram
            Game game = gameLogic.BuildGame(new System.Guid(), 1);

            // Assert
            // Test that the anagram created contains all same the letters of the word returned from the repository but not in the same order
            string anagram = game.Clues[0].GameClue;
            bool isScrambled = testWord.All(s=>anagram.Contains(s)) && anagram != testWord;
            Assert.True(isScrambled);
        }
    }
}