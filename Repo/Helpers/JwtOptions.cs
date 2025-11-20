using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers
{
   public class JwtOptions
    {
    public string Isusser {  get; set; }
    public string Audience { get; set; }
    public int LifeTime { get; set; }
    public string SigningKey {  get; set; }
    }
}
