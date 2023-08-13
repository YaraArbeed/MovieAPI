using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Retrieving all the movies
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]

        public async Task<IActionResult> GetAllAsync() 
        {
            var movies = await _context.Movies.Include(m=>m.Genre).ToListAsync();
            return Ok(movies);
        }

        /// <summary>
        /// Retrieving specific movie by giving the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        ///<response code="401"> Unauthorized</response>
        ///<response code="403"> Forbidden:this user is Authenticated but doesn't have a role to access this end-point</response>
        [Authorize(Roles="Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult>GetByIdAsync(int id)
        {
            var movie=await _context.Movies.Include(m=>m.Genre).SingleOrDefaultAsync(m=>m.Id==id);

            if(movie == null)
                return NotFound();

            return Ok(movie);
           
        }


        /// <summary>
        /// Retrieving specific movie by giving the GenreId
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        [HttpGet("GetByGenreId")]
        public async Task<IActionResult>GetByGenreIdAsync(byte genreId)
        {
            var movie = await _context.Movies.Where(m => m.GenreId == genreId).Include(m => m.Genre).ToListAsync();

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }


        /// <summary>
        /// Creating new Movie by giving id,title,poster,rate,year,storeline
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm]MovieDto dto) 
        {
            if(dto.Poster==null)
                return BadRequest("poster is required");

            var isValidGenre=await _context.Genres.AnyAsync(g=>g.Id==dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid genre ID!");

            using var datastream=new MemoryStream();
            await dto.Poster.CopyToAsync(datastream);
            var movie = new Movie
            {
               GenreId = dto.GenreId,
               Title = dto.Title,
               Poster=datastream.ToArray(),
               Rate = dto.Rate,
               StoreLine = dto.StoreLine,
               Year = dto.Year
            };
            await _context.AddAsync(movie);
            await _context.SaveChangesAsync();
            return Ok(movie);
        }


        /// <summary>
        /// Updating the Movie by giving the id of Movie that you want to Update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm]MovieDto dto)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound($"No movie was found with ID:{id}");

            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid genre ID!");

            if (dto.Poster != null) 
            {
                using var datastream = new MemoryStream();
                await dto.Poster.CopyToAsync(datastream);
                movie.Poster= datastream.ToArray();
            }

            movie.Title = dto.Title;
            movie.GenreId = dto.GenreId;
            movie.Year= dto.Year;
            movie.StoreLine = dto.StoreLine;
            movie.Rate = dto.Rate;
            _context.SaveChanges();
            return Ok(movie);
        }


        /// <summary>
        /// Deleting specific Genre by Giving the id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound($"No movie was found with ID:{id}");
            _context.Remove(movie);
            _context.SaveChanges();
            return Ok(movie);
        }

    }
}
