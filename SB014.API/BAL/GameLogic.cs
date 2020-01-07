using System;
using System.Collections.Generic;
using System.Linq;
using SB014.API.DAL;
using SB014.API.Domain;
using SB014.API.Helpers;
using SB014.API.Models;

namespace SB014.API.BAL
{
    public class GameLogic : IGameLogic
    {
        private readonly ITournamentRepository TournamentRepository;
        private readonly IWordRepository WordRepository; 
        private readonly IDateTimeHelper DateTimeHelper; 
        public GameLogic(IWordRepository wordRepository, IDateTimeHelper dateTimeHelper, ITournamentRepository tournamentRepository)
        {
            WordRepository = wordRepository;
            DateTimeHelper = dateTimeHelper;
            TournamentRepository = tournamentRepository;
        }

        public Game BuildGame(Guid tournamentId, int cluesPerGame)
        {
            IEnumerable<string> unscrambledWords = this.WordRepository.GetWords(cluesPerGame);
            List<Clue> anagrams = new List<Clue>();
            
            // Scramble words into anangrams
            foreach (string word in unscrambledWords)
            {
                anagrams.Add(new Clue
                { 
                    Id = Guid.NewGuid(),
                    GameClue = this.Scramble(word),
                    Answer = word
                });
            }
            
            return new Game{
                Id = Guid.NewGuid(),
                TournamentId = tournamentId,
                Clues = anagrams,
                Created = this.DateTimeHelper.CurrentDateTime
            };
        }
        /// <summary>
        /// Check if the suplierd answer attempt is the correct answer to the supplied clue
        /// </summary>
        /// <param name="answerAttempt"></param>
        /// <param name="clue"></param>
        /// <returns></returns>
        public bool EvaluateSubscriberAnswer(string answerAttempt, Clue clue, out int score)
        {
            score = 0;
            bool isCorrect = answerAttempt.ToLower() == clue.Answer.ToLower();
            if(isCorrect){
                score = clue.Answer.Length;
            }
            return isCorrect;
        }


        public List<SubscriberRankModel> BuildGameRankings(List<SubscriberGameResult> gameResults, int? maxRankingsCount)
        {
            List<SubscriberGameResult> orderedResults;
            if(maxRankingsCount.HasValue)              
            {
                orderedResults=gameResults.OrderByDescending(r=>r.GameScore).Take(maxRankingsCount.Value).ToList();
            }
            else
            {
                orderedResults=gameResults.OrderByDescending(r=>r.GameScore).ToList();
            }
            List<SubscriberRankModel> gameRankings = new List<SubscriberRankModel>();
            int currentRank = 0;
            SubscriberRankModel lastGameRanking = null;
            foreach (SubscriberGameResult gameResult in orderedResults)
            {
                SubscriberRankModel gameRanking = new  SubscriberRankModel
                {
                    SubscriberId = gameResult.SubscriberId,
                    GameScore = gameResult.GameScore
                };

                if(lastGameRanking != null && gameResult.GameScore == lastGameRanking.GameScore)
                {
                    // Same score as last result then same ranking (can have any number of subscribers who got same score so should have same ranking)
                    gameRanking.Rank = lastGameRanking.Rank;
                }
                else
                {
                    // Increment rank number
                    currentRank++;

                    // Set the rank 
                    gameRanking.Rank = currentRank;
                }
                // Get the subscriber and add to the ranking
                var subscriber = this.TournamentRepository.GetSubscriber(gameResult.TournamentId, gameResult.SubscriberId);
                if(subscriber != null)
                {
                    gameRanking.Name = subscriber.Name;                    
                }
                
                gameRankings.Add(gameRanking);
                
                lastGameRanking = gameRanking;              
            }
            
            return gameRankings;
        }

        private string Scramble(string word)
        {
            char[] chars = new char[word.Length]; 
            Random rand = new Random(10000); 
            int index = 0; 
            while (word.Length > 0) 
            { 
                // Get a random number between 0 and the length of the word. 
                // Take the character from the random position 
                int next = rand.Next(0, word.Length - 1); 
                
                //and add to our char array. 
                chars[index] = word[next];                
                
                // Remove the character from the word. 
                word = word.Substring(0, next) + word.Substring(next + 1); 
                ++index; 
            } 
            return new String(chars); 
        }
    }

}