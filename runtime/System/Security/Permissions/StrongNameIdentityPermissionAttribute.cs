/*
 * StrongNameIdentityPermissionAttribute.cs - Implementation of the
 *  "System.Security.Permissions.StrongNameIdentityPermissionAttribute" class.
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

namespace System.Security.Permissions
{

#if CONFIG_POLICY_OBJECTS && CONFIG_PERMISSIONS && !ECMA_COMPAT

using System;
using System.Security;

[AttributeUsage(AttributeTargets.Assembly |
			 	AttributeTargets.Class |
			 	AttributeTargets.Struct |
			 	AttributeTargets.Constructor |
			 	AttributeTargets.Method,
			 	AllowMultiple=true, Inherited=false)]
public sealed class StrongNameIdentityPermissionAttribute
	: CodeAccessSecurityAttribute
{
	// Internal state.
	private String blob;
	private String name;
	private String version;

	// Constructors.
	public StrongNameIdentityPermissionAttribute(SecurityAction action)
			: base(action)
			{
				// Nothing to do here.
			}

	// Get or set the blob value.
	public String PublicKey
			{
				get
				{
					return blob;
				}
				set
				{
					blob = value;
				}
			}

	// Get or set the name value.
	public String Name
			{
				get
				{
					return name;
				}
				set
				{
					name = value;
				}
			}

	// Get or set the version value.
	public String Version
			{
				get
				{
					return version;
				}
				set
				{
					version = value;
				}
			}

	// Create a permission object that corresponds to this attribute.
	public override IPermission CreatePermission()
			{
				if(Unrestricted)
				{
					throw new ArgumentException(_("Arg_PermissionState"));
				}
				else if(blob == null && name == null && version == null)
				{
					return new StrongNameIdentityPermission
						(PermissionState.None);
				}
				else if(blob == null)
				{
					throw new ArgumentException(_("Arg_PublicKeyBlob"));
				}
				else
				{
					StrongNamePublicKeyBlob key;
					Version vers;
					key = new StrongNamePublicKeyBlob(blob);
					if(version != null && version != String.Empty)
					{
						vers = new Version(version);
					}
					else
					{
						vers = null;
					}
					return new StrongNameIdentityPermission(key, name, vers);
				}
			}

}; // class StrongNameIdentityPermissionAttribute

#endif // CONFIG_POLICY_OBJECTS && CONFIG_PERMISSIONS && !ECMA_COMPAT

}; // namespace System.Security.Permissions
