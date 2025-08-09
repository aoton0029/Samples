using CoreLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileSample
{
    /// <summary>
    /// アプリケーション設定データのモデル
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// アプリケーション名
        /// </summary>
        public string ApplicationName { get; set; } = "";

        /// <summary>
        /// ユーザー設定
        /// </summary>
        public UserSettings User { get; set; } = new();

        /// <summary>
        /// UI設定
        /// </summary>
        public UiSettings Ui { get; set; } = new();

        /// <summary>
        /// データベース設定
        /// </summary>
        public DatabaseSettings Database { get; set; } = new();

        /// <summary>
        /// カスタム設定（キー・値ペア）
        /// </summary>
        public Dictionary<string, object> CustomSettings { get; set; } = new();

        /// <summary>
        /// 設定のバージョン
        /// </summary>
        public string SettingsVersion { get; set; } = "1.0";

        /// <summary>
        /// 最終更新日時
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// ユーザー設定
    /// </summary>
    public class UserSettings
    {
        public string UserId { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Language { get; set; } = "ja-JP";
        public string Theme { get; set; } = "Default";
        public bool AutoSave { get; set; } = true;
        public TimeSpan AutoSaveInterval { get; set; } = TimeSpan.FromMinutes(5);
    }

    /// <summary>
    /// UI設定
    /// </summary>
    public class UiSettings
    {
        public int WindowWidth { get; set; } = 1024;
        public int WindowHeight { get; set; } = 768;
        public int WindowX { get; set; } = 100;
        public int WindowY { get; set; } = 100;
        public bool WindowMaximized { get; set; } = false;
        public string FontFamily { get; set; } = "Yu Gothic UI";
        public int FontSize { get; set; } = 12;
        public List<string> RecentFiles { get; set; } = new();
    }

    /// <summary>
    /// データベース設定
    /// </summary>
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = "";
        public int CommandTimeout { get; set; } = 30;
        public bool EnableLogging { get; set; } = false;
        public string BackupDirectory { get; set; } = "";
    }

    /// <summary>
    /// 設定マネージャーの操作結果
    /// </summary>
    public class SettingsOperationResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Version { get; set; }
        public bool HasConflict { get; set; }
        public FileConflictInfo? ConflictInfo { get; set; }
    }

    public class SettingsManager<TSettings> : IDisposable where TSettings : class, new()
    {
        private readonly FileService _fileService;
        private readonly string _settingsFileName;
        private readonly string _userId;
        private TSettings? _cachedSettings;
        private readonly JsonSerializerOptions _jsonOptions;

        public event EventHandler<TSettings>? SettingsChanged;
        public event EventHandler<FileConflictInfo>? ConflictDetected;

        public SettingsManager(string baseDirectory, string userId, string settingsFileName = "app-settings.json")
        {
            _fileService = new FileService(baseDirectory);
            _settingsFileName = settingsFileName;
            _userId = userId;

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter() }
            };
        }

        public async Task<(SettingsOperationResult Result, TSettings? Settings)> LoadSettingsAsync()
        {
            var result = new SettingsOperationResult();

            try
            {
                var (fileResult, content) = await _fileService.ReadFileAsync(
                    _settingsFileName,
                    new FileOperationOptions { UserId = _userId });

                if (!fileResult.Success)
                {
                    if (fileResult.ErrorMessage?.Contains("存在しません") == true)
                    {
                        var defaultSettings = CreateDefaultSettings();
                        var saveResult = await SaveSettingsAsync(defaultSettings);

                        result.Success = saveResult.Success;
                        result.ErrorMessage = saveResult.ErrorMessage;
                        result.Version = saveResult.Version;

                        return (result, defaultSettings);
                    }

                    result.Success = false;
                    result.ErrorMessage = fileResult.ErrorMessage;
                    return (result, null);
                }

                var settings = JsonSerializer.Deserialize<TSettings>(content!, _jsonOptions);
                if (settings != null)
                {
                    _cachedSettings = settings;
                    result.Success = true;
                    result.Version = fileResult.Version;
                    return (result, settings);
                }

                result.Success = false;
                result.ErrorMessage = "設定ファイルの形式が正しくありません";
                return (result, null);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"設定読み込みエラー: {ex.Message}";
                return (result, null);
            }
        }

        public async Task<SettingsOperationResult> SaveSettingsAsync(
            TSettings settings,
            ConflictResolutionStrategy strategy = ConflictResolutionStrategy.Abort)
        {
            var result = new SettingsOperationResult();

            try
            {
                // LastUpdated プロパティが存在する場合のみ更新
                var lastUpdatedProp = typeof(TSettings).GetProperty("LastUpdated");
                if (lastUpdatedProp != null && lastUpdatedProp.PropertyType == typeof(DateTime))
                {
                    lastUpdatedProp.SetValue(settings, DateTime.UtcNow);
                }

                var jsonContent = JsonSerializer.Serialize(settings, _jsonOptions);

                var (sessionResult, session) = await _fileService.StartEditSessionAsync(
                    _settingsFileName,
                    new FileOperationOptions
                    {
                        UserId = _userId,
                        ConflictStrategy = strategy
                    });

                if (!sessionResult.Success || session == null)
                {
                    result.Success = false;
                    result.ErrorMessage = sessionResult.ErrorMessage;
                    return result;
                }

                using (session)
                {
                    var saveResult = await session.SaveAsync(jsonContent, strategy);

                    result.Success = saveResult.Success;
                    result.ErrorMessage = saveResult.ErrorMessage;
                    result.Version = saveResult.Version;

                    if (saveResult.ConflictInfo != null)
                    {
                        result.HasConflict = true;
                        result.ConflictInfo = saveResult.ConflictInfo;
                        ConflictDetected?.Invoke(this, saveResult.ConflictInfo);
                    }

                    if (saveResult.Success)
                    {
                        _cachedSettings = settings;
                        SettingsChanged?.Invoke(this, settings);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"設定保存エラー: {ex.Message}";
                return result;
            }
        }

        public async Task<SettingsOperationResult> UpdateSettingSectionAsync<TSection>(
            Func<TSettings, TSection> selector,
            Action<TSection> updater,
            ConflictResolutionStrategy strategy = ConflictResolutionStrategy.Abort)
        {
            var (loadResult, currentSettings) = await LoadSettingsAsync();
            if (!loadResult.Success || currentSettings == null)
            {
                return loadResult;
            }

            var section = selector(currentSettings);
            updater(section);

            return await SaveSettingsAsync(currentSettings, strategy);
        }

        public TSettings? GetCachedSettings()
        {
            return _cachedSettings;
        }

        public async Task<SettingsOperationResult> CreateBackupAsync(string backupFileName = null)
        {
            var result = new SettingsOperationResult();

            try
            {
                backupFileName ??= $"app-settings-backup-{DateTime.Now:yyyyMMdd-HHmmss}.json";

                var (loadResult, settings) = await LoadSettingsAsync();
                if (!loadResult.Success || settings == null)
                {
                    return loadResult;
                }

                var jsonContent = JsonSerializer.Serialize(settings, _jsonOptions);
                var saveResult = await _fileService.SaveFileAsync(
                    $"backups/{backupFileName}",
                    jsonContent,
                    new FileOperationOptions { UserId = _userId });

                result.Success = saveResult.Success;
                result.ErrorMessage = saveResult.ErrorMessage;
                result.Version = saveResult.Version;

                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"バックアップ作成エラー: {ex.Message}";
                return result;
            }
        }

        public async Task<SettingsOperationResult> RestoreFromBackupAsync(string backupFileName)
        {
            var result = new SettingsOperationResult();

            try
            {
                var (readResult, content) = await _fileService.ReadFileAsync(
                    $"backups/{backupFileName}",
                    new FileOperationOptions { UserId = _userId });

                if (!readResult.Success)
                {
                    result.Success = false;
                    result.ErrorMessage = $"バックアップファイル読み込みエラー: {readResult.ErrorMessage}";
                    return result;
                }

                var settings = JsonSerializer.Deserialize<TSettings>(content!, _jsonOptions);
                if (settings == null)
                {
                    result.Success = false;
                    result.ErrorMessage = "バックアップファイルの形式が正しくありません";
                    return result;
                }

                return await SaveSettingsAsync(settings, ConflictResolutionStrategy.Overwrite);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"バックアップ復元エラー: {ex.Message}";
                return result;
            }
        }

        private TSettings CreateDefaultSettings()
        {
            return new TSettings();
        }

        public void Dispose()
        {
            _fileService?.Dispose();
        }
    }

    /// <summary>
    /// 使用例
    /// </summary>
    public class SettingsManagerExample
    {
        public async Task ExampleUsage()
        {
            using var settingsManager = new SettingsManager(@"C:\MyApp\Settings", "user123");

            // イベントハンドラーの設定
            settingsManager.SettingsChanged += (sender, settings) =>
            {
                Console.WriteLine($"設定が更新されました: {settings.LastUpdated}");
            };

            settingsManager.ConflictDetected += (sender, conflictInfo) =>
            {
                Console.WriteLine($"競合が検出されました: {conflictInfo.ConflictReason}");
            };

            // 設定の読み込み
            var (loadResult, settings) = await settingsManager.LoadSettingsAsync();
            if (loadResult.Success && settings != null)
            {
                Console.WriteLine($"現在のテーマ: {settings.User.Theme}");
            }

            // 特定のセクションの更新
            var updateResult = await settingsManager.UpdateSettingSectionAsync(
                s => s.User,
                user =>
                {
                    user.Theme = "Dark";
                    user.Language = "en-US";
                });

            if (updateResult.Success)
            {
                Console.WriteLine("ユーザー設定を更新しました");
            }

            // バックアップの作成
            var backupResult = await settingsManager.CreateBackupAsync();
            if (backupResult.Success)
            {
                Console.WriteLine("設定のバックアップを作成しました");
            }
        }
    }
}
