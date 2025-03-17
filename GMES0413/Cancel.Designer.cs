namespace CSI.MES.P
{
    partial class Cancel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Cancel));
            this.lblRegId = new DevExpress.XtraEditors.LabelControl();
            this.txtUserNm = new DevExpress.XtraEditors.TextEdit();
            this.lblDash5 = new DevExpress.XtraEditors.LabelControl();
            this.txtRegId = new DevExpress.XtraEditors.TextEdit();
            this.lblDestination = new DevExpress.XtraEditors.LabelControl();
            this.mmoReason = new DevExpress.XtraEditors.MemoEdit();
            this.btnCancel = new DevExpress.XtraEditors.PictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserNm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRegId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmoReason.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRegId
            // 
            this.lblRegId.Location = new System.Drawing.Point(34, 12);
            this.lblRegId.Name = "lblRegId";
            this.lblRegId.Size = new System.Drawing.Size(33, 13);
            this.lblRegId.TabIndex = 0;
            this.lblRegId.Text = "Reg ID";
            // 
            // txtUserNm
            // 
            this.txtUserNm.Location = new System.Drawing.Point(254, 9);
            this.txtUserNm.Name = "txtUserNm";
            this.txtUserNm.Size = new System.Drawing.Size(146, 20);
            this.txtUserNm.TabIndex = 27;
            // 
            // lblDash5
            // 
            this.lblDash5.Location = new System.Drawing.Point(244, 12);
            this.lblDash5.Name = "lblDash5";
            this.lblDash5.Size = new System.Drawing.Size(4, 13);
            this.lblDash5.TabIndex = 26;
            this.lblDash5.Text = "-";
            // 
            // txtRegId
            // 
            this.txtRegId.Location = new System.Drawing.Point(108, 9);
            this.txtRegId.Name = "txtRegId";
            this.txtRegId.Size = new System.Drawing.Size(130, 20);
            this.txtRegId.TabIndex = 25;
            // 
            // lblDestination
            // 
            this.lblDestination.Location = new System.Drawing.Point(39, 37);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(28, 13);
            this.lblDestination.TabIndex = 28;
            this.lblDestination.Text = "Memo";
            // 
            // mmoReason
            // 
            this.mmoReason.Location = new System.Drawing.Point(108, 35);
            this.mmoReason.Name = "mmoReason";
            this.mmoReason.Size = new System.Drawing.Size(292, 55);
            this.mmoReason.TabIndex = 29;
            // 
            // btnCancel
            // 
            this.btnCancel.EditValue = global::CSI.MES.P.Properties.Resources.cancelled;
            this.btnCancel.Location = new System.Drawing.Point(232, 105);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Properties.Appearance.Options.UseBackColor = true;
            this.btnCancel.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.btnCancel.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.btnCancel.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.btnCancel.Size = new System.Drawing.Size(168, 46);
            this.btnCancel.TabIndex = 33;
            this.btnCancel.Click += new System.EventHandler(this.pctSave_Click);
            this.btnCancel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pctSave_MouseDown);
            this.btnCancel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pctSave_MouseUp);
            // 
            // Cancel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 166);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.mmoReason);
            this.Controls.Add(this.lblDestination);
            this.Controls.Add(this.txtUserNm);
            this.Controls.Add(this.lblDash5);
            this.Controls.Add(this.txtRegId);
            this.Controls.Add(this.lblRegId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Cancel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cancel";
            ((System.ComponentModel.ISupportInitialize)(this.txtUserNm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRegId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmoReason.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblRegId;
        private DevExpress.XtraEditors.TextEdit txtUserNm;
        private DevExpress.XtraEditors.LabelControl lblDash5;
        private DevExpress.XtraEditors.TextEdit txtRegId;
        private DevExpress.XtraEditors.LabelControl lblDestination;
        private DevExpress.XtraEditors.MemoEdit mmoReason;
        private DevExpress.XtraEditors.PictureEdit btnCancel;
    }
}