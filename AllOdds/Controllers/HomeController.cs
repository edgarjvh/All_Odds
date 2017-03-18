using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using AllOdds.Models;

namespace AllOdds.Controllers
{
    public class HomeController : Controller
    {
        private static string strConn = ConfigurationManager.ConnectionStrings["defaultConn"].ConnectionString;

        public ActionResult Index(string dep = "")
        {
            if (!dep.Equals(""))
            {
                ViewBag.SqlDep = dep;
            }
            return View();
        }

        public string getCategories()
        {
            using (SportFeedsDBEntities ent = new SportFeedsDBEntities())
            {
                var c = from a in ent.Categories
                        select new
                        {
                            category_id = a.category_id,
                            category_name = a.name
                        };
                c = c.OrderBy(d => d.category_name);

                string result = Newtonsoft.Json.JsonConvert.SerializeObject(c);
                return result;
            }
        }
        
        [HttpPost]
        public ActionResult getMatches(int category_id, string dateFrom, string dateTo)
        {
            string queryMatches = @"select " +
                "m.match_id, " +                
                "m.status," +
                "m.date_time," +
                "m.stadium," +
                "m.category_id," +
                "c.name as 'category_name'," +
                "m.localteam_id," +
                "lt.name as 'localteam_name', " +
                "m.visitorteam_id," +
                "vt.name as 'visitorteam_name'," +
                "m.ht_score," +
                "m.localteam_goals," +
                "m.visitorteam_goals " +                
                "from dbo.matches as m left join dbo.categories as c on m.category_id = c.category_id " +
                "left join teams as lt on m.localteam_id = lt.team_id " +
                "left join teams as vt on m.visitorteam_id = vt.team_id";

            //queryMatches += dateFrom.Equals(dateTo) ?
            //    " where CONVERT(date, m.date_time) = @dateFrom" :
            //    " where CONVERT(date, m.date_time) between @dateFrom and @dateTo";

            queryMatches += dateFrom.Equals(dateTo) ?
                " where CONVERT(varchar(10), m.date_time, 120) = @dateFrom" :
                " where CONVERT(varchar(10), m.date_time, 120) between @dateFrom and @dateTo";

            queryMatches += category_id > 0 ?
                " and m.category_id = @category_id" :
                "";
            
            queryMatches += " order by m.date_time ASC";

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();

                SqlCommand cmdMatches = new SqlCommand(queryMatches, conn);
                cmdMatches.Parameters.AddWithValue("@dateFrom", DateTime.Parse(dateFrom));

                if (queryMatches.Contains("@dateTo"))
                {
                    cmdMatches.Parameters.AddWithValue("@dateTo", DateTime.Parse(dateTo)); 
                }

                if (queryMatches.Contains("@category_id"))
                {
                    cmdMatches.Parameters.AddWithValue("@category_id", category_id);
                }

                SqlDataAdapter daMatches = new SqlDataAdapter(cmdMatches);
                DataSet dsMatches = new DataSet();
                daMatches.Fill(dsMatches);

                List<Matches> matches = new List<Matches>();

                if (dsMatches.Tables[0].Rows.Count > 0)
                {                    
                    // loop matches
                    for (int m = 0; m < dsMatches.Tables[0].Rows.Count; m++)
                    {
                        DataRow mRow = dsMatches.Tables[0].Rows[m];

                        if (mRow["match_id"] != DBNull.Value)
                        {
                            Matches match = new Matches();
                            match.match_id = (long)mRow["match_id"];
                            match.status = mRow["status"] is DBNull ? "no status" : mRow["status"].ToString();
                            match.datetime = mRow["date_time"] is DBNull ? "no date" : ((DateTime)mRow["date_time"]).ToString("dd-MM-yyyy HH:mm");
                            match.stadium = mRow["stadium"] is DBNull ? "no stadium" : mRow["stadium"].ToString();
                            match.category_name = mRow["category_name"] is DBNull ? "no category" : mRow["category_name"].ToString();
                            match.localteam_name = mRow["localteam_name"] is DBNull ? "no team" : mRow["localteam_name"].ToString();
                            match.localteam_goals = mRow["localteam_goals"] is DBNull ? -1 : (int)mRow["localteam_goals"];
                            match.visitorteam_name = mRow["visitorteam_name"] is DBNull ? "no team" : mRow["visitorteam_name"].ToString();
                            match.visitorteam_goals = mRow["visitorteam_goals"] is DBNull ? -1 : (int)mRow["visitorteam_goals"];

                            //get events
                            string queryEvents = @"select " +
                                "e.event_id, " +
                                "e.type, " +
                                "e.minute, " +
                                "e.team, " +
                                "e.result, " +
                                "p1.name as 'player_name', " +
                                "p2.name as 'assist_name' " +
                                "from events as e " +
                                "left join players as p1 on e.player_id = p1.player_id " +
                                "left join players as p2 on e.assist_id = p1.player_id " +
                                "where e.match_id = @match_id " +
                                "order by e.minute asc";

                            SqlCommand cmdEvents = new SqlCommand(queryEvents, conn);
                            cmdEvents.Parameters.AddWithValue("@match_id", match.match_id);
                            SqlDataAdapter daEvents = new SqlDataAdapter(cmdEvents);
                            DataSet dsEvents = new DataSet();
                            daEvents.Fill(dsEvents);
                            
                            if (dsEvents.Tables[0].Rows.Count > 0)
                            {
                                for (int e = 0; e < dsEvents.Tables[0].Rows.Count; e++)
                                {
                                    DataRow eRow = dsEvents.Tables[0].Rows[e];

                                    Events _event = new Events();
                                    _event.event_id = (long)eRow["event_id"];
                                    _event.type = eRow["type"] is DBNull ? "" : eRow["type"].ToString();
                                    _event.minute = eRow["minute"] is DBNull ? "" : eRow["minute"].ToString();
                                    _event.team = eRow["team"] is DBNull ? "" : eRow["team"].ToString();
                                    _event.player_name = eRow["player_name"] is DBNull ? "" : eRow["player_name"].ToString();
                                    _event.assist_name = eRow["assist_name"] is DBNull ? "" : eRow["assist_name"].ToString();
                                    _event.result = eRow["result"] is DBNull ? "" : eRow["result"].ToString();
                                    match.events.Add(_event);
                                }
                            }

                            //get odds
                            string queryOdds = @"select " +
                                "o.odd_id, " +
                                "o.name, " +
                                "o.value, " +
                                "o.handicap, " +
                                "o.total, " +
                                "b.name as 'bookmaker_name', " +
                                "ot.name as 'odd_type_name' " +
                                "from odds as o " +
                                "left join bookmakers as b on o.bookmaker_id = b.bookmaker_id " +
                                "left join oddtypes as ot on o.odd_type_id = ot.odd_type_id " +
                                "where o.match_id = @match_id " +
                                "order by o.odd_id asc";

                            SqlCommand cmdOdds = new SqlCommand(queryOdds, conn);
                            cmdOdds.Parameters.AddWithValue("@match_id", match.match_id);
                            SqlDataAdapter daOdds = new SqlDataAdapter(cmdOdds);
                            DataSet dsOdds = new DataSet();
                            daOdds.Fill(dsOdds);

                            if (dsOdds.Tables[0].Rows.Count > 0)
                            {
                                for (int o = 0; o < dsOdds.Tables[0].Rows.Count; o++)
                                {
                                    DataRow oRow = dsOdds.Tables[0].Rows[o];

                                    Odds odd = new Odds();
                                    odd.odd_id = (long)oRow["odd_id"];
                                    odd.name = oRow["name"] is DBNull ? "" : oRow["name"].ToString();
                                    odd.value = oRow["value"] is DBNull ? -10000 : (double)oRow["value"];
                                    odd.handicap = oRow["handicap"] is DBNull ? -10000 : (double)oRow["handicap"];
                                    odd.total = oRow["total"] is DBNull ? -10000 : (double)oRow["total"];
                                    odd.bookmaker_name = oRow["bookmaker_name"] is DBNull ? "" : oRow["bookmaker_name"].ToString();
                                    odd.odd_type_name = oRow["odd_type_name"] is DBNull ? "" : oRow["odd_type_name"].ToString();
                                    //match.odds.Add(odd);
                                }
                            }

                            matches.Add(match);
                        }

                        #region unused
                        //if (row["match_id"] is DBNull)
                        //{

                        //}else
                        //{


                        //    if (last_match_id == 0) // si es el primer match
                        //    {                                
                        //        match.match_id = incoming_match_id;
                        //        match.status = row["status"] is DBNull ? "no status" : row["status"].ToString();
                        //        match.datetime = row["date_time"] is DBNull ? "no date" : ((DateTime)row["date_time"]).ToString("dd-MM-yyyy HH:mm");
                        //        match.stadium = row["stadium"] is DBNull ? "no stadium" : row["stadium"].ToString();
                        //        match.category_name = row["category_name"] is DBNull ? "no category" : row["category_name"].ToString();
                        //        match.localteam_name = row["localteam_name"] is DBNull ? "no team" : row["localteam_name"].ToString();
                        //        match.localteam_goals = row["localteam_goals"] is DBNull ? -1 : (int)row["localteam_goals"];
                        //        match.visitorteam_name = row["visitorteam_name"] is DBNull ? "no team" : row["visitorteam_name"].ToString();
                        //        match.visitorteam_goals = row["visitorteam_goals"] is DBNull ? -1 : (int)row["visitorteam_goals"];

                        //        if (row["event_id"] is DBNull)
                        //        {

                        //        }
                        //        else
                        //        {
                        //            Events _event = new Events();
                        //            _event.event_id = (long)row["event_id"];
                        //            _event.type = row["type"] is DBNull ? "" : row["type"].ToString();
                        //            _event.minute = row["minute"] is DBNull ? "" : row["minute"].ToString();
                        //            _event.team = row["team"] is DBNull ? "" : row["team"].ToString();
                        //            _event.player_name = row["player_name"] is DBNull ? "" : row["player_name"].ToString();
                        //            _event.assist_name = row["assist_name"] is DBNull ? "" : row["assist_name"].ToString();
                        //            _event.result = row["result"] is DBNull ? "" : row["result"].ToString();
                        //            match.events.Add(_event);
                        //        }

                        //        last_match_id = match.match_id;
                        //    }
                        //    else
                        //    {
                        //        if (last_match_id == incoming_match_id)
                        //        {
                        //            Events _event = new Events();
                        //            _event.event_id = (long)row["event_id"];
                        //            _event.type = row["type"] is DBNull ? "" : row["type"].ToString();
                        //            _event.minute = row["minute"] is DBNull ? "" : row["minute"].ToString();
                        //            _event.team = row["team"] is DBNull ? "" : row["team"].ToString();
                        //            _event.player_name = row["player_name"] is DBNull ? "" : row["player_name"].ToString();
                        //            _event.assist_name = row["assist_name"] is DBNull ? "" : row["assist_name"].ToString();
                        //            _event.result = row["result"] is DBNull ? "" : row["result"].ToString();
                        //            match.events.Add(_event);
                        //        }
                        //        else
                        //        {
                        //            matches.Add(match);

                        //            match = new Matches();
                        //            match.match_id = incoming_match_id;
                        //            match.status = row["status"] is DBNull ? "no status" : row["status"].ToString();
                        //            match.datetime = row["date_time"] is DBNull ? "no date" : ((DateTime)row["date_time"]).ToString("dd-MM-yyyy HH:mm");
                        //            match.stadium = row["stadium"] is DBNull ? "no stadium" : row["stadium"].ToString();
                        //            match.category_name = row["category_name"] is DBNull ? "no category" : row["category_name"].ToString();
                        //            match.localteam_name = row["localteam_name"] is DBNull ? "no team" : row["localteam_name"].ToString();
                        //            match.localteam_goals = row["localteam_goals"] is DBNull ? -1 : (int)row["localteam_goals"];
                        //            match.visitorteam_name = row["visitorteam_name"] is DBNull ? "no team" : row["visitorteam_name"].ToString();
                        //            match.visitorteam_goals = row["visitorteam_goals"] is DBNull ? -1 : (int)row["visitorteam_goals"];

                        //            if (row["event_id"] is DBNull)
                        //            {

                        //            }
                        //            else
                        //            {
                        //                Events _event = new Events();
                        //                _event.event_id = (long)row["event_id"];
                        //                _event.type = row["type"] is DBNull ? "" : row["type"].ToString();
                        //                _event.minute = row["minute"] is DBNull ? "" : row["minute"].ToString();
                        //                _event.team = row["team"] is DBNull ? "" : row["team"].ToString();
                        //                _event.player_name = row["player_name"] is DBNull ? "" : row["player_name"].ToString();
                        //                _event.assist_name = row["assist_name"] is DBNull ? "" : row["assist_name"].ToString();
                        //                _event.result = row["result"] is DBNull ? "" : row["result"].ToString();
                        //                match.events.Add(_event);
                        //            }
                        //            last_match_id = match.match_id;
                        //        }                                
                        //    }                            
                        //}

                        //if (i == ds.Tables[0].Rows.Count - 1) // si es el ultimo registro de guarda el ultimo match
                        //{
                        //    matches.Add(match);
                        //}
                        #endregion
                    }
                }
                return Json(new { data = matches, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public ActionResult getEvents(long match_id)
        {
            List<Events> events = new List<Events>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();

                string queryEvents = @"select " +
                                        "e.event_id, " +
                                        "e.type, " +
                                        "e.minute, " +
                                        "e.team, " +
                                        "e.result, " +
                                        "p1.name as 'player_name', " +
                                        "p2.name as 'assist_name' " +
                                        "from events as e " +
                                        "left join players as p1 on e.player_id = p1.player_id " +
                                        "left join players as p2 on e.assist_id = p1.player_id " +
                                        "where e.match_id = @match_id " +
                                        "order by convert(int,e.minute) asc";

                SqlCommand cmdEvents = new SqlCommand(queryEvents, conn);
                cmdEvents.Parameters.AddWithValue("@match_id", match_id);
                SqlDataAdapter daEvents = new SqlDataAdapter(cmdEvents);
                DataSet dsEvents = new DataSet();
                daEvents.Fill(dsEvents);

                if (dsEvents.Tables[0].Rows.Count > 0)
                {
                    for (int e = 0; e < dsEvents.Tables[0].Rows.Count; e++)
                    {
                        DataRow eRow = dsEvents.Tables[0].Rows[e];

                        Events _event = new Events();
                        _event.event_id = (long)eRow["event_id"];
                        _event.type = eRow["type"] is DBNull ? "" : eRow["type"].ToString();
                        _event.minute = eRow["minute"] is DBNull ? "" : eRow["minute"].ToString();
                        _event.team = eRow["team"] is DBNull ? "" : eRow["team"].ToString();
                        _event.player_name = eRow["player_name"] is DBNull ? "" : eRow["player_name"].ToString();
                        _event.assist_name = eRow["assist_name"] is DBNull ? "" : eRow["assist_name"].ToString();
                        _event.result = eRow["result"] is DBNull ? "" : eRow["result"].ToString();
                        events.Add(_event);
                    }
                } 
            }

            return Json(new { data = events, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult getOdds(long match_id, int odd_type_id)
        {
            List<Bookmaker> books = new List<Bookmaker>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();

                string queryOdds = @"select " +
                    "o.*, " +
                    "b.name as bookmaker_name " +
                    "from odds as o " +
                    "left join bookmakers as b on o.bookmaker_id = b.bookmaker_id " +
                    "where match_id = @match_id and odd_type_id = @odd_type_id " +
                    "order by o.bookmaker_id asc, o.handicap asc, o.total asc, o.odd_id asc";

                SqlCommand cmdOdds = new SqlCommand(queryOdds, conn);
                cmdOdds.Parameters.AddWithValue("@match_id", match_id);
                cmdOdds.Parameters.AddWithValue("@odd_type_id", odd_type_id);
                SqlDataAdapter daOdds = new SqlDataAdapter(cmdOdds);
                DataSet dsOdds = new DataSet();
                daOdds.Fill(dsOdds);

                if (dsOdds.Tables[0].Rows.Count > 0)
                {
                    Bookmaker book = new Bookmaker();
                    Handicap han = new Handicap();
                    Total tot = new Total();
                    Odd od = new Odd();

                    long last_bookmaker_id = 0;
                    double last_handicap = -1000;
                    double last_total = -1000;

                    for (int o = 0; o < dsOdds.Tables[0].Rows.Count; o++)
                    {
                        DataRow row = dsOdds.Tables[0].Rows[o];
                        long odd_id = (long)row["odd_id"];
                        string odd_name = row["name"].ToString();
                        double odd_value = (double)row["value"];
                        long bookmaker_id = (long)row["bookmaker_id"];
                        double handicap = (double)row["handicap"];
                        double total = (double)row["total"];
                        string bookmaker_name = row["bookmaker_name"].ToString();
                        
                        long incoming_bookmaker_id = bookmaker_id;

                        // si es el primer registro
                        if (o == 0)
                        {
                            /*
                             * se obtiene el nombre del bookmaker en la propiedad name de la clase
                             * bookmaker
                             */

                            book.name = bookmaker_name;
                            
                            /*
                             * se valida si el handicap es diferente del valor
                             * considerado como nulo '-1000'
                             */

                            if (handicap != -1000)
                            {
                                book.is_handicap = 1;

                                han.odds.Add(new Odd
                                {
                                    handicap_name = handicap,                                    
                                    odd_id = odd_id,
                                    name = odd_name,
                                    value = odd_value,
                                    handicap = handicap
                                });
                                
                                last_handicap = handicap;
                                last_bookmaker_id = incoming_bookmaker_id;

                                if (o == dsOdds.Tables[0].Rows.Count - 1)
                                {
                                    books.Add(book);
                                }
                                continue;
                            }

                            if (total != -1000)
                            {
                                book.is_total = 1;

                                tot.odds.Add(new Odd
                                {                                   
                                    total_name = total,
                                    odd_id = odd_id,
                                    name = odd_name,
                                    value = odd_value
                                });

                                last_total = total;
                                last_bookmaker_id = incoming_bookmaker_id;

                                if (o == dsOdds.Tables[0].Rows.Count - 1)
                                {
                                    books.Add(book);
                                }
                                continue;               
                            }

                            book.odds.Add(new Odd
                            {                                
                                odd_id = odd_id,
                                name = odd_name,
                                value = odd_value
                            });

                            last_bookmaker_id = incoming_bookmaker_id;

                            if (o == dsOdds.Tables[0].Rows.Count - 1)
                            {
                                books.Add(book);
                            }
                        }

                        /*
                         * Desde el segundo registro en adelante...
                         */

                        else
                        {
                            /*
                             * Se valida se todavia pertenece al bookmaker anterior
                             * y se utiliza el mismo contenedor de odds que en el
                             * anterior ciclo
                             */

                            if (last_bookmaker_id == incoming_bookmaker_id)
                            {
                                if (last_handicap != -1000) // segundo odd del handicap
                                {                                    
                                    han.odds.Add(new Odd
                                    {
                                        handicap_name = handicap,                                        
                                        odd_id = odd_id,
                                        name = odd_name,
                                        value = odd_value,
                                        handicap = handicap * -1
                                    });

                                    book.handicaps.Add(han);
                                    last_handicap = -1000;
                                    last_bookmaker_id = incoming_bookmaker_id;

                                    if (o == dsOdds.Tables[0].Rows.Count - 1)
                                    {
                                        books.Add(book);
                                    }
                                    continue;

                                }else if (book.is_handicap == 1)
                                {
                                    han = new Handicap();

                                    han.odds.Add(new Odd
                                    {
                                        handicap_name = handicap,                                        
                                        odd_id = odd_id,
                                        name = odd_name,
                                        value = odd_value,
                                        handicap = handicap
                                    });
                                    
                                    last_handicap = handicap;
                                    last_bookmaker_id = incoming_bookmaker_id;

                                    if (o == dsOdds.Tables[0].Rows.Count - 1)
                                    {
                                        books.Add(book);
                                    }
                                    continue;
                                }

                                if ((double)row["total"] != -1000)
                                {
                                    tot.odds.Add(new Odd
                                    {
                                        handicap_name = (double)row["handicap"],
                                        total_name = (double)row["total"],
                                        odd_id = (long)row["odd_id"],
                                        name = row["name"].ToString(),
                                        value = (double)row["value"],
                                        handicap = ((double)row["handicap"]) * -1
                                    });

                                    book.totals.Add(tot);
                                    last_total = -1000;
                                    last_bookmaker_id = incoming_bookmaker_id;

                                    if (o == dsOdds.Tables[0].Rows.Count - 1)
                                    {
                                        books.Add(book);
                                    }
                                    continue;
                                }else if (book.is_total == 1)
                                {
                                    tot = new Total();

                                    tot.odds.Add(new Odd
                                    {
                                        handicap_name = (double)row["handicap"],
                                        total_name = (double)row["total"],
                                        odd_id = (long)row["odd_id"],
                                        name = row["name"].ToString(),
                                        value = (double)row["value"],
                                        handicap = (double)row["handicap"]
                                    });

                                    last_total = (double)row["total"];
                                    last_bookmaker_id = incoming_bookmaker_id;

                                    if (o == dsOdds.Tables[0].Rows.Count - 1)
                                    {
                                        books.Add(book);
                                    }
                                    continue;
                                }

                                book.odds.Add(new Odd
                                {
                                    handicap_name = (double)row["handicap"],
                                    total_name = (double)row["total"],
                                    odd_id = (long)row["odd_id"],
                                    name = row["name"].ToString(),
                                    value = (double)row["value"],
                                    handicap = (double)row["handicap"]
                                });

                                last_bookmaker_id = incoming_bookmaker_id;

                                if (o == dsOdds.Tables[0].Rows.Count - 1)
                                {
                                    books.Add(book);
                                }
                            }

                            /*
                             * si es otro bookmaker..
                             */

                            else
                            {
                                books.Add(book);

                                book = new Bookmaker();                                
                                book.name = bookmaker_name;
                                

                                if (handicap != -1000)
                                {
                                    han = new Handicap();
                                    book.is_handicap = 1;

                                    han.odds.Add(new Odd
                                    {
                                        handicap_name = handicap,                                        
                                        odd_id = odd_id,
                                        name = odd_name,
                                        value = odd_value,
                                        handicap = handicap
                                    });

                                    
                                    last_handicap = handicap;
                                    last_bookmaker_id = incoming_bookmaker_id;

                                    if (o == dsOdds.Tables[0].Rows.Count - 1)
                                    {
                                        books.Add(book);
                                    }
                                    continue;
                                }
                                else if (book.is_handicap == 1)
                                {
                                    han = new Handicap();

                                    han.odds.Add(new Odd
                                    {
                                        handicap_name = handicap,                                        
                                        odd_id = odd_id,
                                        name = odd_name,
                                        value = odd_value,
                                        handicap = handicap * -1
                                    });

                                    last_handicap = handicap;
                                    last_bookmaker_id = incoming_bookmaker_id;

                                    if (o == dsOdds.Tables[0].Rows.Count - 1)
                                    {
                                        books.Add(book);
                                    }
                                    continue;
                                }

                                if ((double)row["total"] != -1000)
                                {
                                    tot.odds.Add(new Odd
                                    {
                                        handicap_name = (double)row["handicap"],
                                        total_name = (double)row["total"],
                                        odd_id = (long)row["odd_id"],
                                        name = row["name"].ToString(),
                                        value = (double)row["value"],
                                        handicap = (double)row["handicap"]
                                    });

                                    book.totals.Add(tot);
                                    last_total = -1000;
                                    last_bookmaker_id = incoming_bookmaker_id;

                                    if (o == dsOdds.Tables[0].Rows.Count - 1)
                                    {
                                        books.Add(book);
                                    }
                                    continue;
                                }
                                else if (book.is_total == 1)
                                {
                                    tot = new Total();

                                    tot.odds.Add(new Odd
                                    {
                                        handicap_name = (double)row["handicap"],
                                        total_name = (double)row["total"],
                                        odd_id = (long)row["odd_id"],
                                        name = row["name"].ToString(),
                                        value = (double)row["value"],
                                        handicap = (double)row["handicap"]
                                    });

                                    last_total = (double)row["total"];
                                    last_bookmaker_id = incoming_bookmaker_id;

                                    if (o == dsOdds.Tables[0].Rows.Count - 1)
                                    {
                                        books.Add(book);
                                    }
                                    continue;
                                }

                                book.odds.Add(new Odd
                                {
                                    handicap_name = (double)row["handicap"],
                                    total_name = (double)row["total"],
                                    odd_id = (long)row["odd_id"],
                                    name = row["name"].ToString(),
                                    value = (double)row["value"],
                                    handicap = (double)row["handicap"]
                                });

                                last_bookmaker_id = incoming_bookmaker_id;

                                if (o == dsOdds.Tables[0].Rows.Count - 1)
                                {
                                    books.Add(book);
                                }
                            }
                        }
                    }
                }
            }

            return Json(new { data = books, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult getOddTypes(long match_id)
        {
            List<OddTypes> oddTypes = new List<OddTypes>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();

                string queryOddTypes = @"select " +
                                        "ot.* " +
                                        "from oddtypes as ot " +
                                        "left join odds as o on ot.odd_type_id = o.odd_type_id " +
                                        "where o.match_id = @match_id " +
                                        "group by ot.odd_type_id, ot.name " +
                                        "order by convert(int,ot.odd_type_id) asc";

                SqlCommand cmdOddTypes = new SqlCommand(queryOddTypes, conn);
                cmdOddTypes.Parameters.AddWithValue("@match_id", match_id);
                SqlDataAdapter daOddTypes = new SqlDataAdapter(cmdOddTypes);
                DataSet dsOddTypes = new DataSet();
                daOddTypes.Fill(dsOddTypes);

                if (dsOddTypes.Tables[0].Rows.Count > 0)
                {
                    for (int ot = 0; ot < dsOddTypes.Tables[0].Rows.Count; ot++)
                    {
                        DataRow otRow = dsOddTypes.Tables[0].Rows[ot];

                        OddTypes oddType = new OddTypes();
                        oddType.odd_type_id = (int)otRow["odd_type_id"];
                        oddType.name = otRow["name"] is DBNull ? "" : otRow["name"].ToString();
                        oddTypes.Add(oddType);
                    }
                }
            }

            return Json(new { data = oddTypes, JsonRequestBehavior.AllowGet });
        }
    }
}