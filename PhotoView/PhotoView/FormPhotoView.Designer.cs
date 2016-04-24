namespace PhotoView
{
    partial class FormPhotoView
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if ( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listBoxRoot = new System.Windows.Forms.ListBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.listBoxCategory = new System.Windows.Forms.ListBox();
            this.listBoxAlbum = new System.Windows.Forms.ListBox();
            this.explorerBrowserContents = new Microsoft.WindowsAPICodePack.Controls.WindowsForms.ExplorerBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.explorerBrowserContents);
            this.splitContainer1.Size = new System.Drawing.Size(910, 529);
            this.splitContainer1.SplitterDistance = 303;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listBoxRoot);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(303, 529);
            this.splitContainer2.SplitterDistance = 81;
            this.splitContainer2.TabIndex = 0;
            // 
            // listBoxRoot
            // 
            this.listBoxRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxRoot.FormattingEnabled = true;
            this.listBoxRoot.ItemHeight = 15;
            this.listBoxRoot.Location = new System.Drawing.Point(0, 0);
            this.listBoxRoot.Name = "listBoxRoot";
            this.listBoxRoot.Size = new System.Drawing.Size(303, 81);
            this.listBoxRoot.TabIndex = 0;
            this.listBoxRoot.SelectedIndexChanged += new System.EventHandler(this.listBoxRoot_SelectedIndexChanged);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.listBoxCategory);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.listBoxAlbum);
            this.splitContainer3.Size = new System.Drawing.Size(303, 444);
            this.splitContainer3.SplitterDistance = 124;
            this.splitContainer3.TabIndex = 0;
            // 
            // listBoxCategory
            // 
            this.listBoxCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxCategory.FormattingEnabled = true;
            this.listBoxCategory.ItemHeight = 15;
            this.listBoxCategory.Location = new System.Drawing.Point(0, 0);
            this.listBoxCategory.Name = "listBoxCategory";
            this.listBoxCategory.Size = new System.Drawing.Size(303, 124);
            this.listBoxCategory.TabIndex = 0;
            this.listBoxCategory.SelectedIndexChanged += new System.EventHandler(this.listBoxCategory_SelectedIndexChanged);
            // 
            // listBoxAlbum
            // 
            this.listBoxAlbum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxAlbum.FormattingEnabled = true;
            this.listBoxAlbum.ItemHeight = 15;
            this.listBoxAlbum.Location = new System.Drawing.Point(0, 0);
            this.listBoxAlbum.Name = "listBoxAlbum";
            this.listBoxAlbum.Size = new System.Drawing.Size(303, 316);
            this.listBoxAlbum.TabIndex = 0;
            this.listBoxAlbum.SelectedIndexChanged += new System.EventHandler(this.listBoxAlbum_SelectedIndexChanged);
            // 
            // explorerBrowserContents
            // 
            this.explorerBrowserContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.explorerBrowserContents.Location = new System.Drawing.Point(0, 0);
            this.explorerBrowserContents.Name = "explorerBrowserContents";
            this.explorerBrowserContents.PropertyBagName = "Microsoft.WindowsAPICodePack.Controls.WindowsForms.ExplorerBrowser";
            this.explorerBrowserContents.Size = new System.Drawing.Size(603, 529);
            this.explorerBrowserContents.TabIndex = 0;
            // 
            // FormPhotoView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 529);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormPhotoView";
            this.Text = "PhotoView";
            this.Load += new System.EventHandler(this.FormPhotoView_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox listBoxRoot;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.ListBox listBoxCategory;
        private System.Windows.Forms.ListBox listBoxAlbum;
        private Microsoft.WindowsAPICodePack.Controls.WindowsForms.ExplorerBrowser explorerBrowserContents;
    }
}

