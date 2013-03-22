namespace Carl.Agent
{
    partial class MainForm
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
            this.buttonConfig = new System.Windows.Forms.Button();
            this.buttonListener = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonConfig
            // 
            this.buttonConfig.Location = new System.Drawing.Point(164, 207);
            this.buttonConfig.Name = "buttonConfig";
            this.buttonConfig.Size = new System.Drawing.Size(75, 23);
            this.buttonConfig.TabIndex = 0;
            this.buttonConfig.Text = "Config";
            this.buttonConfig.UseVisualStyleBackColor = true;
            this.buttonConfig.Click += new System.EventHandler(this.buttonConfig_Click);
            // 
            // buttonListener
            // 
            this.buttonListener.Location = new System.Drawing.Point(27, 37);
            this.buttonListener.Name = "buttonListener";
            this.buttonListener.Size = new System.Drawing.Size(140, 23);
            this.buttonListener.TabIndex = 1;
            this.buttonListener.UseVisualStyleBackColor = true;
            this.buttonListener.Click += new System.EventHandler(this.buttonListener_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.buttonListener);
            this.Controls.Add(this.buttonConfig);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonConfig;
        private System.Windows.Forms.Button buttonListener;
    }
}