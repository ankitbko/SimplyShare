using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SimplyShare.Tracker.Models;
using System.Threading.Tasks;

namespace SimplyShare.Tracker.Repository
{
    public class SharingContextRepository : ISharingContextRepository
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly CosmosOption _options;
        private readonly IMongoCollection<SharingContext> _collection;

        public SharingContextRepository(IMongoDatabase mongoDatabase, IOptions<CosmosOption> options)
        {
            _mongoDatabase = mongoDatabase;
            _options = options.Value;
            _collection = _mongoDatabase.GetCollection<SharingContext>(_options.CollectionName);
        }

        public Task CreateSharingContext(SharingContext context)
        {
            return _collection.InsertOneAsync(context);
        }

        public async Task<SharingContext> GetSharingContextForUserByInfoHash(string userId, string infoHash)
        {
            var builder = Builders<SharingContext>.Filter;
            var filter = 
                builder.Eq(context => context.User.Id, userId) 
                & builder.Eq(context => context.InfoHash, infoHash);
            var result = await _collection.FindAsync(filter);
            return await result.FirstOrDefaultAsync();
        }
    }
}
