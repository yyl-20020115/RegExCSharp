namespace Driver
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
            this.btnCompile = new System.Windows.Forms.Button();
            this.txtRegEx = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtStat = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnFindFirst = new System.Windows.Forms.Button();
            this.chkGreedy = new System.Windows.Forms.CheckBox();
            this.btnFindNext = new System.Windows.Forms.Button();
            this.txtSearchString = new System.Windows.Forms.TextBox();
            this.btnFindAll = new System.Windows.Forms.Button();
            this.grdResult = new System.Windows.Forms.DataGridView();
            this.matchStringDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startIndexDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endIndexDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lengthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.matchDS = new Driver.MatchInfoDS();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.matchDS)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCompile
            // 
            this.btnCompile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompile.Location = new System.Drawing.Point(844, 11);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(75, 21);
            this.btnCompile.TabIndex = 1;
            this.btnCompile.Text = "Compile";
            this.btnCompile.UseVisualStyleBackColor = true;
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // txtRegEx
            // 
            this.txtRegEx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRegEx.Font = new System.Drawing.Font("Courier New", 10F);
            this.txtRegEx.HideSelection = false;
            this.txtRegEx.Location = new System.Drawing.Point(171, 11);
            this.txtRegEx.Name = "txtRegEx";
            this.txtRegEx.Size = new System.Drawing.Size(667, 23);
            this.txtRegEx.TabIndex = 0;
            this.txtRegEx.Text = "a_*p";
            this.txtRegEx.Enter += new System.EventHandler(this.txtRegEx_Enter);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 38);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtStat);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Size = new System.Drawing.Size(907, 483);
            this.splitContainer1.SplitterDistance = 377;
            this.splitContainer1.SplitterWidth = 16;
            this.splitContainer1.TabIndex = 2;
            // 
            // txtStat
            // 
            this.txtStat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtStat.Font = new System.Drawing.Font("Courier New", 10F);
            this.txtStat.Location = new System.Drawing.Point(0, 12);
            this.txtStat.Multiline = true;
            this.txtStat.Name = "txtStat";
            this.txtStat.ReadOnly = true;
            this.txtStat.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtStat.Size = new System.Drawing.Size(377, 403);
            this.txtStat.TabIndex = 7;
            this.txtStat.WordWrap = false;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(377, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Statistics";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 415);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(377, 68);
            this.label1.TabIndex = 6;
            this.label1.Text = "LEGENDS:\r\n>    -Starting state\r\n{}   -Accepting state\r\n--   -No transition over t" +
    "he input symbol";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(77, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(257, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "(Enter text here then press a Find button)";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 12);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btnFindFirst);
            this.splitContainer2.Panel1.Controls.Add(this.chkGreedy);
            this.splitContainer2.Panel1.Controls.Add(this.btnFindNext);
            this.splitContainer2.Panel1.Controls.Add(this.txtSearchString);
            this.splitContainer2.Panel1.Controls.Add(this.btnFindAll);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.grdResult);
            this.splitContainer2.Size = new System.Drawing.Size(514, 471);
            this.splitContainer2.SplitterDistance = 280;
            this.splitContainer2.TabIndex = 10;
            // 
            // btnFindFirst
            // 
            this.btnFindFirst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindFirst.Location = new System.Drawing.Point(274, 248);
            this.btnFindFirst.Name = "btnFindFirst";
            this.btnFindFirst.Size = new System.Drawing.Size(75, 21);
            this.btnFindFirst.TabIndex = 6;
            this.btnFindFirst.Text = "Find First";
            this.btnFindFirst.UseVisualStyleBackColor = true;
            this.btnFindFirst.Click += new System.EventHandler(this.btnFindFirst_Click);
            // 
            // chkGreedy
            // 
            this.chkGreedy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkGreedy.AutoSize = true;
            this.chkGreedy.Checked = true;
            this.chkGreedy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGreedy.Location = new System.Drawing.Point(3, 252);
            this.chkGreedy.Name = "chkGreedy";
            this.chkGreedy.Size = new System.Drawing.Size(270, 16);
            this.chkGreedy.TabIndex = 2;
            this.chkGreedy.Text = "Match longest substring possible (greedy)";
            this.chkGreedy.UseVisualStyleBackColor = true;
            this.chkGreedy.CheckedChanged += new System.EventHandler(this.chkGreedy_CheckedChanged);
            // 
            // btnFindNext
            // 
            this.btnFindNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNext.Location = new System.Drawing.Point(355, 248);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(75, 21);
            this.btnFindNext.TabIndex = 4;
            this.btnFindNext.Text = "Find Next";
            this.btnFindNext.UseVisualStyleBackColor = true;
            this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
            // 
            // txtSearchString
            // 
            this.txtSearchString.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchString.Font = new System.Drawing.Font("Courier New", 10F);
            this.txtSearchString.HideSelection = false;
            this.txtSearchString.Location = new System.Drawing.Point(3, 3);
            this.txtSearchString.Multiline = true;
            this.txtSearchString.Name = "txtSearchString";
            this.txtSearchString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSearchString.Size = new System.Drawing.Size(508, 240);
            this.txtSearchString.TabIndex = 2;
            this.txtSearchString.Text = "appleandpotato";
            this.txtSearchString.Enter += new System.EventHandler(this.txtSearchString_Enter);
            this.txtSearchString.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchString_KeyDown);
            this.txtSearchString.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtSearchString_MouseDown);
            // 
            // btnFindAll
            // 
            this.btnFindAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindAll.Location = new System.Drawing.Point(436, 248);
            this.btnFindAll.Name = "btnFindAll";
            this.btnFindAll.Size = new System.Drawing.Size(75, 21);
            this.btnFindAll.TabIndex = 5;
            this.btnFindAll.Text = "Find All";
            this.btnFindAll.UseVisualStyleBackColor = true;
            this.btnFindAll.Click += new System.EventHandler(this.btnFindAll_Click);
            // 
            // grdResult
            // 
            this.grdResult.AllowUserToAddRows = false;
            this.grdResult.AllowUserToDeleteRows = false;
            this.grdResult.AllowUserToOrderColumns = true;
            this.grdResult.AllowUserToResizeColumns = false;
            this.grdResult.AllowUserToResizeRows = false;
            this.grdResult.AutoGenerateColumns = false;
            this.grdResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.matchStringDataGridViewTextBoxColumn,
            this.startIndexDataGridViewTextBoxColumn,
            this.endIndexDataGridViewTextBoxColumn,
            this.lengthDataGridViewTextBoxColumn});
            this.grdResult.DataMember = "MatchInfo";
            this.grdResult.DataSource = this.matchDS;
            this.grdResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResult.Location = new System.Drawing.Point(0, 0);
            this.grdResult.Name = "grdResult";
            this.grdResult.ReadOnly = true;
            this.grdResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdResult.Size = new System.Drawing.Size(514, 187);
            this.grdResult.TabIndex = 6;
            this.grdResult.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdResult_RowEnter);
            // 
            // matchStringDataGridViewTextBoxColumn
            // 
            this.matchStringDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.matchStringDataGridViewTextBoxColumn.DataPropertyName = "MatchString";
            this.matchStringDataGridViewTextBoxColumn.HeaderText = "MatchString";
            this.matchStringDataGridViewTextBoxColumn.Name = "matchStringDataGridViewTextBoxColumn";
            this.matchStringDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // startIndexDataGridViewTextBoxColumn
            // 
            this.startIndexDataGridViewTextBoxColumn.DataPropertyName = "StartIndex";
            this.startIndexDataGridViewTextBoxColumn.HeaderText = "StartIndex";
            this.startIndexDataGridViewTextBoxColumn.Name = "startIndexDataGridViewTextBoxColumn";
            this.startIndexDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // endIndexDataGridViewTextBoxColumn
            // 
            this.endIndexDataGridViewTextBoxColumn.DataPropertyName = "EndIndex";
            this.endIndexDataGridViewTextBoxColumn.HeaderText = "EndIndex";
            this.endIndexDataGridViewTextBoxColumn.Name = "endIndexDataGridViewTextBoxColumn";
            this.endIndexDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lengthDataGridViewTextBoxColumn
            // 
            this.lengthDataGridViewTextBoxColumn.DataPropertyName = "Length";
            this.lengthDataGridViewTextBoxColumn.HeaderText = "Length";
            this.lengthDataGridViewTextBoxColumn.Name = "lengthDataGridViewTextBoxColumn";
            this.lengthDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // matchDS
            // 
            this.matchDS.DataSetName = "MatchInfoDS";
            this.matchDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(514, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "Text Search";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "Enter Regular Expression:";
            // 
            // frmNfa
            // 
            this.AcceptButton = this.btnCompile;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 532);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.txtRegEx);
            this.Controls.Add(this.btnCompile);
            this.Name = "frmNfa";
            this.Text = "Test Regular Expression";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.matchDS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnCompile;
    private System.Windows.Forms.TextBox txtRegEx;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.TextBox txtStat;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.SplitContainer splitContainer2;
    private MatchInfoDS matchDS;
    private System.Windows.Forms.DataGridView grdResult;
    private System.Windows.Forms.DataGridViewTextBoxColumn matchStringDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn startIndexDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn endIndexDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn lengthDataGridViewTextBoxColumn;
    private System.Windows.Forms.TextBox txtSearchString;
    private System.Windows.Forms.Button btnFindNext;
    private System.Windows.Forms.Button btnFindAll;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.CheckBox chkGreedy;
    private System.Windows.Forms.Button btnFindFirst;
  }
}