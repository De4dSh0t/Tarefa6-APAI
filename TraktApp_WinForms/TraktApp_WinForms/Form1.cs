using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TraktApp;

namespace TraktApp_WinForms
{
    public partial class Form1 : Form
    {
        private List<TimeSpan> timeList = new List<TimeSpan>(); //Lista que guarda os tempos necessários para listar na combobox (dropdown)
        private DateTime currentDate; //Guarda a data atual (selecionada)
        
        public Form1()
        {
            InitializeComponent();
            BoldedDates();
        }

        /// <summary>
        /// Coloca em negrito os dias em que existem dados
        /// </summary>
        private void BoldedDates()
        {
            foreach (var dateTime in Database.GetAllDates())
            {
                monthCalendar1.AddBoldedDate(dateTime);
            }
        }
        
        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            comboBox1.Items.Clear(); //Limpa a comboBox de forma a não haver dados duplicados
            timeList.Clear(); //Limpa a lista de tempos de forma a não haver dados duplicados
            
            foreach (var date in Database.GetAllDates())
            {
                if(date == monthCalendar1.SelectionStart.Date) //Verifica se a data selecionada contém alguma informação
                {
                    foreach (var time in Database.GetHoursByDate(date)) //Percorre todas as horas (em que tem informação) desse dia
                    {
                        comboBox1.Items.Add(time);
                        timeList.Add(time);
                    }

                    currentDate = date;
                    break;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = "";
                
            foreach (var info in Database.GetMoviesByDateTime(currentDate, timeList[comboBox1.SelectedIndex])) //Mostra todas as informações dos filmes obtidos num determinado dia e hora (selecionados)
            {
                text += $"{info} " +
                        $"\r\n ------------------------------------------------------------------------------------------------------------------ \r\n " +
                        $"\r\n";
            }

            textBox1.Text = text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Trending> trending = JsonParser.TrendindParser(Downloader.TrendingDownload()); //Lista de objetos "Trending" onde armazena todos os dados relativos ao json previamente transferido
            List<MovieInfo> moviesInfo = new List<MovieInfo>(); //Lista de informação acerca dos filmes obtidos na lista de objetos "Trending" (através do id)
            List<int> ids = new List<int>(); //Guarda todos os ids de cada filme

            foreach (var trend in trending)
            {
                moviesInfo.Add(JsonParser.MovieInfoParser(Downloader.MovieInfoDownload(trend.Movie.Ids.Trakt.ToString()))); //Adiciona à lista de objetos "MovieInfo" todas as informações relativas ao filme em questão
                ids.Add(trend.Movie.Ids.Trakt); //Adiciona o id (trakt) do filme em questão
            }

            int i = 0;
            
            foreach (var info in moviesInfo)
            {
                Database.InsertMoviesData(ids[i], info.Title, info.Year, info.Overview, info.Rating, info.Runtime, info.Genres);
                Database.InsertTrendingData(ids[i], i, trending[i].Watchers);
                i++;
            }

            //Desativa o botão após o 1º clique, de forma a evitar spam
            button1.Enabled = false;
        }
    }
}