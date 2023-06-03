using System.Data;

namespace MemoryGame
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ButtonStart = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // ButtonStart
            // 
            ButtonStart.Location = new Point(365, 261);
            ButtonStart.Name = "ButtonStart";
            ButtonStart.Size = new Size(75, 23);
            ButtonStart.TabIndex = 0;
            ButtonStart.Text = "START";
            ButtonStart.UseVisualStyleBackColor = true;
            ButtonStart.Click += ButtonStartClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(365, 68);
            label1.Name = "label1";
            label1.Size = new Size(42, 15);
            label1.TabIndex = 1;
            label1.Text = "失智";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(ButtonStart);
            Name = "MainForm";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ButtonStart;
        private Label label1;
    }
}