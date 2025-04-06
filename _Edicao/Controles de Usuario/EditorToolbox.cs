using MetroFramework;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Modus
{
    public partial class EditorToolbox : UserControl
    {
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        public int BorderRadius { get; set; } = 20; // Ajuste o raio da borda aqui

        //private readonly bool _isDarkMode = DarkModeHelper.IsDarkModeEnabled();
        private bool appearenceupdated = false;
        private Color HeaderBackColor, ThemeBackColor, ThemeForeColor, StyleForeColor, ThemeDarkColor, ThemeBackColorHighlight, ThemeLightColor, ThemeBasicColor;

        public event EventHandler DarkModeChanged;
        private bool _isDarkMode = DarkModeHelper.IsDarkModeEnabled();
        public bool DarkMode
        {
            get { return _isDarkMode; }
            set
            {
                if (_isDarkMode != value)
                {
                    _isDarkMode = value;
                    OnDarkModeChanged();
                }
            }
        }

        protected virtual void OnDarkModeChanged()
        {
            DarkModeChanged?.Invoke(this, EventArgs.Empty);
            UpdateTheme();
        }

        private MetroThemeStyle _metroTheme;
        public MetroThemeStyle MetroTheme
        {
            get { return mStyleManager.Theme; }
            set
            {
                if (_metroTheme != value)
                {
                    mStyleManager.Theme = value;
                    _metroTheme = value;
                    mStyleManager.Update();
                }
            }
        }

        private MetroColorStyle _metroStyle;
        public MetroColorStyle MetroStyle
        {
            get { return mStyleManager.Style; }
            set
            {
                if (_metroStyle != value)
                {
                    mStyleManager.Style = value;
                    _metroStyle = value;
                    mStyleManager.Update();
                }
            }
        }

        public EditorToolbox()
        {
            InitializeComponent();
            InitializeEvents();
            this.Resize += (s, e) => ApplyRoundedCorners();
            ApplyRoundedCorners();
        }

        private void ApplyRoundedCorners()
        {
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, BorderRadius, BorderRadius));
        }

        public event EventHandler AlignLeftClicked;
        public event EventHandler AlignCenterClicked;
        public event EventHandler AlignRightClicked;
        public event EventHandler CopyClicked;
        public event EventHandler PasteClicked;
        public event EventHandler CutClicked;
        public event EventHandler RemoveClicked;
        public event EventHandler BoldClicked;
        public event EventHandler ItalicClicked;
        public event EventHandler RiskClicked;
        public event EventHandler UnderlinedClicked;
        public event EventHandler TextSizeClicked;
        public event EventHandler TextColorClicked;
        public event EventHandler AddMediaClicked;

        private void InitializeEvents()
        {
            btnAlignLeft.Click += (s, e) => AlignLeftClicked?.Invoke(this, EventArgs.Empty);
            btnAlignCenter.Click += (s, e) => AlignCenterClicked?.Invoke(this, EventArgs.Empty);
            btnAlignRight.Click += (s, e) => AlignRightClicked?.Invoke(this, EventArgs.Empty);
            btnCopy.Click += (s, e) => CopyClicked?.Invoke(this, EventArgs.Empty);
            btnPaste.Click += (s, e) => PasteClicked?.Invoke(this, EventArgs.Empty);
            btnCut.Click += (s, e) => CutClicked?.Invoke(this, EventArgs.Empty);
            btnRemove.Click += (s, e) => RemoveClicked?.Invoke(this, EventArgs.Empty);
            btnBold.Click += (s, e) => BoldClicked?.Invoke(this, EventArgs.Empty);
            btnItalic.Click += (s, e) => ItalicClicked?.Invoke(this, EventArgs.Empty);
            btnRisk.Click += (s, e) => RiskClicked?.Invoke(this, EventArgs.Empty);
            btnUnderlined.Click += (s, e) => UnderlinedClicked?.Invoke(this, EventArgs.Empty);
            btnSize.Click += (s, e) => TextSizeClicked?.Invoke(this, EventArgs.Empty);
            btnColor.Click += (s, e) => TextColorClicked?.Invoke(this, EventArgs.Empty);
            btnSize.Click += (s, e) => TextSizeClicked?.Invoke(this, EventArgs.Empty);
            btnMedia.Click += (s, e) => AddMediaClicked?.Invoke(this, EventArgs.Empty);
        }

        private void EditorToolbox_Load(object sender, EventArgs e)
        {
            UpdateTheme();
        }

        private void UpdateThisAppearence()
        {
            if (appearenceupdated) return;
            appearenceupdated = true;
            this.TabStop = false;

            MetroTheme = _isDarkMode ? MetroThemeStyle.Dark : MetroThemeStyle.Light; //_metroTheme;
            MetroStyle = _metroStyle;
            UpdateTheme();
        }

        private void UpdateTheme()
        {
            HeaderBackColor = _isDarkMode ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 255, 255);
            ThemeBackColor = _isDarkMode ? Color.FromArgb(27, 27, 27) : Color.FromArgb(255, 255, 255);
            ThemeForeColor = _isDarkMode ? Color.FromArgb(190, 190, 190) : Color.FromArgb(17, 17, 17);
            StyleForeColor = _isDarkMode ? MetroColorStyleToColor(MetroStyle) : ThemeForeColor;
            ThemeDarkColor = _isDarkMode ? Color.FromArgb(12, 12, 12) : Color.FromArgb(195, 195, 195);
            ThemeBackColorHighlight = _isDarkMode ? Color.FromArgb(99, 99, 99) : Color.FromArgb(195, 195, 195);
            ThemeLightColor = _isDarkMode ? Color.FromArgb(40, 40, 40) : Color.FromArgb(195, 195, 195);
            ThemeBasicColor = _isDarkMode ? Color.FromArgb(35, 35, 35) : Color.FromArgb(195, 195, 195);

            btnAlignLeft.ButtonBackgroundImage = _isDarkMode ? Properties.Resources.tbicon_dark_alignleft : Properties.Resources.tbicon_light_alignleft;
            btnAlignCenter.ButtonBackgroundImage = _isDarkMode ? Properties.Resources.tbicon_dark_aligncenter : Properties.Resources.tbicon_light_aligncenter;
            btnAlignRight.ButtonBackgroundImage = _isDarkMode ? Properties.Resources.tbicon_dark_alignright : Properties.Resources.tbicon_light_alignright;
            btnCopy.ButtonBackgroundImage = _isDarkMode ? Properties.Resources.tbicon_dark_copy : Properties.Resources.tbicon_light_copy;
            btnPaste.ButtonBackgroundImage = _isDarkMode ? Properties.Resources.tbicon_dark_paste : Properties.Resources.tbicon_light_paste;
            btnCut.ButtonBackgroundImage = _isDarkMode ? Properties.Resources.tbicon_dark_cut : Properties.Resources.tbicon_light_cut;

            this.BackColor = ThemeBackColor;

            ApplyButtomTheme(btnAlignLeft); // Align
            ApplyButtomTheme(btnAlignCenter); // Align
            ApplyButtomTheme(btnAlignRight); // Align
            ApplyButtomTheme(btnCopy); // Clipboard
            ApplyButtomTheme(btnPaste); // Clipboard
            ApplyButtomTheme(btnCut); // Clipboard
            ApplyButtomTheme(btnRemove); // Remove
            ApplyButtomTheme(btnBold); // Font
            ApplyButtomTheme(btnItalic); // Font
            ApplyButtomTheme(btnRisk); // Font
            ApplyButtomTheme(btnUnderlined); // Font
            ApplyButtomTheme(btnSize); // Font
            ApplyButtomTheme(btnMedia); // Media

            System.Single tSize = 14f;
            btnAlignLeft.ButtonText = "";
            btnAlignCenter.ButtonText = "";
            btnAlignRight.ButtonText = "";
            btnCopy.ButtonText = "";
            btnPaste.ButtonText = "";
            btnCut.ButtonText = "";
            btnRemove.ButtonText = "✕";
            btnRemove.ButtonFont = new Font("Yu Gothic UI", tSize, FontStyle.Bold);
            btnBold.ButtonText = "B";
            btnBold.ButtonFont = new Font("Segoe UI", tSize, FontStyle.Bold); // Negrito
            btnItalic.ButtonText = "I";
            btnItalic.ButtonFont = new Font("Segoe UI", tSize, FontStyle.Italic); // Itálico
            btnRisk.ButtonText = "Z";
            btnRisk.ButtonFont = new Font("Segoe UI", tSize, FontStyle.Strikeout); // Tachado
            btnUnderlined.ButtonText = "U";
            btnUnderlined.ButtonFont = new Font("Segoe UI", tSize, FontStyle.Underline); // Sublinhado
            btnColor.ButtonText = "";
            btnColor.ButtonHighlight = true;
            btnColor.ButtonBackColor = Color.Gray;
            btnColor.ButtonHighlightBackColor = Color.Gray;
            btnColor.ButtonBorderClickable = true;
            btnColor.ButtonBorderWidth = new Padding(5);
            btnColor.ButtonBorderColor = ThemeBackColor;
            btnColor.ButtonBorderHighlightColor = ThemeBackColorHighlight;
            btnSize.ButtonText = "T";
            btnSize.ButtonFont = new Font("Segoe UI", tSize, FontStyle.Bold);
            btnMedia.ButtonText = "🞤";
            btnMedia.ButtonFont = new Font("Yu Gothic UI", tSize, FontStyle.Bold);
        }

        private void ApplyButtomTheme(CustomButton button)
        {
            button.ButtonHighlight = true;
            button.ButtonBorderWidth = new Padding(0);
            button.ButtonForeColor = ThemeForeColor;
            button.ButtonBackColor = ThemeBackColor;
            //button.ButtonBorderColor = ThemeForeColor;
            button.ButtonHighlightBackColor = ThemeBackColorHighlight;
            button.ButtonHighlightForeColor = ThemeForeColor;
            //button.ButtonBorderHighlightColor = StyleForeColor;
        }

        public Color MetroColorStyleToColor(MetroColorStyle metroColorStyle)
        {
            // Converte o MetroColorStyle para string
            string metroColorStyleName = metroColorStyle.ToString();

            // Retira o prefixo "Metro" e mapeia para a cor correspondente
            switch (metroColorStyleName)
            {
                case "Black":
                    return Color.Silver;
                case "White":
                    return Color.Silver;
                case "Silver":
                    return Color.Silver;
                case "Blue":
                    return Color.Cyan;
                case "Green":
                    return Color.PaleGreen;
                case "Lime":
                    return Color.Lime;
                case "Teal":
                    return Color.Teal;
                case "Orange":
                    return Color.Orange;
                case "Brown":
                    return Color.Brown;
                case "Pink":
                    return Color.Pink;
                case "Magenta":
                    return Color.Magenta;
                case "Purple":
                    return Color.Purple;
                case "Red":
                    return Color.Red;
                case "Yellow":
                    return Color.Yellow;
                default:
                    return Color.Silver; // Retorna Color.Empty se o estilo não for encontrado
            }
        }

    }


    public class EditorToolboxMessageFilter : IMessageFilter
    {
        private readonly EditorToolbox editorToolbox;

        public EditorToolboxMessageFilter(EditorToolbox editorToolbox)
        {
            this.editorToolbox = editorToolbox;
        }

        public bool PreFilterMessage(ref Message m)
        {
            const int WM_MOUSEMOVE = 0x0200;
            const int WM_LBUTTONDOWN = 0x201;
            const int WM_RBUTTONDOWN = 0x204;
            const int WM_MBUTTONDOWN = 0x207;
            const int WM_NCLBUTTONDOWN = 0x0A1;
            const int WM_NCRBUTTONDOWN = 0x0A4;
            const int WM_NCMBUTTONDOWN = 0x0A7;
            const int WM_MOUSEWHEEL = 0x020A;

            if (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_RBUTTONDOWN || m.Msg == WM_MBUTTONDOWN
                || m.Msg == WM_NCLBUTTONDOWN || m.Msg == WM_NCRBUTTONDOWN || m.Msg == WM_NCMBUTTONDOWN
                || m.Msg == WM_MOUSEWHEEL || (m.Msg == WM_MOUSEMOVE && (Control.MouseButtons & MouseButtons.Left) != 0))
            {
                if (!editorToolbox.RectangleToScreen(editorToolbox.ClientRectangle).Contains(Cursor.Position))
                {
                    // Verifica se o mouse está dentro do menu de contexto antes de fechá-lo
                    if (editorToolbox != null
                        && editorToolbox.Visible
                        && editorToolbox.Bounds.Contains(Cursor.Position))
                    {
                        return true;
                    }

                    editorToolbox.Visible = false;
                }
            }

            return false;
        }
    }
}