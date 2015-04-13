using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Description;
using GeekQuiz.Models;

namespace GeekQuiz.Controllers
{
    // Controller used for returning next quiz question and for accepting an answer to a given question.

    [Authorize]
    public class TriviaController : ApiController
    {
        private readonly TriviaContext _db = new TriviaContext();
        protected override void Dispose(bool disposing)
        {
            if (disposing) { _db.Dispose(); }
            base.Dispose(disposing);
        }

        // GET api/Trivia
        // Returns the next question for this user
        [ResponseType(typeof(TriviaQuestion))]
        public async Task<IHttpActionResult> Get()
        {
            TriviaQuestion nextQuestion = await NextQuestionAsync(User.Identity.Name);
            if (nextQuestion == null) { return NotFound(); }

            return Ok(nextQuestion);
        }

        // POST api/trivia
        // Accepts an answer for a specific question
        [ResponseType(typeof(TriviaAnswer))]
        public async Task<IHttpActionResult> Post(TriviaAnswer answer)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            answer.UserId = User.Identity.Name;

            return Ok(await StoreAsync(answer));
        }

        // Retrieves the next question for this user
        private async Task<TriviaQuestion> NextQuestionAsync(string userId)
        {
            var lastQuestionId = await _db.TriviaAnswers
                .Where(a => a.UserId == userId)
                .GroupBy(a => a.QuestionId)
                .Select(g => new { QuestionId = g.Key, Count = g.Count() })
                .OrderByDescending(q => new { q.QuestionId, q.Count })
                .Select(q => q.QuestionId)
                .FirstOrDefaultAsync();

            var questionsCount = await _db.TriviaQuestions.CountAsync();
            var nextQuestionId = (lastQuestionId % questionsCount) + 1;

            return await _db.TriviaQuestions.FindAsync(CancellationToken.None, nextQuestionId);
        }

        // Saves the answer chosen by the user for a specific question, and returns whether or not the provided answer is correct
        private async Task<bool> StoreAsync(TriviaAnswer answer)
        {
            _db.TriviaAnswers.Add(answer);
            await _db.SaveChangesAsync();

            var selectedOption = await _db.TriviaOptions.FirstOrDefaultAsync(o => o.Id == answer.OptionId && o.QuestionId == answer.QuestionId);

            return selectedOption.IsCorrect;
        }
    }
}