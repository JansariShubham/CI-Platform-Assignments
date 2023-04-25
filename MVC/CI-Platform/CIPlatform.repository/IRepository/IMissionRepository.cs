using CIPlatform.entities.DataModels;
using CIPlatform.entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.IRepository
{
    public interface IMissionRepository : IRepository<Mission>
    {
        
        public List<Mission> getAllMissions();

        public List<Mission> getFilters(Filters obj);

        public List<Mission> GetSearchedMissionList(string? searchText);



    }
}
