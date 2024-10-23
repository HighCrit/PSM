using PSM.Common;
using PSM.Common.MuCalc.ModalFormula;
using PSM.Common.PROPEL;
using PSM.Constructors.PROPEL2MuCalc;
using PSM.Constructors.SM2DOT;

namespace PSM.Gui
{
    public partial class PSMConstructorGui : Form
    {
        private Scope lastScope;
        private Behaviour lastBehaviour;
        private Option lastOption;
        private PSMFactory? psmFactory;

        private TextBox lastTextBox;

        public PSMConstructorGui(string path) : this()
        {
            this.SetFactory(path);
        }

        public PSMConstructorGui()
        {
            this.InitializeComponent();
            this.SetupChoices();

            this.lastTextBox = this.aTextBox;
        }

        public void SetupChoices()
        {
            this.scopeComboBox.DataSource = Enum.GetValues<Scope>();
            this.behaviourComboBox.DataSource = Enum.GetValues<Behaviour>();

            this.optionsList.ItemCheck += (s, e) =>
            {
                var selectedOption = (Option)this.optionsList.Items[e.Index];
                if (e.CurrentValue is CheckState.Indeterminate)
                {
                    e.NewValue = CheckState.Indeterminate;
                    this.lastOption |= selectedOption;
                }
                else if (e.NewValue is CheckState.Checked)
                {
                    this.lastOption |= selectedOption;
                }
                else
                {
                    this.lastOption &= ~selectedOption;
                }
            };

            this.UpdateScope();
            this.UpdateBehaviour();
        }

        public void UpdateScope()
        {
            var selectedScope = (Scope)this.scopeComboBox.SelectedItem!;
            if (selectedScope == this.lastScope)
            {
                return;
            }

            this.lastScope = selectedScope;

            this.startTextBox.Enabled = selectedScope is Scope.After or Scope.Between;
            this.endTextBox.Enabled = selectedScope is Scope.Before or Scope.Between;
            this.UpdateOptions();
        }

        public void UpdateBehaviour()
        {
            var selectedBehaviour = (Behaviour)this.behaviourComboBox.SelectedItem!;
            if (selectedBehaviour == this.lastBehaviour)
            {
                return;
            }

            this.lastBehaviour = selectedBehaviour;

            this.bTextBox.Enabled = selectedBehaviour is Behaviour.Precedence or Behaviour.Response;
            this.UpdateOptions();
        }

        public void UpdateOptions()
        {
            var options = (this.lastScope, this.lastBehaviour).GetOptions();

            var newOption = Option.None;
            this.optionsList.Items.Clear();
            foreach (var option in options)
            {
                var state = CheckState.Unchecked;
                if (option is Option.ScopeRepeatability or Option.PreArity or Option.PostArity)
                {
                    newOption |= option;
                    state = CheckState.Indeterminate;
                }
                else if (option is Option.Immediacy)
                {
                    state = CheckState.Indeterminate;
                }
                else if (this.lastOption.HasFlag(option))
                {
                    newOption |= option;
                    state = CheckState.Checked;
                }

                this.optionsList.Items.Add(option, state);
            }

            this.lastOption = newOption;
        }

        private void updateLastFocusedTextBox(object sender, EventArgs e)
        {
            this.lastTextBox = (TextBox)sender;
        }

        private void scopeComboBox_SelectedIndexChanged(object sender, EventArgs e) => this.UpdateScope();

        private void behaviourComboBox_SelectedIndexChanged(object sender, EventArgs e) => this.UpdateBehaviour();

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                InitialDirectory = "%userprofile%",
                Filter = "eXtensible Markup Language files (*.xml)|*.xml",
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() is DialogResult.OK)
            {
                var path = dialog.FileName;

                this.SetFactory(path);
            }
        }

        private void SetFactory(string path)
        {
            psmFactory = new PSMFactory(path);

            templateBox.Enabled = true;
            eventBox.Enabled = true;
            exportToolStripMenuItem.Enabled = true;
            viewTabControl.Enabled = true;
            optionsBox.Enabled = true;

            PopulateTreeView(variableOptionsTree, this.psmFactory.Atlas.Keys, '.');
        }

        private void mucalculusToolStripMenuItem_Click(object sender, EventArgs e) => this.export(f => f.ToMCRL2());

        private void laTeXToolStripMenuItem_Click(object sender, EventArgs e) => this.export(f => f.ToLatex());

        private void export(Func<IModalFormula, string> appl)
        {
            try
            {
                var formula = psmFactory!.GenerateMuCalcFormula(
                    this.lastBehaviour,
                    this.lastScope,
                    this.lastOption,
                    this.aTextBox.Text,
                    this.bTextBox.Enabled ? this.bTextBox.Text : null,
                    this.startTextBox.Enabled ? this.startTextBox.Text : null,
                    this.endTextBox.Enabled ? this.endTextBox.Text : null);

                Clipboard.SetText(appl(formula));
                MessageBox.Show(
                    "Succesfully copied formula to clipboard.",
                    "Succes",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Failed to generate formula.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
        }

        private void SMImageBox_Click(object sender, EventArgs e)
        {
            this.SMImageBox.Image?.Dispose();

            var labels = new Dictionary<Event, string>();
            foreach (var @event in (this.lastScope, this.lastBehaviour).GetEvents())
            {
                var label = @event switch
                {
                    Event.A => this.aTextBox.Text!,
                    Event.B => this.bTextBox.Text!,
                    Event.Start => this.startTextBox.Text!,
                    Event.End => this.endTextBox.Text!,
                };

                labels.Add(@event, label);
            }

            try
            {
                var sm = SMCatalogue.GetSM(this.lastBehaviour, this.lastScope, this.lastOption, labels);
                var pngPath = SmConverter.ToPNG(sm);

                this.SMImageBox.Image = Image.FromFile(pngPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Failed to generate state-machine.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
        }

        private static void PopulateTreeView(TreeView treeView, IEnumerable<string> paths, char pathSeparator)
        {
            TreeNode lastNode = null;
            string subPathAgg;
            foreach (var path in paths)
            {
                subPathAgg = string.Empty;
                foreach (var subPath in path.Split(pathSeparator))
                {
                    subPathAgg += subPath + pathSeparator;
                    var nodes = treeView.Nodes.Find(subPathAgg, true);
                    if (nodes.Length == 0)
                    {
                        if (lastNode == null)
                        {
                            lastNode = treeView.Nodes.Add(subPathAgg, subPath);
                        }
                        else
                        {
                            lastNode = lastNode.Nodes.Add(subPathAgg, subPath);
                        }
                    }
                    else
                    {
                        lastNode = nodes[0];
                    }
                }
                lastNode = null; // This is the place code was changed
            }
        }

        private void variableOptionsTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var path = e.Node!.FullPath.Replace('\\', '.');
            if (!this.psmFactory!.Atlas.TryGetValue(path, out var info))
            {
                return;
            }

            var addition = info.Type is ModelPropertyType.Command
                ? $"CmdChk({path})"
                : path;

            this.lastTextBox.Text += addition;
        }

        private void dnlLabel_Click(object sender, EventArgs e)
        {
            var dnl = psmFactory!.GetDNL(this.lastBehaviour, this.lastScope, this.lastOption);
            dnlLabel.Text = dnl;
        }
        private void ClearTextbox(object sender, MouseEventArgs e)
        {
            var tb = (TextBox)sender;

            if (e.Button == MouseButtons.Middle)
            {
                tb.Text = string.Empty;
                this.lastTextBox = tb;
            }
        }
    }
}
