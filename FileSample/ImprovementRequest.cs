using CoreLib.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileSample
{
    public class ImprovementRequest
    {
        public string UserId { get; set; }
        public string RequestDetails { get; set; }
        public DateTime RequestDate { get; set; }

        public ImprovementRequest(string userId, string requestDetails)
        {
            UserId = userId;
            RequestDetails = requestDetails;
            RequestDate = DateTime.Now;
        }
        public override string ToString()
        {
            return $"{RequestDate.ToShortDateString()} - {UserId}: {RequestDetails}";
        }

    }

    /// <summary>
    /// FileServiceを使用してジェネリックなオブジェクトの保存・読み込みを行うマネージャクラス
    /// </summary>
    /// <typeparam name="T">管理するオブジェクトの型</typeparam>
    public class JsonFileManager<T> : IDisposable where T : class
    {
        private readonly FileService _fileService;
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileService">FileServiceインスタンス</param>
        /// <param name="filePath">保存するJSONファイルのパス</param>
        /// <param name="jsonOptions">JSONシリアライズオプション（オプション）</param>
        public JsonFileManager(string filePath, JsonSerializerOptions? jsonOptions = null)
        {
            _fileService = new FileService("Service");
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            _jsonOptions = jsonOptions ?? new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// オブジェクトをJSONファイルに保存
        /// </summary>
        /// <param name="data">保存するオブジェクト</param>
        /// <param name="options">ファイル操作オプション</param>
        /// <returns>操作結果</returns>
        public async Task<FileOperationResult> SaveAsync(T data, FileOperationOptions? options = null)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                var jsonContent = JsonSerializer.Serialize(data, _jsonOptions);
                return await _fileService.SaveFileAsync(_filePath, jsonContent, options);
            }
            catch (JsonException ex)
            {
                return new FileOperationResult
                {
                    Success = false,
                    ErrorMessage = $"JSONシリアライゼーションエラー: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new FileOperationResult
                {
                    Success = false,
                    ErrorMessage = $"保存エラー: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// コレクションをJSONファイルに保存
        /// </summary>
        /// <param name="dataCollection">保存するコレクション</param>
        /// <param name="options">ファイル操作オプション</param>
        /// <returns>操作結果</returns>
        public async Task<FileOperationResult> SaveCollectionAsync(IEnumerable<T> dataCollection, FileOperationOptions? options = null)
        {
            if (dataCollection == null)
                throw new ArgumentNullException(nameof(dataCollection));

            try
            {
                var jsonContent = JsonSerializer.Serialize(dataCollection, _jsonOptions);
                return await _fileService.SaveFileAsync(_filePath, jsonContent, options);
            }
            catch (JsonException ex)
            {
                return new FileOperationResult
                {
                    Success = false,
                    ErrorMessage = $"JSONシリアライゼーションエラー: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new FileOperationResult
                {
                    Success = false,
                    ErrorMessage = $"保存エラー: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// JSONファイルからオブジェクトを読み込み
        /// </summary>
        /// <param name="options">ファイル操作オプション</param>
        /// <returns>読み込み結果とオブジェクト</returns>
        public async Task<(FileOperationResult Result, T? Data)> LoadAsync(FileOperationOptions? options = null)
        {
            try
            {
                var (result, content) = await _fileService.ReadFileAsync(_filePath, options);

                if (!result.Success || string.IsNullOrEmpty(content))
                {
                    return (result, default(T));
                }

                var data = JsonSerializer.Deserialize<T>(content, _jsonOptions);
                return (result, data);
            }
            catch (JsonException ex)
            {
                var errorResult = new FileOperationResult
                {
                    Success = false,
                    ErrorMessage = $"JSONデシリアライゼーションエラー: {ex.Message}"
                };
                return (errorResult, default(T));
            }
            catch (Exception ex)
            {
                var errorResult = new FileOperationResult
                {
                    Success = false,
                    ErrorMessage = $"読み込みエラー: {ex.Message}"
                };
                return (errorResult, default(T));
            }
        }

        /// <summary>
        /// JSONファイルからコレクションを読み込み
        /// </summary>
        /// <param name="options">ファイル操作オプション</param>
        /// <returns>読み込み結果とコレクション</returns>
        public async Task<(FileOperationResult Result, List<T>? Data)> LoadCollectionAsync(FileOperationOptions? options = null)
        {
            try
            {
                var (result, content) = await _fileService.ReadFileAsync(_filePath, options);

                if (!result.Success || string.IsNullOrEmpty(content))
                {
                    return (result, null);
                }

                var data = JsonSerializer.Deserialize<List<T>>(content, _jsonOptions);
                return (result, data);
            }
            catch (JsonException ex)
            {
                var errorResult = new FileOperationResult
                {
                    Success = false,
                    ErrorMessage = $"JSONデシリアライゼーションエラー: {ex.Message}"
                };
                return (errorResult, null);
            }
            catch (Exception ex)
            {
                var errorResult = new FileOperationResult
                {
                    Success = false,
                    ErrorMessage = $"読み込みエラー: {ex.Message}"
                };
                return (errorResult, null);
            }
        }

        /// <summary>
        /// ファイル編集セッションを開始してオブジェクトを読み込み
        /// </summary>
        /// <param name="options">ファイル操作オプション</param>
        /// <returns>編集セッション結果とオブジェクト</returns>
        public async Task<(FileOperationResult Result, FileEditSession? Session, T? Data)> StartEditSessionAsync(FileOperationOptions? options = null)
        {
            try
            {
                var (result, session) = await _fileService.StartEditSessionAsync(_filePath, options);

                if (!result.Success || session == null)
                {
                    return (result, null, default(T));
                }

                // セッション内容からオブジェクトをデシリアライズ
                T? data = null;
                if (!string.IsNullOrEmpty(session.BaseContent))
                {
                    data = JsonSerializer.Deserialize<T>(session.BaseContent, _jsonOptions);
                }

                return (result, session, data);
            }
            catch (JsonException ex)
            {
                var errorResult = new FileOperationResult
                {
                    Success = false,
                    ErrorMessage = $"JSONデシリアライゼーションエラー: {ex.Message}"
                };
                return (errorResult, null, default(T));
            }
            catch (Exception ex)
            {
                var errorResult = new FileOperationResult
                {
                    Success = false,
                    ErrorMessage = $"編集セッション開始エラー: {ex.Message}"
                };
                return (errorResult, null, default(T));
            }
        }

        /// <summary>
        /// 編集セッション内でオブジェクトを保存
        /// </summary>
        /// <param name="session">編集セッション</param>
        /// <param name="data">保存するオブジェクト</param>
        /// <param name="strategy">競合解決戦略</param>
        /// <returns>保存結果</returns>
        public async Task<FileOperationResult> SaveInSessionAsync(FileEditSession session, T data, ConflictResolutionStrategy? strategy = null)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                var jsonContent = JsonSerializer.Serialize(data, _jsonOptions);
                return await session.SaveAsync(jsonContent, strategy);
            }
            catch (JsonException ex)
            {
                return new FileOperationResult
                {
                    Success = false,
                    ErrorMessage = $"JSONシリアライゼーションエラー: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new FileOperationResult
                {
                    Success = false,
                    ErrorMessage = $"セッション保存エラー: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// ファイルが存在するかチェック
        /// </summary>
        /// <returns>ファイル存在結果</returns>
        public async Task<(FileOperationResult Result, bool Exists)> ExistsAsync()
        {
            var (result, _) = await _fileService.ReadFileAsync(_filePath);

            if (result.Success)
            {
                return (result, true);
            }

            if (result.ErrorMessage?.Contains("ファイルが存在しません") == true)
            {
                var successResult = new FileOperationResult { Success = true };
                return (successResult, false);
            }

            return (result, false);
        }

        /// <summary>
        /// リソースを解放
        /// </summary>
        public void Dispose()
        {
            _fileService?.Dispose();
        }
    }
}
