namespace SphereManager
{
	partial class FormSettings
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
			this.panelMain = new System.Windows.Forms.Panel();
			this.gbAmpq = new System.Windows.Forms.GroupBox();
			this.tbAmpqQueue = new System.Windows.Forms.TextBox();
			this.labelAmpqQueue = new System.Windows.Forms.Label();
			this.tbAmpqPass = new System.Windows.Forms.TextBox();
			this.labelAmpqPass = new System.Windows.Forms.Label();
			this.tbAmpqUser = new System.Windows.Forms.TextBox();
			this.labelAmpqUser = new System.Windows.Forms.Label();
			this.tbAmpqHostName = new System.Windows.Forms.TextBox();
			this.labelAmpqHostName = new System.Windows.Forms.Label();
			this.tbPortUdpFromXY = new System.Windows.Forms.TextBox();
			this.labelPortUdpFromXY = new System.Windows.Forms.Label();
			this.tbPortUdpFromMarkers = new System.Windows.Forms.TextBox();
			this.labelPortUdpFromMarkers = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.gbService = new System.Windows.Forms.GroupBox();
			this.tbTimeoutMoving = new System.Windows.Forms.TextBox();
			this.labelTimeoutMoving = new System.Windows.Forms.Label();
			this.labelInaccuracy = new System.Windows.Forms.Label();
			this.tbInaccSportsman = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tbInaccMarker = new System.Windows.Forms.TextBox();
			this.labelInaccMarker = new System.Windows.Forms.Label();
			this.panelMain.SuspendLayout();
			this.gbAmpq.SuspendLayout();
			this.gbService.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelMain
			// 
			this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panelMain.Controls.Add(this.gbService);
			this.panelMain.Controls.Add(this.gbAmpq);
			this.panelMain.Controls.Add(this.tbPortUdpFromXY);
			this.panelMain.Controls.Add(this.labelPortUdpFromXY);
			this.panelMain.Controls.Add(this.tbPortUdpFromMarkers);
			this.panelMain.Controls.Add(this.labelPortUdpFromMarkers);
			this.panelMain.Location = new System.Drawing.Point(12, 12);
			this.panelMain.Name = "panelMain";
			this.panelMain.Size = new System.Drawing.Size(618, 525);
			this.panelMain.TabIndex = 3;
			// 
			// gbAmpq
			// 
			this.gbAmpq.Controls.Add(this.tbAmpqQueue);
			this.gbAmpq.Controls.Add(this.labelAmpqQueue);
			this.gbAmpq.Controls.Add(this.tbAmpqPass);
			this.gbAmpq.Controls.Add(this.labelAmpqPass);
			this.gbAmpq.Controls.Add(this.tbAmpqUser);
			this.gbAmpq.Controls.Add(this.labelAmpqUser);
			this.gbAmpq.Controls.Add(this.tbAmpqHostName);
			this.gbAmpq.Controls.Add(this.labelAmpqHostName);
			this.gbAmpq.Location = new System.Drawing.Point(24, 190);
			this.gbAmpq.Name = "gbAmpq";
			this.gbAmpq.Size = new System.Drawing.Size(566, 100);
			this.gbAmpq.TabIndex = 6;
			this.gbAmpq.TabStop = false;
			this.gbAmpq.Text = "Настройки для работы с RabbitMQ";
			// 
			// tbAmpqQueue
			// 
			this.tbAmpqQueue.Location = new System.Drawing.Point(405, 52);
			this.tbAmpqQueue.Name = "tbAmpqQueue";
			this.tbAmpqQueue.Size = new System.Drawing.Size(118, 20);
			this.tbAmpqQueue.TabIndex = 7;
			// 
			// labelAmpqQueue
			// 
			this.labelAmpqQueue.AutoSize = true;
			this.labelAmpqQueue.Location = new System.Drawing.Point(300, 55);
			this.labelAmpqQueue.Name = "labelAmpqQueue";
			this.labelAmpqQueue.Size = new System.Drawing.Size(101, 13);
			this.labelAmpqQueue.TabIndex = 6;
			this.labelAmpqQueue.Text = "Название очереди";
			// 
			// tbAmpqPass
			// 
			this.tbAmpqPass.Location = new System.Drawing.Point(405, 26);
			this.tbAmpqPass.Name = "tbAmpqPass";
			this.tbAmpqPass.Size = new System.Drawing.Size(118, 20);
			this.tbAmpqPass.TabIndex = 5;
			// 
			// labelAmpqPass
			// 
			this.labelAmpqPass.AutoSize = true;
			this.labelAmpqPass.Location = new System.Drawing.Point(345, 29);
			this.labelAmpqPass.Name = "labelAmpqPass";
			this.labelAmpqPass.Size = new System.Drawing.Size(45, 13);
			this.labelAmpqPass.TabIndex = 4;
			this.labelAmpqPass.Text = "Пароль";
			// 
			// tbAmpqUser
			// 
			this.tbAmpqUser.Location = new System.Drawing.Point(127, 49);
			this.tbAmpqUser.Name = "tbAmpqUser";
			this.tbAmpqUser.Size = new System.Drawing.Size(118, 20);
			this.tbAmpqUser.TabIndex = 3;
			// 
			// labelAmpqUser
			// 
			this.labelAmpqUser.AutoSize = true;
			this.labelAmpqUser.Location = new System.Drawing.Point(18, 52);
			this.labelAmpqUser.Name = "labelAmpqUser";
			this.labelAmpqUser.Size = new System.Drawing.Size(103, 13);
			this.labelAmpqUser.TabIndex = 2;
			this.labelAmpqUser.Text = "Имя пользователя";
			// 
			// tbAmpqHostName
			// 
			this.tbAmpqHostName.Location = new System.Drawing.Point(127, 23);
			this.tbAmpqHostName.Name = "tbAmpqHostName";
			this.tbAmpqHostName.Size = new System.Drawing.Size(118, 20);
			this.tbAmpqHostName.TabIndex = 1;
			// 
			// labelAmpqHostName
			// 
			this.labelAmpqHostName.AutoSize = true;
			this.labelAmpqHostName.Location = new System.Drawing.Point(90, 29);
			this.labelAmpqHostName.Name = "labelAmpqHostName";
			this.labelAmpqHostName.Size = new System.Drawing.Size(31, 13);
			this.labelAmpqHostName.TabIndex = 0;
			this.labelAmpqHostName.Text = "Хост";
			// 
			// tbPortUdpFromXY
			// 
			this.tbPortUdpFromXY.Location = new System.Drawing.Point(26, 110);
			this.tbPortUdpFromXY.Name = "tbPortUdpFromXY";
			this.tbPortUdpFromXY.Size = new System.Drawing.Size(100, 20);
			this.tbPortUdpFromXY.TabIndex = 5;
			this.tbPortUdpFromXY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyDigit_KeyPress);
			// 
			// labelPortUdpFromXY
			// 
			this.labelPortUdpFromXY.AutoSize = true;
			this.labelPortUdpFromXY.Location = new System.Drawing.Point(23, 84);
			this.labelPortUdpFromXY.Name = "labelPortUdpFromXY";
			this.labelPortUdpFromXY.Size = new System.Drawing.Size(340, 13);
			this.labelPortUdpFromXY.TabIndex = 4;
			this.labelPortUdpFromXY.Text = "Номер порта для приема пакетов от системы позиционирования";
			// 
			// tbPortUdpFromMarkers
			// 
			this.tbPortUdpFromMarkers.Location = new System.Drawing.Point(26, 48);
			this.tbPortUdpFromMarkers.Name = "tbPortUdpFromMarkers";
			this.tbPortUdpFromMarkers.Size = new System.Drawing.Size(100, 20);
			this.tbPortUdpFromMarkers.TabIndex = 3;
			this.tbPortUdpFromMarkers.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyDigit_KeyPress);
			// 
			// labelPortUdpFromMarkers
			// 
			this.labelPortUdpFromMarkers.AutoSize = true;
			this.labelPortUdpFromMarkers.Location = new System.Drawing.Point(23, 23);
			this.labelPortUdpFromMarkers.Name = "labelPortUdpFromMarkers";
			this.labelPortUdpFromMarkers.Size = new System.Drawing.Size(246, 13);
			this.labelPortUdpFromMarkers.TabIndex = 2;
			this.labelPortUdpFromMarkers.Text = "Номер порта для приема пакетов от маркеров";
			// 
			// btnOk
			// 
			this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(223, 576);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(90, 30);
			this.btnOk.TabIndex = 4;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(329, 576);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(90, 30);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Отмена";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// gbService
			// 
			this.gbService.Controls.Add(this.tbInaccSportsman);
			this.gbService.Controls.Add(this.label2);
			this.gbService.Controls.Add(this.tbInaccMarker);
			this.gbService.Controls.Add(this.labelInaccMarker);
			this.gbService.Controls.Add(this.labelInaccuracy);
			this.gbService.Controls.Add(this.tbTimeoutMoving);
			this.gbService.Controls.Add(this.labelTimeoutMoving);
			this.gbService.Location = new System.Drawing.Point(24, 314);
			this.gbService.Name = "gbService";
			this.gbService.Size = new System.Drawing.Size(566, 193);
			this.gbService.TabIndex = 9;
			this.gbService.TabStop = false;
			this.gbService.Text = "Служебные настройки";
			// 
			// tbTimeoutMoving
			// 
			this.tbTimeoutMoving.Location = new System.Drawing.Point(21, 56);
			this.tbTimeoutMoving.Name = "tbTimeoutMoving";
			this.tbTimeoutMoving.Size = new System.Drawing.Size(118, 20);
			this.tbTimeoutMoving.TabIndex = 10;
			this.tbTimeoutMoving.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyDigit_KeyPress);
			// 
			// labelTimeoutMoving
			// 
			this.labelTimeoutMoving.AutoSize = true;
			this.labelTimeoutMoving.Location = new System.Drawing.Point(18, 33);
			this.labelTimeoutMoving.Name = "labelTimeoutMoving";
			this.labelTimeoutMoving.Size = new System.Drawing.Size(261, 13);
			this.labelTimeoutMoving.TabIndex = 9;
			this.labelTimeoutMoving.Text = "Таймаут занятия маркером нужной позиции (сек)";
			// 
			// labelInaccuracy
			// 
			this.labelInaccuracy.AutoSize = true;
			this.labelInaccuracy.Location = new System.Drawing.Point(18, 94);
			this.labelInaccuracy.Name = "labelInaccuracy";
			this.labelInaccuracy.Size = new System.Drawing.Size(223, 13);
			this.labelInaccuracy.TabIndex = 11;
			this.labelInaccuracy.Text = "Погрешность в определении позиции (мм)";
			// 
			// tbInaccSportsman
			// 
			this.tbInaccSportsman.Location = new System.Drawing.Point(157, 146);
			this.tbInaccSportsman.Name = "tbInaccSportsman";
			this.tbInaccSportsman.Size = new System.Drawing.Size(118, 20);
			this.tbInaccSportsman.TabIndex = 15;
			this.tbInaccSportsman.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyDigit_KeyPress);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(52, 149);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(71, 13);
			this.label2.TabIndex = 14;
			this.label2.Text = "спортсмена:";
			// 
			// tbInaccMarker
			// 
			this.tbInaccMarker.Location = new System.Drawing.Point(157, 120);
			this.tbInaccMarker.Name = "tbInaccMarker";
			this.tbInaccMarker.Size = new System.Drawing.Size(118, 20);
			this.tbInaccMarker.TabIndex = 13;
			this.tbInaccMarker.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyDigit_KeyPress);
			// 
			// labelInaccMarker
			// 
			this.labelInaccMarker.AutoSize = true;
			this.labelInaccMarker.Location = new System.Drawing.Point(52, 123);
			this.labelInaccMarker.Name = "labelInaccMarker";
			this.labelInaccMarker.Size = new System.Drawing.Size(54, 13);
			this.labelInaccMarker.TabIndex = 12;
			this.labelInaccMarker.Text = "маркера:";
			// 
			// FormSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.BurlyWood;
			this.ClientSize = new System.Drawing.Size(642, 619);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.panelMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormSettings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Настройки";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSettings_FormClosing);
			this.Load += new System.EventHandler(this.FormSettings_Load);
			this.panelMain.ResumeLayout(false);
			this.panelMain.PerformLayout();
			this.gbAmpq.ResumeLayout(false);
			this.gbAmpq.PerformLayout();
			this.gbService.ResumeLayout(false);
			this.gbService.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelMain;
		private System.Windows.Forms.TextBox tbPortUdpFromXY;
		private System.Windows.Forms.Label labelPortUdpFromXY;
		private System.Windows.Forms.TextBox tbPortUdpFromMarkers;
		private System.Windows.Forms.Label labelPortUdpFromMarkers;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox gbAmpq;
		private System.Windows.Forms.TextBox tbAmpqUser;
		private System.Windows.Forms.Label labelAmpqUser;
		private System.Windows.Forms.TextBox tbAmpqHostName;
		private System.Windows.Forms.Label labelAmpqHostName;
		private System.Windows.Forms.TextBox tbAmpqPass;
		private System.Windows.Forms.Label labelAmpqPass;
		private System.Windows.Forms.TextBox tbAmpqQueue;
		private System.Windows.Forms.Label labelAmpqQueue;
		private System.Windows.Forms.GroupBox gbService;
		private System.Windows.Forms.TextBox tbTimeoutMoving;
		private System.Windows.Forms.Label labelTimeoutMoving;
		private System.Windows.Forms.TextBox tbInaccSportsman;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbInaccMarker;
		private System.Windows.Forms.Label labelInaccMarker;
		private System.Windows.Forms.Label labelInaccuracy;
	}
}