/*
 * UserPreferenceChangingEventArgs.cs - Implementation of the
 *		Microsoft.Win32.UserPreferenceChangingEventArgs class.
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

namespace Microsoft.Win32
{

#if CONFIG_WIN32_SPECIFICS

using System.Runtime.InteropServices;

public class UserPreferenceChangingEventArgs : EventArgs
{
	// Internal state.
	private UserPreferenceCategory category;

	// Constructor.
	public UserPreferenceChangingEventArgs(UserPreferenceCategory category)
			{
				this.category = category;
			}

	// Get the timer identifier.
	public UserPreferenceCategory Category
			{
				get
				{
					return category;
				}
			}

}; // class UserPreferenceChangingEventArgs

#endif // CONFIG_WIN32_SPECIFICS

}; // namespace Microsoft.Win32