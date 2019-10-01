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

namespace SB014.API.Controllers
{
    [Route("api/tournament")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        
        private readonly ITournamentRepository TournamentRepository;
        private readonly IMapper Mapper;
        private readonly IGameLogic GameLogic;
        private readonly ITournamentLogic TournamentLogic;
        public TournamentController(ITournamentRepository tournamentRepository, IMapper mapper, IGameLogic gameLogic, ITournamentLogic tournamentLogic)
        {
            this.TournamentRepository = tournamentRepository;
            this.Mapper = mapper;
            this.GameLogic = gameLogic;
            this.TournamentLogic = tournamentLogic;
        }         

        [HttpGet]
        public IActionResult GetTournaments()
        {            
            return Ok(Mapper.Map<List<Tournament>,List<TournamentModel>>(this.TournamentRepository.GetAll()));
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
                switch (tournamentUpdate.State)
                {
                    // Updating to preplay
                    case TournamentState.PrePlay:
                        this.TournamentLogic.SetPreplay(tournament);
                    break;
                }
                
            }
            
            
            // Return success
            return NoContent();
        }
    }
}