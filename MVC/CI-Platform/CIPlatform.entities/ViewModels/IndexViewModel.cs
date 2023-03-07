using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class IndexViewModel
    {
        public List<CityViewModel> CityList { get; set; }
        public List<CountryViewModel> CountryList { get;set; }
        public List<PlatformLandingViewModel> MissionList { get; set; }
        public List<SkillsViewModel> SkillsList { get; set; }

        public List<ThemeViewModel> ThemeList { get; set; }
    }
}
