﻿using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public class StoryRepository : Repository<Story>, IStoryRepository
    {
        private AppDbContext _appDbContext;
        public StoryRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public List<Story> getAllStories()
        {
            return _appDbContext.Stories.Include(stories => stories.User)
                .Include(stories => stories.StoryMedia).ToList();
        }
    }
}
