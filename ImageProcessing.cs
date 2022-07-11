using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageComparison
{
    internal class ImageProcessing
    {
        public static Bitmap CropImage(Bitmap image, Rectangle rect)
        {

            Bitmap destImage = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(destImage))
            {
                g.DrawImage(image, -rect.X, -rect.Y);
            }

            return destImage;
        }
    }
}
