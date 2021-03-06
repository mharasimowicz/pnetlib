/*
 * ClrSecurity.cs - Implementation of the
 *		"System.Security.ClrSecurity" class.
 *
 * Copyright (C) 2001  Southern Storm Software, Pty Ltd.
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

namespace System.Security
{

using System;
using System.Runtime.CompilerServices;
using System.Security.Permissions;

internal sealed class ClrSecurity
{

	// Get the permission sets in the current call context, starting
	// at a particular stack frame.  This will scan up the stack until
	// it finds a permissions object.  If none are found, this will
	// return "null".
	[MethodImpl(MethodImplOptions.InternalCall)]
	extern public static ClrPermissions GetPermissionsFrom(int skipFrames);

	// Get the permission set for a particular stack frame.
	// Returns null if no permission set for that stack frame.
	[MethodImpl(MethodImplOptions.InternalCall)]
	extern public static ClrPermissions GetPermissions(int skipFrames);

	// Set the permission sets for a particular stack frame.
	[MethodImpl(MethodImplOptions.InternalCall)]
	extern public static void SetPermissions
			(ClrPermissions perm, int skipFrames);

}; // class ClrSecurity

}; // namespace System.Security
