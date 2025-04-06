using MetroFramework;
using MetroFramework.Components;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using RtfPipe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using Color = System.Drawing.Color;
using Control = System.Windows.Forms.Control;
using FFile = System.IO.File;
using Font = System.Drawing.Font;
using TTask = System.Threading.Tasks.Task;
using TTimer = System.Timers.Timer;
using Timer = System.Windows.Forms.Timer;
using Label = System.Windows.Forms.Label;
using System.Xml;
using Windows.Data.Xml.Dom;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using Windows.ApplicationModel.Resources;
using Modus._Edicao.Controles_de_Usuario;

namespace Modus
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        private bool isPremiumVersion = false; // Versao
        private bool isDevModeEnabled = false; // Em Desenvolvimento
        private bool isErrorLogEnabled = true; // Log de Erros

        private string programFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
        private string appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Asen Lab", "ASL Modus");
        private string tempFolder;

        private string caminhoColecaoAtual = null;
        private string ultimaColecao = Properties.Settings.Default.UltimaColecao;
        private string caminhoDiretorioAtual = string.Empty;

        private readonly SaveQueue saveQueue = new SaveQueue();

        private string ultimoTema, ultimoEstilo;
        private bool isDebugModeEnabled, isFavMenuShowing, isDocReadOnly, isAutoSaveEnabled, isRollDownEnabled;

        //private readonly bool _isDarkMode = DarkModeHelper.IsDarkModeEnabled();
        private int minWidth, minHeight;
        private Color HeaderBackColor, ThemeBackColor, ThemeForeColor, StyleForeColor, ThemeDarkColor, ThemeBackColorHighlight, ThemeLightColor, ThemeBasicColor;
        private Font ThemeFont, ThemeFontBold;
        private Color ThemeColor = Color.OrangeRed;
        private MetroStyleManager mStyleManager = new();
        private ConfigsForm configs = new();
        private SobreForm sobre = new();

        private bool isNewFile = false;
        //private bool isToolBoxOpen = false;
        private string InitialDirectory;
        private bool isLoadingDir = false;
        private TableLayoutPanel tblDoc;
        private Timer timerDocLoad;
        private bool firstDocLoad = true;
        private object copiedItem = null;
        private Button folderButton;
        private bool resetDocTheme = false;
        private string currentStructure;
        private WebView2 webView;
        private Label lblTitle;
        private bool isDocMode;
        private bool textReset = false;
        private RichTextBox rtbDocument;
        private FormWindowState lastWindowState;
        private Collection collection;
        private Image folderIcon;
        private Image docIcon;
        private string emptyDocument;
        private bool houveAlteracao = false;
        // Salvamento Automatico
        private TTimer autoSaveTimer;
        private int saveInterval = 1000 * 3;

        private Stack<string> backHistory = new Stack<string>();
        private Stack<string> forwardHistory = new Stack<string>();

        //private ContextMenuStrip colorMenu;
        private ContextMenuStrip sizeMenu;
        private int TSize = 12;
        private int TColor = 1;

        public string NomeDoProjeto { get; } = "ASL Modus";
        public string VersaoDoArquivo { get; } = ObterVersaoDoArquivo();
        public string NomeDaEmpresa { get; } = "Asen Lab Corporation";

        private static string ObterVersaoDoArquivo()
        {
            FileVersionInfo versaoArquivo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return versaoArquivo.FileVersion;
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

        public Form1()
        {
            InitializeComponent();
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            tempFolder = Path.Combine(appData, "@temp");
            langManager = new LanguageManager(programFolder);
            MoverIconeParaAssets();

            pathNavigator1.AdressChanged += PathNavigator_AdressChanged;
            configs.cbThemeChanged += Configs_ThemeChanged;
            configs.cbStyleChanged += Configs_StyleChanged;
            // Adicionar eventos aos itens
            cortarItem.Click += (s, e) => rtbDocument.Cut();
            copiarItem.Click += (s, e) => rtbDocument.Copy();
            colarItem.Click += (s, e) => rtbDocument.Paste();
            excluirItem.Click += (s, e) => rtbDocument.SelectedText = "";

            mStyleManager.Owner = this;
            this.StyleManager = mStyleManager;

            this.AutoScaleMode = AutoScaleMode.Dpi;
            //this.ClientSize = new Size(1050, 650); // ou o tamanho desejado
            Application.EnableVisualStyles();
            //this.Size = new Size(1050, 650); // Defina o tamanho desejado
            //this.StartPosition = FormStartPosition.CenterScreen; // Centraliza a janela
            //this.StartPosition = FormStartPosition.Manual;  // Impede o comportamento de centralizar
            if (Properties.Settings.Default.WindowSize.Width > 0 &&
                Properties.Settings.Default.WindowSize.Height > 0)
            {
                this.Size = Properties.Settings.Default.WindowSize;
            }

            if (Properties.Settings.Default.WindowLocation.X > 0 &&
                Properties.Settings.Default.WindowLocation.Y > 0)
            {
                this.Location = Properties.Settings.Default.WindowLocation;
            }
            this.WindowState = Properties.Settings.Default.WindowState;

            Application.AddMessageFilter(new EditorToolboxMessageFilter(editorToolbox1)); // Desfocar ao clicar fora
            Application.AddMessageFilter(new PathNavigatorMessageFilter(pathNavigator1)); // Desfocar ao clicar fora
            Application.AddMessageFilter(new WindowBarMessageFilter(windowBar1)); // Desfocar ao clicar fora
            //Application.AddMessageFilter(new DropDownMessageFilter(comboCheckBox1)); // Desfocar ao clicar fora

            emptyDocument = "[ondir]";

            /*
        @"{\rtf1\ansi\deff0
{\fonttbl{\f0\fswiss Arial;}}  // Define a fonte Arial
{\colortbl;\red190\green190\blue190;\red17\green17\blue17;\red255\green0\blue0;\red0\green0\blue255;}  // Define as cores
\f0  // Usa a fonte Arial
\cf1  // Define a cor padrão como a primeira cor da paleta (branco RGB 190, 190, 190)
\b Texto em negrito \b0\par
\i Texto em itálico \i0\par
\ul Texto sublinhado \ul0\par
\cf3 Texto em vermelho \cf0\par
\cf4 Texto em azul \cf0\par
\fs24 Texto com tamanho 12pt\par
\fs36 Texto com tamanho 18pt\par
}";*/

            timerDocLoad = new Timer();
            timerDocLoad.Interval = 100;
            timerDocLoad.Tick += (s, e) =>
            {
                if (isDocReadOnly)
                {
                    if (webView.CoreWebView2 != null)
                    {
                        string docFolder = GetDocFolder();
                        if (Directory.Exists(docFolder))
                        {
                            webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                                "media.local", docFolder, CoreWebView2HostResourceAccessKind.Allow);
                        }
                        webView.CoreWebView2.NavigateToString(ConvertRtfToHtml(currentStructure));
                    }
                    else
                    {

                    }
                }

                // Só executa após o intervalo de espera
                firstDocLoad = false;
                timerDocLoad.Stop();
            };

            this.DragEnter += Form1_DragEnter;
            this.DragDrop += Form1_DragDrop;
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy; // Permite a cópia do arquivo
            }
            else
            {
                e.Effect = DragDropEffects.None; // Não permite drag-and-drop
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            // Recupera o caminho do arquivo arrastado
            string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (filePaths.Length > 0)
            {
                string filePath = filePaths[0];
                ProcessFile(filePath);  // Processa o arquivo
            }
        }

        // Método para processar o arquivo (tanto no arrasto quanto na inicialização)
        public void ProcessFile(string filePath)
        {
            // Exibe o caminho do arquivo, ou faça o processamento necessário
            //MessageBox.Show("Arquivo aberto: " + filePath);
                        
            if (SalvarAntesDeFechar() && !string.IsNullOrEmpty(filePath))
            {
                caminhoColecaoAtual = filePath;
                ultimaColecao = caminhoColecaoAtual;
                Properties.Settings.Default.UltimaColecao = ultimaColecao;
                Properties.Settings.Default.Save();
                SalvamentoPendente(false);
                UpdateInfos();
            }
        }

        private void InitializeEditorToolBox()
        {
            // Criar o EditorToolbox
            editorToolbox1.Visible = false;
            editorToolbox1.Size = new Size(210, 60);

            // Conectar os eventos dos botões
            editorToolbox1.AlignLeftClicked += (s, ev) => SetTextAlign(-1);
            editorToolbox1.AlignCenterClicked += (s, ev) => SetTextAlign(0);
            editorToolbox1.AlignRightClicked += (s, ev) => SetTextAlign(1);
            editorToolbox1.CopyClicked += (s, ev) => rtbDocument.Copy();
            editorToolbox1.PasteClicked += (s, ev) => rtbDocument.Paste();
            editorToolbox1.CutClicked += (s, ev) => rtbDocument.Cut();
            editorToolbox1.RemoveClicked += (s, ev) => rtbDocument.SelectedText = "";
            editorToolbox1.BoldClicked += (s, ev) => SetTextStyle(FontStyle.Bold);
            editorToolbox1.ItalicClicked += (s, ev) => SetTextStyle(FontStyle.Italic);
            editorToolbox1.RiskClicked += (s, ev) => SetTextStyle(FontStyle.Strikeout);
            editorToolbox1.UnderlinedClicked += (s, ev) => SetTextStyle(FontStyle.Underline);
            editorToolbox1.TextSizeClicked += (s, ev) =>
            {
                // Cria o menu de tamanhos
                sizeMenu = new ContextMenuStrip();
                int[] sizes = { 10, 12, 14, 16, 18, 20, 24, 28, 32 };

                foreach (var size in sizes)
                {
                    var item = new ToolStripMenuItem(size.ToString());
                    item.Click += (sender, args) =>
                    {
                        TSize = size;
                        SetTextSize(size); // Aplica o tamanho ao clicar em um item
                    };
                    sizeMenu.Items.Add(item);
                }

                // Exibe o menu de tamanhos na posição do mouse
                sizeMenu.Show(MousePosition);
            };

            editorToolbox1.TextColorClicked += (s, ev) =>
            {
                using (ColorDialog colorDialog = new ColorDialog())
                {
                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        Color selectedColor = colorDialog.Color;
                        TColor = selectedColor.ToArgb(); // Armazena a cor escolhida
                        SetTextColor(selectedColor); // Aplica a cor ao texto
                    }
                }
            };

            editorToolbox1.AddMediaClicked += (s, ev) => AddMediaToDocument();
        }

        public void PathNavigator_AdressChanged(object sender, EventArgs e)
        {
            if (!isChangingAdress)
            {
                if (pathNavigator1.Adress != GetCurrentDirectory(null) && DirectoryExists(pathNavigator1.Adress)) SetCurrentDirectory(pathNavigator1.Adress, true);
            }
            else
            {
                pathNavigator1.Adress = lastDocDirectory;
                //Debug(isDebugConsoleEnabled, $"2 {lastDocDirectory}");
            }
        }
        private bool isChangingAdress = false; // Flag para controlar a mudança de endereço
        private string lastDocDirectory;

        private void SetCurrentDirectory(string adress, bool asksave)
        {
            if (isLoadingDir) return;

            if (collection != null && !string.IsNullOrEmpty(adress))
            {
                if (asksave && isDocMode && houveAlteracao && !SalvarAntesDeFechar())
                {
                    adress = GetCurrentDirectory(null);
                    lastDocDirectory = GetCurrentDirectory(null);
                    //Debug(isDebugConsoleEnabled, $"1 {lastDocDirectory}");

                    // Evita chamada recursiva com a flag
                    isChangingAdress = true;  // Marca que estamos alterando o endereço programaticamente
                    pathNavigator1.Adress = adress; // Atualiza o endereço
                    isChangingAdress = false; // Reseta a flag após a mudança
                    //Debug(isDebugConsoleEnabled, $"3 {lastDocDirectory}");
                    return;
                }
                isLoadingDir = true;
                flwExplorer.Controls.Clear();
                txtSearch.Clear();
                pathNavigator1.Adress = adress;
                caminhoDiretorioAtual = adress;
                LoadDirectory(adress);
                UpdateTitle();
            }
            isLoadingDir = false;
        }

        private string GetCurrentDirectory(string type)
        {
            if ((type == "Folder" || type == "folder") && FolderExists(caminhoDiretorioAtual))
            {
                return caminhoDiretorioAtual;
            }
            if ((type == "Doc" || type == "doc") && DocExists(caminhoDiretorioAtual))
            {
                return caminhoDiretorioAtual;
            }
            if ((string.IsNullOrEmpty(type)))
            {
                return caminhoDiretorioAtual;
            }

            return string.Empty;
        }

        private bool DirectoryExists(string adress)
        {
            return (collection.Folders.Any(folder => folder.Directory.Equals(adress, StringComparison.OrdinalIgnoreCase)) ||
            collection.Documents.Any(doc => doc.Directory.Equals(adress, StringComparison.OrdinalIgnoreCase)));
        }

        private bool FolderExists(string adress)
        {
            return collection.Folders.Any(folder => folder.Directory.Equals(adress, StringComparison.OrdinalIgnoreCase));
        }

        private bool DocExists(string adress)
        {
            return collection.Documents.Any(doc => doc.Directory.Equals(adress, StringComparison.OrdinalIgnoreCase));
        }

        private string GetModuleType(string adress)
        {
            if (FolderExists(adress))
            {
                return "Folder";
            }
            if (DocExists(adress))
            {
                return "Doc";
            }
            return string.Empty;
        }

        public void Configs_ThemeChanged(object sender, EventArgs e)
        {
            MetroTheme = configs.SelectedTheme;
            UpdateTheme();
        }

        public void Configs_StyleChanged(object sender, EventArgs e)
        {
            MetroStyle = configs.SelectedStyle;
            UpdateTheme();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadUserSettings();
            InitializeControls();
            UpdateLanguage();
            UpdateThisAppearence();
            InitializeToolBar();
            InitializeEditorToolBox();
            LoadPreviousFile();
            InitializeTimers();
        }

        private void UpdateLanguage()
        {
            langManager.LoadLanguage(Language);
            InitialDirectory = langManager[1];
            refreshItem.Text = langManager[51];
            pasteItem.Text = langManager[52];
            newItem.Text = langManager[53];
            newFolderItem.Text = langManager[54];
            newFileItem.Text = langManager[55];
            openItem.Text = langManager[56];
            favItem.Text = langManager[57];
            copyItem.Text = langManager[58];
            deleteItem.Text = langManager[59];
            renameItem.Text = langManager[60];
            cortarItem.Text = langManager[72];
            InitializeToolBar();
            windowBar1.Language = Language;
            txtSearch.WaterMark = langManager[71];
            txtSearch.PromptText = langManager[71];
            txtSearch.Invalidate();
            txtSearch.Update();
            cortarItem.Text = langManager[72];
            copiarItem.Text = langManager[58];
            colarItem.Text = langManager[52];
            excluirItem.Text = langManager[59];
        }

        private void InitializeControls()
        {
            // Cria o Label para o título
            lblTitle = new Label()
            {
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Font = ThemeFontBold,
                BackColor = ThemeBasicColor,
                ForeColor = ThemeForeColor,
            };

            // Cria o RichTextBox (modo de edição)
            rtbDocument = new RichTextBox()
            {
                ReadOnly = false,
                AcceptsTab = true,
                Dock = DockStyle.Fill, // O label ocupa toda a área da célula
                BorderStyle = System.Windows.Forms.BorderStyle.None,
                BackColor = ThemeBasicColor,
                ForeColor = ThemeForeColor,
                Font = ThemeFont,
                //SelectionFont = ThemeFont,
                //SelectionColor = ThemeForeColor,
                Margin = new Padding(10),
            };

            rtbDocument.MouseDown += RtbDocument_MouseDown;
            rtbDocument.TextChanged += (s, e) =>
            {
                currentStructure = rtbDocument.Rtf;

                if (resetDocTheme)
                {
                    if (rtbDocument.TextLength < 2)
                    {
                        textReset = true;
                    }

                    if (textReset && rtbDocument.TextLength > 0)
                    {
                        textReset = false;
                        rtbDocument.Font = ThemeFont;
                        rtbDocument.ForeColor = ThemeForeColor;
                        resetDocTheme = false;
                    }
                }

                SalvamentoPendente(!firstDocLoad);
            };

            // Criação do WebView2 (modo de visualização)
            webView = new WebView2()
            {
                Dock = DockStyle.Fill,
                Visible = false, // Começa oculto  
                Margin = new Padding(10),
            };
            // Inicializa o WebView2 e configura o evento quando a inicialização estiver concluída
            webView.EnsureCoreWebView2Async();

            // Evento de clique no botão para alternar entre os modos
            ApplyButtomTheme(btnToggleView, true);
            btnToggleView.MouseDown += (s, e) =>
            {
                isDocReadOnly = !isDocReadOnly; // Alterna o estado
                btnToggleView.ButtonText = isDocReadOnly ? "✍" : "👁️";
                if (isDocMode)
                {
                    if (isDocReadOnly)
                    {
                        string docFolder = GetDocFolder();
                        if (Directory.Exists(docFolder))
                        {
                            webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                                "media.local", docFolder, CoreWebView2HostResourceAccessKind.Allow);
                        }
                        webView.CoreWebView2.NavigateToString(ConvertRtfToHtml(currentStructure));
                    }
                    rtbDocument.Visible = !isDocReadOnly;
                    webView.Visible = isDocReadOnly;
                }
            };

            // Cria o TableLayoutPanel
            tblDoc = new TableLayoutPanel()
            {
                Dock = DockStyle.Top,
                Size = new Size(flwExplorer.ClientSize.Width - 10, flwExplorer.ClientSize.Height - 10),
                Margin = new Padding(5, 0, 5, 5),
                ColumnCount = 1, // Agora tem duas colunas (uma para o título e outra para o botão)
                RowCount = 1,    // Duas linhas
                Padding = new Padding(5),
            };

            // Define o estilo das colunas e linhas
            tblDoc.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); // Título ocupa 90%
            tblDoc.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));      // Primeira linha com altura fixa de 30px

            tblDoc.Controls.Add(rtbDocument, 0, 0);       // RichTextBox (célula 0,1)
            tblDoc.Controls.Add(webView, 0, 0);
        }

        private void UpdateThisAppearence()
        {
            SetupPremiumVersion();

            minWidth = ClientSize.Width / 2 - (minWidth % 1); // Largura minima da janela
            minHeight = ClientSize.Height / 2 - (minHeight % 1); // Altura minima da janela

            MetroThemeStyle temaConvertido;
            MetroColorStyle estiloConvertido;
            MetroTheme = Enum.TryParse(ultimoTema, out temaConvertido) ? temaConvertido : MetroThemeStyle.Dark;
            MetroStyle = Enum.TryParse(ultimoEstilo, out estiloConvertido) ? estiloConvertido : MetroColorStyle.Orange;

            EnableConsole(isDevModeEnabled);
            configs.ChangeIcon(this.Icon);
            sobre.ChangeIcon(this.Icon);
            folderIcon = Properties.Resources.exicon_folder; // Ícone de pasta
            docIcon = Properties.Resources.exicon_doc; // Ícone de arquivo

            UpdateTheme();
            UpdateInfos();
        }

        private void SetupPremiumVersion()
        {
            tblControls.ColumnStyles[7].Width = isPremiumVersion ? 22 : 0;
            tblControls.ColumnStyles[8].Width = isPremiumVersion ? 3 : 0;
            tblControls.ColumnStyles[18].Width = isPremiumVersion ? 3 : 0;
            tblControls.ColumnStyles[19].Width = isPremiumVersion ?22 : 0;
            favItem.Visible = isPremiumVersion;
            btnFavorites.Visible = isPremiumVersion;
            btnToggleView.Visible = isPremiumVersion;
        }

        private void UpdateTheme()
        {
            ThemeFont = new Font("Microsoft Sans Serif", 11);
            ThemeFontBold = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);
            HeaderBackColor = _isDarkMode ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 255, 255);
            ThemeBackColor = _isDarkMode ? Color.FromArgb(27, 27, 27) : Color.FromArgb(255, 255, 255);
            ThemeForeColor = _isDarkMode ? Color.FromArgb(190, 190, 190) : Color.FromArgb(17, 17, 17);
            StyleForeColor = MetroColorStyleToColor(MetroStyle);
            ThemeDarkColor = _isDarkMode ? Color.FromArgb(12, 12, 12) : Color.FromArgb(195, 195, 195);
            ThemeBackColorHighlight = _isDarkMode ? Color.FromArgb(95, 95, 95) : Color.FromArgb(195, 195, 195);
            ThemeLightColor = _isDarkMode ? Color.FromArgb(40, 40, 40) : Color.FromArgb(170, 170, 170);
            ThemeBasicColor = _isDarkMode ? Color.FromArgb(35, 35, 35) : Color.FromArgb(195, 195, 195);

            editorToolbox1.MetroTheme = MetroTheme;
            editorToolbox1.DarkMode = (MetroTheme == MetroThemeStyle.Dark);
            editorToolbox1.MetroStyle = MetroStyle;

            mStyleManager.Theme = MetroTheme;
            configs.MetroTheme = MetroTheme;
            windowBar1.MetroTheme = MetroTheme;
            //windowToolBar1.toolBar1.MetroTheme = MetroTheme;
            pathNavigator1.MetroTheme = MetroTheme;

            this.DarkMode = (MetroTheme == MetroThemeStyle.Dark);
            configs.DarkMode = (MetroTheme == MetroThemeStyle.Dark);
            sobre.DarkMode = (MetroTheme == MetroThemeStyle.Dark);
            windowBar1.DarkMode = (MetroTheme == MetroThemeStyle.Dark);
            //windowToolBar1.toolBar1.DarkMode = (MetroTheme == MetroThemeStyle.Dark);
            pathNavigator1.DarkMode = (MetroTheme == MetroThemeStyle.Dark);

            mStyleManager.Style = MetroStyle;
            configs.MetroStyle = MetroStyle;
            windowBar1.MetroStyle = MetroStyle;
            //windowToolBar1.toolBar1.MetroStyle = MetroStyle;
            pathNavigator1.MetroStyle = MetroStyle;

            tblControls.BackColor = ThemeBackColor;
            flwExplorer.BackColor = ThemeBasicColor;
            flwExplorer.ForeColor = ThemeForeColor;
            flwFavorites.BackColor = ThemeBackColor;
            //flwFavorites.ForeColor = ThemeForeColor;
            rtbDebug.BackColor = ThemeBackColor;
            rtbDebug.ForeColor = ThemeForeColor;
            txtSearch.BackColor = ThemeBackColor;
            txtSearch.ForeColor = ThemeForeColor;

            if (lblTitle != null)
            {
                lblTitle.Font = ThemeFontBold;
                lblTitle.BackColor = ThemeBasicColor;
                lblTitle.ForeColor = ThemeForeColor;
            }

            if (btnToggleView != null)
            {
                ApplyButtomTheme(btnToggleView, true);
            }

            if (rtbDocument != null)
            {
                rtbDocument.BackColor = ThemeBasicColor;
                //rtbDocument.ForeColor = ThemeForeColor;
            }
            if (!isDocMode)
            {
                foreach (Button x in flwExplorer.Controls)
                {
                    if (x != null)
                    {
                        x.FlatAppearance.MouseDownBackColor = ThemeBackColorHighlight;
                        x.FlatAppearance.MouseOverBackColor = ThemeLightColor;
                    }
                }
            }

            foreach (Button x in flwFavorites.Controls)
            {
                if (x != null)
                {
                    x.FlatAppearance.MouseDownBackColor = ThemeBackColorHighlight;
                    x.FlatAppearance.MouseOverBackColor = ThemeLightColor;
                }
            }

            if (webView != null && webView.Visible) webView.CoreWebView2.NavigateToString(ConvertRtfToHtml(currentStructure));

            ApplyButtomTheme(btnNewProject, true);
            ApplyButtomTheme(btnOpenProject, true);
            ApplyButtomTheme(btnSaveProject, true);

            ApplyButtomTheme(btnFavorites, true);
            ApplyButtomTheme(btnBack, true);
            ApplyButtomTheme(btnFoward, true);
            ApplyButtomTheme(btnUp, true);

            foreach (Control c in this.Controls)
            {
                c.ForeColor = ThemeForeColor;
                c.BackColor = ThemeBackColor;
            }
            resetDocTheme = true;

            TTask.Delay(1000);
            pathNavigator1.UpdateAdress();
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

        private void ApplyButtomTheme(CustomButton button, bool highlight)
        {
            button.ButtonHighlight = highlight;
            button.ButtonBorderWidth = new Padding(0);
            button.ButtonForeColor = ThemeForeColor;
            button.ButtonBackColor = ThemeBackColor;
            //button.ButtonBorderColor = ThemeForeColor;
            button.ButtonHighlightBackColor = ThemeBackColor;
            button.ButtonHighlightForeColor = StyleForeColor;
            //button.ButtonBorderHighlightColor = StyleForeColor;
        }

        private void ApplyButtomTheme2(CustomButton button, bool highlight)
        {
            button.ButtonFont = new Font("Yu Gothic UI", 12);
            button.ButtonHighlight = true;
            button.ButtonBorderWidth = new Padding(0);
            button.ButtonBackColor = ThemeBasicColor;
            button.ButtonForeColor = ThemeForeColor;
            button.ButtonHighlightBackColor = ThemeBackColor;
            button.ButtonHighlightForeColor = ThemeForeColor;
            //button.ButtonBorderClickable = true;
            //button.ButtonBorderColor = ThemeBackColorHighlight;
            //button.ButtonBorderHighlightColor = ThemeBackColor;
        }

        private void ApplyPageTheme(TabPage page)
        {
            foreach (Control c in page.Controls)
            {
                c.BackColor = HeaderBackColor;
                c.ForeColor = MetroStyle == MetroColorStyle.Lime ? Color.Lime : ThemeForeColor;
            }
        }

        private void UpdateInfos()
        {
            UpdateTitle();
            /*
            string salvarInfo = "Você tem alterações não salvas...";
            
            if (houveAlteracao)
            {
                labelInfo.Text = $"({selectedItemsCount}) {salvarInfo}";
            }
            else
            {
                labelInfo.Text = $"({selectedItemsCount})";
            }
            */
            //txtAdress.Text = caminhoArquivoAtual;
        }

        private void UpdateTitle()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateTitle));
                return;
            }

            if (caminhoColecaoAtual != null || !isNewFile)
            {
                string nomeArquivo = Path.GetFileName(caminhoColecaoAtual);
                string titleName = $"|    {Path.GetFileNameWithoutExtension(GetCurrentDirectory(null))} - {Path.GetFileNameWithoutExtension(nomeArquivo)}";
                this.Text = titleName;
                windowBar1.Title = titleName;
            }
            else
            {
                this.Text = $"|    {Path.GetFileNameWithoutExtension(GetCurrentDirectory(null))} - {langManager[1]}";
                windowBar1.Title = $"|    {langManager[1]}";
            }

            // Ajuste para redimensionar o controle (se necessário)
            windowBar1.Width += 1;
            windowBar1.Width -= 1;
        }

        private void UpdateControlsSize()
        {
            UpdateInfos();

            pathNavigator1.FixSize();

            if (tblDoc != null)
            {
                tblDoc.Size = new Size(flwExplorer.ClientSize.Width - 10, flwExplorer.ClientSize.Height - 10);
            }

            //SetCurrentDirectory(GetCurrentDirectory(null), false);
            /*
            if (isDocMode)
            {
                var doc = collection.Documents.FirstOrDefault(d => d.Directory == GetCurrentDirectory(null));
                string rsc = Path.Combine(GetResourceFolder(), doc.Directory); // caminho para o arquivo .txt
                if (ConvertRtfToHtml(currentStructure) != ReadRscDocument(rsc))
                {
                    SalvamentoPendente(true);
                }
            }*/

            LoadFavorites();
        }

        protected override void OnResizeBegin(EventArgs e)
        {
            base.OnResizeBegin(e);
            flwExplorer.SuspendLayout();
            MinimumSize = new Size(minWidth, minHeight);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            flwExplorer.ResumeLayout();
            if (Width < minWidth) Width = minWidth;
            if (Height < minHeight) Height = minHeight;
            UpdateControlsSize();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            // Desabilitar redimensionamento estando maximizado
            this.Resizable = WindowState != FormWindowState.Maximized;
            /*foreach (Control a in flpBills.Controls)
            {
                a.Width = flpBills.Width - 19;
            }*/

            UpdateControlsSize();

            //listGames.Columns[0].Width = listGames.Width - 25;
            //xlistGames.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (lastWindowState == FormWindowState.Minimized)
            {
                flwExplorer.PerformLayout();
                flwExplorer.Invalidate();
                //flwExplorer.Size = new Size(flwExplorer.Size.Width + 1, flwExplorer.Size.Height + 1);
                //flwExplorer.Size = new Size(flwExplorer.Size.Width - 1, flwExplorer.Size.Height - 1);
            }

            lastWindowState = this.WindowState;
            UpdateControlsSize();
            // nao funciona remover pra dminuir uso de ram
            //if (WindowState == FormWindowState.Minimized) SetWebViewsState(false);
            //if (lastWindowState == FormWindowState.Minimized) SetWebViewsState(true);
        }

        private void InitializeToolBar()
        {
            windowBar1.ClearButtons();
            // Adicione botões à barra de ferramentas (use "-" como separador)
            windowBar1.AddButton(langManager[3], new List<string> { langManager[4], langManager[5], "-", langManager[6], langManager[7], "-", langManager[8] }, index =>
            {
                if (index == 0)
                {
                    if (SalvarAntesDeFechar())
                    {
                        NovoArquivo();
                    }
                }
                else if (index == 1)
                {
                    if (SalvarAntesDeFechar())
                    {
                        if (!string.IsNullOrEmpty(caminhoColecaoAtual))
                        {
                            using (var ofd = new OpenFileDialog
                            {
                                Filter = $"{langManager[9]} DUS (*.dus)|*.dus",
                                InitialDirectory = Path.GetDirectoryName(caminhoColecaoAtual),
                                FileName = Path.GetFileName(caminhoColecaoAtual)
                            })
                            {
                                if (ofd.ShowDialog() == DialogResult.OK)
                                {
                                    // Chama o método AbrirArquivo com o caminho selecionado
                                    AbrirArquivo(ofd.FileName);
                                }
                            }
                        }
                        else
                        {
                            using (var ofd = new OpenFileDialog
                            {
                                Filter = $"{langManager[9]} DUS (*.dus)|*.dus",
                                InitialDirectory = $"{langManager[2]}" + ".dus",
                                FileName = $"{langManager[2]}" + ".dus"
                            })
                            {
                                if (ofd.ShowDialog() == DialogResult.OK)
                                {
                                    // Chama o método AbrirArquivo com o caminho selecionado
                                    AbrirArquivo(ofd.FileName);
                                }
                            }
                        }
                    }
                }
                else if (index == 3)
                {
                    if (!string.IsNullOrEmpty(caminhoColecaoAtual))
                    {
                        SalvarArquivo(caminhoColecaoAtual);
                    }
                    else
                    {
                        using (var sfd = new SaveFileDialog
                        {
                            Filter = $"{langManager[9]} DUS (*.dus)|*.dus",
                            InitialDirectory = $"{langManager[2]}" + ".dus",
                            FileName = $"{langManager[2]}" + ".dus"
                        })
                        {
                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                // Chama o método SalvarArquivo com o caminho selecionado
                                SalvarArquivo(sfd.FileName);
                            }
                        }
                    }
                }
                else if (index == 4)
                {
                    if (!string.IsNullOrEmpty(caminhoColecaoAtual))
                    {
                        using (var sfd = new SaveFileDialog
                        {                        
                            Filter = $"{langManager[9]} DUS (*.dus)|*.dus",
                            InitialDirectory = Path.GetDirectoryName(caminhoColecaoAtual),
                            FileName = Path.GetFileName(caminhoColecaoAtual)
                        })
                        {
                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                // Chama o método SalvarArquivo com o caminho selecionado
                                SalvarArquivo(sfd.FileName);
                            }
                        }
                    }
                    else
                    {
                        using (var sfd = new SaveFileDialog
                        {
                            Filter = $"{langManager[9]} DUS (*.dus)|*.dus",
                            InitialDirectory = $"{langManager[2]}" + ".dus",
                            FileName = $"{langManager[2]}" + ".dus"
                        })
                        {
                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                // Chama o método SalvarArquivo com o caminho selecionado
                                SalvarArquivo(sfd.FileName);
                            }
                        }
                    }
                }
                else if (index == 6)
                {
                    Application.Exit();
                }
            });

            windowBar1.AddButton(langManager[10], new List<string> {langManager[13], "-", $"{langManager[14]} .DUS", langManager[15] }, index =>
            {/*
                if (index == 0)
                {
                    isAutoSaveEnabled = !isAutoSaveEnabled;
                    Debug(isDebugModeEnabled, $"isAutoSaveEnabled {isAutoSaveEnabled}");
                }*/
                if (index == 0)
                {
                    isRollDownEnabled = !isRollDownEnabled;
                    Debug(isDebugModeEnabled, $"isRollDownEnabled {isRollDownEnabled}");
                }
                else if (index == 2)
                {
                    // Pergunta ao usuário se deseja continuar com a operação
                    var dialogResult = MessageBox.Show(
                        langManager[16],
                        langManager[17],
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2); // Define o botão "Cancelar" como o padrão

                    // Se o usuário clicar em "Sim"
                    if (dialogResult == DialogResult.OK)
                    {
                        // Caminho da pasta onde o ícone está localizado (por exemplo, na pasta "Assets")
                        string iconDirectory = Path.Combine(Application.StartupPath, "Assets");

                        // Caminho completo do ícone
                        string iconFilePath = Path.Combine(iconDirectory, "dus.ico");

                        // Verifica se o arquivo de ícone existe
                        if (File.Exists(iconFilePath))
                        {
                            // Registra a extensão com o ícone localizado na pasta "Assets"
                            RegisterFileType(".dus", "Modus.dusfile", Application.ExecutablePath, iconFilePath);
                        }
                        else
                        {
                            Debug(isErrorLogEnabled, "[Associar .DUS] Ícone não encontrado na pasta 'Assets'.");
                        }
                    }
                }
                else if (index == 3)
                {
                    configs.UpdateUserSettings(MetroTheme, MetroStyle, Language);
                    configs.ShowDialog();
                }
            });

            // Ajuda
            windowBar1.AddButton(langManager[18], new List<string> { langManager[19] }, index =>
            {
                // Sobre
                if (index == 0)
                {
                    sobre.UpdateUserSettings(MetroTheme, MetroStyle, Language);
                    sobre.ShowDialog();
                }
            });

            // Linguagem
            List<string> availableLanguages = langManager.GetAvailableLanguages();

            // Criar botão com todas as opções de idioma disponíveis
            windowBar1.AddButton(langManager[20], availableLanguages, index =>
            {
                if (index >= 0 && index < availableLanguages.Count)
                {
                    string selectedLanguage = availableLanguages[index];
                    langManager.LoadLanguage(selectedLanguage);
                    Language = selectedLanguage;
                }
            });

            if (isDevModeEnabled)
            {
                // Desenvolvedor
                windowBar1.AddButton(langManager[11], null, index =>
                {
                    isDebugModeEnabled = !isDebugModeEnabled;
                    EnableConsole(isDebugModeEnabled);
                    Debug(isDebugModeEnabled, $"isDebugModeEnabled {isDebugModeEnabled}");
                });
            }
        }

        private void EnableConsole(bool value)
        {
            isDebugModeEnabled = value;
            tableLayoutPanel1.RowStyles[1].Height = isDevModeEnabled ? 200 : 0;
            UpdateControlsSize();
        }

        public void MoverIconeParaAssets()
        {
            string iconName = "dus.ico";  // Nome do ícone nos recursos
            string assetsFolder = Path.Combine(Application.StartupPath, "Assets");  // Caminho para a pasta Assets

            // Certifica-se de que a pasta "Assets" existe
            if (!Directory.Exists(assetsFolder))
            {
                Directory.CreateDirectory(assetsFolder);
            }

            // Caminho de destino do ícone dentro da pasta Assets
            string destinationPath = Path.Combine(assetsFolder, iconName);

            if (!File.Exists(destinationPath))
            {
                // Salva o ícone extraído dos recursos para a pasta Assets
                using (FileStream fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
                {
                    // Salva o recurso de ícone no arquivo
                    Properties.Resources.dus.Save(fileStream);
                }

                // Torna o ícone invisível (oculto) após a cópia
                File.SetAttributes(destinationPath, File.GetAttributes(destinationPath) | FileAttributes.Hidden);

                //Debug(enableDebug, $"Ícone movido para: {destinationPath} e agora está oculto.");
            }
        }

        public static void RegisterFileType(string extension, string progId, string applicationPath, string iconPath)
        {
            try
            {
                // Caminho base em HKEY_CURRENT_USER
                string baseKey = @"Software\Classes";

                // Verifica se o caminho do executável existe
                if (!File.Exists(applicationPath))
                {
                    MessageBox.Show("Aplicativo não encontrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 1. Verifica se a extensão já está associada a algum ProgID
                using (RegistryKey extKey = Registry.CurrentUser.OpenSubKey($@"{baseKey}\{extension}", true))
                {
                    if (extKey == null)
                    {
                        // Associa a extensão ao ProgID, caso ainda não tenha sido feita a associação
                        using (RegistryKey newExtKey = Registry.CurrentUser.CreateSubKey($@"{baseKey}\{extension}"))
                        {
                            newExtKey?.SetValue("", progId);
                        }
                    }
                    else
                    {
                        // Caso a chave já exista, apenas atualiza o ProgID
                        extKey.SetValue("", progId);
                    }
                }

                // 2. Define o ProgID e as configurações
                using (RegistryKey progIdKey = Registry.CurrentUser.CreateSubKey($@"{baseKey}\{progId}", true))
                {
                    progIdKey?.SetValue("", "Arquivo DUS");

                    // Comando de abertura
                    progIdKey?.CreateSubKey(@"shell\open\command")?.SetValue("", $"\"{applicationPath}\" \"%1\"");

                    // Ícone do arquivo
                    progIdKey?.CreateSubKey("DefaultIcon")?.SetValue("", $"\"{iconPath}\",0");
                }

                // Notifica o Windows sobre a alteração
                SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);

                //MessageBox.Show("Registro concluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao registrar o arquivo: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [System.Runtime.InteropServices.DllImport("shell32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern void SHChangeNotify(int wEventId, int uFlags, IntPtr dwItem1, IntPtr dwItem2);

        private void LoadUserSettings()
        {
            string dataFolder = Path.Combine(appData, "Data");
            string userSettingsPath = Path.Combine(dataFolder, "user.xml");

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }
            if (Directory.Exists(dataFolder))
            {
                if (FFile.Exists(userSettingsPath))
                {
                    var xml = XDocument.Load(userSettingsPath);
                    var settingsElement = xml.Element("Settings");

                    if (settingsElement != null)
                    {
                        Debug(true, $"[{DateTime.UtcNow}] {NomeDoProjeto} {ObterVersaoDoArquivo()}");
                        //Debug(true, $"{langManager[2]} {langManager[3]}");
                        var ultimoTemaElement = settingsElement.Element("UltimoTema");
                        var ultimoEstiloElement = settingsElement.Element("UltimoEstilo");

                        var linguagemElement = settingsElement.Element("Linguagem");
                        var modoDebugElement = settingsElement.Element("ModoDebug");
                        var exibirFavoritosElement = settingsElement.Element("ExibirFavoritos");
                        var somenteLeituraElement = settingsElement.Element("SomenteLeitura");
                        var salvamentoAutomaticoElement = settingsElement.Element("SalvamentoAutomatico");
                        var rolarPraBaixoElement = settingsElement.Element("RolarPraBaixo");

                        if (ultimoTemaElement != null) ultimoTema = ultimoTemaElement.Value;
                        if (ultimoEstiloElement != null) ultimoEstilo = ultimoEstiloElement.Value;

                        if (linguagemElement != null) Language = linguagemElement.Value;
                        if (modoDebugElement != null) isDebugModeEnabled = (bool.Parse(modoDebugElement.Value) && isDevModeEnabled);
                        if (exibirFavoritosElement != null && isPremiumVersion) isFavMenuShowing = bool.Parse(exibirFavoritosElement.Value);
                        if (somenteLeituraElement != null && isPremiumVersion) isDocReadOnly = bool.Parse(somenteLeituraElement.Value);
                        if (salvamentoAutomaticoElement != null) isAutoSaveEnabled = bool.Parse(salvamentoAutomaticoElement.Value);
                        if (rolarPraBaixoElement != null) isRollDownEnabled = bool.Parse(rolarPraBaixoElement.Value);
                    }
                }
            }
        }

        private void SaveUserSettings()
        {
            saveQueue.EnqueueSave(() =>
            {
                string dataFolder = Path.Combine(appData, "Data");
                string userSettingsPath = Path.Combine(dataFolder, "user.xml");
                var xml = new XDocument();

                if (FFile.Exists(userSettingsPath))
                {
                    xml = XDocument.Load(userSettingsPath);
                }
                else
                {
                    xml.Add(new XElement("Settings"));
                }

                var settingsElement = xml.Element("Settings");

                if (settingsElement == null)
                {
                    settingsElement = new XElement("Settings");
                    xml.Add(settingsElement);
                }

                settingsElement.SetElementValue("UltimoTema", MetroTheme.ToString());
                settingsElement.SetElementValue("UltimoEstilo", MetroStyle.ToString());

                settingsElement.SetElementValue("Linguagem", Language);
                settingsElement.SetElementValue("ModoDebug", isDebugModeEnabled);
                settingsElement.SetElementValue("ExibirFavoritos", isFavMenuShowing);
                settingsElement.SetElementValue("SomenteLeitura", isDocReadOnly);
                settingsElement.SetElementValue("SalvamentoAutomatico", isAutoSaveEnabled);
                settingsElement.SetElementValue("RolarPraBaixo", isRollDownEnabled);
                
                xml.Save(userSettingsPath);
                return Task.CompletedTask;
            });
        }

        private void LoadPreviousFile()
        {
            if (!string.IsNullOrEmpty(ultimaColecao) && FFile.Exists(ultimaColecao))
            {
                // Se existir, carregue o último arquivo
                AbrirArquivo(ultimaColecao);

                //CarregarListaDeArquivo(ultimoArquivo);
            }
            else
            {
                NovoArquivo();
                // Caso contrário, inicie um novo arquivo ou tome outras ações necessárias
                // ...
            }
            UpdateInfos();
        }

        private void AbrirArquivo(string caminho)
        {
            isNewFile = false;
            if (!string.IsNullOrEmpty(caminho) && FFile.Exists(caminho) && IsValidFile(caminho))
            {
                caminhoColecaoAtual = caminho;
                ultimaColecao = caminhoColecaoAtual;
                Properties.Settings.Default.UltimaColecao = ultimaColecao;
                Properties.Settings.Default.Save();

                try
                {
                    //string decryptedXml = DecryptXmlBytes(caminhoColecaoAtual, password); // Descriptografa o conteúdo do XML

                    // Agora, você tem o conteúdo XML como string, então você pode carregá-lo usando o XmlSerializer
                    // Le o xml
                    var serializer = new XmlSerializer(typeof(Collection));
                    using (var reader = new StreamReader(caminhoColecaoAtual))
                    {
                        collection = (Collection)serializer.Deserialize(reader);
                    }

                    //string xmlContent = FFile.ReadAllText(caminhoColecaoAtual);
                    //Debug(enableDebug, $"[AbrirArquivo] Conteúdo do XML: " + xmlContent);

                    if (collection == null)
                    {
                        Debug(isErrorLogEnabled, "Erro: A coleção está nula.");
                        return;
                    }

                    Debug(isDebugModeEnabled, $"[AbrirArquivo] Coleção carregada com sucesso.");

                    VerificarERepararRecursos();
                    btnToggleView.ButtonText = isDocReadOnly ? "✍" : "👁️";
                    LoadFavorites();
                    SetFavMenuVisibility(isFavMenuShowing);
                    if (DirectoryExists(collection.Cache.LastDirectory))
                    {
                        //SetCurrentDirectory(collection.Cache.LastDirectory, true);
                        if (DocExists(collection.Cache.LastDirectory))
                        {
                            //string rsc = Path.Combine(GetResourceFolder(), doc.Directory); // caminho para o arquivo .txt
                            var doc = collection.Documents.FirstOrDefault(d => d.Directory == collection.Cache.LastDirectory);
                            //currentStructure = ConvertBbCodeToRtf(GetDocStructure(doc.Directory, doc.Structure), ThemeFont, ThemeForeColor);

                            //Debug(isDebugConsoleEnabled, $"currentStructure {currentStructure}");
                        }
                        SetCurrentDirectory(collection.Cache.LastDirectory, true);
                    }
                    else
                    {
                        SetCurrentDirectory(InitialDirectory, true);
                    }

                    /*
                    // Verificar se a pasta de mídia existe
                    string resourceFolder = GetResourceFolder();
                    if (!Directory.Exists(resourceFolder))
                    {
                        MessageBox.Show("Aviso: Pasta de recursos não encontrada. Algumas mídias podem estar ausentes.",
                                        "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }*/
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{langManager[21]} " + ex.Message);
                }
            }
            else
            {
                NovoArquivo();
            }
        }

        private void OnAutoSave(object sender, EventArgs e)
        {
            if (!isAutoSaveEnabled) return;

            if (!string.IsNullOrEmpty(caminhoColecaoAtual))
            {
                SalvarArquivo(caminhoColecaoAtual);
                //InvokeIfRequired(rtbDebug, () => { Debug(isDebugConsoleEnabled, $"[OnAutoSave] AutoSave completed at {DateTime.Now}"); });
            }
            else
            {
                using (var sfd = new SaveFileDialog
                {
                    Filter = $"{langManager[9]} DUS (*.dus)|*.dus",
                    InitialDirectory = $"{langManager[2]}" + ".dus",
                    FileName = $"{langManager[2]}" + ".dus"
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        // Chama o método SalvarArquivo com o caminho selecionado
                        SalvarArquivo(sfd.FileName);
                    }
                }
            }
        }

        private void SalvarArquivo(string caminho)
        {
            saveQueue.EnqueueSave(() =>
            {
                string lastCollectionPath = caminhoColecaoAtual;
                caminhoColecaoAtual = caminho;
                ultimaColecao = caminhoColecaoAtual;
                Properties.Settings.Default.UltimaColecao = ultimaColecao;
                Properties.Settings.Default.Save();
                SalvamentoPendente(false);

                try
                {
                    if (isDocMode)
                    {
                        string projectFolder = Path.GetDirectoryName(caminhoColecaoAtual);
                        string resourceFolder = Path.Combine(projectFolder, Path.GetFileNameWithoutExtension(caminhoColecaoAtual) + "_rsc");

                        var doc = collection.Documents.FirstOrDefault(d => d.Directory == GetCurrentDirectory(null));
                        string filePath = Path.Combine(resourceFolder, doc.Directory);

                        //Debug(true, $"caminho {caminho} filePath {filePath}");
                        if (doc.Structure != "[ondir]") doc.Structure = WebUtility.HtmlDecode(ConvertRtfToBBCode(currentStructure));
                        WriteRscDocument(filePath, WebUtility.HtmlDecode(ConvertRtfToBBCode(currentStructure)));
                        doc.ModifiedDate = DateTime.UtcNow;

                        //Debug(isDebugModeEnabled, $"BBCODE: {WebUtility.HtmlDecode(ConvertRtfToBBCode(currentStructure))}");
                        //Debug(isDebugModeEnabled, $"RTF: {currentStructure}");
                    }

                    // Escrever arquivo xml
                    collection.Cache.LastDirectory = GetCurrentDirectory(null);
                    var serializer = new XmlSerializer(typeof(Collection));
                    using (var writer = new StreamWriter(caminho))
                    {
                        serializer.Serialize(writer, collection);
                    }
                    /*
                    // Serializa e Encripta a coleção para XML em uma string 
                    var serializer = new XmlSerializer(typeof(Collection));
                    using (var writer = new StringWriter())
                    {
                        serializer.Serialize(writer, collection);

                        // Agora temos o conteúdo XML como string
                        string xmlContent = writer.ToString();

                        // Converte a string XML para um array de bytes
                        byte[] xmlBytes = Encoding.UTF8.GetBytes(xmlContent);

                        // Chama a função de criptografia passando os bytes do XML
                        EncryptXmlBytes(xmlBytes, caminho, password);
                    }
                    */

                    if (!isNewFile)
                    {
                        // ao salvar, é necessario mover a pasta de recursos para o novo diretorio de salvamento
                        string resourceFolder = GetResourceFolder();
                        string lastProjectFolder = Path.GetDirectoryName(lastCollectionPath);
                        string lastRsc = Path.Combine(lastProjectFolder, Path.GetFileNameWithoutExtension(lastCollectionPath) + "_rsc");
                        //Debug(isDebugConsoleEnabled, $"resourceFolder {resourceFolder} | lastRsc {lastRsc} ");

                        // Se a pasta de rsc existir, copia tudo para o novo local
                        if (Directory.Exists(lastRsc) && lastRsc != resourceFolder)
                        {
                            CopyDirectory(lastRsc, resourceFolder);
                        }
                    }
                    else
                    {
                        isNewFile = false;
                        string resourceFolder = GetResourceFolder();
                        //Debug(isDebugConsoleEnabled, $"isnewfile {isNewFile} || resourceFolder {resourceFolder} || tempFolder_rsc {tempFolder + "_rsc"}");

                        // Se a pasta de rsc existir, move tudo para o novo local
                        if (Directory.Exists(tempFolder + "_rsc"))
                        {
                            CopyDirectory(tempFolder + "_rsc", resourceFolder);
                            Directory.Delete(tempFolder + "_rsc", true);
                        }
                    }

                    VerificarEExcluirRecursos();
                    Debug(isDebugModeEnabled, $"Arquivo salvo com sucesso em: " + caminho);

                    // Listando Bibliotecas Nao Usadas
                    //SeparateUsings();

                    //Debug(enableDebug, $"ProjectName: {Path.GetFileNameWithoutExtension(caminhoColecaoAtual)}\r\n{ConverterXmlParaTextoFacilDeLer(collection)}");

                    //string xmlContent = FFile.ReadAllText(caminhoColecaoAtual);
                    //Debug(enableDebug, $"{ResumirXML(xmlContent)}");

                    // Encriptacao
                    //EncryptXml(caminho, @"C:\Users\Asen\Desktop\ed\encrypted.xml", password);
                    //DecryptXml(@"C:\Users\Asen\Desktop\ed\encrypted.xml", @"C:\Users\Asen\Desktop\ed\newdoc.xml", password);


                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{langManager[22]} {ex.Message}", langManager[25], MessageBoxButtons.OK, MessageBoxIcon.Error);

                    caminhoColecaoAtual = lastCollectionPath;
                    ultimaColecao = caminhoColecaoAtual;
                    Properties.Settings.Default.UltimaColecao = ultimaColecao;
                    Properties.Settings.Default.Save();
                    SalvamentoPendente(true);
                }

                return Task.CompletedTask;
            });
        }

        // Função para copiar diretórios recursivamente
        private void CopyDirectory(string sourceDir, string destinationDir)
        {
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            // Copia todos os arquivos
            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                string destFilePath = Path.Combine(destinationDir, Path.GetFileName(filePath));
                File.Copy(filePath, destFilePath, true); // 'true' sobrescreve se já existir
            }

            // Copia todas as subpastas recursivamente
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string destSubDir = Path.Combine(destinationDir, Path.GetFileName(subDir));
                CopyDirectory(subDir, destSubDir); // Chamada recursiva
            }
        }

        private void SeparateUsings()
        {
            string ProjUsings = ListUsings("C:\\_VSFiles\\Projects\\PC Solutions\\Validata");
            string CSProjUsings = ExtractUsingsFromFile("C:\\_VSFiles\\Projects\\PC Solutions\\Validata\\csprojusings.txt");
            string ToRemoveUsings = "C:\\_VSFiles\\Projects\\PC Solutions\\Validata\\csprojusings_toremove.txt";

            // Garante que o arquivo seja criado e fechado corretamente
            if (!File.Exists(ToRemoveUsings))
            {
                using (File.Create(ToRemoveUsings)) { }
            }

            string notUsed = NotUsedUsings(ProjUsings, CSProjUsings);

            if (!string.IsNullOrEmpty(notUsed))
            {
                File.WriteAllText(ToRemoveUsings, $"Remove Usings:\r\n{notUsed}");
            }
            else
            {
                File.WriteAllText(ToRemoveUsings, "Nenhum using para remover.");
            }
        }

        private bool IsValidFile(string caminho)
        {
            try
            {
                bool enableDebug = false;

                // Verifica a extensão do arquivo
                string extension = Path.GetExtension(caminho).ToLower();
                if (extension != ".dus")
                {
                    Debug(enableDebug, $"Falha na validação: O arquivo não é DUS (extensão inválida).");
                    return false;
                }

                // Deserializa o arquivo XML
                Debug(enableDebug, $"Iniciando deserialização do arquivo DUS...");

                var serializer = new XmlSerializer(typeof(Collection));
                using (var reader = new StreamReader(caminho))
                {
                    var collection = (Collection)serializer.Deserialize(reader);

                    if (collection == null)
                    {
                        Debug(enableDebug, $"Falha na deserialização: A coleção resultante é null.");
                        return false;
                    }

                    Debug(enableDebug, $"Deserialização bem-sucedida. Iniciando validações...");

                    // Valida Cache
                    if (collection.Cache == null)
                    {
                        Debug(enableDebug, $"Falha na validação do Cache: Cache é null.");
                        return false;
                    }

                    if (string.IsNullOrEmpty(collection.Cache.LastDirectory))
                    {
                        Debug(enableDebug, $"Falha na validação do Cache: LastDirectory está vazio.");
                        return false;
                    }

                    Debug(enableDebug, $"Cache válido.");

                    // Valida Folders
                    if (collection.Folders == null || collection.Folders.Count == 0)
                    {
                        Debug(enableDebug, $"Falha na validação de Pastas: Nenhuma pasta encontrada.");
                        return false;
                    }

                    foreach (var folder in collection.Folders)
                    {
                        if (string.IsNullOrEmpty(folder.Directory))
                        {
                            //Debug(enableDebug, $"Falha na validação de Pasta: Diretório vazio na pasta {folder.Directory}.");
                            return false;
                        }
                    }

                    Debug(enableDebug, $"Todas as pastas são válidas.");

                    // Valida Documents
                    if (collection.Documents == null || collection.Documents.Count == 0)
                    {
                        Debug(enableDebug, $"Falha na validação de Documentos: Nenhum documento encontrado.");
                        return true;
                    }

                    foreach (var document in collection.Documents)
                    {
                        if (string.IsNullOrEmpty(document.Directory) || string.IsNullOrEmpty(document.Structure))
                        {
                            Debug(enableDebug, $"Falha na validação de Documento: Diretório ou Estrutura vazio para o documento {document.Directory}.");
                            return false;
                        }
                    }

                    Debug(enableDebug, $"Todos os documentos são válidos.");
                }

                Debug(enableDebug, $"Validação bem-sucedida: O arquivo DUS é válido.");
                return true;
            }
            catch (Exception ex)
            {
                // Captura exceções de deserialização e outros erros
                Debug(isDebugModeEnabled, $"Erro na validação: {ex.Message}");
                return false;
            }
        }

        private void WriteRscDocument(string path, string content)
        {
            saveQueue.EnqueueSave(() =>
            {
                EncryptDocument(password, content, path);
                //File.WriteAllText(path, content);
                return Task.CompletedTask;
            });
        }

        private string ReadRscDocument(string path)
        {
            string content;
            try
            {
                content = DecryptDocument(password, path); // File.ReadAllText(path);
            }
            catch
            {
                content = string.Empty;
            }
            return content;
        }

        private string GetDocStructure(string directory, string structure)
        {
            string rsc = Path.Combine(GetResourceFolder(), directory);
            string rsccont = File.Exists(rsc) ? ReadRscDocument(rsc) : string.Empty;
            return (structure == "[ondir]") ? rsccont : WebUtility.HtmlDecode(structure);
        }

        public void EncryptDocument(string password, string content, string outputFile)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16];  // Gerar um salt aleatório
                rng.GetBytes(salt);

                // Deriva a chave e o IV da senha e do salt
                using (var keyDerivationFunction = new Rfc2898DeriveBytes(password, salt))
                {
                    byte[] key = keyDerivationFunction.GetBytes(16); // Tamanho da chave AES (16 bytes)
                    byte[] iv = keyDerivationFunction.GetBytes(16);  // Tamanho do IV (16 bytes)

                    using (Aes aesAlg = Aes.Create())
                    {
                        aesAlg.Key = key;
                        aesAlg.IV = iv;
                        aesAlg.Padding = PaddingMode.PKCS7;  // Garantir o uso do padding PKCS7

                        using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                        {
                            using (FileStream fsEncrypt = new FileStream(outputFile, FileMode.Create))
                            {
                                // Escreve o salt no início do arquivo
                                fsEncrypt.Write(salt, 0, salt.Length);

                                using (CryptoStream csEncrypt = new CryptoStream(fsEncrypt, encryptor, CryptoStreamMode.Write))
                                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                                {
                                    swEncrypt.Write(content);
                                }
                            }
                        }
                    }
                }
            }
        }

        public string DecryptDocument(string password, string inputFile)
        {
            // Lê o salt do arquivo
            byte[] salt = new byte[16];  // Espera um salt de 16 bytes
            using (FileStream fsDecrypt = new FileStream(inputFile, FileMode.Open))
            {
                fsDecrypt.Read(salt, 0, salt.Length);

                // Deriva a chave e o IV da senha e do salt
                using (var keyDerivationFunction = new Rfc2898DeriveBytes(password, salt))
                {
                    byte[] key = keyDerivationFunction.GetBytes(16); // Tamanho da chave AES (16 bytes)
                    byte[] iv = keyDerivationFunction.GetBytes(16);  // Tamanho do IV (16 bytes)

                    using (Aes aesAlg = Aes.Create())
                    {
                        aesAlg.Key = key;
                        aesAlg.IV = iv;
                        aesAlg.Padding = PaddingMode.PKCS7;  // Garantir o uso do padding PKCS7

                        using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                        {
                            using (CryptoStream csDecrypt = new CryptoStream(fsDecrypt, decryptor, CryptoStreamMode.Read))
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();  // Lê o conteúdo descriptografado
                            }
                        }
                    }
                }
            }
        }


        public static class CryptoHelper
        {
            // Método para gerar a chave e o IV a partir de uma senha
            public static (byte[] key, byte[] iv) DeriveKeyAndIV(string password)
            {
                // Salt fixo ou pode ser gerado de maneira segura, como armazenar em um arquivo ou banco de dados
                byte[] salt = new byte[16];  // Usando um salt fixo de 16 bytes

                using (var keyDerivationFunction = new Rfc2898DeriveBytes(password, salt))
                {
                    byte[] key = keyDerivationFunction.GetBytes(16);  // Tamanho da chave AES (16 bytes)
                    byte[] iv = keyDerivationFunction.GetBytes(16);   // Tamanho do IV (16 bytes)

                    return (key, iv);
                }
            }
        }

        public static void EncryptXmlBytes(byte[] xmlBytes, string encryptedFilePath, string password)
        {
            // Obter chave e IV com base na senha
            var (key, iv) = CryptoHelper.DeriveKeyAndIV(password);

            using (AesManaged aes = new AesManaged())
            {
                aes.Key = key;
                aes.IV = iv;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        // Escreve os bytes do XML criptografado no CryptoStream
                        cryptoStream.Write(xmlBytes, 0, xmlBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }

                    // Salva os bytes criptografados no arquivo
                    File.WriteAllBytes(encryptedFilePath, ms.ToArray());
                }
            }
        }

        public static void EncryptXml(string xmlFilePath, string encryptedFilePath, string password)
        {
            // Obter chave e IV com base na senha
            var (key, iv) = CryptoHelper.DeriveKeyAndIV(password);

            using (AesManaged aes = new AesManaged())
            {
                aes.Key = key;
                aes.IV = iv;

                using (FileStream fsOutput = new FileStream(encryptedFilePath, FileMode.Create))
                using (CryptoStream cryptoStream = new CryptoStream(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (XmlTextWriter xmlWriter = new XmlTextWriter(cryptoStream, Encoding.UTF8))
                {
                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.Load(xmlFilePath);
                    xmlDoc.WriteTo(xmlWriter);  // Criptografa e escreve no CryptoStream
                }
            }
        }

        public static string DecryptXmlBytes(string encryptedFilePath, string password)
        {
            // Obter chave e IV com base na senha
            var (key, iv) = CryptoHelper.DeriveKeyAndIV(password);

            using (AesManaged aes = new AesManaged())
            {
                aes.Key = key;
                aes.IV = iv;

                using (FileStream fsInput = new FileStream(encryptedFilePath, FileMode.Open))
                using (CryptoStream cryptoStream = new CryptoStream(fsInput, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    // Lê os bytes descriptografados
                    using (MemoryStream ms = new MemoryStream())
                    {
                        cryptoStream.CopyTo(ms);
                        byte[] decryptedBytes = ms.ToArray();

                        // Converte os bytes descriptografados para string
                        return Encoding.UTF8.GetString(decryptedBytes);
                    }
                }
            }
        }

        public static void DecryptXml(string encryptedFilePath, string decryptedFilePath, string password)
        {
            // Obter chave e IV com base na senha
            var (key, iv) = CryptoHelper.DeriveKeyAndIV(password);

            using (AesManaged aes = new AesManaged())
            {
                aes.Key = key;
                aes.IV = iv;

                using (FileStream fsInput = new FileStream(encryptedFilePath, FileMode.Open))
                using (CryptoStream cryptoStream = new CryptoStream(fsInput, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    // Carrega o XML descriptografado
                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.Load(cryptoStream);  // Descriptografa e carrega o conteúdo

                    // Configura o XmlWriterSettings para adicionar indentação
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;  // Habilita a indentação
                    settings.IndentChars = "\t";  // Usa tabulação (em vez de espaços)

                    using (XmlWriter writer = XmlWriter.Create(decryptedFilePath, settings))
                    {
                        xmlDoc.Save(writer);  // Salva o XML com formatação
                    }
                }
            }
        }

        public void VerificarEExcluirRecursos()
        {
            bool enableDebug = false;
            string resourceFolder = GetResourceFolder();
            var xmlFolders = collection.Folders.Select(f => NormalizePath(f.Directory)).ToList();
            var xmlDocuments = collection.Documents.Select(d => NormalizePath(d.Directory)).ToList();
            var xmlPaths = xmlFolders.Concat(xmlDocuments).ToList();

            var allFilesInResourceFolder = Directory.GetFiles(resourceFolder, "*", SearchOption.AllDirectories).ToList();
            var allDirsInResourceFolder = Directory.GetDirectories(resourceFolder, "*", SearchOption.AllDirectories).ToList();

            // Combine diretórios e arquivos e ordene para processar as pastas primeiro
            var allPaths = allDirsInResourceFolder.Concat(allFilesInResourceFolder)
                .OrderBy(path => path.EndsWith("_rsc") ? 0 : 1) // Prioriza pastas com o sufixo _rsc
                .ThenBy(path => path) // Ordena lexicograficamente o restante
                .ToList();

            var preservedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var filePath in allPaths)
            {
                var relativePath = NormalizePath(GetRelativePath(resourceFolder, filePath));
                Debug(enableDebug, $"Verificando: {filePath}, Caminho relativo: {relativePath}");

                if (relativePath.EndsWith("_rsc"))
                {
                    // Remove apenas o sufixo "_rsc" para obter o nome base
                    string folderName = Path.GetFileName(relativePath);
                    string baseName = folderName.Substring(0, folderName.Length - 4);

                    // Tente encontrar um documento cujo nome e diretório pai correspondam
                    var correspondingDoc = xmlDocuments.FirstOrDefault(doc =>
                        Path.GetFileNameWithoutExtension(doc).Equals(baseName, StringComparison.OrdinalIgnoreCase) &&
                        NormalizePath(Path.GetDirectoryName(doc)).Equals(NormalizePath(Path.GetDirectoryName(relativePath)), StringComparison.OrdinalIgnoreCase)
                    );

                    if (correspondingDoc != null)
                    {
                        string expectedRscPath = NormalizePath(Path.Combine(Path.GetDirectoryName(correspondingDoc), baseName + "_rsc"));
                        string currentRscPath = NormalizePath(relativePath);

                        if (currentRscPath == expectedRscPath)
                        {
                            Debug(enableDebug, $"correspondingDoc encontrado para: {baseName}");
                            Debug(enableDebug, $"Preservado (recursos válidos): {relativePath}");
                            preservedPaths.Add(currentRscPath);
                            foreach (var subFile in Directory.GetFileSystemEntries(filePath, "*", SearchOption.AllDirectories))
                            {
                                preservedPaths.Add(NormalizePath(GetRelativePath(resourceFolder, subFile)));
                            }
                            continue;
                        }
                        else
                        {
                            Debug(enableDebug, $"Local incorreto para pasta de recursos: {relativePath}, esperado: {expectedRscPath}");
                        }
                    }

                    TryDelete(filePath, relativePath, "pasta de recursos órfã");
                    continue;
                }


                bool isPathInXml = xmlPaths.Any(p => string.Equals(p, relativePath, StringComparison.OrdinalIgnoreCase));

                if (!isPathInXml && !preservedPaths.Contains(relativePath))
                {
                    TryDelete(filePath, relativePath, "Arquivo/Pasta não listado no XML");
                }
                else
                {
                    Debug(enableDebug, $"Preservado: {relativePath}");
                    preservedPaths.Add(relativePath);
                }
            }
        }

        private string NormalizePath(string path)
        {
            return path.Replace("\\", "/").Trim().ToLowerInvariant();
        }

        private void TryDelete(string path, string relativePath, string reason)
        {
            bool enableDebug = false;
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    Debug(enableDebug, $"{reason} - Pasta excluída: {path}");
                }
                else if (File.Exists(path))
                {
                    File.Delete(path);
                    Debug(enableDebug, $"{reason} - Arquivo excluído: {path}");
                }
            }
            catch (Exception ex)
            {
                Debug(enableDebug, $"{reason} - Erro ao excluir {path}: {ex.Message}");
            }
        }

        public void VerificarERepararRecursos()
        {
            bool enableDebug = false;
            string resourceFolder = GetResourceFolder();

            // Obter as pastas e documentos listados no XML
            var xmlFolders = collection.Folders.Select(f => f.Directory).ToList();
            var xmlDocuments = collection.Documents.Select(d => d.Directory).ToList();
            var xmlPaths = xmlFolders.Concat(xmlDocuments).ToList();

            // Obter todos os arquivos e pastas no diretório de recursos
            var allFilesInResourceFolder = Directory.GetFiles(resourceFolder, "*", SearchOption.AllDirectories).ToList();
            var allDirsInResourceFolder = Directory.GetDirectories(resourceFolder, "*", SearchOption.AllDirectories).ToList();

            // Verificar se as pastas e documentos listados no XML existem no sistema de arquivos
            foreach (var xmlPath in xmlPaths)
            {
                // Caminho relativo baseado na estrutura do XML
                var relativePath = xmlPath;

                // Verificar se o caminho de diretório ou arquivo existe no sistema
                if (!allFilesInResourceFolder.Concat(allDirsInResourceFolder).Any(p => p.Equals(relativePath, StringComparison.OrdinalIgnoreCase)))
                {
                    // Se o caminho não existir, criar a pasta ou o documento
                    string fullPath = Path.Combine(resourceFolder, relativePath);

                    // Se for uma pasta
                    if (xmlFolders.Contains(relativePath))
                    {
                        try
                        {
                            if (!Directory.Exists(fullPath))
                            {
                                Directory.CreateDirectory(fullPath);
                                //Debug(enableDebug, $"Pasta criada: {fullPath}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug(enableDebug, $"Erro ao criar pasta {fullPath}: {ex.Message}");
                        }
                    }
                    // Se for um documento
                    else if (xmlDocuments.Contains(relativePath))
                    {
                        try
                        {
                            if (!File.Exists(fullPath))
                            {
                                File.Create(fullPath).Dispose();
                                //Debug(enableDebug, $"Documento criado: {fullPath}");

                                // Verificar a estrutura do documento e salvar conteúdo no arquivo .doc
                                var document = collection.Documents.FirstOrDefault(d => d.Directory.Equals(relativePath, StringComparison.OrdinalIgnoreCase));
                                Debug(isDebugModeEnabled, $"\r\relativePath {relativePath}");
                                if (document != null)
                                {
                                    // Se a estrutura for diferente de "[ondir]", escrever no arquivo
                                    if (document.Structure != "[ondir]")
                                    {
                                        WriteRscDocument(fullPath, document.Structure);
                                        //Debug(enableDebug, $"Conteúdo do documento {fullPath} atualizado.");
                                    }
                                    else
                                    {
                                        // Se for "[ondir]", deixar o arquivo vazio
                                        WriteRscDocument(fullPath, string.Empty);
                                        //Debug(enableDebug, $"Documento {fullPath} deixado vazio (estrutura '[ondir]').");
                                    }
                                }
                            }

                            // Criar a pasta complementar do documento (docfolder)
                            string docFolderName = Path.GetFileNameWithoutExtension(fullPath) + "_rsc"; // O nome do docfolder pode ser modificado conforme necessário
                            string docFolderPath = Path.Combine(resourceFolder, Path.GetDirectoryName(fullPath), docFolderName);
                            if (!Directory.Exists(docFolderPath))
                            {
                                Directory.CreateDirectory(docFolderPath);
                                //Debug(enableDebug, $"DocFolder criado: {docFolderPath}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug(enableDebug, $"Erro ao criar documento ou docfolder {fullPath}: {ex.Message}");
                        }
                    }
                }
            }
        }

        static string ResumirXML(string xmlContent)
        {
            // Carregar o XML
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(xmlContent);

            // Resumo das pastas
            var foldersNodeList = xmlDoc.GetElementsByTagName("Folder");
            string resumoPastas = "Pastas:\n";
            foreach (XmlNode folder in foldersNodeList)
            {
                string folderDirectory = folder.Attributes["Directory"].Value;
                resumoPastas += $" - {folderDirectory}\n";
            }

            // Resumo dos documentos e estrutura BBCode
            var documentsNodeList = xmlDoc.GetElementsByTagName("Document");
            string resumoDocumentos = "Documentos:\n";
            foreach (XmlNode document in documentsNodeList)
            {
                string documentDirectory = document.Attributes["Directory"].Value;
                string structure = document.Attributes["Structure"].Value;

                // Remover tags BBCode para gerar um conteúdo simples
                string simplifiedStructure = SimplificarBBCode(structure);

                resumoDocumentos += $" - {documentDirectory}:\n{simplifiedStructure}\n\n";
            }

            return resumoPastas + "\n" + resumoDocumentos;
        }

        static string SimplificarBBCode(string bbcode)
        {
            // Substitui as tags BBCode por uma versão simplificada
            bbcode = Regex.Replace(bbcode, @"\[b\](.*?)\[/b\]", "$1");  // Remove [b] e [/b] (negrito)
            bbcode = Regex.Replace(bbcode, @"\[i\](.*?)\[/i\]", "$1");  // Remove [i] e [/i] (itálico)
            bbcode = Regex.Replace(bbcode, @"\[s\](.*?)\[/s\]", "$1");  // Remove [i] e [/i] (itálico)
            bbcode = Regex.Replace(bbcode, @"\[u\](.*?)\[/u\]", "$1");  // Remove [u] e [/u] (sublinhado)

            bbcode = Regex.Replace(bbcode, @"\[color=[^]]*\](.*?)\[/color\]", "$1");  // Remove [color] e [/color]
            bbcode = Regex.Replace(bbcode, @"\[color=[^]]*\]", "");  // Remove [color] sem fechamento

            bbcode = Regex.Replace(bbcode, @"\[center\](.*?)\[/center\]", "$1");  // Remove [center] e [/center]
            bbcode = Regex.Replace(bbcode, @"\[right\](.*?)\[/right\]", "$1");  // Remove [right] e [/right]

            bbcode = Regex.Replace(bbcode, @"\[br\]", "\n");  // Substitui [br] por quebra de linha
            bbcode = Regex.Replace(bbcode, @"\[tab\]", "\t");  // Substitui [tab] por tab

            return bbcode;
        }

        public string ListUsings(string directoryPath)
        {
            HashSet<string> usingsWithEqual = new HashSet<string>();  // Para 'using' com '='
            HashSet<string> usingsSet = new HashSet<string>();  // Para 'using' sem '='

            // Verifica se o diretório existe
            if (Directory.Exists(directoryPath))
            {
                // Obtém todos os arquivos .cs no diretório e subdiretórios
                var csFiles = Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);

                foreach (var file in csFiles)
                {
                    // Lê o conteúdo do arquivo
                    var fileContent = File.ReadLines(file);

                    // Filtra as linhas que começam com 'using'
                    foreach (var line in fileContent)
                    {
                        var trimmedLine = line.Trim();

                        if (trimmedLine.StartsWith("using") && !trimmedLine.Contains("("))  // Ignora 'using' de objetos
                        {
                            if (trimmedLine.Contains("="))  // Se contém '=', adiciona à lista separada
                            {
                                usingsWithEqual.Add(trimmedLine);
                            }
                            else  // Caso contrário, adiciona à lista normal
                            {
                                usingsSet.Add(trimmedLine);
                            }
                        }
                    }
                }
            }
            else
            {
                throw new DirectoryNotFoundException("O diretório especificado não foi encontrado.");
            }

            // Ordena ambas as listas e as concatena
            var sortedUsingsWithEqual = usingsWithEqual.OrderBy(u => u).ToList();
            var sortedUsings = usingsSet.OrderBy(u => u).ToList();

            // Junta as listas, com as que têm '=' no início
            var finalUsings = sortedUsingsWithEqual.Concat(sortedUsings).ToList();

            return string.Join(Environment.NewLine, finalUsings);
        }

        public string ExtractUsingsFromFile(string filePath)
        {
            HashSet<string> usingsSet = new HashSet<string>();  // Para garantir que não há duplicatas

            // Verifica se o arquivo existe
            if (File.Exists(filePath))
            {
                // Lê o conteúdo do arquivo
                var fileContent = File.ReadLines(filePath);

                // Expressões regulares para identificar as referências
                string referencePattern = @"<Reference Include=""([^""]+)"" />";
                string packagePattern = @"<PackageReference Include=""([^""]+)""[^>]*>";

                // Processa as linhas do arquivo
                foreach (var line in fileContent)
                {
                    // Verifica se a linha contém uma referência de <Reference> ou <PackageReference>
                    Match referenceMatch = Regex.Match(line, referencePattern);
                    Match packageMatch = Regex.Match(line, packagePattern);

                    if (referenceMatch.Success)
                    {
                        // Adiciona o namespace extraído à lista de usings
                        usingsSet.Add("using " + referenceMatch.Groups[1].Value + ";");
                    }
                    else if (packageMatch.Success)
                    {
                        // Adiciona o pacote extraído à lista de usings
                        usingsSet.Add("using " + packageMatch.Groups[1].Value + ";");
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("O arquivo especificado não foi encontrado.");
            }

            // Ordena e retorna as diretivas 'using' como uma string
            var sortedUsings = usingsSet.OrderBy(u => u).ToList();
            return string.Join(Environment.NewLine, sortedUsings);
        }

        public string NotUsedUsings(string folderUsings, string docUsings)
        {
            // Converte as strings em listas e remove espaços em branco
            var folderList = folderUsings.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                         .Select(u => u.Trim().Replace("using ", "").Replace(";", ""))
                                         .ToList();

            var docList = docUsings.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                   .Select(u => u.Trim().Replace("using ", "").Replace(";", ""))
                                   .ToList();

            // Filtra os usings que não estão na lista do folder (considerando hierarquia)
            var notUsed = docList.Where(docUsing =>
                !folderList.Any(folderUsing => docUsing == folderUsing || docUsing.StartsWith(folderUsing + ".")))
                .Distinct()
                .OrderBy(u => u) // Ordena alfabeticamente
                .ToList();

            // Converte de volta para o formato de "using Namespace;"
            return string.Join(Environment.NewLine, notUsed.Select(u => $"using {u};"));
        }

        private void Debug(bool enabled, string line)
        {
            if (!enabled) return;
            rtbDebug.AppendText($"\r\n{line}");
        }

        public string ConverterXmlParaTextoFacilDeLer(Collection collection)
        {
            StringBuilder sb = new StringBuilder();

            // Cache
            sb.AppendLine("Cache:");
            sb.AppendLine($"  LastDirectory: {collection.Cache.LastDirectory}");
            sb.AppendLine();

            // Folders
            sb.AppendLine("Folders:");
            foreach (var folder in collection.Folders)
            {
                sb.AppendLine($"  Directory: {folder.Directory}");
                sb.AppendLine($"  Favorited: {folder.Favorited}");
                sb.AppendLine();
            }

            // Documents
            sb.AppendLine("Documents:");
            foreach (var document in collection.Documents)
            {
                sb.AppendLine($"  Directory: {document.Directory}");
                sb.AppendLine($"  Favorited: {document.Favorited}");
                sb.AppendLine($"  Structure: {FormatStructure(document.Structure)}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private string FormatStructure(string structure)
        {
            // Aqui podemos criar uma lógica de formatação do conteúdo da estrutura
            // Dependendo da sua necessidade, o formato pode ser diferente.
            structure = $"\r\n{structure}";

            if (structure == "[ondir]")
            {
                return "Empty content (structure is 'on dir' file)";
            }

            // Caso a estrutura tenha marcação de formatação (ex: [b], [i], [br], etc.), podemos formatar de forma simples.
            // Exemplo simples de substituição de tags de formatação por algo legível.
            structure = structure.Replace("[br]", "\r\n[br]");

            return structure;
        }

        public static string GetRelativePath(string fromPath, string toPath)
        {
            // Garantir que ambos os caminhos terminem com o separador de diretório
            fromPath = fromPath.TrimEnd(Path.DirectorySeparatorChar);
            toPath = toPath.TrimEnd(Path.DirectorySeparatorChar);

            // Criar Uri para ambos os caminhos
            Uri fromUri = new Uri(fromPath + Path.DirectorySeparatorChar); // Adiciona o separador se necessário
            Uri toUri = new Uri(toPath);

            // Obter o caminho relativo
            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            return Uri.UnescapeDataString(relativeUri.ToString()).Replace("\\", "/"); // Garantir que o caminho tenha barras normais
        }

        public void VerificarEExcluirMidias()
        {
            bool enableDebug = false;// isDebugConsoleEnabled;
            var doc = collection.Documents.FirstOrDefault(d => d.Directory == GetCurrentDirectory(null));
            if (doc == null) return;

            string docPath = Path.Combine(GetResourceFolder(), doc.Directory);
            if (!File.Exists(docPath)) return;

            string htmlContent = (doc.Structure == "[ondir]") ? ReadRscDocument(docPath) : WebUtility.HtmlDecode(doc.Structure);
            string bbcode = ConvertHtmlToBbCode(htmlContent, true);

            var midiasNoDoc = new List<string>();
            midiasNoDoc.AddRange(Regex.Matches(bbcode, @"\[Image=\s*(.*?)\]").Cast<Match>().Select(m => m.Groups[1].Value.Trim()));
            midiasNoDoc.AddRange(Regex.Matches(bbcode, @"\[Audio=\s*(.*?)\]").Cast<Match>().Select(m => m.Groups[1].Value.Trim()));
            midiasNoDoc.AddRange(Regex.Matches(bbcode, @"\[Video=\s*(.*?)\]").Cast<Match>().Select(m => m.Groups[1].Value.Trim()));

            Debug(enableDebug, $"docPath: {docPath} doc.Structure {doc.Structure}");
            Debug(enableDebug, $"CONTENT: {ReadRscDocument(docPath)}");

            string docFolder = GetDocFolder();
            if (!Directory.Exists(docFolder)) return;

            var arquivosNaPasta = Directory.GetFiles(docFolder).Select(f => Path.GetFileName(f)).ToList();
            var arquivosParaExcluir = arquivosNaPasta.Except(midiasNoDoc, StringComparer.OrdinalIgnoreCase).ToList();

            foreach (var arquivo in arquivosParaExcluir)
            {
                string caminhoArquivo = Path.Combine(docFolder, arquivo);
                try
                {
                    File.Delete(caminhoArquivo);
                    Debug(enableDebug, $"Arquivo de mídia excluído: {caminhoArquivo}");
                }
                catch (Exception ex)
                {
                    Debug(enableDebug, $"Erro ao excluir arquivo {caminhoArquivo}: {ex.Message}");
                }
            }
        }

        // Obtém o caminho da pasta de mídia associada ao documento
        private string GetResourceFolder()
        {
            if (string.IsNullOrEmpty(getCaminhoColecaoAtual()))
                return null;

            string projectFolder = Path.GetDirectoryName(getCaminhoColecaoAtual());
            return Path.Combine(projectFolder, Path.GetFileNameWithoutExtension(getCaminhoColecaoAtual()) + "_rsc");
        }

        private string GetDocFolder()
        {
            if (string.IsNullOrEmpty(getCaminhoColecaoAtual()) || !isDocMode || string.IsNullOrEmpty(caminhoDiretorioAtual))
                return null;

            string folderName = Path.GetFileNameWithoutExtension(caminhoDiretorioAtual) + "_rsc";
            string docFolder = Path.Combine(GetResourceFolder(), Path.GetDirectoryName(caminhoDiretorioAtual), folderName);

            //Debug(enableDebug, $"docFolder = {docFolder}");
            return docFolder;
        }

        private void NovoArquivo()
        {
            // Limpar o arquivo atual (se necessário)
            LimparArquivo();

            isNewFile = true;
            collection = new Collection(); // Cria uma nova instância se estiver nula
            collection.Folders.Add(new Folder { Directory = InitialDirectory }); // Adiciona um Folder com o caminho FirstDir
            collection.Cache.LastDirectory = InitialDirectory;

            //Debug(isDebugConsoleEnabled, $"[NovoArquivo] Pastas inicializadas: " + string.Join(", ", collection.Folders.Select(f => f.Directory)));

            // Carregar o explorador com uma coleção vazia ou inicial
            SetCurrentDirectory(InitialDirectory, true); // Carregar a partir de uma pasta específica
        }

        private void LimparArquivo()
        {
            if (isNewFile)
            {
                isNewFile = false;
                if(Directory.Exists(tempFolder + "_rsc"))
                {
                    Directory.Delete(tempFolder + "_rsc", true);
                }
            }

            flwExplorer.Controls.Clear();
            flwFavorites.Controls.Clear();
            SetFavMenuVisibility(false);

            caminhoColecaoAtual = null;
            ultimaColecao = caminhoColecaoAtual;
            Properties.Settings.Default.UltimaColecao = ultimaColecao;
            Properties.Settings.Default.Save();
            SalvamentoPendente(false);
        }

        private bool SalvarArquivoComo()
        {
            if (string.IsNullOrEmpty(caminhoColecaoAtual))
            {
                string nomePadrao = langManager[2];
                using (var sfd = new SaveFileDialog { Filter = $"{langManager[9]} DUS (*.dus)|*.dus", FileName = nomePadrao + ".dus" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        // Chama o método SalvarArquivo com o caminho selecionado
                        SalvarArquivo(sfd.FileName);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                SalvarArquivo(caminhoColecaoAtual);
                return true;
            }
        }

        private bool SalvarAntesDeFechar()
        {
            if (isAutoSaveEnabled)
            {
                SalvarArquivo(caminhoColecaoAtual);
                return true;
            }
            if (houveAlteracao)
            {
                if (collection.Folders.Count > 0 || collection.Documents.Count > 0 || !string.IsNullOrEmpty(caminhoColecaoAtual))
                {
                    DialogResult resultado = MessageBox.Show(langManager[23], langManager[6], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                    {
                        if (string.IsNullOrEmpty(caminhoColecaoAtual))
                        {
                            // Se for um novo arquivo e o usuário clicar em "Cancelar" no diálogo Salvar Como, evita o fechamento
                            if (!SalvarArquivoComo())
                            {
                                return false;
                            }
                        }
                        else
                        {
                            SalvarArquivo(caminhoColecaoAtual);
                        }
                    }
                    else if (resultado == DialogResult.Cancel)
                    {
                        return false; // Cancelar fechamento do formulário
                    }
                }
            }

            return true; // Se a lista estiver vazia ou se o usuário optar por não salvar, prosseguir com o fechamento
        }

        private void SalvamentoPendente(bool alterado)
        {
            houveAlteracao = alterado;
            UpdateInfos();
        }

        private bool IsValidEncryptedFile(string caminho)
        {
            bool enableDebug = false;
            try
            {
                // Verifica a extensão do arquivo
                string extension = Path.GetExtension(caminho).ToLower();
                Debug(enableDebug, $"Verificando extensão do arquivo: {extension}");

                if (extension != ".dus")
                {
                    Debug(enableDebug, "Extensão inválida. O arquivo não tem a extensão .dus.");
                    return false;
                }

                // Verifica se o arquivo está criptografado (você pode verificar isso de outras formas também, dependendo do seu caso)
                string decryptedXml;
                Debug(enableDebug, "Tentando descriptografar o arquivo...");

                // Descriptografa o conteúdo do XML, caso o arquivo esteja criptografado
                try
                {
                    decryptedXml = DecryptXmlBytes(caminho, password);
                    Debug(enableDebug, "Arquivo descriptografado com sucesso.");
                }
                catch (Exception ex)
                {
                    // Caso ocorra algum erro ao descriptografar, pode-se assumir que o arquivo não está criptografado
                    // ou o erro indica que o arquivo está corrompido ou com senha incorreta
                    Debug(enableDebug, $"Erro ao descriptografar o arquivo: {ex.Message}");
                    return false;
                }

                // Agora, você tem o conteúdo XML como string, então você pode carregá-lo usando o XmlSerializer
                var serializer = new XmlSerializer(typeof(Collection));
                Debug(enableDebug, "Iniciando deserialização do conteúdo XML...");

                using (var reader = new StringReader(decryptedXml))
                {
                    var collection = (Collection)serializer.Deserialize(reader);

                    if (collection == null)
                    {
                        Debug(enableDebug, "Falha na deserialização: coleção nula.");
                        return false;
                    }

                    // Validação Cache
                    if (collection.Cache == null || string.IsNullOrEmpty(collection.Cache.LastDirectory))
                    {
                        Debug(enableDebug, "Falha na validação do Cache: Cache ou LastDirectory são inválidos.");
                        return false;
                    }

                    // Validação Folders
                    if (collection.Folders == null || collection.Folders.Count == 0)
                    {
                        Debug(enableDebug, "Falha na validação das Pastas: Nenhuma pasta encontrada.");
                        return false;
                    }

                    foreach (var folder in collection.Folders)
                    {
                        if (string.IsNullOrEmpty(folder.Directory))
                        {
                            Debug(enableDebug, $"Falha na validação da Pasta: Diretório vazio na pasta {folder.Directory}.");
                            return false;
                        }
                    }

                    // Validação Documents
                    if (collection.Documents == null || collection.Documents.Count == 0)
                    {
                        Debug(enableDebug, "Nenhum documento encontrado.");
                        return true;  // Se documentos não forem obrigatórios, retornamos true aqui
                    }

                    foreach (var document in collection.Documents)
                    {
                        if (string.IsNullOrEmpty(document.Directory) || string.IsNullOrEmpty(document.Structure))
                        {
                            Debug(enableDebug, $"Falha na validação do Documento: Diretório ou Estrutura vazio para o documento {document.Directory}.");
                            return false;
                        }
                    }
                }

                Debug(enableDebug, "Arquivo DUS válido.");
                return true;
            }
            catch (Exception ex)
            {
                // Captura exceções de deserialização e outros erros
                Debug(enableDebug, $"Erro na validação do arquivo: {ex.Message}");
                return false;
            }
        }

        private void InitializeTimers()
        {
            autoSaveTimer = new TTimer(saveInterval);
            autoSaveTimer.Elapsed += OnAutoSave;
            autoSaveTimer.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
             // Perguntar se quer salvar arquivo dus caso tenha modificado
            if (SalvarAntesDeFechar())
            {
                InvokeIfRequired(rtbDebug, () => { Debug(isErrorLogEnabled, $"[Form1_FormClosing] " + $"[{DateTime.UtcNow}] {NomeDoProjeto} está sendo encerrado."); });

                // Definindo o caminho para a pasta Logs e o nome do arquivo
                string logsDirectory = Path.Combine(appData, "Logs");

                // Garantir que a pasta "Logs" existe, caso contrário, criá-la
                if (!Directory.Exists(logsDirectory))
                {
                    Directory.CreateDirectory(logsDirectory);
                }

                // Definir o caminho completo para o arquivo de log
                string logFilePath = Path.Combine(logsDirectory, $"Log {DateTime.Now:yyyy-MM-dd HH-mm-ss}.txt");
                using (StreamWriter writer = new StreamWriter(logFilePath, append: true))
                {
                    writer.WriteLine(rtbDebug.Text); // Escreve o conteúdo do RichTextBox
                    writer.WriteLine(); // Deixa uma linha em branco para separar os logs
                }

                SaveUserSettings();

                if (this.WindowState == FormWindowState.Normal) // Salva somente se não estiver maximizado
                {
                    Properties.Settings.Default.WindowSize = this.Size;
                    Properties.Settings.Default.WindowLocation = this.Location;
                }

                Properties.Settings.Default.WindowState = this.WindowState;
                Properties.Settings.Default.Save();
            }
            else
            {
                e.Cancel = !SalvarAntesDeFechar();
            }
        }

        public void InvokeIfRequired(Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        // ====================================================================================================================================================//
        public static class Prompt
        {
            public static string ShowDialog(string text, string caption, bool isdarktheme, Icon icon, string initialText = "")
            {
                Color ThemeBackColor = isdarktheme ? Color.FromArgb(27, 27, 27) : Color.FromArgb(255, 255, 255);
                Color ThemeForeColor = isdarktheme ? Color.FromArgb(190, 190, 190) : Color.FromArgb(17, 17, 17);
                Color ThemeBasicColor = isdarktheme ? Color.FromArgb(35, 35, 35) : Color.FromArgb(195, 195, 195);

                Form prompt = new Form()
                {
                    Icon = icon,
                    Width = 300,
                    Height = 150,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen,
                    BackColor = ThemeBackColor,
                    ForeColor = ThemeForeColor,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                Label textLabel = new Label()
                {
                    Left = 10,
                    Top = 20,
                    Text = text,
                    ForeColor = ThemeForeColor,
                    Width = 270
                };

                TextBox textBox = new TextBox()
                {
                    Left = 12,
                    Top = 40,
                    Width = 262,
                    BackColor = ThemeBasicColor,
                    ForeColor = ThemeForeColor,
                    BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D,
                    MaxLength = 60,
                    Text = initialText // Definir o texto inicial na caixa
                };

                textBox.KeyPress += (sender, e) =>
                {
                    if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
                    {
                        e.Handled = true;
                    }
                };

                Button confirmation = new Button()
                {
                    Text = "OK",
                    Left = 200,
                    Width = 75,
                    Top = 70,
                    DialogResult = DialogResult.OK,
                    BackColor = ThemeBasicColor,
                    ForeColor = ThemeForeColor,
                    FlatStyle = FlatStyle.Flat
                };
                confirmation.FlatAppearance.BorderSize = 0;
                confirmation.Click += (sender, e) => { prompt.Close(); };

                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(textLabel);

                prompt.AcceptButton = confirmation;
                prompt.Shown += (sender, e) => { textBox.Focus(); };

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            bool enableDebug = false;
            string searchTerm = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                Debug(enableDebug, $"[btnSearch] Caixa de pesquisa vazia. Recarregando o diretório atual.");
                SetCurrentDirectory(GetCurrentDirectory(null), false); // Recarrega o diretório atual
                return;
            }

            Debug(enableDebug, $"[btnSearch] Iniciando busca por: " + searchTerm);

            // Determinar se o termo de busca é provavelmente um nome de arquivo
            bool isFileSearch = searchTerm.Contains(".") && searchTerm.IndexOf('.') < searchTerm.Length - 1;

            // Limpar os controles do FlowLayoutPanel
            flwExplorer.Controls.Clear();

            // Declarar as listas que armazenarão os resultados
            List<Folder> matchingFolders = new List<Folder>();
            List<Document> matchingDocuments = new List<Document>();

            if (isFileSearch)
            {
                // Buscar documentos cujo título contém o termo de pesquisa (parcial)
                matchingDocuments = collection.Documents
                    .Where(doc =>
                        doc.Directory.StartsWith(GetCurrentDirectory(null) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) && // Está no diretório atual ou subpastas
                        Path.GetFileName(doc.Directory).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0) // Nome do arquivo contém o termo
                    .ToList();

                foreach (var doc in matchingDocuments)
                {
                    var docButton = new Button()
                    {
                        Text = Path.GetFileNameWithoutExtension(doc.Directory),
                        Tag = doc,
                        Width = 140,
                        Height = 140,
                        TextAlign = ContentAlignment.BottomCenter,
                        ImageAlign = ContentAlignment.TopCenter,
                        TextImageRelation = TextImageRelation.ImageAboveText,
                        Image = ResizeIcon(docIcon, 104, 104),
                        FlatStyle = FlatStyle.Flat,
                        FlatAppearance = { BorderSize = 0, MouseDownBackColor = ThemeBackColorHighlight, MouseOverBackColor = ThemeLightColor },
                        Margin = new Padding(10, 1, 10, 1),
                    };

                    docButton.Click += (s, e) => SetCurrentDirectory(doc.Directory, true);

                    flwExplorer.Controls.Add(docButton);
                }
            }
            else
            {
                // Buscar pastas cujo título contém o termo de pesquisa (parcial)
                matchingFolders = collection.Folders
                    .Where(folder =>
                        folder.Directory.StartsWith(GetCurrentDirectory(null) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) && // Está no diretório atual ou subpastas
                        Path.GetFileName(folder.Directory).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0) // Nome da pasta contém o termo
                    .ToList();

                matchingDocuments = collection.Documents
                    .Where(doc =>
                        doc.Directory.StartsWith(GetCurrentDirectory(null) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) && // Está no diretório atual ou subpastas
                        Path.GetFileName(doc.Directory).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0) // Nome do arquivo contém o termo
                    .ToList();

                foreach (var folder in matchingFolders)
                {
                    var folderButton = new Button()
                    {
                        Text = Path.GetFileName(folder.Directory),
                        Tag = folder,
                        Width = 140,
                        Height = 140,
                        TextAlign = ContentAlignment.BottomCenter,
                        ImageAlign = ContentAlignment.TopCenter,
                        TextImageRelation = TextImageRelation.ImageAboveText,
                        Image = ResizeIcon(folderIcon, 104, 104),
                        FlatStyle = FlatStyle.Flat,
                        FlatAppearance = { BorderSize = 0, MouseDownBackColor = ThemeBackColorHighlight, MouseOverBackColor = ThemeLightColor },
                        Margin = new Padding(10, 1, 10, 1),
                    };

                    folderButton.Click += (s, e) =>
                    {
                        Debug(enableDebug, $"[LoadDirectory] Abrindo pasta: {folder.Directory}");

                        if (!string.IsNullOrEmpty(GetCurrentDirectory(null)))
                        {
                            backHistory.Push(GetCurrentDirectory(null));
                        }
                        SetCurrentDirectory(folder.Directory, true);
                    };

                    flwExplorer.Controls.Add(folderButton);
                }

                foreach (var doc in matchingDocuments)
                {
                    var docButton = new Button()
                    {
                        Text = Path.GetFileNameWithoutExtension(doc.Directory),
                        Tag = doc,
                        Width = 140,
                        Height = 140,
                        TextAlign = ContentAlignment.BottomCenter,
                        ImageAlign = ContentAlignment.TopCenter,
                        TextImageRelation = TextImageRelation.ImageAboveText,
                        Image = ResizeIcon(docIcon, 104, 104),
                        FlatStyle = FlatStyle.Flat,
                        FlatAppearance = { BorderSize = 0, MouseDownBackColor = ThemeBackColorHighlight, MouseOverBackColor = ThemeLightColor },
                        Margin = new Padding(10, 1, 10, 1),
                    };

                    docButton.Click += (s, e) => SetCurrentDirectory(doc.Directory, true);

                    flwExplorer.Controls.Add(docButton);
                }
            }

            if (!matchingFolders.Any() && !matchingDocuments.Any())
            {
                Debug(enableDebug, $"[btnSearch] Nenhum resultado encontrado.");
                var noResultLabel = new Label()
                {
                    Text = langManager[24],
                    AutoSize = true,
                    ForeColor = Color.Gray,
                    Font = new Font("Segoe UI", 12, FontStyle.Italic)
                };
                flwExplorer.Controls.Add(noResultLabel);
            }
        }


        private void rtbDebug_TextChanged(object sender, EventArgs e)
        {
            rtbDebug.SelectionStart = rtbDebug.Text.Length; // Posiciona o cursor no final
            rtbDebug.ScrollToCaret(); // Rola a visualização para o cursor
        }

        private void btnFavorites_MouseDown(object sender, MouseEventArgs e)
        {
            isFavMenuShowing = !isFavMenuShowing;
            SetFavMenuVisibility(isFavMenuShowing);
        }

        private void SetFavMenuVisibility(bool state)
        {
            isFavMenuShowing = state;
            if (state)
            {
                flwFavorites.Visible = true;
                tblContent.ColumnStyles[0].Width = 200;
            }
            else
            {
                flwFavorites.Visible = false;
                tblContent.ColumnStyles[0].Width = 0;
            }
            UpdateControlsSize();
        }


        private void btnNewProject_MouseDown(object sender, MouseEventArgs e)
        {
            if (SalvarAntesDeFechar())
            {
                NovoArquivo();
            }
        }

        private void btnOpenProject_MouseDown(object sender, MouseEventArgs e)
        {
            if (SalvarAntesDeFechar())
            {
                if (!string.IsNullOrEmpty(caminhoColecaoAtual))
                {
                    using (var ofd = new OpenFileDialog
                    {
                        Filter = $"{langManager[9]} DUS (*.dus)|*.dus",
                        InitialDirectory = Path.GetDirectoryName(caminhoColecaoAtual),
                        FileName = Path.GetFileName(caminhoColecaoAtual)
                    })
                    {
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            // Chama o método AbrirArquivo com o caminho selecionado
                            AbrirArquivo(ofd.FileName);
                        }
                    }
                }
                else
                {
                    using (var ofd = new OpenFileDialog
                    {
                        Filter = $"{langManager[9]} DUS (*.dus)|*.dus",
                        InitialDirectory = $"{langManager[2]}" + ".dus",
                        FileName = $"{langManager[2]}" + ".dus"
                    })
                    {
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            // Chama o método AbrirArquivo com o caminho selecionado
                            AbrirArquivo(ofd.FileName);
                        }
                    }
                }
            }
        }

        private void btnSaveProject_MouseDown(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(caminhoColecaoAtual))
            {
                SalvarArquivo(caminhoColecaoAtual);
            }
            else
            {
                using (var sfd = new SaveFileDialog
                {
                    Filter = $"{langManager[9]} DUS (*.dus)|*.dus",
                    InitialDirectory = $"{langManager[2]}" + ".dus",
                    FileName = $"{langManager[2]}" + ".dus"
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        // Chama o método SalvarArquivo com o caminho selecionado
                        SalvarArquivo(sfd.FileName);
                    }
                }
            }
        }
        private void btnUp_MouseDown(object sender, MouseEventArgs e)
        {
            string parentDirectory = Path.GetDirectoryName(GetCurrentDirectory(null)); // Obtém o diretório pai

            if (!string.IsNullOrEmpty(parentDirectory) && parentDirectory != GetCurrentDirectory(null))
            {
                backHistory.Push(GetCurrentDirectory(null));  // Adiciona o caminho atual à pilha de "voltar"
                forwardHistory.Clear(); // Limpa o histórico de avanço
                SetCurrentDirectory(parentDirectory, true); // Carrega o diretório pai
            }
            txtSearch.Clear();
        }
        private void btnBack_MouseDown(object sender, MouseEventArgs e)
        {
            if (backHistory.Count > 0)
            {
                string lastDirectory = backHistory.Pop();  // Obtém o último diretório da pilha "voltar"
                forwardHistory.Push(GetCurrentDirectory(null));  // Adiciona o caminho atual à pilha de "avançar"
                SetCurrentDirectory(lastDirectory, true);  // Carrega o diretório anterior
            }

            txtSearch.Clear();
        }

        private void btnFoward_MouseDown(object sender, MouseEventArgs e)
        {
            if (forwardHistory.Count > 0)
            {
                string nextDirectory = forwardHistory.Pop();  // Obtém o próximo diretório da pilha "avançar"
                backHistory.Push(GetCurrentDirectory(null));  // Adiciona o caminho atual à pilha de "voltar"
                SetCurrentDirectory(nextDirectory, true);  // Carrega o diretório avançado
            }
            txtSearch.Clear();
        }


        private void flwExplorer_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (!string.IsNullOrEmpty(txtSearch.Text) || (isDocMode)) return;
                // Obter o item que foi clicado
                var clickedItem = flwExplorer.GetChildAtPoint(e.Location);
                if (clickedItem == null)
                {
                    // Se não for um item (área vazia), exibe o menuExplorer
                    menuExplorer.Show(flwExplorer, e.Location);
                }
            }
        }

        private void openItem_Click(object sender, EventArgs e)
        {
            // Acessa o item clicado através do 'Tag' do menuItem
            var clickedItem = menuItem.Tag;
            if (clickedItem != null)
            {
                if (clickedItem is Folder folder)
                {
                    // Se for uma pasta, abra a pasta
                    SetCurrentDirectory(folder.Directory, true); // Exemplo de carregamento de pasta
                }
                else if (clickedItem is Document document)
                {
                    // Se for um documento, abra o documento
                    SetCurrentDirectory(document.Directory, true); // Exemplo de abertura de documento
                }
            }
        }

        private async void favItem_Click(object sender, EventArgs e)
        {
            var clickedItem = menuItem.Tag;

            if (clickedItem != null)
            {
                if (clickedItem is Folder folder)
                {
                    // Alternar o estado de Favorited
                    folder.Favorited = !folder.Favorited;

                    // Atualizar o ListBox com os favoritos
                    LoadFavorites();
                    SalvamentoPendente(true);
                    SetFavMenuVisibility(true);

                    // Criar efeito de piscar o botão
                    await BlinkButton(btnFavorites, TimeSpan.FromMilliseconds(750), 150);
                }
            }
        }

        private async Task BlinkButton(CustomButton button, TimeSpan duration, int interval)
        {
            Color[] colors = {
        Color.IndianRed,        // Laranja médio
        Color.Yellow,     // Azul médio
        Color.DodgerBlue,// Verde médio
        Color.YellowGreen,     // Amarelo médio
        Color.HotPink     // Roxo médio
    };
            // Calcular o número de passos (transições)
            int totalSteps = (int)(duration.TotalMilliseconds / interval);
            totalSteps = Math.Max(totalSteps, 1); // Garantir pelo menos 1 passo

            int index = 0;

            // Começar o efeito de piscar
            var startTime = DateTime.UtcNow;
            while ((DateTime.UtcNow - startTime).TotalMilliseconds < duration.TotalMilliseconds)
            {
                // Para cada transição de cor
                Color startColor = colors[index % colors.Length];
                Color endColor = colors[(index + 1) % colors.Length];

                // Mudar diretamente para a nova cor (sem interpolação)
                button.ButtonForeColor = endColor;

                // Aguardar o intervalo
                await Task.Delay(interval);

                // Avançar para a próxima cor
                index = (index + 1) % colors.Length;
            }
            // Depois de terminar a animação, volta para a cor original
            button.ButtonForeColor = ThemeForeColor;
        }


        // Método para interpolação de cores (fade suave)
        private Color LerpColor(Color c1, Color c2, double t)
        {
            int r = (int)(c1.R + (c2.R - c1.R) * t);
            int g = (int)(c1.G + (c2.G - c1.G) * t);
            int b = (int)(c1.B + (c2.B - c1.B) * t);
            return Color.FromArgb(r, g, b);
        }


        private void copyItem_Click(object sender, EventArgs e)
        {
            var clickedItem = menuItem.Tag;
            if (clickedItem != null)
            {
                copiedItem = clickedItem; // Armazena o item copiado
                pasteItem.Visible = true;
                toolStripSeparator4.Visible = true;
            }
        }

        private void pasteItem_Click(object sender, EventArgs e)
        {
            if (copiedItem == null) return; // Nada foi copiado

            if (copiedItem is Folder folder)
            {
                string currentDirectory = GetCurrentDirectory(null);

                // Impede que a pasta seja colada dentro dela mesma
                if (currentDirectory.Equals(folder.Directory, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(langManager[26], langManager[25], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Impede que a pasta seja colada dentro de uma subpasta dela mesma
                if (currentDirectory.StartsWith(folder.Directory + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(langManager[27], langManager[25], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string baseName = Path.GetFileName(folder.Directory) + $" {langManager[28]}";
                string newFolderPath = Path.Combine(currentDirectory, baseName);

                // Garante um nome único para a cópia
                int copyIndex = 1;
                while (collection.Folders.Any(f => f.Directory.Equals(newFolderPath, StringComparison.OrdinalIgnoreCase)))
                {
                    newFolderPath = Path.Combine(currentDirectory, baseName + $" {copyIndex}");
                    copyIndex++;
                }

                // Criar a nova pasta duplicada
                collection.Folders.Add(new Folder {
                    Directory = newFolderPath,
                    CreationDate = DateTime.UtcNow,
                    ModifiedDate = folder.ModifiedDate,
                    IconColor = folder.IconColor,
                });

                // Duplicar apenas subpastas e documentos da pasta original
                var subFolders = collection.Folders
                    .Where(f => f.Directory.StartsWith(folder.Directory + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                var subDocuments = collection.Documents
                    .Where(d => d.Directory.StartsWith(folder.Directory + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                foreach (var subDoc in subDocuments)
                {
                    string relativePath = subDoc.Directory.Substring(folder.Directory.Length);
                    string newSubDocPath = newFolderPath + relativePath;
                    collection.Documents.Add(new Document {
                        Directory = newSubDocPath,
                        CreationDate = DateTime.UtcNow,
                        ModifiedDate = subDoc.ModifiedDate,
                        IconColor = subDoc.IconColor,
                        Structure = GetDocStructure(subDoc.Directory, subDoc.Structure) });
                }

                // Criar a pasta no diretório de recursos
                string projectFolder = Path.GetDirectoryName(getCaminhoColecaoAtual());
                string resourceFolder = Path.Combine(projectFolder, Path.GetFileNameWithoutExtension(getCaminhoColecaoAtual()) + "_rsc");

                string newResourceFolderPath = Path.Combine(resourceFolder, newFolderPath);
                Directory.CreateDirectory(newResourceFolderPath); // Cria a pasta no _rsc

                // Criação das subpastas e diretórios
                foreach (var subFolder in subFolders)
                {
                    string relativePath = subFolder.Directory.Substring(folder.Directory.Length);
                    string newSubFolderPath = newFolderPath + relativePath;

                    // Adiciona a subpasta na coleção apenas uma vez
                    collection.Folders.Add(new Folder {
                        Directory = newSubFolderPath,
                        CreationDate = DateTime.UtcNow,
                        ModifiedDate = subFolder.ModifiedDate,
                        IconColor = subFolder.IconColor,
                    });

                    // Cria a subpasta correspondente nos recursos
                    string newResourceSubFolderPath = Path.Combine(resourceFolder, newSubFolderPath);
                    Directory.CreateDirectory(newResourceSubFolderPath);
                }

                // Criar os documentos nos recursos
                foreach (var subDoc in subDocuments)
                {
                    string relativePath = subDoc.Directory.Substring(folder.Directory.Length);
                    string newSubDocPath = newFolderPath + relativePath;

                    // Caminho no diretório de recursos
                    string docResourcePath = Path.Combine(resourceFolder, newSubDocPath);

                    // Garantir que o diretório do arquivo exista
                    Directory.CreateDirectory(Path.GetDirectoryName(docResourcePath));

                    // Criar o arquivo .doc
                    File.Create(docResourcePath).Dispose();

                    // Criar a pasta _rsc do documento
                    string docFolderName = Path.GetFileNameWithoutExtension(newSubDocPath) + "_rsc";
                    string docFolderDirectory = Path.Combine(resourceFolder, Path.GetDirectoryName(newSubDocPath), docFolderName);
                    Directory.CreateDirectory(docFolderDirectory);

                    // NOVO: Copiar os arquivos da pasta de recursos original, se existir
                    string originalDocFolder = Path.Combine(resourceFolder, Path.GetDirectoryName(subDoc.Directory), Path.GetFileNameWithoutExtension(subDoc.Directory) + "_rsc");
                    if (Directory.Exists(originalDocFolder))
                    {
                        foreach (var file in Directory.GetFiles(originalDocFolder))
                        {
                            string fileName = Path.GetFileName(file);
                            string destinationPath = Path.Combine(docFolderDirectory, fileName);
                            File.Copy(file, destinationPath, true);
                        }
                    }
                }
            }
            else if (copiedItem is Document document)
            {
                string newDocumentName = Path.GetFileNameWithoutExtension(document.Directory) + $" {langManager[28]}";
                string newDocumentPath = Path.Combine(GetCurrentDirectory(null), newDocumentName + ".doc");

                int copyIndex = 1;
                while (collection.Documents.Any(d => d.Directory.Equals(newDocumentPath, StringComparison.OrdinalIgnoreCase)))
                {
                    newDocumentPath = Path.Combine(GetCurrentDirectory(null), Path.GetFileNameWithoutExtension(document.Directory) + $" ({langManager[28]} {copyIndex})");
                    copyIndex++;
                }

                var newDocument = new Document
                {
                    Directory = newDocumentPath,
                    CreationDate = DateTime.UtcNow,
                    ModifiedDate = document.ModifiedDate,
                    IconColor = document.IconColor,
                    Structure = GetDocStructure(document.Directory, document.Structure)
                };

                collection.Documents.Add(newDocument);

                // Criar o documento no diretório de recursos
                string docFolderName = Path.GetFileNameWithoutExtension(newDocument.Directory) + "_rsc";
                string docFolderDirectory = Path.Combine(GetResourceFolder(), Path.GetDirectoryName(newDocument.Directory), docFolderName);
                string fileDirectory = Path.Combine(GetResourceFolder(), newDocument.Directory);

                // Cria a pasta para o novo documento
                Directory.CreateDirectory(docFolderDirectory);
                File.Create(fileDirectory).Dispose();

                // Copia todos os arquivos da pasta _rsc original para a nova pasta _rsc
                string originalDocFolder = Path.Combine(GetResourceFolder(), Path.GetDirectoryName(document.Directory), Path.GetFileNameWithoutExtension(document.Directory) + "_rsc");

                if (Directory.Exists(originalDocFolder))
                {
                    foreach (var file in Directory.GetFiles(originalDocFolder))
                    {
                        string fileName = Path.GetFileName(file);
                        string destinationPath = Path.Combine(docFolderDirectory, fileName);

                        // Copia o arquivo para a nova pasta
                        File.Copy(file, destinationPath, true);
                    }
                }
            }

            copiedItem = null; // Limpa a variável de cópia após colar

            // Atualizar a exibição
            SetCurrentDirectory(GetCurrentDirectory(null), false);
            LoadFavorites();
            SalvamentoPendente(true);

            pasteItem.Visible = false;
            toolStripSeparator4.Visible = false;
        }

        private void deleteItem_Click(object sender, EventArgs e)
        {
            bool enableDebug = false;
            var clickedItem = menuItem.Tag;
            if (clickedItem != null)
            {
                if (clickedItem is Folder folder)
                {
                    var result = MessageBox.Show(langManager[29],
                             langManager[30],
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Warning,
                             MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.Yes)
                    {
                        // Encontrar todas as subpastas e documentos dentro da pasta a ser excluída
                        var subFolders = collection.Folders.Where(f => f.Directory.StartsWith(folder.Directory + Path.DirectorySeparatorChar)).ToList();
                        var subDocuments = collection.Documents.Where(d => d.Directory.StartsWith(folder.Directory + Path.DirectorySeparatorChar)).ToList();

                        // Remover todas as subpastas e documentos
                        foreach (var subFolder in subFolders)
                        {
                            collection.Folders.Remove(subFolder);
                        }
                        foreach (var subDocument in subDocuments)
                        {
                            collection.Documents.Remove(subDocument);
                        }

                        // Remover a pasta principal
                        collection.Folders.Remove(folder);

                        // Remover a pasta correspondente nos recursos `_rsc`
                        string projectFolder = Path.GetDirectoryName(getCaminhoColecaoAtual());
                        string resourceFolder = Path.Combine(projectFolder, Path.GetFileNameWithoutExtension(getCaminhoColecaoAtual()) + "_rsc");
                        string resourcePath = Path.Combine(resourceFolder, folder.Directory);
                        if (Directory.Exists(resourcePath)) Directory.Delete(resourcePath, true);

                        // Atualizar a exibição
                        SetCurrentDirectory(GetCurrentDirectory(null), false);
                    }
                }
                else if (clickedItem is Document document)
                {
                    var result = MessageBox.Show(langManager[31],
                        langManager[32],
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, // Ícone adicionado
                        MessageBoxDefaultButton.Button2); // "Não" selecionado por padrão

                    if (result == DialogResult.Yes)
                    {
                        collection.Documents.Remove(document);

                        // Caminho da pasta de recursos `_rsc`
                        string projectFolder = Path.GetDirectoryName(getCaminhoColecaoAtual());
                        string resourceFolder = Path.Combine(projectFolder, Path.GetFileNameWithoutExtension(getCaminhoColecaoAtual()) + "_rsc");

                        // Caminho correto do arquivo dentro de `_rsc`
                        string resourcePath = Path.Combine(resourceFolder, document.Directory);

                        // Garantir que tenha a extensão correta
                        if (!resourcePath.EndsWith(".doc", StringComparison.OrdinalIgnoreCase))
                        {
                            resourcePath += ".doc"; // Adiciona a extensão se necessário
                        }

                        // Garantir que o caminho seja absoluto
                        string absoluteResourcePath = Path.GetFullPath(resourcePath);

                        // Verificar e deletar o arquivo
                        if (File.Exists(absoluteResourcePath))
                        {
                            File.Delete(absoluteResourcePath);
                            Debug(enableDebug, $"[Delete] Removido: '{absoluteResourcePath}'.");
                        }
                        else
                        {
                            Debug(enableDebug, $"[Delete] Arquivo não encontrado: '{absoluteResourcePath}'.");
                        }

                        string docFolderName = Path.GetFileNameWithoutExtension(document.Directory) + "_rsc";
                        string docFolderDirectory = Path.Combine(resourceFolder, Path.GetDirectoryName(document.Directory), docFolderName);
                        if (Directory.Exists(docFolderDirectory)) Directory.Delete(docFolderDirectory, true);

                        SetCurrentDirectory(GetCurrentDirectory(null), false);
                    }
                }

                LoadFavorites();
                SalvamentoPendente(true);
            }
        }
        private void renameItem_Click(object sender, EventArgs e)
        {
            var clickedItem = menuItem.Tag;
            if (clickedItem != null)
            {
                if (clickedItem is Folder folder)
                {
                    string currentFolderName = Path.GetFileName(folder.Directory);
                    string newFolderName = Prompt.ShowDialog(langManager[33], langManager[34], _isDarkMode, this.Icon, currentFolderName);
                    if (!string.IsNullOrEmpty(newFolderName))
                    {
                        string parentPath = Path.GetDirectoryName(folder.Directory);
                        string newFolderPath = Path.Combine(parentPath, newFolderName);

                        if (collection.Folders.Any(f => f.Directory.Equals(newFolderPath, StringComparison.OrdinalIgnoreCase)))
                        {
                            MessageBox.Show(langManager[35], langManager[25], MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string oldFolderPath = folder.Directory;

                        // Atualizar o caminho da pasta principal
                        folder.Directory = newFolderPath;

                        // Atualizar caminhos das subpastas
                        foreach (var subFolder in collection.Folders.Where(f => f.Directory.StartsWith(oldFolderPath + Path.DirectorySeparatorChar)).ToList())
                        {
                            subFolder.Directory = subFolder.Directory.Replace(oldFolderPath, newFolderPath);
                        }

                        // Atualizar caminhos dos documentos dentro dessa pasta
                        foreach (var subDocument in collection.Documents.Where(d => d.Directory.StartsWith(oldFolderPath + Path.DirectorySeparatorChar)).ToList())
                        {
                            subDocument.Directory = subDocument.Directory.Replace(oldFolderPath, newFolderPath);
                        }

                        // Renomear a pasta correspondente nos recursos `_rsc`
                        string projectFolder = Path.GetDirectoryName(getCaminhoColecaoAtual());
                        string resourceFolder = Path.Combine(projectFolder, Path.GetFileNameWithoutExtension(getCaminhoColecaoAtual()) + "_rsc");
                        string oldResourcePath = Path.Combine(resourceFolder, oldFolderPath);
                        string newResourcePath = Path.Combine(resourceFolder, newFolderPath);
                        if (Directory.Exists(oldResourcePath)) Directory.Move(oldResourcePath, newResourcePath);

                        // Atualizar exibição
                        SetCurrentDirectory(GetCurrentDirectory(null), false);
                    }
                }
                else if (clickedItem is Document document)
                {
                    string currentDocumentName = Path.GetFileNameWithoutExtension(document.Directory);
                    string newDocumentName = Prompt.ShowDialog(langManager[36], langManager[37], _isDarkMode, this.Icon, currentDocumentName);
                    if (!string.IsNullOrEmpty(newDocumentName))
                    {
                        string oldRelativePath = document.Directory;
                        string newRelativePath = Path.Combine(Path.GetDirectoryName(oldRelativePath), newDocumentName + ".doc");

                        if (collection.Documents.Any(d => d.Directory.Equals(newRelativePath, StringComparison.OrdinalIgnoreCase)))
                        {
                            MessageBox.Show(langManager[38], langManager[25], MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string projectFolder = Path.GetDirectoryName(getCaminhoColecaoAtual());
                        string resourceFolder = Path.Combine(projectFolder, Path.GetFileNameWithoutExtension(getCaminhoColecaoAtual()) + "_rsc");

                        string oldFilePath = Path.Combine(resourceFolder, oldRelativePath);
                        string newFilePath = Path.Combine(resourceFolder, newRelativePath);

                        if (File.Exists(oldFilePath))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(newFilePath));
                            File.Move(oldFilePath, newFilePath);
                        }

                        string oldDocFolderName = Path.GetFileNameWithoutExtension(oldRelativePath) + "_rsc";
                        string oldDocFolderDirectory = Path.Combine(resourceFolder, Path.GetDirectoryName(oldRelativePath), oldDocFolderName);

                        string newDocFolderName = Path.GetFileNameWithoutExtension(newRelativePath) + "_rsc";
                        string newDocFolderDirectory = Path.Combine(resourceFolder, Path.GetDirectoryName(newRelativePath), newDocFolderName);

                        if (Directory.Exists(oldDocFolderDirectory))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(newDocFolderDirectory));
                            Directory.Move(oldDocFolderDirectory, newDocFolderDirectory);
                        }

                        document.Directory = newRelativePath;

                        SetCurrentDirectory(GetCurrentDirectory(null), false);
                        LoadFavorites();
                        SalvamentoPendente(true);
                    }
                }

                LoadFavorites();
                SalvamentoPendente(true);
            }
        }


        private void newFolderItem_Click(object sender, EventArgs e)
        {
            // Exibir um prompt para o nome da nova pasta
            string folderName = Prompt.ShowDialog(langManager[39], langManager[40], _isDarkMode, this.Icon, langManager[42]);

            if (!string.IsNullOrEmpty(folderName))
            {
                // Verificar se já existe uma pasta com o mesmo nome
                if (collection.Folders.Any(f => f.Directory.Equals(Path.Combine(GetCurrentDirectory(null), folderName), StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show(langManager[41], langManager[25], MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Adicionar a nova pasta à coleção
                string newFolderPath = Path.Combine(GetCurrentDirectory(null), folderName);

                // Adicionando o Folder ao invés de uma string
                collection.Folders.Add(new Folder
                {
                    Directory = newFolderPath,
                    CreationDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IconColor = "Yellow",
                    Favorited = false
                });

                // Adiciona a pasta aos resources
                string projectFolder = Path.GetDirectoryName(getCaminhoColecaoAtual());
                string resourceFolder = Path.Combine(projectFolder, Path.GetFileNameWithoutExtension(getCaminhoColecaoAtual()) + "_rsc");
                string newResource = Path.Combine(resourceFolder, newFolderPath);
                if (!Directory.Exists(newResource)) Directory.CreateDirectory(newResource);

                // Recarregar a exibição das pastas no explorador
                SetCurrentDirectory(GetCurrentDirectory(null), false);
                SalvamentoPendente(true);
            }
        }

        private string getCaminhoColecaoAtual()
        {
            return isNewFile ? tempFolder : caminhoColecaoAtual;
        }

        private void newDocItem_Click(object sender, EventArgs e)
        {
            // Exibir um prompt para o nome do novo arquivo
            string documentName = Prompt.ShowDialog(langManager[43], langManager[44], _isDarkMode, this.Icon, langManager[46]);

            if (!string.IsNullOrEmpty(documentName))
            {
                // Verificar se já existe um documento com o mesmo nome
                if (collection.Documents.Any(d => d.Directory.Equals(Path.Combine(GetCurrentDirectory(null), documentName + ".doc"), StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show(langManager[45], langManager[25], MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Adicionar o novo arquivo à coleção
                var newDocument = new Document
                {
                    Directory = Path.Combine(GetCurrentDirectory(null), documentName) + ".doc",
                    CreationDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IconColor = "Blue",
                    Favorited = false,
                    Structure = emptyDocument
                };

                collection.Documents.Add(newDocument);

                string docFolderName = Path.GetFileNameWithoutExtension(newDocument.Directory) + "_rsc";
                string docFolderDirectory = Path.Combine(GetResourceFolder(), Path.GetDirectoryName(newDocument.Directory), docFolderName);
                string fileDirectory = Path.Combine(GetResourceFolder(), newDocument.Directory);

                Directory.CreateDirectory(docFolderDirectory);
                File.Create(fileDirectory).Dispose();

                // Atualizar o explorador para refletir a mudança
                SetCurrentDirectory(GetCurrentDirectory(null), false);
                SalvamentoPendente(true);
            }
        }

        private void LoadDirectory(string path)
        {
            bool enableDebug = false;
            // Limpar o painel para evitar duplicações
            flwExplorer.Controls.Clear();

            // Verificar se o caminho refere-se a uma pasta ou a um documento
            string mod = GetModuleType(path);

            if (mod == "Folder")
            {
                isDocMode = false;
                pathNavigator1.SetIcon(folderIcon);

                // Carregar as pastas
                if (collection.Folders == null) collection.Folders = new List<Folder>();

                var folders = collection.Folders
                    .Where(f => f.Directory.StartsWith(path + Path.DirectorySeparatorChar) &&
                                f.Directory.Count(c => c == Path.DirectorySeparatorChar) == path.Count(c => c == Path.DirectorySeparatorChar) + 1)
                    .ToList();

                foreach (var folder in folders)
                {
                    var folderButton = new Button()
                    {
                        Text = Path.GetFileName(folder.Directory),
                        Tag = folder,
                        Width = 140,
                        Height = 140,
                        TextAlign = ContentAlignment.BottomCenter,
                        ImageAlign = ContentAlignment.TopCenter,
                        TextImageRelation = TextImageRelation.ImageAboveText,
                        Image = ResizeIcon(folderIcon, 104, 104),
                        FlatStyle = FlatStyle.Flat,
                        FlatAppearance = { BorderSize = 0, MouseDownBackColor = ThemeBackColorHighlight, MouseOverBackColor = ThemeLightColor },
                        Margin = new Padding(10, 1, 10, 1),
                    };

                    folderButton.MouseUp += (s, e) =>
                    {
                        if (e.Button == MouseButtons.Right && string.IsNullOrEmpty(txtSearch.Text) && !isDocMode)
                        {
                            favItem.Visible = isPremiumVersion;
                            favItem.Text = (folder.Favorited) ? langManager[48] : langManager[47];
                            menuItem.Tag = folder;
                            menuItem.Show(folderButton, e.Location);
                        }
                    };

                    folderButton.Click += (s, e) =>
                    {
                        Debug(enableDebug, $"[LoadDirectory] Abrindo pasta: {folder.Directory}");

                        if (!string.IsNullOrEmpty(GetCurrentDirectory(null)))
                        {
                            backHistory.Push(GetCurrentDirectory(null));
                        }
                        SetCurrentDirectory(folder.Directory, true);
                    };

                    flwExplorer.Controls.Add(folderButton);
                }

                // Carregar os documentos
                if (collection.Documents == null) collection.Documents = new List<Document>();

                var documents = collection.Documents
                    .Where(f => f.Directory.StartsWith(path + Path.DirectorySeparatorChar) &&
                                f.Directory.Count(c => c == Path.DirectorySeparatorChar) == path.Count(c => c == Path.DirectorySeparatorChar) + 1)
                    .ToList();

                foreach (var doc in documents)
                {
                    var docButton = new Button()
                    {
                        Text = Path.GetFileNameWithoutExtension(doc.Directory),
                        Tag = doc,
                        Width = 140,
                        Height = 140,
                        TextAlign = ContentAlignment.BottomCenter,
                        ImageAlign = ContentAlignment.TopCenter,
                        TextImageRelation = TextImageRelation.ImageAboveText,
                        Image = ResizeIcon(docIcon, 104, 104),
                        FlatStyle = FlatStyle.Flat,
                        FlatAppearance = { BorderSize = 0, MouseDownBackColor = ThemeBackColorHighlight, MouseOverBackColor = ThemeLightColor },
                        Margin = new Padding(10, 1, 10, 1),
                    };

                    docButton.MouseUp += (s, e) =>
                    {
                        if (e.Button == MouseButtons.Right && string.IsNullOrEmpty(txtSearch.Text))
                        {
                            favItem.Visible = false;
                            menuItem.Tag = doc;
                            menuItem.Show(docButton, e.Location);
                        }
                    };

                    docButton.Click += (s, e) => SetCurrentDirectory(doc.Directory, true);

                    flwExplorer.Controls.Add(docButton);
                }
            }

            else if (mod == "Doc")
            {
                var doc = collection.Documents.FirstOrDefault(d => d.Directory == path);
                if (doc != null)
                {
                    isDocMode = true;
                    pathNavigator1.SetIcon(docIcon);
                    firstDocLoad = true;
                    timerDocLoad.Start();


                    //Debug(isDebugModeEnabled, $"docStructure:\n {GetDocStructure(doc.Directory, doc.Structure)}\n");
                    //Debug(isDebugModeEnabled, $"rtf:\n {ConvertBbCodeToRtf(GetDocStructure(doc.Directory, doc.Structure), ThemeFont, ThemeForeColor)}\n");

                    VerificarEExcluirMidias();
                    string rsc = Path.Combine(GetResourceFolder(), doc.Directory); // caminho para o arquivo .txt
                    lastDocDirectory = doc.Directory;
                    rtbDocument.Rtf = string.Empty; // corrigir bug cor inicial indo de um documento pra outro
                    rtbDocument.Rtf = ConvertBbCodeToRtf(GetDocStructure(doc.Directory, doc.Structure), ThemeFont, ThemeForeColor);
                    rtbDocument.Rtf = ConvertBbCodeToRtf(GetDocStructure(doc.Directory, doc.Structure), ThemeFont, ThemeForeColor); // 2x pra corrigir primeiro documento aberto sem formatacao

                    //Debug(isDebugConsoleEnabled, $"doc.Structure: {WebUtility.HtmlDecode(doc.Structure)}");
                    //Debug(isDebugConsoleEnabled, $"rtbDocument.Rtf: {rtbDocument.Rtf}");

                    lblTitle.Text = Path.GetFileNameWithoutExtension(doc.Directory);

                    rtbDocument.Visible = !isDocReadOnly;
                    webView.Visible = isDocReadOnly;

                    if (isRollDownEnabled && rtbDocument.Visible)
                    {
                        rtbDocument.SelectionStart = rtbDocument.Text.Length;
                        rtbDocument.ScrollToCaret();
                    }
                    flwExplorer.Controls.Add(tblDoc);
                }
                else
                {
                    Debug(enableDebug, $"[LoadDirectory] Documento não encontrado na coleção.");
                }
            }
        }
        
        private void RtbDocument_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (isPremiumVersion)
                {
                    editorToolbox1.Visible = true;

                    // Definir a posição baseada no cursor
                    var mousePosition = this.PointToClient(Cursor.Position); // Obtém a posição do mouse relativa ao formulário
                    editorToolbox1.Location = new Point(
                        Math.Min(mousePosition.X, this.ClientSize.Width - editorToolbox1.Width), // Impedir que saia pela borda direita
                        Math.Min(mousePosition.Y, this.ClientSize.Height - editorToolbox1.Height) // Impedir que saia pela borda inferior
                    );

                    // Manter o foco e fechar quando necessário
                    editorToolbox1.BringToFront(); // Garante que o toolbox esteja acima de outros controles
                    editorToolbox1.Focus(); // Dá foco ao toolbox
                }
                else
                {
                    simpleTextMenu.Show(rtbDocument, e.Location);
                }
            }
        }

        private void SetTextAlign(int pos)
        {
            if (pos == -1)
            {
                rtbDocument.SelectionAlignment = HorizontalAlignment.Left;
            }
            if (pos == 0)
            {
                rtbDocument.SelectionAlignment = HorizontalAlignment.Center;
            }
            if (pos == 1)
            {
                rtbDocument.SelectionAlignment = HorizontalAlignment.Right;
            }
            editorToolbox1.Visible = false;
        }

        private void SetTextStyle(FontStyle style)
        {
            if (rtbDocument.SelectionFont != null)
            {
                var currentFont = rtbDocument.SelectionFont;
                var newStyle = currentFont.Style ^ style; // Alterna o estilo
                rtbDocument.SelectionFont = new Font(currentFont, newStyle);
            }
            editorToolbox1.Visible = false;
        }

        private void SetTextSize(int size)
        {
            if (rtbDocument.SelectionFont != null)
            {
                float newSize = size; // Novo tamanho da fonte
                var currentFont = rtbDocument.SelectionFont;
                rtbDocument.SelectionFont = new Font(currentFont.FontFamily, newSize, currentFont.Style);
            }

            // Esconde o menu após a seleção do tamanho
            editorToolbox1.Visible = false;
        }

        private void SetTextColor(Color color)
        {
            if (rtbDocument.SelectionFont != null)
            {
                var currentFont = rtbDocument.SelectionFont;
                rtbDocument.SelectionColor = color; // Aplica a cor de texto selecionado
            }

            // Esconde o menu de ferramentas após selecionar a cor
            editorToolbox1.Visible = false;
        }

        private void AddMediaToDocument()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = langManager[49];
                openFileDialog.Filter = "Todos os arquivos (*.*)|*.*"; // Aceita todos os tipos de arquivos
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string extension = Path.GetExtension(filePath).ToLower();

                    // Copiar o arquivo para a pasta do projeto e obter a URL local
                    string mediaUrl = CopyMediaToProject(filePath);
                    if (mediaUrl == null)
                    {
                        Debug(isErrorLogEnabled, "Erro ao copiar a mídia.");
                        return;
                    }

                    // Identifica e adiciona a mídia ao documento com base na extensão
                    AddMediaByExtension(mediaUrl, extension);
                }
            }
        }

        // Método que verifica a extensão e adiciona a mídia ao documento
        private void AddMediaByExtension(string mediaUrl, string extension)
        {
            // Defina os tipos de mídia e suas respectivas funções
            var mediaTypes = new Dictionary<string, Action<string>>()
    {
        { ".jpg", InsertImage },
        { ".jpeg", InsertImage },
        { ".png", InsertImage },
        { ".bmp", InsertImage },
        { ".gif", InsertImage },
        { ".mp3", InsertAudio },
        { ".wav", InsertAudio },
        { ".ogg", InsertAudio },
        { ".mp4", InsertVideo },
        { ".avi", InsertVideo },
        { ".mov", InsertVideo },
        { ".wmv", InsertVideo }
    };

            // Verifica se a extensão é válida e chama a função apropriada
            if (mediaTypes.ContainsKey(extension))
            {
                mediaTypes[extension](mediaUrl);  // Chama a função correspondente
            }
            else
            {
                MessageBox.Show(langManager[50], langManager[25], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Copia o arquivo de mídia para a pasta do documento
        private string CopyMediaToProject(string originalPath)
        {
            if (string.IsNullOrEmpty(getCaminhoColecaoAtual()))
            {
                Debug(isErrorLogEnabled, "Salve o documento antes de adicionar mídias.");
                return null;
            }

            string docFolder = GetDocFolder();

            if (!Directory.Exists(docFolder))
                Directory.CreateDirectory(docFolder);

            string fileName = Path.GetFileName(originalPath);
            string destinationPath = Path.Combine(docFolder, fileName);

            try
            {
                File.Copy(originalPath, destinationPath, true);
                return $"http://media.local/{fileName}";
            }
            catch (Exception ex)
            {
                Debug(isErrorLogEnabled, "Erro ao copiar mídia: " + ex.Message);
                return null;
            }
        }

        // Métodos auxiliares para identificar tipos de arquivos
        private bool IsImage(string extension) => new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" }.Contains(extension);
        private bool IsAudio(string extension) => new[] { ".mp3", ".wav", ".ogg" }.Contains(extension);

        private bool IsVideo(string extension) => new[] { ".mp4", ".avi", ".mov", ".wmv" }.Contains(extension);

        private void InsertImage(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            rtbDocument.SelectedText = $"[Image={fileName}]\n";  // Insere onde o cursor estiver
        }

        private void btnFavorites_MouseLeave(object sender, EventArgs e)
        {
            btnFavorites.ButtonForeColor = ThemeForeColor;
        }

        private void InsertAudio(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            rtbDocument.SelectedText = $"[Audio={fileName}]\n";  // Insere onde o cursor estiver
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            editorToolbox1.Visible = false;
        }

        private void InsertVideo(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            rtbDocument.SelectedText = $"[Video={fileName}]\n";  // Insere onde o cursor estiver
        }

        //gpt
        public static string ConvertBbCodeToRtf(string bbcode, Font ThemeFont, Color ThemeForeColor)
        {
            bbcode = WebUtility.HtmlDecode(bbcode);

            // Inicia o RTF e define a estrutura inicial
            string rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0";

            // Inicializa uma lista de cores usadas no BBCode
            HashSet<string> usedColors = new HashSet<string>();

            // Adiciona ThemeForeColor como a primeira cor
            string themeHex = $"#{ThemeForeColor.R:X2}{ThemeForeColor.G:X2}{ThemeForeColor.B:X2}";
            usedColors.Add(themeHex);

            // Processa recursivamente as tags [color] para identificar as cores usadas
            string colorPattern = @"\[color=#([0-9a-fA-F]{6})\](.*?)\[/color\]";
            while (Regex.IsMatch(bbcode, colorPattern, RegexOptions.Singleline))
            {
                bbcode = Regex.Replace(bbcode, colorPattern, m =>
                {
                    string hex = "#" + m.Groups[1].Value.ToUpper();
                    usedColors.Add(hex); // Adiciona a cor à lista de cores usadas
                    string content = m.Groups[2].Value;
                    string rtfColor = ConvertHexColorToRtf(hex, usedColors); // Passa o conjunto de cores usadas
                    return $"{{{rtfColor} {content}}}";
                }, RegexOptions.Singleline);
            }

            // Cria a tabela de cores dinâmica com ThemeForeColor primeiro
            rtf += "{\\colortbl ;";
            foreach (string hex in usedColors)
            {
                int red = Convert.ToInt32(hex.Substring(1, 2), 16);
                int green = Convert.ToInt32(hex.Substring(3, 2), 16);
                int blue = Convert.ToInt32(hex.Substring(5, 2), 16);
                rtf += $"\\red{red}\\green{green}\\blue{blue};";
            }
            rtf += "}";

            // Define a fonte inicial baseada em ThemeFont
            string fontName = ThemeFont.Name;  // Obtém o nome da fonte do ThemeFont
            int fontSize = (int)ThemeFont.Size; // Obtém o tamanho da fonte (em pontos)
            rtf += $"{{\\fonttbl{{\\f0\\fnil\\fcharset0 {fontName};}}}}";  // Define a fonte

            // Obtém o índice correto de ThemeForeColor na tabela de cores
            int colorIndex = GetColorIndex(themeHex, usedColors);

            // Aplica o reset de formatação e define a cor e o tamanho da fonte
            rtf += $"\\plain\\cf{colorIndex}\\fs{fontSize * 2} ";

            // Processa as tags e formatação básica
            bbcode = ProcessBasicFormatting(bbcode);

            // Finaliza o documento RTF
            rtf += bbcode + "}";

            return rtf;
        }
        private static string ProcessBasicFormatting(string bbcode)
        {
            // Processa alinhamentos e formatações básicas
            bbcode = Regex.Replace(bbcode, @"\[center\](.*?)\[/center\]", m => $"{{\\pard\\qc {m.Groups[1].Value}\\par}}", RegexOptions.Singleline);
            bbcode = Regex.Replace(bbcode, @"\[right\](.*?)\[/right\]", m => $"{{\\pard\\qr {m.Groups[1].Value}\\par}}", RegexOptions.Singleline);
            bbcode = Regex.Replace(bbcode, @"\[b\](.*?)\[/b\]", m => $"{{\\b {m.Groups[1].Value}}}", RegexOptions.Singleline);
            bbcode = Regex.Replace(bbcode, @"\[i\](.*?)\[/i\]", m => $"{{\\i {m.Groups[1].Value}}}", RegexOptions.Singleline);
            bbcode = Regex.Replace(bbcode, @"\[s\](.*?)\[/s\]", m => $"{{\\strike {m.Groups[1].Value}}}", RegexOptions.Singleline);
            bbcode = Regex.Replace(bbcode, @"\[u\](.*?)\[/u\]", m => $"{{\\ul {m.Groups[1].Value}}}", RegexOptions.Singleline);

            // Processa as tags de tamanho de forma recursiva
            while (Regex.IsMatch(bbcode, @"\[size=(\d+)\](.*?)\[/size\]", RegexOptions.Singleline))
            {
                bbcode = Regex.Replace(bbcode, @"\[size=(\d+)\](.*?)\[/size\]", m =>
                {
                    int size = int.Parse(m.Groups[1].Value);
                    string content = m.Groups[2].Value;
                    return $"{{\\fs{size * 2} {content}}}";
                }, RegexOptions.Singleline);
            }

            // Processa as quebras de linha e tabs
            bbcode = Regex.Replace(bbcode, @"\[br\]", "{\\par}");
            bbcode = Regex.Replace(bbcode, @"\[tab\]", @"\tab ");

            // Remove quaisquer tags não processadas (por exemplo, [center], [right], etc.) – cuidado para não remover conteúdo já substituído
            bbcode = Regex.Replace(bbcode, @"\[(\/?)(center|right|b|i|s|u|size)\]", "", RegexOptions.IgnoreCase);

            return bbcode;
        }


        private static string ConvertHexColorToRtf(string hex, HashSet<string> usedColors)
        {
            // Se a cor já foi adicionada, só retorna o índice correspondente
            int index = GetColorIndex(hex, usedColors);
            return $"\\cf{index}";
        }

        private static int GetColorIndex(string hex, HashSet<string> usedColors)
        {
            // Se a cor já foi usada, retorna o índice dela
            int index = 1; // Começar com índice 1, porque o índice 0 é reservado para a cor padrão
            foreach (string color in usedColors)
            {
                if (color.Equals(hex, StringComparison.OrdinalIgnoreCase))
                    return index;
                index++;
            }
            return index; // Caso não tenha sido encontrada, adiciona com o próximo índice
        }





        public string ConverHtmlToRTF(string code)
        {
            code = ConvertHtmlToBbCode(code, true);
            code = ConvertBbCodeToRtf(code, ThemeFont, ThemeForeColor);
            //Debug(enableDebug, $"RTF: {code}");
            return code;
        }

        public string ConvertRtfToBBCode(string code)
        {
            code = ConvertRtfToHtml(code);
            code = ConvertHtmlToBbCode(code, true);
            return code;
        }

        public static string ConvertHtmlToBbCode(string html, bool fixalign)
        {
            // Remove estilos e tags desnecessárias
            html = Regex.Replace(html, @"<style[^>]*>.*?</style>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<div[^>]*>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"</div>", "", RegexOptions.IgnoreCase);

            html = Regex.Replace(html, @"<br>", "");
            html = Regex.Replace(html, @"\t", @"[tab]", RegexOptions.IgnoreCase);

            // Conversão de parágrafo
            html = Regex.Replace(html, @"<p([^>]*)>(.*?)</p>", m =>
            {
                string style = m.Groups[1].Value;
                string content = m.Groups[2].Value;

                // Verifica o tamanho da fonte e a cor primeiro
                Match sizeMatch = Regex.Match(style, @"font-size:\s*(\d+)pt", RegexOptions.IgnoreCase);
                if (sizeMatch.Success)
                    content = "[size=" + sizeMatch.Groups[1].Value + "]" + content + "[/size]";

                Match colorMatch = Regex.Match(style, @"color:\s*#?([0-9a-fA-F]{6})", RegexOptions.IgnoreCase);
                if (colorMatch.Success)
                    content = "[color=#" + colorMatch.Groups[1].Value + "]" + content + "[/color]";

                // Verifica se tem alinhamento center ou right
                if (Regex.IsMatch(style, @"text-align:\s*center", RegexOptions.IgnoreCase))
                    content = "[center]" + content + "[/center]" + "[wbr]";
                else if (Regex.IsMatch(style, @"text-align:\s*right", RegexOptions.IgnoreCase))
                    content = "[right]" + content + "[/right]" + "[wbr]";
                else
                    // Adiciona [br] apenas se não houver alinhamento
                    content += "[br]";

                return content;
            }, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            html = fixalign
    ? Regex.Replace(html, @"\[wbr\]", "")          // Remove [wbr] se fixalign for true
    : Regex.Replace(html, @"\[wbr\]", "[br]");     // Substitui por [br] se fixalign for false

            html = Regex.Replace(html, @"<span style\s*=\s*\""display:inline-block;width:\d+px\""\s*></span>", "[tab]");

            // Conversão de span
            html = Regex.Replace(html, @"<span([^>]*)>(.*?)</span>", m =>
            {
                string style = m.Groups[1].Value;
                string content = m.Groups[2].Value;

                Match sizeMatch = Regex.Match(style, @"font-size:\s*(\d+)pt", RegexOptions.IgnoreCase);
                if (sizeMatch.Success)
                    content = "[size=" + sizeMatch.Groups[1].Value + "]" + content + "[/size]";

                Match colorMatch = Regex.Match(style, @"color:\s*#?([0-9a-fA-F]{6})", RegexOptions.IgnoreCase);
                if (colorMatch.Success)
                    content = "[color=#" + colorMatch.Groups[1].Value + "]" + content + "[/color]";

                return content;
            }, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Conversão de formatação geral com estilos
            html = Regex.Replace(html, @"<(strong|b)([^>]*)>(.*?)</\1>", m =>
            {
                string style = m.Groups[2].Value;
                string content = m.Groups[3].Value;

                // Aplica font-size
                Match sizeMatch = Regex.Match(style, @"font-size:\s*(\d+)pt", RegexOptions.IgnoreCase);
                if (sizeMatch.Success)
                    content = "[size=" + sizeMatch.Groups[1].Value + "]" + content + "[/size]";

                // Aplica color
                Match colorMatch = Regex.Match(style, @"color:\s*#?([0-9a-fA-F]{6})", RegexOptions.IgnoreCase);
                if (colorMatch.Success)
                    content = "[color=#" + colorMatch.Groups[1].Value + "]" + content + "[/color]";

                return "[b]" + content + "[/b]";
            }, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            html = Regex.Replace(html, @"<(em|i)([^>]*)>(.*?)</\1>", m =>
            {
                string style = m.Groups[2].Value;
                string content = m.Groups[3].Value;

                // Aplica font-size
                Match sizeMatch = Regex.Match(style, @"font-size:\s*(\d+)pt", RegexOptions.IgnoreCase);
                if (sizeMatch.Success)
                    content = "[size=" + sizeMatch.Groups[1].Value + "]" + content + "[/size]";

                // Aplica color
                Match colorMatch = Regex.Match(style, @"color:\s*#?([0-9a-fA-F]{6})", RegexOptions.IgnoreCase);
                if (colorMatch.Success)
                    content = "[color=#" + colorMatch.Groups[1].Value + "]" + content + "[/color]";

                return "[i]" + content + "[/i]";
            }, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            html = Regex.Replace(html, @"<u([^>]*)>(.*?)</u>", m =>
            {
                string style = m.Groups[1].Value;
                string content = m.Groups[2].Value;

                // Aplica font-size
                Match sizeMatch = Regex.Match(style, @"font-size:\s*(\d+)pt", RegexOptions.IgnoreCase);
                if (sizeMatch.Success)
                    content = "[size=" + sizeMatch.Groups[1].Value + "]" + content + "[/size]";

                // Aplica color
                Match colorMatch = Regex.Match(style, @"color:\s*#?([0-9a-fA-F]{6})", RegexOptions.IgnoreCase);
                if (colorMatch.Success)
                    content = "[color=#" + colorMatch.Groups[1].Value + "]" + content + "[/color]";

                return "[u]" + content + "[/u]";
            }, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            html = Regex.Replace(html, @"<s([^>]*)>(.*?)</s>", m =>
            {
                string style = m.Groups[1].Value;
                string content = m.Groups[2].Value;

                // Aplica font-size
                Match sizeMatch = Regex.Match(style, @"font-size:\s*(\d+)pt", RegexOptions.IgnoreCase);
                if (sizeMatch.Success)
                    content = "[size=" + sizeMatch.Groups[1].Value + "]" + content + "[/size]";

                // Aplica color
                Match colorMatch = Regex.Match(style, @"color:\s*#?([0-9a-fA-F]{6})", RegexOptions.IgnoreCase);
                if (colorMatch.Success)
                    content = "[color=#" + colorMatch.Groups[1].Value + "]" + content + "[/color]";

                return "[s]" + content + "[/s]";
            }, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Converte imagens
            html = Regex.Replace(html, @"<img[^>]*src=['""]([^'""]+)['""][^>]*>", m =>
            {
                // Captura o valor do src
                string src = m.Groups[1].Value;

                // Decodifica a URL, removendo qualquer codificação de caracteres (%20 para espaço, por exemplo)
                string decodedSrc = Uri.UnescapeDataString(src);

                // Agora, removemos qualquer tag HTML que possa estar dentro da URL (como <span>)
                decodedSrc = Regex.Replace(decodedSrc, @"<[^>]+>", string.Empty); // Remove qualquer tag HTML

                // Obtemos o nome do arquivo da URL limpa
                string fileName = Path.GetFileName(decodedSrc);

                // Retorna a string do BBCode
                return $"[Image={fileName}]";
            }, RegexOptions.IgnoreCase);

            // Converte áudios
            html = Regex.Replace(html, @"<audio[^>]*>.*?<source[^>]*src=['""]([^'""]+)['""][^>]*>.*?</audio>", m =>
            {
                // Captura o valor do src
                string src = m.Groups[1].Value;

                // Decodifica a URL, removendo qualquer codificação de caracteres (%20 para espaço, por exemplo)
                string decodedSrc = Uri.UnescapeDataString(src);

                // Remove qualquer tag HTML dentro do src, como <span> ou outras
                decodedSrc = Regex.Replace(decodedSrc, @"<[^>]+>", string.Empty);

                // Obtemos o nome do arquivo da URL limpa
                string fileName = Path.GetFileName(decodedSrc);

                // Retorna a string do BBCode
                return $"[Audio={fileName}]";
            }, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Converte vídeos
            html = Regex.Replace(html, @"<video[^>]*>.*?<source[^>]*src=['""]([^'""]+)['""][^>]*>.*?</video>", m =>
            {
                // Captura o valor do src
                string src = m.Groups[1].Value;

                // Decodifica a URL, removendo qualquer codificação de caracteres
                string decodedSrc = Uri.UnescapeDataString(src);

                // Remove qualquer tag HTML dentro do src
                decodedSrc = Regex.Replace(decodedSrc, @"<[^>]+>", string.Empty);

                // Obtemos o nome do arquivo da URL limpa
                string fileName = Path.GetFileName(decodedSrc);

                // Retorna a string do BBCode
                return $"[Video={fileName}]";
            }, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Remover tags HTML restantes
            html = Regex.Replace(html, @"<[^>]+>", "", RegexOptions.IgnoreCase);

            // Limpeza de espaços extras
           // html = Regex.Replace(html, @"(\[br\]){2,}", "[br]");
            html = html.Trim();

            return html;
        }
        
        public string ConvertRtfToHtml(string rtf)
        {
            // Converte RTF para HTML usando RtfPipe (se for necessário)
            string htmlContent = Rtf.ToHtml(rtf);

            // Substitui as tags personalizadas (como [Image: screen-11.jpg]) por tags HTML válidas
            htmlContent = ReplaceMediaTags(htmlContent);

            // Substitui '\tab ' por um span com espaçamento
            htmlContent = Regex.Replace(
                htmlContent,
                @"<span style=\""display:inline-block;width:(\d+)px\""></span>",
                match =>
                {
                    int width = int.Parse(match.Groups[1].Value);
                    int tabSize = 48; // Definindo 48px como tamanho de um tab
                    int tabCount = width / tabSize;
                    return new string('\t', tabCount); // Retorna a quantidade correta de tabs
                }
            );

            // Adiciona o CSS inline para manter a cor de fundo, de texto, impedir rolagem horizontal
            // E também o espaço entre os parágrafos e as letras
            htmlContent = "<html><head><style>" +
                          "body { background-color: " + ColorToHex(ThemeBasicColor) + "; color: " + ColorToHex(ThemeForeColor) + "; " +
                          "overflow-x: hidden; padding: 0; margin: 0;" +
                          "word-wrap: break-word; overflow-wrap: break-word; " +
                          "letter-spacing: 0.5px; white-space: pre-wrap; tab-size: 13;} " +
                          "p { margin-bottom: 1px !important; white-space: pre-wrap; } " +  // Força o espaçamento entre parágrafos
                          "html { overflow-x: hidden; }" +
                          "</style></head><body>" + htmlContent + "</body></html>";

            //Debug(enableDebug, $"htmlContent: {htmlContent}");
            // Remove qualquer estilo inline de margin em tags <p>
            return htmlContent;
        }

        private string ReplaceMediaTags(string html)
        {
            // Define a URL base para a mídia
            string mediaBaseUrl = "http://media.local/";

            // Corrige a tag de imagem
            html = Regex.Replace(html, @"\[Image=\s*(.*?)\]", match =>
            {
                string fileName = match.Groups[1].Value.Trim(); // Obtém o nome do arquivo
                string path = mediaBaseUrl + Uri.EscapeDataString(fileName); // Codifica o nome do arquivo corretamente

                // Substitui a tag [Image: ...] por uma tag <img>
                return $"<img src=\"{path}\" style=\"max-width:100%; height:auto;\" />";
            }, RegexOptions.IgnoreCase);

            // Corrige a tag de áudio
            html = Regex.Replace(html, @"\[Audio=\s*(.*?)\]", match =>
            {
                string fileName = match.Groups[1].Value.Trim(); // Obtém o nome do arquivo
                string path = mediaBaseUrl + Uri.EscapeDataString(fileName); // Codifica o nome do arquivo corretamente

                // Substitui a tag [Audio: ...] por uma tag <audio>
                return $"<audio controls><source src=\"{path}\" type=\"audio/mpeg\"></audio>";
            }, RegexOptions.IgnoreCase);

            // Corrige a tag de vídeo
            html = Regex.Replace(html, @"\[Video=\s*(.*?)\]", match =>
            {
                string fileName = match.Groups[1].Value.Trim(); // Obtém o nome do arquivo
                string path = mediaBaseUrl + Uri.EscapeDataString(fileName); // Codifica o nome do arquivo corretamente

                // Substitui a tag [Video: ...] por uma tag <video>
                return $"<video controls width=\"400\"><source src=\"{path}\" type=\"video/mp4\"></video>";
            }, RegexOptions.IgnoreCase);

            // Substitui todas as ocorrências de %26nbsp%3B por %20
            html = html.Replace("%26nbsp%3B", "%20");

            return html;
        }

        // Função para converter uma cor System.Drawing.Color para o formato hexadecimal (#RRGGBB)
        private string ColorToHex(System.Drawing.Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        private (int, int, int) HexColorToRgb(string hex)
        {
            // Remover o sinal '#' caso presente
            hex = hex.TrimStart('#');

            // Verifica se o formato da cor é válido
            if (hex.Length != 6)
                throw new ArgumentException("Cor hexadecimal inválida.");

            // Converte para RGB
            int r = Convert.ToInt32(hex.Substring(0, 2), 16);
            int g = Convert.ToInt32(hex.Substring(2, 2), 16);
            int b = Convert.ToInt32(hex.Substring(4, 2), 16);

            return (r, g, b);
        }

        private Image ResizeIcon(Image originalImage, int width, int height)
        {
            // Ajusta o ícone para o tamanho desejado
            return new Bitmap(originalImage, new Size(width, height));
        }

        private void LoadFavorites()
        {
            flwFavorites.Controls.Clear(); // Limpar o FlowLayoutPanel antes de adicionar novos itens

            if (collection == null) return;

            // Adicionar pastas favoritas ao FlowLayoutPanel
            foreach (var folder in collection.Folders.Where(f => f.Favorited))
            {
                // Criar o botão de pasta favorita (ícone + nome)
                folderButton = new Button()
                {
                    Text = Path.GetFileName(folder.Directory),
                    Tag = folder,
                    Width = flwFavorites.Width,
                    Height = 24, // Tamanho ajustado para um visual mais compacto
                    TextAlign = ContentAlignment.MiddleLeft, // Alinha o texto à esquerda
                    ImageAlign = ContentAlignment.MiddleLeft, // Coloca o ícone à esquerda
                    TextImageRelation = TextImageRelation.ImageBeforeText, // Coloca o ícone antes do texto
                    Image = ResizeIcon(folderIcon, 16, 16), // Ícone pequeno
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 0, MouseDownBackColor = ThemeBackColorHighlight, MouseOverBackColor = ThemeLightColor },
                    Margin = new Padding(0), // Ajuste de margem para espaçamento adequado
                    Padding = new Padding(10, 0, 10, 0), // Ajuste de margem para espaçamento adequado
                };

                // Ao clicar na pasta, exibir o diretório
                folderButton.Click += (s, e) =>
                {
                    SetCurrentDirectory(folder.Directory, true);
                };
                // Adicionar o painel da pasta ao FlowLayoutPanel
                flwFavorites.Controls.Add(folderButton);
            }
        }



    } // Fim do Form1

    public class SaveQueue
    {
        private readonly ConcurrentQueue<Func<Task>> saveTasks = new ConcurrentQueue<Func<Task>>();
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1); // Permite apenas 1 tarefa por vez
        private bool isProcessing = false;

        // Adiciona um novo salvamento à fila
        public void EnqueueSave(Func<Task> saveTask)
        {
            saveTasks.Enqueue(saveTask);
            ProcessQueue();
        }

        // Processa a fila de salvamentos
        private async void ProcessQueue()
        {
            if (isProcessing) return; // Evita múltiplos processamentos simultâneos

            isProcessing = true;

            while (saveTasks.TryDequeue(out var task))
            {
                await semaphore.WaitAsync();
                try
                {
                    await task(); // Executa o salvamento
                }
                finally
                {
                    semaphore.Release();
                }
            }

            isProcessing = false;
        }
    }

    [Serializable]
    public class Collection
    {
        public Cache Cache { get; set; }
        public List<Folder> Folders { get; set; }
        public List<Document> Documents { get; set; }

        public Collection()
        {
            Cache = new Cache();
            Folders = new List<Folder>();
            Documents = new List<Document>();
        }
    }

    [Serializable]
    public class Cache
    {
        [XmlAttribute]  // Serializa como atributo XML
        public string LastDirectory { get; set; }
    }

    public class Folder
    {
        [XmlAttribute]  // Serializa como atributo XML
        public string Directory { get; set; }

        [XmlAttribute]  // Serializa como atributo XML
        public DateTime CreationDate { get; set; }

        [XmlAttribute]  // Serializa como atributo XML
        public DateTime ModifiedDate { get; set; }

        [XmlAttribute]  // Serializa como atributo XML
        public string IconColor { get; set; }

        [XmlAttribute]  // Serializa como atributo XML
        public bool Favorited { get; set; }
    }

    public class Document
    {
        [XmlAttribute]  // Serializa como atributo XML
        public string Directory { get; set; }

        [XmlAttribute]  // Serializa como atributo XML
        public DateTime CreationDate { get; set; }

        [XmlAttribute]  // Serializa como atributo XML
        public DateTime ModifiedDate { get; set; }

        [XmlAttribute]  // Serializa como atributo XML
        public string IconColor { get; set; }

        [XmlAttribute]  // Serializa como atributo XML
        public bool Favorited { get; set; }

        [XmlAttribute]  // Serializa como atributo XML
        public string Structure { get; set; }
    }

} // Fim do namespace