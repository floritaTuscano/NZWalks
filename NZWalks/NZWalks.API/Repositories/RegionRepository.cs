using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;
        public RegionRepository(NZWalksDBContext nZWalksDBContext) 
        {
            this.nZWalksDBContext = nZWalksDBContext;//inject the service into the class
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await nZWalksDBContext.AddAsync(region);
            await nZWalksDBContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await nZWalksDBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region != null)
            {
                nZWalksDBContext.Regions.Remove(region);
                await nZWalksDBContext.SaveChangesAsync();
                return region;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nZWalksDBContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await nZWalksDBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var ExistingRegion = await nZWalksDBContext.Regions.FindAsync(id);
            if (ExistingRegion == null)
            {
                return null;
            }
            else
            {
                ExistingRegion.Code = region.Code;
                ExistingRegion.Area = region.Area;
                ExistingRegion.Name = region.Name;
                ExistingRegion.Lat = region.Lat;
                ExistingRegion.Long = region.Long;
                ExistingRegion.Population = region.Population;

                await nZWalksDBContext.SaveChangesAsync();

                return ExistingRegion;
            }
        }

    }
}
