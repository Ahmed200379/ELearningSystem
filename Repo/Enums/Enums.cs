using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Enums
{
    public enum Role{
        Student,
        Teacher,
        Admin,
        SuperAdmin,
    }
    public enum TypeOfMaterial
    {
        HomeWork,
        Book,
        Reference,
        Exam,
        Reverision,
        video
    }
    public enum PaymentMethod
    {
        VodaphoneCash,
        InstaPay,
        VesaPay,
        FawryPay,
    }
    public enum QuestionType
    {
        Choose
    }
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Canceled
    }
}
