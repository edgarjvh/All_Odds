using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using System.Threading;
using System.Windows.Forms;

namespace AllOddsSqlListener
{
    class Program
    {          
        static void Main(string[] args)
        {
            Console.Title = "All Odds Listener";
            Console.WriteLine("Starting SQL dependency");
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            SqlDependency.Stop(connectionString);
            SqlDependency.Start(connectionString);

            StartListeningBookmakers();
            StartListeningCategories();
            StartListeningEvents();
            StartListeningMatches();
            StartListeningOdds();
            StartListeningOddTypes();
            StartListeningPlayers();
            StartListeningTeams();

            Console.ReadLine();
        }

        private static void StartListeningBookmakers()
        {
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText =
                                    @"select " +
                                        "[bookmaker_id]," +
                                        "[name]," +
                                        "[extra] " +
                                    "from [dbo].[bookmakers]";

                    cmd.Notification = null;
                    SqlDependency depBookmakers = new SqlDependency(cmd);
                    depBookmakers.OnChange += depBookmakers_OnChange;

                    cn.Open();
                    cmd.ExecuteReader();
                }
            }
            Console.WriteLine("Listening Bookmakers...");
        }

        private static void depBookmakers_OnChange(object sender, SqlNotificationEventArgs e)
        {
            try
            {
                Console.WriteLine("Bookmakers Change caught!");
                var connection = new HubConnection(Properties.Settings.Default.serverAddress);
                var hub = connection.CreateHubProxy("OddsHub");
                connection.Start().Wait();
                hub.Invoke("notifyUpdate");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bookmakers Error: " + ex.Message);
            }
            finally
            {
                StartListeningBookmakers();
            }
        }

        private static void StartListeningCategories()
        {
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText =
                                    @"select " +
                                        "[category_id]," +
                                        "[gid]," +
                                        "[name], " +
                                        "[file_group], " +
                                        "[is_cup] " +
                                    "from [dbo].[categories]";

                    cmd.Notification = null;
                    SqlDependency depCategories = new SqlDependency(cmd);
                    depCategories.OnChange += depCategories_OnChange;

                    cn.Open();
                    cmd.ExecuteReader();
                }
            }
            Console.WriteLine("Listening Categories...");
        }

        private static void depCategories_OnChange(object sender, SqlNotificationEventArgs e)
        {
            try
            {
                Console.WriteLine("Categories Change caught!");
                var connection = new HubConnection(Properties.Settings.Default.serverAddress);
                var hub = connection.CreateHubProxy("OddsHub");
                connection.Start().Wait();
                hub.Invoke("notifyUpdate");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Categories Error: " + ex.Message);
            }
            finally
            {
                StartListeningCategories();
            }
        }               

        private static void StartListeningEvents()
        {
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText =
                                    @"select " +
                                        "[event_id]," +
                                        "[type]," +
                                        "[minute], " +
                                        "[team], " +
                                        "[player_id], " +
                                        "[assist_id], " +
                                        "[result], " +
                                        "[match_id] " +
                                    "from [dbo].[events]";

                    cmd.Notification = null;
                    SqlDependency depEvents = new SqlDependency(cmd);
                    depEvents.OnChange += depEvents_OnChange;

                    cn.Open();
                    cmd.ExecuteReader();
                }
            }
            Console.WriteLine("Listening Events...");
        }

        private static void depEvents_OnChange(object sender, SqlNotificationEventArgs e)
        {
            try
            {
                Console.WriteLine("Events Change caught!");
                var connection = new HubConnection(Properties.Settings.Default.serverAddress);
                var hub = connection.CreateHubProxy("OddsHub");
                connection.Start().Wait();
                hub.Invoke("notifyUpdate");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Events Error: " + ex.Message);
            }
            finally
            {
                StartListeningEvents();
            }
        }

        private static void StartListeningMatches()
        {
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText =
                                    @"select " +
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

                    cmd.Notification = null;                   
                    SqlDependency depMatches = new SqlDependency(cmd);                    
                    depMatches.OnChange += depMatches_OnChange;

                    cn.Open();
                    cmd.ExecuteReader();
                }
            }
            Console.WriteLine("Listening Matches...");
        }

        private static void depMatches_OnChange(object sender, SqlNotificationEventArgs e)
        {
            try
            {
                Console.WriteLine("Matches Change caught!");
                var connection = new HubConnection(Properties.Settings.Default.serverAddress);
                var hub = connection.CreateHubProxy("OddsHub");
                connection.Start().Wait();
                hub.Invoke("notifyUpdate");                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Matches Error: " + ex.Message);
            }
            finally
            {
                StartListeningMatches();
            }
        }

        private static void StartListeningOdds()
        {
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText =
                                    @"select " +
                                        "[odd_id]," +
                                        "[name]," +
                                        "[value]," +
                                        "[bookmaker_id]," +
                                        "[odd_type_id]," +
                                        "[handicap]," +
                                        "[total]," +
                                        "[main]," +
                                        "[match_id] " +                                        
                                    "from [dbo].[odds]";

                    cmd.Notification = null;
                    SqlDependency depOdds = new SqlDependency(cmd);
                    depOdds.OnChange += depOdds_OnChange;

                    cn.Open();
                    cmd.ExecuteReader();
                }
            }
            Console.WriteLine("Listening Odds...");
        }

        private static void depOdds_OnChange(object sender, SqlNotificationEventArgs e)
        {
            try
            {
                Console.WriteLine("Odds Change caught!");
                var connection = new HubConnection(Properties.Settings.Default.serverAddress);
                var hub = connection.CreateHubProxy("OddsHub");
                connection.Start().Wait();
                hub.Invoke("notifyUpdate");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Odds Error: " + ex.Message);
            }
            finally
            {
                StartListeningOdds();
            }
        }

        private static void StartListeningOddTypes()
        {
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText =
                                    @"select " +
                                        "[odd_type_id]," +
                                        "[name] " +                                        
                                    "from [dbo].[oddtypes]";

                    cmd.Notification = null;
                    SqlDependency depOddTypes = new SqlDependency(cmd);
                    depOddTypes.OnChange += depOddTypes_OnChange;

                    cn.Open();
                    cmd.ExecuteReader();
                }
            }
            Console.WriteLine("Listening OddTypes...");
        }

        private static void depOddTypes_OnChange(object sender, SqlNotificationEventArgs e)
        {
            try
            {
                Console.WriteLine("OddTypes Change caught!");
                var connection = new HubConnection(Properties.Settings.Default.serverAddress);
                var hub = connection.CreateHubProxy("OddsHub");
                connection.Start().Wait();
                hub.Invoke("notifyUpdate");
            }
            catch (Exception ex)
            {
                Console.WriteLine("OddTypes Error: " + ex.Message);
            }
            finally
            {
                StartListeningOddTypes();
            }
        }

        private static void StartListeningPlayers()
        {
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText =
                                    @"select " +
                                        "[player_id]," +
                                        "[name] " +
                                    "from [dbo].[players]";

                    cmd.Notification = null;
                    SqlDependency depPlayers = new SqlDependency(cmd);
                    depPlayers.OnChange += depPlayers_OnChange;

                    cn.Open();
                    cmd.ExecuteReader();
                }
            }
            Console.WriteLine("Listening Players...");
        }

        private static void depPlayers_OnChange(object sender, SqlNotificationEventArgs e)
        {
            try
            {
                Console.WriteLine("Players Change caught!");
                var connection = new HubConnection(Properties.Settings.Default.serverAddress);
                var hub = connection.CreateHubProxy("OddsHub");
                connection.Start().Wait();
                hub.Invoke("notifyUpdate");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Players Error: " + ex.Message);
            }
            finally
            {
                StartListeningPlayers();
            }
        }

        private static void StartListeningTeams()
        {
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText =
                                    @"select " +
                                        "[team_id]," +
                                        "[name] " +
                                    "from [dbo].[teams]";

                    cmd.Notification = null;
                    SqlDependency depTeams = new SqlDependency(cmd);
                    depTeams.OnChange += depTeams_OnChange;

                    cn.Open();
                    cmd.ExecuteReader();
                }
            }
            Console.WriteLine("Listening Teams...");
        }

        private static void depTeams_OnChange(object sender, SqlNotificationEventArgs e)
        {
            try
            {
                Console.WriteLine("Teams Change caught!");
                var connection = new HubConnection(Properties.Settings.Default.serverAddress);
                var hub = connection.CreateHubProxy("OddsHub");
                connection.Start().Wait();
                hub.Invoke("notifyUpdate");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Teams Error: " + ex.Message);
            }
            finally
            {
                StartListeningTeams();
            }
        }
    }
}
