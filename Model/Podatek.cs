using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestStoeitev.Model
{
    public class Podatek        // Razred se uporablja za podatke, ki jih he potrbno združiti predno so poslani naprej !
    {
        public Competitor competitor { get; set; }
        public Race race { get; set; }
        public Result result { get; set; }
        public Podatek Join(Competitor competitor, Race race , Result result) // Združimo podatke iz vseh 3 tabel
        {
            var Podatek = new Podatek();
            Podatek.competitor = competitor;
            Podatek.race = race;
            Podatek.result = result;

            return Podatek;
        }

        public Podatek Join(Competitor competitor, Result result) // Združimo podatke iz vseh 3 tabel
        {
            var Podatek = new Podatek();
            Podatek.competitor = competitor;
            Podatek.result = result;

            return Podatek;
        
        }


        // TODO: dodaj združevenje drugih tabel če bo to potrebno 


    }
}
