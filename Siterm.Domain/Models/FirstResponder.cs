using System;

namespace Siterm.Domain.Models
{
    public class FirstResponder
    {
        public FirstResponder(string name, string facility, string phone, string lastTraining, string nextTraining)
        {
            Name = name;
            Facility = facility;
            Phone = phone;
            LastTraining = DateTime.Parse(lastTraining);
            NextTraining = DateTime.Parse(nextTraining);
        }

        public string Name { get; set; }
        public string Facility { get; set; }
        public string Phone { get; set; }
        public DateTime LastTraining { get; set; }
        public DateTime NextTraining { get; set; }
    }
}