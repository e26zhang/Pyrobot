﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace ChampionMains.Pyrobot.Services
{
    public class RoleService
    {
        private readonly ICollection<string> _admins; 

        public RoleService()
        {
            _admins = ConfigurationManager
                .AppSettings["security.admins"].Split(',').Select(x => x.Trim()).ToList();
        }

        public Task<bool> IsAdminAsync(string name)
        {
            return Task.FromResult(_admins.Contains(name, StringComparer.OrdinalIgnoreCase));
        }
    }
}