using MongoDB.Bson.Serialization.Attributes;

namespace BusinessLogic.Data
{
    public class Customer
    {
        [BsonId]
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string City { get; set; }

        public string Email { get; set; }
    }
}
