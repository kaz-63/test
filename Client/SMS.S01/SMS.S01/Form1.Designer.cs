namespace SMS.S01
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.sheet1 = new GrapeCity.Win.ElTabelle.Sheet();
            ((System.ComponentModel.ISupportInitialize)(this.sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // sheet1
            // 
            this.sheet1.Data = ((GrapeCity.Win.ElTabelle.SheetData)(resources.GetObject("sheet1.Data")));
            this.sheet1.Location = new System.Drawing.Point(45, 52);
            this.sheet1.Name = "sheet1";
            this.sheet1.Size = new System.Drawing.Size(240, 150);
            this.sheet1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.sheet1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GrapeCity.Win.ElTabelle.Sheet sheet1;
    }
}