using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public class MissionRatingRepository : Repository<MissionRating>, IMissionRatingRepository
    {
        AppDbContext _appDbContext;
        public MissionRatingRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext= appDbContext;
        }

        public void AddOrUpdateRatings(int userId, int missionId, byte rating)
        {
            var ratingsObj = _appDbContext.MissionRatings.FirstOrDefault(m => m.MissionId == missionId && m.UserId == userId);
            MissionRating missionRatingObj = new()
            {
                UserId = userId,
                MissionId = missionId,
                Rating = rating,
                CreatedAt = DateTimeOffset.Now
            };
            if (ratingsObj != null)
            {
                ratingsObj.Rating = rating;

                _appDbContext.MissionRatings.Update(ratingsObj);
            }
            else
            {
                _appDbContext.MissionRatings.Add(missionRatingObj);
            }
            _appDbContext.SaveChanges();

        }
    }
}
