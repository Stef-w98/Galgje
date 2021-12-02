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

namespace Galgje
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool gevonden = false;
        char[] teRadenWoord;
        char[] geradenWoord;        
        string fouteLetters = "";
        int levens = 10;
        

        public MainWindow()
        {
            InitializeComponent();
            timer();
            txbInput.Focus();
            lblLevens.Visibility = Visibility.Hidden;
            btnRaad.Visibility = Visibility.Hidden;
            
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
                    imgGalg.Source = new BitmapImage(new Uri(@"C:\Users\Stef\OneDrive - PXL\school\Semester 1\Code\C# Essentials 2021\Galgje\Galgje\img\hangman" + levens + ".png", UriKind.RelativeOrAbsolute));

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
                geradenWoord = teRadenWoord;
                MessageBox.Show("Sorry, Je hebt veloren!");
                btnRaad.Visibility = Visibility.Hidden;

            }

            updateScherm();
            txbInput.Text = string.Empty;
            txbInput.Focus();
        }

        private void btnNieuwSpel_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void btnVerbergWoord_Click(object sender, RoutedEventArgs e)
        {

            teRadenWoord = txbInput.Text.ToLower().ToCharArray();  
            txbInput.Text = string.Empty;
            btnVerbergWoord.Visibility = Visibility.Hidden;
            geradenWoord = new char[teRadenWoord.Length];
            char[] charsToTrim = { '*', ' ', '\''};

            //bool isIntString = txbInput.Text.All(char.IsDigit);
/*
            if (isIntString == false)
            {
                MessageBox.Show("Hello");
            }
            else
            {
                MessageBox.Show("Error");
            }*/


            for (int i = 0; i < geradenWoord.Length; i++)
            {
                geradenWoord[i] = '＿';

            }
                
            btnRaad.Visibility= Visibility.Visible;
            updateScherm();

                        
        }
        private void updateScherm()
        {
            string woord = "";
            string hartjes = "";

            for(int i = 0; i < geradenWoord.Length; i++)
            {
                woord += geradenWoord[i] + " ";
            }

            for(int i = 0; i < levens; i++)
            {
                hartjes += "♥ ";
            }

            lblLevens.Visibility = Visibility.Visible;
            lblHartjes.Content = hartjes;
            lblResultaat.Content = $"{woord}\n{fouteLetters}";

            
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
                MessageBox.Show("Proficiat! Speler 2 heeft gewonnen!");
                btnRaad.Visibility = Visibility.Hidden;
                gevonden = true;
            }
            else
            {
                levens--;                                                           //Als het woord fout geraden is gaat er 1 leven af
                imgGalg.Source = new BitmapImage(new Uri(@"C:\Users\Stef\OneDrive - PXL\school\Semester 1\Code\C# Essentials 2021\Galgje\Galgje\img\hangman" + levens + ".png", UriKind.RelativeOrAbsolute));
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

       

    }
}
