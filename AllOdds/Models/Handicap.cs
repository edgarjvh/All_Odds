using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllOdds.Models
{
    public class Handicap
    {
        public List<Odd> odds { get; set; }

        public Handicap()
        {
            odds = new List<Odd>();
        }
    }
}