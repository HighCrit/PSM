namespace PSM.Gui
{
    partial class PSMConstructorGui
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
            pictureBox1 = new PictureBox();
            templateBox = new GroupBox();
            behaviourLabel = new Label();
            scopeLabel = new Label();
            behaviourComboBox = new ComboBox();
            scopeComboBox = new ComboBox();
            eventBox = new GroupBox();
            endTextBox = new TextBox();
            startTextBox = new TextBox();
            bTextBox = new TextBox();
            aTextBox = new TextBox();
            endLabel = new Label();
            startLabel = new Label();
            bLabel = new Label();
            aLabel = new Label();
            optionsBox = new GroupBox();
            optionsList = new CheckedListBox();
            generateBtn = new Button();
            reloadFSABtn = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            templateBox.SuspendLayout();
            eventBox.SuspendLayout();
            optionsBox.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(337, 25);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(767, 540);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // templateBox
            // 
            templateBox.Controls.Add(behaviourLabel);
            templateBox.Controls.Add(scopeLabel);
            templateBox.Controls.Add(behaviourComboBox);
            templateBox.Controls.Add(scopeComboBox);
            templateBox.Location = new Point(12, 12);
            templateBox.Name = "templateBox";
            templateBox.Size = new Size(300, 114);
            templateBox.TabIndex = 1;
            templateBox.TabStop = false;
            templateBox.Text = "Template";
            // 
            // behaviourLabel
            // 
            behaviourLabel.AutoSize = true;
            behaviourLabel.Location = new Point(6, 72);
            behaviourLabel.Name = "behaviourLabel";
            behaviourLabel.Size = new Size(90, 25);
            behaviourLabel.TabIndex = 5;
            behaviourLabel.Text = "Behaviour";
            // 
            // scopeLabel
            // 
            scopeLabel.AutoSize = true;
            scopeLabel.Location = new Point(6, 33);
            scopeLabel.Name = "scopeLabel";
            scopeLabel.Size = new Size(61, 25);
            scopeLabel.TabIndex = 4;
            scopeLabel.Text = "Scope";
            // 
            // behaviourComboBox
            // 
            behaviourComboBox.FormattingEnabled = true;
            behaviourComboBox.Location = new Point(112, 69);
            behaviourComboBox.Name = "behaviourComboBox";
            behaviourComboBox.Size = new Size(182, 33);
            behaviourComboBox.TabIndex = 1;
            behaviourComboBox.SelectedIndexChanged += behaviourComboBox_SelectedIndexChanged;
            // 
            // scopeComboBox
            // 
            scopeComboBox.FormattingEnabled = true;
            scopeComboBox.Location = new Point(112, 30);
            scopeComboBox.Name = "scopeComboBox";
            scopeComboBox.Size = new Size(182, 33);
            scopeComboBox.TabIndex = 0;
            scopeComboBox.SelectedIndexChanged += scopeComboBox_SelectedIndexChanged;
            // 
            // eventBox
            // 
            eventBox.Controls.Add(endTextBox);
            eventBox.Controls.Add(startTextBox);
            eventBox.Controls.Add(bTextBox);
            eventBox.Controls.Add(aTextBox);
            eventBox.Controls.Add(endLabel);
            eventBox.Controls.Add(startLabel);
            eventBox.Controls.Add(bLabel);
            eventBox.Controls.Add(aLabel);
            eventBox.Location = new Point(18, 132);
            eventBox.Name = "eventBox";
            eventBox.Size = new Size(300, 189);
            eventBox.TabIndex = 2;
            eventBox.TabStop = false;
            eventBox.Text = "Events";
            // 
            // endTextBox
            // 
            endTextBox.Enabled = false;
            endTextBox.Location = new Point(73, 143);
            endTextBox.Name = "endTextBox";
            endTextBox.Size = new Size(215, 31);
            endTextBox.TabIndex = 16;
            endTextBox.Text = "END";
            // 
            // startTextBox
            // 
            startTextBox.Enabled = false;
            startTextBox.Location = new Point(73, 104);
            startTextBox.Name = "startTextBox";
            startTextBox.Size = new Size(215, 31);
            startTextBox.TabIndex = 15;
            startTextBox.Text = "START";
            // 
            // bTextBox
            // 
            bTextBox.Enabled = false;
            bTextBox.Location = new Point(73, 65);
            bTextBox.Name = "bTextBox";
            bTextBox.Size = new Size(215, 31);
            bTextBox.TabIndex = 14;
            bTextBox.Text = "B";
            // 
            // aTextBox
            // 
            aTextBox.Location = new Point(73, 26);
            aTextBox.Name = "aTextBox";
            aTextBox.Size = new Size(215, 31);
            aTextBox.TabIndex = 6;
            aTextBox.Text = "A";
            // 
            // endLabel
            // 
            endLabel.AutoSize = true;
            endLabel.Location = new Point(6, 146);
            endLabel.Name = "endLabel";
            endLabel.Size = new Size(47, 25);
            endLabel.TabIndex = 13;
            endLabel.Text = "END";
            // 
            // startLabel
            // 
            startLabel.AutoSize = true;
            startLabel.Location = new Point(6, 107);
            startLabel.Name = "startLabel";
            startLabel.Size = new Size(61, 25);
            startLabel.TabIndex = 12;
            startLabel.Text = "START";
            // 
            // bLabel
            // 
            bLabel.AutoSize = true;
            bLabel.Location = new Point(6, 68);
            bLabel.Name = "bLabel";
            bLabel.Size = new Size(22, 25);
            bLabel.TabIndex = 9;
            bLabel.Text = "B";
            // 
            // aLabel
            // 
            aLabel.AutoSize = true;
            aLabel.Location = new Point(6, 29);
            aLabel.Name = "aLabel";
            aLabel.Size = new Size(24, 25);
            aLabel.TabIndex = 8;
            aLabel.Text = "A";
            // 
            // optionsBox
            // 
            optionsBox.Controls.Add(optionsList);
            optionsBox.Location = new Point(12, 333);
            optionsBox.Name = "optionsBox";
            optionsBox.Size = new Size(300, 325);
            optionsBox.TabIndex = 3;
            optionsBox.TabStop = false;
            optionsBox.Text = "Options";
            // 
            // optionsList
            // 
            optionsList.FormattingEnabled = true;
            optionsList.Location = new Point(12, 30);
            optionsList.Name = "optionsList";
            optionsList.Size = new Size(282, 284);
            optionsList.TabIndex = 4;
            // 
            // generateBtn
            // 
            generateBtn.Location = new Point(983, 571);
            generateBtn.Name = "generateBtn";
            generateBtn.Size = new Size(121, 87);
            generateBtn.TabIndex = 4;
            generateBtn.Text = "Generate mu-calc";
            generateBtn.UseVisualStyleBackColor = true;
            generateBtn.Click += generateBtn_Click;
            // 
            // reloadFSABtn
            // 
            reloadFSABtn.Location = new Point(337, 571);
            reloadFSABtn.Name = "reloadFSABtn";
            reloadFSABtn.Size = new Size(121, 87);
            reloadFSABtn.TabIndex = 5;
            reloadFSABtn.Text = "Reload FSA";
            reloadFSABtn.UseVisualStyleBackColor = true;
            // 
            // PSMConstructorGui
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1130, 670);
            Controls.Add(reloadFSABtn);
            Controls.Add(generateBtn);
            Controls.Add(optionsBox);
            Controls.Add(eventBox);
            Controls.Add(templateBox);
            Controls.Add(pictureBox1);
            Name = "PSMConstructorGui";
            Text = "PSM";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            templateBox.ResumeLayout(false);
            templateBox.PerformLayout();
            eventBox.ResumeLayout(false);
            eventBox.PerformLayout();
            optionsBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private GroupBox templateBox;
        private GroupBox eventBox;
        private GroupBox optionsBox;
        private Label scopeLabel;
        private ComboBox behaviourComboBox;
        private ComboBox scopeComboBox;
        private Label behaviourLabel;
        private Label endLabel;
        private Label startLabel;
        private Label bLabel;
        private Label aLabel;
        private CheckedListBox optionsList;
        private Button generateBtn;
        private Button reloadFSABtn;
        private TextBox endTextBox;
        private TextBox startTextBox;
        private TextBox bTextBox;
        private TextBox aTextBox;
    }
}
