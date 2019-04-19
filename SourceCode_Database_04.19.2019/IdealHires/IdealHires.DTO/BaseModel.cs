using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO
{
    public class BaseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public bool IsEmailed { get; set; }
        public bool IsApp { get; set; }
    }
}
