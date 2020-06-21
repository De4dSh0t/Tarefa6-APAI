using Npgsql;
using NpgsqlTypes;

namespace TraktApp
{
    public class Database
    {
        public static void InsertMoviesData(int id, string title, int year, string overview, float rating, int runtime, string[] genres)
        {
            string conInfo = "Host=localhost;Username=tarefa6user;Password=tarefa6;Database=Tarefa6";
            
            using var con = new NpgsqlConnection(conInfo);
            con.Open();
            
            var sql = "INSERT INTO movies(id, title, year, overview, rating, runtime, genres) VALUES (@id, @title, @year, @overview, @rating, @runtime, @genres) ON CONFLICT DO NOTHING";
            using var cmd = new NpgsqlCommand(sql, con);
            
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("title", title);
            cmd.Parameters.AddWithValue("year", year);
            cmd.Parameters.AddWithValue("overview", overview);
            cmd.Parameters.AddWithValue("rating", rating);
            cmd.Parameters.AddWithValue("runtime", runtime);
            cmd.Parameters.AddWithValue("genres", genres);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public static void InsertTrendingData(int id_movie, int position)
        {
            string conInfo = "Host=localhost;Username=tarefa6user;Password=tarefa6;Database=Tarefa6";
            
            using var con = new NpgsqlConnection(conInfo);
            con.Open();
            
            var sql = "INSERT INTO trending(id_movie, date, time, position) VALUES (@id_movie, @date, @time, @position)";
            using var cmd = new NpgsqlCommand(sql, con);
            
            cmd.Parameters.AddWithValue("id_movie", id_movie);
            cmd.Parameters.AddWithValue("date", NpgsqlDate.Today);
            cmd.Parameters.AddWithValue("time", NpgsqlDateTime.Now);
            cmd.Parameters.AddWithValue("position", position);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}