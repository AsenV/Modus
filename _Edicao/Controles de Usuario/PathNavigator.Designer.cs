namespace Modus
{
    partial class PathNavigator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PathNavigator));
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.mStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.flwAdress = new System.Windows.Forms.FlowLayoutPanel();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnMain = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mStyleManager)).BeginInit();
            this.tblMain.SuspendLayout();
            this.pnMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // picIcon
            // 
            this.picIcon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.picIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picIcon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picIcon.Image = ((System.Drawing.Image)(resources.GetObject("picIcon.Image")));
            this.picIcon.Location = new System.Drawing.Point(0, 0);
            this.picIcon.Margin = new System.Windows.Forms.Padding(0);
            this.picIcon.Name = "picIcon";
            this.picIcon.Padding = new System.Windows.Forms.Padding(2);
            this.picIcon.Size = new System.Drawing.Size(22, 20);
            this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picIcon.TabIndex = 4;
            this.picIcon.TabStop = false;
            // 
            // mStyleManager
            // 
            this.mStyleManager.Owner = this;
            // 
            // flwAdress
            // 
            this.flwAdress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flwAdress.Location = new System.Drawing.Point(22, 0);
            this.flwAdress.Margin = new System.Windows.Forms.Padding(0);
            this.flwAdress.Name = "flwAdress";
            this.flwAdress.Size = new System.Drawing.Size(186, 20);
            this.flwAdress.TabIndex = 13;
            this.flwAdress.WrapContents = false;
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 2;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Controls.Add(this.picIcon, 0, 0);
            this.tblMain.Controls.Add(this.flwAdress, 1, 0);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(0, 0);
            this.tblMain.Margin = new System.Windows.Forms.Padding(0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 1;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblMain.Size = new System.Drawing.Size(208, 20);
            this.tblMain.TabIndex = 0;
            // 
            // pnMain
            // 
            this.pnMain.Controls.Add(this.tblMain);
            this.pnMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMain.Location = new System.Drawing.Point(1, 1);
            this.pnMain.Margin = new System.Windows.Forms.Padding(0);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new System.Drawing.Size(208, 20);
            this.pnMain.TabIndex = 1;
            // 
            // PathNavigator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnMain);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PathNavigator";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(210, 22);
            this.Load += new System.EventHandler(this.PathNavigator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mStyleManager)).EndInit();
            this.tblMain.ResumeLayout(false);
            this.pnMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.PictureBox picIcon;
        private MetroFramework.Components.MetroStyleManager mStyleManager;
        private System.Windows.Forms.FlowLayoutPanel flwAdress;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.Panel pnMain;
    }
}
