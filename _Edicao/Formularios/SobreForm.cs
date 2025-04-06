using MetroFramework;
using System;
using System.Drawing;
using System.IO;

namespace Modus
{
    public partial class SobreForm : MetroFramework.Forms.MetroForm
    {
        private string programFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
        //private readonly bool _isDarkMode = DarkModeHelper.IsDarkModeEnabled();
        private int minWidth, minHeight, maxWidth, maxHeight;
        private Color HeaderBackColor, ThemeBackColor, ThemeForeColor;

        public void ChangeIcon(Icon newIcon)
        {
            this.Icon = newIcon;
        }

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

        LanguageManager langManager;
        public event EventHandler LanguageChanged;
        private string _language = "ptbr";
        public string Language
        {
            get { return _language; }
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnLanguageChanged();
                }
            }
        }

        protected virtual void OnLanguageChanged()
        {
            LanguageChanged?.Invoke(this, EventArgs.Empty);
            UpdateLanguage();
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

        public SobreForm()
        {
            InitializeComponent();
            InitializeEvents();
        }

        private void Sobre_Load(object sender, EventArgs e)
        {
            UpdateLanguage();
            UpdateThisAppearence();
        }

        private void InitializeEvents()
        {
            langManager = new LanguageManager(programFolder);
            this.StyleManager = mStyleManager;
        }

        private void UpdateLanguage()
        {
            langManager.LoadLanguage(Language);
            windowBar1.Title = langManager[64];

            Form1 formPrincipal = new Form1();
            string nomeDoProjeto = formPrincipal.NomeDoProjeto;
            string versaoDoArquivo = formPrincipal.VersaoDoArquivo;
            string nomeDaEmpresa = formPrincipal.NomeDaEmpresa;
            string dataAtual = DateTime.Now.Year.ToString();
            label1.Text = $"{nomeDoProjeto}\r\n{langManager[65]} {versaoDoArquivo}\r\n© {dataAtual} {nomeDaEmpresa}\r\n{langManager[66]}";
        }

        private void UpdateThisAppearence()
        {
            minWidth = ClientSize.Width / 2 - (minWidth % 1); // Largura minima da janela
            minHeight = ClientSize.Height / 2 - (minHeight % 1); // Altura minima da janela
            maxWidth = ClientSize.Width;
            maxHeight = ClientSize.Height;

            MetroTheme = _metroTheme;
            _isDarkMode = (MetroTheme == MetroThemeStyle.Dark);
            MetroStyle = _metroStyle;

            UpdateTheme();
        }

        private void UpdateTheme()
        {
            HeaderBackColor = _isDarkMode ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 255, 255);
            ThemeBackColor = _isDarkMode ? Color.FromArgb(17, 17, 17) : Color.FromArgb(225, 225, 225);
            ThemeForeColor = _isDarkMode ? Color.FromArgb(225, 225, 225) : Color.FromArgb(17, 17, 17);

            windowBar1.DarkMode = _isDarkMode;
            label1.ForeColor = ThemeForeColor;
        }

        protected override void OnResizeBegin(EventArgs e)
        {
            base.OnResizeBegin(e);
            MinimumSize = new Size(minWidth, minHeight);
            MaximumSize = new Size(maxWidth, maxHeight);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            if (Width < minWidth) Width = minWidth;
            if (Height < minHeight) Height = minHeight;
            if (Width > maxWidth) Width = maxWidth;
            if (Height > maxHeight) Height = maxHeight;
        }

        private void Sobre_SizeChanged(object sender, EventArgs e)
        {
            // Desabilitar redimensionamento estando maximizado
            this.Resizable = false;// WindowState != FormWindowState.Maximized;
        }

        public void UpdateUserSettings(MetroThemeStyle theme, MetroColorStyle style, string language)
        {
            MetroTheme = theme;
            MetroStyle = style;
            Language = language;
        }
    }
}
