using System.Collections.Generic;

namespace Siterm.Domain.Models
{
    public class Facility : DomainObject
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int OrderNr { get; set; }
        public IEnumerable<Device> Devices { get; set; }
    }
}