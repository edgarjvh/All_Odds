﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AllOdds
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SportFeedsDBEntities : DbContext
    {
        public SportFeedsDBEntities()
            : base("name=SportFeedsDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Bookmakers> Bookmakers { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<FeedUrls> FeedUrls { get; set; }
        public virtual DbSet<Matches> Matches { get; set; }
        public virtual DbSet<OddTypes> OddTypes { get; set; }
        public virtual DbSet<Players> Players { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }
        public virtual DbSet<TvStations> TvStations { get; set; }
        public virtual DbSet<Odds> Odds { get; set; }
    
        public virtual int Bookmakers_save(Nullable<long> bookmaker_id, string name, string extra)
        {
            var bookmaker_idParameter = bookmaker_id.HasValue ?
                new ObjectParameter("bookmaker_id", bookmaker_id) :
                new ObjectParameter("bookmaker_id", typeof(long));
    
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var extraParameter = extra != null ?
                new ObjectParameter("extra", extra) :
                new ObjectParameter("extra", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Bookmakers_save", bookmaker_idParameter, nameParameter, extraParameter);
        }
    
        public virtual int Categories_save(Nullable<long> category_id, Nullable<long> gid, string name, string file_group, Nullable<int> is_cup)
        {
            var category_idParameter = category_id.HasValue ?
                new ObjectParameter("category_id", category_id) :
                new ObjectParameter("category_id", typeof(long));
    
            var gidParameter = gid.HasValue ?
                new ObjectParameter("gid", gid) :
                new ObjectParameter("gid", typeof(long));
    
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var file_groupParameter = file_group != null ?
                new ObjectParameter("file_group", file_group) :
                new ObjectParameter("file_group", typeof(string));
    
            var is_cupParameter = is_cup.HasValue ?
                new ObjectParameter("is_cup", is_cup) :
                new ObjectParameter("is_cup", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Categories_save", category_idParameter, gidParameter, nameParameter, file_groupParameter, is_cupParameter);
        }
    
        public virtual int Events_save(Nullable<long> event_id, string type, Nullable<int> minute, string team, Nullable<long> player_id, Nullable<long> assist_id, string result, Nullable<long> match_id)
        {
            var event_idParameter = event_id.HasValue ?
                new ObjectParameter("event_id", event_id) :
                new ObjectParameter("event_id", typeof(long));
    
            var typeParameter = type != null ?
                new ObjectParameter("type", type) :
                new ObjectParameter("type", typeof(string));
    
            var minuteParameter = minute.HasValue ?
                new ObjectParameter("minute", minute) :
                new ObjectParameter("minute", typeof(int));
    
            var teamParameter = team != null ?
                new ObjectParameter("team", team) :
                new ObjectParameter("team", typeof(string));
    
            var player_idParameter = player_id.HasValue ?
                new ObjectParameter("player_id", player_id) :
                new ObjectParameter("player_id", typeof(long));
    
            var assist_idParameter = assist_id.HasValue ?
                new ObjectParameter("assist_id", assist_id) :
                new ObjectParameter("assist_id", typeof(long));
    
            var resultParameter = result != null ?
                new ObjectParameter("result", result) :
                new ObjectParameter("result", typeof(string));
    
            var match_idParameter = match_id.HasValue ?
                new ObjectParameter("match_id", match_id) :
                new ObjectParameter("match_id", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Events_save", event_idParameter, typeParameter, minuteParameter, teamParameter, player_idParameter, assist_idParameter, resultParameter, match_idParameter);
        }
    
        public virtual int Matches_save(Nullable<long> match_id, string status, Nullable<System.DateTime> date_time, string stadium, Nullable<long> static_id, Nullable<long> fix_id, Nullable<long> category_id, Nullable<long> localteam_id, Nullable<long> visitorteam_id, string ht_score, string ft_score, Nullable<int> localteam_goals, Nullable<int> visitorteam_goals)
        {
            var match_idParameter = match_id.HasValue ?
                new ObjectParameter("match_id", match_id) :
                new ObjectParameter("match_id", typeof(long));
    
            var statusParameter = status != null ?
                new ObjectParameter("status", status) :
                new ObjectParameter("status", typeof(string));
    
            var date_timeParameter = date_time.HasValue ?
                new ObjectParameter("date_time", date_time) :
                new ObjectParameter("date_time", typeof(System.DateTime));
    
            var stadiumParameter = stadium != null ?
                new ObjectParameter("stadium", stadium) :
                new ObjectParameter("stadium", typeof(string));
    
            var static_idParameter = static_id.HasValue ?
                new ObjectParameter("static_id", static_id) :
                new ObjectParameter("static_id", typeof(long));
    
            var fix_idParameter = fix_id.HasValue ?
                new ObjectParameter("fix_id", fix_id) :
                new ObjectParameter("fix_id", typeof(long));
    
            var category_idParameter = category_id.HasValue ?
                new ObjectParameter("category_id", category_id) :
                new ObjectParameter("category_id", typeof(long));
    
            var localteam_idParameter = localteam_id.HasValue ?
                new ObjectParameter("localteam_id", localteam_id) :
                new ObjectParameter("localteam_id", typeof(long));
    
            var visitorteam_idParameter = visitorteam_id.HasValue ?
                new ObjectParameter("visitorteam_id", visitorteam_id) :
                new ObjectParameter("visitorteam_id", typeof(long));
    
            var ht_scoreParameter = ht_score != null ?
                new ObjectParameter("ht_score", ht_score) :
                new ObjectParameter("ht_score", typeof(string));
    
            var ft_scoreParameter = ft_score != null ?
                new ObjectParameter("ft_score", ft_score) :
                new ObjectParameter("ft_score", typeof(string));
    
            var localteam_goalsParameter = localteam_goals.HasValue ?
                new ObjectParameter("localteam_goals", localteam_goals) :
                new ObjectParameter("localteam_goals", typeof(int));
    
            var visitorteam_goalsParameter = visitorteam_goals.HasValue ?
                new ObjectParameter("visitorteam_goals", visitorteam_goals) :
                new ObjectParameter("visitorteam_goals", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Matches_save", match_idParameter, statusParameter, date_timeParameter, stadiumParameter, static_idParameter, fix_idParameter, category_idParameter, localteam_idParameter, visitorteam_idParameter, ht_scoreParameter, ft_scoreParameter, localteam_goalsParameter, visitorteam_goalsParameter);
        }
    
        public virtual int Odds_save(Nullable<long> odd_id, string name, string value, Nullable<long> bookmaker_id, Nullable<int> odd_type_id, Nullable<double> handicap, Nullable<double> total, Nullable<double> main, Nullable<long> match_id)
        {
            var odd_idParameter = odd_id.HasValue ?
                new ObjectParameter("odd_id", odd_id) :
                new ObjectParameter("odd_id", typeof(long));
    
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var valueParameter = value != null ?
                new ObjectParameter("value", value) :
                new ObjectParameter("value", typeof(string));
    
            var bookmaker_idParameter = bookmaker_id.HasValue ?
                new ObjectParameter("bookmaker_id", bookmaker_id) :
                new ObjectParameter("bookmaker_id", typeof(long));
    
            var odd_type_idParameter = odd_type_id.HasValue ?
                new ObjectParameter("odd_type_id", odd_type_id) :
                new ObjectParameter("odd_type_id", typeof(int));
    
            var handicapParameter = handicap.HasValue ?
                new ObjectParameter("handicap", handicap) :
                new ObjectParameter("handicap", typeof(double));
    
            var totalParameter = total.HasValue ?
                new ObjectParameter("total", total) :
                new ObjectParameter("total", typeof(double));
    
            var mainParameter = main.HasValue ?
                new ObjectParameter("main", main) :
                new ObjectParameter("main", typeof(double));
    
            var match_idParameter = match_id.HasValue ?
                new ObjectParameter("match_id", match_id) :
                new ObjectParameter("match_id", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Odds_save", odd_idParameter, nameParameter, valueParameter, bookmaker_idParameter, odd_type_idParameter, handicapParameter, totalParameter, mainParameter, match_idParameter);
        }
    
        public virtual int OddTypes_save(Nullable<int> odd_type_id, string odd_type_name)
        {
            var odd_type_idParameter = odd_type_id.HasValue ?
                new ObjectParameter("odd_type_id", odd_type_id) :
                new ObjectParameter("odd_type_id", typeof(int));
    
            var odd_type_nameParameter = odd_type_name != null ?
                new ObjectParameter("odd_type_name", odd_type_name) :
                new ObjectParameter("odd_type_name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("OddTypes_save", odd_type_idParameter, odd_type_nameParameter);
        }
    
        public virtual int Players_save(Nullable<long> player_id, string name)
        {
            var player_idParameter = player_id.HasValue ?
                new ObjectParameter("player_id", player_id) :
                new ObjectParameter("player_id", typeof(long));
    
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Players_save", player_idParameter, nameParameter);
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        public virtual int Teams_save(Nullable<long> team_id, string name)
        {
            var team_idParameter = team_id.HasValue ?
                new ObjectParameter("team_id", team_id) :
                new ObjectParameter("team_id", typeof(long));
    
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Teams_save", team_idParameter, nameParameter);
        }
    
        public virtual int TvStations_save(string name, Nullable<int> match_id)
        {
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var match_idParameter = match_id.HasValue ?
                new ObjectParameter("match_id", match_id) :
                new ObjectParameter("match_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TvStations_save", nameParameter, match_idParameter);
        }
    }
}
