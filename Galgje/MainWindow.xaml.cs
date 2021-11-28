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

            for(int i = 0; i < geradenWoord.Length; i++)
            {
                geradenWoord[i] = '＿';

            }

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

            lblLevens.Content = hartjes;
            lblResultaat.Content = $"{woord}\n{fouteLetters}";

            
        }
               
        private void raadLetter(char letter) 
        {
            for (int i = 0; i < teRadenWoord.Length; i++)
            {
                if(letter.Equals(teRadenWoord[i]))
                {
                    
                    geradenWoord[i] = letter;
                    gevonden = true;
                }
                
            }
        }

        private void raadWoord(char[] gebruikersInput)
        {
            if (gebruikersInput.SequenceEqual(teRadenWoord))
            {
                geradenWoord = teRadenWoord;
                MessageBox.Show("Proficiat! Speler 2 heeft gewonnen!");
                btnRaad.Visibility = Visibility.Hidden;
                gevonden = true;
            }
            else
            {
                levens--;
            }
        }




    }
}
