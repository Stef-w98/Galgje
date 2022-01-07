using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.VisualBasic;

namespace Galgje
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Single : Window
    {
        public static string[] galgjeWoorden = new string[]
{
    "grafeem",
    "tjiftjaf",
    "maquette",
    "kitsch",
    "pochet",
    "convocaat",
    "jakkeren",
    "collaps",
    "zuivel",
    "cesium",
    "voyant",
    "spitten",
    "pancake",
    "gietlepel",
    "karwats",
    "dehydreren",
    "viswijf",
    "flater",
    "cretonne",
    "sennhut",
    "tichel",
    "wijten",
    "cadeau",
    "trotyl",
    "chopper",
    "pielen",
    "vigeren",
    "vrijuit",
    "dimorf",
    "kolchoz",
    "janhen",
    "plexus",
    "borium",
    "ontweien",
    "quiche",
    "ijverig",
    "mecenaat",
    "falset",
    "telexen",
    "hieruit",
    "femelaar",
    "cohesie",
    "exogeen",
    "plebejer",
    "opbouw",
    "zodiak",
    "volder",
    "vrezen",
    "convex",
    "verzenden",
    "ijstijd",
    "fetisj",
    "gerekt",
    "necrose",
    "conclaaf",
    "clipper",
    "poppetjes",
    "looikuip",
    "hinten",
    "inbreng",
    "arbitraal",
    "dewijl",
    "kapzaag",
    "welletjes",
    "bissen",
    "catgut",
    "oxymoron",
    "heerschaar",
    "ureter",
    "kijkbuis",
    "dryade",
    "grofweg",
    "laudanum",
    "excitatie",
    "revolte",
    "heugel",
    "geroerd",
    "hierbij",
    "glazig",
    "pussen",
    "liquide",
    "aquarium",
    "formol",
    "kwelder",
    "zwager",
    "vuldop",
    "halfaap",
    "hansop",
    "windvaan",
    "bewogen",
    "vulstuk",
    "efemeer",
    "decisief",
    "omslag",
    "prairie",
    "schuit",
    "weivlies",
    "ontzeggen",
    "schijn",
    "sousafoon"
};
        static Random randWoord = new Random();
        int index = randWoord.Next(1, galgjeWoorden.Length);        
        bool gevonden = false;
        string speler2;
        List<Speler> highscoreLijst = new List<Speler>();
        bool hintGebruikt = false;
        bool gameLost = false;
        bool gameWon = false;
        bool newgame = false;
        const char underscore = '＿';
        char[] teRadenWoord;
        char[] geradenWoord;
        string fouteLetters = "";
        int levens = 10;
        DispatcherTimer timer2 = new DispatcherTimer();
        int time = 10;

        public Single()
        {
            InitializeComponent();
            timer();
            afteller();
            txbInput.Visibility = Visibility.Hidden;
            lblAfteller.Visibility = Visibility.Hidden;
            lblWoord.Visibility = Visibility.Hidden;
            lblFout.Visibility = Visibility.Hidden;
            imgGalg.Visibility = Visibility.Hidden;
            lblLevens.Visibility = Visibility.Hidden;
            btnRaad.Visibility = Visibility.Hidden;
            btnHint.Visibility = Visibility.Hidden;
            spelersToevoegen();
            timer2.Stop();
        }

        private void txbInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnRaad_Click(object sender, RoutedEventArgs e)
        {
            gevonden = false;
            string gebruikersInput = txbInput.Text.ToLower();

            if (gebruikersInput.Length == 1)
            {
                raadLetter(gebruikersInput[0]);

                if (!gevonden && !fouteLetters.Contains(gebruikersInput))
                {
                    fouteLetters += gebruikersInput;
                    fouteLetters += " ";
                    levens--;
                    imgGalg.Source = new BitmapImage(new Uri(@"/img/hangman" + levens + ".png", UriKind.RelativeOrAbsolute));
                }
            }
            else if (gebruikersInput.Length > 1)
            {
                raadWoord(gebruikersInput.ToCharArray());
            }
            else
            {
                return;
            }

            if (levens == 0)
            {
                timer2.Stop();
                geradenWoord = teRadenWoord;
                MessageBox.Show("Sorry, Je hebt veloren!");
                btnRaad.Visibility = Visibility.Hidden;
            }

            if (gameLost)
            {
                updateScherm();
                timer2.Stop();
                MessageBox.Show($"Sorry {speler2}, je hebt veloren!");
            }
            else
            {
                updateScherm();
                txbInput.Text = string.Empty;
                txbInput.Focus();
            }
            
        }

        private void btnNieuwSpel_Click(object sender, RoutedEventArgs e)
        {
            timer2.Stop();
            newgame = true;
            resetgame();
        }

        private void btnVerbergWoord_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show($"{galgjeWoorden[index]}");
            //time = int.Parse(Interaction.InputBox("Enter a number", "Set Timer"));

            if (galgjeWoorden[index].Length == 1)
            {
                MessageBox.Show("Woord moet minimum 2 letters bevatten");
                return;
            }
            else
            {
                teRadenWoord = galgjeWoorden[index].ToLower().ToCharArray();
                geradenWoord = new char[teRadenWoord.Length];                

                    for (int i = 0; i < geradenWoord.Length; i++)
                    {
                        geradenWoord[i] = '＿';

                    }

                btnRaad.Visibility = Visibility.Visible;
                txbInput.Text = string.Empty;
                updateScherm();
                btnVerbergWoord.Visibility = Visibility.Hidden;
                txbInput.Visibility = Visibility.Visible;
                txbInput.Focus();
            }

        }
        private void updateScherm()
        {
            string woord = "";
            string hartjes = "";

            for (int i = 0; i < geradenWoord.Length; i++)
            {
                woord += geradenWoord[i] + " ";
            }

            for (int i = 0; i < levens; i++)
            {
                hartjes += "♥ ";
            }

            lblWoord.Visibility = Visibility.Visible;
            lblFout.Visibility = Visibility.Visible;
            imgGalg.Visibility = Visibility.Visible;
            lblLevens.Visibility = Visibility.Visible;
            lblAfteller.Visibility = Visibility.Visible;
            btnHint.Visibility = Visibility.Visible;
            lblHartjes.Visibility = Visibility.Visible;
            lblHartjes.Content = hartjes;
            lblResultaat.Content = $"{woord}\n{fouteLetters}";
            SolidColorBrush solidColor = new SolidColorBrush(Color.FromRgb(47, 47, 47));
            kleur.Background = solidColor;            
            time = 10;
            timer2.Start();
            
        }

        private void raadLetter(char letter)
        {
            for (int i = 0; i < teRadenWoord.Length; i++)                           //Gaat over de lengte van het woord
            {
                if (letter.Equals(teRadenWoord[i]))                                  //Kijkt of het ingegeven karakter voorkomt in het te raden woord
                {
                    geradenWoord[i] = letter;                                       //Zet de _ om in het juist geraden karakter
                    gevonden = true;
                }
            }
        }

        private void raadWoord(char[] gebruikersInput)
        {
            if (gebruikersInput.SequenceEqual(teRadenWoord))                        //Kijkt of het ingegeven woord gelijk is aan het te raden woord
            {
                geradenWoord = teRadenWoord;                                        //Vervangt de _ met het juist geraden woord
                btnRaad.Visibility = Visibility.Hidden;
                gevonden = true;
                gameWon = true;
                updateScherm();
                timer2.Stop();
            }
            else
            {
                neemLeven();
            }

            if (gameWon)
            {
                DateTime huidigeTijd = DateTime.Now;
                updateHighscore(huidigeTijd, levens, speler2);
                timer2.Stop();
                MessageBox.Show($"Proficiat! {speler2} heeft gewonnen!", "Winner!", MessageBoxButton.OK);
                MessageBox.Show(highscoreLijststr(), "Highscore", MessageBoxButton.OK);
            }
        }

        private void neemLeven()
        {
            levens--;                                                           //Als het woord fout geraden is gaat er 1 leven af en komt er een stuk van de afbeelding bij
            imgGalg.Source = new BitmapImage(new Uri(@"/img/hangman" + levens + ".png", UriKind.RelativeOrAbsolute));
            if (levens == 0)
            {
                gameLost = true;
                geradenWoord = teRadenWoord;
                btnRaad.Visibility = Visibility.Hidden;
            }
        }

        private void timer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblTimer.Content = DateTime.Now.ToString("HH:mm:ss");                   //Zet de timer op het label
        }
        private void afteller()
        {
            timer2.Tick += Timer_Tick1;
            timer2.Interval = new TimeSpan(0, 0, 1);
        }

        private void Timer_Tick1(object sender, EventArgs e)
        {

            lblAfteller.Content = time;
            time--;
            if (time < 0)
            {
                timer2.Stop();
                neemLeven();
                SolidColorBrush solidColorRed = new SolidColorBrush(Colors.Red);
                kleur.Background = solidColorRed;
                MessageBox.Show("Te traag");
                updateScherm();
                //timer2.Start();

            }
        }

        private void btnHint_Click(object sender, RoutedEventArgs e)
        {

            bool hintFound = false;
            while (!hintFound && geradenWoord.Contains(underscore))
            {
                Random randomLetter = new Random();
                int index = randomLetter.Next(0, teRadenWoord.Length);
                if (geradenWoord[index] == underscore)
                {
                    hintFound = true;
                    raadLetter(teRadenWoord[index]);
                }

                hintGebruikt = true;
                updateScherm();
            }

        }

        private void resetgame()
        {
            if (newgame)
            {
                InitializeComponent();
                timer();
                txbInput.Visibility = Visibility.Hidden;
                lblResultaat.Content = "Start het spel";
                btnVerbergWoord.Visibility = Visibility.Visible;
                lblAfteller.Visibility = Visibility.Hidden;
                lblWoord.Visibility = Visibility.Hidden;
                lblFout.Visibility = Visibility.Hidden;
                imgGalg.Visibility = Visibility.Hidden;
                lblLevens.Visibility = Visibility.Hidden;
                lblHartjes.Visibility = Visibility.Hidden;
                btnRaad.Visibility = Visibility.Hidden;
                btnHint.Visibility = Visibility.Hidden;
                spelersToevoegen();                   
                index = randWoord.Next(1, galgjeWoorden.Length);                
                hintGebruikt = false;
                gameLost = false;
                gevonden = false;
                newgame = false;
                fouteLetters = "";
                levens = 10;
                imgGalg.Source = new BitmapImage(new Uri(@"/img/hangman" + levens + ".png", UriKind.RelativeOrAbsolute));
                timer2.Stop();
            }
        }

        private void spelersToevoegen()
        {
            
            speler2 = Interaction.InputBox("Geef je naam", "Speler");
            MessageBox.Show($"Naam: {speler2}");

        }

        private void updateHighscore(DateTime tijd, int levens, string naam)
        {
            if (hintGebruikt)
            {
                return;
            }
            Speler speler = new Speler(naam, levens, tijd);
            highscoreLijst.Add(speler);
            highscoreLijst.Sort(vergelijk);
            for (int i = highscoreLijst.Count - 1; i >= 5; i--)
            {
                highscoreLijst.RemoveAt(i);
            }
        }

        public string highscoreLijststr()
        {
            string lijst = "";

            foreach (Speler speler in highscoreLijst)
            {
                lijst += speler.info + "\n";
            }
            return lijst;
        }

        private int vergelijk(Speler a, Speler b)
        {
            return b.Score.CompareTo(a.Score);
        }

        private void btnAfsluiten_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnHighscore_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(highscoreLijststr(), "Highscore", MessageBoxButton.OK);
        }

    }
}
