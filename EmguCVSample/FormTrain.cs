using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmguCVSample
{
    public partial class FormTrain : Form
    {
        private VideoCapture _capture;
        private bool _captureInProgress;
        private Mat _frame;
        private CascadeClassifier _faceDetector;
        private List<Mat> _faceImages = new List<Mat>();
        private List<int> _faceLabels = new List<int>();
        private List<string> _personNames = new List<string>();
        private EigenFaceRecognizer _recognizer;
        private int _nextLabel = 0;
        private string _dataPath = Path.Combine(Application.StartupPath, "FaceData");
        private string _modelPath;

        public FormTrain()
        {
            InitializeComponent();
            _frame = new Mat();

            // 顔検出器の初期化
            _faceDetector = new CascadeClassifier(Path.Combine(Application.StartupPath, "haarcascade_frontalface_default.xml"));

            // データ保存用ディレクトリの作成
            if (!Directory.Exists(_dataPath))
            {
                Directory.CreateDirectory(_dataPath);
            }

            _modelPath = Path.Combine(_dataPath, "trained_model.xml");

            // 既存のデータの読み込み
            LoadExistingData();
        }

        private void FormTrain_Load(object sender, EventArgs e)
        {
            try
            {
                _capture = new VideoCapture(0); // デフォルトのカメラを使用
                _captureInProgress = true;
                Application.Idle += ProcessFrame;
                lblStatus.Text = "カメラ接続完了";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"カメラの接続に失敗しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "カメラ接続エラー";
            }
        }

        private void FormTrain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_captureInProgress)
            {
                Application.Idle -= ProcessFrame;
                _captureInProgress = false;
            }
            _capture?.Dispose();
            _frame?.Dispose();
            _faceDetector?.Dispose();
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            if (_capture != null && _captureInProgress)
            {
                _capture.Read(_frame);

                if (!_frame.IsEmpty)
                {
                    // 顔検出を実行
                    var grayFrame = new Mat();
                    CvInvoke.CvtColor(_frame, grayFrame, ColorConversion.Bgr2Gray);

                    // ヒストグラム平坦化で照明条件の影響を軽減
                    CvInvoke.EqualizeHist(grayFrame, grayFrame);

                    var faces = new Rectangle[0];
                    using (var ugray = new UMat())
                    {
                        grayFrame.CopyTo(ugray);

                        // 顔検出
                        faces = _faceDetector.DetectMultiScale(
                            ugray,
                            1.1,
                            10,
                            new Size(30, 30));
                    }

                    // 検出された顔に枠を描画
                    foreach (var face in faces)
                    {
                        CvInvoke.Rectangle(_frame, face, new MCvScalar(0, 255, 0), 2);
                    }

                    // 画像の表示
                    pictureBoxCamera.Image = _frame.ToBitmap();
                    grayFrame.Dispose();
                }
            }
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("名前を入力してください", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 現在のフレームから顔を抽出
            var grayFrame = new Mat();
            CvInvoke.CvtColor(_frame, grayFrame, ColorConversion.Bgr2Gray);
            CvInvoke.EqualizeHist(grayFrame, grayFrame);

            var faces = _faceDetector.DetectMultiScale(
                grayFrame,
                1.1,
                10,
                new Size(30, 30));

            if (faces.Length > 0)
            {
                // 最も大きな顔を選択
                var largestFaceIndex = 0;
                var largestFaceArea = 0;
                for (int i = 0; i < faces.Length; i++)
                {
                    var area = faces[i].Width * faces[i].Height;
                    if (area > largestFaceArea)
                    {
                        largestFaceArea = area;
                        largestFaceIndex = i;
                    }
                }

                var face = faces[largestFaceIndex];
                var faceROI = new Mat(grayFrame, face);

                // 顔画像をリサイズして保存
                var resizedFace = new Mat();
                CvInvoke.Resize(faceROI, resizedFace, new Size(100, 100));

                // 既存のラベル確認
                string personName = txtName.Text.Trim();
                int label;
                if (_personNames.Contains(personName))
                {
                    label = _personNames.IndexOf(personName);
                }
                else
                {
                    label = _nextLabel++;
                    _personNames.Add(personName);
                }

                // 学習データに追加
                _faceImages.Add(resizedFace.Clone());
                _faceLabels.Add(label);

                // 画像を保存
                string fileName = $"{Guid.NewGuid()}.png";
                string filePath = Path.Combine(_dataPath, fileName);
                CvInvoke.Imwrite(filePath, resizedFace);

                // メタデータ記録
                File.AppendAllText(Path.Combine(_dataPath, "metadata.csv"),
                    $"{fileName},{label},{personName}\n");

                lblStatus.Text = $"{personName}の顔を追加しました";
            }
            else
            {
                lblStatus.Text = "顔が検出されませんでした";
                MessageBox.Show("顔が検出されませんでした。再試行してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            grayFrame.Dispose();
        }

        private void btnSaveTrainingData_Click(object sender, EventArgs e)
        {
            if (_faceImages.Count == 0 || _faceLabels.Count == 0)
            {
                MessageBox.Show("トレーニングデータがありません。顔をキャプチャしてください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // ラベル配列の作成
                var labels = new VectorOfInt(_faceLabels.ToArray());

                // トレーニングデータMatを作成
                using var faces = new VectorOfMat();
                foreach (var image in _faceImages)
                {
                    faces.Push(image);
                }

                // 顔認識器の作成とトレーニング
                _recognizer = new EigenFaceRecognizer();
                _recognizer.Train(faces, labels);

                // モデルを保存
                _recognizer.Write(_modelPath);

                // 名前リストの保存
                File.WriteAllLines(Path.Combine(_dataPath, "names.txt"), _personNames);

                lblStatus.Text = "トレーニングが完了しました";
                MessageBox.Show("顔認識モデルのトレーニングが完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "トレーニングエラー";
                MessageBox.Show($"トレーニング中にエラーが発生しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadExistingData()
        {
            // メタデータがあれば読み込む
            string metadataPath = Path.Combine(_dataPath, "metadata.csv");
            if (File.Exists(metadataPath))
            {
                try
                {
                    var lines = File.ReadAllLines(metadataPath);
                    foreach (var line in lines)
                    {
                        var parts = line.Split(',');
                        if (parts.Length >= 3)
                        {
                            string fileName = parts[0];
                            int label = int.Parse(parts[1]);
                            string personName = parts[2];

                            // 名前リストに追加（重複なし）
                            if (!_personNames.Contains(personName))
                            {
                                _personNames.Add(personName);
                                if (label >= _nextLabel)
                                {
                                    _nextLabel = label + 1;
                                }
                            }

                            // 顔画像を読み込む
                            string imagePath = Path.Combine(_dataPath, fileName);
                            if (File.Exists(imagePath))
                            {
                                var faceImage = CvInvoke.Imread(imagePath, ImreadModes.Grayscale);
                                _faceImages.Add(faceImage);
                                _faceLabels.Add(label);
                            }
                        }
                    }
                    lblStatus.Text = $"{_faceImages.Count}個の既存トレーニングデータを読み込みました";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"既存データの読み込み中にエラーが発生しました: {ex.Message}", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
