using System.Drawing;

public class PixelRGB
{
    public required int HorizontalPos { get; set; }
    public required int VerticalPos { get; set; }
    public int Red = 0;
    public int Green = 0;
    public int Blue = 0;
    public bool ExistInsideImage = true;

    public void DoesntExist()
    {
        this.Red = 0;
        this.Green = 0;
        this.Blue = 0;
        this.ExistInsideImage = false;
    }
}

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap image;
            string outputName;
            System.Drawing.Imaging.ImageFormat outputFormat;

            try
            {
                Console.Write("Image name (with extension): ");
                string imageName = Console.ReadLine();
                string path = @"C:\Users\AntonyLanglois\RiderProjects\LearningCSHARP\LearningCSHARP\" +
                              imageName;

                if (!File.Exists(path))
                {
                    throw new Exception("File doesn't exist");
                }

                Console.Write("Image name output: ");
                outputName = Console.ReadLine();

                if (outputName == "")
                {
                    throw new Exception("Please enter a valid output name");
                }

                image = new Bitmap(path);
                outputFormat = image.RawFormat;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            string[] creationResult = createNewImages(image, outputName, outputFormat);
            if (creationResult[0] == "success")
            {
                Console.WriteLine("Edge detection done, here are you files names:\n" + creationResult[1] + "\n" +
                                  creationResult[2]);
            }
        }

        static string[] createNewImages(Bitmap image, string outputName,
            System.Drawing.Imaging.ImageFormat outputFormat)
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height);
            Bitmap newImage2 = new Bitmap(image.Width, image.Height);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    PixelRGB topLeft = new PixelRGB() { HorizontalPos = x - 1, VerticalPos = y - 1 };

                    PixelRGB top = new PixelRGB() { HorizontalPos = x, VerticalPos = y - 1 };

                    PixelRGB topRight = new PixelRGB() { HorizontalPos = x + 1, VerticalPos = y - 1 };

                    PixelRGB left = new PixelRGB() { HorizontalPos = x - 1, VerticalPos = y };

                    PixelRGB right = new PixelRGB() { HorizontalPos = x + 1, VerticalPos = y };

                    PixelRGB bottomLeft = new PixelRGB() { HorizontalPos = x - 1, VerticalPos = y + 1 };

                    PixelRGB bottom = new PixelRGB() { HorizontalPos = x, VerticalPos = y + 1 };

                    PixelRGB bottomRight = new PixelRGB() { HorizontalPos = x + 1, VerticalPos = y + 1 };


                    if (x == 0)
                    {
                        topLeft.DoesntExist();
                        left.DoesntExist();
                        bottomLeft.DoesntExist();
                    }
                    else if (x == image.Width - 1)
                    {
                        topRight.DoesntExist();
                        right.DoesntExist();
                        bottomRight.DoesntExist();
                    }

                    if (y == 0)
                    {
                        topLeft.DoesntExist();
                        top.DoesntExist();
                        topRight.DoesntExist();
                    }
                    else if (y == image.Height - 1)
                    {
                        bottomLeft.DoesntExist();
                        bottom.DoesntExist();
                        bottomRight.DoesntExist();
                    }

                    PixelRGB[] currentPixels = { topLeft, top, topRight, left, right, bottomLeft, bottom, bottomRight };
                    foreach (PixelRGB pixel in currentPixels)
                    {
                        if (pixel.ExistInsideImage)
                        {
                            pixel.Red = image.GetPixel(pixel.HorizontalPos, pixel.VerticalPos).R;
                            pixel.Green = image.GetPixel(pixel.HorizontalPos, pixel.VerticalPos).G;
                            pixel.Blue = image.GetPixel(pixel.HorizontalPos, pixel.VerticalPos).B;
                        }
                    }

                    int red_gx = topLeft.Red + topRight.Red * -1 + left.Red * 2 + right.Red * -2 + bottomLeft.Red +
                                 bottomRight.Red * -1;
                    int green_gx = topLeft.Green + topRight.Green * -1 + left.Green * 2 + right.Green * -2 +
                                   bottomLeft.Green +
                                   bottomRight.Green * -1;
                    int blue_gx = topLeft.Blue + topRight.Blue * -1 + left.Blue * -2 + right.Blue * 2 + -
                                      bottomLeft.Blue +
                                  bottomRight.Blue * -1;

                    int red_gy = topLeft.Red + top.Red * 2 + topRight.Red + bottomLeft.Red * -1 +
                                 bottom.Red * -2 + bottomRight.Red * -1;
                    int green_gy = topLeft.Green + top.Green * 2 + topRight.Green + bottomLeft.Green * -1 +
                                   bottom.Green * -2 + bottomRight.Green * -1;
                    int blue_gy = topLeft.Blue + top.Blue * 2 + topRight.Blue + bottomLeft.Blue * -1 +
                                  bottom.Blue * -2 + bottomRight.Blue * -1;

                    double totalRed = red_gx * red_gx + red_gy * red_gy;
                    double totalGreen = green_gx * green_gx + green_gy * green_gy;
                    double totalBlue = blue_gx * blue_gx + blue_gy * blue_gy;

                    double red = Math.Round(Math.Sqrt(totalRed));
                    double green = Math.Round(Math.Sqrt(totalGreen));
                    double blue = Math.Round(Math.Sqrt(totalBlue));

                    if (red > 255)
                    {
                        red = 255;
                    }
                    else if (red < 0)
                    {
                        red = 0;
                    }

                    if (green > 255)
                    {
                        green = 255;
                    }
                    else if (green < 0)
                    {
                        green = 0;
                    }

                    if (blue > 255)
                    {
                        blue = 255;
                    }
                    else if (blue < 0)
                    {
                        blue = 0;
                    }

                    newImage.SetPixel(x, y, Color.FromArgb(100, (int)red, (int)green, (int)blue));

                    if (red + green + blue / 3 > 255)
                    {
                        red = 255;
                        green = 255;
                        blue = 255;
                    }
                    else
                    {
                        red = 0;
                        green = 0;
                        blue = 0;
                    }

                    newImage2.SetPixel(x, y, Color.FromArgb(100, (int)red, (int)green, (int)blue));
                }
            }

            newImage.Save(@"C:\\Users\\AntonyLanglois\\RiderProjects\\LearningCSHARP\\LearningCSHARP\\" + outputName +
                          "." + outputFormat);
            newImage2.Save(@"C:\\Users\\AntonyLanglois\\RiderProjects\\LearningCSHARP\\LearningCSHARP\\" + outputName +
                           "-blackandwhite." + outputFormat);
            string[] package =
                { "success", outputName + "." + outputFormat, outputName + "-blackandwhite." + outputFormat };
            return package;
        }
    }
}