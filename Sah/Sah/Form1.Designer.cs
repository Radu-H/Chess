namespace Sah
{
    partial class Form1
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
            this.start_button = new System.Windows.Forms.Button();
            this.juc1_lb = new System.Windows.Forms.Label();
            this.juc2_lb = new System.Windows.Forms.Label();
            this.juc1 = new System.Windows.Forms.TextBox();
            this.juc2 = new System.Windows.Forms.TextBox();
            this.jc1 = new System.Windows.Forms.Label();
            this.jc2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // start_button
            // 
            this.start_button.Location = new System.Drawing.Point(399, 270);
            this.start_button.Name = "start_button";
            this.start_button.Size = new System.Drawing.Size(150, 60);
            this.start_button.TabIndex = 0;
            this.start_button.Text = "START!";
            this.start_button.UseVisualStyleBackColor = true;
            this.start_button.Click += new System.EventHandler(this.start_Click);
            // 
            // juc1_lb
            // 
            this.juc1_lb.AutoSize = true;
            this.juc1_lb.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.juc1_lb.Location = new System.Drawing.Point(148, 286);
            this.juc1_lb.Name = "juc1_lb";
            this.juc1_lb.Size = new System.Drawing.Size(82, 24);
            this.juc1_lb.TabIndex = 1;
            this.juc1_lb.Text = "Player 1:";
            // 
            // juc2_lb
            // 
            this.juc2_lb.AutoSize = true;
            this.juc2_lb.BackColor = System.Drawing.SystemColors.Control;
            this.juc2_lb.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.juc2_lb.Location = new System.Drawing.Point(699, 286);
            this.juc2_lb.Name = "juc2_lb";
            this.juc2_lb.Size = new System.Drawing.Size(82, 24);
            this.juc2_lb.TabIndex = 2;
            this.juc2_lb.Text = "Player 2:";
            // 
            // juc1
            // 
            this.juc1.Location = new System.Drawing.Point(76, 340);
            this.juc1.Name = "juc1";
            this.juc1.Size = new System.Drawing.Size(237, 20);
            this.juc1.TabIndex = 3;
            // 
            // juc2
            // 
            this.juc2.Location = new System.Drawing.Point(632, 340);
            this.juc2.Name = "juc2";
            this.juc2.Size = new System.Drawing.Size(237, 20);
            this.juc2.TabIndex = 4;
            // 
            // jc1
            // 
            this.jc1.AutoSize = true;
            this.jc1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jc1.Location = new System.Drawing.Point(889, 622);
            this.jc1.Name = "jc1";
            this.jc1.Size = new System.Drawing.Size(0, 17);
            this.jc1.TabIndex = 5;
            this.jc1.Visible = false;
            // 
            // jc2
            // 
            this.jc2.AutoSize = true;
            this.jc2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jc2.Location = new System.Drawing.Point(36, 40);
            this.jc2.Name = "jc2";
            this.jc2.Size = new System.Drawing.Size(0, 17);
            this.jc2.TabIndex = 6;
            this.jc2.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 691);
            this.Controls.Add(this.jc2);
            this.Controls.Add(this.jc1);
            this.Controls.Add(this.juc2);
            this.Controls.Add(this.juc1);
            this.Controls.Add(this.juc2_lb);
            this.Controls.Add(this.juc1_lb);
            this.Controls.Add(this.start_button);
            this.Name = "Form1";
            this.Text = "Sah";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.LocationChanged += new System.EventHandler(this.Form1_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button start_button;
        private System.Windows.Forms.Label juc1_lb;
        private System.Windows.Forms.Label juc2_lb;
        private System.Windows.Forms.TextBox juc1;
        private System.Windows.Forms.TextBox juc2;
        private System.Windows.Forms.Label jc1;
        private System.Windows.Forms.Label jc2;
    }
}

