using System;

namespace DSEmbeddingExample {
    partial class MainWindow {

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && ( components != null )) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ExecuteSourceButton_Click(object sender, EventArgs e) {
            this.ExecuteSource();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.ExecuteSourceButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ExecutionResult_StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.SourceTextBox = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExecuteSourceButton
            // 
            this.ExecuteSourceButton.Location = new System.Drawing.Point(12, 203);
            this.ExecuteSourceButton.Name = "ExecuteSourceButton";
            this.ExecuteSourceButton.Size = new System.Drawing.Size(75, 23);
            this.ExecuteSourceButton.TabIndex = 0;
            this.ExecuteSourceButton.Text = "&Execute";
            this.ExecuteSourceButton.UseVisualStyleBackColor = true;
            this.ExecuteSourceButton.Click += new System.EventHandler(this.ExecuteSourceButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExecutionResult_StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 238);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(534, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ExecutionResult_StatusLabel
            // 
            this.ExecutionResult_StatusLabel.Name = "ExecutionResult_StatusLabel";
            this.ExecutionResult_StatusLabel.Size = new System.Drawing.Size(114, 17);
            this.ExecutionResult_StatusLabel.Text = "<< not executed >>";
            // 
            // SourceTextBox
            // 
            this.SourceTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SourceTextBox.Location = new System.Drawing.Point(12, 12);
            this.SourceTextBox.Multiline = true;
            this.SourceTextBox.Name = "SourceTextBox";
            this.SourceTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.SourceTextBox.Size = new System.Drawing.Size(508, 185);
            this.SourceTextBox.TabIndex = 3;
            this.SourceTextBox.Text = resources.GetString("SourceTextBox.Text");
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 260);
            this.Controls.Add(this.SourceTextBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ExecuteSourceButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.Text = "DocScript Embedding - Example";
            this.TopMost = true;
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ExecuteSourceButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox SourceTextBox;
        private System.Windows.Forms.ToolStripStatusLabel ExecutionResult_StatusLabel;
    }

}