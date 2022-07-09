using System.Diagnostics;

namespace ImageComparison
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var pos = this.PointToClient(Cursor.Position);

            Debug.WriteLine(pos.ToString());
        }
    }
}