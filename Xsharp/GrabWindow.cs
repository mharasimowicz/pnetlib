/*
 * GrabWindow.cs - Window that captures all events during an active grab.
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

namespace Xsharp
{

using System;
using System.Runtime.InteropServices;
using Xsharp.Events;

// When a "PopupWindow" instance is displayed on-screen, all mouse
// and keyboard events are routed to the grab window.  From there,
// they are dispatched to the approriate popup window.  The grab
// ends when the last popup window is unmapped.

internal class GrabWindow : OverrideWindow
{
	// Internal state.
	private PopupWindow[] list;
	private PopupWindow lastEntered;
	private PopupWindow lastButton;
	private IntPtr keyBuffer;

	// Constructor.
	public GrabWindow(Widget parent)
			: base(parent, -1, -1, 1, 1)
			{
				// Create the initial list of popup windows.
				list = new PopupWindow [0];

				// The grab window is always mapped, but just off-screen
				// so that it isn't visible to the user.
				Map();
			}

	// Destructor.
	~GrabWindow()
			{
				if(keyBuffer != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(keyBuffer);
					keyBuffer = IntPtr.Zero;
				}
			}

	// Grab control of the mouse and keyboard to manage popups.
	private void Grab()
			{
				try
				{
					IntPtr display = dpy.Lock();
					Xlib.Window handle = GetWidgetHandle();
					Xlib.XGrabKeyboard
						(display, handle, Xlib.Bool.False,
						 1 /* GrabModeAsync */, 1 /* GrabModeAsync */,
						 dpy.knownEventTime);
					Xlib.XGrabPointer
						(display, handle, Xlib.Bool.False,
						 (uint)(EventMask.ButtonPressMask |
						 		EventMask.ButtonReleaseMask |
								EventMask.PointerMotionMask),
						 1 /* GrabModeAsync */, 1 /* GrabModeAsync */,
						 Xlib.Window.Zero, Xlib.Cursor.Zero,
						 dpy.knownEventTime);
					Xlib.XFlush(display);
				}
				finally
				{
					dpy.Unlock();
				}
			}

	// Ungrab the mouse and keyboard because there are no more popups.
	private void Ungrab()
			{
				try
				{
					IntPtr display = dpy.Lock();
					Xlib.XUngrabPointer(display, dpy.knownEventTime);
					Xlib.XUngrabKeyboard(display, dpy.knownEventTime);
					Xlib.XFlush(display);
				}
				finally
				{
					dpy.Unlock();
				}
			}

	// Add a popup to the top of the mapped list.
	public void AddPopup(PopupWindow popup)
			{
				int index;
				lock(this)
				{
					// See if the popup is already in the list.
					for(index = 0; index < list.Length; ++index)
					{
						if(list[index] == popup)
						{
							while(index < (list.Length - 1))
							{
								list[index] = list[index + 1];
								++index;
							}
							list[index] = popup;
							return;
						}
					}

					// Re-allocate the list and add the new item.
					PopupWindow[] newList = new PopupWindow [list.Length + 1];
					Array.Copy(list, 0, newList, 0, list.Length);
					newList[list.Length] = popup;
					list = newList;

					// If the list now contains one item, then grab.
					if(list.Length == 1)
					{
						Grab();
					}
				}
			}

	// Remove a popup from the mapped list.
	public void RemovePopup(PopupWindow popup)
			{
				int index;
				lock(this)
				{
					for(index = list.Length - 1; index >= 0; --index)
					{
						if(list[index] == popup)
						{
							// Remove the item from the list.
							PopupWindow[] newList;
							newList = new PopupWindow [list.Length - 1];
							Array.Copy(list, 0, newList, 0, index);
							Array.Copy(list, index + 1, newList, index,
									   list.Length - index - 1);
							list = newList;

							// If this was the entered window, then
							// send it a fake "LeaveNotify" event.
							if(lastEntered == popup)
							{
								lastEntered = null;
								FakeLeave(popup);
							}

							// If this was the button window, then clear it.
							if(lastButton == popup)
							{
								lastButton = null;
							}

							// If the list is now empty, then ungrab.
							if(list.Length == 0)
							{
								Ungrab();
							}
							return;
						}
					}
				}
			}

	// Lower a popup to the bottom of the mapped list.
	public void LowerPopup(PopupWindow popup)
			{
				int index;
				lock(this)
				{
					for(index = list.Length - 1; index >= 0; --index)
					{
						if(list[index] == popup)
						{
							while(index > 0)
							{
								list[index] = list[index - 1];
								--index;
							}
							list[0] = popup;
							return;
						}
					}
				}
			}

	// Send a fake "EnterNotify" event to a popup window.
	private static void FakeEnter(PopupWindow popup)
			{
				if(popup != null)
				{
					XEvent xevent = new XEvent();
					xevent.xany.type__ =
						(Xlib.Xint)(int)(EventType.EnterNotify);
					popup.DispatchEvent(ref xevent);
				}
			}

	// Send a fake "LeaveNotify" event to a popup window.
	private static void FakeLeave(PopupWindow popup)
			{
				if(popup != null)
				{
					XEvent xevent = new XEvent();
					xevent.xany.type__ =
						(Xlib.Xint)(int)(EventType.LeaveNotify);
					popup.DispatchEvent(ref xevent);
				}
			}

	// Change the "lastEntered" window.
	private void ChangeEntered(PopupWindow popup)
			{
				PopupWindow before = null;
				PopupWindow after = null;
				lock(this)
				{
					if(lastEntered != popup)
					{
						before = lastEntered;
						after = popup;
						lastEntered = popup;
					}
				}
				if(before != null)
				{
					FakeLeave(before);
				}
				if(after != null)
				{
					FakeEnter(after);
				}
			}

	// Find the popup window that contains a particular mouse position.
	private PopupWindow Find(int x, int y, bool defaultIsTop)
			{
				int index;
				PopupWindow popup;
				for(index = 0; index < list.Length; ++index)
				{
					popup = list[index];
					if(x >= popup.x && x < (popup.x + popup.width) &&
					   y >= popup.y && y < (popup.y + popup.height))
					{
						return popup;
					}
				}
				if(defaultIsTop && list.Length > 0)
				{
					return list[list.Length - 1];
				}
				else
				{
					return null;
				}
			}

	// Dispatch an event to this widget.
	internal override void DispatchEvent(ref XEvent xevent)
			{
				Xlib.KeySym keysym;
				PopupWindow popup;

				switch(xevent.type)
				{
					case EventType.ButtonPress:
					{
						// A mouse button was pressed during the grab.
						lock(this)
						{
							if(lastButton != null)
							{
								// We currently have a button window, so
								// all mouse events should go to it.
								popup = lastButton;
							}
							else
							{
								// Determine which popup contains the mouse.
								// If nothing contains, then use the top.
								popup = Find(xevent.xbutton.x_root,
											 xevent.xbutton.y_root, true);
							}
							lastButton = popup;
						}
						ChangeEntered(popup);
						if(popup != null)
						{
							// Adjust the co-ordinates and re-dispatch.
							xevent.xbutton.x__ =
								(Xlib.Xint)(xevent.xbutton.x_root - popup.x);
							xevent.xbutton.y__ = 
								(Xlib.Xint)(xevent.xbutton.y_root - popup.y);
							popup.DispatchEvent(ref xevent);
						}
					}
					break;

					case EventType.ButtonRelease:
					{
						// A mouse button was released during the grab.
						lock(this)
						{
							popup = lastButton;
							if(popup != null)
							{
								// Reset "lastButton" if this is the last
								// button to be released.
								ModifierMask mask = ModifierMask.AllButtons;
								mask &= ~(ModifierMask.Button1Mask <<
									(((int)(xevent.xbutton.button__)) - 1));
								if((xevent.xbutton.state & mask) == 0)
								{
									lastButton = null;
								}
							}
						}
						ChangeEntered(popup);
						if(popup != null)
						{
							// Adjust the co-ordinates and re-dispatch.
							xevent.xbutton.x__ =
								(Xlib.Xint)(xevent.xbutton.x_root - popup.x);
							xevent.xbutton.y__ = 
								(Xlib.Xint)(xevent.xbutton.y_root - popup.y);
							popup.DispatchEvent(ref xevent);
						}
					}
					break;

					case EventType.MotionNotify:
					{
						// The mouse pointer was moved during the grab.
						lock(this)
						{
							// If there is a last button window, then use
							// that, otherwise find the one under the mouse.
							popup = lastButton;
							if(popup == null)
							{
								popup = Find(xevent.xmotion.x_root,
											 xevent.xmotion.y_root, false);
							}
						}
						ChangeEntered(popup);
						if(popup != null)
						{
							// Adjust the co-ordinates and re-dispatch.
							xevent.xmotion.x__ =
								(Xlib.Xint)(xevent.xmotion.x_root - popup.x);
							xevent.xmotion.y__ = 
								(Xlib.Xint)(xevent.xmotion.y_root - popup.y);
							popup.DispatchEvent(ref xevent);
						}
					}
					break;

					case EventType.KeyPress:
					{
						// Convert the event into a symbol and a string.
						if(keyBuffer == IntPtr.Zero)
						{
							keyBuffer = Marshal.AllocHGlobal(32);
						}
						keysym = 0;
						int len = Xlib.XLookupString
							(ref xevent.xkey, keyBuffer, 32,
							 ref keysym, IntPtr.Zero);
						String str;
						if(len > 0)
						{
							str = Marshal.PtrToStringAnsi(keyBuffer, len);
						}
						else
						{
							str = null;
						}

						// Dispatch the event to the top-most popup.
						lock(this)
						{
							if(list.Length > 0)
							{
								popup = list[list.Length - 1];
							}
							else
							{
								popup = null;
							}
						}
						if(popup != null)
						{
							popup.DispatchKeyEvent
								((KeyName)keysym, xevent.xkey.state, str);
						}
					}
					break;

					case EventType.KeyRelease:
					{
						// Convert the event into a symbol and a string.
						if(keyBuffer == IntPtr.Zero)
						{
							keyBuffer = Marshal.AllocHGlobal(32);
						}
						keysym = 0;
						int len = Xlib.XLookupString
							(ref xevent.xkey, keyBuffer, 32,
							 ref keysym, IntPtr.Zero);

						// Dispatch the event to the top-most popup.
						lock(this)
						{
							if(list.Length > 0)
							{
								popup = list[list.Length - 1];
							}
							else
							{
								popup = null;
							}
						}
						if(popup != null)
						{
							popup.DispatchKeyReleaseEvent
								((KeyName)keysym, xevent.xkey.state);
						}
					}
					break;

					default:
					{
						// Everything else is handled normally.
						base.DispatchEvent(ref xevent);
					}
					break;
				}
			}

} // class GrabWindow

} // namespace Xsharp