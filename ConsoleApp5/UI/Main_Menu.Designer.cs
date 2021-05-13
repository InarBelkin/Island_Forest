namespace Forest_Game.UI
{
    partial class Main_Menu
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
            this.GameMenu = new System.Windows.Forms.Panel();
            this.Exit = new System.Windows.Forms.Button();
            this.LoadGame = new System.Windows.Forms.Button();
            this.StartGame = new System.Windows.Forms.Button();
            this.GameTitle = new System.Windows.Forms.Label();
            this.GameMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // GameMenu
            // 
            this.GameMenu.Controls.Add(this.Exit);
            this.GameMenu.Controls.Add(this.LoadGame);
            this.GameMenu.Controls.Add(this.StartGame);
            this.GameMenu.Controls.Add(this.GameTitle);
            this.GameMenu.Location = new System.Drawing.Point(10, 10);
            this.GameMenu.Name = "GameMenu";
            this.GameMenu.Size = new System.Drawing.Size(310, 375);
            this.GameMenu.TabIndex = 0;
            // 
            // Exit
            // 
            this.Exit.Location = new System.Drawing.Point(30, 315);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(250, 50);
            this.Exit.TabIndex = 1;
            this.Exit.Text = "Выйти";
            this.Exit.UseVisualStyleBackColor = true;
            this.Exit.Click += new System.EventHandler(this.CloseGame);
            //
            //  LoadGame
            //
            this.LoadGame.Location = new System.Drawing.Point(30, 215);
            this.LoadGame.Name = "GameLoad";
            this.LoadGame.Size = new System.Drawing.Size(250, 50);
            this.LoadGame.TabIndex = 2;
            this.LoadGame.Text = "Загрузить игру";
            this.LoadGame.UseVisualStyleBackColor = true;
            this.LoadGame.Click += new System.EventHandler(this.CloseGame);
            //
            //  StartGame
            //
            this.StartGame.Location = new System.Drawing.Point(30, 115);
            this.StartGame.Name = "StartGame";
            this.StartGame.Size = new System.Drawing.Size(250, 50);
            this.StartGame.TabIndex = 3;
            this.StartGame.Text = "Начать игру";
            this.StartGame.UseVisualStyleBackColor = true;
            this.StartGame.Click += new System.EventHandler(this.GameStart);
            // 
            // GameTitle
            // 
            this.GameTitle.AutoSize = true;
            this.GameTitle.Font = new System.Drawing.Font("Comic Sans MS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GameTitle.Location = new System.Drawing.Point(30, 10);
            this.GameTitle.Name = "GameTitle";
            this.GameTitle.Size = new System.Drawing.Size(252, 45);
            this.GameTitle.TabIndex = 4;
            this.GameTitle.Text = "Игра Леса";
            // 
            // Main_Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 26F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(334, 416);
            this.Controls.Add(GameMenu);
            this.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.Name = "Main_Menu";
            this.Text = "Игра леса";
            this.GameMenu.ResumeLayout(false);
            this.GameMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel GameMenu;
        private System.Windows.Forms.Button Exit;
        private System.Windows.Forms.Button LoadGame;
        private System.Windows.Forms.Button StartGame;
        private System.Windows.Forms.Label GameTitle;


    }
}