﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ChampionMains.Pyrobot.Data;
using ChampionMains.Pyrobot.Data.Models;

namespace ChampionMains.Pyrobot.Services
{
    public class LeagueUpdateService : ILeagueUpdateService
    {
        private readonly IUnitOfWork _context;
        private readonly ApplicationConfiguration _config;

        public LeagueUpdateService(IUnitOfWork context, ApplicationConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<ICollection<Summoner>> GetSummonersForUpdateAsync(int max)
        {
            var cutoff = DateTimeOffset.Now - _config.LeagueDataStaleTime;
            var query = from summoner in _context.Summoners
                        where summoner.SummonerInfo.UpdatedTime.HasValue
                              && summoner.SummonerInfo.UpdatedTime < cutoff
                        select summoner;
            return await query.Take(max).ToListAsync();
        }

        public async Task<bool> SetUpdatedAsync(IEnumerable<Summoner> summoners)
        {
            foreach (var s in summoners)
            {
                s.SummonerInfo.UpdatedTime = DateTimeOffset.Now;
            }
            return await _context.SaveChangesAsync() > 0;
        } 
    }
}