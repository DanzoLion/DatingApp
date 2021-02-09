namespace API.Entities
{
    public class Connection    // Connection is an Entity  // iit needs a default constructor as it may show an error
    {
        public Connection()  // default constructor
        {
        }

        public Connection(string connectionId, string username)   // when we create a new connection we just open parenthesis and pass in these two properties
        {
            ConnectionId = connectionId;
            Username = username;
        }

        public string ConnectionId { get; set; }       // identity automatically considers this the name of the primary key if it is string of Connection
        public string Username { get; set; }
    }
}