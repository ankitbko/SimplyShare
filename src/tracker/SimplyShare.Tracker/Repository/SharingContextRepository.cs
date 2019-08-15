using MongoDB.Driver;
using SimplyShare.Tracker.Models;
using System.Threading.Tasks;

namespace SimplyShare.Tracker.Repository
{
    public class SharingContextRepository
    {
        private const string COLLECTION_NAME = "share";

        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<SharingContext> _collection;

        public SharingContextRepository(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
            _collection = _mongoDatabase.GetCollection<SharingContext>(COLLECTION_NAME);
        }

        public Task CreateSharingContextRepository(SharingContext context)
        {
            return _collection.InsertOneAsync(context);
        }

        public async Task<SharingContext> GetSharingContextForUserByInfoHash(string userId, string infoHash)
        {
            var builder = Builders<SharingContext>.Filter;
            var filter = builder.Eq(context => context.User.Id, userId) & builder.Eq(context => context.InfoHash, infoHash);
            var result = await _collection.FindAsync(filter);
            return await result.FirstOrDefaultAsync();
        }
    }
}
