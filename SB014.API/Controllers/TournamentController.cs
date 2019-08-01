using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SB014.API.Repository;
using SB014.API.Models;
using System;
using SB014.API.Entities;

namespace SB014.API.Controllers
{
    [Route("api/tournament")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        
        private readonly ITournamentRepository TournamentRepository;
        private readonly IMapper _mapper;
        public TournamentController(ITournamentRepository tournamentRepository, IMapper mapper)
        {
            this.TournamentRepository = tournamentRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult GetTournaments()
        {            
            return Ok(_mapper.Map<List<Tournament>,List<TournamentModel>>(this.TournamentRepository.GetAll()));
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
            return Ok(_mapper.Map<Subscriber,SubscriberModel>(subscriber));
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

            Subscriber subscriber = _mapper.Map<SubscribeToTournamentModel,Subscriber>(subscribeToTournamentModel);
            subscriber.TournamentId = id;            
            Subscriber newSubscriber = this.TournamentRepository.AddSubscriber(subscriber);

            return CreatedAtRoute("TournamentSubscriber", new {
               tournamentid = id,
               id = newSubscriber.Id 
            },_mapper.Map<Tournament, TournamentModel>(tournament));
        }

    }
}