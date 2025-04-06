namespace Modus
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.tblControls = new System.Windows.Forms.TableLayoutPanel();
            this.txtSearch = new MetroFramework.Controls.MetroTextBox();
            this.tblContent = new System.Windows.Forms.TableLayoutPanel();
            this.flwFavorites = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flwExplorer = new System.Windows.Forms.FlowLayoutPanel();
            this.rtbDebug = new System.Windows.Forms.RichTextBox();
            this.menuNew = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.newFolderItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFileItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.openItem = new System.Windows.Forms.ToolStripMenuItem();
            this.favItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.copyItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExplorer = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.refreshItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.pasteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.simpleTextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cortarItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copiarItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colarItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excluirItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowBar1 = new Modus.WindowBar();
            this.editorToolbox1 = new Modus.EditorToolbox();
            this.btnSaveProject = new Modus.CustomButton();
            this.btnOpenProject = new Modus.CustomButton();
            this.btnNewProject = new Modus.CustomButton();
            this.btnUp = new Modus.CustomButton();
            this.btnFoward = new Modus.CustomButton();
            this.btnBack = new Modus.CustomButton();
            this.btnFavorites = new Modus.CustomButton();
            this.pathNavigator1 = new Modus.PathNavigator();
            this.btnToggleView = new Modus.CustomButton();
            this.tblMain.SuspendLayout();
            this.tblControls.SuspendLayout();
            this.tblContent.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.menuNew.SuspendLayout();
            this.menuItem.SuspendLayout();
            this.menuExplorer.SuspendLayout();
            this.simpleTextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Controls.Add(this.tblControls, 0, 0);
            this.tblMain.Controls.Add(this.tblContent, 0, 1);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(0, 30);
            this.tblMain.Margin = new System.Windows.Forms.Padding(0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Size = new System.Drawing.Size(961, 518);
            this.tblMain.TabIndex = 19;
            // 
            // tblControls
            // 
            this.tblControls.ColumnCount = 21;
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tblControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tblControls.Controls.Add(this.btnSaveProject, 5, 1);
            this.tblControls.Controls.Add(this.btnOpenProject, 3, 1);
            this.tblControls.Controls.Add(this.btnNewProject, 1, 1);
            this.tblControls.Controls.Add(this.btnUp, 13, 1);
            this.tblControls.Controls.Add(this.btnFoward, 11, 1);
            this.tblControls.Controls.Add(this.btnBack, 9, 1);
            this.tblControls.Controls.Add(this.btnFavorites, 7, 1);
            this.tblControls.Controls.Add(this.txtSearch, 17, 1);
            this.tblControls.Controls.Add(this.pathNavigator1, 15, 1);
            this.tblControls.Controls.Add(this.btnToggleView, 19, 1);
            this.tblControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblControls.Location = new System.Drawing.Point(0, 0);
            this.tblControls.Margin = new System.Windows.Forms.Padding(0);
            this.tblControls.Name = "tblControls";
            this.tblControls.RowCount = 3;
            this.tblControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tblControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tblControls.Size = new System.Drawing.Size(961, 32);
            this.tblControls.TabIndex = 0;
            // 
            // txtSearch
            // 
            // 
            // 
            // 
            this.txtSearch.CustomButton.Image = null;
            this.txtSearch.CustomButton.Location = new System.Drawing.Point(130, 2);
            this.txtSearch.CustomButton.Name = "";
            this.txtSearch.CustomButton.Size = new System.Drawing.Size(17, 17);
            this.txtSearch.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtSearch.CustomButton.TabIndex = 1;
            this.txtSearch.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtSearch.CustomButton.UseSelectable = true;
            this.txtSearch.CustomButton.Visible = false;
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Lines = new string[0];
            this.txtSearch.Location = new System.Drawing.Point(781, 5);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0);
            this.txtSearch.MaxLength = 32767;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PasswordChar = '\0';
            this.txtSearch.PromptText = "Search..";
            this.txtSearch.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSearch.SelectedText = "";
            this.txtSearch.SelectionLength = 0;
            this.txtSearch.SelectionStart = 0;
            this.txtSearch.ShortcutsEnabled = true;
            this.txtSearch.ShowClearButton = true;
            this.txtSearch.Size = new System.Drawing.Size(150, 22);
            this.txtSearch.TabIndex = 101;
            this.txtSearch.UseSelectable = true;
            this.txtSearch.WaterMark = "Search..";
            this.txtSearch.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtSearch.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // tblContent
            // 
            this.tblContent.ColumnCount = 2;
            this.tblContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tblContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblContent.Controls.Add(this.flwFavorites, 0, 0);
            this.tblContent.Controls.Add(this.tableLayoutPanel1, 1, 0);
            this.tblContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblContent.Location = new System.Drawing.Point(0, 32);
            this.tblContent.Margin = new System.Windows.Forms.Padding(0);
            this.tblContent.Name = "tblContent";
            this.tblContent.RowCount = 1;
            this.tblContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblContent.Size = new System.Drawing.Size(961, 486);
            this.tblContent.TabIndex = 1;
            // 
            // flwFavorites
            // 
            this.flwFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flwFavorites.Location = new System.Drawing.Point(0, 10);
            this.flwFavorites.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.flwFavorites.Name = "flwFavorites";
            this.flwFavorites.Size = new System.Drawing.Size(200, 466);
            this.flwFavorites.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flwExplorer, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rtbDebug, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(200, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(761, 486);
            this.tableLayoutPanel1.TabIndex = 101;
            // 
            // flwExplorer
            // 
            this.flwExplorer.AutoScroll = true;
            this.flwExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flwExplorer.Location = new System.Drawing.Point(0, 0);
            this.flwExplorer.Margin = new System.Windows.Forms.Padding(0);
            this.flwExplorer.Name = "flwExplorer";
            this.flwExplorer.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.flwExplorer.Size = new System.Drawing.Size(761, 286);
            this.flwExplorer.TabIndex = 0;
            this.flwExplorer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.flwExplorer_MouseClick);
            // 
            // rtbDebug
            // 
            this.rtbDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDebug.Location = new System.Drawing.Point(0, 286);
            this.rtbDebug.Margin = new System.Windows.Forms.Padding(0);
            this.rtbDebug.Name = "rtbDebug";
            this.rtbDebug.Size = new System.Drawing.Size(761, 200);
            this.rtbDebug.TabIndex = 1;
            this.rtbDebug.Text = "";
            this.rtbDebug.TextChanged += new System.EventHandler(this.rtbDebug_TextChanged);
            // 
            // menuNew
            // 
            this.menuNew.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFolderItem,
            this.newFileItem});
            this.menuNew.Name = "metroContextMenu1";
            this.menuNew.OwnerItem = this.newItem;
            this.menuNew.ShowImageMargin = false;
            this.menuNew.Size = new System.Drawing.Size(113, 48);
            // 
            // newFolderItem
            // 
            this.newFolderItem.Name = "newFolderItem";
            this.newFolderItem.Size = new System.Drawing.Size(112, 22);
            this.newFolderItem.Text = "Pasta";
            this.newFolderItem.Click += new System.EventHandler(this.newFolderItem_Click);
            // 
            // newFileItem
            // 
            this.newFileItem.Name = "newFileItem";
            this.newFileItem.Size = new System.Drawing.Size(112, 22);
            this.newFileItem.Text = "Documento";
            this.newFileItem.Click += new System.EventHandler(this.newDocItem_Click);
            // 
            // newItem
            // 
            this.newItem.DropDown = this.menuNew;
            this.newItem.Name = "newItem";
            this.newItem.Size = new System.Drawing.Size(120, 22);
            this.newItem.Text = "Novo";
            // 
            // menuItem
            // 
            this.menuItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openItem,
            this.favItem,
            this.toolStripSeparator1,
            this.copyItem,
            this.toolStripSeparator2,
            this.deleteItem,
            this.renameItem});
            this.menuItem.Name = "metroContextMenu1";
            this.menuItem.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuItem.ShowImageMargin = false;
            this.menuItem.Size = new System.Drawing.Size(173, 126);
            // 
            // openItem
            // 
            this.openItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openItem.Name = "openItem";
            this.openItem.Size = new System.Drawing.Size(172, 22);
            this.openItem.Text = "Abrir";
            this.openItem.Click += new System.EventHandler(this.openItem_Click);
            // 
            // favItem
            // 
            this.favItem.Name = "favItem";
            this.favItem.Size = new System.Drawing.Size(172, 22);
            this.favItem.Text = "Adicionar aos Favoritos";
            this.favItem.Click += new System.EventHandler(this.favItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
            // 
            // copyItem
            // 
            this.copyItem.Name = "copyItem";
            this.copyItem.Size = new System.Drawing.Size(172, 22);
            this.copyItem.Text = "Copiar";
            this.copyItem.Click += new System.EventHandler(this.copyItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(169, 6);
            // 
            // deleteItem
            // 
            this.deleteItem.Name = "deleteItem";
            this.deleteItem.Size = new System.Drawing.Size(172, 22);
            this.deleteItem.Text = "Deletar";
            this.deleteItem.Click += new System.EventHandler(this.deleteItem_Click);
            // 
            // renameItem
            // 
            this.renameItem.Name = "renameItem";
            this.renameItem.Size = new System.Drawing.Size(172, 22);
            this.renameItem.Text = "Renomear";
            this.renameItem.Click += new System.EventHandler(this.renameItem_Click);
            // 
            // menuExplorer
            // 
            this.menuExplorer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshItem,
            this.toolStripSeparator4,
            this.pasteItem,
            this.toolStripSeparator3,
            this.newItem});
            this.menuExplorer.Name = "menuExplorer";
            this.menuExplorer.Size = new System.Drawing.Size(121, 82);
            // 
            // refreshItem
            // 
            this.refreshItem.Name = "refreshItem";
            this.refreshItem.Size = new System.Drawing.Size(120, 22);
            this.refreshItem.Text = "Atualizar";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(117, 6);
            this.toolStripSeparator4.Visible = false;
            // 
            // pasteItem
            // 
            this.pasteItem.Name = "pasteItem";
            this.pasteItem.Size = new System.Drawing.Size(120, 22);
            this.pasteItem.Text = "Colar";
            this.pasteItem.Visible = false;
            this.pasteItem.Click += new System.EventHandler(this.pasteItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(117, 6);
            // 
            // simpleTextMenu
            // 
            this.simpleTextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cortarItem,
            this.copiarItem,
            this.colarItem,
            this.excluirItem});
            this.simpleTextMenu.Name = "simpleTextMenu";
            this.simpleTextMenu.Size = new System.Drawing.Size(181, 92);
            // 
            // cortarItem
            // 
            this.cortarItem.Name = "cortarItem";
            this.cortarItem.Size = new System.Drawing.Size(180, 22);
            this.cortarItem.Text = "toolStripMenuItem1";
            // 
            // copiarItem
            // 
            this.copiarItem.Name = "copiarItem";
            this.copiarItem.Size = new System.Drawing.Size(180, 22);
            this.copiarItem.Text = "toolStripMenuItem2";
            // 
            // colarItem
            // 
            this.colarItem.Name = "colarItem";
            this.colarItem.Size = new System.Drawing.Size(180, 22);
            this.colarItem.Text = "toolStripMenuItem1";
            // 
            // excluirItem
            // 
            this.excluirItem.Name = "excluirItem";
            this.excluirItem.Size = new System.Drawing.Size(180, 22);
            this.excluirItem.Text = "toolStripMenuItem1";
            // 
            // windowBar1
            // 
            this.windowBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.windowBar1.BaseHeight = 30;
            this.windowBar1.CloseButton = true;
            this.windowBar1.DarkMode = true;
            this.windowBar1.FixedPos = new System.Drawing.Point(0, 0);
            this.windowBar1.FixExtraWidth = false;
            this.windowBar1.Language = "ptbr";
            this.windowBar1.Location = new System.Drawing.Point(0, 0);
            this.windowBar1.Margin = new System.Windows.Forms.Padding(0);
            this.windowBar1.MaximizeButton = true;
            this.windowBar1.MetroStyle = MetroFramework.MetroColorStyle.Blue;
            this.windowBar1.MetroTheme = MetroFramework.MetroThemeStyle.Light;
            this.windowBar1.MinimizeButton = true;
            this.windowBar1.Name = "windowBar1";
            this.windowBar1.Owner = this;
            this.windowBar1.ShowIcon = true;
            this.windowBar1.Size = new System.Drawing.Size(961, 30);
            this.windowBar1.TabIndex = 102;
            this.windowBar1.Title = "Title";
            // 
            // editorToolbox1
            // 
            this.editorToolbox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(27)))), ((int)(((byte)(27)))));
            this.editorToolbox1.BorderRadius = 20;
            this.editorToolbox1.DarkMode = true;
            this.editorToolbox1.Location = new System.Drawing.Point(0, 562);
            this.editorToolbox1.Margin = new System.Windows.Forms.Padding(0);
            this.editorToolbox1.MetroStyle = MetroFramework.MetroColorStyle.Blue;
            this.editorToolbox1.MetroTheme = MetroFramework.MetroThemeStyle.Light;
            this.editorToolbox1.Name = "editorToolbox1";
            this.editorToolbox1.Padding = new System.Windows.Forms.Padding(1);
            this.editorToolbox1.Size = new System.Drawing.Size(10, 10);
            this.editorToolbox1.TabIndex = 101;
            this.editorToolbox1.Visible = false;
            // 
            // btnSaveProject
            // 
            this.btnSaveProject.ButtonBackColor = System.Drawing.Color.Transparent;
            this.btnSaveProject.ButtonBackgroundImage = null;
            this.btnSaveProject.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSaveProject.ButtonBorderClickable = false;
            this.btnSaveProject.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnSaveProject.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnSaveProject.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnSaveProject.ButtonFont = new System.Drawing.Font("Segoe UI Emoji", 9.75F);
            this.btnSaveProject.ButtonForeColor = System.Drawing.Color.Transparent;
            this.btnSaveProject.ButtonHighlight = false;
            this.btnSaveProject.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnSaveProject.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnSaveProject.ButtonText = "💾 ";
            this.btnSaveProject.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSaveProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveProject.Location = new System.Drawing.Point(55, 5);
            this.btnSaveProject.Margin = new System.Windows.Forms.Padding(0);
            this.btnSaveProject.Name = "btnSaveProject";
            this.btnSaveProject.Size = new System.Drawing.Size(22, 22);
            this.btnSaveProject.TabIndex = 103;
            this.btnSaveProject.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSaveProject_MouseDown);
            // 
            // btnOpenProject
            // 
            this.btnOpenProject.ButtonBackColor = System.Drawing.Color.Transparent;
            this.btnOpenProject.ButtonBackgroundImage = null;
            this.btnOpenProject.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOpenProject.ButtonBorderClickable = false;
            this.btnOpenProject.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnOpenProject.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnOpenProject.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnOpenProject.ButtonFont = new System.Drawing.Font("Segoe UI Historic", 10F);
            this.btnOpenProject.ButtonForeColor = System.Drawing.Color.Transparent;
            this.btnOpenProject.ButtonHighlight = false;
            this.btnOpenProject.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnOpenProject.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnOpenProject.ButtonText = "📂";
            this.btnOpenProject.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnOpenProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenProject.Location = new System.Drawing.Point(30, 5);
            this.btnOpenProject.Margin = new System.Windows.Forms.Padding(0);
            this.btnOpenProject.Name = "btnOpenProject";
            this.btnOpenProject.Size = new System.Drawing.Size(22, 22);
            this.btnOpenProject.TabIndex = 104;
            this.btnOpenProject.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnOpenProject_MouseDown);
            // 
            // btnNewProject
            // 
            this.btnNewProject.ButtonBackColor = System.Drawing.Color.Transparent;
            this.btnNewProject.ButtonBackgroundImage = null;
            this.btnNewProject.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNewProject.ButtonBorderClickable = false;
            this.btnNewProject.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnNewProject.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnNewProject.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnNewProject.ButtonFont = new System.Drawing.Font("Segoe UI Emoji", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewProject.ButtonForeColor = System.Drawing.Color.Transparent;
            this.btnNewProject.ButtonHighlight = false;
            this.btnNewProject.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnNewProject.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnNewProject.ButtonText = "📄";
            this.btnNewProject.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNewProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNewProject.Location = new System.Drawing.Point(5, 5);
            this.btnNewProject.Margin = new System.Windows.Forms.Padding(0);
            this.btnNewProject.Name = "btnNewProject";
            this.btnNewProject.Size = new System.Drawing.Size(22, 22);
            this.btnNewProject.TabIndex = 105;
            this.btnNewProject.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNewProject_MouseDown);
            // 
            // btnUp
            // 
            this.btnUp.ButtonBackColor = System.Drawing.Color.Transparent;
            this.btnUp.ButtonBackgroundImage = null;
            this.btnUp.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUp.ButtonBorderClickable = false;
            this.btnUp.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnUp.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnUp.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnUp.ButtonFont = new System.Drawing.Font("Yu Gothic UI", 11F);
            this.btnUp.ButtonForeColor = System.Drawing.Color.Transparent;
            this.btnUp.ButtonHighlight = false;
            this.btnUp.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnUp.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnUp.ButtonText = "🡱";
            this.btnUp.ButtonTextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUp.Location = new System.Drawing.Point(155, 5);
            this.btnUp.Margin = new System.Windows.Forms.Padding(0);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(22, 22);
            this.btnUp.TabIndex = 3;
            this.btnUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseDown);
            // 
            // btnFoward
            // 
            this.btnFoward.ButtonBackColor = System.Drawing.Color.Transparent;
            this.btnFoward.ButtonBackgroundImage = null;
            this.btnFoward.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFoward.ButtonBorderClickable = false;
            this.btnFoward.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnFoward.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnFoward.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnFoward.ButtonFont = new System.Drawing.Font("Yu Gothic UI", 11F);
            this.btnFoward.ButtonForeColor = System.Drawing.Color.Transparent;
            this.btnFoward.ButtonHighlight = false;
            this.btnFoward.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnFoward.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnFoward.ButtonText = "🡲";
            this.btnFoward.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnFoward.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFoward.Location = new System.Drawing.Point(130, 5);
            this.btnFoward.Margin = new System.Windows.Forms.Padding(0);
            this.btnFoward.Name = "btnFoward";
            this.btnFoward.Size = new System.Drawing.Size(22, 22);
            this.btnFoward.TabIndex = 2;
            this.btnFoward.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnFoward_MouseDown);
            // 
            // btnBack
            // 
            this.btnBack.ButtonBackColor = System.Drawing.Color.Transparent;
            this.btnBack.ButtonBackgroundImage = null;
            this.btnBack.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBack.ButtonBorderClickable = false;
            this.btnBack.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnBack.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnBack.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnBack.ButtonFont = new System.Drawing.Font("Yu Gothic UI", 11F);
            this.btnBack.ButtonForeColor = System.Drawing.Color.Transparent;
            this.btnBack.ButtonHighlight = false;
            this.btnBack.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnBack.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnBack.ButtonText = "🡰";
            this.btnBack.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBack.Location = new System.Drawing.Point(105, 5);
            this.btnBack.Margin = new System.Windows.Forms.Padding(0);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(22, 22);
            this.btnBack.TabIndex = 1;
            this.btnBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnBack_MouseDown);
            // 
            // btnFavorites
            // 
            this.btnFavorites.ButtonBackColor = System.Drawing.Color.Transparent;
            this.btnFavorites.ButtonBackgroundImage = null;
            this.btnFavorites.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFavorites.ButtonBorderClickable = false;
            this.btnFavorites.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnFavorites.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnFavorites.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnFavorites.ButtonFont = new System.Drawing.Font("Yu Gothic UI", 11F);
            this.btnFavorites.ButtonForeColor = System.Drawing.Color.Transparent;
            this.btnFavorites.ButtonHighlight = false;
            this.btnFavorites.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnFavorites.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnFavorites.ButtonText = "★";
            this.btnFavorites.ButtonTextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFavorites.Location = new System.Drawing.Point(80, 5);
            this.btnFavorites.Margin = new System.Windows.Forms.Padding(0);
            this.btnFavorites.Name = "btnFavorites";
            this.btnFavorites.Size = new System.Drawing.Size(22, 22);
            this.btnFavorites.TabIndex = 0;
            this.btnFavorites.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnFavorites_MouseDown);
            this.btnFavorites.MouseLeave += new System.EventHandler(this.btnFavorites_MouseLeave);
            // 
            // pathNavigator1
            // 
            this.pathNavigator1.Adress = "C:\\";
            this.pathNavigator1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.pathNavigator1.DarkMode = true;
            this.pathNavigator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pathNavigator1.Location = new System.Drawing.Point(180, 5);
            this.pathNavigator1.Margin = new System.Windows.Forms.Padding(0);
            this.pathNavigator1.MetroStyle = MetroFramework.MetroColorStyle.Blue;
            this.pathNavigator1.MetroTheme = MetroFramework.MetroThemeStyle.Light;
            this.pathNavigator1.Name = "pathNavigator1";
            this.pathNavigator1.Padding = new System.Windows.Forms.Padding(1);
            this.pathNavigator1.ShowIcon = true;
            this.pathNavigator1.Size = new System.Drawing.Size(598, 22);
            this.pathNavigator1.TabIndex = 8;
            this.pathNavigator1.TextMode = false;
            // 
            // btnToggleView
            // 
            this.btnToggleView.ButtonBackColor = System.Drawing.Color.Transparent;
            this.btnToggleView.ButtonBackgroundImage = null;
            this.btnToggleView.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnToggleView.ButtonBorderClickable = false;
            this.btnToggleView.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnToggleView.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnToggleView.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnToggleView.ButtonFont = new System.Drawing.Font("Yu Gothic UI", 11F);
            this.btnToggleView.ButtonForeColor = System.Drawing.Color.Transparent;
            this.btnToggleView.ButtonHighlight = false;
            this.btnToggleView.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnToggleView.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnToggleView.ButtonText = "👁️";
            this.btnToggleView.ButtonTextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnToggleView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnToggleView.Location = new System.Drawing.Point(934, 5);
            this.btnToggleView.Margin = new System.Windows.Forms.Padding(0);
            this.btnToggleView.Name = "btnToggleView";
            this.btnToggleView.Size = new System.Drawing.Size(22, 22);
            this.btnToggleView.TabIndex = 102;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(961, 570);
            this.ControlBox = false;
            this.Controls.Add(this.windowBar1);
            this.Controls.Add(this.editorToolbox1);
            this.Controls.Add(this.tblMain);
            this.DisplayHeader = false;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Movable = false;
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 22);
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.tblMain.ResumeLayout(false);
            this.tblControls.ResumeLayout(false);
            this.tblContent.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.menuNew.ResumeLayout(false);
            this.menuItem.ResumeLayout(false);
            this.menuExplorer.ResumeLayout(false);
            this.simpleTextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.TableLayoutPanel tblContent;
        private System.Windows.Forms.FlowLayoutPanel flwExplorer;
        private System.Windows.Forms.TableLayoutPanel tblControls;
        private CustomButton btnUp;
        private CustomButton btnFoward;
        private CustomButton btnBack;
        private CustomButton btnFavorites;
        private MetroFramework.Controls.MetroTextBox txtSearch;
        private PathNavigator pathNavigator1;
        private MetroFramework.Controls.MetroContextMenu menuNew;
        private System.Windows.Forms.ToolStripMenuItem newFolderItem;
        private System.Windows.Forms.ToolStripMenuItem newFileItem;
        private MetroFramework.Controls.MetroContextMenu menuItem;
        private System.Windows.Forms.ToolStripMenuItem openItem;
        private System.Windows.Forms.ToolStripMenuItem deleteItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem renameItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox rtbDebug;
        private System.Windows.Forms.ToolStripMenuItem favItem;
        private EditorToolbox editorToolbox1;
        private System.Windows.Forms.FlowLayoutPanel flwFavorites;
        private MetroFramework.Controls.MetroContextMenu menuExplorer;
        private System.Windows.Forms.ToolStripMenuItem newItem;
        private System.Windows.Forms.ToolStripMenuItem copyItem;
        private System.Windows.Forms.ToolStripMenuItem pasteItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem refreshItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private WindowBar windowBar1;
        private CustomButton btnToggleView;
        private CustomButton btnOpenProject;
        private CustomButton btnNewProject;
        private CustomButton btnSaveProject;
        private System.Windows.Forms.ContextMenuStrip simpleTextMenu;
        private System.Windows.Forms.ToolStripMenuItem cortarItem;
        private System.Windows.Forms.ToolStripMenuItem copiarItem;
        private System.Windows.Forms.ToolStripMenuItem colarItem;
        private System.Windows.Forms.ToolStripMenuItem excluirItem;
    }
}

