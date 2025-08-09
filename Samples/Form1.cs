using CoreLib.Services;
using System.Windows.Forms.Design;

namespace Samples
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            
        }

        

        // 例: ボタンイベントで実行
        private void MoveMdFilesToFolders(string targetDirectory)
        {
            if (!Directory.Exists(targetDirectory))
            {
                MessageBox.Show("指定したフォルダが存在しません。");
                return;
            }

            var mdFiles = Directory.GetFiles(targetDirectory, "*.pdf", SearchOption.TopDirectoryOnly);

            foreach (var filePath in mdFiles)
            {
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
                // ファイル名にフォルダ名として使用できない文字が含まれている場合は置換
                string folderName = string.Join("_", fileNameWithoutExt.Split(Path.GetInvalidFileNameChars()));
                //フォルダ名が長い場合は切り詰める
                if (folderName.Length > 100)
                {
                    folderName = folderName.Substring(0, 100);
                }
                // フォルダ名の最後に空白がある場合は削除
                folderName = folderName.TrimEnd();

                var newFolderPath = Path.Combine(targetDirectory, folderName);

                // フォルダがなければ作成
                Directory.CreateDirectory(newFolderPath);

                var destFilePath = Path.Combine(newFolderPath, Path.GetFileName(filePath));
                
                // 既に移動先にファイルがある場合は上書き
                File.Move(filePath, destFilePath, overwrite: true);
            }

            MessageBox.Show("完了しました。");
        }
    }
}
