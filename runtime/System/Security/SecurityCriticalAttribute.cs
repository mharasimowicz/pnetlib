/*
 * SecurityCriticalAttribute.cs - Implementation of the
 *					"System.Security.SecurityCriticalAttribute" class.
 *
 * Copyright (C) 2009  Free Software Foundation Inc.
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

#if !ECMA_COMPAT && CONFIG_FRAMEWORK_2_0 && !CONFIG_COMPACT_FRAMEWORK

[AttributeUsage(AttributeTargets.Assembly |
				AttributeTargets.Module |
				AttributeTargets.Class |
				AttributeTargets.Struct |
				AttributeTargets.Enum |
				AttributeTargets.Constructor |
				AttributeTargets.Method |
				AttributeTargets.Property |
				AttributeTargets.Field |
				AttributeTargets.Event |
				AttributeTargets.Interface |
				AttributeTargets.Delegate,
				AllowMultiple = false, Inherited = false)]
public sealed class SecurityCriticalAttribute : Attribute
{

	SecurityCriticalScope scope;

	public SecurityCriticalAttribute()
	{
	}

	public SecurityCriticalAttribute(SecurityCriticalScope scope)
	{
		this.scope = scope;
	}

	public SecurityCriticalScope Scope
	{
		get
		{
			return scope;
		}
	}

}; // class SecurityCriticalAttribute

#endif // !ECMA_COMPAT && CONFIG_FRAMEWORK_2_0 && !CONFIG_COMPACT_FRAMEWORK

}; // namespace System.Security