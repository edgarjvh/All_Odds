using AllOdds.Controllers;
using AllOdds.hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AllOdds.SqlDependencies
{    
    public class UpdateComponent
    {
        private static string strConn = ConfigurationManager.ConnectionStrings["defaultConn"].ConnectionString;

        #region Models_Updated
        public void MatchesUpdated()
        {
            string query = @"select " +
                "[match_id]," +
                "[status]," +
                "[date_time]," +
                "[stadium]," +
                "[static_id]," +
                "[fix_id]," +
                "[category_id]," +
                "[localteam_id]," +
                "[visitorteam_id]," +
                "[ht_score]," +
                "[ft_score]," +
                "[localteam_goals]," +
                "[visitorteam_goals] " +
                "from [dbo].[matches]";

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Notification = null;

                SqlDependency dep_Matches = new SqlDependency(cmd);
                dep_Matches.OnChange += Dep_Matches_OnChange;

                using (SqlDataReader reader = cmd.ExecuteReader()) {

                }
            }
        }

        public void BookmakersUpdated()
        {
            string query = @"select " +
                "[bookmaker_id]," +
                "[name]," +
                "[extra] " +
                "from [dbo].[bookmakers]";

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Notification = null;

                SqlDependency dep_Bookmakers = new SqlDependency(cmd);
                dep_Bookmakers.OnChange += Dep_Bookmakers_OnChange;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                }
            }
        }

        public void CategoriesUpdated()
        {
            string query = @"select " +
                "[category_id]," +
                "[gid]," +
                "[name], " +
                "[file_group], " +
                "[is_cup] " +
                "from [dbo].[categories]";

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Notification = null;

                SqlDependency dep_Categories = new SqlDependency(cmd);
                dep_Categories.OnChange += Dep_Categories_OnChange;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                }
            }
        }

        public void EventsUpdated()
        {
            string query = @"select " +
                "[event_id]," +
                "[type]," +
                "[minute], " +
                "[team], " +
                "[player_id], " +
                "[assist_id], " +
                "[result], " +                
                "[match_id] " +
                "from [dbo].[events]";

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Notification = null;

                SqlDependency dep_Events = new SqlDependency(cmd);
                dep_Events.OnChange += Dep_Events_OnChange;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                }
            }
        }

        public void PlayersUpdated()
        {
            string query = @"select " +
                "[player_id]," +
                "[name] " +                
                "from [dbo].[players]";

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Notification = null;

                SqlDependency dep_Players = new SqlDependency(cmd);
                dep_Players.OnChange += Dep_Players_OnChange;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                }
            }
        }

        public void TeamsUpdated()
        {
            string query = @"select " +
                "[team_id]," +
                "[name] " +
                "from [dbo].[teams]";

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Notification = null;

                SqlDependency dep_Teams = new SqlDependency(cmd);
                dep_Teams.OnChange += Dep_Teams_OnChange;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                }
            }
        }
        
        #endregion

        #region OnChange_Events
        private void Dep_Matches_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency dep_Matches = sender as SqlDependency;
                dep_Matches.OnChange -= Dep_Matches_OnChange;                         
                notify();
                MatchesUpdated();
            }
        }

        private void Dep_Bookmakers_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency dep_Bookmakers = sender as SqlDependency;
                dep_Bookmakers.OnChange -= Dep_Bookmakers_OnChange;
                notify();
                BookmakersUpdated();
            }
        }

        private void Dep_Categories_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency dep_Categories = sender as SqlDependency;
                dep_Categories.OnChange -= Dep_Categories_OnChange;
                notify();
                CategoriesUpdated();
            }
        }

        private void Dep_Events_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency dep_Events = sender as SqlDependency;
                dep_Events.OnChange -= Dep_Events_OnChange;
                notify();
                EventsUpdated();
            }            
        }

        private void Dep_Players_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency dep_Players = sender as SqlDependency;
                dep_Players.OnChange -= Dep_Players_OnChange;
                notify();
                PlayersUpdated();
            }
        }

        private void Dep_Teams_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency dep_Teams = sender as SqlDependency;
                dep_Teams.OnChange -= Dep_Teams_OnChange;
                notify();
                TeamsUpdated();
            }
        }
        #endregion

        private void notify()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<OddsHub>();
            context.Clients.All.notifyUpdate();
        }
    }
}