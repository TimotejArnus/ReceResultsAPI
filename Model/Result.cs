using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RestStoeitev.Model
{
    public class Result
    {
        public int id { get; set; }
        public string genderRank { get; set; }
        public string divRank { get; set; }
        public string overallRank { get; set; }
        public string bib { get; set; }
        public string points { get; set; }
        public string swim { get; set; }
        public string t1 { get; set; }
        public string bike { get; set; }
        public string t2 { get; set; }
        public string run { get; set; }
        public string overall { get; set; }
        public string comment { get; set; }
        public int compatitorID { get; set; }
        public int raceID { get; set; }

        public Result ReadToResult(MySqlDataReader reader)
        {
            //var result = new Result();

            this.id = int.Parse(reader["id"].ToString());
            this.genderRank = reader["genderRank"].ToString();
            this.divRank = reader["divRank"].ToString();
            this.overallRank = reader["overallRank"].ToString();
            this.bib = reader["bib"].ToString();
            this.points = reader["points"].ToString();
            this.swim = reader["swim"].ToString();
            this.t1 = reader["t1"].ToString();
            this.bike = reader["bike"].ToString();
            this.t2 = reader["t2"].ToString();
            this.run = reader["run"].ToString();
            this.overall = reader["overall"].ToString();
            this.comment = reader["comments"].ToString();

            this.compatitorID = int.Parse(reader["competitor_id"].ToString());
            this.raceID = int.Parse(reader["race_id"].ToString());

            return this;
        }

    }
}
