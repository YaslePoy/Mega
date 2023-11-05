using System.Diagnostics;

namespace Mega
{
    public static class TimeMeasurementService
    {
        static string name;
        static Stopwatch time;
        static TimeMeasurementService()
        {
            time = new Stopwatch();
        }
        public static void Start(string measureName)
        {
            if (name == measureName)
                return;
            if (name == null)
            {
                name = measureName;
                time.Start();
                return;
            }
            ShowTime();
            name = measureName;
            time.Restart();
        }
        static void ShowTime()
        {
            time.Stop();
            Console.WriteLine($"{name} -> {time.Elapsed}({time.Elapsed.TotalMicroseconds} us)");
        }
        public static void Stop()
        {
            ShowTime();
        }
    }
}
