using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace ImageComparison
{
    public partial class Form1 : Form
    {
        Bitmap canvas = new Bitmap(1, 1);
        //Bitmap img1 = new Bitmap("C:\\Users\\Nevenit\\Pictures\\kirito.png");
        //Bitmap img2 = new Bitmap("C:\\Users\\Nevenit\\Pictures\\kirito3.png");

        Bitmap img1 = new Bitmap("C:\\Users\\Nevenit\\Pictures\\1k.jpg");
        Bitmap img2 = new Bitmap("C:\\Users\\Nevenit\\Pictures\\1k2.jpg");

        float zoomValue = 1.0f;
        int[] zoomPoint = new int[2] { 500, 500 };

        Stopwatch stopwatch = new Stopwatch();
         
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            //zoomPoint = new int[2]{ pictureBox1.Width, pictureBox1.Height};
            int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
            int numberOfPixelsToMove = numberOfTextLinesToMove * 2;
            int wtfScaling = numberOfPixelsToMove / 6;

            zoomValue += (zoomValue / 20) * wtfScaling;


            Debug.WriteLine(zoomValue.ToString());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            stopwatch.Restart();
            var timeStart = stopwatch.ElapsedMilliseconds;

            int[] canvasSize = { pictureBox1.Width, pictureBox1.Height };
            var pos = this.PointToClient(Cursor.Position);

            canvas.Dispose();
            canvas = new Bitmap(canvasSize[0], canvasSize[1]);

            

            if (pos.X < 1)
                pos.X = 1;
            if (pos.Y < 1)
                pos.Y = 1;

            int[] imgPos = { (int)((canvasSize[0] / 2) - (zoomPoint[0] * zoomValue)),
                             (int)((canvasSize[1] / 2) - (zoomPoint[1] * zoomValue))};

            using (Graphics g = Graphics.FromImage(canvas))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;

                g.DrawImage(img1, imgPos[0], imgPos[1], img1.Width * zoomValue, img1.Height * zoomValue);

                //using (Bitmap croppedImg = ImageProcessing.CropImage(img1, new Rectangle((int)(img1.Width * zoomValue * (pos.X / (float)imgSize[0])), 0, imgSize[0] - (int)(img1.Width * zoomValue * (pos.X / (float)imgSize[0])), (int)(img1.Height * zoomValue))))
                //{
                //    g.DrawImage(croppedImg, pos.X, 0, imgSize[0], imgSize[1]);
                //}

                //using (Bitmap croppedImg = ImageProcessing.CropImage(img2, new Rectangle(0, 0, (int)(img2.Width * zoomValue * (pos.X / (float)imgSize[0])), (int)(img2.Height * zoomValue))))
                //{
                //    g.DrawImage(croppedImg, 0, 0, pos.X, imgSize[1]);
                //}

                Pen pen = new Pen(Color.FromArgb(255, 255, 255));
                g.DrawLine(pen, pos.X, 0, pos.X, pictureBox1.Height);
            }
            var timeEnd = stopwatch.ElapsedMilliseconds;
            label1.Text = timeEnd - timeStart + "ms";

            pictureBox1.Image = canvas;

            
        }
    }
}