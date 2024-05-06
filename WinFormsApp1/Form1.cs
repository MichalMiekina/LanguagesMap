using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
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
            comboBox1.Items.Add("Italian");
            comboBox1.Items.Add("German");
            comboBox1.Items.Add("Russian");

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
                {"Italian", "it"},
                {"German", "de"},
                {"Russian", "ru"}
            };

            // Create a list to store labels that need to be removed
            List<Label> labelsToRemove = new List<Label>();

            // Identify labels that need to be removed
            foreach (Control control in Controls)
            {
                if (control is Label && control.Name.StartsWith("translatedLabel_"))
                {
                    labelsToRemove.Add((Label)control);
                }
            }

            // Remove and dispose of labels
            foreach (Label label in labelsToRemove)
            {
                Controls.Remove(label);
                label.Dispose();
            }

            // Translate the input text into each target language and display the result using labels
            int currentY = LabelStartY;
            foreach (var kvp in languageCodes)
            {
                string targetLanguage = kvp.Value;

                // Skip translation if the target language is the same as the input language
                if (targetLanguage == fromLanguage)
                    continue;

                string translatedText = TranslateText(inputText, fromLanguage, targetLanguage);
                Label label = new Label
                {
                    Text = $"{kvp.Key}: {translatedText}",
                    Location = new System.Drawing.Point(LabelMargin, currentY),
                    Size = new System.Drawing.Size(LabelWidth, LabelHeight),
                    Name = $"translatedLabel_{kvp.Key}" // Assigning a name with the prefix "translatedLabel_"
                };

                Controls.Add(label);
                currentY += LabelHeight + LabelMargin;
            }

            // Stop the stopwatch after translation is complete
            stopwatch.Stop();

            // Display elapsed time in the label
            elapsedTimeLabel.Text = $"Translation took {stopwatch.ElapsedMilliseconds} ms";

            // Reset the stopwatch for the next translation
            stopwatch.Reset();

            string imagePath = "./europe.png"; // Update this with the path to your image file
            try
            {
                using (FileStream fs = new FileStream(imagePath, FileMode.Open))
                {
                    Image originalImage = Image.FromStream(fs); // Create image from file stream
                    DisplayTranslatedImage(originalImage, textBox1.Text); // Display translated image with text
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
            using (Font font = new Font("Arial", 4, FontStyle.Bold))
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



        private void DisplayTranslatedImage(Image originalImage, string text)
        {
            // Scale image and assign to PictureBox
            pictureBox1.Image = ScaleImage(originalImage, pictureBox1.Width, pictureBox1.Height);

            // Add text with its center at specific pixel coordinates relative to the original image size
            int centerX = 1377; // Example: X coordinate of the center of the text on the original image
            int centerY = 1277; // Example: Y coordinate of the center of the text on the original image
            int originalWidth = originalImage.Width; // Original width of the image
            int originalHeight = originalImage.Height; // Original height of the image
            AddTextToImage(pictureBox1.Image, text, centerX, centerY, originalWidth, originalHeight);
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
                {"Italian", "it"},
                {"German", "de"},
                {"Russian", "ru"}
            };

            // Retrieve language code from dictionary based on language name
            return languageCodes.ContainsKey(languageName) ? languageCodes[languageName] : ""; // Return empty string if language name is not found
        }


        private Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            int newWidth;
            int newHeight;
            double aspectRatio = (double)image.Width / image.Height;

            if (aspectRatio > 1) // Landscape orientation
            {
                newWidth = maxWidth;
                newHeight = (int)(maxWidth / aspectRatio);
            }
            else // Portrait or square orientation
            {
                newWidth = (int)(maxHeight * aspectRatio);
                newHeight = maxHeight;
            }

            return new Bitmap(image, newWidth, newHeight);
        }


    }
}
