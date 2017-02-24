using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace AllOdds.Controllers
{
    public class HomeController : Controller
    {
        private static string strConn = ConfigurationManager.ConnectionStrings["defaultConn"].ConnectionString;

        public ActionResult Index()
        {
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

                string result = Newtonsoft.Json.JsonConvert.SerializeObject(c);
                return result;
            }
        }
        
        [HttpPost]
        public ActionResult getMatches(int category_id, string dateFrom, string dateTo)
        {
            string query = @"select " +                
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
                "m.visitorteam_goals," +
                "e.*," +
                "p1.name as 'player_name'," +
                "p2.name as 'assist_name' " +
                "from dbo.matches as m left join dbo.categories as c on m.category_id = c.category_id " +
                "left join teams as lt on m.localteam_id = lt.team_id " +
                "left join teams as vt on m.visitorteam_id = vt.team_id " +
                "left join Events as e on m.match_id = e.match_id " +
                "left join Players as p1 on e.player_id = p1.player_id " +
                "left join Players as p2 on e.assist_id = p2.player_id " +
                "where m.date_time >= @dateFrom and m.date_time <= @dateTo";

            if (category_id > 0)
            {
                query += " and m.[category_id] = @category_id";
            }

            query += " order by m.[date_time] ASC";

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@dateFrom", DateTime.Parse(dateFrom));
                cmd.Parameters.AddWithValue("@dateTo", DateTime.Parse(dateTo));

                if (query.Contains("@category_id"))
                {
                    cmd.Parameters.AddWithValue("@category_id", category_id);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                List<Matches> matches = new List<Matches>();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    long last_match_id = 0;
                    Matches match = new Matches();

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = ds.Tables[0].Rows[i];

                        if (row["match_id"] is DBNull)
                        {
                            
                        }else
                        {
                            long incoming_match_id = (long)row["match_id"];

                            if (last_match_id == 0) // si es el primer match
                            {                                
                                match.match_id = incoming_match_id;
                                match.status = row["status"] is DBNull ? "no status" : row["status"].ToString();
                                match.datetime = row["date_time"] is DBNull ? "no date" : ((DateTime)row["date_time"]).ToString("dd-MM-yyyy HH:mm");
                                match.stadium = row["stadium"] is DBNull ? "no stadium" : row["stadium"].ToString();
                                match.category_name = row["category_name"] is DBNull ? "no category" : row["category_name"].ToString();
                                match.localteam_name = row["localteam_name"] is DBNull ? "no team" : row["localteam_name"].ToString();
                                match.localteam_goals = row["localteam_goals"] is DBNull ? -1 : (int)row["localteam_goals"];
                                match.visitorteam_name = row["visitorteam_name"] is DBNull ? "no team" : row["visitorteam_name"].ToString();
                                match.visitorteam_goals = row["visitorteam_goals"] is DBNull ? -1 : (int)row["visitorteam_goals"];

                                if (row["event_id"] is DBNull)
                                {
                                    
                                }
                                else
                                {
                                    Events _event = new Events();
                                    _event.event_id = (long)row["event_id"];
                                    _event.type = row["type"] is DBNull ? "" : row["type"].ToString();
                                    _event.minute = row["minute"] is DBNull ? "" : row["minute"].ToString();
                                    _event.team = row["team"] is DBNull ? "" : row["team"].ToString();
                                    _event.player_name = row["player_name"] is DBNull ? "" : row["player_name"].ToString();
                                    _event.assist_name = row["assist_name"] is DBNull ? "" : row["assist_name"].ToString();
                                    _event.result = row["result"] is DBNull ? "" : row["result"].ToString();
                                    match.events.Add(_event);
                                }

                                last_match_id = match.match_id;
                            }
                            else
                            {
                                if (last_match_id == incoming_match_id)
                                {
                                    Events _event = new Events();
                                    _event.event_id = (long)row["event_id"];
                                    _event.type = row["type"] is DBNull ? "" : row["type"].ToString();
                                    _event.minute = row["minute"] is DBNull ? "" : row["minute"].ToString();
                                    _event.team = row["team"] is DBNull ? "" : row["team"].ToString();
                                    _event.player_name = row["player_name"] is DBNull ? "" : row["player_name"].ToString();
                                    _event.assist_name = row["assist_name"] is DBNull ? "" : row["assist_name"].ToString();
                                    _event.result = row["result"] is DBNull ? "" : row["result"].ToString();
                                    match.events.Add(_event);
                                }
                                else
                                {
                                    matches.Add(match);

                                    match = new Matches();
                                    match.match_id = incoming_match_id;
                                    match.status = row["status"] is DBNull ? "no status" : row["status"].ToString();
                                    match.datetime = row["date_time"] is DBNull ? "no date" : ((DateTime)row["date_time"]).ToString("dd-MM-yyyy HH:mm");
                                    match.stadium = row["stadium"] is DBNull ? "no stadium" : row["stadium"].ToString();
                                    match.category_name = row["category_name"] is DBNull ? "no category" : row["category_name"].ToString();
                                    match.localteam_name = row["localteam_name"] is DBNull ? "no team" : row["localteam_name"].ToString();
                                    match.localteam_goals = row["localteam_goals"] is DBNull ? -1 : (int)row["localteam_goals"];
                                    match.visitorteam_name = row["visitorteam_name"] is DBNull ? "no team" : row["visitorteam_name"].ToString();
                                    match.visitorteam_goals = row["visitorteam_goals"] is DBNull ? -1 : (int)row["visitorteam_goals"];

                                    if (row["event_id"] is DBNull)
                                    {
                                        
                                    }
                                    else
                                    {
                                        Events _event = new Events();
                                        _event.event_id = (long)row["event_id"];
                                        _event.type = row["type"] is DBNull ? "" : row["type"].ToString();
                                        _event.minute = row["minute"] is DBNull ? "" : row["minute"].ToString();
                                        _event.team = row["team"] is DBNull ? "" : row["team"].ToString();
                                        _event.player_name = row["player_name"] is DBNull ? "" : row["player_name"].ToString();
                                        _event.assist_name = row["assist_name"] is DBNull ? "" : row["assist_name"].ToString();
                                        _event.result = row["result"] is DBNull ? "" : row["result"].ToString();
                                        match.events.Add(_event);
                                    }
                                    last_match_id = match.match_id;
                                }                                
                            }                            
                        }

                        if (i == ds.Tables[0].Rows.Count - 1) // si es el ultimo registro de guarda el ultimo match
                        {
                            matches.Add(match);
                        }
                    }
                }
                return Json(new { data = matches, JsonRequestBehavior.AllowGet });
            }
        }
    }
}