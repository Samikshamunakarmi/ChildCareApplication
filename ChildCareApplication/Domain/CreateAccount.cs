using MongoDB.Bson.Serialization.Attributes;

namespace ChildCareApplication.Domain
{
    public class CreateAccount : BaseEntities
    {
        [BsonElement("username")]
        public string UserName { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("phonenum")]
        public int PhoneNumber { get; set; }

        [BsonElement("salt")]
        public string Salt { get; set; }
    }
}
