using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace peopledex
{
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string Birthday { get; set; }
        public string Likes { get; set; }
        public string Description { get; set; }
        public List<ProfileEvent> ProfileEvents { get; set; }
    }
}
