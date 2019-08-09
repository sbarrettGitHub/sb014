using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SB014.API.Models;
using System;
using SB014.API.DAL;
using SB014.API.BAL;

namespace SB014.API.Controllers
{
    [Route("api/tournament")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        
        private readonly ITournamentRepository TournamentRepository;
        private readonly IMapper Mapper;
        private readonly IGameLogic GameLogic;
        public TournamentController(ITournamentRepository tournamentRepository, IMapper mapper, IGameLogic gameLogic)
        {
            this.TournamentRepository = tournamentRepository;
            Mapper = mapper;
            GameLogic = gameLogic;
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

            // If no game exists create one
            bool doesGameExist = this.TournamentRepository.HasGame(id);
            if(doesGameExist == false)
            {
                this.GameLogic.BuildGame(id, tournament.CluesPerGame);
            }            

            return CreatedAtRoute("TournamentSubscriber", new {
               tournamentid = id,
               id = newSubscriber.Id 
            },Mapper.Map<Tournament, TournamentModel>(tournament));
        }

    }
}