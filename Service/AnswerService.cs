
using Domain.Entities;
using Domain.Interfaces;
using Services.Abstractions;
using Shared.Dtos.Answer;
using Shared.Helpers;

namespace Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AnswerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ResponseAnswerDto>> GetAllAnswers()
        {
            var answers = await _unitOfWork.GetRepository<Answer>().GetAllAsync();
            return answers.Select(a => new ResponseAnswerDto
            {
                Text = a.Text,
                IsCorrect = a.IsCorrect,
                QuestionId = a.QuestionId,
                Image = a.Image,
            }).ToList();
        }

        public async Task<ResponseAnswerDto> GetAnswerById(string id)
        {
            var answer = await _unitOfWork.GetRepository<Answer>().GetByIdAsync(id);
            if (answer == null)
            {
                throw new Exception("Answer not found");
            }
            return answer != null ? new ResponseAnswerDto
            {
                Text = answer.Text,
                IsCorrect = answer.IsCorrect,
                QuestionId = answer.QuestionId,
                Image = answer.Image,
            } : null!;
        }
        public async Task<Answer> CreateAnswer(RequestAnswerDto answerDto)
        {
            SaveFiles(answerDto);
            var question = await _unitOfWork.GetRepository<Question>().GetByIdAsync(answerDto.QuestionId);
            if (question == null)
            {
                throw new Exception("Question not found");
            }
            var answer = new Answer
            {
                Id = Guid.NewGuid().ToString(),
                Text = answerDto.Text,
                IsCorrect = answerDto.IsCorrect,
                QuestionId = answerDto.QuestionId,
                Image = answerDto.Image != null ? $"{BaseUrl.BaseUrlValue}/Answers/images/{answerDto.Image.FileName}": null
            };
            await _unitOfWork.GetRepository<Answer>().AddAsync(answer);
            await _unitOfWork.SaveChanges();
            return answer;
        }
        public async Task<Answer> UpdateAnswer(string id, RequestAnswerDto answerDto)
        {
            var answer = _unitOfWork.GetRepository<Answer>();
            var existingAnswer = await answer.GetByIdAsync(id);
            if (existingAnswer == null)
            {
                throw new Exception("Answer not found");
            }
            var question = await _unitOfWork.GetRepository<Question>().GetByIdAsync(answerDto.QuestionId);
            if (question == null)
            {
                throw new Exception("Question not found");
            }
            if(existingAnswer.IsCorrect != null)
                existingAnswer.IsCorrect = answerDto.IsCorrect;
            if(existingAnswer.QuestionId != null)
                existingAnswer.QuestionId = answerDto.QuestionId;
            if(existingAnswer.Text != null)
                existingAnswer.Text = answerDto.Text;
            if (answerDto.Image != null)
            {
                existingAnswer.Image = $"{BaseUrl.BaseUrlValue}/Answers/images/{answerDto.Image.FileName}";
                await SaveFiles(answerDto);
            }
            answer.Update(existingAnswer);
            await _unitOfWork.SaveChanges();
            return existingAnswer;
        }

        public async Task<string> DeleteAnswer(string id)
        {
            var answer = _unitOfWork.GetRepository<Answer>();
            var existingAnswer = await answer.GetByIdAsync(id);
            if (existingAnswer == null)
            {
                throw new Exception("Answer not found");
            }
            answer.Delete(existingAnswer);
            await _unitOfWork.SaveChanges();
            return "Answer Deleted Sunccessfully";
        }
        private async Task<string?> SaveFiles(RequestAnswerDto request)
        {
            var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Answers", "Images");
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
