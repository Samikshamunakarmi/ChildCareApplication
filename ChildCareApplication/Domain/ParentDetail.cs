using MongoDB.Bson.Serialization.Attributes;

namespace ChildCareApplication.Domain
{
    public class ParentDetail
    {

        [BsonElement("firstname")]
        public string FirstName { get; set; }

        [BsonElement("lastname")]

        public string LastName { get; set; }

        [BsonElement("relationship")]
        public string Relationship { get; set; }

        [BsonElement("mobnumber")]
        public string ContactNumber { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("address")]
        public AddressDetail Address { get; set; }

    }
}
