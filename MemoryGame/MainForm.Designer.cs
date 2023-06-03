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
            comboBoxLang1 = new ComboBox();
            comboBoxLang2 = new ComboBox();
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
            // comboBox1
            // 
            comboBoxLang1.FormattingEnabled = true;
            comboBoxLang1.Location = new Point(238, 261);
            comboBoxLang1.Name = "comboBoxLang1";
            comboBoxLang1.Size = new Size(121, 23);
            comboBoxLang1.TabIndex = 1;
            var rows1 = LangDataTable.Rows.Cast<DataRow>();
            var distinctValues1 = rows1.Select(row => row["Lang"]).Distinct().ToList();
            comboBoxLang1.DataSource = distinctValues1;
            comboBoxLang1.DisplayMember = "Lang";
            // 
            // comboBox2
            // 
            comboBoxLang2.FormattingEnabled = true;
            comboBoxLang2.Location = new Point(446, 261);
            comboBoxLang2.Name = "comboBoxLang2";
            comboBoxLang2.Size = new Size(121, 23);
            comboBoxLang2.TabIndex = 2;
            var rows2 = LangDataTable.Rows.Cast<DataRow>();
            var distinctValues2 = rows2.Select(row => row["Lang"]).Distinct().ToList();
            comboBoxLang2.DataSource = distinctValues2;
            comboBoxLang2.DisplayMember = "Lang";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(comboBoxLang2);
            Controls.Add(comboBoxLang1);
            Controls.Add(ButtonStart);
            Name = "MainForm";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ButtonStart;
        private ComboBox comboBoxLang1, comboBoxLang2;
    }
}