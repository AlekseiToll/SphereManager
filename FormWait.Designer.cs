namespace SphereManager
{
	partial class FormWait
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
			this.btnCancel = new System.Windows.Forms.Button();
			this.labelMain = new System.Windows.Forms.Label();
			this.pbMain = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(153, 134);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Отмена";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// labelMain
			// 
			this.labelMain.AutoSize = true;
			this.labelMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelMain.Location = new System.Drawing.Point(30, 26);
			this.labelMain.Name = "labelMain";
			this.labelMain.Size = new System.Drawing.Size(305, 18);
			this.labelMain.TabIndex = 1;
			this.labelMain.Text = "Подождите, идет расстановка маркеров...";
			// 
			// pbMain
			// 
			this.pbMain.Location = new System.Drawing.Point(12, 73);
			this.pbMain.MarqueeAnimationSpeed = 50;
			this.pbMain.Name = "pbMain";
			this.pbMain.Size = new System.Drawing.Size(365, 23);
			this.pbMain.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.pbMain.TabIndex = 2;
			// 
			// FormWait
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Tan;
			this.ClientSize = new System.Drawing.Size(389, 165);
			this.Controls.Add(this.pbMain);
			this.Controls.Add(this.labelMain);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormWait";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label labelMain;
		private System.Windows.Forms.ProgressBar pbMain;
	}
}