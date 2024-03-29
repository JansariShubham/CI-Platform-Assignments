﻿using CIPlatform.entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.IRepository
{
    public interface IBannerRepository : IRepository<Banner>
    {
        public int ChangeStatus(int bannerId, bool status);

        public void Update(Banner banner);
    }
}
