using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBookingApplication.BookingModels;
using MovieBookingApplication.BookingModels.DataTransferObjects;
using MovieBookingApplication.BookingRepositories.Interfaces;

namespace MovieBookingApplication.Controllers
{
    [Route("api/v1.0/moviebooking")]
    [ApiController]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieInterface _MovieRepository;
        private readonly ITicketInterface _TicketRepository;
        public MoviesController(IMovieInterface movieRepository, ITicketInterface ticketRepository)
        {
            _MovieRepository = movieRepository;
            _TicketRepository = ticketRepository;
        }


        [HttpGet("all")]
        public ActionResult<List<Movie>> Get()
        {
            return Ok(_MovieRepository.Get());
        }


        [HttpGet("movies/search/{moviename}")]
        public ActionResult<Movie> GetMovieInfo(string moviename)
        {
            var student = _MovieRepository.Get(moviename);
            if (student == null) return NotFound($"Movie with ID '${moviename}' not found!");
            return student;
        }


        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Movie> AdminAddsMoive([FromBody] MovieDataTransferObject2 movie)
        {
            Movie newMovie = new Movie()
            {
                NumberOfTicketsBooked = 0,
                MovieName = movie.MovieName,
                TheatreName = movie.TheatreName,
                TotalTicketsAlloted = movie.TotalTicketsAlloted,
                Status = "Available"


            };
            if (newMovie.TotalTicketsAlloted <= 0) return Content("Need to Allocate atleast 1 seat.");
            var movieExistsOrNot = _MovieRepository.Exists(movie.MovieName, movie.TheatreName);
            if (movieExistsOrNot != null) { return Content("Such an entry already exists."); }

            _MovieRepository.Create(newMovie);
            return CreatedAtAction(nameof(Get), new { id = newMovie.MovieId }, newMovie);
        }

        /*
         * [HttpPost("add/{movieName}")]
        public ActionResult<Movie> Post(string movieName, [FromBody] Movie movie)
        {

            if (movieName != movie.MovieName) { return BadRequest(); }
            _MovieRepository.Create(movie);
            return CreatedAtAction(nameof(Get), new { id = movie.MovieId }, movie);
        }

         */

        /*
         public ActionResult<Movie> Post(string movieName, string theatreName, int numberOfTickets)
        {
            var movie = _MovieRepository.Exists(movieName, theatreName);
            if (movie == null) return NotFound();
            movie.NumberOfTicketsBooked = movie.NumberOfTicketsBooked + numberOfTickets;
            movie.TotalTicketsAlloted = movie.TotalTicketsAlloted - numberOfTickets;
            if (movie.TotalTicketsAlloted < 0) return Content("Housefull...cannot book these many tickets.");
            Ticket ticket = new Ticket()
            {
                MovieName = movieName,
                TheatreName = theatreName,
                NumberOfTicketsBooked = numberOfTickets

            };
            _TicketRepository.Create(ticket);
            _MovieRepository.Update(movie.MovieId, movie);
            return Ok();
        }
         */
        /*
          [HttpPost("{moviename}/add")]
         public ActionResult<Movie> Post(string moviename, TicketDataTransferObject2 ticketData)
         {

             var movie = _MovieRepository.Exists(moviename, ticketData.TheatreName);
             if (movie == null) return NotFound();

             movie.NumberOfTicketsBooked = movie.NumberOfTicketsBooked + movie.NumberOfTicketsBooked;
             movie.TotalTicketsAlloted = movie.TotalTicketsAlloted - movie.NumberOfTicketsBooked;
             return Content(movie.TotalTicketsAlloted.ToString());
             if (movie.TotalTicketsAlloted < 0) return Content("Housefull...cannot book these many tickets.");
             Ticket ticket = new Ticket()
             {
                 MovieName = moviename,
                 TheatreName = ticketData.TheatreName,
                 NumberOfTicketsBooked = ticketData.NumberOfTicketsBooked

             };
             _TicketRepository.Create(ticket);
             _MovieRepository.Update(movie.MovieId, movie);
             return Ok();
         }
         */
        [Authorize(Roles = "User")]
        [HttpPost("add/{movieName}")]
        public ActionResult<Movie> Post(string movieName, string theatreName, int numberOfTickets)
        {
            var movie = _MovieRepository.Exists(movieName, theatreName);
            if (movie == null) return NotFound();
            movie.NumberOfTicketsBooked = movie.NumberOfTicketsBooked + numberOfTickets;
            movie.TotalTicketsAlloted = movie.TotalTicketsAlloted - numberOfTickets;
            if (movie.TotalTicketsAlloted < 0) return Content("Housefull...cannot book these many tickets.");
            Ticket ticket = new Ticket()
            {
                MovieName = movieName,
                TheatreName = theatreName,
                NumberOfTicketsBooked = numberOfTickets

            };
            _TicketRepository.Create(ticket);
            _MovieRepository.Update(movie.MovieId, movie);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("updatemoviebyadmin/{id}")]
        public ActionResult UpdateTheMovieTable(string id, [FromBody] MovieDataTransferObject movie)
        {
            var existingMovie = _MovieRepository.GetMovie(id);
            if (existingMovie == null) return NotFound($"Movie with ID '${id}' not found!");
            Movie movieResult = new Movie()
            {
                MovieId = existingMovie.MovieId,
                MovieName = movie.MovieName,
                TheatreName = movie.TheatreName,
                NumberOfTicketsBooked = movie.NumberOfTicketsBooked,
                TotalTicketsAlloted = movie.TotalTicketsAlloted,
                Status = movie.Status

            };
            _MovieRepository.Update(id, movieResult);
            return CreatedAtAction(nameof(Get), new { id = existingMovie.MovieId }, movieResult);
        }

        /*
         [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Movie movie)
        {
            var existingMovie = _MovieRepository.GetMovie(id);
            if (existingMovie == null) return NotFound($"Student with ID '${id}' not found!");
            _MovieRepository.Update(id, movie);
            return CreatedAtAction(nameof(Get), new { id = existingMovie.MovieId }, movie);
        }
         */

        /*
          [HttpPut("{id}")]
         public ActionResult Put(string id, [FromBody] Movie movie)
         {
             var existingMovie = _MovieRepository.GetMovie(id);
             if (existingMovie == null) return NotFound($"Student with ID '${id}' not found!");
             _MovieRepository.Update(id, movie);
             return CreatedAtAction(nameof(Get), new { id = existingMovie.MovieId }, movie);
         }
         */
        /*
          [HttpPut("{id}")]
         public ActionResult Put(string id, [FromBody] MovieDataTransferObject movie)
         {
             var existingMovie = _MovieRepository.GetMovie(id);
             if (existingMovie == null) return NotFound($"Movie with ID '${id}' not found!");
             Movie movieResult = new Movie()
             {
                 MovieId = existingMovie.MovieId,
                 MovieName = movie.MovieName,
                 TheatreName = movie.TheatreName,
                 NumberOfTicketsBooked = movie.NumberOfTicketsBooked,
                 TotalTicketsAlloted = movie.TotalTicketsAlloted

             };
             _MovieRepository.Update(id, movieResult);
             return CreatedAtAction(nameof(Get), new { id = existingMovie.MovieId }, movieResult);
         }
         */
        //[HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [HttpPut("{moviename}/update/{ticketId}")]
        public ActionResult Put(string moviename, string ticketId, [FromBody] UpdateTicket ticket)
        {
            var existingTicket = _TicketRepository.GetMovie(ticketId);
            if (existingTicket == null) return Content("No such ticket found.");

            Ticket ticketResult = new Ticket()
            {
                TicketId = existingTicket.TicketId,
                MovieName = existingTicket.MovieName,
                TheatreName = existingTicket.TheatreName,
                NumberOfTicketsBooked = existingTicket.NumberOfTicketsBooked,
                //NumberOfTicketsBooked = ticket.NumberOfTicketsBooked,
                TicketStatus = ticket.TicketStatus

            };
            if (ticket.TicketStatus == "cancelled")
            {
                ticketResult.NumberOfTicketsBooked = 0;
                var movieUpdate = _MovieRepository.Exists(existingTicket.MovieName, existingTicket.TheatreName);
                int tickets = existingTicket.NumberOfTicketsBooked;
                movieUpdate.NumberOfTicketsBooked = movieUpdate.NumberOfTicketsBooked - tickets;
                movieUpdate.TotalTicketsAlloted = movieUpdate.TotalTicketsAlloted + tickets;

                _MovieRepository.Update(movieUpdate.MovieId, movieUpdate);

            }
            _TicketRepository.Update(ticketId, ticketResult);
            return CreatedAtAction(nameof(Get), new { id = existingTicket.TicketId }, ticketResult);
            //return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{moviename}/delete/{id}")]
        public ActionResult Delete(string id, string moviename)
        {
            var existingMovie = _MovieRepository.GetMovie(id);
            if (existingMovie == null) return NotFound($"Movie with ID '${id}' not found!");
            if (existingMovie.NumberOfTicketsBooked != 0)
            {
                return BadRequest();
            }
            if (existingMovie.MovieName != moviename) return Content("No such movie found");

            _MovieRepository.Delete(id);
            return StatusCode(204, $"Movie with ID '${id}' deleted.");
        }
    }
}
