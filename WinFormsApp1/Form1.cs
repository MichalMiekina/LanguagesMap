using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using LanguagesMap;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private const int LabelWidth = 300;
        private const int LabelHeight = 20;
        private const int LabelMargin = 5;
        private const int LabelStartY = 100;

        private Stopwatch stopwatch; // Stopwatch to measure time
        private Label elapsedTimeLabel; // Label to display elapsed time

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Add language options to the ComboBox
            comboBox1.Items.Add("Polish");
            comboBox1.Items.Add("French");
            comboBox1.Items.Add("English");
            //comboBox1.Items.Add("Italian");
            //comboBox1.Items.Add("German");
            //comboBox1.Items.Add("Russian");

            // Set default selection
            comboBox1.SelectedIndex = 0; // Select the first item by default

            // Initialize the stopwatch
            stopwatch = new Stopwatch();

            // Create the label for displaying elapsed time
            elapsedTimeLabel = new Label
            {
                Location = new System.Drawing.Point(LabelMargin, LabelStartY - LabelHeight - LabelMargin),
                Size = new System.Drawing.Size(300, LabelHeight),
                Name = "elapsedTimeLabel"
            };
            Controls.Add(elapsedTimeLabel); // Add the label to the form
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Start the stopwatch when the button is clicked
            stopwatch.Start();

            string inputText = textBox1.Text;
            string fromLanguageName = comboBox1.Text; // Get the selected language name from the ComboBox
            string fromLanguageCode = GetLanguageCode(fromLanguageName); // Convert language name to language code
            TranslateAndDisplay(inputText, fromLanguageCode);
        }

        private void TranslateAndDisplay(string inputText, string fromLanguage)
        {
            // Dictionary to map language names to language codes
            Dictionary<string, string> languageCodes = new Dictionary<string, string>
            {
                {"Polish", "pl"},
                {"French", "fr"},
                {"English", "en"},
                {"German", "de"},
                {"Italian", "it"},
                {"Spanish", "es"},
                {"Dutch", "nl"},
                {"Luxembourgish", "lb"},
                {"Czech", "cs"},
                {"Slovak", "sk"},
                {"Hungarian", "hu"},
                {"Croatian", "hr"},
                {"Bosnian", "bs"},
                {"Serbian", "sr"},
                {"Montenegrin", "cnr"},
                {"Albanian", "sq"},
                {"Greek", "el"},
                {"Bulgarian", "bg"},
                {"Romanian", "ro"},
                {"Slovene", "sl"},
                {"Macedonian", "mk"},
                {"Ukrainian", "uk"},
                {"Belarusian", "be"},
                {"Lithuanian", "lt"},
                {"Latvian", "lv"},
                {"Estonian", "et"},
                {"Norwegian", "no"},
                {"Swedish", "sv"},
                {"Finnish", "fi"},
                {"Icelandic", "is"},
                {"Portuguese", "pt"},
                {"Irish", "ga"},
                {"Maltese", "mt"},
                {"Russian", "ru"},
                {"Moldovan", "ro"},  // Moldovan is essentially Romanian
                {"Kosovan", "sq"}, // Official languages in Kosovo are Albanian and Serbian
                {"Cypriot", "el"} // Greek is one of the official languages in Cyprus
            };

            List<CountryTranslation> countryTranslations = new List<CountryTranslation>
            {
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1377, 1277) }, "Poland", "Polish", "pl"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(700, 1640) }, "France", "French", "fr"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(580, 1280) }, "UK", "English", "en"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1900, 1350) }, "Ukraine", "Ukrainian", "uk"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1038, 1300) }, "Germany", "German", "de"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(370, 2000), Tuple.Create(669, 2096) }, "Spain", "Spanish", "es"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1100, 1922) }, "Italy", "Italian", "it"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1750, 1700) }, "Romania", "Romanian", "ro"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(2200, 900) }, "Russia", "Russian", "ru"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1040, 800) }, "Norway", "Norwegian", "no"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(115, 2025) }, "Portugal", "Portuguese", "pt"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(835, 1321) }, "Netherlands", "Dutch", "nl"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(800, 1400) }, "Belgium", "Dutch", "nl"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(940, 1645) }, "Switzerland", "German", "de"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1227, 1587) }, "Austria", "German", "de"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1247, 1463) }, "Czech Republic", "Czech", "cs"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1441, 1600) }, "Hungary", "Hungarian", "hu"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1420, 1500) }, "Slovakia", "Slovak", "sk"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1230, 1700) }, "Slovenia", "Slovene", "sl"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1333, 1717) }, "Croatia", "Croatian", "hr"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1380, 1810) }, "Bosnia and Herzegovina", "Bosnian", "bs"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1530, 1790) }, "Serbia", "Serbian", "sr"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1450, 1800) }, "Montenegro", "Montenegrin", "sr"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1506, 1980) }, "Albania", "Albanian", "sq"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1580, 1950) }, "North Macedonia", "Macedonian", "mk"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1700, 1900) }, "Bulgaria", "Bulgarian", "bg"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1700, 2137) }, "Greece", "Greek", "el"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(2222, 1888) }, "Turkey", "Turkish", "tr"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1450, 715) }, "Finland", "Finnish", "fi"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1200, 700) }, "Sweden", "Swedish", "sv"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1000, 1050) }, "Denmark", "Danish", "da"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1566, 1040) }, "Lithuania", "Lithuanian", "lt"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1560, 950) }, "Latvia", "Latvian", "lv"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1540, 840) }, "Estonia", "Estonian", "et"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1700, 1170) }, "Belarus", "Belarusian", "be"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1814, 1520) }, "Moldova", "Moldovan", "ro"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(1536, 1890) }, "Kosovo", "Kosovan", "sq"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(350, 1150) }, "Ireland", "Irish", "ga"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(263, 347) }, "Iceland", "Icelandic", "is"),
                new CountryTranslation(new List<Tuple<int, int>> { Tuple.Create(2233, 2222) }, "Cyprus", "Cypriot", "el")
            };


            // Translate country names and update the results
            foreach (var country in countryTranslations)
            {
                country.TranslationResult = TranslateText(inputText, fromLanguage, country.LanguageCode);
            }
            // Create a new list with only CountryName and TranslationResult properties
            var simplifiedTranslations = countryTranslations.Select(ct => new { CountryName = ct.CountryName, TranslationResult = ct.TranslationResult }).ToList();

            // Serialize the list of simplified translations into JSON format
            string jsonOutput = JsonConvert.SerializeObject(simplifiedTranslations);

            // Specify the path for the JSON output file
            string jsonFilePath = Path.Combine("translated", "translations.json");

            // Write the JSON data to a file
            File.WriteAllText(jsonFilePath, jsonOutput);

            // Display a message box with the path to the JSON file
            MessageBox.Show($"Translation results saved to: {jsonFilePath}");
            // Load the map image
            string imagePath = "./europe.png"; // Update this with the path to your image file
            try
            {
                using (FileStream fs = new FileStream(imagePath, FileMode.Open))
                {
                    Bitmap originalImage = (Bitmap)Image.FromStream(fs);

                    // Fill regions with color before adding text
                    foreach (var country in countryTranslations)
                    {
                        foreach (var coord in country.Coordinates)
                        {
                            Point regionPoint = new Point(coord.Item1, coord.Item2); // Point within the region
                            Color targetColor = originalImage.GetPixel(regionPoint.X, regionPoint.Y); // The color to be replaced
                            Color fillColor = Color.FromArgb(128, Color.Red); // Semi-transparent red fill color
                            FloodFill(originalImage, regionPoint, targetColor, fillColor);
                        }
                    }

                    // Display translated image with text
                    DisplayTranslatedImage(originalImage, countryTranslations);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load image: {ex.Message}");
            }
        }

        private void FloodFill(Bitmap bmp, Point pt, Color targetColor, Color replacementColor)
        {
            if (targetColor.ToArgb() == replacementColor.ToArgb())
                return;

            Stack<Point> pixels = new Stack<Point>();
            pixels.Push(pt);

            while (pixels.Count > 0)
            {
                Point currentPixel = pixels.Pop();
                if (currentPixel.X < 0 || currentPixel.X >= bmp.Width || currentPixel.Y < 0 || currentPixel.Y >= bmp.Height)
                    continue;

                if (bmp.GetPixel(currentPixel.X, currentPixel.Y) == targetColor)
                {
                    bmp.SetPixel(currentPixel.X, currentPixel.Y, replacementColor);

                    pixels.Push(new Point(currentPixel.X - 1, currentPixel.Y));
                    pixels.Push(new Point(currentPixel.X + 1, currentPixel.Y));
                    pixels.Push(new Point(currentPixel.X, currentPixel.Y - 1));
                    pixels.Push(new Point(currentPixel.X, currentPixel.Y + 1));
                }
            }
        }

        private void AddTextToImage(Image image, string text, int centerX, int centerY, int originalWidth, int originalHeight)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            using (Font font = new Font("Arial", 20, FontStyle.Bold))
            using (StringFormat stringFormat = new StringFormat())
            {
                // Set string alignment to center
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                // Calculate the scaling factor between the original and displayed image sizes
                float scaleX = (float)image.Width / originalWidth;
                float scaleY = (float)image.Height / originalHeight;

                // Calculate the absolute pixel coordinates on the original image
                int absCenterX = (int)(centerX * scaleX);
                int absCenterY = (int)(centerY * scaleY);

                // Calculate the rectangle to draw the text around the specified center point
                Size textSize = TextRenderer.MeasureText(text, font);
                Rectangle rect = new Rectangle(absCenterX - textSize.Width / 2, absCenterY - textSize.Height / 2, textSize.Width, textSize.Height);

                // Draw the text on the image
                graphics.DrawString(text, font, Brushes.Black, rect, stringFormat);
            }
        }

        private void DisplayTranslatedImage(Image originalImage, List<CountryTranslation> countryTranslations)
        {
            // Create a copy of the original image to avoid modifying the original
            Image modifiedImage = new Bitmap(originalImage);

            // Add translated text to the image for each country
            foreach (var country in countryTranslations)
            {
                AddTextToImage(modifiedImage, country.TranslationResult,country.Coordinates[0].Item1, country.Coordinates[0].Item2, originalImage.Width, originalImage.Height);
            }

            // Save the modified image locally
            string outputDirectory = @"E:\europes"; // Output directory
            string outputFileName = $"europe_{DateTime.Now:yyyyMMddHHmmss}.png"; // Output file name
            string outputPath = Path.Combine(outputDirectory, outputFileName); // Full output path
            try
            {
                // Create directory if it doesn't exist
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Save the image
                modifiedImage.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);

                // Display a message box with the path to the saved image
                MessageBox.Show($"Modified image saved to: {outputPath}");

                // Display the modified image in a new window
                ShowModifiedImageForm(modifiedImage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save image: {ex.Message}");
            }
        }

        private void ShowModifiedImageForm(Image modifiedImage)
        {
            // Create a new instance of ModifiedImageForm
            ModifiedImageForm modifiedImageForm = new ModifiedImageForm();

            // Set the PictureBox image to the modified image
            modifiedImageForm.pictureBoxModifiedImage.Image = modifiedImage;

            // Show the ModifiedImageForm
            modifiedImageForm.ShowDialog();
        }

        static string TranslateText(string input, string fromLanguage, string toLanguage)
        {
            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(input)}";
            using (var webClient = new WebClient())
            {
                var result = webClient.DownloadString(url);
                try
                {
                    // Parse the JSON response to extract the translated text
                    var jsonArray = JArray.Parse(result);
                    var translatedText = jsonArray[0][0][0].ToString();
                    return translatedText;
                }
                catch (Exception)
                {
                    return "error";
                }
            }
        }

        private string GetLanguageCode(string languageName)
        {
            Dictionary<string, string> languageCodes = new Dictionary<string, string>
            {
                {"Polish", "pl"},
                {"French", "fr"},
                {"English", "en"},
                {"German", "de"},
                {"Italian", "it"},
                {"Spanish", "es"},
                {"Dutch", "nl"},
                {"Luxembourgish", "lb"},
                {"Czech", "cs"},
                {"Slovak", "sk"},
                {"Hungarian", "hu"},
                {"Croatian", "hr"},
                {"Bosnian", "bs"},
                {"Serbian", "sr"},
                {"Montenegrin", "cnr"},
                {"Albanian", "sq"},
                {"Greek", "el"},
                {"Bulgarian", "bg"},
                {"Romanian", "ro"},
                {"Slovenian", "sl"},
                {"Macedonian", "mk"},
                {"Ukrainian", "uk"},
                {"Belarusian", "be"},
                {"Lithuanian", "lt"},
                {"Latvian", "lv"},
                {"Estonian", "et"},
                {"Norwegian", "no"},
                {"Swedish", "sv"},
                {"Finnish", "fi"},
                {"Icelandic", "is"},
                {"Portuguese", "pt"},
                {"Irish", "ga"},
                {"Maltese", "mt"},
                {"Russian", "ru"},
                {"Moldovan", "ro"}, // Moldovan is essentially Romanian
                {"Kosovan", "sq"}, // Official languages in Kosovo are Albanian and Serbian
                {"Cypriot", "el"} // Greek is one of the official languages in Cyprus
            };

            // Retrieve language code from dictionary based on language name
            return languageCodes.ContainsKey(languageName) ? languageCodes[languageName] : ""; // Return empty string if language name is not found
        }
    }
}
