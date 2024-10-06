using PSM.Common;
using PSM.Common.MuCalc.ModalFormula;
using PSM.Common.PROPEL;
using PSM.Constructors.PROPEL2MuCalc;
using PSM.Parsers.Labels;
using PSM.Parsers.Labels.Labels;

namespace PSM.Gui
{
    public partial class PSMConstructorGui : Form
    {
        private Scope lastScope;
        private Behaviour lastBehaviour;
        private Option lastOption;

        public PSMConstructorGui()
        {
            this.InitializeComponent();
            this.SetupChoices();
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

            var oldOption = this.lastOption;
            this.lastOption = Option.None;
            this.optionsList.Items.Clear();
            foreach (var option in options)
            {
                var state = CheckState.Unchecked;
                if (option is Option.Immediacy or Option.ScopeRepeatability or Option.PreArity or Option.PostArity)
                {
                    this.lastOption |= option;
                    state = CheckState.Indeterminate;
                }

                if (oldOption.HasFlag(option))
                {
                    this.lastOption |= option;
                    state = CheckState.Checked;
                }

                this.optionsList.Items.Add(option, state);
            }
        }

        private void scopeComboBox_SelectedIndexChanged(object sender, EventArgs e) => this.UpdateScope();

        private void behaviourComboBox_SelectedIndexChanged(object sender, EventArgs e) => this.UpdateBehaviour();

        private void generateBtn_Click(object sender, EventArgs e)
        {
            IModalFormula pattern;
            try
            {
                pattern = PatternCatalogue.GetPattern(this.lastBehaviour, this.lastScope, this.lastOption);
            }
            catch
            {
                MessageBox.Show(
                    "Unable to find pattern for provided scope, behaviour and options.",
                    "Missing pattern",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            var substitutions = new Dictionary<Event, IExpression>();
            foreach (var @event in (this.lastScope, this.lastBehaviour).GetEvents())
            {
                IExpression exp;
                try
                {
                    exp = @event switch
                    {
                        Event.A => LabelParser.Parse(this.aTextBox.Text)!,
                        Event.B => LabelParser.Parse(this.bTextBox.Text)!,
                        Event.Start => LabelParser.Parse(this.startTextBox.Text)!,
                        Event.End => LabelParser.Parse(this.endTextBox.Text)!,
                    };
                }
                catch
                {
                    MessageBox.Show(
                        $"Failed to parse expression for token '{@event}'.",
                        "Failed to parse expression",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }

                substitutions.Add(@event, exp);
            }

            var muCalc = pattern.ApplySubstitutions(substitutions).Flatten().ToMCRL2();
            Clipboard.SetText(muCalc);
            MessageBox.Show(
                "Succesfully copied mu-calculus formula to clipboard.",
                "Succes",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
