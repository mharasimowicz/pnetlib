/*
 * XmlAttributeAttribute.cs - Implementation of the
 *			"System.Xml.Serialization.XmlAttributeAttribute" class.
 *
 * Copyright (C) 2003  Free Software Foundation, Inc.
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

namespace System.Xml.Serialization
{

#if !ECMA_COMPAT

using System;
using System.Xml;
using System.Xml.Schema;

[AttributeUsage(AttributeTargets.Property |
                AttributeTargets.Field |
                AttributeTargets.Parameter |
                AttributeTargets.ReturnValue)]
public class XmlAttributeAttribute : Attribute
{
	// Internal state.
	private String attributeName;
	private String dataType;
	private XmlSchemaForm form;
	private String nspace;
	private Type type;

	// Constructors.
	public XmlAttributeAttribute()
			: this(null, null)
			{
			}
	public XmlAttributeAttribute(String attributeName)
			: this(attributeName, null)
			{
			}
	public XmlAttributeAttribute(Type type)
			: this(null, type)
			{
			}
	public XmlAttributeAttribute(String attributeName, Type type)
			: base()
			{
				if (attributeName == null) { attributeName = ""; }
				this.attributeName = attributeName;
				this.dataType = "";
				this.form = XmlSchemaForm.None;
				this.nspace = null;
				this.type = type;
			}

	// Properties.
	public String AttributeName
			{
				get { return attributeName; }
				set
				{
					if (value == null) { value = ""; }
					attributeName = value;
				}
			}
	public String DataType
			{
				get { return dataType; }
				set
				{
					if (value == null) { value = ""; }
					dataType = value;
				}
			}
	public XmlSchemaForm Form
			{
				get { return form; }
				set { form = value; }
			}
	public String Namespace
			{
				get { return nspace; }
				set { nspace = value; }
			}
	public Type Type
			{
				get { return type; }
				set { type = value; }
			}

}; // class XmlAttributeAttribute

#endif // !ECMA_COMPAT

}; // namespace System.Xml.Serialization