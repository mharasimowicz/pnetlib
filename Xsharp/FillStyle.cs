/*
 * FillStyle.cs - GC area fill styles.
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
/// <para>The <see cref="T:Xsharp.FillStyle"/> enumeration specifies
/// the area fill style for graphics objects
/// (<see cref="T:Xsharp.Graphics"/>).
/// </para>
/// </summary>
public enum FillStyle
{

	FillSolid          = 0,
	FillTiled          = 1,
	FillStippled       = 2,
	FillOpaqueStippled = 3

} // enum FillStyle

} // namespace Xsharp
