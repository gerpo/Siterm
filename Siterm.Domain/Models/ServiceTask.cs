using System;
using System.Collections.Generic;
using System.Text;

namespace Siterm.Domain.Models
{
    public class ServiceTask
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public string Comment { get; set; }
    }
}
