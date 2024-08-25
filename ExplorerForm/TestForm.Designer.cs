namespace ExplorerForm
{
    partial class TestForm
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
            buttonStart = new Button();
            labelStatusText = new Label();
            labelStatus = new Label();
            labelPathText = new Label();
            labelPath = new Label();
            buttonTopMost = new Button();
            buttonVisibleTop = new Button();
            SuspendLayout();
            // 
            // buttonStart
            // 
            buttonStart.Location = new Point(12, 123);
            buttonStart.Name = "buttonStart";
            buttonStart.Size = new Size(151, 23);
            buttonStart.TabIndex = 0;
            buttonStart.Text = "Run Explorer";
            buttonStart.UseVisualStyleBackColor = true;
            buttonStart.Click += buttonStart_Click;
            // 
            // labelStatusText
            // 
            labelStatusText.AutoSize = true;
            labelStatusText.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            labelStatusText.Location = new Point(12, 9);
            labelStatusText.Name = "labelStatusText";
            labelStatusText.Size = new Size(44, 15);
            labelStatusText.TabIndex = 1;
            labelStatusText.Text = "Status";
            // 
            // labelStatus
            // 
            labelStatus.AutoSize = true;
            labelStatus.Location = new Point(12, 37);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(24, 15);
            labelStatus.TabIndex = 2;
            labelStatus.Text = "Off";
            // 
            // labelPathText
            // 
            labelPathText.AutoSize = true;
            labelPathText.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            labelPathText.Location = new Point(12, 68);
            labelPathText.Name = "labelPathText";
            labelPathText.Size = new Size(33, 15);
            labelPathText.TabIndex = 3;
            labelPathText.Text = "Path";
            // 
            // labelPath
            // 
            labelPath.AutoSize = true;
            labelPath.Location = new Point(12, 96);
            labelPath.Name = "labelPath";
            labelPath.Size = new Size(0, 15);
            labelPath.TabIndex = 4;
            // 
            // buttonTopMost
            // 
            buttonTopMost.Location = new Point(12, 152);
            buttonTopMost.Name = "buttonTopMost";
            buttonTopMost.Size = new Size(151, 23);
            buttonTopMost.TabIndex = 5;
            buttonTopMost.Text = "TopMost";
            buttonTopMost.UseVisualStyleBackColor = true;
            buttonTopMost.Click += buttonTopMost_Click;
            // 
            // buttonVisibleTop
            // 
            buttonVisibleTop.Location = new Point(12, 181);
            buttonVisibleTop.Name = "buttonVisibleTop";
            buttonVisibleTop.Size = new Size(151, 23);
            buttonVisibleTop.TabIndex = 6;
            buttonVisibleTop.Text = "Visible Top";
            buttonVisibleTop.UseVisualStyleBackColor = true;
            buttonVisibleTop.Click += buttonVisibleTop_Click;
            // 
            // TestForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(405, 218);
            Controls.Add(buttonVisibleTop);
            Controls.Add(buttonTopMost);
            Controls.Add(labelPath);
            Controls.Add(labelPathText);
            Controls.Add(labelStatus);
            Controls.Add(labelStatusText);
            Controls.Add(buttonStart);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Name = "TestForm";
            Text = "Test";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonStart;
        private Label labelStatusText;
        private Label labelStatus;
        private Label labelPathText;
        private Label labelPath;
        private Button buttonTopMost;
        private Button buttonVisibleTop;
    }
}
