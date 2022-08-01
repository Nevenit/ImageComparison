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
        Bitmap img2 = new Bitmap("C:\\Users\\Nevenit\\Pictures\\SteamScreenshots\\2160p.png");
        Bitmap img1 = new Bitmap("C:\\Users\\Nevenit\\Pictures\\SteamScreenshots\\1080p.png");

        //Camera variables
        float zoomValue = 1.0f;
        float[] zoomPoint = new float[2] { 500, 500 };

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

        private void MouseDownEvent(object sender, MouseEventArgs e)
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

        private void MouseUpEvent(object sender, MouseEventArgs e)
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

            // Calculate image pos
            int[] imgPos = { (int)((canvasSize[0] / 2) - (zoomPoint[0] * zoomValue)),
                              (int)((canvasSize[1] / 2) - (zoomPoint[1] * zoomValue))};

            // Calculate image size
            int[] imgSize = {
                            (int)(Math.Max(img1.Width, img2.Width) * zoomValue),
                            (int)(Math.Max(img1.Height, img2.Height) * zoomValue) };

            double pixelSize = (double)imgSize[0] / img2.Width;


            // Make sure the mouse isnt beyond the image
            int[] mousePos = { Math.Clamp(pos.X, imgPos[0], imgPos[0] + imgSize[0]), 
                                 Math.Clamp(pos.Y, imgPos[1], imgPos[1] + imgSize[1])};

            // Move camera
            if (lMouseDown)
            {
                zoomPoint[0] -= (pos.X - pPos.X) / zoomValue;
                zoomPoint[1] -= (pos.Y - pPos.Y) / zoomValue;
            }

            using (Graphics g = Graphics.FromImage(canvas))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.CompositingQuality = CompositingQuality.HighSpeed;

                g.DrawImage(img1, imgPos[0], imgPos[1], imgSize[0], imgSize[1]);


                // Mouse x relative to the image
                int mouseXOnImg = mousePos[0] - imgPos[0];

                // Large scaled image to small original image
                int scaledImageWidth = (int)(mouseXOnImg / pixelSize);

                // Width to nearest pixel
                float flooredWidth = (float)(pixelSize * scaledImageWidth);

                //label1.Text = String.Format("X: {0} \nY: {1}\nW: {2}\nMoI: {3}\nMoIBound: {4}\nBORKED?: {5}\nPixelSize: {6}\nFlooredWidth: {7}", imgPos[0], imgPos[1], scaledImageWidth, mouseXOnImg, (mouseXOnImg % pixelSize), mouseXOnImg - (mouseXOnImg % pixelSize), pixelSize, flooredWidth);

                if (scaledImageWidth > 0)
                    using (Bitmap croppedImg = ImageProcessing.CropImage(img2, new Rectangle(0, 0, scaledImageWidth, img2.Height)))
                    {
                        g.DrawImage(croppedImg, imgPos[0], imgPos[1], flooredWidth, imgSize[1]);
                    }

                Pen pen = new Pen(Color.FromArgb(255, 255, 255));
                g.DrawLine(pen, pos.X, 0, pos.X, pictureBox1.Height);
            }
            var timeEnd = stopwatch.ElapsedMilliseconds;
            //label1.Text = timeEnd - timeStart + "ms";

            pictureBox1.Image = canvas;

            plMouseDown = lMouseDown;
            pPos = pos;
        }
    }
}

/* TODO
 * mouse limits only apply to the form not to the image itself
 * crop background image as well as the fron
 * 
 * 
 * 
 */