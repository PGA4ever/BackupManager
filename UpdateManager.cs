using System;
using System.Threading.Tasks;
using System.Windows;

namespace BackupManagerPro
{
    public class UpdateManager
    {
        private static UpdateManager? _instance;
        private string _githubUrl = "https://github.com/PGA4ever/`";
        private string _feedUrl = "https://github.com/PGA4ever/BackupManager/releases";

        public static UpdateManager Instance => _instance ??= new UpdateManager();

        public string CurrentVersion => GetVersion();

        public void Initialize()
        {
            try
            {
                // Velopack initialization is handled here
                // In a production environment, you would configure Velopack to check for updates
                System.Diagnostics.Debug.WriteLine("Update system initialized. Feed URL: " + _feedUrl);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Update initialization error: {ex.Message}");
            }
        }

        public async Task<bool> CheckForUpdatesAsync()
        {
            try
            {
                MessageBox.Show(
                    $"Current Version: {CurrentVersion}\n\n" +
                    "Checking GitHub releases...\n\n" +
                    "GitHub Repository:\n" + _githubUrl + "\n\n" +
                    "For automatic updates, ensure:\n" +
                    "1. Release tags follow format: v1.0.0\n" +
                    "2. Release contains BackupManager.exe\n" +
                    "3. All dependencies are included\n\n" +
                    "Click OK to visit the releases page.",
                    "Check for Updates",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Information);

                // Open GitHub releases page in browser
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = _feedUrl,
                        UseShellExecute = true
                    });
                }
                catch { }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for updates: {ex.Message}", "Update Check Failed", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task DownloadAndInstallUpdatesAsync()
        {
            try
            {
                MessageBox.Show(
                    "Update system is configured to work with GitHub releases.\n\n" +
                    "Repository: " + _githubUrl + "\n\n" +
                    "To create a new release:\n\n" +
                    "1. Build Release version:\n" +
                    "   dotnet publish -c Release\n\n" +
                    "2. Create a new release on GitHub\n" +
                    "3. Tag it with version (v1.0.1, v1.0.2, etc)\n" +
                    "4. Upload the exe files\n\n" +
                    "See VELOPACK_SETUP.md for detailed instructions.",
                    "Update Management",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Update Failed", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetVersion()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            return version?.ToString(3) ?? "1.0.0";
        }

        public void SetGitHubRepository(string owner, string repo)
        {
            _githubUrl = $"https://github.com/{owner}/{repo}";
            _feedUrl = $"https://github.com/{owner}/{repo}/releases";
        }

        public string GitHubUrl => _githubUrl;
        public string FeedUrl => _feedUrl;
    }
}



