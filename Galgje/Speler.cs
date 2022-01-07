using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galgje
{
    internal class Speler
    {
        private string naam;
        private int score;
        private DateTime tijd;

        public Speler(string naam, int score, DateTime tijd)
        {
            this.Naam = naam;
            this.Score = score; 
            this.tijd = tijd;
        }

        public string Naam { get => naam; set => naam = value; }
        public int Score { get => score; set => score = value; }
        public DateTime Tijd { get => tijd; set => tijd = value; }

        public string info { get => $"{naam} \t{score.ToString()} \t{tijd.ToString("T")}";}
    }
}
