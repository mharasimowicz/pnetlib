/*
 * IToolkit.cs - Implementation of the "System.Drawing.Toolkit.IToolkit" class.
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

using System.Drawing.Drawing2D;

public interface IToolkit
{
	// Run the main event processing loop for the toolkit.
	void Run();

	// Send a quit message to the toolkit, which should cause
	// it to exit from the "Run" method.
	void Quit();

	// Resolve a system color to an RGB value.  Returns -1 if the
	// system does not support the color and a default should be used.
	int ResolveSystemColor(KnownColor color);

	// Create an IToolkitGraphics object from a HDC.
	IToolkitGraphics CreateFromHdc(IntPtr hdc, IntPtr hdevice);

	// Create an IToolkitGraphics object from a HWND.
	IToolkitGraphics CreateFromHwnd(IntPtr hwnd);

	// Create a solid toolkit brush.
	IToolkitBrush CreateSolidBrush(Color color);

	// Create a hatched toolkit brush.
	IToolkitBrush CreateHatchBrush(HatchStyle style, Color foreColor,
								   Color backColor);

	// Create a linear gradient brush.  Returns null if the
	// toolkit does not support linear gradient brushes.
	IToolkitBrush CreateLinearGradientBrush
			(RectangleF rect, Color color1, Color color2,
			 LinearGradientMode mode);
	IToolkitBrush CreateLinearGradientBrush
			(RectangleF rect, Color color1, Color color2,
			 float angle, bool isAngleScaleable);

	// Create a toolkit pen from the properties in the specified object.
	// If the toolkit does not support the precise combination of pen
	// properties, it will return the closest matching pen.
	IToolkitPen CreatePen(Pen pen);

	// Create a toolkit font from the properties in the specified object.
	IToolkitFont CreateFont(Font font);

	// Get the handle for the halftone palette.  IntPtr.Zero if not supported.
	IntPtr GetHalftonePalette();

	// Create a top-level application window.
	IToolkitWindow CreateTopLevelWindow(int width, int height);

	// Create a top-level dialog shell.
	IToolkitWindow CreateTopLevelDialog
			(int width, int height, bool modal, bool resizable,
			 IToolkitWindow dialogParent);

	// Create a top-level menu shell.
	IToolkitWindow CreateTopLevelMenu(int x, int y, int width, int height);

	// Create a child window.  If "parent" is null, then the child
	// does not yet have a "real" parent - it will be reparented later.
	IToolkitWindow CreateChildWindow(IToolkitWindow parent,
									 int x, int y, int width, int height);

}; // interface IToolkit

}; // namespace System.Drawing.Toolkit