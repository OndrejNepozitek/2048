namespace GUI
{
	partial class GameWindow
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
			this.boardContainer = new System.Windows.Forms.Panel();
			this.scoreLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// boardContainer
			// 
			this.boardContainer.Location = new System.Drawing.Point(16, 71);
			this.boardContainer.Name = "boardContainer";
			this.boardContainer.Size = new System.Drawing.Size(600, 600);
			this.boardContainer.TabIndex = 0;
			// 
			// scoreLabel
			// 
			this.scoreLabel.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.scoreLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.scoreLabel.Location = new System.Drawing.Point(13, 22);
			this.scoreLabel.Name = "scoreLabel";
			this.scoreLabel.Size = new System.Drawing.Size(199, 23);
			this.scoreLabel.TabIndex = 1;
			this.scoreLabel.Text = "Score: 0";
			// 
			// GameWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(632, 683);
			this.Controls.Add(this.scoreLabel);
			this.Controls.Add(this.boardContainer);
			this.Name = "GameWindow";
			this.Text = "GameWindow";
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameWindow_KeyUp);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel boardContainer;
		private System.Windows.Forms.Label scoreLabel;
	}
}