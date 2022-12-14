using Movie.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Movie.Services
{
    public class MovieReviewService
    {
        private readonly IMongoCollection<MovieReview> _movieReviewCollection;

        public MovieReviewService(
            IOptions<DBSetting> movieReviewDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                movieReviewDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                movieReviewDatabaseSettings.Value.DatabaseName);

            _movieReviewCollection = mongoDatabase.GetCollection<MovieReview>(
                movieReviewDatabaseSettings.Value.CollectionName);
        }

        public async Task<List<MovieReview>> GetAsync() =>
            await _movieReviewCollection.Find(_ => true).ToListAsync();

        public async Task<MovieReview?> GetAsync(string id) =>
            await _movieReviewCollection.Find(x => x.MovieId == id).FirstOrDefaultAsync();

        public async Task CreateAsync(MovieReview newMovieReview)
        {
            await _movieReviewCollection.UpdateOneAsync(
                    Builders<MovieReview>.Filter.Eq(x => x.MovieId, newMovieReview.MovieId),
                    Builders<MovieReview>.Update.PushEach(x => x.Review, newMovieReview.Review));
        }

        public async Task UpdateAsync(MovieReview updatedMovieReview) 
        { 
            var filter = Builders<MovieReview>.Filter.Eq(x => x.MovieId, updatedMovieReview.MovieId);

            var update = Builders<MovieReview>.Update.Set("Review.$[f].Comment", updatedMovieReview.Review.Select(r => r.Comment).FirstOrDefault());

            var ids = string.Join(',',  updatedMovieReview.Review.Select(r => $"ObjectId('{r.UserId}')"));

            var arrayFilters = new[]
            {
                new JsonArrayFilterDefinition<BsonDocument>($"< \"f.UserId\" : <$in: [{ids}] >>".Replace("<", "{").Replace(">", "}"))
            };

            await _movieReviewCollection.UpdateOneAsync(filter, update, new UpdateOptions { ArrayFilters = arrayFilters });
        }

        public async Task RemoveAsync(MovieReview deletedMovieReview)
        {
            var filter = Builders<MovieReview>.Filter.Eq(x => x.MovieId, deletedMovieReview.MovieId);

            var delete = Builders<MovieReview>.Update.PullFilter("Review", Builders<Review>.Filter.Eq("UserId", deletedMovieReview.Review.Select(r => r.UserId).FirstOrDefault()));

            await _movieReviewCollection.FindOneAndUpdateAsync(filter, delete);
        }
    }
}