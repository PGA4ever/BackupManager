using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using System;
using Task = System.Threading.Tasks.Task;  // Alias pentru Task (nu cel din TaskScheduler)
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;


namespace BackupManagerPro
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private List<GameConfig> _games = new();
        private readonly string ConfigPath = "backup_manager.json";
        private readonly string LogPath = "backup_manager.log";
        private string _defaultBackupDir = @"D:\Backup";
        private bool _notificationsEnabled = true;
        private bool _startupEnabled = false;

        // Auto-backup fields
        private DispatcherTimer _autoBackupTimer = new();
        private bool _autoBackupEnabled = false;
        private string _autoBackupInterval = "1 ora";
        private DateTime _lastBackupTime = DateTime.MinValue;

        public bool NotificationsEnabled
        {
            get => _notificationsEnabled;
            set { _notificationsEnabled = value; OnPropertyChanged(); }
        }

        public string DefaultBackupDir
        {
            get => _defaultBackupDir;
            set { _defaultBackupDir = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadConfig();
            RefreshGamesList();
            InitializeAutoBackup();
            InitializeStartup();
            InitializeVersionDisplay();

            // TRAY ICON
            SetupTrayIcon();

            // Essential events
            StateChanged += MainWindow_StateChanged;
            Closing += MainWindow_Closing;
        }

        private void InitializeVersionDisplay()
        {
            var version = UpdateManager.Instance.CurrentVersion;
            CurrentVersionText.Text = version;
        }

        private void Log(string message)
        {
            string normalized = NormalizeText(message);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logLine = $"{timestamp} - INFO - {normalized}";
            File.AppendAllText(LogPath, logLine + Environment.NewLine);
            Dispatcher.Invoke(() => LogList.Items.Add(logLine));
        }

        private string NormalizeText(string text)
        {
            return text.Replace("ș", "s").Replace("ț", "t").Replace("ă", "a").Replace("â", "a").Replace("î", "i")
                       .Replace("Ș", "S").Replace("Ț", "T").Replace("Ă", "A").Replace("Â", "A").Replace("Î", "I");
        }

        private void LoadConfig()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    var json = File.ReadAllText(ConfigPath);
                    var config = JsonConvert.DeserializeObject<Config>(json) ?? new Config();
                    _games = config.Games ?? new List<GameConfig>();
                    NotificationsEnabled = config.Notifications;
                    DefaultBackupDir = config.DefaultBackupDir ?? DefaultBackupDir;
                    _autoBackupEnabled = config.AutoBackupEnabled;
                    _autoBackupInterval = config.AutoBackupInterval ?? "1 hour";
                    _lastBackupTime = config.LastBackupTime;
                    _startupEnabled = config.StartupEnabled;
                    Log($"Configuration loaded: {_games.Count} games");
                }
                else
                {
                    Log($"Configuration file not found: {Path.GetFullPath(ConfigPath)}");
                }
            }
            catch (Exception ex)
            {
                Log($"Error loading configuration: {ex.Message}");
            }
        }

        private void SaveConfig()
        {
            var config = new Config
            {
                Games = _games,
                Notifications = NotificationsEnabled,
                DefaultBackupDir = DefaultBackupDir,
                AutoBackupEnabled = _autoBackupEnabled,
                AutoBackupInterval = _autoBackupInterval,
                LastBackupTime = _lastBackupTime,
                StartupEnabled = _startupEnabled
            };
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(config, Formatting.Indented));
        }

        private void InitializeAutoBackup()
        {
            // Set up the timer
            _autoBackupTimer.Interval = TimeSpan.FromMinutes(1); // Check every minute
            _autoBackupTimer.Tick += AutoBackupTimer_Tick;

            // Update UI
            Dispatcher.Invoke(() =>
            {
                AutoBackupEnabled.IsChecked = _autoBackupEnabled;
                if (!string.IsNullOrEmpty(_autoBackupInterval))
                {
                    var index = AutoBackupInterval.Items.IndexOf(_autoBackupInterval);
                    if (index >= 0)
                        AutoBackupInterval.SelectedIndex = index;
                }
                UpdateAutoBackupUI();

                // Check if backup is due on startup
                if (_autoBackupEnabled && IsBackupDue())
                {
                    Log("Auto-backup due at startup!");
                    PerformAutoBackup();
                }
                else if (_autoBackupEnabled)
                {
                    _autoBackupTimer.Start();
                }

                // Initialize startup checkbox
                StartupCheck.IsChecked = _startupEnabled;
            });
        }

        private void InitializeStartup()
        {
            // Check if the app is already in registry
            _startupEnabled = IsAppInStartup();
            StartupCheck.IsChecked = _startupEnabled;
        }

        private bool IsAppInStartup()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false);
                return key?.GetValue("BackupManagerPro") != null;
            }
            catch
            {
                return false;
            }
        }

        private void SetAppStartup(bool enable)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                if (enable)
                {
                    string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    key?.SetValue("BackupManagerPro", appPath);
                    Log("Application added to Windows startup!");
                }
                else
                {
                    key?.DeleteValue("BackupManagerPro", false);
                    Log("Application removed from Windows startup!");
                }
            }
            catch (Exception ex)
            {
                Log($"Error configuring startup: {ex.Message}");
                MessageBox.Show($"Error configuring Windows startup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StartupCheck_Checked(object sender, RoutedEventArgs e)
        {
            _startupEnabled = true;
            SetAppStartup(true);
            SaveConfig();
        }

        private void StartupCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            _startupEnabled = false;
            SetAppStartup(false);
            SaveConfig();
        }

        private void AutoBackupTimer_Tick(object? sender, EventArgs e)
        {
            if (_autoBackupEnabled && IsBackupDue())
            {
                _autoBackupTimer.Stop();
                PerformAutoBackup();
                _autoBackupTimer.Start();
            }
        }

        private bool IsBackupDue()
        {
            TimeSpan interval = GetIntervalTimeSpan(_autoBackupInterval);
            return DateTime.Now >= _lastBackupTime.Add(interval);
        }

        private TimeSpan GetIntervalTimeSpan(string interval)
        {
            return interval switch
            {
                "30 minutes" => TimeSpan.FromMinutes(30),
                "1 hour" => TimeSpan.FromHours(1),
                "2 hours" => TimeSpan.FromHours(2),
                "6 hours" => TimeSpan.FromHours(6),
                "24 hours" => TimeSpan.FromHours(24),
                "2 days" => TimeSpan.FromDays(2),
                "7 days" => TimeSpan.FromDays(7),
                _ => TimeSpan.FromHours(1)
            };
        }

        private async void PerformAutoBackup()
        {
            var selected = _games.Where(g => g.IsSelected).ToList();
            if (!selected.Any())
            {
                Log("Auto-backup: no games selected!");
                return;
            }

            Log("Auto-backup started...");
            Dispatcher.Invoke(() => { ProgressBar.Value = 0; StatusText.Text = "Auto-backup in progress..."; });

            int total = selected.Count;
            int done = 0;

            foreach (var game in selected)
            {
                await Task.Run(() => CreateBackup(game));
                done++;
                Dispatcher.Invoke(() => { ProgressBar.Value = (done * 100.0) / total; });
            }

            _lastBackupTime = DateTime.Now;
            SaveConfig();
            Dispatcher.Invoke(() =>
            {
                ProgressBar.Value = 100;
                StatusText.Text = "Auto-backup completed!";
                UpdateAutoBackupUI();
            });

            if (NotificationsEnabled)
                TrayIcon.ShowBalloonTip("Auto-Backup Completed", $"{done} games backed up.", BalloonIcon.Info);
        }

        private void UpdateAutoBackupUI()
        {
            Dispatcher.Invoke(() =>
            {
                if (_autoBackupEnabled)
                {
                    AutoBackupStatus.Text = $"Active - Interval: {_autoBackupInterval}";
                    AutoBackupStatus.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkGreen);
                }
                else
                {
                    AutoBackupStatus.Text = "Disabled";
                    AutoBackupStatus.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
                }

                if (_lastBackupTime == DateTime.MinValue)
                    LastBackupTime.Text = "Never";
                else
                    LastBackupTime.Text = _lastBackupTime.ToString("yyyy-MM-dd HH:mm:ss");
            });
        }

        private void AutoBackupEnabled_Checked(object sender, RoutedEventArgs e)
        {
            _autoBackupEnabled = true;
            _lastBackupTime = DateTime.Now; // Start the interval from now
            SaveConfig();
            UpdateAutoBackupUI();
            _autoBackupTimer.Start();
            Log("Auto-backup enabled!");
        }

        private void AutoBackupEnabled_Unchecked(object sender, RoutedEventArgs e)
        {
            _autoBackupEnabled = false;
            _autoBackupTimer.Stop();
            SaveConfig();
            UpdateAutoBackupUI();
            Log("Auto-backup disabled!");
        }

        private void AutoBackupInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AutoBackupInterval.SelectedItem is ComboBoxItem item && item.Content is string interval)
            {
                _autoBackupInterval = interval;
                SaveConfig();
                UpdateAutoBackupUI();
                Log($"Auto-backup interval changed to: {interval}");
            }
        }

        private void RefreshGamesList()
        {
            foreach (var game in _games)
            {
                game.SizeMB = $"{GetFolderSize(game.BackupPath):F2} MB";
                game.BackupCount = Directory.Exists(game.BackupPath) ? Directory.GetFiles(game.BackupPath, "*.zip").Length : 0;
            }
            GamesGrid.ItemsSource = null;
            GamesGrid.ItemsSource = _games;
        }

        private double GetFolderSize(string path)
        {
            if (!Directory.Exists(path)) return 0;
            return Directory.GetFiles(path, "*", SearchOption.AllDirectories)
                .Sum(f => new FileInfo(f).Length) / (1024.0 * 1024.0);
        }

        private void SelectSource_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                SourceDirEntry.Text = dialog.FileName;
        }

        private void SelectBackup_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                BackupDirEntry.Text = dialog.FileName;
        }

        private void SelectDefaultBackup_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                DefaultBackupDir = dialog.FileName;
        }

        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            string name = GameNameEntry.Text.Trim();
            string source = SourceDirEntry.Text.Trim();
            string backup = string.IsNullOrEmpty(BackupDirEntry.Text.Trim()) ? DefaultBackupDir : BackupDirEntry.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(source))
            {
                MessageBox.Show("Game name and source path are required!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Directory.Exists(source))
            {
                MessageBox.Show("Source path does not exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var game = new GameConfig { Name = name, Source = source, Backup = backup, IsSelected = true };
            _games.Add(game);
            SaveConfig();
            RefreshGamesList();
            Log($"Game added: {name}");
            GameNameEntry.Text = "";
            SourceDirEntry.Text = "";
            BackupDirEntry.Text = DefaultBackupDir;
        }
            private async void ManualBackup_Click(object sender, RoutedEventArgs e)
        {
            var selected = _games.Where(g => g.IsSelected).ToList();
            if (!selected.Any())
            {
                MessageBox.Show("No games selected!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ProgressBar.Value = 0;
            StatusText.Text = "Backup starting...";
            int total = selected.Count;
            int done = 0;

            foreach (var game in selected)
            {
                await Task.Run(() => CreateBackup(game));
                done++;
                ProgressBar.Value = (done * 100.0) / total;
            }

            StatusText.Text = "Backup completed!";
            if (NotificationsEnabled)
                TrayIcon.ShowBalloonTip("Backup Completed", $"{done} games backed up.", BalloonIcon.Info);
        }

        private bool HasFilesChanged(GameConfig game)
        {
            string backupDir = game.BackupPath;
            if (!Directory.Exists(backupDir)) return true;

            var zips = Directory.GetFiles(backupDir, "*.zip");
            if (!zips.Any()) return true;

            string latest = zips.OrderByDescending(f => File.GetLastWriteTime(f)).First();
            DateTime latestTime = File.GetLastWriteTime(latest);

            foreach (string file in Directory.GetFiles(game.Source, "*", SearchOption.AllDirectories))
            {
                if (File.GetLastWriteTime(file) > latestTime) return true;
            }

            try
            {
                string temp = Path.Combine(backupDir, "temp_check");
                Directory.CreateDirectory(temp);
                ZipFile.ExtractToDirectory(latest, temp);

                foreach (string file in Directory.GetFiles(game.Source, "*", SearchOption.AllDirectories))
                {
                    string rel = Path.GetRelativePath(game.Source, file);
                    string extracted = Path.Combine(temp, Path.GetFileName(game.Source), rel);
                    if (!File.Exists(extracted) || GetFileHash(file) != GetFileHash(extracted))
                    {
                        Directory.Delete(temp, true);
                        return true;
                    }
                }
                Directory.Delete(temp, true);
            }
            catch { return true; }

            return false;
        }

        private string? GetFileHash(string path)
        {
            try
            {
                using var md5 = MD5.Create();
                using var stream = File.OpenRead(path);
                return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");
            }
            catch { return null; }
        }

        private void CreateBackup(GameConfig game)
        {
            try
            {
                if (!HasFilesChanged(game))
                {
                    Log($"No files changed for {game.Name}, backup skipped.");
                    return;
                }

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string zipPath = Path.Combine(game.BackupPath, $"backup_{timestamp}.zip");
                Directory.CreateDirectory(game.BackupPath);

                using var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create);
                foreach (string file in Directory.GetFiles(game.Source, "*", SearchOption.AllDirectories))
                {
                    string rel = Path.Combine(Path.GetFileName(game.Source), Path.GetRelativePath(game.Source, file));
                    try { zip.CreateEntryFromFile(file, rel); }
                    catch (Exception ex) { Log($"File skipped: {file} - {ex.Message}"); }
                }

                Log($"Backup created: {game.Name} → {zipPath}");
            }
            catch (Exception ex)
            {
                Log($"Backup error {game.Name}: {ex.Message}");
            }
        }

        private void RestoreBackup_Click(object sender, RoutedEventArgs e)
        {
            var selected = GamesGrid.SelectedItems.Cast<GameConfig>().ToList();
            if (selected.Count != 1)
            {
                MessageBox.Show("Select exactly one game!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var game = selected[0];
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = false,
                InitialDirectory = game.BackupPath,
                Filters = { new CommonFileDialogFilter("ZIP Files", "*.zip") }
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (MessageBox.Show($"All files in {game.Source} will be deleted!\nContinue?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    foreach (string file in Directory.GetFiles(game.Source, "*", SearchOption.AllDirectories))
                        File.Delete(file);
                    foreach (string dir in Directory.GetDirectories(game.Source, "*", SearchOption.AllDirectories).OrderByDescending(d => d))
                        Directory.Delete(dir);
                    ZipFile.ExtractToDirectory(dialog.FileName, Path.GetDirectoryName(game.Source)!);
                    Log($"Restored: {game.Name}");
                }
            }
        }

        private void DeleteGame_Click(object sender, RoutedEventArgs e)
        {
            var selected = GamesGrid.SelectedItems.Cast<GameConfig>().ToList();
            if (!selected.Any()) return;

            if (MessageBox.Show("Delete selected games from list?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (var g in selected) _games.Remove(g);
                SaveConfig();
                RefreshGamesList();
            }
        }

        private void OpenBackupFolder_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var contextMenu = menuItem?.Parent as ContextMenu;
            var dataGrid = contextMenu?.PlacementTarget as DataGrid;
            var game = dataGrid?.SelectedItem as GameConfig;
            if (game != null && Directory.Exists(game.BackupPath))
                Process.Start("explorer.exe", game.BackupPath);
        }

        private void CopyText_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var contextMenu = menuItem?.Parent as ContextMenu;
            var dataGrid = contextMenu?.PlacementTarget as DataGrid;
            if (dataGrid?.CurrentCell.Item is GameConfig game)
            {
                var column = dataGrid.CurrentCell.Column as DataGridTextColumn;
                var value = column?.GetCellContent(game)?.ToString() ?? "";
                Clipboard.SetText(value);
            }
        }

        private void GamesGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Save config whenever a cell is edited (especially the IsSelected checkbox)
            SaveConfig();
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            SaveConfig();
            MessageBox.Show("Settings saved!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OpenLog_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(LogPath)) Process.Start("notepad.exe", LogPath);
        }

        private void ClearLog_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Clear entire log?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                File.WriteAllText(LogPath, "");
                LogList.Items.Clear();
                Log("Log cleared.");
            }
        }

        private async void CheckForUpdates_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
                button.IsEnabled = false;

            try
            {
                UpdateStatus.Text = "Checking for updates...";
                UpdateStatus.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);

                bool updateAvailable = await UpdateManager.Instance.CheckForUpdatesAsync();

                if (updateAvailable)
                {
                    UpdateStatus.Text = "Update available! Click 'Check for Updates' to install.";
                    UpdateStatus.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkOrange);

                    // Offer to install immediately
                    if (MessageBox.Show("An update is available. Would you like to download and install it now?", 
                        "Update Available", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        await UpdateManager.Instance.DownloadAndInstallUpdatesAsync();
                    }
                }
                else
                {
                    UpdateStatus.Text = "You are using the latest version.";
                    UpdateStatus.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkGreen);
                }

                Log("Update check completed.");
            }
            catch (Exception ex)
            {
                UpdateStatus.Text = $"Error: {ex.Message}";
                UpdateStatus.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                Log($"Update check error: {ex.Message}");
            }
            finally
            {
                if (button != null)
                    button.IsEnabled = true;
            }
        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            // Save configuration when closing
            SaveConfig();
            // Allow the application to close
            e.Cancel = false;
        }
        private void MainWindow_StateChanged(object? sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                // Hide completely from taskbar and Alt+Tab
                Hide();
                ShowInTaskbar = false;

                if (NotificationsEnabled)
                    TrayIcon.ShowBalloonTip("Backup Manager Pro", "Running in background", BalloonIcon.Info);
            }
        }
        private void SetupTrayIcon()
        {
            // Double-click = restore window
            var openCommand = new RelayCommand(() =>
            {
                Show();
                WindowState = WindowState.Normal;
                ShowInTaskbar = true;
                Activate();
                BringIntoView();
            });

            TrayIcon.DoubleClickCommand = openCommand;

            // Right-click menu
            var menu = new ContextMenu();
            menu.Items.Add(new MenuItem
            {
                Header = "Open Application",
                Command = openCommand
            });
            menu.Items.Add(new Separator());
            menu.Items.Add(new MenuItem
            {
                Header = "Exit",
                Command = new RelayCommand(() => Application.Current.Shutdown())
            });

            TrayIcon.ContextMenu = menu;
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class GameConfig : INotifyPropertyChanged
    {
        public string Name { get; set; } = "";
        public string Source { get; set; } = "";
        public string Backup { get; set; } = "";
        
        private bool _isSelected = true;
        public bool IsSelected 
        { 
            get => _isSelected; 
            set { _isSelected = value; OnPropertyChanged(); } 
        }
        
        public string BackupPath => Path.Combine(Backup, Name);
        private string _sizeMB = "0 MB";
        public string SizeMB { get => _sizeMB; set { _sizeMB = value; OnPropertyChanged(); } }
        private int _backupCount;
        public int BackupCount { get => _backupCount; set { _backupCount = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class Config
    {
        public List<GameConfig>? Games { get; set; }
        public bool Notifications { get; set; } = true;
        public string? DefaultBackupDir { get; set; }
        public bool AutoBackupEnabled { get; set; } = false;
        public string? AutoBackupInterval { get; set; } = "1 ora";
        public DateTime LastBackupTime { get; set; } = DateTime.MinValue;
        public bool StartupEnabled { get; set; } = false;
    }
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        public RelayCommand(Action execute) => _execute = execute ?? throw new ArgumentNullException(nameof(execute));

        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter) => true;
        public void Execute(object? parameter) => _execute();  // AICI: _execute() – nu _execute
    }
}