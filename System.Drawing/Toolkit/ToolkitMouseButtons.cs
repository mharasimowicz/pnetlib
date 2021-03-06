/*
 * ToolkitMouseButtons.cs - Implementation of the
 *			"System.Windows.Forms.ToolkitMouseButtons" class.
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

// Definition must match "System.Windows.Forms.MouseButtons".
[Flags]
[NonStandardExtra]
public enum ToolkitMouseButtons
{
	None		= 0x00000000,
	Left		= 0x00100000,
	Right		= 0x00200000,
#if !CONFIG_COMPACT_FORMS
	Middle		= 0x00400000,
	XButton1	= 0x00800000,
	XButton2	= 0x01000000,
#endif

}; // enum ToolkitMouseButtons

}; // namespace System.Drawing.Toolkit
