namespace TemperaturStyring.Models
{
    public class Window
    {
        public int WindowId { get; set; }
        public bool WindowStatus { get; private set; }
        public Heater HeaterStatus { get; private set; }
        public bool IsItRaining { get; set; }
        //public Co2 Co2 { get; set; }
        public double CurrentTemp { get; set; }
        public double OutsideTemp { get; set; }

        public void OpenWindow()
        {
            WindowStatus = true;
            HeaterStatus?.TurnOffHeater();
        }

        public void CloseWindow()
        {
            WindowStatus = false;
            HeaterStatus?.TurnOnHeater();
        }
    }
}
