﻿using System;
using System.Collections.Generic;
using Npgsql;
using NpgsqlTypes;

namespace TraktApp
{
    public class Database
    {
        //Informação sobre a ligação à base de dados
        static string conInfo = "Host=localhost;Username=tarefa6user;Password=tarefa6;Database=Tarefa6";
        
        /// <summary>
        /// Insere dados relativos à informação dos filmes (dependendo do id)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="year"></param>
        /// <param name="overview"></param>
        /// <param name="rating"></param>
        /// <param name="runtime"></param>
        /// <param name="genres"></param>
        public static void InsertMoviesData(int id, string title, int year, string overview, float rating, int runtime, string[] genres)
        {
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

        /// <summary>
        /// Insere os dados relativos às tendênicas atuais
        /// </summary>
        /// <param name="id_movie"></param>
        /// <param name="position"></param>
        /// <param name="watchers"></param>
        public static void InsertTrendingData(int id_movie, int position, int watchers)
        {
            using var con = new NpgsqlConnection(conInfo);
            con.Open();
            
            var sql = "INSERT INTO trending(id_movie, date, time, position, watchers) VALUES (@id_movie, @date, @time, @position, @watchers)";
            using var cmd = new NpgsqlCommand(sql, con);
            
            cmd.Parameters.AddWithValue("id_movie", id_movie);
            cmd.Parameters.AddWithValue("date", NpgsqlDate.Today);
            cmd.Parameters.AddWithValue("time", NpgsqlDateTime.Now);
            cmd.Parameters.AddWithValue("position", position);
            cmd.Parameters.AddWithValue("watchers", watchers);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
        
        /// <summary>
        /// Seleciona todas as datas presentes na tabela "trending"
        /// </summary>
        /// <returns></returns>
        public static List<DateTime> GetAllDates()
        { 
            using var con = new NpgsqlConnection(conInfo);
            con.Open();
            
            var sql = "SELECT date FROM trending";
            using var cmd = new NpgsqlCommand(sql, con);

            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            List<DateTime> data = new List<DateTime>();

            while (dataReader.Read())
            {
                data.Add(dataReader.GetDateTime(0));
            }

            return data;
        }

        /// <summary>
        /// Seleciona todas as informações sobre os filmes (usando o id), numa data específica
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static List<string> GetMoviesByDateTime(DateTime dateTime, TimeSpan time)
        {
            using var con = new NpgsqlConnection(conInfo);
            con.Open();
            
            var sql = $"SELECT m.title, m.year, m.overview, m.rating, m.runtime, watchers FROM trending JOIN movies m ON id_movie = m.id " +
                      $"WHERE date = '{dateTime.Date}' AND extract(hour from time) = '{time.Hours}' AND extract(minute from time) = '{time.Minutes}' " +
                      $"AND extract(second from time) >= {time.Seconds} AND extract(second from time) <= {time.Seconds+1};";
            using var cmd = new NpgsqlCommand(sql, con);

            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            List<string> data = new List<string>();

            while (dataReader.Read())
            {
                data.Add($" {dataReader["title"]} ({dataReader["year"]}) \r\n" +
                         $"\r\n Overview: {dataReader["overview"]} \r\n" +
                         $"\r\n Rating: {dataReader["rating"]} " +
                         $"\r\n Runtime: {dataReader["runtime"]} " +
                         $"\r\n Watchers: {dataReader["watchers"]} \r\n");
            }

            return data;
        }

        /// <summary>
        /// Seleciona todos as horas de uma data específica, em que a posição do filme é igual a 0 (refere-se à primeira posição do filme no top 10)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static List<TimeSpan> GetHoursByDate(DateTime dateTime)
        { 
            using var con = new NpgsqlConnection(conInfo);
            con.Open();
            
            var sql = $"SELECT time FROM trending WHERE date = '{dateTime.Date}' AND position = 0";
            using var cmd = new NpgsqlCommand(sql, con);

            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            List<TimeSpan> data = new List<TimeSpan>();

            while (dataReader.Read())
            {
                data.Add(dataReader.GetTimeSpan(0));
            }

            return data;
        }
    }
}