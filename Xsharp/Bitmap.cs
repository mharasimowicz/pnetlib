/*
 * Bitmap.cs - Basic bitmap handling for X applications.
 *
 * Copyright (C) 2002, 2003  Southern Storm Software, Pty Ltd.
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

/// <summary>
/// <para>The <see cref="T:Xsharp.Bitmap"/> class manages off-screen
/// bitmaps on an X display screen.</para>
/// </summary>
public sealed class Bitmap : Drawable
{
	/// <summary>
	/// <para>Constructs a new <see cref="T:Xsharp.Bitmap"/> instance
	/// that represents an off-screen bitmap.  The bitmap is created
	/// on the default screen of the primary display.</para>
	/// </summary>
	///
	/// <param name="width">
	/// <para>The width of the new bitmap.</para>
	/// </param>
	///
	/// <param name="height">
	/// <para>The height of the new bitmap.</para>
	/// </param>
	///
	/// <exception cref="T:Xsharp.XException">
	/// <para>The <paramref name="width"/> or <paramref name="height"/>
	/// values are out of range.</para>
	/// </exception>
	public Bitmap(int width, int height)
			: base(Xsharp.Application.Primary.Display,
				   Xsharp.Application.Primary.Display.DefaultScreenOfDisplay,
				   DrawableKind.Bitmap)
			{
				if(width < 1 || width > 32767 ||
				   height < 1 || height > 32767)
				{
					throw new XException(S._("X_InvalidBitmapSize"));
				}
				try
				{
					IntPtr display = dpy.Lock();
					SetPixmapHandle(Xlib.XCreatePixmap
						(display, (Xlib.Drawable)
							Xlib.XRootWindowOfScreen(screen.screen),
						 (uint)width, (uint)height, (uint)1));
				}
				finally
				{
					dpy.Unlock();
				}
			}

	/// <summary>
	/// <para>Constructs a new <see cref="T:Xsharp.Bitmap"/> instance
	/// that represents an off-screen bitmap.  The bitmap is created
	/// on the specified <paramref name="screen"/>.</para>
	/// </summary>
	///
	/// <param name="screen">
	/// <para>The screen upon which to create the new bitmap.</para>
	/// </param>
	///
	/// <param name="width">
	/// <para>The width of the new bitmap.</para>
	/// </param>
	///
	/// <param name="height">
	/// <para>The height of the new bitmap.</para>
	/// </param>
	///
	/// <exception cref="T:System.ArgumentNullException">
	/// <para>The <paramref name="screen"/> value is <see langword="null"/>.
	/// </para>
	/// </exception>
	///
	/// <exception cref="T:Xsharp.XException">
	/// <para>The <paramref name="width"/> or <paramref name="height"/>
	/// values are out of range.</para>
	/// </exception>
	public Bitmap(Screen screen, int width, int height)
			: base(GetDisplay(screen), screen, DrawableKind.Bitmap)
			{
				if(width < 1 || width > 32767 ||
				   height < 1 || height > 32767)
				{
					throw new XException(S._("X_InvalidBitmapSize"));
				}
				try
				{
					IntPtr display = dpy.Lock();
					SetPixmapHandle(Xlib.XCreatePixmap
						(display, (Xlib.Drawable)
							Xlib.XRootWindowOfScreen(screen.screen),
						 (uint)width, (uint)height, (uint)1));
				}
				finally
				{
					dpy.Unlock();
				}
			}

	// Get the display value from a specified screen, and check for null.
	private static Display GetDisplay(Screen screen)
			{
				if(screen == null)
				{
					throw new ArgumentNullException("screen");
				}
				return screen.DisplayOfScreen;
			}

	/// <summary>
	/// <para>Destroy this bitmap if it is currently active.</para>
	/// </summary>
	public override void Destroy()
			{
				try
				{
					IntPtr d = dpy.Lock();
					if(handle != Xlib.Drawable.Zero)
					{
						Xlib.XFreePixmap(d, (Xlib.Pixmap)handle);
						handle = Xlib.Drawable.Zero;
					}
				}
				finally
				{
					dpy.Unlock();
				}
			}

	// Convert a color into a pixel value, relative to this drawable.
	// Should be called with the display lock.
	internal override Xlib.Pixel ToPixel(Color color)
			{
				// Expand standard colors before conversion.
				if(color.Index != StandardColor.RGB)
				{
					color = screen.ToColor(color.Index);
				}

				// Convert the color into monochrome, where 1 is black
				// foreground and 0 is white background.
				if(((color.Red * 54 + color.Green * 183 +
				     color.Blue * 19) / 256) < 128)
				{
					return (Xlib.Pixel)1;
				}
				else
				{
					return (Xlib.Pixel)0;
				}
			}

} // class Bitmap

} // namespace Xsharp