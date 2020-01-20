using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using SB014.API.BAL;
using SB014.API.DAL;
using SB014.API.Helpers;
using SB014.API.Notifications;
using SB014.API.Models;

namespace SB014.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(setupAction =>setupAction.RespectBrowserAcceptHeader = true)
                    .AddNewtonsoftJson();
            services.AddCors();
            services.AddSingleton<ITournamentRepository, TournamentRepositoryFake>();
            services.AddSingleton<IDateTimeHelper, DateTimeHelper>();
            services.AddScoped<IWordRepository, WordRepositoryFake>();
            services.AddScoped<ITournamentLogic, TournamentLogic>();
            services.AddScoped<IGameLogic, GameLogic>();            
            services.AddScoped<ITournamentBroadcast, TournamentBroadcast>();            
            services.AddAutoMapper(typeof(AutomapperProfile));
            services.AddSignalR(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors(
                options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
            ); //This needs to set everything allowed
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<TournamentHub>("/tournamentHub");
            });
            
        }
    }
}
