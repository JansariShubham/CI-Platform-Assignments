using CIPlatform.entities.DataModels;
using CIPlatform.entities.ViewModels;
using CIPlatform.repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CIPlatform.repository.Repository
{
    public class MissionRepository : Repository<Mission>, IMissionRepository
    {
        // var userId = Context.Session.GetString("userId");


        private AppDbContext _appDbContext;

        public MissionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public int ChangeMissionStatus(int missionId, bool status)
        {
            string query = "UPDATE mission SET isActive = {0} WHERE mission_id = {1}";
            return _appDbContext.Database.ExecuteSqlRaw(query, status, missionId);
        }
        public int CloseMission(long missionId)
        {
            string query = "UPDATE mission SET status = {0} WHERE mission_id = {1}";
            return _appDbContext.Database.ExecuteSqlRaw(query, false, missionId);
        }

        public List<Mission> getAllMissions()
        {
            var missionList = _appDbContext.Missions.Include(mission => mission.MissionApplications)
                .ThenInclude(u => u.User)
                 .Include(missions => missions.GoalMissions)
                 .Include(missions => missions.MissionMedia)
                 .Include(missions => missions.FavouriteMissions)
                 .Include(missions => missions.MissionSkills).ThenInclude(m => m.Skill)
                 .Include(missions => missions.Theme)
                 .Include(missions => missions.City)
                 .Include(missions => missions.Country)
                 .Include(missions => missions.MissionRatings)
                 .Include(missions => missions.MissionDocuments).ToList();

            return missionList;
        }





        public List<Mission> getFilters(Filters obj)
        {
            var mission = getAllMissions();
            var filterMission = mission;

            //var mission = _appDbContext.Missions.ToList();
            StringComparison comp = StringComparison.OrdinalIgnoreCase;

            PlatformLandingViewModel missionVm = new();

            if (obj.SearchText != "" && obj.SearchText != null)
            {
                filterMission = filterMission.Where(m => m.Title.Contains(obj.SearchText, comp)).ToList();
                //Console.WriteLine(mission.Count);
            }

            if (obj.Countries.Count() > 0)
            {
                filterMission = filterMission.Where(m => obj.Countries.Contains((int)m.CountryId)).ToList();
                //mission = filterMission;
            }

            if (obj.Themes.Count() > 0)
            {
                filterMission = filterMission.Where(m => obj.Themes.Contains((int)m.ThemeId)).ToList();
                //mission = filterMission;
            }
            if (obj.Cties.Count() > 0)
            {
                filterMission = filterMission.Where(m => obj.Cties.Contains((int)m.CityId)).ToList();
                // mission = filterMission;
            }
            if (obj.Skills.Count() > 0)
            {
                filterMission = filterMission.Where(m =>
                   m.MissionSkills.Any(ms => obj.Skills.Any(s => s == ms.SkillId))
                 ).ToList();


                // mission = filterMission;
                //var missionSkillObj = _appDbContext.MissionSkills.Where(m =>obj.Skills.ToList().Contains((int)m.MissionSkillId));

                //mission = mission.Where(m => obj.Skills.Any(s => m.MissionSkills.Any(ms => ms.MissionSkillId == s))).ToList();
            }
            if (obj.sortingList != 0)
            {
                if (obj.sortingList == 1)
                {
                    filterMission = filterMission.OrderByDescending(m => m.StartDate).ToList();

                }
                if (obj.sortingList == 2)
                {
                    filterMission = filterMission.OrderBy(m => m.StartDate).ToList();

                }
                if (obj.sortingList == 3)
                {
                    filterMission = filterMission.OrderBy(m => m.TotalSeats - m.MissionApplications.Where(ma => ma.ApprovalStatus == 1).Count()).ToList();


                }
                if (obj.sortingList == 4)
                {
                    filterMission = filterMission.OrderByDescending(m => m.TotalSeats - m.MissionApplications.Where(ma => ma.ApprovalStatus == 1).Count()).ToList();

                }
                if (obj.sortingList == 5)
                {
                    if (obj.userId != 0)
                    {
                        //    var favMissions = filterMission.SelectMany(m => m.FavouriteMissions);

                        //    var favMissionsId = favMissions.Select(msn => msn.UserId);
                        //    filterMission = filterMission.Where(m => favMissionsId.Contains(obj.userId)).ToList();

                        filterMission = filterMission.Where(m => m.FavouriteMissions.Any(fm => fm.UserId == obj.userId)).ToList();
                    }

                }
                if (obj.sortingList == 6)
                {
                    filterMission = filterMission.OrderByDescending(m => m.EndDate).ToList();

                }
            }

            if (obj.Explore != 0)
            {
                //top themes
                if (obj.Explore == 7)
                {

                    filterMission = filterMission.GroupBy(m => m.Theme.MissionThemeId).OrderByDescending(g => g.Count()).SelectMany(g => g).ToList();

                }
                //most ranked
                if (obj.Explore == 8)
                {
                    var query = @"
                                SELECT m.*, avg_rating FROM mission AS m 
                                LEFT JOIN (SELECT mission_id, AVG(rating) AS avg_rating FROM mission_rating GROUP BY mission_id) AS mr 
                                ON m.mission_id = mr.mission_id 
                                ORDER BY mr.avg_rating DESC";

                    filterMission = _appDbContext.Missions.FromSqlRaw(query).ToList();
                    //filterMission =  filterMission.OrderByDescending(m => m?.MissionRatings?.Average(mr => mr.Rating)).ToList();
                    //var query = from m in _appDbContext.Missions
                    //            join mr in _appDbContext.MissionRatings on m.MissionId equals mr.MissionId
                    //            group mr by mr.MissionId into g
                    //            orderby g.Average(x => x.Rating) descending
                    //            select new
                    //            {
                    //                missionid = g.Key,
                    //                avgrating = g.Average(x => x.Rating)
                    //            };

                    //var missionids = query.Select(x => x.missionid).ToList();
                    //List<Mission> topratedmission = new();

                    ////filtermission = null;
                    //missionids.ForEach((mid) =>
                    // {

                    //     topratedmission.Add(GetAll().FirstOrDefault(m => m.MissionId == mid));
                    // }) ;
                    //filterMission = topratedmission;
                }
                //top favourites
                if (obj.Explore == 9)
                {
                    filterMission = filterMission.OrderByDescending(fm => fm.FavouriteMissions.Count()).ToList();
                }
                //random
                if (obj.Explore == 10)
                {
                    Random random = new();
                    var filterMissionCnt= getAllMissions().Count();
                    int randomIndex = random.Next(filterMissionCnt);
                    filterMission = filterMission.Skip(randomIndex).Take(filterMissionCnt).ToList();

                }
            }

            return filterMission;
            //return filterMission;
            //



        }

        public Mission GetMissionById(long missionId)
        {

            var missions = _appDbContext.Missions.Include(missions => missions.GoalMissions)
                 .Include(missions => missions.MissionMedia)

                 .Include(missions => missions.MissionSkills).ThenInclude(m => m.Skill)
                 .Include(missions => missions.Theme)
                 .Include(missions => missions.City)
                 .Include(missions => missions.Country)

                 .Include(missions => missions.MissionDocuments).FirstOrDefault(m => m.MissionId == missionId);
            return missions!;
        }

        public List<Mission> GetSearchedMissionList(string? searchText)
        {
            if (searchText != null && searchText != "")
            {
                var mission = getAllMissions();

                return mission.Where(m => m.Title.ToLower().Contains(searchText.ToLower())).ToList();
            }
            return null!;
        }

        public void Update(Mission mission)
        {
            _appDbContext.Missions.Update(mission);
        }
    }
}
