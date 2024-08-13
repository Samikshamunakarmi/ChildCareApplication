using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChildCareApplication.Domain
{
    public class BaseEntities
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id {  get; set; }

        [BsonElement("datetime")]
        public DateTime DateAdded { get; set; }
    }
}
