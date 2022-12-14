using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Movie.Models
{
    [BsonIgnoreExtraElements]
    public class MovieReview
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MovieId { get; set; }
        public string Title { get; set; }
        public Review[] Review { get; set; } = new Review[0];
    }

    public class Review
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public string Comment { get; set; }
    }
}