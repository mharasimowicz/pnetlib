/*
 * IToolkitEventSink.cs - Implementation of the
 *			"System.Drawing.Toolkit.IToolkitEventSink" class.
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

// This interface is implemented by higher-level controls
// that need to receive events from an IToolkitWindow.

public interface IToolkitEventSink
{
	// Event that is emitted for an expose on this window.
	void ToolkitExpose(Graphics graphics);

	// Event that is emitted when the mouse enters this window.
	void ToolkitMouseEnter();

	// Event that is emitted when the mouse leaves this window.
	void ToolkitMouseLeave();

	// Event that is emitted when the focus enters this window.
	void ToolkitFocusEnter();

	// Event that is emitted when the focus leaves this window.
	void ToolkitFocusLeave();

	// Event that is emitted for a key down event.
	void ToolkitKeyDown(ToolkitKeys key);

	// Event that is emitted for a key up event.
	void ToolkitKeyUp(ToolkitKeys key);

	// Event that is emitted for a key character event.
	void ToolkitKeyChar(char charCode);

	// Event that is emitted for a mouse down event.
	void ToolkitMouseDown
		(ToolkitMouseButtons buttons, ToolkitKeys modifiers,
		 int clicks, int x, int y, int delta);

	// Event that is emitted for a mouse up event.
	void ToolkitMouseUp
		(ToolkitMouseButtons buttons, ToolkitKeys modifiers,
		 int clicks, int x, int y, int delta);

	// Event that is emitted for a mouse hover event.
	void ToolkitMouseHover
		(ToolkitMouseButtons buttons, ToolkitKeys modifiers,
		 int clicks, int x, int y, int delta);

	// Event that is emitted for a mouse move event.
	void ToolkitMouseMove
		(ToolkitMouseButtons buttons, ToolkitKeys modifiers,
		 int clicks, int x, int y, int delta);

	// Event that is emitted for a mouse wheel event.
	void ToolkitMouseWheel
		(ToolkitMouseButtons buttons, ToolkitKeys modifiers,
		 int clicks, int x, int y, int delta);

	// Event that is emitted when the window is moved by
	// external means (e.g. the user dragging the window).
	void ToolkitExternalMove(int x, int y);

	// Event that is emitted when the window is resized by
	// external means (e.g. the user resizing the window).
	void ToolkitExternalResize(int width, int height);

}; // interface IToolkitEventSink

}; // namespace System.Drawing.Toolkit