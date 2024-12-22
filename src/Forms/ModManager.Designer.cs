namespace Erdtree_Launcher
{
    partial class ModManager
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
            btnClose = new Button();
            listAllMods = new ListBox();
            btnInstallSeamless = new Button();
            lbModLoaders = new Label();
            panel1 = new Panel();
            btnInstallMe2 = new Button();
            btnInstallEml = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            lbSeamless = new Label();
            lbEml = new Label();
            lbSeamlessStatus = new Label();
            lbEmlStatus = new Label();
            lbMe2 = new Label();
            lbMe2Status = new Label();
            lbInstalledMods = new Label();
            btnImportMod = new Button();
            lbImportMods = new Label();
            panel2 = new Panel();
            lbDownloadProgress = new Label();
            pbInstallLoader = new ProgressBar();
            tbImportModDisplayName = new TextBox();
            lbImportModName = new Label();
            rbModTypeEml = new RadioButton();
            rbModTypeMe2Dll = new RadioButton();
            rbModTypeMe2Folder = new RadioButton();
            lbModImportType = new Label();
            btnReloadUi = new Button();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // btnClose
            // 
            btnClose.Location = new Point(382, 425);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 0;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += BtnClose_Click;
            // 
            // listAllMods
            // 
            listAllMods.DrawMode = DrawMode.OwnerDrawFixed;
            listAllMods.FormattingEnabled = true;
            listAllMods.ItemHeight = 15;
            listAllMods.Items.AddRange(new object[] { "[Mods are displayed here]" });
            listAllMods.Location = new Point(12, 234);
            listAllMods.Name = "listAllMods";
            listAllMods.Size = new Size(213, 214);
            listAllMods.TabIndex = 1;
            listAllMods.DrawItem += ListAllMods_DrawItem;
            // 
            // btnInstallSeamless
            // 
            btnInstallSeamless.Location = new Point(308, 37);
            btnInstallSeamless.Name = "btnInstallSeamless";
            btnInstallSeamless.Size = new Size(112, 25);
            btnInstallSeamless.TabIndex = 2;
            btnInstallSeamless.Text = "Install Seamless";
            btnInstallSeamless.UseVisualStyleBackColor = true;
            btnInstallSeamless.Click += BtnInstallSeamless_Click;
            // 
            // lbModLoaders
            // 
            lbModLoaders.AutoSize = true;
            lbModLoaders.Font = new Font("Segoe UI", 15F);
            lbModLoaders.Location = new Point(44, 0);
            lbModLoaders.Name = "lbModLoaders";
            lbModLoaders.Size = new Size(127, 28);
            lbModLoaders.TabIndex = 0;
            lbModLoaders.Text = "Mod Loaders";
            // 
            // panel1
            // 
            panel1.Controls.Add(btnInstallMe2);
            panel1.Controls.Add(btnInstallEml);
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Controls.Add(btnInstallSeamless);
            panel1.Controls.Add(lbModLoaders);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(445, 143);
            panel1.TabIndex = 3;
            // 
            // btnInstallMe2
            // 
            btnInstallMe2.Location = new Point(308, 102);
            btnInstallMe2.Name = "btnInstallMe2";
            btnInstallMe2.Size = new Size(112, 23);
            btnInstallMe2.TabIndex = 4;
            btnInstallMe2.Text = "Install ME2";
            btnInstallMe2.UseVisualStyleBackColor = true;
            btnInstallMe2.Click += BtnInstallMe2_Click;
            // 
            // btnInstallEml
            // 
            btnInstallEml.Location = new Point(308, 70);
            btnInstallEml.Name = "btnInstallEml";
            btnInstallEml.Size = new Size(112, 23);
            btnInstallEml.TabIndex = 3;
            btnInstallEml.Text = "Install EML";
            btnInstallEml.UseVisualStyleBackColor = true;
            btnInstallEml.Click += BtnInstallEml_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Controls.Add(lbSeamless, 0, 0);
            tableLayoutPanel1.Controls.Add(lbEml, 0, 1);
            tableLayoutPanel1.Controls.Add(lbSeamlessStatus, 1, 0);
            tableLayoutPanel1.Controls.Add(lbEmlStatus, 1, 1);
            tableLayoutPanel1.Controls.Add(lbMe2, 0, 2);
            tableLayoutPanel1.Controls.Add(lbMe2Status, 1, 2);
            tableLayoutPanel1.Location = new Point(3, 31);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(5);
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Size = new Size(299, 108);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // lbSeamless
            // 
            lbSeamless.AutoSize = true;
            lbSeamless.Font = new Font("Segoe UI", 13F);
            lbSeamless.Location = new Point(8, 5);
            lbSeamless.Name = "lbSeamless";
            lbSeamless.Size = new Size(134, 25);
            lbSeamless.TabIndex = 0;
            lbSeamless.Text = "Seamless Coop";
            // 
            // lbEml
            // 
            lbEml.AutoSize = true;
            lbEml.Font = new Font("Segoe UI", 13F);
            lbEml.Location = new Point(8, 37);
            lbEml.Name = "lbEml";
            lbEml.Size = new Size(157, 25);
            lbEml.TabIndex = 1;
            lbEml.Text = "Elden Mod Loader";
            // 
            // lbSeamlessStatus
            // 
            lbSeamlessStatus.AutoSize = true;
            lbSeamlessStatus.Font = new Font("Segoe UI", 15F);
            lbSeamlessStatus.Location = new Point(239, 5);
            lbSeamlessStatus.Name = "lbSeamlessStatus";
            lbSeamlessStatus.Size = new Size(36, 28);
            lbSeamlessStatus.TabIndex = 2;
            lbSeamlessStatus.Text = "[X]";
            // 
            // lbEmlStatus
            // 
            lbEmlStatus.AutoSize = true;
            lbEmlStatus.Font = new Font("Segoe UI", 15F);
            lbEmlStatus.Location = new Point(239, 37);
            lbEmlStatus.Name = "lbEmlStatus";
            lbEmlStatus.Size = new Size(36, 28);
            lbEmlStatus.TabIndex = 3;
            lbEmlStatus.Text = "[X]";
            // 
            // lbMe2
            // 
            lbMe2.AutoSize = true;
            lbMe2.Font = new Font("Segoe UI", 13F);
            lbMe2.Location = new Point(8, 69);
            lbMe2.Name = "lbMe2";
            lbMe2.Size = new Size(123, 25);
            lbMe2.TabIndex = 4;
            lbMe2.Text = "Mod Engine 2";
            // 
            // lbMe2Status
            // 
            lbMe2Status.AutoSize = true;
            lbMe2Status.Font = new Font("Segoe UI", 15F);
            lbMe2Status.Location = new Point(239, 69);
            lbMe2Status.Name = "lbMe2Status";
            lbMe2Status.Size = new Size(36, 28);
            lbMe2Status.TabIndex = 5;
            lbMe2Status.Text = "[X]";
            // 
            // lbInstalledMods
            // 
            lbInstalledMods.AutoSize = true;
            lbInstalledMods.Font = new Font("Segoe UI", 13F);
            lbInstalledMods.Location = new Point(12, 206);
            lbInstalledMods.Name = "lbInstalledMods";
            lbInstalledMods.Size = new Size(129, 25);
            lbInstalledMods.TabIndex = 4;
            lbInstalledMods.Text = "Installed Mods";
            // 
            // btnImportMod
            // 
            btnImportMod.Location = new Point(254, 396);
            btnImportMod.Name = "btnImportMod";
            btnImportMod.Size = new Size(203, 23);
            btnImportMod.TabIndex = 5;
            btnImportMod.Text = "Import Mod";
            btnImportMod.UseVisualStyleBackColor = true;
            btnImportMod.Click += BtnImportMod_Click;
            // 
            // lbImportMods
            // 
            lbImportMods.AutoSize = true;
            lbImportMods.Font = new Font("Segoe UI", 13F);
            lbImportMods.Location = new Point(254, 206);
            lbImportMods.Name = "lbImportMods";
            lbImportMods.Size = new Size(159, 25);
            lbImportMods.TabIndex = 7;
            lbImportMods.Text = "Import/Add Mods";
            // 
            // panel2
            // 
            panel2.Controls.Add(lbDownloadProgress);
            panel2.Controls.Add(pbInstallLoader);
            panel2.Location = new Point(12, 161);
            panel2.Name = "panel2";
            panel2.Size = new Size(445, 29);
            panel2.TabIndex = 9;
            // 
            // lbDownloadProgress
            // 
            lbDownloadProgress.AutoSize = true;
            lbDownloadProgress.Font = new Font("Segoe UI", 11F);
            lbDownloadProgress.Location = new Point(308, 3);
            lbDownloadProgress.Name = "lbDownloadProgress";
            lbDownloadProgress.Size = new Size(50, 20);
            lbDownloadProgress.TabIndex = 1;
            lbDownloadProgress.Text = "Ready";
            // 
            // pbInstallLoader
            // 
            pbInstallLoader.Location = new Point(3, 3);
            pbInstallLoader.Name = "pbInstallLoader";
            pbInstallLoader.Size = new Size(299, 20);
            pbInstallLoader.TabIndex = 0;
            // 
            // tbImportModDisplayName
            // 
            tbImportModDisplayName.Enabled = false;
            tbImportModDisplayName.Location = new Point(254, 367);
            tbImportModDisplayName.Name = "tbImportModDisplayName";
            tbImportModDisplayName.Size = new Size(203, 23);
            tbImportModDisplayName.TabIndex = 10;
            // 
            // lbImportModName
            // 
            lbImportModName.AutoSize = true;
            lbImportModName.Font = new Font("Segoe UI", 10F);
            lbImportModName.Location = new Point(252, 345);
            lbImportModName.Name = "lbImportModName";
            lbImportModName.Size = new Size(205, 19);
            lbImportModName.TabIndex = 11;
            lbImportModName.Text = "Display Name (ME2 Folder only)";
            // 
            // rbModTypeEml
            // 
            rbModTypeEml.AutoSize = true;
            rbModTypeEml.BackColor = Color.WhiteSmoke;
            rbModTypeEml.Location = new Point(271, 273);
            rbModTypeEml.Name = "rbModTypeEml";
            rbModTypeEml.Size = new Size(171, 19);
            rbModTypeEml.TabIndex = 12;
            rbModTypeEml.TabStop = true;
            rbModTypeEml.Text = "Elden Mod Loader (.dll/.zip)";
            rbModTypeEml.UseVisualStyleBackColor = false;
            rbModTypeEml.CheckedChanged += RbModTypeEml_CheckedChanged;
            // 
            // rbModTypeMe2Dll
            // 
            rbModTypeMe2Dll.AutoSize = true;
            rbModTypeMe2Dll.Location = new Point(271, 298);
            rbModTypeMe2Dll.Name = "rbModTypeMe2Dll";
            rbModTypeMe2Dll.Size = new Size(148, 19);
            rbModTypeMe2Dll.TabIndex = 13;
            rbModTypeMe2Dll.TabStop = true;
            rbModTypeMe2Dll.Text = "Mod Engine 2 (.dll/.zip)";
            rbModTypeMe2Dll.UseVisualStyleBackColor = true;
            rbModTypeMe2Dll.CheckedChanged += RbModTypeMe2Dll_CheckedChanged;
            // 
            // rbModTypeMe2Folder
            // 
            rbModTypeMe2Folder.AutoSize = true;
            rbModTypeMe2Folder.Location = new Point(271, 323);
            rbModTypeMe2Folder.Name = "rbModTypeMe2Folder";
            rbModTypeMe2Folder.Size = new Size(142, 19);
            rbModTypeMe2Folder.TabIndex = 14;
            rbModTypeMe2Folder.TabStop = true;
            rbModTypeMe2Folder.Text = "Mod Engine 2 (Folder)";
            rbModTypeMe2Folder.UseVisualStyleBackColor = true;
            rbModTypeMe2Folder.CheckedChanged += RbModTypeMe2Folder_CheckedChanged;
            // 
            // lbModImportType
            // 
            lbModImportType.AutoSize = true;
            lbModImportType.Font = new Font("Segoe UI", 11F);
            lbModImportType.Location = new Point(260, 248);
            lbModImportType.Name = "lbModImportType";
            lbModImportType.Size = new Size(75, 20);
            lbModImportType.TabIndex = 15;
            lbModImportType.Text = "Mod Type";
            // 
            // btnReloadUi
            // 
            btnReloadUi.Location = new Point(254, 425);
            btnReloadUi.Name = "btnReloadUi";
            btnReloadUi.Size = new Size(124, 23);
            btnReloadUi.TabIndex = 16;
            btnReloadUi.Text = "Reload UI";
            btnReloadUi.UseVisualStyleBackColor = true;
            btnReloadUi.Click += BtnReloadUi_Click;
            // 
            // ModManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(471, 460);
            Controls.Add(btnReloadUi);
            Controls.Add(lbModImportType);
            Controls.Add(rbModTypeMe2Folder);
            Controls.Add(rbModTypeMe2Dll);
            Controls.Add(rbModTypeEml);
            Controls.Add(lbImportModName);
            Controls.Add(tbImportModDisplayName);
            Controls.Add(panel2);
            Controls.Add(lbImportMods);
            Controls.Add(btnImportMod);
            Controls.Add(lbInstalledMods);
            Controls.Add(panel1);
            Controls.Add(listAllMods);
            Controls.Add(btnClose);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ModManager";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ModManager";
            FormClosing += ModManager_FormClosing;
            Load += ModManager_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnClose;
        private ListBox listAllMods;
        private Button btnInstallSeamless;
        private Label lbModLoaders;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lbSeamless;
        private Label lbEml;
        private Label lbSeamlessStatus;
        private Label lbEmlStatus;
        private Label lbMe2;
        private Label lbMe2Status;
        private Button btnInstallEml;
        private Button btnInstallMe2;
        private Label lbInstalledMods;
        private Button btnImportMod;
        private Label lbImportMods;
        private Panel panel2;
        private ProgressBar pbInstallLoader;
        private TextBox tbImportModDisplayName;
        private Label lbImportModName;
        private RadioButton rbModTypeEml;
        private RadioButton rbModTypeMe2Dll;
        private RadioButton rbModTypeMe2Folder;
        private Label lbModImportType;
        private Label lbDownloadProgress;
        private Button btnReloadUi;
    }
}