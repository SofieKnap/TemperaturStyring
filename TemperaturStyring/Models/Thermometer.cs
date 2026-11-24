namespace TemperaturStyring.Models
{
    public class Thermometer
    {
        public int ThermometerId { get; set; }
        public double CurrentTemp { get; set; }
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        
        public const double MinAllowedTemp = 10.0;
        public const double MaxAllowedTemp = 30.0;

        public Thermometer()
        {
            
        }

        public Thermometer(int thermometerId, double currentTemperature,
            double minTemperature, double maxTemperature)
        {
            ThermometerId = thermometerId;
            CurrentTemp = currentTemperature;
            MinTemp = minTemperature;
            MaxTemp = maxTemperature;
        }

        // Validate current temperature limits
        public void ValidateTemp()
        {
            ValidateTempLimits(MinTemp, MaxTemp);
        }

        // Validate temperature limits
        private void ValidateTempLimits(double minTemperature, double maxTemperature)
        {
            if (minTemperature < MinAllowedTemp)
            {
                throw new ArgumentException("Minimum temperatur skal være mindst 10 grader.");
            }

            if (maxTemperature > MaxAllowedTemp)
            {
                throw new ArgumentException("Maximum temperatur må ikke overstige 30 grader.");
            }

            if (minTemperature >= maxTemperature)
            {
                throw new ArgumentException("Minimum temperatur skal være lavere end maximum temperatur.");
            }
        }

        // Update temperature limits
        public void UpdateTempLimits(double minTemperature, double maxTemperature)
        {
            ValidateTempLimits(minTemperature, maxTemperature);

            MinTemp = minTemperature;
            MaxTemp = maxTemperature;
        }

        // ToString
        public override string ToString()
        {
            return $"Thermometer ID: {ThermometerId}, Current Temperature: {CurrentTemp}, Min. Temperature: {MinTemp}, Max. Temperature: {MaxTemp}";
        }
    }
}
