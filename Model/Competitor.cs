using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RestStoeitev
{
    public class Competitor
    {
        public int id { get; set; }
        public string name { get; set; }
        public string division { get; set; }    
        public string age { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string profession { get; set; }

        public Competitor ReadToCompatitor(MySqlDataReader reader)
        {
            //var competitor = new Competitor();

            this.id = int.Parse(reader["id"].ToString());
            this.name = reader["name"].ToString();
            this.division = reader["division"].ToString();
            this.state = reader["state"].ToString();
            this.country = reader["contry"].ToString();
            this.profession = reader["profession"].ToString();
            this.age = reader["age_catagory"].ToString();

            return this;
        }

        //public string genderRank { get; set; }
        //public string divRank { get; set; }
        //public string overallRank { get; set; }
        //public string bib { get; set; }
        //public string points { get; set; }
        //public TimeSpan swim { get; set; }
        //public TimeSpan t1 { get; set; }
        //public TimeSpan bike { get; set; }
        //public TimeSpan t2 { get; set; }
        //public TimeSpan run { get; set; }
        //public TimeSpan overall { get; set; }
        //public string comment { get; set; }
        //public string naziv { get; set; }
        //public string vrsta { get; set; }
        //public string swimDistance { get; set; }
        //public string runDistance { get; set; }
        //public string bikeDistance { get; set; }
        //public string country_name { get; set; }
        //public string year { get; set; }



    }


}
