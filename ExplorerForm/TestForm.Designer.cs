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
            buttonTopMost = new Button();
            buttonVisibleTop = new Button();
            comboBoxWindowIDs = new ComboBox();
            buttonNoTopMost = new Button();
            SuspendLayout();
            // 
            // buttonStart
            // 
            buttonStart.Location = new Point(12, 67);
            buttonStart.Name = "buttonStart";
            buttonStart.Size = new Size(171, 23);
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
            // buttonTopMost
            // 
            buttonTopMost.Location = new Point(12, 96);
            buttonTopMost.Name = "buttonTopMost";
            buttonTopMost.Size = new Size(88, 23);
            buttonTopMost.TabIndex = 5;
            buttonTopMost.Text = "TopMost";
            buttonTopMost.UseVisualStyleBackColor = true;
            buttonTopMost.Click += buttonTopMost_Click;
            // 
            // buttonVisibleTop
            // 
            buttonVisibleTop.Location = new Point(12, 125);
            buttonVisibleTop.Name = "buttonVisibleTop";
            buttonVisibleTop.Size = new Size(171, 23);
            buttonVisibleTop.TabIndex = 6;
            buttonVisibleTop.Text = "Foreground Window";
            buttonVisibleTop.UseVisualStyleBackColor = true;
            buttonVisibleTop.Click += buttonVisibleTop_Click;
            // 
            // comboBoxWindowIDs
            // 
            comboBoxWindowIDs.FormattingEnabled = true;
            comboBoxWindowIDs.Location = new Point(189, 68);
            comboBoxWindowIDs.Name = "comboBoxWindowIDs";
            comboBoxWindowIDs.Size = new Size(137, 23);
            comboBoxWindowIDs.TabIndex = 7;
            // 
            // buttonNoTopMost
            // 
            buttonNoTopMost.Location = new Point(106, 96);
            buttonNoTopMost.Name = "buttonNoTopMost";
            buttonNoTopMost.Size = new Size(77, 23);
            buttonNoTopMost.TabIndex = 8;
            buttonNoTopMost.Text = "UnTopMost";
            buttonNoTopMost.UseVisualStyleBackColor = true;
            buttonNoTopMost.Click += buttonNoTopMost_Click;
            // 
            // TestForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(342, 159);
            Controls.Add(buttonNoTopMost);
            Controls.Add(comboBoxWindowIDs);
            Controls.Add(buttonVisibleTop);
            Controls.Add(buttonTopMost);
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
        private Button buttonTopMost;
        private Button buttonVisibleTop;
        private ComboBox comboBoxWindowIDs;
        private Button buttonNoTopMost;
    }
}
