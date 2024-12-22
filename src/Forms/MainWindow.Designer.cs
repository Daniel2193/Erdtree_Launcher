namespace Erdtree_Launcher
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            btnModded = new Button();
            btnCoop = new Button();
            btnVanillaOffline = new Button();
            btnVanillaOnline = new Button();
            panelLaunch = new Panel();
            panelModsEnabled = new Panel();
            label1 = new Label();
            listModsEnabled = new ListBox();
            panelModsDisabled = new Panel();
            label2 = new Label();
            listModsDisabled = new ListBox();
            btnQuit = new Button();
            btnModManager = new Button();
            btnUpdate = new Button();
            lbModsEnabledSubtitle = new Label();
            lbModsDisabledSubtitle = new Label();
            panelLaunch.SuspendLayout();
            panelModsEnabled.SuspendLayout();
            panelModsDisabled.SuspendLayout();
            SuspendLayout();
            // 
            // btnModded
            // 
            resources.ApplyResources(btnModded, "btnModded");
            btnModded.Name = "btnModded";
            btnModded.UseVisualStyleBackColor = true;
            btnModded.Click += BtnModded_Click;
            // 
            // btnCoop
            // 
            resources.ApplyResources(btnCoop, "btnCoop");
            btnCoop.Name = "btnCoop";
            btnCoop.UseVisualStyleBackColor = true;
            btnCoop.Click += BtnCoop_Click;
            // 
            // btnVanillaOffline
            // 
            resources.ApplyResources(btnVanillaOffline, "btnVanillaOffline");
            btnVanillaOffline.Name = "btnVanillaOffline";
            btnVanillaOffline.UseVisualStyleBackColor = true;
            btnVanillaOffline.Click += BtnVanillaOffline_Click;
            // 
            // btnVanillaOnline
            // 
            resources.ApplyResources(btnVanillaOnline, "btnVanillaOnline");
            btnVanillaOnline.Name = "btnVanillaOnline";
            btnVanillaOnline.UseVisualStyleBackColor = true;
            btnVanillaOnline.Click += BtnVanillaOnline_Click;
            // 
            // panelLaunch
            // 
            panelLaunch.Controls.Add(btnModded);
            panelLaunch.Controls.Add(btnVanillaOnline);
            panelLaunch.Controls.Add(btnCoop);
            panelLaunch.Controls.Add(btnVanillaOffline);
            resources.ApplyResources(panelLaunch, "panelLaunch");
            panelLaunch.Name = "panelLaunch";
            // 
            // panelModsEnabled
            // 
            panelModsEnabled.Controls.Add(lbModsEnabledSubtitle);
            panelModsEnabled.Controls.Add(label1);
            panelModsEnabled.Controls.Add(listModsEnabled);
            resources.ApplyResources(panelModsEnabled, "panelModsEnabled");
            panelModsEnabled.Name = "panelModsEnabled";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // listModsEnabled
            // 
            listModsEnabled.AllowDrop = true;
            listModsEnabled.DrawMode = DrawMode.OwnerDrawFixed;
            listModsEnabled.FormattingEnabled = true;
            resources.ApplyResources(listModsEnabled, "listModsEnabled");
            listModsEnabled.Name = "listModsEnabled";
            listModsEnabled.DrawItem += ListMods_DrawItem;
            listModsEnabled.SelectedIndexChanged += ListModsEnabled_SelectedIndexChanged;
            listModsEnabled.DragDrop += ListMods_DragDrop;
            listModsEnabled.DragOver += ListMods_DragOver;
            listModsEnabled.MouseDown += ListMods_MouseDown;
            // 
            // panelModsDisabled
            // 
            panelModsDisabled.Controls.Add(lbModsDisabledSubtitle);
            panelModsDisabled.Controls.Add(label2);
            panelModsDisabled.Controls.Add(listModsDisabled);
            resources.ApplyResources(panelModsDisabled, "panelModsDisabled");
            panelModsDisabled.Name = "panelModsDisabled";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // listModsDisabled
            // 
            listModsDisabled.AllowDrop = true;
            listModsDisabled.DrawMode = DrawMode.OwnerDrawFixed;
            listModsDisabled.FormattingEnabled = true;
            resources.ApplyResources(listModsDisabled, "listModsDisabled");
            listModsDisabled.Name = "listModsDisabled";
            listModsDisabled.DrawItem += ListMods_DrawItem;
            listModsDisabled.DragDrop += ListMods_DragDrop;
            listModsDisabled.DragOver += ListMods_DragOver;
            listModsDisabled.MouseDown += ListMods_MouseDown;
            // 
            // btnQuit
            // 
            resources.ApplyResources(btnQuit, "btnQuit");
            btnQuit.Name = "btnQuit";
            btnQuit.UseVisualStyleBackColor = true;
            btnQuit.Click += BtnQuit_Click;
            // 
            // btnModManager
            // 
            resources.ApplyResources(btnModManager, "btnModManager");
            btnModManager.Name = "btnModManager";
            btnModManager.UseVisualStyleBackColor = true;
            btnModManager.Click += BtnModManager_Click;
            // 
            // btnUpdate
            // 
            resources.ApplyResources(btnUpdate, "btnUpdate");
            btnUpdate.Name = "btnUpdate";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += BtnUpdate_Click;
            // 
            // lbModsEnabledSubtitle
            // 
            resources.ApplyResources(lbModsEnabledSubtitle, "lbModsEnabledSubtitle");
            lbModsEnabledSubtitle.Name = "lbModsEnabledSubtitle";
            // 
            // lbModsDisabledSubtitle
            // 
            resources.ApplyResources(lbModsDisabledSubtitle, "lbModsDisabledSubtitle");
            lbModsDisabledSubtitle.Name = "lbModsDisabledSubtitle";
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnUpdate);
            Controls.Add(btnModManager);
            Controls.Add(btnQuit);
            Controls.Add(panelModsDisabled);
            Controls.Add(panelModsEnabled);
            Controls.Add(panelLaunch);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainWindow";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Hide;
            Load += Form1_Load;
            panelLaunch.ResumeLayout(false);
            panelModsEnabled.ResumeLayout(false);
            panelModsEnabled.PerformLayout();
            panelModsDisabled.ResumeLayout(false);
            panelModsDisabled.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button btnModded;
        private Button btnCoop;
        private Button btnVanillaOffline;
        private Button btnVanillaOnline;
        private Panel panelLaunch;
        private Panel panelModsEnabled;
        private ListBox listModsEnabled;
        private Label label1;
        private Panel panelModsDisabled;
        private Label label2;
        private ListBox listModsDisabled;
        private Button btnQuit;
        private Button btnModManager;
        private Button btnUpdate;
        private Label lbModsEnabledSubtitle;
        private Label lbModsDisabledSubtitle;
    }
}
