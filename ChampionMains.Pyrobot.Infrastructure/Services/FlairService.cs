﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ChampionMains.Pyrobot.Data;
using ChampionMains.Pyrobot.Data.Models;

namespace ChampionMains.Pyrobot.Services
{
    public class FlairService
    {
        private readonly UnitOfWork _context;
        private readonly TimeSpan _staleTime;

        public FlairService(UnitOfWork context, ApplicationConfiguration config)
        {
            _context = context;
            _staleTime = config.FlairStaleTime;
        }

        public async Task<ICollection<SubredditUserFlair>> GetFlairsForUpdateAsync()
        {
            var staleAfter = DateTimeOffset.Now - _staleTime;
            return await _context.SubredditUserFlairs.Where(s => s.LastUpdate < staleAfter)
                    .Include(x => x.User).Include(x => x.Subreddit).ToListAsync();
        }


        public async Task<ICollection<SubredditUserFlair>> GetSubredditUserFlairs(User user)
        {
            return await _context.SubredditUserFlairs.Where(f => f.User.Id == user.Id).ToListAsync();
        }

        public async Task<bool> UpdateSubredditUserFlair(int userId, int subredditId, bool rankEnabled,
            bool championMasteryEnaabled, bool prestigeEnabled, string flairText)
        {
            var subredditUserFlair =
                _context.SubredditUserFlairs.FirstOrDefault(u => u.UserId == userId && u.SubredditId == subredditId);

            if (subredditUserFlair == null)
            {
                var subreddit = _context.Subreddits.Find(subredditId);
                if (subreddit == null)
                    return false;

                subredditUserFlair = new SubredditUserFlair
                {
                    UserId = userId,
                    SubredditId = subredditId
                };
                _context.SubredditUserFlairs.Add(subredditUserFlair);
            }

            subredditUserFlair.RankEnabled = rankEnabled;
            subredditUserFlair.ChampionMasteryEnabled = championMasteryEnaabled;
            subredditUserFlair.PrestigeEnabled = prestigeEnabled;
            subredditUserFlair.FlairText = flairText;

            subredditUserFlair.LastUpdate = DateTimeOffset.Now;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}