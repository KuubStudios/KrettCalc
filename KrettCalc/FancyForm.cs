using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KrettCalc {
    public class FancyForm : Form {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        private const int STATUS_BAR_HEIGHT = 5;
        private const int MENU_WIDTH = 250;
        private const int ACTION_BAR_HEIGHT = 46;

        private const int WM_SYSCOMMAND = 0x0112;
        private const int WS_MINIMIZEBOX = 0x20000;
        private const int WS_SYSMENU = 0x00080000;

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_RBUTTONDOWN = 0x0204;

        private const uint TPM_LEFTALIGN = 0x0000;
        private const uint TPM_RETURNCMD = 0x0100;

        private Brush primaryBrush;
        private Color primaryColor;
        public Color PrimaryColor {
            get { return primaryColor; }
            set {
                primaryColor = value;
                primaryBrush = new SolidBrush(primaryColor);
            }
        }

        private Brush darkPrimaryBrush;
        private Color darkPrimaryColor;
        public Color DarkPrimaryColor {
            get { return primaryColor; }
            set {
                darkPrimaryColor = value;
                darkPrimaryBrush = new SolidBrush(darkPrimaryColor);
            }
        }

        private Rectangle xButtonBounds;
        private Rectangle actionBarBounds;
        private Rectangle statusBarBounds;
        private Rectangle menuBounds;

        private ButtonState closedButtonState = ButtonState.Normal;

        private readonly Pen buttonPen = new Pen(Color.White, 2);
        private readonly Brush textBrush = new SolidBrush(Color.White);

        protected override CreateParams CreateParams {
            get {
                CreateParams par = base.CreateParams;
                par.Style = par.Style | WS_MINIMIZEBOX | WS_SYSMENU;
                return par;
            }
        }

        public FancyForm() {
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            PrimaryColor = Color.FromArgb(0, 170, 173);
            DarkPrimaryColor = Color.FromArgb(25, 25, 31);
        }

        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);
            if(DesignMode || IsDisposed) return;

            Point cursorPos = PointToClient(Cursor.Position);

            if(m.Msg == WM_LBUTTONDOWN && (statusBarBounds.Contains(cursorPos) || actionBarBounds.Contains(cursorPos)) && !xButtonBounds.Contains(PointToClient(Cursor.Position))) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            } else if(m.Msg == WM_RBUTTONDOWN && ((statusBarBounds.Contains(cursorPos) || actionBarBounds.Contains(cursorPos)) && !xButtonBounds.Contains(cursorPos))) {
                int id = TrackPopupMenuEx(
                    GetSystemMenu(Handle, false),
                    TPM_LEFTALIGN | TPM_RETURNCMD,
                    Cursor.Position.X, Cursor.Position.Y, Handle, IntPtr.Zero);

                SendMessage(Handle, WM_SYSCOMMAND, id, 0);
            }
        }

        protected void OnGlobalMouseMove(object sender, MouseEventArgs e) {
            if(IsDisposed) return;

            Point pos = PointToClient(e.Location);
            OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, pos.X, pos.Y, 0));
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            if(DesignMode) return;
            UpdateButtons(e);

            if(e.Button == MouseButtons.Left) ReleaseCapture();

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            if(DesignMode) return;
            UpdateButtons(e, true);

            base.OnMouseUp(e);
            ReleaseCapture();
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            if(DesignMode) return;

            UpdateButtons(e);
        }

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);
            if(DesignMode) return;
            closedButtonState = ButtonState.Normal;
            Invalidate();
        }

        private void UpdateButtons(MouseEventArgs e, bool up = false) {
            if(DesignMode) return;

            ButtonState oldState = closedButtonState;

            if(e.Button == MouseButtons.Left && !up) {
                closedButtonState = xButtonBounds.Contains(e.Location) ? ButtonState.Pushed : ButtonState.Normal;
            } else {
                if(xButtonBounds.Contains(e.Location)) {
                    closedButtonState = ButtonState.Flat;
                    if(oldState == ButtonState.Pushed) Close();
                } else {
                    closedButtonState = ButtonState.Normal;
                }
            }

            if(oldState != closedButtonState) Invalidate();
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);

            statusBarBounds = new Rectangle(MENU_WIDTH, 0, Width - MENU_WIDTH, STATUS_BAR_HEIGHT);
            actionBarBounds = new Rectangle(MENU_WIDTH, STATUS_BAR_HEIGHT, Width - MENU_WIDTH, ACTION_BAR_HEIGHT);
            menuBounds = new Rectangle(0, 0, MENU_WIDTH, Height);

            xButtonBounds = new Rectangle((Width - ACTION_BAR_HEIGHT) + STATUS_BAR_HEIGHT, STATUS_BAR_HEIGHT + 5, ACTION_BAR_HEIGHT - STATUS_BAR_HEIGHT * 2, ACTION_BAR_HEIGHT - STATUS_BAR_HEIGHT * 2);
        }

        protected override void OnPaint(PaintEventArgs e) {
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            e.Graphics.Clear(BackColor);

            e.Graphics.FillRectangle(darkPrimaryBrush, statusBarBounds);
            e.Graphics.FillRectangle(primaryBrush, actionBarBounds);
            e.Graphics.FillRectangle(darkPrimaryBrush, menuBounds);

            if(closedButtonState == ButtonState.Flat) {
                e.Graphics.FillRectangle(darkPrimaryBrush, xButtonBounds);
            } else if(closedButtonState == ButtonState.Pushed) {
                e.Graphics.FillRectangle(darkPrimaryBrush, xButtonBounds);
            }

            e.Graphics.DrawLine(
                buttonPen,
                xButtonBounds.X + (int)(xButtonBounds.Width * 0.33),
                xButtonBounds.Y + (int)(xButtonBounds.Height * 0.33),
                xButtonBounds.X + (int)(xButtonBounds.Width * 0.66),
                xButtonBounds.Y + (int)(xButtonBounds.Height * 0.66));

            e.Graphics.DrawLine(
                buttonPen,
                xButtonBounds.X + (int)(xButtonBounds.Width * 0.66),
                xButtonBounds.Y + (int)(xButtonBounds.Height * 0.33),
                xButtonBounds.X + (int)(xButtonBounds.Width * 0.33),
                xButtonBounds.Y + (int)(xButtonBounds.Height * 0.66));

            e.Graphics.DrawString(Text, Font, textBrush, new Rectangle(MENU_WIDTH + STATUS_BAR_HEIGHT, STATUS_BAR_HEIGHT, Width, ACTION_BAR_HEIGHT), new StringFormat { LineAlignment = StringAlignment.Center });
        }
    }
}