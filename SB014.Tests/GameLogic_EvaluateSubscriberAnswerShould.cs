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
    public class GameLogic_EvaluateSubscriberAnswerShould
    {
        [Fact]
        public void EvaluateAsCorrect_WhenSuppliedAnswerMatchesClueAnswer()
        {
            // Arrange
            var wordRepositoryFake = new Mock<IWordRepository>();
            wordRepositoryFake.Setup(p=>p.GetWords(It.IsAny<int>())).Returns(new List<string>{"one", "two"});
            var DateTimeHelperFake = new Mock<DateTimeHelper>();
            IGameLogic gameLogic = new GameLogic(wordRepositoryFake.Object, DateTimeHelperFake.Object);
            string answer = "answer";
            Clue clue = new Clue {Answer = answer};
            int score;
            // Act
            bool isCorrect = gameLogic.EvaluateSubscriberAnswer(answer, clue, out score);

            // Assert
            Assert.True(isCorrect);
        }
        [Fact]
        public void EvaluateAsIncorrect_WhenSuppliedAnswerDoesNotMatchesClueAnswer()
        {
            // Arrange
            var wordRepositoryFake = new Mock<IWordRepository>();
            wordRepositoryFake.Setup(p=>p.GetWords(It.IsAny<int>())).Returns(new List<string>{"one", "two"});
            var DateTimeHelperFake = new Mock<DateTimeHelper>();
            IGameLogic gameLogic = new GameLogic(wordRepositoryFake.Object, DateTimeHelperFake.Object);
            string answer = "WrongAnswer";
            Clue clue = new Clue {Answer = "answer"};
            int score;
            // Act
            bool isCorrect = gameLogic.EvaluateSubscriberAnswer(answer, clue, out score);

            // Assert
            Assert.False(isCorrect);
        }
        [Fact]
        public void DisregardCase_WhenSuppliedAnswerAndClueAnswerDifferInCase()
        {
            // Arrange
            var wordRepositoryFake = new Mock<IWordRepository>();
            wordRepositoryFake.Setup(p=>p.GetWords(It.IsAny<int>())).Returns(new List<string>{"one", "two"});
            var DateTimeHelperFake = new Mock<DateTimeHelper>();
            IGameLogic gameLogic = new GameLogic(wordRepositoryFake.Object, DateTimeHelperFake.Object);
            string answer = "answer";
            Clue clue = new Clue {Answer = "AnSwER"};
            int score;
            // Act
            bool isCorrect = gameLogic.EvaluateSubscriberAnswer(answer, clue, out score);

            // Assert
            Assert.True(isCorrect);
        }
        [Fact]
        public void CalculateAnswerScore_WhenAnswerIsCorrect()
        {
            // Arrange
            var wordRepositoryFake = new Mock<IWordRepository>();
            var DateTimeHelperFake = new Mock<DateTimeHelper>();
            IGameLogic gameLogic = new GameLogic(wordRepositoryFake.Object, DateTimeHelperFake.Object);
            string answer = "answer";
            Clue clue = new Clue {Answer = answer};
            int score;
            // Act
            bool isCorrect = gameLogic.EvaluateSubscriberAnswer(answer, clue, out score);

            // Assert
            Assert.True(score > 0);
        }
        [Fact]
        public void CalculateAnswerScoreAsTheLengthOfTheAnswer_WhenAnswerIsCorrect()
        {
            // Arrange
            var wordRepositoryFake = new Mock<IWordRepository>();
            var DateTimeHelperFake = new Mock<DateTimeHelper>();
            IGameLogic gameLogic = new GameLogic(wordRepositoryFake.Object, DateTimeHelperFake.Object);
            string answer = "answer";
            Clue clue = new Clue {Answer = answer};
            int score;
            // Act
            bool isCorrect = gameLogic.EvaluateSubscriberAnswer(answer, clue, out score);

            // Assert
            Assert.Equal(answer.Length, score );
        }
    }
}