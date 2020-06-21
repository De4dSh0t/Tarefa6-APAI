﻿using System;
using System.Net.Http;

namespace TraktApp
{
    public class Downloader
    {
        static readonly Uri baseAddress = new Uri("https://api.trakt.tv/");

        /// <summary>
        /// Transfere da API o Json acerca das "Trendings"
        /// </summary>
        /// <returns></returns>
        public static string TrendingDownload()
        {
            HttpClient client = new HttpClient {BaseAddress = baseAddress};

            client.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2"); //Versão da API
            client.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", 
                "c8850f6b0d76450b4890e41e30fc8bf95b031967a8c85c950bf0fb490c241862"); //Chave do client

            return client.GetStringAsync("movies/trending").GetAwaiter().GetResult();
        }

        /// <summary>
        /// Transfere da API o Json acerca das informações do filme (através do id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string MovieInfoDownload(string id)
        {
            HttpClient client = new HttpClient {BaseAddress = baseAddress};

            client.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2"); //Versão da API
            client.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", 
                "c8850f6b0d76450b4890e41e30fc8bf95b031967a8c85c950bf0fb490c241862"); //Chave do client

            return client.GetStringAsync($"movies/{id}?extended=full").GetAwaiter().GetResult();
        }
    }
}