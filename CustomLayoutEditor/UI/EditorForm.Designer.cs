using System.ComponentModel;

namespace CustomLayoutEditor.UI;

partial class EditorForm
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
            this.flpComponents = new System.Windows.Forms.FlowLayoutPanel();
            this.pnGridContainer = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbName = new System.Windows.Forms.TextBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.nmMinimumHeight = new System.Windows.Forms.NumericUpDown();
            this.nmMinimumWidth = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nmDefaultHeight = new System.Windows.Forms.NumericUpDown();
            this.nmDefaultWidth = new System.Windows.Forms.NumericUpDown();
            this.cbResizable = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbLoadToMain = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmMinimumHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMinimumWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmDefaultHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmDefaultWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // flpComponents
            // 
            this.flpComponents.AutoScroll = true;
            this.flpComponents.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.flpComponents.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpComponents.Location = new System.Drawing.Point(0, 0);
            this.flpComponents.Name = "flpComponents";
            this.flpComponents.Size = new System.Drawing.Size(987, 155);
            this.flpComponents.TabIndex = 0;
            this.flpComponents.Tag = "color:dark3";
            this.flpComponents.WrapContents = false;
            // 
            // pnGridContainer
            // 
            this.pnGridContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnGridContainer.Location = new System.Drawing.Point(0, 201);
            this.pnGridContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnGridContainer.Name = "pnGridContainer";
            this.pnGridContainer.Padding = new System.Windows.Forms.Padding(16);
            this.pnGridContainer.Size = new System.Drawing.Size(987, 385);
            this.pnGridContainer.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.panel1.Controls.Add(this.tbName);
            this.panel1.Controls.Add(this.btnOpen);
            this.panel1.Controls.Add(this.btnSaveAs);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.nmMinimumHeight);
            this.panel1.Controls.Add(this.nmMinimumWidth);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.nmDefaultHeight);
            this.panel1.Controls.Add(this.nmDefaultWidth);
            this.panel1.Controls.Add(this.cbResizable);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbLoadToMain);
            this.panel1.AutoScroll = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 155);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(987, 46);
            this.panel1.TabIndex = 0;
            this.panel1.Tag = "color:dark3";
            // 
            // tbName
            // 
            this.tbName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.tbName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbName.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.tbName.ForeColor = System.Drawing.Color.White;
            this.tbName.Location = new System.Drawing.Point(682, 4);
            this.tbName.MaxLength = 400;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(122, 22);
            this.tbName.TabIndex = 198;
            this.tbName.TabStop = false;
            this.tbName.Tag = "color:dark2";
            this.tbName.Text = "New Custom Layout";
            this.tbName.WordWrap = false;
            // 
            // btnOpen
            // 
            this.btnOpen.BackColor = System.Drawing.Color.Gray;
            this.btnOpen.FlatAppearance.BorderSize = 0;
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpen.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnOpen.ForeColor = System.Drawing.Color.White;
            this.btnOpen.Location = new System.Drawing.Point(810, 5);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(45, 21);
            this.btnOpen.TabIndex = 197;
            this.btnOpen.Tag = "color:light1";
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = false;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.BackColor = System.Drawing.Color.Gray;
            this.btnSaveAs.FlatAppearance.BorderSize = 0;
            this.btnSaveAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveAs.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSaveAs.ForeColor = System.Drawing.Color.White;
            this.btnSaveAs.Location = new System.Drawing.Point(906, 5);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(68, 21);
            this.btnSaveAs.TabIndex = 196;
            this.btnSaveAs.Tag = "color:light1";
            this.btnSaveAs.Text = "Save As...";
            this.btnSaveAs.UseVisualStyleBackColor = false;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Gray;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(858, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(45, 21);
            this.btnSave.TabIndex = 195;
            this.btnSave.Tag = "color:light1";
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(576, 8);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 194;
            this.label4.Text = "by";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nmMinimumHeight
            // 
            this.nmMinimumHeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmMinimumHeight.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmMinimumHeight.ForeColor = System.Drawing.Color.White;
            this.nmMinimumHeight.Location = new System.Drawing.Point(598, 4);
            this.nmMinimumHeight.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nmMinimumHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmMinimumHeight.Name = "nmMinimumHeight";
            this.nmMinimumHeight.Size = new System.Drawing.Size(38, 22);
            this.nmMinimumHeight.TabIndex = 193;
            this.nmMinimumHeight.Tag = "color:dark1";
            this.nmMinimumHeight.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.nmMinimumHeight.ValueChanged += new System.EventHandler(this.nmMinimumHeight_ValueChanged);
            // 
            // nmMinimumWidth
            // 
            this.nmMinimumWidth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmMinimumWidth.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmMinimumWidth.ForeColor = System.Drawing.Color.White;
            this.nmMinimumWidth.Location = new System.Drawing.Point(535, 4);
            this.nmMinimumWidth.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nmMinimumWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmMinimumWidth.Name = "nmMinimumWidth";
            this.nmMinimumWidth.Size = new System.Drawing.Size(38, 22);
            this.nmMinimumWidth.TabIndex = 192;
            this.nmMinimumWidth.Tag = "color:dark1";
            this.nmMinimumWidth.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.nmMinimumWidth.ValueChanged += new System.EventHandler(this.nmMinimumWidth_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(365, 8);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 13);
            this.label3.TabIndex = 191;
            this.label3.Text = "by";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nmDefaultHeight
            // 
            this.nmDefaultHeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmDefaultHeight.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmDefaultHeight.ForeColor = System.Drawing.Color.White;
            this.nmDefaultHeight.Location = new System.Drawing.Point(387, 4);
            this.nmDefaultHeight.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nmDefaultHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmDefaultHeight.Name = "nmDefaultHeight";
            this.nmDefaultHeight.Size = new System.Drawing.Size(38, 22);
            this.nmDefaultHeight.TabIndex = 190;
            this.nmDefaultHeight.Tag = "color:dark1";
            this.nmDefaultHeight.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.nmDefaultHeight.ValueChanged += new System.EventHandler(this.nmDefaultHeight_ValueChanged);
            // 
            // nmDefaultWidth
            // 
            this.nmDefaultWidth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmDefaultWidth.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmDefaultWidth.ForeColor = System.Drawing.Color.White;
            this.nmDefaultWidth.Location = new System.Drawing.Point(324, 4);
            this.nmDefaultWidth.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nmDefaultWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmDefaultWidth.Name = "nmDefaultWidth";
            this.nmDefaultWidth.Size = new System.Drawing.Size(38, 22);
            this.nmDefaultWidth.TabIndex = 189;
            this.nmDefaultWidth.Tag = "color:dark1";
            this.nmDefaultWidth.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.nmDefaultWidth.ValueChanged += new System.EventHandler(this.nmDefaultWidth_ValueChanged);
            // 
            // cbResizable
            // 
            this.cbResizable.AutoSize = true;
            this.cbResizable.BackColor = System.Drawing.Color.Transparent;
            this.cbResizable.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbResizable.ForeColor = System.Drawing.Color.White;
            this.cbResizable.Location = new System.Drawing.Point(157, 7);
            this.cbResizable.Name = "cbResizable";
            this.cbResizable.Size = new System.Drawing.Size(74, 17);
            this.cbResizable.TabIndex = 188;
            this.cbResizable.Text = "Resizable";
            this.cbResizable.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(451, 8);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 187;
            this.label2.Text = "Minimum Size:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(250, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 186;
            this.label1.Text = "Default Size:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbLoadToMain
            // 
            this.cbLoadToMain.AutoSize = true;
            this.cbLoadToMain.BackColor = System.Drawing.Color.Transparent;
            this.cbLoadToMain.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbLoadToMain.ForeColor = System.Drawing.Color.White;
            this.cbLoadToMain.Location = new System.Drawing.Point(12, 7);
            this.cbLoadToMain.Name = "cbLoadToMain";
            this.cbLoadToMain.Size = new System.Drawing.Size(123, 17);
            this.cbLoadToMain.TabIndex = 185;
            this.cbLoadToMain.Text = "Load to Main Form";
            this.cbLoadToMain.UseVisualStyleBackColor = false;
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(987, 586);
            this.Controls.Add(this.pnGridContainer);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.flpComponents);
            this.Name = "EditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "color:dark2";
            this.Text = "Custom Layout Editor";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmMinimumHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMinimumWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmDefaultHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmDefaultWidth)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.FlowLayoutPanel flpComponents;
    private System.Windows.Forms.Panel pnGridContainer;
    private System.Windows.Forms.Panel panel1;
    public System.Windows.Forms.CheckBox cbLoadToMain;
    public System.Windows.Forms.CheckBox cbResizable;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    public System.Windows.Forms.NumericUpDown nmDefaultWidth;
    private System.Windows.Forms.Label label3;
    public System.Windows.Forms.NumericUpDown nmDefaultHeight;
    private System.Windows.Forms.Label label4;
    public System.Windows.Forms.NumericUpDown nmMinimumHeight;
    public System.Windows.Forms.NumericUpDown nmMinimumWidth;
    public System.Windows.Forms.Button btnSave;
    public System.Windows.Forms.Button btnSaveAs;
    public System.Windows.Forms.Button btnOpen;
    private System.Windows.Forms.TextBox tbName;
}