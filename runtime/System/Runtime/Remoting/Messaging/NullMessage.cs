/*
 * NullMessage.cs - Implementation of the
 *			"System.Runtime.Remoting.Messaging.NullMessage" class.
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

namespace System.Runtime.Remoting.Messaging
{

#if CONFIG_SERIALIZATION

using System.Collections;
using System.Reflection;

internal class NullMessage : IMessage, IMethodMessage, IMethodCallMessage
{
	// Constructor.
	public NullMessage() {}

	// Implement the IMessage interface.
	public IDictionary Properties
			{
				get
				{
					return null;
				}
			}

	// Implement the IMethodMessage interface.
	public int ArgCount
			{
				get
				{
					return 0;
				}
			}
	public Object[] Args
			{
				get
				{
					return null;
				}
			}
	public bool HasVarArgs
			{
				get
				{
					return false;
				}
			}
	public LogicalCallContext LogicalCallContext
			{
				get
				{
					return null;
				}
			}
	public MethodBase MethodBase
			{
				get
				{
					return null;
				}
			}
	public String MethodName
			{
				get
				{
					return "Unknown";
				}
			}
	public Object MethodSignature
			{
				get
				{
					return null;
				}
			}
	public String TypeName
			{
				get
				{
					return "Unknown";
				}
			}
	public String Uri
			{
				get
				{
					return "Exception";
				}
				set
				{
					// Nothing
				}
			}
	public Object GetArg(int argNum)
			{
				return null;
			}
	public String GetArgName(int index)
			{
				return null;
			}

	// Implement the IMethodCallMessage interface.
	public int InArgCount
			{
				get
				{
					return 0;
				}
			}
	public Object[] InArgs
			{
				get
				{
					return null;
				}
			}
	public Object GetInArg(int argNum)
			{
				return null;
			}
	public String GetInArgName(int index)
			{
				return "Unknown";
			}

}; // class NullMessage

#endif // CONFIG_SERIALIZATION

}; // namespace System.Runtime.Remoting.Messaging
