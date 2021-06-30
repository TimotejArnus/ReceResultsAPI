using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RestStoeitev.Model
{
    public class Race
    {
        public int id { get; set; }
        public string naziv { get; set; }
        public string country_name { get; set; }
        public int year { get; set; }
        public string vrsta { get; set; }
        public string swimDistance { get; set; }
        public string runDistance { get; set; }
        public string bikeDistance { get; set; }

        public Race ReadToRace(MySqlDataReader reader)
        {
            //var race = new Race();

            this.id = int.Parse(reader["id"].ToString());
            this.naziv = reader["naziv"].ToString();
            this.vrsta = reader["vrsta"].ToString();
            this.swimDistance = reader["swimDistance"].ToString();
            this.bikeDistance = reader["bikeDistance"].ToString();
            this.runDistance = reader["runDostance"].ToString();
            this.country_name = reader["contry"].ToString();
            this.year = int.Parse(reader["year"].ToString());

            return this;
        }
    }
}
