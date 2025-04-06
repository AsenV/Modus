namespace Modus
{
    partial class EditorToolbox
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        //private System.ComponentModel.IContainer components = null;
        
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
        /*protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }*/

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.btnMedia = new Modus.CustomButton();
            this.btnSize = new Modus.CustomButton();
            this.btnColor = new Modus.CustomButton();
            this.btnRisk = new Modus.CustomButton();
            this.btnItalic = new Modus.CustomButton();
            this.btnBold = new Modus.CustomButton();
            this.btnRemove = new Modus.CustomButton();
            this.btnCut = new Modus.CustomButton();
            this.btnPaste = new Modus.CustomButton();
            this.btnCopy = new Modus.CustomButton();
            this.btnAlignRight = new Modus.CustomButton();
            this.btnAlignCenter = new Modus.CustomButton();
            this.btnAlignLeft = new Modus.CustomButton();
            this.btnUnderlined = new Modus.CustomButton();
            ((System.ComponentModel.ISupportInitialize)(this.mStyleManager)).BeginInit();
            this.tblMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // mStyleManager
            // 
            this.mStyleManager.Owner = this;
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 7;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tblMain.Controls.Add(this.btnUnderlined, 3, 1);
            this.tblMain.Controls.Add(this.btnRisk, 2, 1);
            this.tblMain.Controls.Add(this.btnItalic, 1, 1);
            this.tblMain.Controls.Add(this.btnBold, 0, 1);
            this.tblMain.Controls.Add(this.btnRemove, 6, 0);
            this.tblMain.Controls.Add(this.btnCut, 5, 0);
            this.tblMain.Controls.Add(this.btnPaste, 4, 0);
            this.tblMain.Controls.Add(this.btnCopy, 3, 0);
            this.tblMain.Controls.Add(this.btnAlignRight, 2, 0);
            this.tblMain.Controls.Add(this.btnAlignCenter, 1, 0);
            this.tblMain.Controls.Add(this.btnAlignLeft, 0, 0);
            this.tblMain.Controls.Add(this.btnMedia, 6, 1);
            this.tblMain.Controls.Add(this.btnSize, 5, 1);
            this.tblMain.Controls.Add(this.btnColor, 4, 1);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(0, 0);
            this.tblMain.Margin = new System.Windows.Forms.Padding(0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblMain.Size = new System.Drawing.Size(210, 60);
            this.tblMain.TabIndex = 0;
            // 
            // btnMedia
            // 
            this.btnMedia.ButtonBackColor = System.Drawing.Color.White;
            this.btnMedia.ButtonBackgroundImage = null;
            this.btnMedia.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMedia.ButtonBorderClickable = false;
            this.btnMedia.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnMedia.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnMedia.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnMedia.ButtonFont = new System.Drawing.Font("Yu Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMedia.ButtonForeColor = System.Drawing.Color.Black;
            this.btnMedia.ButtonHighlight = false;
            this.btnMedia.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnMedia.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnMedia.ButtonText = "X";
            this.btnMedia.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnMedia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMedia.Location = new System.Drawing.Point(180, 30);
            this.btnMedia.Margin = new System.Windows.Forms.Padding(0);
            this.btnMedia.Name = "btnMedia";
            this.btnMedia.Size = new System.Drawing.Size(30, 30);
            this.btnMedia.TabIndex = 12;
            // 
            // btnSize
            // 
            this.btnSize.ButtonBackColor = System.Drawing.Color.White;
            this.btnSize.ButtonBackgroundImage = null;
            this.btnSize.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSize.ButtonBorderClickable = false;
            this.btnSize.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnSize.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnSize.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnSize.ButtonFont = new System.Drawing.Font("Yu Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSize.ButtonForeColor = System.Drawing.Color.Black;
            this.btnSize.ButtonHighlight = false;
            this.btnSize.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnSize.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnSize.ButtonText = "X";
            this.btnSize.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSize.Location = new System.Drawing.Point(150, 30);
            this.btnSize.Margin = new System.Windows.Forms.Padding(0);
            this.btnSize.Name = "btnSize";
            this.btnSize.Size = new System.Drawing.Size(30, 30);
            this.btnSize.TabIndex = 11;
            // 
            // btnColor
            // 
            this.btnColor.ButtonBackColor = System.Drawing.Color.White;
            this.btnColor.ButtonBackgroundImage = null;
            this.btnColor.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnColor.ButtonBorderClickable = false;
            this.btnColor.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnColor.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnColor.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnColor.ButtonFont = new System.Drawing.Font("Yu Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnColor.ButtonForeColor = System.Drawing.Color.Black;
            this.btnColor.ButtonHighlight = false;
            this.btnColor.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnColor.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnColor.ButtonText = "X";
            this.btnColor.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnColor.Location = new System.Drawing.Point(120, 30);
            this.btnColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(30, 30);
            this.btnColor.TabIndex = 10;
            // 
            // btnRisk
            // 
            this.btnRisk.ButtonBackColor = System.Drawing.Color.White;
            this.btnRisk.ButtonBackgroundImage = null;
            this.btnRisk.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRisk.ButtonBorderClickable = false;
            this.btnRisk.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnRisk.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnRisk.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnRisk.ButtonFont = new System.Drawing.Font("Yu Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRisk.ButtonForeColor = System.Drawing.Color.Black;
            this.btnRisk.ButtonHighlight = false;
            this.btnRisk.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnRisk.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnRisk.ButtonText = "X";
            this.btnRisk.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRisk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRisk.Location = new System.Drawing.Point(60, 30);
            this.btnRisk.Margin = new System.Windows.Forms.Padding(0);
            this.btnRisk.Name = "btnRisk";
            this.btnRisk.Size = new System.Drawing.Size(30, 30);
            this.btnRisk.TabIndex = 9;
            // 
            // btnItalic
            // 
            this.btnItalic.ButtonBackColor = System.Drawing.Color.White;
            this.btnItalic.ButtonBackgroundImage = null;
            this.btnItalic.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnItalic.ButtonBorderClickable = false;
            this.btnItalic.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnItalic.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnItalic.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnItalic.ButtonFont = new System.Drawing.Font("Yu Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnItalic.ButtonForeColor = System.Drawing.Color.Black;
            this.btnItalic.ButtonHighlight = false;
            this.btnItalic.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnItalic.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnItalic.ButtonText = "X";
            this.btnItalic.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnItalic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnItalic.Location = new System.Drawing.Point(30, 30);
            this.btnItalic.Margin = new System.Windows.Forms.Padding(0);
            this.btnItalic.Name = "btnItalic";
            this.btnItalic.Size = new System.Drawing.Size(30, 30);
            this.btnItalic.TabIndex = 8;
            // 
            // btnBold
            // 
            this.btnBold.ButtonBackColor = System.Drawing.Color.White;
            this.btnBold.ButtonBackgroundImage = null;
            this.btnBold.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBold.ButtonBorderClickable = false;
            this.btnBold.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnBold.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnBold.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnBold.ButtonFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBold.ButtonForeColor = System.Drawing.Color.Black;
            this.btnBold.ButtonHighlight = false;
            this.btnBold.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnBold.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnBold.ButtonText = "X";
            this.btnBold.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnBold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBold.Location = new System.Drawing.Point(0, 30);
            this.btnBold.Margin = new System.Windows.Forms.Padding(0);
            this.btnBold.Name = "btnBold";
            this.btnBold.Size = new System.Drawing.Size(30, 30);
            this.btnBold.TabIndex = 7;
            // 
            // btnRemove
            // 
            this.btnRemove.ButtonBackColor = System.Drawing.Color.White;
            this.btnRemove.ButtonBackgroundImage = null;
            this.btnRemove.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRemove.ButtonBorderClickable = false;
            this.btnRemove.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnRemove.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnRemove.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnRemove.ButtonFont = new System.Drawing.Font("Yu Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.ButtonForeColor = System.Drawing.Color.Black;
            this.btnRemove.ButtonHighlight = false;
            this.btnRemove.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnRemove.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnRemove.ButtonText = "X";
            this.btnRemove.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemove.Location = new System.Drawing.Point(180, 0);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(0);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(30, 30);
            this.btnRemove.TabIndex = 6;
            // 
            // btnCut
            // 
            this.btnCut.ButtonBackColor = System.Drawing.Color.White;
            this.btnCut.ButtonBackgroundImage = global::Modus.Properties.Resources.tbicon_light_cut;
            this.btnCut.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCut.ButtonBorderClickable = false;
            this.btnCut.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnCut.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnCut.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnCut.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCut.ButtonForeColor = System.Drawing.Color.Black;
            this.btnCut.ButtonHighlight = false;
            this.btnCut.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnCut.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnCut.ButtonText = "";
            this.btnCut.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCut.Location = new System.Drawing.Point(150, 0);
            this.btnCut.Margin = new System.Windows.Forms.Padding(0);
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(30, 30);
            this.btnCut.TabIndex = 5;
            // 
            // btnPaste
            // 
            this.btnPaste.ButtonBackColor = System.Drawing.Color.White;
            this.btnPaste.ButtonBackgroundImage = global::Modus.Properties.Resources.tbicon_light_paste;
            this.btnPaste.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPaste.ButtonBorderClickable = false;
            this.btnPaste.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnPaste.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnPaste.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnPaste.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnPaste.ButtonForeColor = System.Drawing.Color.Black;
            this.btnPaste.ButtonHighlight = false;
            this.btnPaste.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnPaste.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnPaste.ButtonText = "";
            this.btnPaste.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPaste.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPaste.Location = new System.Drawing.Point(120, 0);
            this.btnPaste.Margin = new System.Windows.Forms.Padding(0);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(30, 30);
            this.btnPaste.TabIndex = 4;
            // 
            // btnCopy
            // 
            this.btnCopy.ButtonBackColor = System.Drawing.Color.White;
            this.btnCopy.ButtonBackgroundImage = global::Modus.Properties.Resources.tbicon_light_copy;
            this.btnCopy.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCopy.ButtonBorderClickable = false;
            this.btnCopy.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnCopy.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnCopy.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnCopy.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCopy.ButtonForeColor = System.Drawing.Color.Black;
            this.btnCopy.ButtonHighlight = false;
            this.btnCopy.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnCopy.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnCopy.ButtonText = "";
            this.btnCopy.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCopy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCopy.Location = new System.Drawing.Point(90, 0);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(0);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(30, 30);
            this.btnCopy.TabIndex = 3;
            // 
            // btnAlignRight
            // 
            this.btnAlignRight.ButtonBackColor = System.Drawing.Color.White;
            this.btnAlignRight.ButtonBackgroundImage = global::Modus.Properties.Resources.tbicon_light_alignright;
            this.btnAlignRight.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAlignRight.ButtonBorderClickable = false;
            this.btnAlignRight.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnAlignRight.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnAlignRight.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnAlignRight.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnAlignRight.ButtonForeColor = System.Drawing.Color.Black;
            this.btnAlignRight.ButtonHighlight = false;
            this.btnAlignRight.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnAlignRight.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnAlignRight.ButtonText = "";
            this.btnAlignRight.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAlignRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAlignRight.Location = new System.Drawing.Point(60, 0);
            this.btnAlignRight.Margin = new System.Windows.Forms.Padding(0);
            this.btnAlignRight.Name = "btnAlignRight";
            this.btnAlignRight.Size = new System.Drawing.Size(30, 30);
            this.btnAlignRight.TabIndex = 2;
            // 
            // btnAlignCenter
            // 
            this.btnAlignCenter.ButtonBackColor = System.Drawing.Color.White;
            this.btnAlignCenter.ButtonBackgroundImage = global::Modus.Properties.Resources.tbicon_light_aligncenter;
            this.btnAlignCenter.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAlignCenter.ButtonBorderClickable = false;
            this.btnAlignCenter.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnAlignCenter.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnAlignCenter.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnAlignCenter.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnAlignCenter.ButtonForeColor = System.Drawing.Color.Black;
            this.btnAlignCenter.ButtonHighlight = false;
            this.btnAlignCenter.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnAlignCenter.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnAlignCenter.ButtonText = "";
            this.btnAlignCenter.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAlignCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAlignCenter.Location = new System.Drawing.Point(30, 0);
            this.btnAlignCenter.Margin = new System.Windows.Forms.Padding(0);
            this.btnAlignCenter.Name = "btnAlignCenter";
            this.btnAlignCenter.Size = new System.Drawing.Size(30, 30);
            this.btnAlignCenter.TabIndex = 1;
            // 
            // btnAlignLeft
            // 
            this.btnAlignLeft.ButtonBackColor = System.Drawing.Color.White;
            this.btnAlignLeft.ButtonBackgroundImage = global::Modus.Properties.Resources.tbicon_light_alignleft;
            this.btnAlignLeft.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAlignLeft.ButtonBorderClickable = false;
            this.btnAlignLeft.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnAlignLeft.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnAlignLeft.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnAlignLeft.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnAlignLeft.ButtonForeColor = System.Drawing.Color.Black;
            this.btnAlignLeft.ButtonHighlight = false;
            this.btnAlignLeft.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnAlignLeft.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnAlignLeft.ButtonText = "";
            this.btnAlignLeft.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAlignLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAlignLeft.Location = new System.Drawing.Point(0, 0);
            this.btnAlignLeft.Margin = new System.Windows.Forms.Padding(0);
            this.btnAlignLeft.Name = "btnAlignLeft";
            this.btnAlignLeft.Size = new System.Drawing.Size(30, 30);
            this.btnAlignLeft.TabIndex = 0;
            // 
            // btnUnderlined
            // 
            this.btnUnderlined.ButtonBackColor = System.Drawing.Color.White;
            this.btnUnderlined.ButtonBackgroundImage = null;
            this.btnUnderlined.ButtonBackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUnderlined.ButtonBorderClickable = false;
            this.btnUnderlined.ButtonBorderColor = System.Drawing.Color.Black;
            this.btnUnderlined.ButtonBorderHighlightColor = System.Drawing.Color.Empty;
            this.btnUnderlined.ButtonBorderWidth = new System.Windows.Forms.Padding(0);
            this.btnUnderlined.ButtonFont = new System.Drawing.Font("Yu Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUnderlined.ButtonForeColor = System.Drawing.Color.Black;
            this.btnUnderlined.ButtonHighlight = false;
            this.btnUnderlined.ButtonHighlightBackColor = System.Drawing.Color.Empty;
            this.btnUnderlined.ButtonHighlightForeColor = System.Drawing.Color.Empty;
            this.btnUnderlined.ButtonText = "X";
            this.btnUnderlined.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnUnderlined.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUnderlined.Location = new System.Drawing.Point(90, 30);
            this.btnUnderlined.Margin = new System.Windows.Forms.Padding(0);
            this.btnUnderlined.Name = "btnUnderlined";
            this.btnUnderlined.Size = new System.Drawing.Size(30, 30);
            this.btnUnderlined.TabIndex = 13;
            // 
            // EditorToolbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tblMain);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "EditorToolbox";
            this.Size = new System.Drawing.Size(210, 60);
            this.Load += new System.EventHandler(this.EditorToolbox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mStyleManager)).EndInit();
            this.tblMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.IContainer components;
        private MetroFramework.Components.MetroStyleManager mStyleManager;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private CustomButton btnAlignLeft;
        private CustomButton btnMedia;
        private CustomButton btnSize;
        private CustomButton btnColor;
        private CustomButton btnRisk;
        private CustomButton btnItalic;
        private CustomButton btnBold;
        private CustomButton btnRemove;
        private CustomButton btnCut;
        private CustomButton btnPaste;
        private CustomButton btnCopy;
        private CustomButton btnAlignRight;
        private CustomButton btnAlignCenter;
        private CustomButton btnUnderlined;
    }
}
