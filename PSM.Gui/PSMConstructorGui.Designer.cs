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
            SMImageBox = new PictureBox();
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
            menuStrip1 = new MenuStrip();
            importToolStripMenuItem = new ToolStripMenuItem();
            exportToolStripMenuItem = new ToolStripMenuItem();
            mucalculusToolStripMenuItem = new ToolStripMenuItem();
            laTeXToolStripMenuItem = new ToolStripMenuItem();
            viewTabControl = new TabControl();
            smTab = new TabPage();
            dnlTab = new TabPage();
            dnlLabel = new RichTextBox();
            variableOptionsTree = new TreeView();
            ((System.ComponentModel.ISupportInitialize)SMImageBox).BeginInit();
            templateBox.SuspendLayout();
            eventBox.SuspendLayout();
            optionsBox.SuspendLayout();
            menuStrip1.SuspendLayout();
            viewTabControl.SuspendLayout();
            smTab.SuspendLayout();
            dnlTab.SuspendLayout();
            SuspendLayout();
            // 
            // SMImageBox
            // 
            SMImageBox.BackColor = Color.White;
            SMImageBox.Location = new Point(2, 2);
            SMImageBox.Margin = new Padding(2);
            SMImageBox.Name = "SMImageBox";
            SMImageBox.Size = new Size(621, 626);
            SMImageBox.TabIndex = 0;
            SMImageBox.TabStop = false;
            SMImageBox.Click += SMImageBox_Click;
            // 
            // templateBox
            // 
            templateBox.Controls.Add(behaviourLabel);
            templateBox.Controls.Add(scopeLabel);
            templateBox.Controls.Add(behaviourComboBox);
            templateBox.Controls.Add(scopeComboBox);
            templateBox.Enabled = false;
            templateBox.Location = new Point(10, 38);
            templateBox.Margin = new Padding(2);
            templateBox.Name = "templateBox";
            templateBox.Padding = new Padding(2);
            templateBox.Size = new Size(406, 91);
            templateBox.TabIndex = 1;
            templateBox.TabStop = false;
            templateBox.Text = "Template";
            // 
            // behaviourLabel
            // 
            behaviourLabel.AutoSize = true;
            behaviourLabel.Location = new Point(5, 58);
            behaviourLabel.Margin = new Padding(2, 0, 2, 0);
            behaviourLabel.Name = "behaviourLabel";
            behaviourLabel.Size = new Size(75, 20);
            behaviourLabel.TabIndex = 5;
            behaviourLabel.Text = "Behaviour";
            // 
            // scopeLabel
            // 
            scopeLabel.AutoSize = true;
            scopeLabel.Location = new Point(5, 26);
            scopeLabel.Margin = new Padding(2, 0, 2, 0);
            scopeLabel.Name = "scopeLabel";
            scopeLabel.Size = new Size(50, 20);
            scopeLabel.TabIndex = 4;
            scopeLabel.Text = "Scope";
            // 
            // behaviourComboBox
            // 
            behaviourComboBox.FormattingEnabled = true;
            behaviourComboBox.Location = new Point(90, 55);
            behaviourComboBox.Margin = new Padding(2);
            behaviourComboBox.Name = "behaviourComboBox";
            behaviourComboBox.Size = new Size(314, 28);
            behaviourComboBox.TabIndex = 1;
            behaviourComboBox.SelectedIndexChanged += behaviourComboBox_SelectedIndexChanged;
            // 
            // scopeComboBox
            // 
            scopeComboBox.FormattingEnabled = true;
            scopeComboBox.Location = new Point(90, 24);
            scopeComboBox.Margin = new Padding(2);
            scopeComboBox.Name = "scopeComboBox";
            scopeComboBox.Size = new Size(314, 28);
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
            eventBox.Enabled = false;
            eventBox.Location = new Point(14, 134);
            eventBox.Margin = new Padding(2);
            eventBox.Name = "eventBox";
            eventBox.Padding = new Padding(2);
            eventBox.Size = new Size(402, 151);
            eventBox.TabIndex = 2;
            eventBox.TabStop = false;
            eventBox.Text = "Events";
            // 
            // endTextBox
            // 
            endTextBox.Enabled = false;
            endTextBox.Location = new Point(58, 114);
            endTextBox.Margin = new Padding(2);
            endTextBox.Name = "endTextBox";
            endTextBox.Size = new Size(342, 27);
            endTextBox.TabIndex = 16;
            endTextBox.Text = "END";
            endTextBox.Enter += updateLastFocusedTextBox;
            endTextBox.MouseDown += ClearTextbox;
            // 
            // startTextBox
            // 
            startTextBox.Enabled = false;
            startTextBox.Location = new Point(58, 83);
            startTextBox.Margin = new Padding(2);
            startTextBox.Name = "startTextBox";
            startTextBox.Size = new Size(342, 27);
            startTextBox.TabIndex = 15;
            startTextBox.Text = "START";
            startTextBox.Enter += updateLastFocusedTextBox;
            startTextBox.MouseDown += ClearTextbox;
            // 
            // bTextBox
            // 
            bTextBox.Enabled = false;
            bTextBox.Location = new Point(58, 52);
            bTextBox.Margin = new Padding(2);
            bTextBox.Name = "bTextBox";
            bTextBox.Size = new Size(342, 27);
            bTextBox.TabIndex = 14;
            bTextBox.Text = "B";
            bTextBox.Enter += updateLastFocusedTextBox;
            bTextBox.MouseDown += ClearTextbox;
            // 
            // aTextBox
            // 
            aTextBox.Location = new Point(58, 21);
            aTextBox.Margin = new Padding(2);
            aTextBox.Name = "aTextBox";
            aTextBox.Size = new Size(342, 27);
            aTextBox.TabIndex = 6;
            aTextBox.Text = "A";
            aTextBox.Enter += updateLastFocusedTextBox;
            aTextBox.MouseDown += ClearTextbox;
            // 
            // endLabel
            // 
            endLabel.AutoSize = true;
            endLabel.Location = new Point(5, 117);
            endLabel.Margin = new Padding(2, 0, 2, 0);
            endLabel.Name = "endLabel";
            endLabel.Size = new Size(39, 20);
            endLabel.TabIndex = 13;
            endLabel.Text = "END";
            // 
            // startLabel
            // 
            startLabel.AutoSize = true;
            startLabel.Location = new Point(5, 86);
            startLabel.Margin = new Padding(2, 0, 2, 0);
            startLabel.Name = "startLabel";
            startLabel.Size = new Size(50, 20);
            startLabel.TabIndex = 12;
            startLabel.Text = "START";
            // 
            // bLabel
            // 
            bLabel.AutoSize = true;
            bLabel.Location = new Point(5, 54);
            bLabel.Margin = new Padding(2, 0, 2, 0);
            bLabel.Name = "bLabel";
            bLabel.Size = new Size(18, 20);
            bLabel.TabIndex = 9;
            bLabel.Text = "B";
            // 
            // aLabel
            // 
            aLabel.AutoSize = true;
            aLabel.Location = new Point(5, 23);
            aLabel.Margin = new Padding(2, 0, 2, 0);
            aLabel.Name = "aLabel";
            aLabel.Size = new Size(19, 20);
            aLabel.TabIndex = 8;
            aLabel.Text = "A";
            // 
            // optionsBox
            // 
            optionsBox.Controls.Add(optionsList);
            optionsBox.Enabled = false;
            optionsBox.Location = new Point(10, 294);
            optionsBox.Margin = new Padding(2);
            optionsBox.Name = "optionsBox";
            optionsBox.Padding = new Padding(2);
            optionsBox.Size = new Size(406, 409);
            optionsBox.TabIndex = 3;
            optionsBox.TabStop = false;
            optionsBox.Text = "Options";
            // 
            // optionsList
            // 
            optionsList.FormattingEnabled = true;
            optionsList.Location = new Point(10, 24);
            optionsList.Margin = new Padding(2);
            optionsList.Name = "optionsList";
            optionsList.Size = new Size(394, 378);
            optionsList.TabIndex = 4;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.ControlLight;
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { importToolStripMenuItem, exportToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1371, 28);
            menuStrip1.TabIndex = 6;
            menuStrip1.Text = "menuStrip";
            // 
            // importToolStripMenuItem
            // 
            importToolStripMenuItem.Name = "importToolStripMenuItem";
            importToolStripMenuItem.Size = new Size(68, 24);
            importToolStripMenuItem.Text = "Import";
            importToolStripMenuItem.Click += importToolStripMenuItem_Click;
            // 
            // exportToolStripMenuItem
            // 
            exportToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mucalculusToolStripMenuItem, laTeXToolStripMenuItem });
            exportToolStripMenuItem.Enabled = false;
            exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            exportToolStripMenuItem.Size = new Size(66, 24);
            exportToolStripMenuItem.Text = "Export";
            // 
            // mucalculusToolStripMenuItem
            // 
            mucalculusToolStripMenuItem.Name = "mucalculusToolStripMenuItem";
            mucalculusToolStripMenuItem.Size = new Size(171, 26);
            mucalculusToolStripMenuItem.Text = "mu-calculus";
            mucalculusToolStripMenuItem.Click += mucalculusToolStripMenuItem_Click;
            // 
            // laTeXToolStripMenuItem
            // 
            laTeXToolStripMenuItem.Name = "laTeXToolStripMenuItem";
            laTeXToolStripMenuItem.Size = new Size(171, 26);
            laTeXToolStripMenuItem.Text = "LaTeX";
            laTeXToolStripMenuItem.Click += laTeXToolStripMenuItem_Click;
            // 
            // viewTabControl
            // 
            viewTabControl.Controls.Add(smTab);
            viewTabControl.Controls.Add(dnlTab);
            viewTabControl.Enabled = false;
            viewTabControl.Location = new Point(421, 38);
            viewTabControl.Name = "viewTabControl";
            viewTabControl.SelectedIndex = 0;
            viewTabControl.Size = new Size(633, 665);
            viewTabControl.TabIndex = 8;
            // 
            // smTab
            // 
            smTab.Controls.Add(SMImageBox);
            smTab.Location = new Point(4, 29);
            smTab.Name = "smTab";
            smTab.Padding = new Padding(3, 3, 3, 3);
            smTab.Size = new Size(625, 632);
            smTab.TabIndex = 0;
            smTab.Text = "State Machine";
            smTab.UseVisualStyleBackColor = true;
            // 
            // dnlTab
            // 
            dnlTab.Controls.Add(dnlLabel);
            dnlTab.Location = new Point(4, 29);
            dnlTab.Name = "dnlTab";
            dnlTab.Padding = new Padding(3, 3, 3, 3);
            dnlTab.Size = new Size(625, 483);
            dnlTab.TabIndex = 1;
            dnlTab.Text = "DNL";
            dnlTab.UseVisualStyleBackColor = true;
            // 
            // dnlLabel
            // 
            dnlLabel.Location = new Point(0, 0);
            dnlLabel.Name = "dnlLabel";
            dnlLabel.Size = new Size(625, 483);
            dnlLabel.TabIndex = 0;
            dnlLabel.Text = "";
            dnlLabel.Click += dnlLabel_Click;
            // 
            // variableOptionsTree
            // 
            variableOptionsTree.Location = new Point(1056, 67);
            variableOptionsTree.Name = "variableOptionsTree";
            variableOptionsTree.Size = new Size(309, 632);
            variableOptionsTree.TabIndex = 9;
            variableOptionsTree.AfterSelect += variableOptionsTree_AfterSelect;
            // 
            // PSMConstructorGui
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1371, 706);
            Controls.Add(variableOptionsTree);
            Controls.Add(viewTabControl);
            Controls.Add(optionsBox);
            Controls.Add(eventBox);
            Controls.Add(templateBox);
            Controls.Add(menuStrip1);
            Margin = new Padding(2);
            Name = "PSMConstructorGui";
            Text = "PSM";
            ((System.ComponentModel.ISupportInitialize)SMImageBox).EndInit();
            templateBox.ResumeLayout(false);
            templateBox.PerformLayout();
            eventBox.ResumeLayout(false);
            eventBox.PerformLayout();
            optionsBox.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            viewTabControl.ResumeLayout(false);
            smTab.ResumeLayout(false);
            dnlTab.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox SMImageBox;
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
        private TextBox endTextBox;
        private TextBox startTextBox;
        private TextBox bTextBox;
        private TextBox aTextBox;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem importToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem mucalculusToolStripMenuItem;
        private ToolStripMenuItem laTeXToolStripMenuItem;
        private TabControl viewTabControl;
        private TabPage smTab;
        private TabPage dnlTab;
        private TreeView variableOptionsTree;
        private RichTextBox dnlLabel;
    }
}
