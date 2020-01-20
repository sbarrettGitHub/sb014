using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SB014.API.Models;
using System;
using SB014.API.DAL;
using SB014.API.BAL;
using SB014.API.Domain;
using Microsoft.AspNetCore.JsonPatch;
using SB014.API.Domain.Enums;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using SB014.API.Notifications;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace SB014.API.Controllers
{
    [EnableCors()]
    [Route("api/tournament")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        
        private readonly ITournamentRepository TournamentRepository;
        private readonly IMapper Mapper;
        private readonly IGameLogic GameLogic;
        private readonly ITournamentLogic TournamentLogic;

        private readonly ITournamentBroadcast TournamentBroadcast;
        public TournamentController(ITournamentRepository tournamentRepository, IMapper mapper, IGameLogic gameLogic, ITournamentLogic tournamentLogic, ITournamentBroadcast tournamentBroadcast)
        {
            this.TournamentRepository = tournamentRepository;
            this.Mapper = mapper;
            this.GameLogic = gameLogic;
            this.TournamentLogic = tournamentLogic;
            this.TournamentBroadcast = tournamentBroadcast;
        }
        [HttpGet]
        public IActionResult GetTournaments()
        {            
            return Ok(Mapper.Map<List<Tournament>,List<TournamentModel>>(this.TournamentRepository.GetAll()));
        }


        [HttpGet]
        [Route("{id}", Name="Tournament")]
        public IActionResult GetTournament(Guid id)
        {
            Tournament tournament = this.TournamentRepository.Get(id);
            if(tournament == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<Tournament,TournamentModel>(this.TournamentRepository.Get(id)));
        }

        [HttpGet]
        [Route("{tournamentid}/game/{id}", Name="TournamentGame")]
        public IActionResult GetTournamentGame(Guid tournamentid, Guid id)
        {
            Game tournamentGame = this.TournamentRepository.GetGame(tournamentid,id);
            if(tournamentGame == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<Game,GameModel>(tournamentGame));
        }

        [HttpGet]
        [Route("{tournamentid}/subscriber/{id}", Name="TournamentSubscriber")]
        public IActionResult GetTournamentSubscriber(Guid tournamentid, Guid id)
        {
            Tournament tournament = this.TournamentRepository.Get(tournamentid);
            if(tournament == null)
            {
                return NotFound();
            }

            Subscriber subscriber = this.TournamentRepository.GetSubscriber(tournamentid, id);
            if(subscriber == null)
            {
                return NotFound();  
            }
            return Ok(Mapper.Map<Subscriber,SubscriberModel>(subscriber));
        }

        [HttpPost]
        [Route("{id}/subscriber")]
        public IActionResult SubscribeToTournament(Guid id , [FromBody] SubscribeToTournamentModel subscribeToTournamentModel)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest();          
            }
            Tournament tournament = this.TournamentRepository.Get(id);
            if(tournament == null)
            {
                return NotFound();
            }

            Subscriber subscriber = Mapper.Map<SubscribeToTournamentModel,Subscriber>(subscribeToTournamentModel);
            subscriber.TournamentId = id;            
            Subscriber newSubscriber = this.TournamentRepository.AddSubscriber(subscriber);
            SubscriberModel tournamentSubscriberModel = Mapper.Map<Subscriber, SubscriberModel>(newSubscriber);

            // If no game exists create one
            bool doesGameExist = this.TournamentRepository.HasGame(id);
            if(doesGameExist == false)
            {
                var newGame  = this.GameLogic.BuildGame(id, tournament.CluesPerGame);
                var game = this.TournamentRepository.UpdateGame(newGame);
            }            
            
            return CreatedAtRoute("TournamentSubscriber", new {
               tournamentid = id,
               id = newSubscriber.Id 
            },tournamentSubscriberModel);
        }
        
        [HttpDelete]
        [Route("{id}/subscriber/{subscriberId}")]
        public IActionResult UnsubscribeFromTournament(Guid id, Guid subscriberId)
        {
            Tournament tournament = this.TournamentRepository.Get(id);
            if(tournament == null)
            {
                return NotFound();
            }
            Subscriber subscriber = this.TournamentRepository.GetSubscriber(id, subscriberId);
            if(subscriber == null)
            {
                return NotFound();  
            }

            this.TournamentRepository.RemoveSubscriber(id, subscriberId);

            return NoContent();
        }
        #region StateManagement
        [HttpPatch]
        [Route("{id}")]
        public IActionResult Update(Guid id, [FromBody]JsonPatchDocument<TournamentStateUpdateModel> jsonPatchDocument)
        {
            // Get the tournament
            Tournament tournament = this.TournamentRepository.Get(id);
            if(tournament == null)
            {
                return NotFound();
            }
            
            // Get the current tournament status 
            TournamentState initialState = tournament.State;

            // Update the tournament
            TournamentStateUpdateModel tournamentUpdate = new TournamentStateUpdateModel();
            jsonPatchDocument.ApplyTo(tournamentUpdate);

            // Only honour the update of a state
            if(tournamentUpdate.State != initialState)
            {
                // Set tournament state based on logic rules
                SetState(tournament, initialState, tournamentUpdate);

                // Update the tournament state
                this.TournamentRepository.Update(tournament);
            }
            
            
            // Return success
            return NoContent();
        }
        
        [HttpPost]
        [Route("{id}/bell")]
        public async Task<IActionResult> AddTournamentStateBell(Guid id)
        {
            Tournament tournament = this.TournamentRepository.Get(id);
            if(tournament == null)
            {
                return NotFound();
            }
            // Get the current tournament status 
            TournamentState initialState = tournament.State;

            TournamentStateUpdateModel tournamentStateChange = this.TournamentLogic.AddBell(tournament);
            
            // Set tournament state based on logic rules
            SetState(tournament, initialState, tournamentStateChange);

            // Update the tournament state
            this.TournamentRepository.Update(tournament);

            // Broadcast to all subscribers that the state has changed
            if(initialState != tournamentStateChange.State)
            {            
                await this.TournamentBroadcast.TournamentStateChangeAsync(id, tournament.State, tournament.PreplayGameId, tournament.InplayGameId, tournament.PostplayGameId);
            }
           
            // Return the updated tournament
            return Ok(Mapper.Map<Tournament,TournamentModel>(this.TournamentRepository.Get(id)));
        }

        #endregion
        
        #region GamePlay
        [HttpPost]
        [Route("{tournamentid}/subscriber/{subscriberid}/game/{gameid}/clue/{id}/answerattempt")]
        public IActionResult AddTournamentSubscriberGameAnswerAttempt(Guid tournamentid, Guid subscriberid, Guid gameid, Guid id, [FromBody] AnswerAttemptUpdateModel answerAttempt)
        {
            // Get the tournament
            Tournament tournament = this.TournamentRepository.Get(tournamentid);
            if(tournament == null)
            {
                return NotFound();
            }
            Subscriber subscriber = this.TournamentRepository.GetSubscriber(tournamentid, subscriberid);
            if(subscriber == null)
            {
                return NotFound();  
            }

            Game tournamentGame = this.TournamentRepository.GetGame(tournamentid, gameid);
            if(tournamentGame == null)
            {
                return NotFound();
            }
            
            // Reject any answer to a game that not in play
            if(tournament.InplayGameId != tournamentGame.Id)
            {
                return BadRequest();
            }

            Clue clue = tournamentGame.Clues.FirstOrDefault(x=>x.Id == id);
            if(clue == null)
            {
                return NotFound();
            }
            
            // Check the answer
            int score;
            bool isCorrect = this.GameLogic.EvaluateSubscriberAnswer(answerAttempt.Answer, clue, out score);
            
            // Records the answer attempt and score
            this.TournamentRepository.UpdateSubscriberGameResult(tournamentid, subscriberid, gameid, clue.Id, answerAttempt.Answer, score);

            if(isCorrect)
            {                
                return Ok();
            }
            
            return BadRequest();
        }
        #endregion

        #region  PostGame
        
        /// <summary>
        /// Get the results of a particular tournament game for a specified subscriber
        /// </summary>
        /// <param name="tournamentid"></param>
        /// <param name="subscriberid"></param>
        /// <param name="gameid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{tournamentid}/subscriber/{subscriberid}/game/{id}/results")]
        public IActionResult GetTournamentSubscriberGameResults(Guid tournamentid, Guid subscriberid, Guid id)
        {
            // Get the tournament
            Tournament tournament = this.TournamentRepository.Get(tournamentid);
            if(tournament == null)
            {
                return NotFound();
            }
            Game tournamentGame = this.TournamentRepository.GetGame(tournamentid, id);
            if(tournamentGame == null)
            {
                return NotFound();
            }

            Subscriber subscriber = this.TournamentRepository.GetSubscriber(tournamentid, subscriberid);
            if(subscriber == null)
            {
                return NotFound();  
            }

            // Get the subscriber result of this game
            SubscriberGameResult subscriberGameResults = this.TournamentRepository.GetSubscriberGameResult(tournamentid, subscriberid, id);

            if(subscriberGameResults == null)
            {
                return NotFound(); 
            }

            SubscriberGameResultModel model = Mapper.Map<SubscriberGameResult,SubscriberGameResultModel>(subscriberGameResults);

            // Get the subscribers rank in the overall game rankings
            var allRankings = this.GameLogic.BuildGameRankings(this.TournamentRepository.GetAllSubscriberGameResults(tournamentid, id), null);
            var ranking = allRankings.FirstOrDefault(r=>r.SubscriberId == subscriberid);
            if(ranking != null)
            {       
                model.Rank = ranking.Rank;
            }
            else
            {
                model.Rank = int.MaxValue;
            }
            
            // Set subscriber rank by finding him in the game rankings
            return Ok(model);
        }
        [HttpGet]
        [Route("{tournamentid}/game/{id}/results")]
        public IActionResult GetTournamentGameResults(Guid tournamentid, Guid id)
        {
            // Get the tournament
            Tournament tournament = this.TournamentRepository.Get(tournamentid);
            if(tournament == null)
            {
                return NotFound();
            }
            Game tournamentGame = this.TournamentRepository.GetGame(tournamentid,id);
            if(tournamentGame == null)
            {
                return NotFound();
            }

            if(tournamentGame.Id == tournament.InplayGameId)
            {
                return BadRequest();
            }
            
            GameResultsModel gameResults = new GameResultsModel
            {
                TournamentId = tournament.Id,
                GameId = tournamentGame.Id,
                Created = tournamentGame.Created,
                ClueAnswers = Mapper.Map<List<Clue>,List<ClueAnswerModel>>(tournamentGame.Clues), // Get the answers of the game
                Rankings = this.GameLogic.BuildGameRankings(this.TournamentRepository.GetAllSubscriberGameResults(tournamentid, id), tournament.RankingCutOff)
                
            };          
           
            return Ok(gameResults);
        }
        #endregion
        private void SetState(Tournament tournament, TournamentState initialState, TournamentStateUpdateModel tournamentUpdate)
        {
            // If the current state is No play then treat any state change as an update to PrePlay
            if(initialState == TournamentState.NoPlay)
            {
                tournamentUpdate.State = TournamentState.PrePlay;
            }

            Game newPreplayGame;
            // Apply logic rules to state change
            switch (tournamentUpdate.State)
            {
                // Updating to preplay
                case TournamentState.PrePlay:
                    // Apply the rules for setting the new Preplay state
                    tournament = this.TournamentLogic.SetPreplay(tournament, out newPreplayGame);
                    // Save the new preplay if one is created
                    if(newPreplayGame != null)
                    {
                        this.TournamentRepository.UpdateGame(newPreplayGame);
                    }
                break;
                // Updating to preplay
                case TournamentState.InPlay:
                    // Apply the rules for setting the new Inplay state
                    tournament = this.TournamentLogic.SetInplay(tournament, out newPreplayGame);
                    // Save the new preplay if one is created
                    if(newPreplayGame != null)
                    {
                        this.TournamentRepository.UpdateGame(newPreplayGame);
                    }
                    break;
                // Updating to post play
                case TournamentState.PostPlay:
                    // Apply the rules for setting the new Inplay state
                    tournament = this.TournamentLogic.SetPostplay(tournament, out newPreplayGame);
                    // Save the new preplay if one is created
                    if(newPreplayGame != null)
                    {
                        this.TournamentRepository.UpdateGame(newPreplayGame);
                    }
                    break;
                default:                        
                    break;
            }
        }

        
    }
}