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

            if (!OnStartup) ComputeNeededWins();
        }

        private void cmbDivision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default.cmbDivisionIndex = cmbDivision.SelectedIndex;
            Settings.Default.Save();

            if (!OnStartup) ComputeNeededWins();
        }

        private void cmbLeagueGoal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default.cmbLeagueGoalIndex = cmbLeagueGoal.SelectedIndex;
            Settings.Default.Save();

            if (!OnStartup) ComputeNeededWins();
        }

        private void cmbDivisionGoal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default.cmbDivisionGoalIndex = cmbDivisionGoal.SelectedIndex;
            Settings.Default.Save();

            if (!OnStartup) ComputeNeededWins();
        }

        private void cmbHrLeague_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default.cmbHrLeague = cmbHrLeague.SelectedIndex;
            Settings.Default.Save();

            if (!OnStartup) ComputeNeededWins();
        }

        private void cmbHrDivision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default.cmbHrDivision = cmbHrDivision.SelectedIndex;
            Settings.Default.Save();

            if (!OnStartup) ComputeNeededWins();
        }

        private void tbLP_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Settings.Default.tbLP = Convert.ToInt32(tbLP.Text);
                Settings.Default.Save();

                if (!OnStartup) ComputeNeededWins();
            }
            catch (FormatException) { }
        }

        private void tbLPgain_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Settings.Default.tbLPgain = Convert.ToInt32(tbLPgain.Text);
                Settings.Default.Save();

                if (!OnStartup) ComputeNeededWins();
            }
            catch (FormatException) { }
        }

        private void tbLPGoal_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Settings.Default.tbLPGoal = Convert.ToInt32(tbLPGoal.Text);
                Settings.Default.Save();

                if (!OnStartup) ComputeNeededWins();
            }
            catch (FormatException) { }
        }

        private void tbWinrate_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Settings.Default.tbWinrate = Convert.ToSingle(tbWinrate.Text);
                Settings.Default.Save();

                if (!OnStartup) ComputeNeededWins();
            }
            catch (FormatException) { }
        }

        private void ComputeNeededWins()
        {
            //  Error handling
            if (tbWinsNeeded == null) return;
            if (Convert.ToInt32(Settings.Default.tbLPgain) == 0)
            {
                tbWinsNeeded.Text = "0";
                return;
            }
            if (Convert.ToInt32(Settings.Default.tbLP) > 100)
            {
                MessageBox.Show("LP cannot be higher than 100.");
                return;
            }

            //  Initialize variables
            int winsNeeded = 0;

            int currentLeague = Settings.Default.cmbLeagueIndex;
            int currentDivision = Settings.Default.cmbDivisionIndex;
            int currentLp = Convert.ToInt32(Settings.Default.tbLP);
            int currentLpGain = Convert.ToInt32(Settings.Default.tbLPgain);

            int highestLeague = Settings.Default.cmbHrLeague;
            int highestDivision = Settings.Default.cmbHrDivision;

            int goalLeague = Settings.Default.cmbLeagueGoalIndex;
            int goalDivision = Settings.Default.cmbDivisionGoalIndex;
            int goalLp = Settings.Default.tbLPGoal;

            float winrate = Settings.Default.tbWinrate;

            //  Compute wins from current rank to goal rank
            while (currentLeague < goalLeague || (currentLeague == goalLeague && currentDivision < goalDivision))
            {
                //  Compute wins from LP difference
                while (currentLp < 100)
                {
                    winsNeeded++;
                    currentLp += currentLpGain;
                }

                //  Check for league/division promotion
                if (currentDivision == 3)
                {
                    //  Promote
                    winsNeeded += 3;
                    currentLeague++;
                    currentDivision = 0;
                }
                else
                {
                    //  Check if free win is granted
                    if (highestLeague > currentLeague || (highestLeague == currentLeague && highestDivision > currentDivision))
                    {
                        winsNeeded += 1;
                    }
                    else
                    {
                        winsNeeded += 2;
                    }
                    currentDivision++;
                }
                currentLp = 0;
            }

            //  Compute wins from LP difference
            if (currentLp < goalLp)
            {
                int wins = (goalLp - currentLp) / currentLpGain;
                if ((goalLp - currentLp) % currentLpGain > 0) wins++;
                winsNeeded += wins;
            }

            //  Apply winrate
            winsNeeded = (int)(winsNeeded / winrate * 100);

            tbWinsNeeded.Text = winsNeeded.ToString();
        }
    }
}
