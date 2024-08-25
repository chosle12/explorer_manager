using ExplorerManager;

namespace ExplorerForm
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            UpdateUIState();
        }

        private readonly Dictionary<IntPtr, ExplorerProcessManager> _explorerProcessManagers = new();

        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartNewExplorer();
        }

        private void buttonTopMost_Click(object sender, EventArgs e)
        {
            SetTopMostForSelectedExplorer();
        }

        private void buttonNoTopMost_Click(object sender, EventArgs e)
        {
            SetNoTopMostForSelectedExplorer();
        }

        private void buttonVisibleTop_Click(object sender, EventArgs e)
        {
            BringSelectedExplorerToForeground();
        }

        private void StartNewExplorer()
        {
            var explorerProcessManager = new ExplorerProcessManager();
            explorerProcessManager.OnExplorerClosed += OnExplorerClosed;

            var explorerHandle = explorerProcessManager.StartExplorer();
            if (explorerHandle != IntPtr.Zero)
            {
                _explorerProcessManagers.Add(explorerHandle, explorerProcessManager);
                comboBoxWindowIDs.Items.Add(explorerHandle);
                comboBoxWindowIDs.SelectedItem = explorerHandle;
                UpdateStatusLabel($"New explorer.exe window found with handle: {explorerHandle}");
            }
            else
            {
                UpdateStatusLabel("No new explorer.exe window was found.");
            }

            UpdateUIState();
        }

        private void OnExplorerClosed(IntPtr hWnd)
        {
            _explorerProcessManagers.Remove(hWnd);
            RemoveItemFromComboBox(comboBoxWindowIDs, hWnd);
            UpdateStatusLabel($"Explorer.exe window closed: {hWnd}");
            UpdateUIState();
        }

        private void SetTopMostForSelectedExplorer()
        {
            if (TryGetSelectedExplorerManager(out var explorerProcessManager))
            {
                explorerProcessManager.SetTopMost();
            }
        }

        private void SetNoTopMostForSelectedExplorer()
        {
            if (TryGetSelectedExplorerManager(out var explorerProcessManager))
            {
                explorerProcessManager.SetNoTopMost();
            }
        }

        private void BringSelectedExplorerToForeground()
        {
            if (TryGetSelectedExplorerManager(out var explorerProcessManager))
            {
                explorerProcessManager.SetForegroundWindow();
            }
        }

        private bool TryGetSelectedExplorerManager(out ExplorerProcessManager explorerProcessManager)
        {
            explorerProcessManager = null;
            if (comboBoxWindowIDs.SelectedItem is IntPtr hWnd)
            {
                return _explorerProcessManagers.TryGetValue(hWnd, out explorerProcessManager);
            }
            return false;
        }

        private void UpdateUIState()
        {
            bool hasExplorerWindows = _explorerProcessManagers.Count > 0;
            SetControlEnabledState(buttonTopMost, hasExplorerWindows);
            SetControlEnabledState(buttonVisibleTop, hasExplorerWindows);
            SetControlEnabledState(buttonNoTopMost, hasExplorerWindows);
            SetControlEnabledState(comboBoxWindowIDs, hasExplorerWindows);
        }

        private void UpdateStatusLabel(string text)
        {
            UpdateControlText(labelStatus, text);
        }

        private void UpdateControlText(Control control, string text)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<Control, string>(UpdateControlText), control, text);
            }
            else
            {
                control.Text = text;
            }
        }

        private void SetControlEnabledState(Control control, bool enabled)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<Control, bool>(SetControlEnabledState), control, enabled);
            }
            else
            {
                control.Enabled = enabled;
            }
        }

        private void RemoveItemFromComboBox(ComboBox comboBox, IntPtr hWnd)
        {
            if (comboBox.InvokeRequired)
            {
                comboBox.Invoke(new Action<ComboBox, IntPtr>(RemoveItemFromComboBox), comboBox, hWnd);
            }
            else
            {
                comboBox.Items.Remove(hWnd);
            }
        }
    }
}
