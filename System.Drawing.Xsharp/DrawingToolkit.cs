/*
 * DrawingToolkit.cs - Implementation of IToolkit for Xsharp.
 *
 * Copyright (C) 2003  Southern Storm Software, Pty Ltd.
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

namespace System.Drawing.Toolkit
{

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Toolkit;
using Xsharp;

public sealed class DrawingToolkit : IToolkit
{
	// Internal state.
	internal Application app;
	internal Widget placeholder;

	// Constructor.
	public DrawingToolkit()
			{
				// Create an Xsharp application instance.
				app = new Xsharp.Application(null, null);

				// Get the placeholder widget for the screen.
				placeholder = app.Display.DefaultScreenOfDisplay.Placeholder;
			}

	// Run the main event processing loop for the toolkit.
	public void Run()
			{
				app.Run();
			}

	// Send a quit message to the toolkit, which should cause
	// it to exit from the "Run" method.
	public void Quit()
			{
				app.Display.Quit();
			}

	// Resolve a system color to an RGB value.  Returns -1 if the
	// system does not support the color and a default should be used.
	public int ResolveSystemColor(KnownColor color)
			{
				return -1;
			}

	// Create an IToolkitGraphics object from a HDC.
	public IToolkitGraphics CreateFromHdc(IntPtr hdc, IntPtr hdevice)
			{
				// We don't use HDC's in this implementation.
				return null;
			}

	// Create an IToolkitGraphics object from a HWND.
	public IToolkitGraphics CreateFromHwnd(IntPtr hwnd)
			{
				// We don't use HWND's in this implementation.
				return null;
			}

	// Create a solid toolkit brush.
	public IToolkitBrush CreateSolidBrush(System.Drawing.Color color)
			{
				return new DrawingSolidBrush(color);
			}

	// Create a hatched toolkit brush.
	public IToolkitBrush CreateHatchBrush
					(HatchStyle style, System.Drawing.Color foreColor,
					 System.Drawing.Color backColor)
			{
				return new DrawingHatchBrush(style, foreColor, backColor);
			}

	// Create a linear gradient brush.  Returns null if the
	// toolkit does not support linear gradient brushes.
	public IToolkitBrush CreateLinearGradientBrush
				(RectangleF rect, System.Drawing.Color color1,
				 System.Drawing.Color color2,
				 LinearGradientMode mode)
			{
				return null;
			}
	public IToolkitBrush CreateLinearGradientBrush
				(RectangleF rect, System.Drawing.Color color1,
				 System.Drawing.Color color2, float angle,
				 bool isAngleScaleable)
			{
				return null;
			}

	// Create a toolkit pen from the properties in the specified object.
	// If the toolkit does not support the precise combination of pen
	// properties, it will return the closest matching pen.
	public IToolkitPen CreatePen(Pen pen)
			{
				return new DrawingPen(pen);
			}

	// Create a toolkit font from the properties in the specified object.
	public IToolkitFont CreateFont(System.Drawing.Font font)
			{
				// TODO
				return null;
			}

	// Get the handle for the halftone palette.  IntPtr.Zero if not supported.
	public IntPtr GetHalftonePalette()
			{
				return IntPtr.Zero;
			}

	// Create a top-level application window.
	public IToolkitWindow CreateTopLevelWindow(int width, int height)
			{
				return new DrawingTopLevelWindow
					(this, String.Empty, width, height);
			}

	// Create a top-level dialog shell.
	public IToolkitWindow CreateTopLevelDialog
				(int width, int height, bool modal, bool resizable,
				 IToolkitWindow dialogParent)
			{
				DrawingTopLevelWindow window;
				window = new DrawingTopLevelWindow
					(this, String.Empty, width, height);
				if(dialogParent is TopLevelWindow)
				{
					window.TransientFor = (TopLevelWindow)dialogParent;
				}
				if(modal)
				{
					window.InputType = MotifInputType.ApplicationModal;
				}
				else
				{
					window.InputType = MotifInputType.Modeless;
				}
				if(!resizable)
				{
					window.Decorations = MotifDecorations.Border |
										 MotifDecorations.Title |
										 MotifDecorations.Menu;
					window.Functions = MotifFunctions.Move |
									   MotifFunctions.Close;
				}
				return window;
			}

	// Create a top-level menu shell.
	public IToolkitWindow CreateTopLevelMenu
				(int x, int y, int width, int height)
			{
				// TODO
				return null;
			}

	// Create a child window.  If "parent" is null, then the child
	// does not yet have a "real" parent - it will be reparented later.
	public IToolkitWindow CreateChildWindow
				(IToolkitWindow parent, int x, int y, int width, int height)
			{
				Widget wparent;
				if(parent is Widget)
				{
					wparent = ((Widget)parent);
				}
				else
				{
					wparent = placeholder;
				}
				return new DrawingWindow(this, wparent, x, y, width, height);
			}

	// Map a System.Drawing color into an Xsharp color.
	public static Xsharp.Color DrawingToXColor(System.Drawing.Color color)
			{
				int argb = color.ToArgb();
				return new Xsharp.Color((argb >> 16) & 0xFF,
										(argb >> 8) & 0xFF, argb & 0xFF);
			}

}; // class DrawingToolkit

}; // namespace System.Drawing.Toolkit