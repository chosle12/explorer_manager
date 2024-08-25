using ExplorerManager;

namespace ExplorerForm
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            UpdateEnableds();
        }

        private Dictionary<IntPtr, ExplorerProcessManager> explorerProcessManagers = new Dictionary<IntPtr, ExplorerProcessManager>();

        private void buttonStart_Click(object sender, EventArgs e)
        {
            var explorerProcessManager = new ExplorerProcessManager();
            explorerProcessManager.OnExplorerClosed += (hWnd) =>
            {
                UpdateText(labelStatus, $"Close({hWnd})");
                explorerProcessManagers.Remove(hWnd);
                comboBoxWindowIDs.Items.Remove(hWnd);
                UpdateEnableds();
            };

            var explorer = explorerProcessManager.StartExplorer();
            if (explorer != IntPtr.Zero)
            {
                explorerProcessManagers.Add(explorer, explorerProcessManager);
                comboBoxWindowIDs.Items.Add(explorer);
                comboBoxWindowIDs.SelectedItem = explorer;
                UpdateText(labelStatus, $"New explorer.exe window found with handle: {explorer}");
            }
            else
            {
                UpdateText(labelStatus, "No new explorer.exe window was found.");
            }

            UpdateEnableds();
        }

        private void buttonTopMost_Click(object sender, EventArgs e)
        {
            if (comboBoxWindowIDs.SelectedItem is IntPtr hWnd)
            {
                if (explorerProcessManagers.TryGetValue(hWnd, out var explorerProcessManager))
                {
                    explorerProcessManager.SetTopMost();
                }
            }
        }

        private void buttonNoTopMost_Click(object sender, EventArgs e)
        {
            if (comboBoxWindowIDs.SelectedItem is IntPtr hWnd)
            {
                if (explorerProcessManagers.TryGetValue(hWnd, out var explorerProcessManager))
                {
                    explorerProcessManager.SetNoTopMost();
                }
            }
        }

        private void buttonVisibleTop_Click(object sender, EventArgs e)
        {
            if (comboBoxWindowIDs.SelectedItem is IntPtr hWnd)
            {
                if (explorerProcessManagers.TryGetValue(hWnd, out var explorerProcessManager))
                {
                    explorerProcessManager.SetForegroundWindow();
                }
            }
        }

        private void UpdateEnableds()
        {
            bool isEnable = explorerProcessManagers.Count > 0;
            UpdateEnabled(buttonTopMost, isEnable);
            UpdateEnabled(buttonVisibleTop, isEnable);
            UpdateEnabled(buttonNoTopMost, isEnable);
            UpdateEnabled(comboBoxWindowIDs, isEnable);
        }

        #region thread safe
        private void UpdateText(object s, string text)
        {
            if (s is Label label)
            {
                if (label.InvokeRequired)
                {
                    label.Invoke(new Action<object, string>(UpdateText), s, text);
                }
                else
                {
                    label.Text = text;
                }
            }
        }

        private void UpdateEnabled(object s, bool enabled)
        {
            if (s is Button button)
            {
                if (button.InvokeRequired)
                {
                    button.Invoke(new Action<object, bool>(UpdateEnabled), s, enabled);
                }
                else
                {
                    button.Enabled = enabled;
                }
            }
            else if (s is ComboBox comboBox)
            {
                if (comboBox.InvokeRequired)
                {
                    comboBox.Invoke(new Action<object, bool>(UpdateEnabled), s, enabled);
                }
                else
                {
                    comboBox.Enabled = enabled;
                }
            }
        }

        #endregion
    }
}
