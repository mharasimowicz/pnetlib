/*
 * MarginsConverter.cs - Implementation of the
 *			"System.Drawing.Printing.MarginsConverter" class.
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

namespace System.Drawing.Printing
{

#if CONFIG_COMPONENT_MODEL

using System.ComponentModel;
using System.Globalization;
using System.Collections;

public class MarginsConverter : ExpandableObjectConverter
{
	// Constructor.
	public MarginsConverter() {}

	// Determine if we can convert from a given type to "Margins".
	public override bool CanConvertFrom
				(ITypeDescriptorContext context, Type sourceType)
			{
				if(sourceType == typeof(String))
				{
					return true;
				}
				else
				{
					return base.CanConvertFrom(context, sourceType);
				}
			}

	// Determine if we can convert to a given type from "Margins".
	[TODO]
	public override bool CanConvertTo
				(ITypeDescriptorContext context, Type destinationType)
			{
				// TODO
				return base.CanConvertTo(context, destinationType);
			}

	// Convert from a source type to "Margins".
	[TODO]
	public override Object ConvertFrom
				(ITypeDescriptorContext context,
				 CultureInfo culture, Object value)
			{
				// TODO
				return null;
			}

	// Convert from "Margins" to a destination type.
	[TODO]
	public override Object ConvertTo
				(ITypeDescriptorContext context,
				 CultureInfo culture, Object value,
				 Type destinationType)
			{
				// TODO
				return null;
			}

	// Create an object instance given a set of property values.
	[TODO]
	public override Object CreateInstance
				(ITypeDescriptorContext context, IDictionary propertyValues)
			{
				// TODO
				return null;
			}

	// Determine if "CreateInstance" is supported.
	public override bool GetCreateInstanceSupported
				(ITypeDescriptorContext context)
			{
				return true;
			}

}; // class MarginsConverter

#endif // CONFIG_COMPONENT_MODEL

}; // namespace System.Drawing.Printing
