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
                //{"Italian", "it"},
                //{"German", "de"},
                //{"Russian", "ru"}
            };

            // List of country translations
            List<CountryTranslation> countryTranslations = new List<CountryTranslation>
            {
                // Add data for each country
                new CountryTranslation(1377, 1277, "Poland", "Polish", "pl"),
                new CountryTranslation(700, 1640, "France", "French", "fr"),
                new CountryTranslation(580, 1280, "UK", "English", "en"),
                // Add more countries as needed
            };

            // Translate country names and update the results
            foreach (var country in countryTranslations)
            {
                country.TranslationResult = TranslateText(inputText, fromLanguage, languageCodes[country.Language]);
            }

            // Load the map image
            string imagePath = "./europe.png"; // Update this with the path to your image file
            try
            {
                using (FileStream fs = new FileStream(imagePath, FileMode.Open))
                {
                    Image originalImage = Image.FromStream(fs);

                    // TODO add bucket/filling country with a color

                    // Display translated image with text
                    DisplayTranslatedImage(originalImage, countryTranslations);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load image: {ex.Message}");
            }
        }


        private void AddTextToImage(Image image, string text, int centerX, int centerY, int originalWidth, int originalHeight)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            using (Font font = new Font("Arial", 40, FontStyle.Bold))
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
                AddTextToImage(modifiedImage, country.TranslationResult, country.X, country.Y, originalImage.Width, originalImage.Height);
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
            // Dictionary to map language names to language codes
            Dictionary<string, string> languageCodes = new Dictionary<string, string>
            {
                {"Polish", "pl"},
                {"French", "fr"},
                {"English", "en"},
                //{"Italian", "it"},
                //{"German", "de"},
                //{"Russian", "ru"}
            };

            // Retrieve language code from dictionary based on language name
            return languageCodes.ContainsKey(languageName) ? languageCodes[languageName] : ""; // Return empty string if language name is not found
        }




    }
}
