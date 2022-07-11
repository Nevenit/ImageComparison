using System.Diagnostics;

namespace ImageComparison
{
    public partial class Form1 : Form
    {
        Bitmap canvas = new Bitmap(1, 1);
        Bitmap img1 = new Bitmap("C:\\Users\\Nevenit\\Pictures\\kirito.png");
        Bitmap img2 = new Bitmap("C:\\Users\\Nevenit\\Pictures\\kirito3.png");

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var pos = this.PointToClient(Cursor.Position);

            canvas.Dispose();
            canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            //int imgOffset = ClientSize.Width / 2 - img1.Width / 2;

            if (pos.X < 1)
                pos.X = 1;
            if (pos.Y < 1)
                pos.Y = 1;

            using (Graphics g = Graphics.FromImage(canvas))
            {
                g.DrawImage(img1, new Point(0, 0));

                using (Bitmap croppedImg = ImageProcessing.CropImage(img2, new Rectangle(0, 0, pos.X, pictureBox1.Height)))
                {
                    g.DrawImage(croppedImg, new Point(0, 0));
                }

                Pen pen = new Pen(Color.FromArgb(255, 255, 255));
                g.DrawLine(pen, pos.X, 0, pos.X, pictureBox1.Height);
            }

            pictureBox1.Image = canvas;

        }
    }
}