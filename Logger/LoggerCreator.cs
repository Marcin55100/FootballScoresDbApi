using Serilog;
using System.Reflection;

namespace FootballScoresDbApi.Logger
{
    public class LoggerCreator
    {
        private const int MAX_LOG_FILES = 50;
        private const int FILE_SIZE_BYTES_LIMIT = 20_000_000;

        private const string LOG_FILE_NAME = "footballAPI.log";
        private const string OUTPUT_TEMPLATE = "{Timestamp:yyMMdd HH:mm:ss.fff} [{ThreadId,3}] {Level:u3} {Message:lj}{NewLine}{Exception}";

        public static void CreateLogger()
        {
            var filePath = GetFilePath();
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo
                    .Console()
                    .WriteTo
                    .File(filePath, rollOnFileSizeLimit: true, retainedFileCountLimit: MAX_LOG_FILES, fileSizeLimitBytes: FILE_SIZE_BYTES_LIMIT, outputTemplate: OUTPUT_TEMPLATE).CreateLogger();
                Log.Logger.Information($"[{nameof(LoggerCreator)}] [{nameof(CreateLogger)}] Succeded");
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }

        private static string GetFilePath()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + Path.DirectorySeparatorChar + $"{LOG_FILE_NAME}";
        }
    }
}
