/*
 * ComponentChangingEventArgs.cs - Implementation of the
 *		"System.ComponentModel.Design.ComponentChangingEventArgs" class.
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

namespace System.ComponentModel.Design
{

#if CONFIG_COMPONENT_MODEL_DESIGN

using System.Runtime.InteropServices;

[ComVisible(true)]
public sealed class ComponentChangingEventArgs : EventArgs
{
	// Internal state.
	private Object component;
	private MemberDescriptor member;

	// Constructor.
	public ComponentChangingEventArgs
				(Object component, MemberDescriptor member)
			{
				this.component = component;
				this.member = member;
			}

	// Get this object's properties.
	public Object Component
			{
				get
				{
					return component;
				}
			}
	public MemberDescriptor Member
			{
				get
				{
					return member;
				}
			}

}; // class ComponentChangingEventArgs

#endif // CONFIG_COMPONENT_MODEL_DESIGN

}; // namespace System.ComponentModel.Design
