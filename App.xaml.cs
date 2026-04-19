using System.Windows;

namespace BackupManagerPro
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize Velopack for automatic updates
            try
            {
                UpdateManager.Instance.Initialize();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Update initialization error: {ex.Message}");
                // Don't crash the app if updates fail to initialize
            }
        }
    }
}