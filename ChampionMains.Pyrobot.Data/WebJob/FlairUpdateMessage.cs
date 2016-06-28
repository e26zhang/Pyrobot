﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChampionMains.Pyrobot.Data.WebJob
{
    public class FlairUpdateMessage
    {
        public int UserId { get; set; }
        public string SubRedditName { get; set; }
        public bool RankEnabled { get; set; }
        public bool ChampionMasteryEnabled { get; set; }
        public string FlairText { get; set; }
    }
}
