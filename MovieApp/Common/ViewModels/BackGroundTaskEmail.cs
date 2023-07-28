using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class BackGroundTaskEmail
    {
        public string Subject { get; set; }
        public IEnumerable<string> ReceiverEmail { get; set; }
        public string Message { get; set; }
    }
}
