/*
 * XmlTokenizedType.cs - Implementation of the
 *		"System.Xml.XmlTokenizedType" class.
 *
 * Copyright (C) 2002 Southern Storm Software, Pty Ltd.
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
 
namespace System.Xml
{

#if ECMA_COMPAT
internal
#else
public
#endif
enum XmlTokenizedType
{
	CDATA       = 0,
	ID          = 1,
	IDREF       = 2,
	IDREFS      = 3,
	ENTITY      = 4,
	ENTITIES    = 5,
	NMTOKEN     = 6,
	NMTOKENS    = 7,
	NOTATION    = 8,
	ENUMERATION = 9,
	QName       = 10,
	NCName      = 11,
	None        = 12

}; // enum XmlTokenizedType

}; // namespace System.Xml
