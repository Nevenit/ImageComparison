using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace ImageComparison
{
    public partial class Form1 : Form
    {
        Bitmap canvas = new Bitmap(1, 1);
        //Bitmap img1 = new Bitmap("C:\\Users\\Nevenit\\Pictures\\kirito.png");
        //Bitmap img2 = new Bitmap("C:\\Users\\Nevenit\\Pictures\\kirito3.png");

        //Temprorary way to chose images
        Bitmap img1 = new Bitmap("C:\\Users\\Nevenit\\Pictures\\8k.jpg");
        Bitmap img2 = new Bitmap("C:\\Users\\Nevenit\\Pictures\\1k2.jpg");

        //Camera variables
        float zoomValue = 1.0f;
        int[] zoomPoint = new int[2] { 500, 500 };

        //Mouse variables
        bool lMouseDown = false;
        bool rMouseDown = false;
        bool mMouseDown = false;

        Stopwatch stopwatch = new Stopwatch();
         
        public Form1()
        {
            InitializeComponent();
        }

        private void MouseWheelEvent(object sender, MouseEventArgs e)
        {
            int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
            int numberOfPixelsToMove = numberOfTextLinesToMove * 2;
            int scale = numberOfPixelsToMove / 6;

            zoomValue += (zoomValue / 20) * scale;
        }

        private void MouseDownEvent(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    lMouseDown = true;
                    break;
                case MouseButtons.Right:
                    rMouseDown = true;
                    break;
                case MouseButtons.Middle:
                    mMouseDown = true;
                    break;
                case MouseButtons.None:
                default:
                    break;
            }
        }

        private void MouseUpEvent(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    lMouseDown = false;
                    break;
                case MouseButtons.Right:
                    rMouseDown = false;
                    break;
                case MouseButtons.Middle:
                    mMouseDown = false;
                    break;
                case MouseButtons.None:
                default:
                    break;
            }
        }

        bool plMouseDown = false;
        Point pPos = new Point(0,0);
        private void mainLoop_Tick(object sender, EventArgs e)
        {
            stopwatch.Restart();
            var timeStart = stopwatch.ElapsedMilliseconds;

            // Update the canvas size for when the window is resized
            int[] canvasSize = { pictureBox1.Width, pictureBox1.Height };
            var pos = this.PointToClient(Cursor.Position);

            // Dispose of the last canvas
            canvas.Dispose();
            canvas = new Bitmap(canvasSize[0], canvasSize[1]);


            // Set bounds for the mosuse position
            if (pos.X < 1)
                pos.X = 1;
            if (pos.Y < 1)
                pos.Y = 1;

            // Calculate image pos
            int[] imgPos = { (int)((canvasSize[0] / 2) - (zoomPoint[0] * zoomValue)),
                             (int)((canvasSize[1] / 2) - (zoomPoint[1] * zoomValue))};

            // Calculate image size
            int[] imgSize = { 
                            (int)(Math.Max(img1.Width, img2.Width) * zoomValue),
                            (int)(Math.Max(img1.Height, img2.Height) * zoomValue) };

            // Move camera
            if (lMouseDown)
            {
                zoomPoint[0] -= (int)((pos.X - pPos.X) / zoomValue);
                zoomPoint[1] -= (int)((pos.Y - pPos.Y) / zoomValue);
            }

            using (Graphics g = Graphics.FromImage(canvas))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;

                g.DrawImage(img1, imgPos[0], imgPos[1], imgSize[0], imgSize[1]);

                g.DrawImage(img2, imgPos[0], imgPos[1], imgSize[0], imgSize[1]);

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

            plMouseDown = lMouseDown;
            pPos = pos;
        }
    }
}

/* TODO
 * grab and move
 * zoom relative to mouse pointer, zoom into mouse
 * 
 * 
 * 
 */