/*
 * ProgressBar.cs - Implementation of "System.Windows.Forms.ProgressBar" 
 *
 * Copyright (C) 2003  Southern Storm Software, Pty Ltd.
 * 
 * Contributed by Gopal.V <gopalv82@symonds.net> 
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
	public sealed class ProgressBar: Control
	{
		
		private int min=0,max=100,value=0;
		private int step=10;
		private int range=max-min;

		public ProgressBar() : base("ProgressBar")
			{
			}

		/* NOTE: not really sure how the stuff is drawn , but
		*  this sure looks like it :-)
		*/
		private void Draw(Graphics graphics)
			{
				if(!Visible || !IsHandleCreated) return;

				Size clientSize = ClientSize;
				int x = 0;
				int y = 0;
				int width = clientSize.Width;
				int height = clientSize.Height;
				int steps=range/step;
				int blockWidth, blockHeight, xSpacing, ySpacing;
				
				using(Brush brush=CreateBackgroundBrush())
				{
					graphics.FillRectangle(brush,x,y,width,height);
				}

				ControlPaint.DrawBorder3D(graphics, 
											  x,y,width,height);

				width-=4;
				height-=4;
				x+=2;
				y+=2;

				xSpacing=2;
				ySpacing=2;
				width=width-((steps-1)*xSpacing);
				blockWidth=width/steps;
				blockHeight=height-ySpacing-1;
						
				x+=2*xSpacing;

				for(int i=0;i<steps;i++)
				{
					if((i*step) < value)
					{
						ControlPaint.DrawBlock(graphics, x, y+ySpacing, 
											blockWidth,
											blockHeight,
											SystemColors.Highlight);
					}
					x+=blockWidth+xSpacing;
				}
			}


		private void Redraw()
			{
				if(!Visible || !IsHandleCreated)
				{
					return;
				}

				using(Graphics graphics = CreateGraphics())
				{
					Draw(graphics);
				}
			}

		protected override void OnPaint(PaintEventArgs args)
			{
				Redraw();
				base.OnPaint(args);
			}

		public void Increment(int value)
			{
				Value=this.value+value;
				Redraw();
			}

		public void PerformStep()
			{
				if(value>=max)
				{
					value=min;
				}
				else
				{
					value=(value + (step - (value % step)));
				}
				Redraw();
			}

		protected override Size DefaultSize 
			{
 				get
				{
					return new Size(160,20);
				}
 			}	

		public int Maximum 
			{
 				get
				{
					return max;
				}

 				set
				{
					if(value < min)
					{
						throw new ArgumentOutOfRangeException("Maximum");
					}
					max=value;
					range=max-min;
					Redraw();
				}
	 		}

		public int Minimum 
			{
 				get
				{
					return min;
				}
	
 				set
				{
					if(value > max)
					{
						throw new ArgumentOutOfRangeException("Minimum");
					}
					min=value;
					range=max-min;
					Redraw();
				}

 			}

		public int Step 
			{
	 			get
				{
					return step;
				}

	 			set
				{
					if(step <=0)
					{
						throw new ArgumentOutOfRangeException("Step");
					}
					step=value;
					Redraw();
				}

 			}

		public int Value 
			{
	 			get
				{
					return value;
				}

	 			set
				{
					if(value > max || value < min)
					{
						throw new ArgumentOutOfRangeException("Value");
					}
					this.value=value;
					Redraw();
				}

	 		}

	}
}//namespace