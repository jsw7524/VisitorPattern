﻿namespace WinFormsApp1
{
    partial class Form1
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
            button1 = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            button2 = new Button();
            richTextBox1 = new RichTextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(32, 27);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(332, 126);
            button1.TabIndex = 0;
            button1.Text = "Select a Directory";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Enabled = false;
            button2.Location = new Point(402, 27);
            button2.Name = "button2";
            button2.Size = new Size(332, 131);
            button2.TabIndex = 1;
            button2.Text = "Show File Map";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(32, 189);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(2502, 1167);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2579, 1406);
            Controls.Add(richTextBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Margin = new Padding(4);
            MaximizeBox = false;
            Name = "Form1";
            Text = "File Map";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private FolderBrowserDialog folderBrowserDialog1;
        private Button button2;
        private RichTextBox richTextBox1;
    }
}