namespace Siterm.Domain.Models
{
    public class ServiceTask : DomainObject
    {
        public string Area { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public string Comment { get; set; }
    }
}