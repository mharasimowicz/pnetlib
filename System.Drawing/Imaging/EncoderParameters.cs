/*
 * EncoderParameters.cs - Implementation of the
 *			"System.Drawing.Imaging.EncoderParameters" class.
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

namespace System.Drawing.Imaging
{

public sealed class EncoderParameters : IDisposable
{
	// Internal state.
	private EncoderParameter[] parameters;

	// Constructors.
	public EncoderParameters()
			{
				parameters = new EncoderParameter [1];
			}
	public EncoderParameters(int count)
			{
				parameters = new EncoderParameter [count];
			}

	// Get or set the parameter array.
	public EncoderParameter[] Param
			{
				get
				{
					return parameters;
				}
				set
				{
					parameters = value;
				}
			}

	// Dispose this object.
	public void Dispose()
			{
				parameters = null;
				GC.SuppressFinalize(this);
			}

}; // class EncoderParameters

}; // namespace System.Drawing.Imaging