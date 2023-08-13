using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Retrieving all the Genres
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public  async Task<IActionResult> GetAllAsync() 
        {
            var genres = await _context.Genres.OrderBy(g=>g.Name).ToListAsync();
            return Ok(genres);
        }

        /// <summary>
        /// Creating new Genre by giving the name
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]

        public async Task<IActionResult>CreateAsync(CreateGenreDto dto) 
        {
            var genre=new Genre { Name=dto.Name };
            await _context.AddAsync(genre);
            _context.SaveChanges();
            return Ok(genre);
        }


        /// <summary>
        /// Updating the Genre by giving the id of Genre that you want to Update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]

        public async Task<IActionResult>UpdateAsync(int id,CreateGenreDto dto) 
        {
            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
            if (genre == null)
                return NotFound($"No genre was found with ID:{id}");
            genre.Name= dto.Name;
            _context.SaveChanges();
            return Ok(genre);
        }

        /// <summary>
        /// Deleting specific Genre by Giving the id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
            if (genre == null)
                return NotFound($"No genre was found with ID:{id}");
            _context.Remove(genre);
            _context.SaveChanges();
            return Ok(genre);
        }


    }
}
