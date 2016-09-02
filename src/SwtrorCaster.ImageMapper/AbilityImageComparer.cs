using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace SwtrorCaster.ImageMapper
{
    public class AbilityImageComparer : IEqualityComparer<Bitmap>
    {
        public bool Equals(Bitmap bitmapA, Bitmap bitmapB)
        {
            bool equals = true;

            Rectangle rectA = new Rectangle(0, 0, bitmapA.Width, bitmapA.Height);
            Rectangle rectB = new Rectangle(0, 0, bitmapB.Width, bitmapB.Height);
            BitmapData bmpData1 = bitmapA.LockBits(rectA, ImageLockMode.ReadOnly, bitmapA.PixelFormat);
            BitmapData bmpData2 = bitmapB.LockBits(rectB, ImageLockMode.ReadOnly, bitmapB.PixelFormat);

            unsafe
            {
                byte* ptr1 = (byte*)bmpData1.Scan0.ToPointer();
                byte* ptr2 = (byte*)bmpData2.Scan0.ToPointer();

                int width = rectA.Width * 3;

                for (int y = 0; equals && y < rectA.Height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (*ptr1 != *ptr2)
                        {
                            equals = false;
                            break;
                        }
                        ptr1++;
                        ptr2++;
                    }
                    ptr1 += bmpData1.Stride - width;
                    ptr2 += bmpData2.Stride - width;
                }
            }

            bitmapA.UnlockBits(bmpData1);
            bitmapB.UnlockBits(bmpData2);

            return equals;
        }

        public int GetHashCode(Bitmap gameImage)
        {
            return 0;
        }
    }
}