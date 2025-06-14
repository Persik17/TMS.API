namespace TMS.API.Configuration
{
    public static class ConfigureLogging
    {
        /// <summary>
        /// Configures logging providers and options for the application.
        /// </summary>
        /// <param name="logging">The logging builder.</param>
        public static void Apply(ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.AddSimpleConsole(options =>
            {
                options.SingleLine = true;
                options.TimestampFormat = "HH:mm:ss ";
            });
            logging.AddDebug();
            logging.AddEventSourceLogger();
        }
    }
}
