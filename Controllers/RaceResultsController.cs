using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestStoeitev.Database;
using RestStoeitev.Model;

namespace RestStoeitev.Controllers
{
        // GET 

    [ApiController]
    [Route("[controller]")]
    public class RaceResultsController : ControllerBase // pripravi bazo ter dodaj drzavo in leto 
    {
        // GET: api/RaceResults
        //[HttpGet]
        //public IEnumerable<Podatek> Get()    // Pridobi seznam vseh tekmovalcev iz vseh tekem
        //{
        //   return DatabaseMySQL.QueryAll();
        //}

        //[HttpGet("Competitors")]
        //public IEnumerable<Competitor> GetAllCompetitors()    // Pridobi seznam vseh tekmovalcev iz vseh tekem
        //{
        //    return DatabaseMySQL.QueryCompetitors();
        //}

        [HttpGet("Race/Country")] 
        public IEnumerable<Race> GetAllRaceCountries()    
        {
            return DatabaseMySQL.QueryRaces(); 
        }

        [HttpGet("Race/Country={country}")] 
        public IEnumerable<Race> GetAllRaceByCountry(string country)    
        {
            return DatabaseMySQL.QueryRacesByCountry(country); 
        }

        [HttpGet("Race/Country={country}/Year={year}")] 
        public IEnumerable<Podatek> QueryCompetitorsByRace(string country, int year)    
        {
            return DatabaseMySQL.QueryCompetitorsByRace(country, year); 
        }

        [HttpGet("Race/Id/Country={country}/Year={year}")] 
        public int GetRaceId(string country, int year)    
        {
            return DatabaseMySQL.GetIdForRaceAndYear(country, year); 
        }

        //........................................................................................................................................

        [HttpPost("Competitor/Change/Id={id}")]
        public void ChangeCompetitor(int id, [FromBody] Podatek podatek)
        {
            DatabaseMySQL.UpdateCompetitor(id, podatek);
            DatabaseMySQL.UpdateResult(id, podatek);
        }

        [HttpPost("Competitor/Insert/Id={id}")] // vstavljanje 
        public void InsertCompetitor(int id, [FromBody] Podatek podatek)
        {
            DatabaseMySQL.InsertCompetitor(id, podatek);
            DatabaseMySQL.InsertResult(id, podatek);
        }

        [HttpDelete("Competitor/Id={id}")]
        public void DeleteCompetitorAndResult(int id)
        {
            DatabaseMySQL.SetCheckTo0();
            DatabaseMySQL.DeleteCompetitor(id);
            DatabaseMySQL.DeleteResult(id);
        }


        // ...................................................................................................................................

        [HttpGet("Competitor/Id={id}")]
        public Podatek GetByCompetitorById(int id)   // Pridobi tekmovalce po imenu oz ujemanju imena 'LIKE' 
        {
            return DatabaseMySQL.QueryCompetitorById(id);
        }

        [HttpGet("Competitor/Maxid")]
        public int GetCompetitorsMaxId()   // Pridobi tekmovalce po imenu oz ujemanju imena 'LIKE' 
        {
            return DatabaseMySQL.MaxIdCompetitor();
        }

        //............................................................
        // JeZIK 

        [HttpGet("Lang")] // pridobi izbran jezik aplikacije
        public string GetLang()
        {
            return DatabaseMySQL.GetLang();
        }

        [HttpGet("Lang/{lang}")] // Spremeni izbran jezik
        public void SetLang(string lang)
        {
            DatabaseMySQL.ChangeLang(lang);
        }

        //[HttpGet("Competitors/Country")] // TODO countries in specific race ! 
        //public IEnumerable<Competitor> GetAllCompetitors()    // Pridobi seznam vseh tekmovalcev iz vseh tekem
        //{
        //    return DatabaseMySQL.QueryCompetitors(); // vrne drzave in stevilo tekmovalcez iz te drzave 
        //}

        //[HttpGet("Competitor/Name={name}")]           
        //public IEnumerable<Podatek> GetByName(string name)   // Pridobi tekmovalce po imenu oz ujemanju imena 'LIKE' 
        //{
        //    return DatabaseMySQL.Query(name);
        //}

        // GET: api/RaceResults/5
        //[HttpGet("{id}")]             !!
        //public Competitor GetById(int id)
        //{
        //   return DatabaseMySQL.Query(id);
        //}

        //[HttpGet("{id}", Name = "Get")]
        //public Competitor GetById(int id)
        //{
        //    return DatabaseMySQL.Query(id);
        //}



        //[HttpGet("Race/{id}")]
        //public Competitor GetByRaceId(int id)    
        //{
        //    return DatabaseMySQL.QueryRace(id);
        //    //return DatabaseMySQL.Query(name);
        //}

        //[HttpGet("Race/Country={country}")] // iskanje po drzavah 'LIKE'              !!
        //public IEnumerable<Competitor> GetByRaceName(string country)    
        //{
        //    return DatabaseMySQL.QueryRace(country);

        //}

        //[HttpGet("Race/Year={year}")]
        //public IEnumerable<Competitor> GetByRaceYear(string year)      // iskanje po letu             !!
        //{
        //    return DatabaseMySQL.QueryRaceYaer(year);

        //}

        //[HttpGet("Competitor/Naziv={naziv}")]
        //public IEnumerable<Competitor> GetByRaceCompetitors(string naziv)    
        //{
        //    return DatabaseMySQL.QueryRaceYaer(naziv);
        //}

        //[HttpGet("Race/All")]

        //public IEnumerable<Race> GetAllRaces()
        //{
        //    return DatabaseMySQL.QueryAllRaces();
        //}

        //[HttpDelete("Competitor/{id}")]
        //public void DeleteByid(int id)
        //{
        //    DatabaseMySQL.DeleteCompetitorById(id);
        //}

        //////https://localhost:44332/raceresults/Competitor/Name=Janez/Division=34-55/State=AM/Country=NY/Profession=Ni/Age_catagory=34

        //[HttpPost("Competitor/Name={name}/Division={division}/State={state}/Country={country}/Profession={profession}/Age_catagory={age_catagory}")]
        //public void PostCompetitor(string name, string division, string state, string country, string profession, string age_catagory)
        //{
        //    DatabaseMySQL.PostCompetitor(name, division, state, country, profession, age_catagory);
        //}

        //[HttpPost("Race/Naziv={naziv}/Contry_name={contry_name}/Year={year}Vrsta={vrsta}")] // TODO: dodaj distance na podlagi vrste 
        //public void PostRace(string naziv,string coutry_name, int year, string vrsta) 
        //{
        //    //DatabaseMySQL.PostCompetitor(name, division, state, country, profession, age_catagory);
        //}

        //[HttpPost("Result/GenderRank={genderRank}/DivRank={divRank}/OverallRank={overallRank}/Bib={bib}/Points={points}/Swim={swim}/T1={t1}/Bike={bike}/T2={t2}/Run={run}/Overall={overall}/Comment={comment}/ID_Compatitor={compatitorID}/ID_Race={raceID}")]  // TODO baza daj points to int
        //public void PostResult(string genderRank, string division, string state, string country, string profession, string age_catagory)
        //{
        //    //DatabaseMySQL.PostCompetitor(name, division, state, country, profession, age_catagory);
        //}

    }
}
