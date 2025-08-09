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

        

        // ��: �{�^���C�x���g�Ŏ��s
        private void MoveMdFilesToFolders(string targetDirectory)
        {
            if (!Directory.Exists(targetDirectory))
            {
                MessageBox.Show("�w�肵���t�H���_�����݂��܂���B");
                return;
            }

            var mdFiles = Directory.GetFiles(targetDirectory, "*.pdf", SearchOption.TopDirectoryOnly);

            foreach (var filePath in mdFiles)
            {
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
                // �t�@�C�����Ƀt�H���_���Ƃ��Ďg�p�ł��Ȃ��������܂܂�Ă���ꍇ�͒u��
                string folderName = string.Join("_", fileNameWithoutExt.Split(Path.GetInvalidFileNameChars()));
                //�t�H���_���������ꍇ�͐؂�l�߂�
                if (folderName.Length > 100)
                {
                    folderName = folderName.Substring(0, 100);
                }
                // �t�H���_���̍Ō�ɋ󔒂�����ꍇ�͍폜
                folderName = folderName.TrimEnd();

                var newFolderPath = Path.Combine(targetDirectory, folderName);

                // �t�H���_���Ȃ���΍쐬
                Directory.CreateDirectory(newFolderPath);

                var destFilePath = Path.Combine(newFolderPath, Path.GetFileName(filePath));
                
                // ���Ɉړ���Ƀt�@�C��������ꍇ�͏㏑��
                File.Move(filePath, destFilePath, overwrite: true);
            }

            MessageBox.Show("�������܂����B");
        }
    }
}
