using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Description;
using GeekQuiz.Models;

namespace GeekQuiz.Controllers
{
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
        [ResponseType(typeof(TriviaQuestion))]
        public async Task<IHttpActionResult> Get()
        {
            TriviaQuestion nextQuestion = await NextQuestionAsync(User.Identity.Name);
            if (nextQuestion == null) { return NotFound(); }
            return Ok(nextQuestion);
        }

        // GET api/Trivia/GetAllQuestions
        [ResponseType(typeof(System.Collections.Generic.IEnumerable<TriviaQuestion>))]
        public async Task<IHttpActionResult> GetAllQuestions()
        {
            return Ok(await _db.TriviaQuestions.ToListAsync());
        }

        //GET api/Trivia/5
        [ResponseType(typeof(TriviaQuestion))]
        public async Task<IHttpActionResult> Get(int id)
        {
            TriviaQuestion returnQuestion = await _db.TriviaQuestions.Where(q => q.Id == id).FirstOrDefaultAsync();
            if (returnQuestion == null) { return NotFound(); }
            return Ok(returnQuestion);
        }

        // POST api/trivia
        [ResponseType(typeof(TriviaAnswer))]
        public async Task<IHttpActionResult> Post(TriviaAnswer answer)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            answer.UserId = User.Identity.Name;
            return Ok(await StoreAsync(answer));
        }

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

        private async Task<bool> StoreAsync(TriviaAnswer answer)
        {
            _db.TriviaAnswers.Add(answer);
            await _db.SaveChangesAsync();

            var selectedOption = await _db.TriviaOptions.FirstOrDefaultAsync(o => o.Id == answer.OptionId && o.QuestionId == answer.QuestionId);
            return selectedOption.IsCorrect;
        }
    }
}