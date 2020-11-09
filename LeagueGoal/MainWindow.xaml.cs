using LeagueGoal.Properties;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LeagueGoal
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool OnStartup = true;

        public MainWindow()
        {
            InitializeComponent();

            cmbLeague.SelectedIndex = Settings.Default.cmbLeagueIndex;
            cmbDivision.SelectedIndex = Settings.Default.cmbDivisionIndex;
            cmbHrLeague.SelectedIndex = Settings.Default.cmbHrLeague;
            cmbHrDivision.SelectedIndex = Settings.Default.cmbHrDivision;
            cmbLeagueGoal.SelectedIndex = Settings.Default.cmbLeagueGoalIndex;
            cmbDivisionGoal.SelectedIndex = Settings.Default.cmbDivisionGoalIndex;
            tbLP.Text = Settings.Default.tbLP.ToString();
            tbLPgain.Text = Settings.Default.tbLPgain.ToString();

            OnStartup = false;
        }

        private void cmbLeague_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default.cmbLeagueIndex = cmbLeague.SelectedIndex;
            Settings.Default.Save();

            if(!OnStartup) ComputeNeededWins();
        }

        private void cmbDivision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default.cmbDivisionIndex = cmbDivision.SelectedIndex;
            Settings.Default.Save();

            if(!OnStartup) ComputeNeededWins();
        }

        private void cmbLeagueGoal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default.cmbLeagueGoalIndex = cmbLeagueGoal.SelectedIndex;
            Settings.Default.Save();

            if(!OnStartup) ComputeNeededWins();
        }

        private void cmbDivisionGoal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default.cmbDivisionGoalIndex = cmbDivisionGoal.SelectedIndex;
            Settings.Default.Save();

            if(!OnStartup) ComputeNeededWins();
        }

        private void cmbHrLeague_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default.cmbHrLeague = cmbHrLeague.SelectedIndex;
            Settings.Default.Save();

            if(!OnStartup) ComputeNeededWins();
        }

        private void cmbHrDivision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default.cmbHrDivision = cmbHrDivision.SelectedIndex;
            Settings.Default.Save();

            if(!OnStartup) ComputeNeededWins();
        }

        private void tbLP_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.Default.tbLP = Convert.ToInt32(tbLP.Text);
            Settings.Default.Save();

            if(!OnStartup) ComputeNeededWins();
        }

        private void tbLPgain_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.Default.tbLPgain = Convert.ToInt32(tbLPgain.Text);
            Settings.Default.Save();

            if(!OnStartup) ComputeNeededWins();
        }

        private void ComputeNeededWins()
        {
            if (tbWinsNeeded == null) return;
            if (Settings.Default.tbLPgain == 0)
            {
                tbWinsNeeded.Text = "0";
                return;
            }
            if (Settings.Default.tbLP > 100)
            {
                MessageBox.Show("LP cannot be higher than 100.");
                return;
            }

            int winsNeeded = 0;

            int currentLeague = Settings.Default.cmbLeagueIndex;
            int currentDivision = Settings.Default.cmbDivisionIndex;
            int currentLp = Settings.Default.tbLP;
            int currentLpGain = Settings.Default.tbLPgain;

            int highestLeague = Settings.Default.cmbHrLeague;
            int highestDivision = Settings.Default.cmbHrDivision;

            int goalLeague = Settings.Default.cmbLeagueGoalIndex;
            int goalDivision = Settings.Default.cmbDivisionGoalIndex;

            while(currentLeague < goalLeague || (currentLeague == goalLeague && currentDivision < goalDivision))
            {
                while (currentLp < 100)
                {
                    winsNeeded++;
                    currentLp += currentLpGain;
                }

                if (currentDivision == 3)
                {
                    winsNeeded += 3;
                    currentLeague++;
                    currentDivision = 0;
                }
                else
                {
                    if (highestLeague > currentLeague || (highestLeague == currentLeague && highestDivision > currentDivision))
                    {
                        winsNeeded += 1;
                    }
                    else
                    {
                        winsNeeded += 2;
                    }
                    currentDivision++;
                    currentLp = 0;
                }
            }

            tbWinsNeeded.Text = winsNeeded.ToString();
        }
    }
}
