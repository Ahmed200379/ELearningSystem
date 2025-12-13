
using Domain.Entities;
using Domain.Interfaces;
using Org.BouncyCastle.Asn1.Ocsp;
using Services.Abstractions;
using Shared.Dtos.Answer;
using Shared.Dtos.Question;
using Shared.Dtos.Quiz;
using Shared.Helpers;
using System.Linq.Expressions;

namespace Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public QuestionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ResponseQuestionDto>> GetAllQuestionsAsync()
        {
            Expression<Func<Question, object>> includes = q => q.Answers;

            var questions = await _unitOfWork.GetRepository<Question>().GetAllAsyncs(includes: includes);
            var response = questions.Select(q => new ResponseQuestionDto
            {
                Id = q.Id,
                Title = q.Title,
                Image = q.Image,
                Type = q.Type,
                TimeLimit = q.TimeLimit,
                QuizId = q.QuizId,
                Answers = q.Answers.Select(a => new ResponseAnswerWithoutQuestionIdDto
                {
                    Id = a.Id,
                    Text = a.Text,
                    IsCorrect = a.IsCorrect,
                    Image = a.Image
                }).ToList()
            });
            return response;
        }

        public async Task<ResponseQuestionDto> GetQuestionByIdAsync(string id)
        {
            Expression<Func<Question, object>> includes = q => q.Answers;
            var question = await _unitOfWork.GetRepository<Question>().GetFirstOrDefault(q => q.Id == id, includes: includes);
            if (question == null)
            {
                return null;
            }
            var response = new ResponseQuestionDto
            {
                Id = question.Id,
                Title = question.Title,
                Image = question.Image,
                Type = question.Type,
                TimeLimit = question.TimeLimit,
                QuizId = question.QuizId,
                Answers = question.Answers.Select(a => new ResponseAnswerWithoutQuestionIdDto
                {
                    Id = a.Id,
                    Text = a.Text,
                    IsCorrect = a.IsCorrect,
                    Image = a.Image
                }).ToList()
            };
            return response;
        }

        public async Task<Question> CreateQuestionAsync(RequestQuestionDto question)
        {
            saveFiles(question);

            var quizQuery =  _unitOfWork.GetRepository<Quiz>();
            var exitQuiz = await quizQuery.GetByIdAsync(question.QuizId);
            if (exitQuiz == null)
            {
                throw new Exception("Quiz not found");
            }

            var newQuestion = new Question
            {
                Id = Guid.NewGuid().ToString(),
                Title = question.Title,
                Image = question.Image != null ? $"{BaseUrl.BaseUrlValue}/Questions/images/{question.Image.FileName}" : null,
                Type = question.Type,
                TimeLimit = question.TimeLimit,
                QuizId = question.QuizId,
            };
            await _unitOfWork.GetRepository<Question>().AddAsync(newQuestion);
            await _unitOfWork.SaveChanges();
            foreach(var a in question.Answers)
            {
                var answer = new Answer
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = a.Text,
                    IsCorrect = a.IsCorrect,
                    Image = a.Image != null ? $"{BaseUrl.BaseUrlValue}/Answers/images/{a.Image.FileName}" : null,
                    QuestionId = newQuestion.Id,
                };
                await _unitOfWork.GetRepository<Answer>().AddAsync(answer);
                await _unitOfWork.SaveChanges();
            }
            await _unitOfWork.SaveChanges();
            return newQuestion;
        }

        public async Task<Question> UpdateQuestionAsync(string id, RequestQuestionDto question)
        {
            var questionQuery = _unitOfWork.GetRepository<Question>();
            var existingQuestion = await questionQuery.GetFirstOrDefault(q => q.Id == id);
            if (existingQuestion == null)
                throw new Exception("Question not found");

            var QuizQuery = _unitOfWork.GetRepository<Quiz>();
            var existingQuize = await QuizQuery.GetFirstOrDefault(q=> q.Id == question.QuizId);
            if (existingQuize == null)
                throw new Exception("Quiz not found");
           await saveFiles(question);
         
            if (question.Title != null)
                existingQuestion.Title = question.Title;
            if(question.Type != null)
                existingQuestion.Type = question.Type;
            if (question.QuizId != null)
                existingQuestion.QuizId = question.QuizId;
            if (question.TimeLimit != null)
                existingQuestion.TimeLimit = question.TimeLimit;

            existingQuestion.Image = question.Image != null ? $"{BaseUrl.BaseUrlValue}/Questions/images/{question.Image.FileName}" : existingQuestion.Image;

            questionQuery.Update(existingQuestion);
            await _unitOfWork.SaveChanges();
            return existingQuestion;

        }
        public async Task<string> DeleteQuestionAsync(string id)
        {
            var questionQuery =  _unitOfWork.GetRepository<Question>();
            var question = await questionQuery.GetFirstOrDefault(q=>q.Id==id);
            if (question == null)
                throw new Exception("Question not Found");
            questionQuery.Delete(question);
            await _unitOfWork.SaveChanges();
            return "Question Deleted Successfuly";
        }

        private async Task<string?> saveFiles(RequestQuestionDto request)
        {
            var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Questions");
            Directory.CreateDirectory(imageFolder);
            var imageFile = request.Image;

            if (imageFile != null && imageFile.Length > 0)
            {
                var filePath = Path.Combine(imageFolder, imageFile.FileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(fileStream);
                return filePath;
            }
            return null;
        }

    }
}
