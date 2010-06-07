namespace Sem.Sync.Connector.Statistic
{
    using System.Windows.Forms;

    public partial class ConfigurationEditor : Form
    {
        public ConfigurationEditor()
        {
            InitializeComponent();
        }

        public string GroupingPropertNyme
        {
            get
            {
                return cboGroupingProperty.Text;
            }

            set
            {
                cboGroupingProperty.Text = value;
            }
        }
    }
}
