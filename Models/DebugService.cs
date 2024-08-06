using System.Diagnostics;
using System.Globalization;

namespace VisualLog.Models
{
    public static class DebugService
    {
        private static int DebugLogNumber { get; set; } = 0;

        public static void Log(string message)
        {
            if (ApplicationSettings.DebugMode)
            {
                DateTime localTime = DateTime.Now;
                var culture = new CultureInfo("en-US");
                DebugLogNumber += 1;
                string debugLogNumber = DebugLogNumber.ToString("D3");

                Debug.WriteLine($"{localTime.ToString(culture)} |{debugLogNumber}| {message}");
            }
            return;
        }
    }
}
