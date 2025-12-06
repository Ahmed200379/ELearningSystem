
using Domain.Entities;
using Domain.Interfaces;
using Services.Abstractions;
using Shared.Dtos.Answer;
using Shared.Dtos.Question;
using Shared.Dtos.Quiz;
using Shared.Helpers;
using System.Linq.Expressions;

namespace Services
{
    public class QuizService : IQuizService
    {
        private readonly IUnitOfWork _unitOfWork;
        public QuizService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ResponseQuizDto>> GetAll()
        {
            Expression<Func<Quiz, object>>[] includes = {
                q => q.Questions!.Select(ques => ques.Answers)
            };
            var quizzes = await _unitOfWork.GetRepository<Quiz>().GetAllAsyncs
                (
                includes : includes
                );

            var response = quizzes.Select(q => new ResponseQuizDto
            {
                Id = q.Id,
                NumberOfQuestions = q.NumberOfQuestions,
                Description = q.Description,
                CreationDate = q.CreationDate,
                Points = q.Points,
                Questions = q.Questions != null ? q.Questions.Select(ques => new ResponseQuestionWithExamDto
                {
                    Id = ques.Id,
                    Title = ques.Title,
                    Image = ques.Image,
                    Type = ques.Type,
                    TimeLimit = ques.TimeLimit,
                    Answers = ques.Answers != null ? ques.Answers.Select(a => new ResponseAnswerWithoutQuestionIdDto
                    {
                        Id = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect,
                        Image = a.Image,
                    }).ToList() : new List<ResponseAnswerWithoutQuestionIdDto>(),
                }).ToList() : new List<ResponseQuestionWithExamDto>()
            });
            return response;
        }
        public async Task<Quiz> Add(RequestQuizDto requet)
        {
            saveFiles(requet);
            var quiz = new Quiz
            {
                Id = Guid.NewGuid().ToString(),
                NumberOfQuestions = requet.NumberOfQuestions,
                Description = requet.Description,
                CreationDate = requet.CreationDate,
                Points = requet.Points,
                Image = requet.Image != null ? $"{BaseUrl.BaseUrlValue}/Quizzes/images/{requet.Image.FileName}" : null,

            };
            await _unitOfWork.GetRepository<Quiz>().AddAsync(quiz);
            await _unitOfWork.SaveChanges();
            foreach (var q in requet.Questions) 
            {
                var question = new Question
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = q.Title,
                    Image = q.Image != null ? $"{BaseUrl.BaseUrlValue}/Questions/images/{q.Image.FileName}" : null,
                    Type = q.Type,
                    TimeLimit = q.TimeLimit,
                    QuizId = quiz.Id,
                };
                await _unitOfWork.GetRepository<Question>().AddAsync(question);
                if(q.Answers != null && q.Answers.Any())
                {
                    foreach (var a in q.Answers)
                    {
                        var answer = new Answer
                        {
                            Id = Guid.NewGuid().ToString(),
                            Text = a.Text,
                            IsCorrect = a.IsCorrect,
                            Image = q.Image != null ? $"{BaseUrl.BaseUrlValue}/Answers/images/{q.Image.FileName}" : null,
                            QuestionId = question.Id,
                        };
                        await _unitOfWork.GetRepository<Answer>().AddAsync(answer);
                    }
                }
                await _unitOfWork.SaveChanges();
            }
            return quiz;
        }
        public async Task<Quiz> Update(string id, RequestUpdateQuizeDto request)
        {
            var oldquize = await _unitOfWork.GetRepository<Quiz>().GetByIdAsync(id);
            if (oldquize == null)
                throw new Exception("Quize not found");
            await saveUpdateFiles(request);

            //update
            if (request.NumberOfQuestions != null)
                oldquize.NumberOfQuestions = request.NumberOfQuestions;
            if (request.Points != null)
                oldquize.Points = request.Points;
            if (request.Description != null)
                oldquize.Description = request.Description;
            oldquize.Image = request.Image != null ? $"{BaseUrl.BaseUrlValue}/Quizzes/images/{request.Image.FileName}" : oldquize.Image;

            _unitOfWork.GetRepository<Quiz>().Update(oldquize);

            //update or Add Question 
            if (request.Questions != null && request.Questions.Any())
            {
                foreach (var q in request.Questions)
                {
                    Question question;

                    if (!string.IsNullOrWhiteSpace(q.Id))
                        question = await _unitOfWork.GetRepository<Question>().GetByIdAsync(q.Id);
                    else
                        question = null;

                    if (question != null)
                    {
                        //update question
                        question.Title = q.Title;
                        question.Image = q.Image != null ? $"{BaseUrl.BaseUrlValue}/Questions/images/{q.Image.FileName}" : question.Image;
                        question.Type = q.Type;
                        question.TimeLimit = q.TimeLimit;
                        _unitOfWork.GetRepository<Question>().Update(question);
                    }
                    else
                    {
                        //add new question
                        question = new Question
                        {
                            Id = Guid.NewGuid().ToString(),
                            Title = q.Title,
                            Image = q.Image != null ? $"{BaseUrl.BaseUrlValue}/Questions/images/{q.Image.FileName}" : null,
                            Type = q.Type,
                            TimeLimit = q.TimeLimit,
                            QuizId = oldquize.Id,
                        };
                        await _unitOfWork.GetRepository<Question>().AddAsync(question);
                        await _unitOfWork.SaveChanges();
                    }
                    //update or add answers
                    if (q.Answers != null && q.Answers.Any())
                    {
                        foreach (var a in q.Answers)
                        {
                            Answer answer;
                            if (!string.IsNullOrWhiteSpace(a.Id))
                                answer = await _unitOfWork.GetRepository<Answer>().GetByIdAsync(a.Id);
                            else
                                answer = null;
                            if (answer != null)
                            {
                                //update answer
                                answer.Text = a.Text;
                                answer.IsCorrect = a.IsCorrect;
                                answer.Image = a.Image != null ? $"{BaseUrl.BaseUrlValue}/Answers/images/{a.Image.FileName}" : answer.Image;
                                _unitOfWork.GetRepository<Answer>().Update(answer);
                            }
                            else
                            {
                                //add new answer
                                answer = new Answer
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Text = a.Text,
                                    IsCorrect = a.IsCorrect,
                                    Image = a.Image != null ? $"{BaseUrl.BaseUrlValue}/Answers/images/{a.Image.FileName}" : null,
                                    QuestionId = question.Id,
                                };
                                await _unitOfWork.GetRepository<Answer>().AddAsync(answer);
                                await _unitOfWork.SaveChanges();
                            }
                        }
                    }
                }
            }
            await _unitOfWork.SaveChanges();
            return oldquize;
        }
        public async Task<string> Delete(string id)
        {
            var quiz = await _unitOfWork.GetRepository<Quiz>().GetByIdAsync(id);
            if (quiz == null)
                throw new Exception("Quiz not found");
            
            _unitOfWork.GetRepository<Quiz>().Delete(quiz);
            await _unitOfWork.SaveChanges();
            return "Quiz deleted successfully";
        }
        private async Task<string?> saveFiles(RequestQuizDto request)
        {
            var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "quizzes");
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
        private async Task<string?> saveUpdateFiles(RequestUpdateQuizeDto request)
        {
            var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "quizzes");
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
