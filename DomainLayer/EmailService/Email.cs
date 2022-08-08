using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.EmailService
{
    public class Email
    {
        public string toEmail { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
    }
}
