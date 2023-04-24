namespace SphereManager
{
	partial class FormMain
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
			this.scMain = new System.Windows.Forms.SplitContainer();
			this.gbMoveMarker = new System.Windows.Forms.GroupBox();
			this.btnMoveMarker = new System.Windows.Forms.Button();
			this.numAngle = new System.Windows.Forms.NumericUpDown();
			this.labelAngle = new System.Windows.Forms.Label();
			this.numDistance = new System.Windows.Forms.NumericUpDown();
			this.labelDistance = new System.Windows.Forms.Label();
			this.btnDeleteMarker = new System.Windows.Forms.Button();
			this.labelList = new System.Windows.Forms.Label();
			this.lvMarkerList = new System.Windows.Forms.ListView();
			this.colId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colCoordinates = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnStopTest = new System.Windows.Forms.Button();
			this.btnStartTest = new System.Windows.Forms.Button();
			this.btnOpenTestFile = new System.Windows.Forms.Button();
			this.menuMain = new System.Windows.Forms.MenuStrip();
			this.miMode = new System.Windows.Forms.ToolStripMenuItem();
			this.miModeService = new System.Windows.Forms.ToolStripMenuItem();
			this.miModeTestXML = new System.Windows.Forms.ToolStripMenuItem();
			this.miSettingsMain = new System.Windows.Forms.ToolStripMenuItem();
			this.miSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDlgXml = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
			this.scMain.Panel1.SuspendLayout();
			this.scMain.Panel2.SuspendLayout();
			this.scMain.SuspendLayout();
			this.gbMoveMarker.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numAngle)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numDistance)).BeginInit();
			this.menuMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// scMain
			// 
			this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.scMain.IsSplitterFixed = true;
			this.scMain.Location = new System.Drawing.Point(0, 0);
			this.scMain.Name = "scMain";
			this.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// scMain.Panel1
			// 
			this.scMain.Panel1.Controls.Add(this.gbMoveMarker);
			this.scMain.Panel1.Controls.Add(this.btnDeleteMarker);
			this.scMain.Panel1.Controls.Add(this.labelList);
			this.scMain.Panel1.Controls.Add(this.lvMarkerList);
			// 
			// scMain.Panel2
			// 
			this.scMain.Panel2.BackColor = System.Drawing.Color.BurlyWood;
			this.scMain.Panel2.Controls.Add(this.btnStopTest);
			this.scMain.Panel2.Controls.Add(this.btnStartTest);
			this.scMain.Panel2.Controls.Add(this.btnOpenTestFile);
			this.scMain.Size = new System.Drawing.Size(863, 646);
			this.scMain.SplitterDistance = 417;
			this.scMain.TabIndex = 1;
			// 
			// gbMoveMarker
			// 
			this.gbMoveMarker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbMoveMarker.Controls.Add(this.btnMoveMarker);
			this.gbMoveMarker.Controls.Add(this.numAngle);
			this.gbMoveMarker.Controls.Add(this.labelAngle);
			this.gbMoveMarker.Controls.Add(this.numDistance);
			this.gbMoveMarker.Controls.Add(this.labelDistance);
			this.gbMoveMarker.Enabled = false;
			this.gbMoveMarker.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.gbMoveMarker.Location = new System.Drawing.Point(518, 57);
			this.gbMoveMarker.Name = "gbMoveMarker";
			this.gbMoveMarker.Size = new System.Drawing.Size(323, 222);
			this.gbMoveMarker.TabIndex = 4;
			this.gbMoveMarker.TabStop = false;
			this.gbMoveMarker.Text = "Переместить маркер";
			// 
			// btnMoveMarker
			// 
			this.btnMoveMarker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnMoveMarker.Enabled = false;
			this.btnMoveMarker.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnMoveMarker.Location = new System.Drawing.Point(53, 147);
			this.btnMoveMarker.Name = "btnMoveMarker";
			this.btnMoveMarker.Size = new System.Drawing.Size(123, 38);
			this.btnMoveMarker.TabIndex = 331;
			this.btnMoveMarker.Text = "Переместить";
			this.btnMoveMarker.UseVisualStyleBackColor = true;
			this.btnMoveMarker.Click += new System.EventHandler(this.btnMoveMarker_Click);
			// 
			// numAngle
			// 
			this.numAngle.Location = new System.Drawing.Point(152, 87);
			this.numAngle.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
			this.numAngle.Minimum = new decimal(new int[] {
            50000,
            0,
            0,
            -2147483648});
			this.numAngle.Name = "numAngle";
			this.numAngle.Size = new System.Drawing.Size(98, 26);
			this.numAngle.TabIndex = 330;
			this.numAngle.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// labelAngle
			// 
			this.labelAngle.AutoSize = true;
			this.labelAngle.BackColor = System.Drawing.Color.Transparent;
			this.labelAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.labelAngle.Location = new System.Drawing.Point(21, 84);
			this.labelAngle.Name = "labelAngle";
			this.labelAngle.Size = new System.Drawing.Size(45, 20);
			this.labelAngle.TabIndex = 329;
			this.labelAngle.Text = "Угол";
			// 
			// numDistance
			// 
			this.numDistance.DecimalPlaces = 1;
			this.numDistance.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numDistance.Location = new System.Drawing.Point(152, 50);
			this.numDistance.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
			this.numDistance.Minimum = new decimal(new int[] {
            50000,
            0,
            0,
            -2147483648});
			this.numDistance.Name = "numDistance";
			this.numDistance.Size = new System.Drawing.Size(98, 26);
			this.numDistance.TabIndex = 327;
			this.numDistance.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// labelDistance
			// 
			this.labelDistance.AutoSize = true;
			this.labelDistance.BackColor = System.Drawing.Color.Transparent;
			this.labelDistance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.labelDistance.Location = new System.Drawing.Point(21, 47);
			this.labelDistance.Name = "labelDistance";
			this.labelDistance.Size = new System.Drawing.Size(98, 20);
			this.labelDistance.TabIndex = 326;
			this.labelDistance.Text = "Расстояние";
			// 
			// btnDeleteMarker
			// 
			this.btnDeleteMarker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDeleteMarker.Enabled = false;
			this.btnDeleteMarker.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnDeleteMarker.Location = new System.Drawing.Point(12, 362);
			this.btnDeleteMarker.Name = "btnDeleteMarker";
			this.btnDeleteMarker.Size = new System.Drawing.Size(90, 32);
			this.btnDeleteMarker.TabIndex = 3;
			this.btnDeleteMarker.Text = "Удалить";
			this.btnDeleteMarker.UseVisualStyleBackColor = true;
			this.btnDeleteMarker.Click += new System.EventHandler(this.btnDeleteMarker_Click);
			// 
			// labelList
			// 
			this.labelList.AutoSize = true;
			this.labelList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelList.Location = new System.Drawing.Point(12, 34);
			this.labelList.Name = "labelList";
			this.labelList.Size = new System.Drawing.Size(140, 20);
			this.labelList.TabIndex = 2;
			this.labelList.Text = "Список маркеров";
			// 
			// lvMarkerList
			// 
			this.lvMarkerList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lvMarkerList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colId,
            this.colName,
            this.colCoordinates,
            this.colIP});
			this.lvMarkerList.FullRowSelect = true;
			this.lvMarkerList.Location = new System.Drawing.Point(12, 57);
			this.lvMarkerList.MultiSelect = false;
			this.lvMarkerList.Name = "lvMarkerList";
			this.lvMarkerList.Size = new System.Drawing.Size(479, 299);
			this.lvMarkerList.TabIndex = 1;
			this.lvMarkerList.UseCompatibleStateImageBehavior = false;
			this.lvMarkerList.View = System.Windows.Forms.View.Details;
			this.lvMarkerList.SelectedIndexChanged += new System.EventHandler(this.lvMarkerList_SelectedIndexChanged);
			// 
			// colId
			// 
			this.colId.Text = "Id";
			this.colId.Width = 150;
			// 
			// colName
			// 
			this.colName.Text = "Название";
			this.colName.Width = 0;
			// 
			// colCoordinates
			// 
			this.colCoordinates.Text = "Координаты";
			this.colCoordinates.Width = 0;
			// 
			// colIP
			// 
			this.colIP.Text = "IP адрес";
			this.colIP.Width = 200;
			// 
			// btnStopTest
			// 
			this.btnStopTest.Enabled = false;
			this.btnStopTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnStopTest.Location = new System.Drawing.Point(16, 143);
			this.btnStopTest.Name = "btnStopTest";
			this.btnStopTest.Size = new System.Drawing.Size(203, 52);
			this.btnStopTest.TabIndex = 2;
			this.btnStopTest.Text = "Остановить выполнение теста";
			this.btnStopTest.UseVisualStyleBackColor = true;
			this.btnStopTest.Click += new System.EventHandler(this.btnStopTest_Click);
			// 
			// btnStartTest
			// 
			this.btnStartTest.Enabled = false;
			this.btnStartTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnStartTest.Location = new System.Drawing.Point(16, 85);
			this.btnStartTest.Name = "btnStartTest";
			this.btnStartTest.Size = new System.Drawing.Size(203, 52);
			this.btnStartTest.TabIndex = 1;
			this.btnStartTest.Text = "Начать выполнение теста";
			this.btnStartTest.UseVisualStyleBackColor = true;
			this.btnStartTest.Click += new System.EventHandler(this.btnStartTest_Click);
			// 
			// btnOpenTestFile
			// 
			this.btnOpenTestFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnOpenTestFile.Location = new System.Drawing.Point(16, 27);
			this.btnOpenTestFile.Name = "btnOpenTestFile";
			this.btnOpenTestFile.Size = new System.Drawing.Size(203, 39);
			this.btnOpenTestFile.TabIndex = 0;
			this.btnOpenTestFile.Text = "Открыть файл теста";
			this.btnOpenTestFile.UseVisualStyleBackColor = true;
			this.btnOpenTestFile.Click += new System.EventHandler(this.btnOpenTestFile_Click);
			// 
			// menuMain
			// 
			this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMode,
            this.miSettingsMain});
			this.menuMain.Location = new System.Drawing.Point(0, 0);
			this.menuMain.Name = "menuMain";
			this.menuMain.Size = new System.Drawing.Size(863, 24);
			this.menuMain.TabIndex = 6;
			this.menuMain.Text = "menuStrip1";
			// 
			// miMode
			// 
			this.miMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miModeService,
            this.miModeTestXML});
			this.miMode.Name = "miMode";
			this.miMode.Size = new System.Drawing.Size(57, 20);
			this.miMode.Text = "Режим";
			// 
			// miModeService
			// 
			this.miModeService.CheckOnClick = true;
			this.miModeService.Name = "miModeService";
			this.miModeService.Size = new System.Drawing.Size(202, 22);
			this.miModeService.Text = "Тестовый";
			this.miModeService.Click += new System.EventHandler(this.miModeService_Click);
			// 
			// miModeTestXML
			// 
			this.miModeTestXML.Checked = true;
			this.miModeTestXML.CheckOnClick = true;
			this.miModeTestXML.CheckState = System.Windows.Forms.CheckState.Checked;
			this.miModeTestXML.Name = "miModeTestXML";
			this.miModeTestXML.Size = new System.Drawing.Size(202, 22);
			this.miModeTestXML.Text = "Выполнение теста XML";
			this.miModeTestXML.Click += new System.EventHandler(this.miModeTestXML_Click);
			// 
			// miSettingsMain
			// 
			this.miSettingsMain.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSettings});
			this.miSettingsMain.Name = "miSettingsMain";
			this.miSettingsMain.Size = new System.Drawing.Size(79, 20);
			this.miSettingsMain.Text = "Настройки";
			// 
			// miSettings
			// 
			this.miSettings.Name = "miSettings";
			this.miSettings.Size = new System.Drawing.Size(143, 22);
			this.miSettings.Text = "Настройки...";
			this.miSettings.Click += new System.EventHandler(this.miSettings_Click);
			// 
			// openFileDlgXml
			// 
			this.openFileDlgXml.FileName = "openFileDialog1";
			this.openFileDlgXml.Filter = "\"XML files (*.xml)|*.xml|All files (*.*)|*.*\"";
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Tan;
			this.ClientSize = new System.Drawing.Size(863, 646);
			this.Controls.Add(this.menuMain);
			this.Controls.Add(this.scMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.Name = "FormMain";
			this.Text = "Управление маркерами";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.scMain.Panel1.ResumeLayout(false);
			this.scMain.Panel1.PerformLayout();
			this.scMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
			this.scMain.ResumeLayout(false);
			this.gbMoveMarker.ResumeLayout(false);
			this.gbMoveMarker.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numAngle)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numDistance)).EndInit();
			this.menuMain.ResumeLayout(false);
			this.menuMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer scMain;
		private System.Windows.Forms.Button btnDeleteMarker;
		private System.Windows.Forms.Label labelList;
		private System.Windows.Forms.ListView lvMarkerList;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colCoordinates;
		private System.Windows.Forms.ColumnHeader colId;
		private System.Windows.Forms.ColumnHeader colIP;
		private System.Windows.Forms.GroupBox gbMoveMarker;
		private System.Windows.Forms.Button btnMoveMarker;
		private System.Windows.Forms.NumericUpDown numAngle;
		private System.Windows.Forms.Label labelAngle;
		private System.Windows.Forms.NumericUpDown numDistance;
		private System.Windows.Forms.Label labelDistance;
		private System.Windows.Forms.MenuStrip menuMain;
		private System.Windows.Forms.ToolStripMenuItem miMode;
		private System.Windows.Forms.ToolStripMenuItem miModeService;
		private System.Windows.Forms.ToolStripMenuItem miModeTestXML;
		private System.Windows.Forms.Button btnOpenTestFile;
		private System.Windows.Forms.Button btnStartTest;
		private System.Windows.Forms.OpenFileDialog openFileDlgXml;
		private System.Windows.Forms.Button btnStopTest;
		private System.Windows.Forms.ToolStripMenuItem miSettingsMain;
		private System.Windows.Forms.ToolStripMenuItem miSettings;
	}
}

