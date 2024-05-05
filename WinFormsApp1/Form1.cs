using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Windows.Forms;
using System.Drawing;
using System.IO;



namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private const int LabelWidth = 100;
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

            string imageUrl = "https://img.redro.pl/plakaty/simplified-map-of-europe-rounded-shapes-of-states-with-smoothed-border-grey-simple-flat-blank-vector-map-700-255204020.jpg"; // Replace with the actual URL of the image
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    byte[] imageData = webClient.DownloadData(imageUrl); // Download image data
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        Image originalImage = Image.FromStream(ms); // Create image from data
                        pictureBox1.Image = ScaleImage(originalImage, pictureBox1.Width, pictureBox1.Height); // Scale image and assign to PictureBox
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load image from URL: {ex.Message}");
            }


        }

        static string TranslateText(string input, string fromLanguage, string toLanguage)
        {
            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(input)}";
            var webClient = new WebClient
            {
                Encoding = System.Text.Encoding.UTF8
            };
            var result = webClient.DownloadString(url);
            try
            {
                result = result.Substring(4, result.IndexOf("\"", 4, StringComparison.Ordinal) - 4);
                return result;
            }
            catch (Exception)
            {
                return "error";
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
