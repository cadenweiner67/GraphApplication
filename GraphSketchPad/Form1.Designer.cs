
using System.Drawing;

namespace GraphSketchPad
{
    /// <summary>
    /// This is the class of forms.
    /// </summary>
    partial class Form1
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.Algorithm = new System.Windows.Forms.ComboBox();
            this.VerticesDisplay = new System.Windows.Forms.TextBox();
            this.EdgesDisplay = new System.Windows.Forms.TextBox();
            this.ComponentsDisplay = new System.Windows.Forms.TextBox();
            this.RunButton = new System.Windows.Forms.Button();
            this.ColorComboBox = new System.Windows.Forms.ComboBox();
            this.SourceBox = new System.Windows.Forms.TextBox();
            this.TargetBox = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Identify = new System.Windows.Forms.TextBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.EdgeSettings = new System.Windows.Forms.ComboBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.Canvas.BackColor = System.Drawing.Color.Black;
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Canvas.Image = ((System.Drawing.Image)(resources.GetObject("Canvas.Image")));
            this.Canvas.Location = new System.Drawing.Point(0, 0);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(1924, 450);
            this.Canvas.TabIndex = 0;
            this.Canvas.TabStop = false;
            this.Canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.Canvas_Paint);
            this.Canvas.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDoubleClick);
            this.Canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDown);
            this.Canvas.MouseHover += new System.EventHandler(this.Canvas_MouseHover);
            this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            this.Canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseUp);
            // 
            // Algorithm
            // 
            this.Algorithm.FormattingEnabled = true;
            this.Algorithm.Location = new System.Drawing.Point(942, -3);
            this.Algorithm.Name = "Algorithm";
            this.Algorithm.Size = new System.Drawing.Size(156, 28);
            this.Algorithm.TabIndex = 4;
            this.Algorithm.SelectedIndexChanged += new System.EventHandler(this.Algorithm_SelectedIndexChanged);
            // 
            // VerticesDisplay
            // 
            this.VerticesDisplay.Location = new System.Drawing.Point(131, -1);
            this.VerticesDisplay.Name = "VerticesDisplay";
            this.VerticesDisplay.Size = new System.Drawing.Size(100, 26);
            this.VerticesDisplay.TabIndex = 5;
            this.VerticesDisplay.Tag = "Vertices: ";
            // 
            // EdgesDisplay
            // 
            this.EdgesDisplay.Location = new System.Drawing.Point(237, -1);
            this.EdgesDisplay.Name = "EdgesDisplay";
            this.EdgesDisplay.Size = new System.Drawing.Size(100, 26);
            this.EdgesDisplay.TabIndex = 6;
            this.EdgesDisplay.Tag = "Edges: ";
            // 
            // ComponentsDisplay
            // 
            this.ComponentsDisplay.Location = new System.Drawing.Point(0, -1);
            this.ComponentsDisplay.Name = "ComponentsDisplay";
            this.ComponentsDisplay.Size = new System.Drawing.Size(125, 26);
            this.ComponentsDisplay.TabIndex = 7;
            this.ComponentsDisplay.Tag = "Components: ";
            // 
            // RunButton
            // 
            this.RunButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.RunButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RunButton.Location = new System.Drawing.Point(639, -3);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(297, 59);
            this.RunButton.TabIndex = 8;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = false;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // ColorComboBox
            // 
            this.ColorComboBox.FormattingEnabled = true;
            this.ColorComboBox.Location = new System.Drawing.Point(343, 0);
            this.ColorComboBox.Name = "ColorComboBox";
            this.ColorComboBox.Size = new System.Drawing.Size(142, 28);
            this.ColorComboBox.TabIndex = 9;
            // 
            // SourceBox
            // 
            this.SourceBox.Location = new System.Drawing.Point(1134, -1);
            this.SourceBox.Name = "SourceBox";
            this.SourceBox.Size = new System.Drawing.Size(50, 26);
            this.SourceBox.TabIndex = 10;
            this.SourceBox.TextChanged += new System.EventHandler(this.SourceBox_TextChanged);
            // 
            // TargetBox
            // 
            this.TargetBox.Location = new System.Drawing.Point(1220, 0);
            this.TargetBox.Name = "TargetBox";
            this.TargetBox.Size = new System.Drawing.Size(53, 26);
            this.TargetBox.TabIndex = 11;
            this.TargetBox.TextChanged += new System.EventHandler(this.TargetBox_TextChanged);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBox1.Location = new System.Drawing.Point(1104, -1);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(24, 26);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "S:";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBox2.Location = new System.Drawing.Point(1190, 0);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(24, 26);
            this.textBox2.TabIndex = 13;
            this.textBox2.Text = "T:";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // Identify
            // 
            this.Identify.Location = new System.Drawing.Point(1372, -1);
            this.Identify.Name = "Identify";
            this.Identify.Size = new System.Drawing.Size(160, 26);
            this.Identify.TabIndex = 14;
            this.Identify.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBox3.Location = new System.Drawing.Point(1279, -1);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(87, 26);
            this.textBox3.TabIndex = 16;
            this.textBox3.Text = "Vertex Set: ";
            // 
            // EdgeSettings
            // 
            this.EdgeSettings.FormattingEnabled = true;
            this.EdgeSettings.Location = new System.Drawing.Point(491, -1);
            this.EdgeSettings.Name = "EdgeSettings";
            this.EdgeSettings.Size = new System.Drawing.Size(142, 28);
            this.EdgeSettings.TabIndex = 17;
            this.EdgeSettings.SelectedIndexChanged += new System.EventHandler(this.EdgeSettings_SelectedIndexChanged);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(1527, -1);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(390, 367);
            this.textBox4.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 450);
            this.Controls.Add(this.EdgeSettings);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.Identify);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.TargetBox);
            this.Controls.Add(this.SourceBox);
            this.Controls.Add(this.ColorComboBox);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.ComponentsDisplay);
            this.Controls.Add(this.EdgesDisplay);
            this.Controls.Add(this.VerticesDisplay);
            this.Controls.Add(this.Algorithm);
            this.Controls.Add(this.Canvas);
            this.Controls.Add(this.textBox4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Canvas;
        private System.Windows.Forms.ComboBox Algorithm;
        private System.Windows.Forms.TextBox VerticesDisplay;
        private System.Windows.Forms.TextBox EdgesDisplay;
        private System.Windows.Forms.TextBox ComponentsDisplay;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.ComboBox ColorComboBox;
        private System.Windows.Forms.TextBox SourceBox;
        private System.Windows.Forms.TextBox TargetBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox Identify;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.ComboBox EdgeSettings;
        private System.Windows.Forms.TextBox textBox4;
    }
}

