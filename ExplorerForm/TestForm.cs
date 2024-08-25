using ExplorerManager;

namespace ExplorerForm
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            UpdateEnabledButtons();
        }

        private List<ExplorerProcessManager> explorerProcessManagers = new List<ExplorerProcessManager>();

        private void buttonStart_Click(object sender, EventArgs e)
        {
            var explorerProcessManager = new ExplorerProcessManager();
            explorerProcessManagers.Add(explorerProcessManager);
            explorerProcessManager.OnExplorerClosed += (hWnd) =>
            {
                UpdateText(labelStatus, $"Close({hWnd})");
                explorerProcessManagers.Remove(explorerProcessManager);
                UpdateEnabledButtons();
            };

            var explorer = explorerProcessManager.StartExplorer();
            if (explorer != IntPtr.Zero)
            {
                UpdateText(labelStatus, $"New explorer.exe window found with handle: {explorer}");
            }
            else
            {
                UpdateText(labelStatus, "No new explorer.exe window was found.");
            }

            UpdateEnabledButtons();
        }

        private void buttonTopMost_Click(object sender, EventArgs e)
        {
            if (explorerProcessManagers.Count > 0)
            {
                explorerProcessManagers.Last().SetTopMost();
            }
        }

        private void buttonVisibleTop_Click(object sender, EventArgs e)
        {
            if (explorerProcessManagers.Count > 0)
            {
                explorerProcessManagers.Last().SetWindowTopForMoment();
            }
        }

        private void UpdateEnabledButtons()
        {
            bool isEnable = explorerProcessManagers.Count > 0;
            UpdateEnabled(buttonTopMost, isEnable);
            UpdateEnabled(buttonVisibleTop, isEnable);
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
        }

        #endregion

    }
}
