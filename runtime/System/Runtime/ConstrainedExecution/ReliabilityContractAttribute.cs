/*
 * ReliabilityContractAttribute.cs - Implementation of the
 *	"System.Runtime.ConstrainedExecution.ReliabilityContractAttribute" class.
 *
 * Copyright (C) 2004  Southern Storm Software, Pty Ltd.
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

namespace System.Runtime.ConstrainedExecution
{

using System.Runtime.InteropServices;

#if CONFIG_FRAMEWORK_2_0

#if !ECMA_COMPAT
[ComVisible(false)]
#endif
[AttributeUsage(AttributeTargets.Method |
				AttributeTargets.Constructor |
				AttributeTargets.Struct |
				AttributeTargets.Class |
				AttributeTargets.Interface |
				AttributeTargets.Assembly,
				Inherited=false)]
public sealed class ReliabilityContractAttribute : Attribute
{
	// Internal state.
	private Consistency consistencyGuarantee;
	private CER cer;

	// Constructor.
	public ReliabilityContractAttribute() {}
	public ReliabilityContractAttribute
				(Consistency consistencyGuarantee, CER cer)
			{
				this.consistencyGuarantee = consistencyGuarantee;
				this.cer = cer;
			}

	// Get or set this attribute's properties.
	public Consistency ConsistencyGuarantee
			{
				get
				{
					return consistencyGuarantee;
				}
				set
				{
					consistencyGuarantee = value;
				}
			}
	public CER CER
			{
				get
				{
					return cer;
				}
				set
				{
					cer = value;
				}
			}

}; // class ReliabilityContractAttribute

#endif // CONFIG_FRAMEWORK_2_0

}; // namespace System.Runtime.ConstrainedExecution