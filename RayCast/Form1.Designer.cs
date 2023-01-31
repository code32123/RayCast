
namespace RayCast {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.raycastView = new System.Windows.Forms.PictureBox();
			this.minimap = new System.Windows.Forms.PictureBox();
			this.gameTimer = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.raycastView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.minimap)).BeginInit();
			this.SuspendLayout();
			// 
			// raycastView
			// 
			this.raycastView.Location = new System.Drawing.Point(12, 12);
			this.raycastView.Name = "raycastView";
			this.raycastView.Size = new System.Drawing.Size(835, 852);
			this.raycastView.TabIndex = 0;
			this.raycastView.TabStop = false;
			this.raycastView.Paint += new System.Windows.Forms.PaintEventHandler(this.RaycastView_Paint);
			// 
			// minimap
			// 
			this.minimap.Location = new System.Drawing.Point(853, 12);
			this.minimap.Name = "minimap";
			this.minimap.Size = new System.Drawing.Size(723, 852);
			this.minimap.TabIndex = 1;
			this.minimap.TabStop = false;
			this.minimap.Paint += new System.Windows.Forms.PaintEventHandler(this.Minimap_Paint);
			// 
			// gameTimer
			// 
			this.gameTimer.Enabled = true;
			this.gameTimer.Interval = 60;
			this.gameTimer.Tick += new System.EventHandler(this.GameTimer_Tick);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1588, 876);
			this.Controls.Add(this.minimap);
			this.Controls.Add(this.raycastView);
			this.Name = "Form1";
			this.Text = "Form1";
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.Form1_Layout);
			((System.ComponentModel.ISupportInitialize)(this.raycastView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.minimap)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox raycastView;
		private System.Windows.Forms.PictureBox minimap;
		private System.Windows.Forms.Timer gameTimer;
	}
}

