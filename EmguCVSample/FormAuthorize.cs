using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
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
    public partial class FormAuthorize : Form
    {
        private VideoCapture _capture;
        private bool _captureInProgress;
        private Mat _frame;
        private CascadeClassifier _faceDetector;
        private EigenFaceRecognizer _recognizer;
        private List<string> _personNames = new List<string>();
        private string _dataPath = Path.Combine(Application.StartupPath, "FaceData");
        private string _modelPath;

        public FormAuthorize()
        {
            InitializeComponent();
            _frame = new Mat();

            // 顔検出器の初期化
            _faceDetector = new CascadeClassifier(Path.Combine(Application.StartupPath, "haarcascade_frontalface_default.xml"));

            _modelPath = Path.Combine(_dataPath, "trained_model.xml");

            // 名前リストの読み込み
            LoadPersonNames();

            // 顔認識器の初期化
            InitializeRecognizer();
        }

        private void FormAuthorize_Load(object sender, EventArgs e)
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

        private void FormAuthorize_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_captureInProgress)
            {
                Application.Idle -= ProcessFrame;
                _captureInProgress = false;
            }
            _capture?.Dispose();
            _frame?.Dispose();
            _faceDetector?.Dispose();
            _recognizer?.Dispose();
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            if (_capture != null && _captureInProgress && _recognizer != null)
            {
                _capture.Read(_frame);

                if (!_frame.IsEmpty)
                {
                    // 顔検出を実行
                    var grayFrame = new Mat();
                    CvInvoke.CvtColor(_frame, grayFrame, ColorConversion.Bgr2Gray);
                    CvInvoke.EqualizeHist(grayFrame, grayFrame);

                    var faces = _faceDetector.DetectMultiScale(
                        grayFrame,
                        1.1,
                        10,
                        new Size(30, 30));

                    foreach (var face in faces)
                    {
                        // 検出された顔に枠を描画
                        CvInvoke.Rectangle(_frame, face, new MCvScalar(0, 255, 0), 2);

                        // 顔認識を実行
                        var faceROI = new Mat(grayFrame, face);
                        var resizedFace = new Mat();
                        CvInvoke.Resize(faceROI, resizedFace, new Size(100, 100));

                        // 予測
                        var result = _recognizer.Predict(resizedFace);
                        int label = result.Label;
                        double distance = result.Distance;

                        // 認識結果を表示
                        string name;
                        if (distance < 4000 && label >= 0 && label < _personNames.Count)
                        {
                            name = _personNames[label];

                            // 名前を顔の上に表示
                            Point textPoint = new Point(face.X, face.Y - 10);
                            if (textPoint.Y < 10) textPoint.Y = face.Y + face.Height + 20;

                            CvInvoke.PutText(_frame, name, textPoint,
                                FontFace.HersheyComplex, 0.8, new MCvScalar(0, 255, 0), 2);

                            lblRecognitionResult.Text = $"認識結果：{name}";
                            lblRecognitionResult.ForeColor = Color.Green;
                        }
                        else
                        {
                            // 信頼度が低い場合
                            CvInvoke.PutText(_frame, "不明",
                                new Point(face.X, face.Y - 10),
                                FontFace.HersheyComplex, 0.8, new MCvScalar(0, 0, 255), 2);

                            lblRecognitionResult.Text = "認識結果：不明";
                            lblRecognitionResult.ForeColor = Color.Red;
                        }
                    }

                    // 画像の表示
                    pictureBoxCamera.Image = _frame.ToBitmap();
                    grayFrame.Dispose();
                }
            }
        }

        private void LoadPersonNames()
        {
            string namesFilePath = Path.Combine(_dataPath, "names.txt");
            if (File.Exists(namesFilePath))
            {
                _personNames = new List<string>(File.ReadAllLines(namesFilePath));
                lblStatus.Text = $"{_personNames.Count}人の名前データを読み込みました";
            }
            else
            {
                lblStatus.Text = "名前データが見つかりません";
                MessageBox.Show("トレーニングデータが見つかりませんでした。まずトレーニングを行ってください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void InitializeRecognizer()
        {
            try
            {
                if (File.Exists(_modelPath))
                {
                    _recognizer = new EigenFaceRecognizer();
                    _recognizer.Read(_modelPath);
                    lblStatus.Text = "認識モデルを読み込みました";
                }
                else
                {
                    lblStatus.Text = "認識モデルが見つかりません";
                    MessageBox.Show("トレーニング済みのモデルが見つかりませんでした。まずトレーニングを行ってください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "モデルの読み込みに失敗しました";
                MessageBox.Show($"認識モデルの読み込みに失敗しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
