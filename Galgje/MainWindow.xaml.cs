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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.VisualBasic;

namespace Galgje
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Multi : Window
    {
        string speler1;
        string speler2;
        string[] highscoreLijst1 = new string[3] { "naam", "levens", "tijd" };
        string[] highscoreLijst2 = new string[3] { "naam", "levens", "tijd" };
        string[] highscoreLijst3 = new string[3] { "naam", "levens", "tijd" };
        string[] highscoreLijst4 = new string[3] { "naam", "levens", "tijd" };
        string[] highscoreLijst5 = new string[3] { "naam", "levens", "tijd" };


        bool gameLost = false;
        bool gameWon = false;
        bool gevonden = false;
        bool newgame = false;
        char[] teRadenWoord;
        char[] geradenWoord;        
        string fouteLetters = "";
        int levens = 10;
        const char underscore = '＿';

        DispatcherTimer timer2 = new DispatcherTimer();
        int userTime;
        int time;
        
        public Multi()
        {            
            InitializeComponent();
            timer();
            afteller();
            txbInput.Focus();
            lblAfteller.Visibility = Visibility.Hidden;
            lblWoord.Visibility = Visibility.Hidden;
            lblFout.Visibility = Visibility.Hidden;
            imgGalg.Visibility = Visibility.Hidden;
            lblLevens.Visibility = Visibility.Hidden;
            btnRaad.Visibility = Visibility.Hidden;
            btnHint.Visibility = Visibility.Hidden;
            spelersToevoegen();            
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
                    neemLeven();
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
                gameLost = true;
                geradenWoord = teRadenWoord;                
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
            newgame = true;
            resetgame();
            /*System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();*/
        }

        private void btnVerbergWoord_Click(object sender, RoutedEventArgs e)
        {

            userTime = int.Parse(Interaction.InputBox("Zet de timer tussen de 5 en de 20 seconden", "Zet de timer"));
            if (userTime < 5)
            {
                MessageBox.Show("Timer moet boven de 5 seconden zijn");                         //Vraagt user om tijd in te stellen
                return;
            }
            else if (userTime > 20)                                                             //Check of dit tussen de 5 en de 20 sec is
            {                                                                                   //Zo niet -> return
                MessageBox.Show("Timer moet onder de 20 seconden zijn");
                return ;
            }

            if (txbInput.Text.Length == 1)
            {
                MessageBox.Show("Woord moet minimum 2 letters bevatten");                       //Kijkt of het een woord is, woord bevat meer dan 1 letter
                return;

            }
            else
            {
                teRadenWoord = txbInput.Text.ToLower().ToCharArray();                           //Zet hoofdletters naar lowercase en zet het dan in een char array
                geradenWoord = new char[teRadenWoord.Length];

                bool isIntString = txbInput.Text.Any(c => char.IsDigit(c));                     //Check op cijfers

                if (isIntString == false && !string.IsNullOrWhiteSpace(txbInput.Text) && !txbInput.Text.Any(Char.IsWhiteSpace))  //Check op spaties
                {
                
                     for (int i = 0; i < geradenWoord.Length; i++)
                     {
                        geradenWoord[i] = '＿';                                                  //Zet elke char om naar een underscore    
                     }
                
                    btnRaad.Visibility= Visibility.Visible;
                    btnHint.Visibility= Visibility.Visible;
                    txbInput.Text = string.Empty;
                    updateScherm();
                }
                else
                {
                    MessageBox.Show("Woord mag geen spaties of cijfers bevatten");
                    return;
                }

                btnVerbergWoord.Visibility = Visibility.Hidden;
            }

        }
        private void updateScherm()
        {
            string woord = "";
            string hartjes = "";

            for(int i = 0; i < geradenWoord.Length; i++)                        //Update het scherm
            {                                                                   //Zet spaties op de juiste plaats
                woord += geradenWoord[i] + " ";
            }

            for(int i = 0; i < levens; i++)
            {
                hartjes += "♥ ";                                                //Zorgt voor het afbeelde van het juiste aantal levens
            }
                        

            lblWoord.Visibility = Visibility.Visible;
            lblFout.Visibility = Visibility.Visible;
            imgGalg.Visibility= Visibility.Visible;
            lblLevens.Visibility = Visibility.Visible;
            lblAfteller.Visibility = Visibility.Visible;
            lblHartjes.Content = hartjes;
            lblResultaat.Content = $"{woord}\n{fouteLetters}";
            SolidColorBrush solidColor = new SolidColorBrush(Color.FromRgb(47, 47, 47));    //Zet de achtergrond terug op de juiste kleur
            kleur.Background = solidColor;
            time = userTime;            
            timer2.Start();

        }
               
        private void raadLetter(char letter) 
        {
            for (int i = 0; i < teRadenWoord.Length; i++)                           //Gaat over de lengte van het woord
            {
                if(letter.Equals(teRadenWoord[i]))                                  //Kijkt of het ingegeven karakter voorkomt in het te raden woord
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
                timer2.Stop();
                updateScherm();
            }
            else
            {
                neemLeven();                
            }

            if (gameWon)
            {              
                DateTime huidigeTijd = DateTime.UtcNow;
                string huidigeTijdStr = huidigeTijd.ToString("T");
                timer2.Stop();
                string score = levens.ToString();
                highscoreLijst1[1] = score;
                highscoreLijst1[2] = huidigeTijdStr;
                MessageBox.Show($"Proficiat! {speler2} heeft gewonnen!");
                MessageBox.Show($"{highscoreLijst1[0]} {highscoreLijst1[1]} {highscoreLijst1[2]}");
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
                timer2.Start();
                
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
                    
                updateScherm();

            }

        }

        private void resetgame()
        {
            if (newgame)
            {
                timer2.Stop();
                InitializeComponent();
                timer();
                afteller();
                txbInput.Focus();
                lblResultaat.Content = "Geef een geheim woord";
                btnVerbergWoord.Visibility = Visibility.Visible;
                lblAfteller.Visibility = Visibility.Hidden;
                lblWoord.Visibility = Visibility.Hidden;
                lblFout.Visibility = Visibility.Hidden;
                imgGalg.Visibility = Visibility.Hidden;
                lblLevens.Visibility = Visibility.Hidden;
                btnRaad.Visibility = Visibility.Hidden;
                btnHint.Visibility = Visibility.Hidden;
                spelersToevoegen();
                gameLost = false;
                gevonden = false;
                newgame = false;                
                fouteLetters = "";
                levens = 10;
                              
            }
        }
         
        private void spelersToevoegen()
        {
            speler1 = Interaction.InputBox("Geef de naam van speler 1", "Speler 1");
            speler2 = Interaction.InputBox("Geef de naam van speler 2", "Speler 2");
            MessageBox.Show($"Speler 1: {speler1} \rSpeler 2: {speler2}");
            highscoreLijst1[0] = speler2;              

        }

        /*private void highscore()
        {
            for (int i = 0; highscoreLijst.Length > 0; i++)
            {
                if (highscoreLijst[i])
                {
                    highscoreLijst[i] = speler2;
                }
            }
            MessageBox.Show($"{highscoreLijst}");
        }*/

    }

}
