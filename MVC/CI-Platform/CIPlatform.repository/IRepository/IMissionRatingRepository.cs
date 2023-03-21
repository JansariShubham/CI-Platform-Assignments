using CIPlatform.entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.IRepository
{
    public interface IMissionRatingRepository : IRepository<MissionRating>
    {
       
       public  void AddOrUpdateRatings(int userId, int missionId, byte rating);
    }
}
