using FootballScoresDbApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace FootballScoresDbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        protected readonly AuthenticationContext Context;
        private readonly ILogger<TeamsController> _logger;

        public TeamsController(AuthenticationContext context, ILogger<TeamsController> logger) 
        {
            _logger = logger;
            Context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string email)
        {
            var team = await Context.Teams.FirstOrDefaultAsync(x => x.Users.Any(u => u.Email == email));
            return Ok(new { name = team?.Name });
        }

        [HttpPost]
        public async Task<IActionResult> Add(string name, string email)
        {
            var user = await Context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user != null)
            {
                await Context.Teams.AddAsync(new Team { Name = name, Users = new List<ApplicationUser>() { user } });
                await Context.SaveChangesAsync();
            }
            return Ok();
        }
    }
}
