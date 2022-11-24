using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly IConfiguration _config;

        public SuperHeroController(IConfiguration config)
        {
            _config = config;
            
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAllSuperHeros()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<SuperHero> heros = await SelectAllHeros(connection);
            return Ok(heros);

        }

       

        [HttpGet("{heroId}")]
        public async Task<ActionResult<List<SuperHero>>> GetHero(int heroId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var hero = await connection.QueryFirstAsync<SuperHero>("select * from superheros where id =@Id",new {Id = heroId});

                return Ok(hero);
            
         
        }




        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> CreateSuperHero( SuperHero superHero)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
             await connection.ExecuteAsync("insert into SuperHeros(Name,FirstName,LastName,Place) Values (@Name,@FirstName,@LastName,@Place)", superHero);
            return Ok(await SelectAllHeros(connection));

        }


        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero superHero)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("Update SuperHeros set name =@Name,firstname = @FirstName,lastname = @LastName,place = @Place where id = @Id", superHero);
            return Ok(await SelectAllHeros(connection));

        }



        [HttpDelete("{heroId}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteHero(int heroId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("delete from superheros where id=@Id", new {Id = heroId});
            return Ok(await SelectAllHeros(connection));

        }

        private static async Task<IEnumerable<SuperHero>> SelectAllHeros(SqlConnection connection)
        {
            return await connection.QueryAsync<SuperHero>("select * from superheros");
        }
    }
}
