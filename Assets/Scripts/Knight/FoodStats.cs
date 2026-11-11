using System;
using System.Collections.Generic;

namespace Knight
{
    public abstract class FoodStats
    {
        private List<Stats> _goodStats;
        private List<Stats> _badStats;

        public FoodStats(List<Stats> goodStats, List<Stats> badStats)
        {
            if (goodStats.Count == 0)
            {
                throw new Exception("A created item must hve at least 1 food that gives positive stats");
            }

            if (badStats.Count == 0)
            {
                throw new Exception("A created item must have at least 1 food that gives negative stats");
            }
        }
    }
}