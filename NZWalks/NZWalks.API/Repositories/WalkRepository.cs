using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
//using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public WalkRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }
        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();
            await nZWalksDBContext.Walks.AddAsync(walk);
            await nZWalksDBContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Models.Domain.Walk> DeleteAsync(Guid id)
        {
            var existingWalks = await nZWalksDBContext.Walks.FindAsync(id);
            if (existingWalks == null)
            {
                return null;
            }
             nZWalksDBContext.Walks.Remove(existingWalks);
                await nZWalksDBContext.SaveChangesAsync();
                return existingWalks;

            
        }

        public async Task<IEnumerable<Models.Domain.Walk>> GetAllAsync()
        {
            return await nZWalksDBContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
               .ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid id) //not working
        {

            return await nZWalksDBContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var ExistingWalk = await nZWalksDBContext.Walks.FindAsync(id);
            if (ExistingWalk != null)
            {
                ExistingWalk.Length = walk.Length;
                ExistingWalk.Name = walk.Name;
                ExistingWalk.RegionId = walk.RegionId;
                ExistingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                await nZWalksDBContext.SaveChangesAsync();
                return ExistingWalk;
            }
            else
            {
                return null;
            }
        }
    }
}
