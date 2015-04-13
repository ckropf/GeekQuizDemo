using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GeekQuiz.Models;

namespace GeekQuiz.Controllers
{
    // Controller used for managing quiz questions

    [Authorize]
    public class TriviaQuestionsApiController : ApiController
    {
        private readonly TriviaContext _db = new TriviaContext();
        protected override void Dispose(bool disposing)
        {
            if (disposing) { _db.Dispose(); }
            base.Dispose(disposing);
        }

        // GET: api/TriviaQuestionsApi/GetTriviaQuestions
        // Returns all questions
        [ResponseType(typeof(System.Collections.Generic.IEnumerable<TriviaQuestion>))]
        public async Task<IHttpActionResult> GetTriviaQuestions()
        {
            return Ok(await _db.TriviaQuestions.ToListAsync());
        }

        // GET: api/TriviaQuestionsApi/5
        // Returns a specific question
        [ResponseType(typeof(TriviaQuestion))]
        public async Task<IHttpActionResult> GetTriviaQuestion(int id)
        {
            TriviaQuestion triviaQuestion = await _db.TriviaQuestions.FindAsync(id);
            if (triviaQuestion == null) { return NotFound(); }

            return Ok(triviaQuestion);
        }

        // PUT: api/TriviaQuestionsApi/PutTriviaQuestion/5
        // Updates a specific question
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTriviaQuestion(int id, TriviaQuestion triviaQuestion)
        {
            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }

            if (id != triviaQuestion.Id)
            { return BadRequest(); }

            _db.Entry(triviaQuestion).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TriviaQuestionExists(id))
                { return NotFound(); }
                else { throw; }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/TriviaQuestionsApi/PostTriviaQuestion
        // Creates a new question
        [ResponseType(typeof(TriviaQuestion))]
        public async Task<IHttpActionResult> PostTriviaQuestion(TriviaQuestion triviaQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.TriviaQuestions.Add(triviaQuestion);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("ApiByAction", new { id = triviaQuestion.Id }, triviaQuestion);
        }

        // DELETE: api/TriviaQuestionsApi/DeleteTriviaQuestion/5
        // Deletes a specific question
        [ResponseType(typeof(TriviaQuestion))]
        public async Task<IHttpActionResult> DeleteTriviaQuestion(int id)
        {
            TriviaQuestion triviaQuestion = await _db.TriviaQuestions.FindAsync(id);
            if (triviaQuestion == null)
            { return NotFound(); }

            _db.TriviaQuestions.Remove(triviaQuestion);
            await _db.SaveChangesAsync();

            return Ok(triviaQuestion);
        }

        // Returns whether or not a question with the specified id exists
        private bool TriviaQuestionExists(int id)
        {
            return _db.TriviaQuestions.Any(e => e.Id == id);
        }
    }
}