namespace PhotoList
{
    partial class FormPhotoList
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
            this.listBoxPicasaRoot = new System.Windows.Forms.ListBox();
            this.textBoxAlbumList = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
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
            this.splitContainer1.Panel2.Controls.Add(this.textBoxAlbumList);
            this.splitContainer1.Size = new System.Drawing.Size(523, 373);
            this.splitContainer1.SplitterDistance = 265;
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
            this.splitContainer2.Panel2.Controls.Add(this.listBoxPicasaRoot);
            this.splitContainer2.Size = new System.Drawing.Size(265, 373);
            this.splitContainer2.SplitterDistance = 169;
            this.splitContainer2.TabIndex = 0;
            // 
            // listBoxRoot
            // 
            this.listBoxRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxRoot.FormattingEnabled = true;
            this.listBoxRoot.ItemHeight = 15;
            this.listBoxRoot.Location = new System.Drawing.Point(0, 0);
            this.listBoxRoot.Name = "listBoxRoot";
            this.listBoxRoot.Size = new System.Drawing.Size(265, 169);
            this.listBoxRoot.TabIndex = 1;
            this.listBoxRoot.SelectedIndexChanged += new System.EventHandler(this.listBoxRoot_SelectedIndexChanged);
            // 
            // listBoxPicasaRoot
            // 
            this.listBoxPicasaRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPicasaRoot.FormattingEnabled = true;
            this.listBoxPicasaRoot.ItemHeight = 15;
            this.listBoxPicasaRoot.Location = new System.Drawing.Point(0, 0);
            this.listBoxPicasaRoot.Name = "listBoxPicasaRoot";
            this.listBoxPicasaRoot.Size = new System.Drawing.Size(265, 200);
            this.listBoxPicasaRoot.TabIndex = 1;
            this.listBoxPicasaRoot.SelectedIndexChanged += new System.EventHandler(this.listBoxPicasaRoot_SelectedIndexChanged);
            // 
            // textBoxAlbumList
            // 
            this.textBoxAlbumList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAlbumList.Location = new System.Drawing.Point(0, 0);
            this.textBoxAlbumList.Multiline = true;
            this.textBoxAlbumList.Name = "textBoxAlbumList";
            this.textBoxAlbumList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxAlbumList.Size = new System.Drawing.Size(254, 373);
            this.textBoxAlbumList.TabIndex = 0;
            // 
            // FormPhotoList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 373);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormPhotoList";
            this.Text = "PhotoList";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox listBoxRoot;
        private System.Windows.Forms.ListBox listBoxPicasaRoot;
        private System.Windows.Forms.TextBox textBoxAlbumList;
    }
}

