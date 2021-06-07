using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineField
{
    class HitRateCalculation
    {
        protected internal static string HitRate(int Hits, int Tries)
        {
            double dblPercent;

            // Percent of Hits out of Tries or total games
            // Convert Integers to Double to get correct calculation
            dblPercent = Convert.ToDouble(Hits) / Convert.ToDouble(Tries);

            // Round and format decimal as percent
            return dblPercent.ToString("P");
        }
    }
}
