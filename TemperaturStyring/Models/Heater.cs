

namespace TemperaturStyring.Models
{
    public class Heater
    {
        public double CurrentTemp { get; set; }
        public Window WindowStatus { get; private set; }
        public bool HeaterStatus { get; private set; }
        public int HeaterId { get; set; }

        public double GetTemp()
        {
            return CurrentTemp;
        }

        public void TurnOnHeater()
        {
            HeaterStatus = true;
        }

        public void TurnOffHeater()
        {
            HeaterStatus = false;
        }


    }
}
