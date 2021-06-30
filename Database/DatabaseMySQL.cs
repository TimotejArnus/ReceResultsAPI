using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using RestStoeitev.Model;


namespace RestStoeitev.Database
{
    public class DatabaseMySQL
    {
        private static MySqlConnection connection;

        static DatabaseMySQL()
        {
            connection = new MySqlConnection("server=127.0.0.1;user id=root;database=ozra;password=root");
        }

        //public static int MaxIdResults()
        //{
        //    connection.Open();
        //    string query = "select max(id) as steviloIdjev from rezultattekme";

        //    var cmd = new MySqlCommand(query, connection);
        //    MySqlDataReader reader = cmd.ExecuteReader();

        //    int id =0;

        //    while (reader.Read())
        //    {
        //        id = int.Parse(reader["id"].ToString());
        //    }

        //    return id;
        //}

        public static int MaxIdCompetitor()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Open();
            string query = "select max(id) as steviloIdjev from competitor";

            var cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            int id = 0;

            while (reader.Read())
            {
                id = int.Parse(reader["steviloIdjev"].ToString());
            }

            connection.Close();
            return id;
        }

        public static int MaxIdRace()
        {

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Open();
            string query = "select max(id) as steviloIdjev from race";

            var cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            int id = 0;

            while (reader.Read())
            {
                id = int.Parse(reader["steviloIdjev"].ToString());
            }

            connection.Close();
            return id;
        }

        public static IEnumerable<Podatek> QueryAll() // vrne seznam vseh tekmovalcev iz vseh tekem OMEJEN na 20
        {

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Open();
            string query = @"select * from rezultattekme
            inner join competitor on rezultattekme.competitor_id = competitor.id
            inner join race on rezultattekme.race_id = race.id;";

            var cmd = new MySqlCommand(query, connection);

            List<Podatek>
                list = new List<Podatek>(); // Generični razred ki združi podatke iz večih razredov da jih sravi v en podatek katerega lahko nato vrnemo na REST

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Podatek().Join(new Competitor().ReadToCompatitor(reader),
                    new Race().ReadToRace(reader), new Result().ReadToResult(reader)));

                if (list.Count >= 20)
                {
                    break;
                }
            }

            connection.Close();
            return list;
        }

        public IEnumerable<Competitor> QueryCompetitors()
        {

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Open();
            string query = @"select * from competitor";

            List<Competitor> list = new List<Competitor>();

            var cmd = new MySqlCommand(query, connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Competitor().ReadToCompatitor(reader));

            }

            connection.Close();
            return list;
        }

        public static IEnumerable<Race> QueryRaces()
        {

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Open();
            string query = @"select * from race"; // Napaka pri branju TODO: profession to country

            List<Race> list = new List<Race>();

            var cmd = new MySqlCommand(query, connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Race().ReadToRace(reader));
            }

            connection.Close();
            return list;
        }

        public static IEnumerable<Race> QueryRacesByCountry(string country)
        {

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Open();
            string query = @"select * from race where contry=" + "'" + country +
                           "'"; // Napaka pri branju TODO: profession to country

            List<Race> list = new List<Race>();

            var cmd = new MySqlCommand(query, connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Race().ReadToRace(reader));
            }

            connection.Close();
            return list;
        }

        public static int GetIdForRaceAndYear(string country, int year)
        {
            int id = 0;

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Open();
            string query = @"select id from race where contry=" + "'" + country + "'" + " and year=" +
                           year; // Napaka pri branju TODO: profession to country

            var cmd = new MySqlCommand(query, connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                id = int.Parse(reader["id"].ToString());
            }

            connection.Close();
            return id;
        }

        public static IEnumerable<Podatek> QueryCompetitorsByRace(string country, int year)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }


            int Raceid = GetIdForRaceAndYear(country, year); // pridobimo id dirke da lahko poiscemo njene tekmovalce
            connection.Open();
            string query =
                @"select * from competitor inner join rezultattekme on rezultattekme.competitor_id = competitor.id where rezultattekme.race_id = " +
                Raceid;

            List<Podatek> list = new List<Podatek>();

            var cmd = new MySqlCommand(query, connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Podatek().Join(new Competitor().ReadToCompatitor(reader),
                    new Result().ReadToResult(reader)));
            }

            connection.Close();
            return list;
        }






        // ..................................................................

        public static Podatek QueryCompetitorById(int id)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Open();
            string query =
                @"select * from competitor inner join rezultattekme r on competitor.id = r.competitor_id where competitor.id = " +
                id;


            var cmd = new MySqlCommand(query, connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            Podatek competitor = null;
            while (reader.Read())
            {
                competitor = new Podatek().Join(new Competitor().ReadToCompatitor(reader),
                    new Result().ReadToResult(reader));
            }

            connection.Close();
            return competitor;
        }

        public static void UpdateCompetitor(int id, Podatek podatek)
        {
            Competitor competitor = podatek.competitor;

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Open();
            string query = @"update competitor set name = " + "'" + competitor.name + "'" + ", division= " + "'" +
                           competitor.division + "'" +
                           ", state = " + "'" + competitor.state + "'" + ", contry= " + "'" + competitor.country + "'" +
                           ", profession = " + "'" +
                           competitor.profession + "'" + ", age_catagory = " + "'" + competitor.age + "'" +
                           " where id = " + id;
            var cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            connection.Close();

        }

        public static void InsertCompetitor(int id, Podatek podatek)
        {
            Competitor competitor = podatek.competitor;

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Open();
            string query = @"insert into competitor values(" + id + "," + "'" + competitor.name + "'" + "," + "'" +
                           competitor.division + "'" + "," + "'" + competitor.state + "'" + "," + "'" +
                           competitor.country + "'" + "," + "'" + competitor.profession + "'" + "," + "'" +
                           competitor.age + "'" + ")";
            var cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            connection.Close();
        }

        public static void InsertResult(int id, Podatek podatek)
        {
            Result result = podatek.result;

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Open();
            string query = @"insert into rezultattekme values (" + id + "," + result.genderRank + "," + result.divRank +
                           "," + "'" + result.overallRank + "'" + "," + result.bib + "," + "'" + result.points +
                           "'" + "," + "'" + result.swim + "'" + "," + "'" + result.t1 + "'" + "," + "'" +
                           result.bike + "'" + "," + "'" + "," + result.t2 + "'" + "," + "'" + result.run + "'" + "," +
                           "'" + result.overall + "'" + "," + "'" + result.comment + "'" + "," + result.compatitorID +
                           "," + result.raceID + ")";
            var cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            connection.Close();
        }



        public static void UpdateResult(int id, Podatek podatek)
        {
            Result result = podatek.result;

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Open();
            string query = @"update rezultattekme set genderRank = " + result.genderRank + " , divRank = " +
                           result.divRank + ", overallRank = " + "'" + result.overallRank + "'" + ", bib = " +
                           result.bib +
                           ", points = " + "'" + result.points + "'" + ", swim = " + "'" + result.swim + "'" +
                           ", t1 = " + "'" + result.t1 + "'" +
                           ", bike = " + "'" + result.bike + "'" + ", t2 = " + "'" + result.t2 + "'" + ",run = " + "'" +
                           result.run + "'" + ",overall = " + "'" +
                           result.overall + "'" + ", comments = " + "'" + result.comment + "'" + " where id =" + id;
            var cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            connection.Close();

        }

        public static void SetCheckTo0()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Open();

            string query = @"SET FOREIGN_KEY_CHECKS = 0";
            var cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            connection.Close();
        }

        public static void DeleteCompetitor(int id)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Open();

            string query = @" Delete from competitor where id = " + id;
            var cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            connection.Close();
        }

        public static void DeleteResult(int id)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Open();

            string query = @" Delete from rezultattekme where id = " + id;
            var cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            connection.Close();
        }


        // ...........................................................................
        // Langs

        public static string GetLang()
        {
            string language = "";

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();

            string query = @"select * from lang where id = 1";
            var cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            

            while (reader.Read())
            {
                language = reader["language"].ToString();
            }

            connection.Close();
            return language;
        }
        public static void ChangeLang(string Lang)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();

            string query = @"update Lang set language = " + "'" + Lang + "'" + " where id = 1";
            var cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            connection.Close();
        }
        


        //public static int MaxIdCompetitor()
        //{
        //    int maxid = 0;

        //    if (connection.State == ConnectionState.Open)
        //    {
        //        connection.Close();
        //    }

        //    connection.Open();
        //    string query = @"select max(competitor.id) from competitor";


        //    var cmd = new MySqlCommand(query, connection);
        //    MySqlDataReader reader = cmd.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        maxid = int.Parse(reader["id"].ToString());
        //    }

        //    return maxid;

        //    connection.Close();
        //}






        //public static Dictionary<string, int> QueryCompetitorsCountry()
        //{
        //    connection.Open();
        //    string query = @"select profession from competitor";    // Napaka pri branju TODO: profession to country

        //    List<string> list = new List<string>();
        //    Dictionary<string, int> listCountries = new Dictionary<string, int>();


        //    var cmd = new MySqlCommand(query, connection);

        //    MySqlDataReader reader = cmd.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        list.Add(reader["profession"].ToString());
        //    }

        //    foreach (var country in list)
        //    {
        //        if (listCountries.ContainsKey(country))
        //        {
        //            int value = listCountries[country];
        //            listCountries[country] = ++value;
        //        }
        //        else
        //        {
        //            listCountries.Add(country, 1 );       // lahko se uporabi kot statistika koliko tekmovalcev je iz iste drzave
        //        }
        //    }

        //    connection.Close();
        //    return listCountries;


        //}


        public static IEnumerable<Podatek> Query(string name)    // poisce tekmovalca z dolocenima imenom in rezultat
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();

            name.Replace('"', ' ');

            string query = @"select * from rezultattekme inner join competitor on rezultattekme.competitorid = competitor.id inner join race on rezultattekme.raceid = race.id where competitor.name  LIKE " + "'" + "%" + name + "%" + "'";
            var cmd = new MySqlCommand(query, connection);

            List<Podatek> list = new List<Podatek>();
            Competitor competitor = null;
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {

                list.Add(new Podatek().Join(new Competitor().ReadToCompatitor(reader),
                    new Race().ReadToRace(reader), new Result().ReadToResult(reader)));

            }

            connection.Close();
            return list;

        }

        public static IEnumerable<Race> QueryAllRaces()    // poisce tekmovalca z dolocenima imenom in rezultat
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();

            string query = @"select * from race ";
            var cmd = new MySqlCommand(query, connection);

            List<Race> list = new List<Race>();
            Competitor competitor = null;
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Race().ReadToRace(reader));
            }

            connection.Close();
            return list;

        }



        //public static Competitor QueryCompetitorName(int id)    // poisce tekmovalca z dolocenima id-jom in rezultat
        //{
        //    connection.Open();

        //    string query = @"select * from rezultattekme
        //    inner join competitor on rezultattekme.competitorid = competitor.id
        //    inner join race on rezultattekme.raceid = race.id
        //    where competitor.id = " + id + "";
        //    var cmd = new MySqlCommand(query, connection);

        //    Competitor competitor = null;
        //    MySqlDataReader reader = cmd.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        list.Add(new Podatek().Join(new Competitor().ReadToCompatitor(reader),
        //            new Race().ReadToRace(reader), new Result().ReadToRace(reader)));
        //    }

        //    connection.Close();
        //    return competitor;

        //}



        //public static Competitor QueryRace(int id)
        //{
        //    connection.Open();

        //    string query = @"select * from race where id = " + id + "";
        //    var cmd = new MySqlCommand(query, connection);

        //    Competitor competitor = null;
        //    MySqlDataReader reader = cmd.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        competitor = new Competitor();
        //        competitor.vrsta = reader["vrsta"].ToString();
        //        competitor.swimDistance = reader["swimDistance"].ToString();
        //        competitor.bikeDistance = reader["bikeDistance"].ToString();
        //        competitor.runDistance = reader["runDostance"].ToString();
        //        competitor.country_name = reader["country_name"].ToString();
        //        competitor.year = reader["year"].ToString();
        //    }

        //    connection.Close();
        //    return competitor;
        //}

        //public static IEnumerable<Competitor> QueryRace(string country)
        //{
        //    connection.Open();

        //    string query = @"select * from rezultattekme inner join competitor on rezultattekme.competitorid = competitor.id inner join race on rezultattekme.raceid = race.id where race.country_name  LIKE " + "'" + "%" + country + "%" + "'";
        //    var cmd = new MySqlCommand(query, connection);

        //    Competitor competitor = null;
        //    List<Competitor> list = new List<Competitor>();
        //    MySqlDataReader reader = cmd.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        competitor = new Competitor();
        //        competitor.vrsta = reader["vrsta"].ToString();
        //        competitor.swimDistance = reader["swimDistance"].ToString();
        //        competitor.bikeDistance = reader["bikeDistance"].ToString();
        //        competitor.runDistance = reader["runDostance"].ToString();
        //        competitor.country_name = reader["country_name"].ToString();
        //        competitor.year = reader["year"].ToString();
        //        list.Add(competitor);
        //    }

        //    connection.Close();
        //    return list;
        //}

        //public static IEnumerable<Competitor> QueryRaceYaer(string year)
        //{
        //    connection.Open();

        //    string query = @"select * from rezultattekme inner join competitor on rezultattekme.competitorid = competitor.id inner join race on rezultattekme.raceid = race.id where race.year  LIKE " + "'" + "%" + year + "%" + "'";
        //    var cmd = new MySqlCommand(query, connection);

        //    Competitor competitor = null;
        //    List<Competitor> list = new List<Competitor>();
        //    MySqlDataReader reader = cmd.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        competitor = new Competitor();
        //        competitor.vrsta = reader["vrsta"].ToString();
        //        competitor.swimDistance = reader["swimDistance"].ToString();
        //        competitor.bikeDistance = reader["bikeDistance"].ToString();
        //        competitor.runDistance = reader["runDostance"].ToString();
        //        competitor.country_name = reader["country_name"].ToString();
        //        competitor.year = reader["year"].ToString();
        //        list.Add(competitor);
        //    }

        //    connection.Close();
        //    return list;
        //}

        //public static IEnumerable<Competitor> QueryRaceCompetitors(string name)
        //{
        //    connection.Open();

        //    string query = @"select * from rezultattekme inner join competitor on rezultattekme.competitorid = competitor.id inner join race on rezultattekme.raceid = race.id where race.maziv  LIKE " + "'" + "%" + name + "%" + "'";
        //    var cmd = new MySqlCommand(query, connection);

        //    Competitor competitor = null;
        //    List<Competitor> list = new List<Competitor>();
        //    MySqlDataReader reader = cmd.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        competitor = new Competitor();
        //        competitor.vrsta = reader["vrsta"].ToString();
        //        competitor.swimDistance = reader["swimDistance"].ToString();
        //        competitor.bikeDistance = reader["bikeDistance"].ToString();
        //        competitor.runDistance = reader["runDostance"].ToString();
        //        competitor.country_name = reader["country_name"].ToString();
        //        competitor.year = reader["year"].ToString();
        //        list.Add(competitor);
        //    }

        //    connection.Close();
        //    return list;
        //}

        public static void UpdateCompetitorsName(int id, string name)
        {
            connection.Open();

            string query = @"UPDATE competitor set name = "+ "'"+ name +"'"+ " where id = " + "'" + id + "'" + "";
            var cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public static void DeleteCompetitorById(int id)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();

            string query = @"DELETE FROM competitor WHERE id = " + id +"";
            var cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public static void PostCompetitor(string name, string division, string state, string country,
            string profession, string age_catagory)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            int id = MaxIdCompetitor() + 1;

            connection.Open();

            string query = @"insert into competitor values(" + id + "," + "'" + name + "'" + "," + "'" + division + "'" + "," + "'" + state + "'" + "," + "'" + country + "'" + "," + "'" + profession + "'" + "," + "'" + age_catagory + "'" + ")";
            var cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();

            connection.Close();

        }

        ~DatabaseMySQL()
        {
            connection.Close();
        }
    }
}
