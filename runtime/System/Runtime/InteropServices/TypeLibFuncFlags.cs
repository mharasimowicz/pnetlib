/*
 * TypeLibFuncFlags.cs - Implementation of the
 *			"System.Runtime.InteropServices.TypeLibFuncFlags" class.
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

namespace System.Runtime.InteropServices
{

#if CONFIG_COM_INTEROP

[Serializable]
[Flags]
public enum TypeLibFuncFlags
{
	FRestricted       = 0x0001,
	FSource           = 0x0002,
	FBindable         = 0x0004,
	FRequestEdit      = 0x0008,
	FDisplayBind      = 0x0010,
	FDefaultBind      = 0x0020,
	FHidden           = 0x0040,
	FUsesGetLastError = 0x0080,
	FDefaultCollelem  = 0x0100,
	FUiDefault        = 0x0200,
	FNonBrowsable     = 0x0400,
	FReplaceable      = 0x0800,
	FImmediateBind    = 0x1000

}; // enum TypeLibFuncFlags

#endif // CONFIG_COM_INTEROP

}; // namespace System.Runtime.InteropServices
