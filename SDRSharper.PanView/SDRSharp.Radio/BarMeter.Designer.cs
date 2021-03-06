﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SDRSharp.Radio
{
	// Token: 0x02000002 RID: 2
	public class BarMeter : UserControl
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public BarMeter()
		{
			this.InitializeComponent();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x00002088 File Offset: 0x00000288
		public int Max
		{
			get
			{
				return this._max;
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002090 File Offset: 0x00000290
		public bool Draw(int value, int timOut = 0)
		{
			if (timOut > 0)
			{
				if (--this._timOut > 0)
				{
					return false;
				}
				this._timOut = timOut;
			}
			float num = (float)value / (float)this._max;
			if ((double)num > 1.0)
			{
				this._step = (int)((float)this._max * num / 5f);
				int num2 = 1;
				for (int i = 1; i <= 4; i++)
				{
					int num3 = num2;
					if (num2 >= this._step)
					{
						break;
					}
					num2 = num3 * 2;
					if (num2 >= this._step)
					{
						break;
					}
					num2 = num3 * 5;
					if (num2 > this._step)
					{
						break;
					}
					num2 = num3 * 10;
				}
				this._step = num2;
				this._max = this._step * 5;
				num = (float)value / (float)this._max;
				this.DrawBackground();
			}
			if (num < this._len)
			{
				this._grP.Clear(Color.FromArgb(50, 50, 50));
			}
			this._grP.FillRectangle(this._brush, 0f, 0f, num * (float)this.pic.Width, (float)this.pic.Height);
			this._len = num;
			return true;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000021A8 File Offset: 0x000003A8
		public void DrawBackground()
		{
			this.DrawBackground(this._min, this._max, this._step);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021C4 File Offset: 0x000003C4
		public void DrawBackground(int min, int max, int step)
		{
			this._min = min;
			this._max = max;
			this._step = step;
			this._grU.Clear(Color.FromArgb(64, 64, 64));
			using (Font font = new Font("Aerial", 6f))
			{
				using (Pen pen = new Pen(Color.Yellow))
				{
					for (int i = min; i <= max; i += step)
					{
						string text = i.ToString();
						float num = Math.Max(1f, (float)this.pic.Size.Width * (float)i / (float)max);
						this._grU.DrawLine(pen, num, (float)(this.pic.Location.Y - this.tickSize), num, (float)this.pic.Location.Y);
						float height = this._grU.MeasureString(text, font).Height;
						float num2 = (i == min) ? 2f : this._grU.MeasureString(text, font).Width;
						this._grU.DrawString(text, font, Brushes.Yellow, num - num2 / 2f, (float)(this.pic.Location.Y - this.tickSize) - height);
					}
				}
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002364 File Offset: 0x00000564
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			this.DrawBackground();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002374 File Offset: 0x00000574
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (this._grU != null)
			{
				this._grU.Dispose();
				this._grP.Dispose();
			}
			this._grU = base.CreateGraphics();
			this._grP = this.pic.CreateGraphics();
			if (this._brush != null)
			{
				this._brush.Dispose();
			}
			Rectangle rect = new Rectangle(0, 0, this.pic.Width, this.pic.Height);
			this._brush = new LinearGradientBrush(rect, Color.Black, Color.White, LinearGradientMode.Vertical);
			ColorBlend colorBlend = new ColorBlend();
			colorBlend.Colors = new Color[]
			{
				Color.Green,
				Color.LightGreen,
				Color.Green
			};
			colorBlend.Positions = new float[]
			{
				0f,
				0.33f,
				1f
			};
			this._brush.InterpolationColors = colorBlend;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000247D File Offset: 0x0000067D
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000249C File Offset: 0x0000069C
		private void InitializeComponent()
		{
			this.pic = new PictureBox();
			((ISupportInitialize)this.pic).BeginInit();
			base.SuspendLayout();
			this.pic.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.pic.BackColor = SystemColors.ButtonFace;
			this.pic.BorderStyle = BorderStyle.FixedSingle;
			this.pic.Location = new Point(0, 15);
			this.pic.Name = "pic";
			this.pic.Size = new Size(190, 20);
			this.pic.TabIndex = 0;
			this.pic.TabStop = false;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.pic);
			base.Name = "BarMeter";
			base.Size = new Size(200, 35);
			((ISupportInitialize)this.pic).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04000001 RID: 1
		private int _min;

		// Token: 0x04000002 RID: 2
		private int _max = 100;

		// Token: 0x04000003 RID: 3
		private int _step = 20;

		// Token: 0x04000004 RID: 4
		private float _len = 1f;

		// Token: 0x04000005 RID: 5
		private int tickSize = 5;

		// Token: 0x04000006 RID: 6
		private Graphics _grU;

		// Token: 0x04000007 RID: 7
		private Graphics _grP;

		// Token: 0x04000008 RID: 8
		private LinearGradientBrush _brush;

		// Token: 0x04000009 RID: 9
		private int _timOut = 10;

		// Token: 0x0400000A RID: 10
		private IContainer components;

		// Token: 0x0400000B RID: 11
		private PictureBox pic;
	}
}
