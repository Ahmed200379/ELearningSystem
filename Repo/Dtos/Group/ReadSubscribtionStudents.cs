using Shared.Dtos.Subscribe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Group
{
    public class ReadSubscribtionStudents
    {
        public List<ReadStudentDto> Students { get; set; } = new List<ReadStudentDto>();
    }
}
