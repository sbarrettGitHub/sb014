using Moq;
using SB014.API.BAL;
using SB014.API.Domain;
using Xunit;
using System.Linq;
using SB014.API.DAL;
using System.Collections.Generic;
using SB014.API.Helpers;
using System;

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
            var DateTimeHelperFake = new Mock<DateTimeHelper>();
            IGameLogic gameLogic = new GameLogic(wordRepositoryFake.Object, DateTimeHelperFake.Object);

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
            var DateTimeHelperFake = new Mock<DateTimeHelper>();
            IGameLogic gameLogic = new GameLogic(wordRepositoryFake.Object, DateTimeHelperFake.Object);

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
            var DateTimeHelperFake = new Mock<DateTimeHelper>();
            IGameLogic gameLogic = new GameLogic(wordRepositoryMock.Object, DateTimeHelperFake.Object);

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
            var DateTimeHelperFake = new Mock<DateTimeHelper>();
            IGameLogic gameLogic = new GameLogic(wordRepositoryFake.Object, DateTimeHelperFake.Object);

            // Act 
            // Build a game with a single anagram
            Game game = gameLogic.BuildGame(new System.Guid(), 1);

            // Assert
            // Test that the anagram created contains all same the letters of the word returned from the repository but not in the same order
            string anagram = game.Clues[0].GameClue;
            bool isScrambled = testWord.All(s=>anagram.Contains(s)) && anagram != testWord;
            Assert.True(isScrambled);
        }
        [Fact]
        public void SetTheCurrentDateAndTimeOnGame()
        {
            // Arrange
            var wordRepositoryFake = new Mock<IWordRepository>();
            var DateTimeHelperFake = new Mock<DateTimeHelper>();
            DateTime currentDateTime = new System.DateTime(2100,1,1);
            DateTimeHelperFake.SetupGet<DateTime>(x=>x.CurrentDateTime).Returns(currentDateTime);
            IGameLogic gameLogic = new GameLogic(wordRepositoryFake.Object, DateTimeHelperFake.Object);

            // Act 
            Game game = gameLogic.BuildGame(new System.Guid(), 2);

            // Assert
            Assert.Equal(game.Created, currentDateTime);
        }
    }
}