using System.Windows.Forms;

namespace LanguagesMap
{
    partial class ModifiedImageForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        //private void InitializeComponent()
        //{
        //    this.components = new System.ComponentModel.Container();
        //    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //    this.ClientSize = new System.Drawing.Size(800, 450);
        //    this.Text = "Form2";
        //}
        private void InitializeComponent()
        {
            this.pictureBoxModifiedImage = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxModifiedImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxModifiedImage
            // 
            this.pictureBoxModifiedImage.Dock = DockStyle.Fill;
            this.pictureBoxModifiedImage.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxModifiedImage.Name = "pictureBoxModifiedImage";
            this.pictureBoxModifiedImage.Size = new System.Drawing.Size(800, 450);
            this.pictureBoxModifiedImage.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxModifiedImage.TabIndex = 0;
            this.pictureBoxModifiedImage.TabStop = false;
            // 
            // ModifiedImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBoxModifiedImage);
            this.Name = "ModifiedImageForm";
            this.Text = "Modified Image";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxModifiedImage)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
    }
}