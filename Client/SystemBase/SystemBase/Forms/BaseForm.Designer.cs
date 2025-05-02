namespace SystemBase.Forms
{
    partial class BaseForm
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
            System.Windows.Forms.TableLayoutPanel tlpTitle;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseForm));
            this.fbrFunction = new DSWControl.DSWFunctionBar.DSWFunctionBar();
            this.lblMessage = new SystemBase.Controls.MessageLabel();
            this.txtRoleName = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlTitleSpace = new System.Windows.Forms.Panel();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lblCorner = new System.Windows.Forms.Label();
            this.sspStatus = new System.Windows.Forms.StatusStrip();
            this.lblStatusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDateTime = new SystemBase.Controls.ToolStripDateTimeLabel();
            tlpTitle = new System.Windows.Forms.TableLayoutPanel();
            tlpTitle.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.sspStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpTitle
            // 
            resources.ApplyResources(tlpTitle, "tlpTitle");
            tlpTitle.Controls.Add(this.lblMessage, 0, 1);
            tlpTitle.Controls.Add(this.txtRoleName, 3, 0);
            tlpTitle.Controls.Add(this.txtUserName, 4, 0);
            tlpTitle.Controls.Add(this.lblTitle, 1, 0);
            tlpTitle.Controls.Add(this.pnlTitleSpace, 2, 0);
            tlpTitle.Name = "tlpTitle";
            // 
            // fbrFunction
            // 
            resources.ApplyResources(this.fbrFunction, "fbrFunction");
            this.fbrFunction.BackColor = System.Drawing.Color.DarkGray;
            this.fbrFunction.ButtonForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // fbrFunction.F01Button
            // 
            resources.ApplyResources(this.fbrFunction.F01Button, "fbrFunction.F01Button");
            this.fbrFunction.F01Button.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fbrFunction.F01Button.UseVisualStyleBackColor = false;
            this.fbrFunction.F01Button.Click += new System.EventHandler(this.fbrFunction_F01Button_Click);
            // 
            // fbrFunction.F02Button
            // 
            resources.ApplyResources(this.fbrFunction.F02Button, "fbrFunction.F02Button");
            this.fbrFunction.F02Button.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fbrFunction.F02Button.UseVisualStyleBackColor = false;
            this.fbrFunction.F02Button.Click += new System.EventHandler(this.fbrFunction_F02Button_Click);
            // 
            // fbrFunction.F03Button
            // 
            resources.ApplyResources(this.fbrFunction.F03Button, "fbrFunction.F03Button");
            this.fbrFunction.F03Button.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fbrFunction.F03Button.UseVisualStyleBackColor = false;
            this.fbrFunction.F03Button.Click += new System.EventHandler(this.fbrFunction_F03Button_Click);
            // 
            // fbrFunction.F04Button
            // 
            resources.ApplyResources(this.fbrFunction.F04Button, "fbrFunction.F04Button");
            this.fbrFunction.F04Button.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fbrFunction.F04Button.UseVisualStyleBackColor = false;
            this.fbrFunction.F04Button.Click += new System.EventHandler(this.fbrFunction_F04Button_Click);
            // 
            // fbrFunction.F05Button
            // 
            resources.ApplyResources(this.fbrFunction.F05Button, "fbrFunction.F05Button");
            this.fbrFunction.F05Button.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fbrFunction.F05Button.UseVisualStyleBackColor = false;
            this.fbrFunction.F05Button.Click += new System.EventHandler(this.fbrFunction_F05Button_Click);
            // 
            // fbrFunction.F06Button
            // 
            resources.ApplyResources(this.fbrFunction.F06Button, "fbrFunction.F06Button");
            this.fbrFunction.F06Button.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fbrFunction.F06Button.UseVisualStyleBackColor = false;
            this.fbrFunction.F06Button.Click += new System.EventHandler(this.fbrFunction_F06Button_Click);
            // 
            // fbrFunction.F07Button
            // 
            resources.ApplyResources(this.fbrFunction.F07Button, "fbrFunction.F07Button");
            this.fbrFunction.F07Button.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fbrFunction.F07Button.UseVisualStyleBackColor = false;
            this.fbrFunction.F07Button.Click += new System.EventHandler(this.fbrFunction_F07Button_Click);
            // 
            // fbrFunction.F08Button
            // 
            resources.ApplyResources(this.fbrFunction.F08Button, "fbrFunction.F08Button");
            this.fbrFunction.F08Button.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fbrFunction.F08Button.UseVisualStyleBackColor = false;
            this.fbrFunction.F08Button.Click += new System.EventHandler(this.fbrFunction_F08Button_Click);
            // 
            // fbrFunction.F09Button
            // 
            resources.ApplyResources(this.fbrFunction.F09Button, "fbrFunction.F09Button");
            this.fbrFunction.F09Button.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fbrFunction.F09Button.UseVisualStyleBackColor = false;
            this.fbrFunction.F09Button.Click += new System.EventHandler(this.fbrFunction_F09Button_Click);
            // 
            // fbrFunction.F10Button
            // 
            resources.ApplyResources(this.fbrFunction.F10Button, "fbrFunction.F10Button");
            this.fbrFunction.F10Button.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fbrFunction.F10Button.UseVisualStyleBackColor = false;
            this.fbrFunction.F10Button.Click += new System.EventHandler(this.fbrFunction_F10Button_Click);
            // 
            // fbrFunction.F11Button
            // 
            resources.ApplyResources(this.fbrFunction.F11Button, "fbrFunction.F11Button");
            this.fbrFunction.F11Button.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fbrFunction.F11Button.UseVisualStyleBackColor = false;
            this.fbrFunction.F11Button.Click += new System.EventHandler(this.fbrFunction_F11Button_Click);
            // 
            // fbrFunction.F12Button
            // 
            resources.ApplyResources(this.fbrFunction.F12Button, "fbrFunction.F12Button");
            this.fbrFunction.F12Button.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fbrFunction.F12Button.UseVisualStyleBackColor = false;
            this.fbrFunction.F12Button.Click += new System.EventHandler(this.fbrFunction_F12Button_Click);
            this.fbrFunction.LabelFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fbrFunction.LabelForeColor = System.Drawing.SystemColors.Window;
            this.fbrFunction.LabelHeight = 12;
            this.fbrFunction.Name = "fbrFunction";
            this.fbrFunction.PaddingBottom = 3;
            // 
            // lblMessage
            // 
            resources.ApplyResources(this.lblMessage, "lblMessage");
            this.lblMessage.BackColor = System.Drawing.Color.White;
            this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tlpTitle.SetColumnSpan(this.lblMessage, 5);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.TabStop = false;
            // 
            // txtRoleName
            // 
            this.txtRoleName.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.txtRoleName, "txtRoleName");
            this.txtRoleName.Name = "txtRoleName";
            this.txtRoleName.ReadOnly = true;
            this.txtRoleName.TabStop = false;
            // 
            // txtUserName
            // 
            this.txtUserName.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.txtUserName, "txtUserName");
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.ReadOnly = true;
            this.txtUserName.TabStop = false;
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Name = "lblTitle";
            // 
            // pnlTitleSpace
            // 
            resources.ApplyResources(this.pnlTitleSpace, "pnlTitleSpace");
            this.pnlTitleSpace.Name = "pnlTitleSpace";
            // 
            // pnlTitle
            // 
            this.pnlTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.pnlTitle.Controls.Add(tlpTitle);
            resources.ApplyResources(this.pnlTitle, "pnlTitle");
            this.pnlTitle.Name = "pnlTitle";
            // 
            // pnlMain
            // 
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(217)))), ((int)(((byte)(238)))));
            this.pnlMain.Controls.Add(this.fbrFunction);
            this.pnlMain.Controls.Add(this.lblCorner);
            this.pnlMain.Name = "pnlMain";
            // 
            // lblCorner
            // 
            resources.ApplyResources(this.lblCorner, "lblCorner");
            this.lblCorner.Name = "lblCorner";
            // 
            // sspStatus
            // 
            this.sspStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusMessage,
            this.lblDateTime});
            resources.ApplyResources(this.sspStatus, "sspStatus");
            this.sspStatus.Name = "sspStatus";
            this.sspStatus.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            // 
            // lblStatusMessage
            // 
            this.lblStatusMessage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblStatusMessage.ForeColor = System.Drawing.Color.White;
            this.lblStatusMessage.Name = "lblStatusMessage";
            resources.ApplyResources(this.lblStatusMessage, "lblStatusMessage");
            this.lblStatusMessage.Spring = true;
            this.lblStatusMessage.Click += new System.EventHandler(this.lblStatusMessage_Click);
            // 
            // lblDateTime
            // 
            this.lblDateTime.BackColor = System.Drawing.Color.Transparent;
            this.lblDateTime.ForeColor = System.Drawing.Color.White;
            this.lblDateTime.LabelBorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblDateTime, "lblDateTime");
            this.lblDateTime.Name = "lblDateTime";
            // 
            // BaseForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.sspStatus);
            this.Controls.Add(this.pnlTitle);
            this.KeyPreview = true;
            this.Name = "BaseForm";
            tlpTitle.ResumeLayout(false);
            tlpTitle.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.sspStatus.ResumeLayout(false);
            this.sspStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.StatusStrip sspStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusMessage;
        private SystemBase.Controls.ToolStripDateTimeLabel lblDateTime;
        protected System.Windows.Forms.Panel pnlTitle;
        protected DSWControl.DSWFunctionBar.DSWFunctionBar fbrFunction;
        protected System.Windows.Forms.TextBox txtUserName;
        protected System.Windows.Forms.TextBox txtRoleName;
        protected System.Windows.Forms.Label lblCorner;
        private System.Windows.Forms.Label lblTitle;
        private SystemBase.Controls.MessageLabel lblMessage;
        protected System.Windows.Forms.Panel pnlTitleSpace;

    }
}