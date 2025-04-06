using MetroFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Modus
{
    public partial class PathNavigator : UserControl
    {
        //private readonly bool _isDarkMode = DarkModeHelper.IsDarkModeEnabled();
        private bool appearenceupdated = false;
        private Color HeaderBackColor, ThemeBackColor, ThemeForeColor, StyleForeColor, ThemeDarkColor, ThemeBackColorHighlight, ThemeLightColor, ThemeBasicColor;

        public event EventHandler AdressChanged;

        private string _adress = "C:\\";

        public string Adress
        {
            get { return _adress; }
            set
            {
                if (_adress != value) // Evitar chamadas redundantes
                {
                    _adress = value;
                    OnAdressChanged();
                }
            }
        }

        public void FixSize()
        {
            InitializePathNavigator(_adress);
        }

        protected virtual void OnAdressChanged()
        {
            AdressChanged?.Invoke(this, EventArgs.Empty);
            InitializePathNavigator(_adress); // Atualiza o PathNavigator ao mudar o endereço
        }

        public void UpdateAdress()
        {
            InitializePathNavigator(_adress);
        }

        private bool _isTextMode = false;
        public bool TextMode
        {
            get { return _isTextMode; }
            set
            {
                if (_isTextMode != value)
                {
                    _isTextMode = value;
                }
            }
        }

        public bool ShowIcon { get; set; } = true;
        public void SetIcon(Image icon)
        {
            picIcon.Image = icon;
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

        public PathNavigator()
        {
            InitializeComponent();
            InitializeEvents();
            UpdateAdress();
        }

        private void InitializeEvents()
        {
        }

        private void PathNavigator_Load(object sender, EventArgs e)
        {
            UpdateTheme();
        }

        private void UpdateThisAppearence()
        {
            if (appearenceupdated) return;
            appearenceupdated = true;
            this.TabStop = false;

            picIcon.Visible = ShowIcon;
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

            this.BackColor = ThemeBackColorHighlight;
            pnMain.BackColor = ThemeBackColor;
            picIcon.BackColor = ThemeBackColor;
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

        private void InitializePathNavigator(string fullAdress)
        {
            _isTextMode = false;
            string adress = Path.ChangeExtension(fullAdress, null);

            // Configurações da fonte e do layout
            var buttonFont = new Font("Segoe UI", 8.25f, FontStyle.Regular);
            const int buttonPadding = 5; // Espaço extra para margem/padding

            // Limpar os controles atuais no FlowLayoutPanel
            flwAdress.Controls.Clear();

            // Dividir o endereço da pasta em partes (exemplo: C:\_VSFiles\Projects)
            var pathParts = adress.Split(Path.DirectorySeparatorChar);

            // Lista para armazenar os botões criados e a largura total acumulada
            List<CustomButton> buttonList = new List<CustomButton>();
            int totalUsedWidth = 0;
            string cumulativePath = string.Empty;

            foreach (var part in pathParts)
            {
                if (string.IsNullOrEmpty(part))
                    continue;

                // Constrói o caminho acumulado
                cumulativePath = string.IsNullOrEmpty(cumulativePath)
                    ? part
                    : Path.Combine(cumulativePath, part);

                // Se o nome do botão for maior que 14 caracteres, trunca e adiciona ".."
                string displayName = part.Length > 14 ? part.Substring(0, 14) + ".." : part;

                // Define o texto do botão com sufixo " >"
                string buttonText = $"{displayName} >";

                // Calcula a largura necessária para o botão
                int buttonWidth = TextRenderer.MeasureText(buttonText, buttonFont).Width + buttonPadding;

                var button = new CustomButton()
                {
                    ButtonText = buttonText,
                    Font = buttonFont,
                    Tag = cumulativePath, // mantém o caminho completo
                    Width = buttonWidth,
                    Height = flwAdress.Height - 3,
                    Margin = new Padding(0, 2, 0, 0),
                };

                // Atualiza a largura total e adiciona o botão na lista
                totalUsedWidth += buttonWidth;
                buttonList.Add(button);

                // Evento de clique para navegação
                button.MouseDown += (sender, e) =>
                {
                    Adress = (string)((CustomButton)sender).Tag;
                };
            }

            // Se a largura total for maior que a disponível no painel, remova botões do começo
            // até que caiba pelo menos a última parte
            while (buttonList.Count > 1 && totalUsedWidth > flwAdress.Width)
            {
                // Remove o primeiro botão e atualiza a largura total
                totalUsedWidth -= buttonList[0].Width;
                buttonList.RemoveAt(0);
            }

            // Adiciona os botões restantes ao FlowLayoutPanel e aplica o tema a cada um
            foreach (var btn in buttonList)
            {
                ApplyButtomTheme(btn);
                flwAdress.Controls.Add(btn);
            }

            // Calcula a largura restante no painel e cria um botão vazio para preencher o espaço
            int remainingWidth = Math.Max(0, flwAdress.Width - totalUsedWidth);
            var emptyButton = new CustomButton()
            {
                ButtonText = "",
                Font = buttonFont,
                Width = remainingWidth,
                Height = flwAdress.Height - 3,
                Margin = new Padding(0, 2, 0, 0),
            };

            emptyButton.MouseDown += (sender, e) =>
            {
                ShowTextBoxForFullPath(fullAdress);
            };

            emptyButton.ButtonHighlight = true;
            emptyButton.ButtonBorderWidth = new Padding(0);
            emptyButton.ButtonForeColor = ThemeForeColor;
            emptyButton.ButtonBackColor = ThemeBackColor;
            emptyButton.ButtonHighlightBackColor = ThemeBackColor;
            emptyButton.ButtonHighlightForeColor = ThemeForeColor;

            flwAdress.Controls.Add(emptyButton);
        }



        private void ShowTextBoxForFullPath(string fullPath)
        {
            _isTextMode = true;
            // Criar o MetroTextBox para editar o caminho completo
            flwAdress.Controls.Clear();

            // Criar o botão vazio para preencher o espaço restante
            var panel = new Panel()
            {
                BackColor = ThemeBackColor,
                Width = flwAdress.Width,
                Height = flwAdress.Height - 3, // Define a altura igual à altura do PathNavigator
                Margin = new Padding(0, 2, 0, 0),
            };

            var textBox = new TextBox()
            {
                Text = fullPath,
                BackColor = ThemeBackColor,
                ForeColor = ThemeForeColor,
                Width = flwAdress.Width,
                Margin = new Padding(0),
                BorderStyle = BorderStyle.FixedSingle,
            };

            textBox.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    // Quando pressionar Enter, esconde o MetroTextBox e recria os botões
                    flwAdress.Controls.Clear();
                    Adress = textBox.Text;
                    InitializePathNavigator(textBox.Text);
                }
            };
            panel.Controls.Add(textBox);
            flwAdress.Controls.Add(panel);
            textBox.Focus();
            textBox.SelectAll();
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

    }


    public class PathNavigatorMessageFilter : IMessageFilter
    {
        private readonly PathNavigator pathNavigator;

        public PathNavigatorMessageFilter(PathNavigator pathNavigator)
        {
            this.pathNavigator = pathNavigator;
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
                if (!pathNavigator.RectangleToScreen(pathNavigator.ClientRectangle).Contains(Cursor.Position) && pathNavigator.TextMode)
                {
                    // Verifica se o mouse está dentro do controle de usuario antes de alterar
                    if (pathNavigator != null && pathNavigator.Bounds.Contains(Cursor.Position))
                    {
                        return true;
                    }

                    pathNavigator.UpdateAdress();
                }
            }

            return false;
        }
    }
}