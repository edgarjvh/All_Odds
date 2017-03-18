using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllOdds.Models
{
    public class Total
    {
        public List<Odd> odds { get; set; }

        public Total()
        {
            odds = new List<Odd>();
        }
    }
}