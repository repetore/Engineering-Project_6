using OpenCvSharp;
using OpenCvSharp.Aruco;
using OpenCvSharp.Extensions;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.SceneGraph.Assets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Point = OpenCvSharp.Point;

namespace Koshkin_IP
{
    public partial class Form1 : Form
    {
        OpenGL gl;
        Thread cameraThread, dataFromApiThread;
        bool runVideo;
        VideoCapture capture;
        DetectorParameters detectorParam = new DetectorParameters();
        Mat matInput;
        Mat bgmatInput;
        Point2f[][] corners;
        int[] ids = new int[] { };
        Mat rvec, tvec;
        bool[] displayMode = new bool[] { false, true };
        bool showOneMarker;
        int idMark = 0;
        List<EquipmentData> data = new List<EquipmentData>();
        readonly Mat cameraMatrix = new Mat(3, 3, MatType.CV_64F, new double[] { 968.08624052, 0, 644.63057786, 0, 955.40821957, 364.58219136, 0, 0, 1 });
        readonly Mat distCoeffs = new Mat(5, 1, MatType.CV_64F, new double[] { -4.73423959e-02, -1.25875642e+00, 1.12354237e-04, 4.72701099e-03, 6.96144663e+00 });
        readonly Point3f[] objectPoints = new Point3f[] { new Point3f(1.0f, 1.0f, 0), new Point3f(-1.0f, 1.0f, 0), new Point3f(-1.0f, -1.0f, 0), new Point3f(1.0f, -1.0f, 0) };
        readonly Scalar[] colorsScalar = new Scalar[] { Scalar.Red, Scalar.Green, Scalar.Black, Scalar.Blue };
        readonly Dictionary dictionary;
        readonly Texture cameraTexture = new Texture();
        string filePath;

        VideoCapture OnLabelVideoCapture;
        VideoCapture BackgroundVideoCapture;
        Texture videoTexture = new Texture();
        Texture bgvideoTexture = new Texture();
        Mat labelvideoframe;


        private bool isRecording = false;
        private VideoWriter videoWriter;
        private string outputFilePath;
        



        private void button1_Click(object sender, EventArgs e)
        {
            if (OnLabelVideoCapture == null || BackgroundVideoCapture == null)
            {
                MessageBox.Show("Сначала выберите оба видео");
            }
            else
            {
                if (runVideo)
                {
                    runVideo = false;
                    timer1.Stop();
                    DisposeVideo();
                    button1.Text = "Старт";
                }
                else
                {
                    timer1.Start();
                    runVideo = true;
                    matInput = new Mat();
                    capture = new VideoCapture(0)
                    {
                        FrameHeight = 720,
                        FrameWidth = 1280,
                        AutoFocus = true
                    };

                    cameraThread = new Thread(new ThreadStart(CaptureCameraCallback));
                    cameraThread.Start();
                    button1.Text = "Стоп";
                }
            }
            

        }
        private void StartRecording()
        {
            if (isRecording) return;

            // Define the output file path
            outputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "recorded_video.avi");

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(outputFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Print the output file path for debugging
            Console.WriteLine($"Output file path: {outputFilePath}");

            // Define the frame size and codec
            int frameWidth = openGLControl1.Width;
            int frameHeight = openGLControl1.Height;
            var frameSize = new OpenCvSharp.Size(frameWidth, frameHeight);

            // Use MJPG codec instead of H264
            videoWriter = new VideoWriter(outputFilePath, FourCC.MJPG, 30, frameSize);

            // Check if VideoWriter is opened successfully
            if (!videoWriter.IsOpened())
            {
                MessageBox.Show("Не удалось открыть VideoWriter.");
                return;
            }

            isRecording = true;
            recordingLabel.Visible = true;
            MessageBox.Show($"Начата запись: {outputFilePath}");
        }

        private void StopRecording()
        {
            if (!isRecording) return;

            // Release the VideoWriter and finalize the video file
            videoWriter.Release();
            videoWriter.Dispose();

            isRecording = false;
            recordingLabel.Visible = false;
            MessageBox.Show($"Запись завершена: {outputFilePath}");
        }

        private void DisposeVideo()
        {
            if (cameraThread != null && cameraThread.IsAlive) cameraThread.Abort();
            if (dataFromApiThread != null && dataFromApiThread.IsAlive) dataFromApiThread.Abort();
            matInput?.Dispose();
            capture?.Dispose();
        }

        public Form1()
        {
            InitializeComponent();
            dictionary = CvAruco.GetPredefinedDictionary(PredefinedDictionaryName.Dict6X6_50);


            recordingLabel.Text = "ЗАПИСЬ ИДЕТ";
            recordingLabel.ForeColor = Color.Red;
            recordingLabel.BackColor = Color.Transparent;
        }

        private void openGLControl1_OpenGLInitialized(object sender, EventArgs e)
        {
            gl = openGLControl1.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
        }
        private void openGLControl1_OpenGLDraw(object sender, RenderEventArgs args)
        {

            double trackbarX = Convert.ToDouble(trackBarX.Value);
            double trackbarY = Convert.ToDouble(trackBarY.Value);
            double trackbarZ = Convert.ToDouble(trackBarZ.Value);

            if (!checkBox1.Checked && radioButton3.Checked)
            {
                if (runVideo && !matInput.Empty())
                {
                    gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

                    ShowCameraGL();

                    gl.MatrixMode(OpenGL.GL_PROJECTION);
                    gl.LoadIdentity();
                    gl.Perspective(45, openGLControl1.Width / (double)openGLControl1.Height, 0.1f, 100.0f);
                    gl.MatrixMode(OpenGL.GL_MODELVIEW);
                    gl.LoadIdentity();

                    if (ids.Length > 0)
                    {
                        for (int i = 0; i < ids.Length; i++)
                        {
                            if (showOneMarker && ids[i] != idMark) continue;
                            rvec = new Mat();
                            tvec = new Mat();
                            Cv2.SolvePnP(InputArray.Create(objectPoints), InputArray.Create(corners[i]), cameraMatrix, distCoeffs, rvec, tvec);
                            if (!rvec.Empty() && !tvec.Empty())
                            {
                                var matrix = TransitionToMark(rvec, tvec);
                                gl.LoadMatrix(matrix);

                                ReceivingData();
                                PrintEquipmentData(matrix, trackbarX, trackbarY, trackbarZ);
                                
                            }
                        }
                    }
                }
                
            }
            if(!checkBox1.Checked && radioButton1.Checked)
            {
                
                if (runVideo && !matInput.Empty())
                {

                    gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

                    ShowCameraGL();

                    gl.MatrixMode(OpenGL.GL_PROJECTION);
                    gl.LoadIdentity();
                    gl.Perspective(45, openGLControl1.Width / (double)openGLControl1.Height, 0.1f, 100.0f);
                    gl.MatrixMode(OpenGL.GL_MODELVIEW);
                    gl.LoadIdentity();

                    if (ids.Length > 0)
                    {
                        for (int i = 0; i < ids.Length; i++)
                        {
                            if (showOneMarker && ids[i] != idMark) continue;
                            rvec = new Mat();
                            tvec = new Mat();
                            Cv2.SolvePnP(InputArray.Create(objectPoints), InputArray.Create(corners[i]), cameraMatrix, distCoeffs, rvec, tvec);
                            if (!rvec.Empty() && !tvec.Empty())
                            {
                                var matrix = TransitionToMark(rvec, tvec);
                                gl.LoadMatrix(matrix);

                                VideoProjectionOnSurface(matrix, trackbarX, trackbarY, trackbarZ);
                                

                            }
                        }
                    }
                }

            }
            if (checkBox1.Checked && radioButton1.Checked)
            {
                if (cameraThread.IsAlive)
                {
                    cameraThread.Suspend();
                }
                


                bgmatInput = BackgroundVideoCapture.RetrieveMat();

                CvAruco.DetectMarkers(bgmatInput, dictionary, out corners, out ids, detectorParam, out _);
                if (ids.Length > 0)
                {

                    for (int j = 0; j < ids.Length; j++)
                    {
                        if (showOneMarker && ids[j] != idMark) continue;

                        Point center = new Point((corners[j][0].X + corners[j][1].X + corners[j][2].X + corners[j][3].X) / 4,
                                                 (corners[j][0].Y + corners[j][1].Y + corners[j][2].Y + corners[j][3].Y) / 4);

                        Cv2.Circle(bgmatInput, center, 10, Scalar.Yellow, 2);

                        for (int i = 0; i < 4; i++)
                        {
                            Cv2.Line(bgmatInput, corners[j][i].ToPoint(), corners[j][(i + 1) % 4].ToPoint(), Scalar.Red, 3);
                            Cv2.Circle(matInput, corners[j][i].ToPoint(), 5, 0, 2);
                        }
                    }

                }
                


                gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

                gl.MatrixMode(OpenGL.GL_PROJECTION);
                gl.LoadIdentity();
                gl.Perspective(45, openGLControl1.Width / (double)openGLControl1.Height, 0.1f, 100.0f);
                gl.MatrixMode(OpenGL.GL_MODELVIEW);
                gl.LoadIdentity();

                if (ids.Length > 0)
                {
                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (showOneMarker && ids[i] != idMark) continue;
                        rvec = new Mat();
                        tvec = new Mat();
                        Cv2.SolvePnP(InputArray.Create(objectPoints), InputArray.Create(corners[i]), cameraMatrix, distCoeffs, rvec, tvec);
                        if (!rvec.Empty() && !tvec.Empty())
                        {
                            var matrix = TransitionToMark(rvec, tvec);
                            gl.LoadMatrix(matrix);

                            VideoProjectionOnSurface(matrix, trackbarX, trackbarY, trackbarZ);
                            ShowBackgroundVideoGL(matrix, trackbarX, trackbarY, trackbarZ);

                        }
                    }
                }
                
            }

            gl.Flush();

            if (isRecording)
            {
                CaptureFrame();
                
            }
        }
        private void CaptureFrame()
        {
            if (openGLControl1.Width > 0 && openGLControl1.Height > 0)
            {
                // Capture the frame from the OpenGL framebuffer
                Bitmap bitmap = new Bitmap(openGLControl1.Width, openGLControl1.Height, PixelFormat.Format24bppRgb);
                BitmapData data = bitmap.LockBits(new Rectangle(0, 0, openGLControl1.Width, openGLControl1.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                gl.ReadPixels(0, 0, openGLControl1.Width, openGLControl1.Height, OpenGL.GL_BGR, OpenGL.GL_UNSIGNED_BYTE, data.Scan0);

                bitmap.UnlockBits(data);
                bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY); // Flip the image vertically

                Mat frame = BitmapConverter.ToMat(bitmap);
                videoWriter.Write(frame);

                frame.Dispose();
                bitmap.Dispose();
            }
        }


        private void ShowBackgroundVideoGL(double[] matrix, double trackbarX, double trackbarY, double trackbarZ)
        {

            if (BackgroundVideoCapture != null && BackgroundVideoCapture.IsOpened())
            {
                Mat backgroundFrame = new Mat();
                BackgroundVideoCapture.Read(backgroundFrame);

                if (!backgroundFrame.Empty())
                {
                    //
                    //DetectAndDisplayMarker(backgroundFrame, matrix, trackbarX, trackbarY, trackbarZ);
                    //

                    Bitmap bitmap = BitmapConverter.ToBitmap(backgroundFrame);
                    

                    gl.MatrixMode(OpenGL.GL_PROJECTION);
                    gl.LoadIdentity();
                    gl.Ortho(-1, 1, -1, 1, 0.1f, 100);
                    gl.MatrixMode(OpenGL.GL_MODELVIEW);
                    gl.LoadIdentity();
                    gl.Translate(0.0f, 0.0f, -100.0f);

                    bgvideoTexture.Create(gl, bitmap);
                    bgvideoTexture.Bind(gl);
                    gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                    gl.Color(1.0f, 1.0f, 1.0f, 1.0f);
                    gl.Begin(OpenGL.GL_QUADS);
                    gl.TexCoord(0.0f, 0.0f); gl.Vertex(1.0f, 1.0f, 0);
                    gl.TexCoord(1.0f, 0.0f); gl.Vertex(-1.0f, 1.0f, 0);
                    gl.TexCoord(1.0f, 1.0f); gl.Vertex(-1.0f, -1.0f, 0);
                    gl.TexCoord(0.0f, 1.0f); gl.Vertex(1.0f, -1.0f, 0);

                    gl.End();
                }
            }
        }

        
        private void PrintEquipmentData(double[] matrix, double trackbarX, double trackbarY, double trackbarZ)
        {
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0.5f, 0.5f, 1f,0.75f); //gray
            gl.Vertex(-1.5f + (trackbarX), -1.5f + (trackbarY), -trackbarZ);
            gl.Vertex(-1.5f + (trackbarX), 1.5f + (trackbarY), -trackbarZ);
            gl.Vertex(1.5f + (trackbarX), 1.5f + (trackbarY), -trackbarZ);
            gl.Vertex(1.5f + (trackbarX), -1.5f + (trackbarY), -trackbarZ);
            gl.End();
            gl.Translate(-1 + (trackbarX), 0.6 + (trackbarY), -0.01f + (-trackbarZ));


            gl.LoadMatrix(matrix);

            gl.Translate(0 + (trackbarX), 0 + (trackbarY), -0.01f + (-trackbarZ));
        }

        private double[] TransitionToMark(Mat localRvec, Mat localTvec)
        {

            gl.LoadIdentity();
            Mat rotation = new Mat();
            Mat viewMatrix = Mat.Zeros(rows: 4, 4, MatType.CV_64F);
            Cv2.Rodrigues(localRvec, rotation);
            for (int row = 0; row < 3; ++row)
            {
                for (int col = 0; col < 3; ++col)
                {
                    viewMatrix.At<double>(row, col) = rotation.At<double>(row, col);
                }
                viewMatrix.At<double>(row, 3) = localTvec.At<double>(row, 0);
            }
            viewMatrix.At<double>(3, 3) = 1.0f;

            Mat cvToGl = Mat.Zeros(rows: 4, 4, MatType.CV_64F);
            cvToGl.At<double>(0, 0) = -1.0f; // Invert the x axis из-за отзеркаливания
            cvToGl.At<double>(1, 1) = -1.0f; // Invert the y axis
            cvToGl.At<double>(2, 2) = -1.0f; // invert the z axis
            cvToGl.At<double>(3, 3) = 1.0f;
            viewMatrix = cvToGl * viewMatrix;

            Mat glViewMatrix = Mat.Zeros(rows: 4, 4, MatType.CV_64F);
            Cv2.Transpose(viewMatrix, glViewMatrix);
            double[] doubleArray = new double[glViewMatrix.Rows * glViewMatrix.Cols];

            for (int row = 0; row < glViewMatrix.Rows; row++)
            {
                for (int col = 0; col < glViewMatrix.Cols; col++)
                {
                    doubleArray[row * glViewMatrix.Cols + col] = glViewMatrix.At<double>(row, col);
                }
            }
            return doubleArray;
        }

        private void ShowCameraGL()
        {
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Ortho(-1, 1, -1, 1, 0.1f, 100);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();
            gl.Translate(0.0f, 0.0f, -100.0f);

            cameraTexture.Create(gl, matInput.ToBitmap());
            cameraTexture.Bind(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.Color(1.0f, 1.0f, 1.0f, 1.0f);
            gl.Begin(OpenGL.GL_QUADS);
            gl.TexCoord(0.0f, 0.0f); gl.Vertex(1.0f, 1.0f, 0);
            gl.TexCoord(1.0f, 0.0f); gl.Vertex(-1.0f, 1.0f, 0);
            gl.TexCoord(1.0f, 1.0f); gl.Vertex(-1.0f, -1.0f, 0);
            gl.TexCoord(0.0f, 1.0f); gl.Vertex(1.0f, -1.0f, 0);

            gl.End();
        }

        private void CaptureCameraCallback()
        {
            while (runVideo)
            {

                matInput = capture.RetrieveMat();
                CvAruco.DetectMarkers(matInput, dictionary, out corners, out ids, detectorParam, out _);
                if (ids.Length > 0)
                {
                    
                        for (int j = 0; j < ids.Length; j++)
                        {
                            if (showOneMarker && ids[j] != idMark) continue;

                            Point center = new Point((corners[j][0].X + corners[j][1].X + corners[j][2].X + corners[j][3].X) / 4,
                                                     (corners[j][0].Y + corners[j][1].Y + corners[j][2].Y + corners[j][3].Y) / 4);

                            Cv2.Circle(matInput, center, 10, Scalar.Yellow, 2);

                            for (int i = 0; i < 4; i++)
                            {
                                Cv2.Line(matInput, corners[j][i].ToPoint(), corners[j][(i + 1) % 4].ToPoint(), Scalar.Red, 3);
                                Cv2.Circle(matInput, corners[j][i].ToPoint(), 5, colorsScalar[i], 2);
                            }
                        }
                    
                }
                Invoke(new Action(() =>
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }));
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            
        }


        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {   
            displayMode[0] = false;
            displayMode[1] = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Add("IdMark", "IdMark");
            dataGridView1.Columns.Add("Coords", "Coords");

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ids.Length > 0)
            {
                dataGridView1.Rows.Clear();

                for (int i = 0; i < ids.Length; i++)
                {
                    Point center = new Point((corners[i][0].X + corners[i][1].X + corners[i][2].X + corners[i][3].X) / 4,
                                             (corners[i][0].Y + corners[i][1].Y + corners[i][2].Y + corners[i][3].Y) / 4);

                    dataGridView1.Rows.Add($"{ids[i]}", $"{center}");

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "";
                openFileDialog.Filter = "mp4 files (*.mp4)|*.mp4";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    // Инициализируйте VideoCapture только один раз
                    if (OnLabelVideoCapture != null)
                    {
                        OnLabelVideoCapture.Release();
                        OnLabelVideoCapture.Dispose();
                    }

                    OnLabelVideoCapture = new VideoCapture(filePath);
                    if (!OnLabelVideoCapture.IsOpened())
                    {
                        MessageBox.Show("Не удалось открыть видеофайл.");
                        OnLabelVideoCapture = null;
                    }
                }
            }
        }

        private void VideoProjectionOnSurface(double[] matrix, double trackbarX, double trackbarY, double trackbarZ)
        {
            if (OnLabelVideoCapture != null && OnLabelVideoCapture.IsOpened())
            {
                if (labelvideoframe != null)
                {
                    labelvideoframe.Dispose();
                }

                labelvideoframe = new Mat();
                OnLabelVideoCapture.Read(labelvideoframe);

                if (!labelvideoframe.Empty())
                {
                    Bitmap bitmap = BitmapConverter.ToBitmap(labelvideoframe);
                    videoTexture.Create(gl, bitmap);

                    // Сброс состояния цвета
                    gl.Color(1.0f, 1.0f, 1.0f, 1.0f);

                    // Включение текстуры
                    videoTexture.Bind(gl);

                    // Задание параметров текстуры
                    gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR);
                    gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR);

                    // Наложение текстуры на поверхность
                    gl.Begin(OpenGL.GL_QUADS);

                    gl.TexCoord(0f, 1f); gl.Vertex(-1f + (trackbarX), -1f + (trackbarY), 0f - trackbarZ);
                    gl.TexCoord(1f, 1f); gl.Vertex(1f + (trackbarX), -1f + (trackbarY), 0f - trackbarZ);
                    gl.TexCoord(1f, 0f); gl.Vertex(1f + (trackbarX), 1f + (trackbarY), 0f - trackbarZ);
                    gl.TexCoord(0f, 0f); gl.Vertex(-1f + (trackbarX), 1f + (trackbarY), 0f - trackbarZ);

                    gl.End();

                    // Загрузка матрицы
                    gl.LoadMatrix(matrix);

                    // Трансляция на указанную позицию
                    gl.Translate(0 + (trackbarX), 0 + (trackbarY), -0.01f + (-trackbarZ));
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "";
                openFileDialog.Filter = "mp4 files (*.mp4)|*.mp4";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;

                    BackgroundVideoCapture = new VideoCapture(filePath);
                    if (!BackgroundVideoCapture.IsOpened())
                    {
                        MessageBox.Show("Не удалось открыть видеофайл.");
                        BackgroundVideoCapture = null;
                    }
                }
            }
        }
        bool prev_check1_state = true;

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            prev_check1_state = !prev_check1_state;
            if (prev_check1_state == false && cameraThread.IsAlive)
            {
                cameraThread.Abort();
            }
            if (prev_check1_state)
            {
                cameraThread = new Thread(CaptureCameraCallback); 
                cameraThread.Start();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            StartRecording();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            StopRecording();
        }

        private void ReceivingData()
        {
            var tempData = new EquipmentData(idMark);
            data.Add(tempData);
            
            
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeVideo();
        }


    }
    public class EquipmentData
    {
        public int ID;
        public EquipmentData(int ID)
        {
            this.ID = ID;
        }
    }

}