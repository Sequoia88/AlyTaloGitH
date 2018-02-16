
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

namespace Talo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class Thermostat
    {
        public int Temperature { get; set; }
        public bool Switched { get; set; }
        public void SetTemperature(int temp)
        {
            if (temp == 0)
            {
                Switched = false;
                Temperature = 0;

            }
            else
            {
                Switched = true;
                Temperature = temp;
            }
        }

        public Thermostat()
        {
            /* This is our constructor method.
             * It takes one parameter, an integer */
            Temperature = 20; // default temperature is 20
        }
    }
    public class Sauna
    {
        public bool Switched { get; set; }
        // Create a new Thermostat for the sauna
        public Thermostat saunaThermostat = new Thermostat();

        public Sauna()
        {
            /* Constructor method for Sauna class. */
            Switched = false;
        }
    }
    public class Lights
    {
        public string Dimmer { get; set; }
        public bool Switched { get; set; }

        public Lights()
        {
            /* This is our constructor method.
             * It gets executed whenever a new instance of the class
             * gets created */
            Switched = false;
            Dimmer = "0";
        }
        public void LightSwitch()
        {
            if (Switched)
            {
                // Lights are currently on. Turn off.
                Switched = false;
                Dimmer = "0";
            }
            else
            {
                // Lights are currently off. Turn on.
                Switched = true;
                Dimmer = "100";
            }
        }
    }

    public partial class MainWindow : Window
    {
        public Lights KitchenLights = new Lights();
        public Lights LivingroomLights = new Lights();
        public Sauna SaunaRoom = new Sauna();
        public Thermostat Thermo = new Thermostat();

        // Sauna automatic setter stuff
        private DispatcherTimer temperatureSetter = new DispatcherTimer();
        private void StartSaunaTimer()
        {
            // Set timer interval to 5 seconds
            temperatureSetter.Interval = new TimeSpan(0, 0, 5);
            // Attach the temperatureSetterTick() method as a callback
            temperatureSetter.Tick += new EventHandler(temperatureSetterTick);
            // Start the timer
            temperatureSetter.Start();
        }
        private void temperatureSetterTick(object sender, EventArgs e)
        {
            // This gets called every temperatureSetter.Interval, in our case 5 seconds
            SaunaRoom.saunaThermostat.Temperature += 1;
            // Set TextBlock's text to the temperature
            saunaTextBlock.Text = SaunaRoom.saunaThermostat.Temperature.ToString();
        }
        public MainWindow()
        {
            InitializeComponent();
            TxtTemperature.Text = "21";
        }

        private void kitchenLightsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Cast the slider to an int
            int sliderValue = (int)kitchenLightsSlider.Value;
            // Set the kitchen text block to the value
            kitchenTextBlock.Text = sliderValue.ToString();
            KitchenLights.Dimmer = sliderValue.ToString();

        }

        private void kitchenLightsOnButton_Click(object sender, RoutedEventArgs e)
        {
            // Activate the lights switch
            KitchenLights.LightSwitch();

            if (KitchenLights.Switched)
            {
                // If the lights are on, enable slider
                kitchenLightsSlider.IsEnabled = true;
                kitchenLightsOnButton.Content = "Turn Lights Off";
            }
            else
            {
                // If the lights are on, disable slider
                kitchenLightsSlider.IsEnabled = false;
                kitchenLightsOnButton.Content = "Turn Lights On";
                // Set TextBlock to 0
                kitchenTextBlock.Text = "0";
                // Set slider value to 0
                kitchenLightsSlider.Value = 0;
            }

        }

        private void saunaOnButton_Click(object sender, RoutedEventArgs e)
        {
            if (!SaunaRoom.Switched)
            {
                // Start sauna temperature timer
                StartSaunaTimer();
                saunaOnButton.Content = "Turn Sauna Off";
                SaunaRoom.Switched = true;
            }
            else
            {
                temperatureSetter.Stop();
                saunaTextBlock.Text = "0";
                saunaOnButton.Content = "Turn Sauna On";
                SaunaRoom.Switched = false;
            }
        }
        private void btnTurnLightsOn_Click(object sender, RoutedEventArgs e)
        {
            // Activate the light switch
            LivingroomLights.LightSwitch();

            if (LivingroomLights.Switched)
            {
                livingroomLightsSlider.IsEnabled = true;
                btnLivingroomLightsOn.Content = "Turn Lights Off";
            }
            else
            {
                livingroomLightsSlider.IsEnabled = false;
                btnLivingroomLightsOn.Content = " Turn Lights on";
                livingRmTextBlock.Text = "0";
                livingroomLightsSlider.Value = 0;

            }

        }

        private void livingroomLightsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int sliderValue = (int)livingroomLightsSlider.Value;
            livingRmTextBlock.Text = sliderValue.ToString();
            LivingroomLights.Dimmer = sliderValue.ToString();

        }

        private void ThermostatBtn_Click(object sender, RoutedEventArgs e)
        {
            int TempSetValue;
            try
            {
                TempSetValue = Int32.Parse(TxtNewTemp.Text);
                Thermo.SetTemperature(TempSetValue);
                TxtTemperature.Text = Thermo.Temperature.ToString();
                TxtNewTemp.Text = "";

            }
            catch
            {
                TxtNewTemp.Text = "error, use another number value";

            }
        }
    }
}

