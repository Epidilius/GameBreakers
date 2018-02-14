using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameBreakersDBManagement
{
    public partial class Startup : Form
    {
        MtG MtGForm;
        CardboardConnection CCForm;
        static Startup StartupInstance;

        #region System Menu Requirements
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

        public const Int32 WM_SYSCOMMAND = 0x112;
        public const Int32 MF_SEPARATOR = 0x800;
        public const Int32 MF_BYPOSITION = 0x400;
        public const Int32 MF_STRING = 0x0;

        //Custom system menu items
        public const Int32 _SettingsSysMenuID = 1000;
        public const Int32 _HelpSysMenuID = 1001;
        #endregion

        public Startup()
        {
            InitializeComponent();

            DatabaseManager.UnlockAllSets();

            SetupForms();
            AddSystemMenuItems();

            StartupInstance = this;
        }
        public static Startup GetInstance()
        {
            return StartupInstance;
        }

        void SetupForms()
        {
            MtGForm           = new MtG();
            CCForm            = new CardboardConnection();
            MtGForm.MdiParent = this;
            MtGForm.Dock      = DockStyle.Fill;
            CCForm.MdiParent  = this;
            CCForm.Dock       = DockStyle.Fill;
        }
        void AddSystemMenuItems()
        {
            /// Get the Handle for the Forms System Menu
            IntPtr systemMenuHandle = GetSystemMenu(this.Handle, false);

            /// Create our new System Menu items just before the Close menu item
            InsertMenu(systemMenuHandle, 5, MF_BYPOSITION | MF_SEPARATOR, 0, string.Empty); // <-- Add a menu seperator
            InsertMenu(systemMenuHandle, 6, MF_BYPOSITION, _SettingsSysMenuID, "Settings");
            InsertMenu(systemMenuHandle, 7, MF_BYPOSITION, _HelpSysMenuID, "Help");
        }

        //MENU/TRAY FUNCTIONS
        protected override void WndProc(ref Message m)
        {
            // Check if a System Command has been executed
            if (m.Msg == WM_SYSCOMMAND)
            {
                // Execute the appropriate code for the System Menu item that was clicked
                switch (m.WParam.ToInt32())
                {
                    case _SettingsSysMenuID:
                        Settings settings = new Settings();
                        settings.Show();
                        break;
                    case _HelpSysMenuID:
                        MessageBox.Show("For help, send an email to joel.cright@gmail.com", "Help", MessageBoxButtons.OK);
                        break;
                }
            }

            base.WndProc(ref m);
        }

        public void UpdateCarts()
        {
            MtGForm.LoadCarts();
            CCForm.LoadCarts();
        }

        private void button_MtG_Click(object sender, EventArgs e)
        {
            if (!MtGForm.Enabled)
                MtGForm = new MtG();
            MtGForm.Show();
        }

        private void button_CardboardConnection_Click(object sender, EventArgs e)
        {
            CCForm = new CardboardConnection();
            CCForm.Show();
        }

        private void magicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CCForm.Hide();
            MtGForm.Show();
        }

        private void cardboardConnectionToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MtGForm.Hide();
            CCForm.Show();
        }
    }
}
