using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;
using Newtonsoft.Json;

namespace NodeNetwork
{
    public class Server
    {
        public class Movie
        {
            [JsonProperty(PropertyName = "title")]
            public string Title { get; set; }

            [JsonProperty(PropertyName = "released")]
            public int Released { get; set; }

            [JsonProperty(PropertyName = "tagline")]
            public string TagLine { get; set; }
        }

        private GraphClient client;

        public void Connect()
        {
            client = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "asdf");
            client.Connect();

            var movies = client.Cypher
                  .Match("(m:Movie)")
                  .Return(m => m.As<Movie>())
                  .Limit(10)
                  .Results;

            foreach (var movie in movies)
                Console.WriteLine("{0} ({1}) - {2}", movie.Title, movie.Released, movie.TagLine);
        }

        public void Disconnect()
        {
            client.Dispose();
        }
    }
}
