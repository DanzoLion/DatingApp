using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace API.Entities
{
    public class Group
    {
        public Group()   // default constructor req. as Group is an Entity  // empty constructor req. for creating empty tables
        {
        }

        public Group(string name)
        {
            Name = name;
          //  Connections = connections;    // removed as we just initialise with the name of the group
        }

        [Key]
        public string Name { get; set; }     // the only property that will act as a key  // [Key] will name and andex the group for use with Name
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();   
    }
}