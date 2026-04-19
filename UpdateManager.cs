using System;
using System.Threading.Tasks;
using Velopack;
using System.Windows;

namespace BackupManagerPro
{
    public class UpdateManager
    {
        private static UpdateManager? _instance;
        private string _githubUrl = "https://github.com/yourusername/BackupManager"; // Replace with your GitHub repo URL
        private string _releasesUrl = "https://github.com/yourusername/BackupManager/releases/download"; // Replace with your releases URL

        public static UpdateManager Instance => _instance ??= new UpdateManager();

        public string CurrentVersion => GetVersion();

        public async Task<bool> CheckForUpdatesAsync()
        {
            try
            {
                // You need to configure Velopack with your GitHub feed URL
                // This would typically be done in App.xaml.cs or during startup
                // For now, we'll show a notification about setting up GitHub

                MessageBox.Show(
                    "To enable automatic updates, you need to:\n\n" +
                    "1. Create a GitHub repository\n" +
                    "2. Set up Velopack releases\n" +
                    "3. Configure the update feed URL\n\n" +
                    "GitHub URL: " + _githubUrl,
                    "Update Configuration",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return false;
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
                    "Update feature will be available once you configure your GitHub repository.\n\n" +
                    "Steps to enable updates:\n" +
                    "1. Push your app to GitHub\n" +
                    "2. Create releases with your built packages\n" +
                    "3. Configure Velopack feed URL",
                    "Update Setup Required",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error with updates: {ex.Message}", "Update Failed", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetVersion()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            return version?.ToString() ?? "1.0.0.0";
        }

        public void SetGitHubRepository(string owner, string repo)
        {
            _githubUrl = $"https://github.com/{owner}/{repo}";
            _releasesUrl = $"https://github.com/{owner}/{repo}/releases/download";
        }

        public string GitHubUrl => _githubUrl;
    }
}


