using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public WalkDifficultyRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }
        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficult)
        {
            walkDifficult.Id = Guid.NewGuid();
            await nZWalksDBContext.WalkDifficulty.AddAsync(walkDifficult);
            await nZWalksDBContext.SaveChangesAsync();
            return walkDifficult;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingwalkdifficulty = await nZWalksDBContext.WalkDifficulty.FindAsync(id);

            if (existingwalkdifficulty != null)
            {
                nZWalksDBContext.WalkDifficulty.Remove(existingwalkdifficulty);
                await nZWalksDBContext.SaveChangesAsync();
                return existingwalkdifficulty;
            }

            return null;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDBContext.WalkDifficulty.ToListAsync();

        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await nZWalksDBContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingwalkdifficulty = await nZWalksDBContext.WalkDifficulty.FindAsync(id);

            if (existingwalkdifficulty == null)
            {
                return null;
            }

            existingwalkdifficulty.Code = walkDifficulty.Code;
            await nZWalksDBContext.SaveChangesAsync();
            return existingwalkdifficulty;
        }
    }
}
