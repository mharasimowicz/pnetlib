/*
 * WatcherChangeTypes.cs - Implementation of the
 *		"System.IO.WatcherChangeTypes" class.
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

namespace System.IO
{

#if CONFIG_WIN32_SPECIFICS

[Flags]
public enum WatcherChangeTypes
{
	Created = 0x0001,
	Deleted = 0x0002,
	Changed = 0x0004,
	Renamed = 0x0008,
	All     = 0x000F

}; // enum WatcherChangeTypes

#endif // CONFIG_WIN32_SPECIFICS

}; // namespace System.IO