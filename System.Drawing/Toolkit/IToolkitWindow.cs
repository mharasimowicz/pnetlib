/*
 * IToolkitWindow.cs - Implementation of the
 *			"System.Drawing.Toolkit.IToolkitWindow" class.
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

// This interface provides a primitive windowing facility for
// managing rectangular regions in a stack.  It can be used to
// create owner-draw widgets and the like.

public interface IToolkitWindow
{
	// Get the toolkit that owns this window.
	IToolkit Toolkit { get; }

	// Get the current dimensions of this window.
	Rectangle Dimensions { get; }

	// Get or set the mapped state of the window.
	bool IsMapped { get; set; }

	// Destroy this window and all of its children.
	void Destroy();

	// Move or resize this window.
	void MoveResize(int x, int y, int width, int height);

	// Raise this window respective to its siblings.
	void Raise();

	// Lower this window respective to its siblings.
	void Lower();

	// Iconify the window (top-level windows only).
	void Iconify();

	// Reparent this window to underneath a new parent.
	void Reparent(IToolkitWindow parent, int x, int y);

	// Get a toolkit graphics object for this window.
	IToolkitGraphics GetGraphics();

	// Set the window title (top-level windows only).
	void SetTitle(String title);

	// Set the background of the window to a solid color.
	void SetBackground(Color color);

	// Change the set of supported decorations and functions.
	void SetFunctions(ToolkitDecorations decorations,
					  ToolkitFunctions functions);

	// Event that is emitted for an expose on this window.
	event ToolkitExposeHandler Expose;

}; // interface IToolkitWindow

}; // namespace System.Drawing.Toolkit