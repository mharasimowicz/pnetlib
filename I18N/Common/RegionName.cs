/*
 * RegionName.cs - Implementation of the "I18N.Common.RegionName" class.
 *
 * Copyright (C) 2002  Southern Storm Software, Pty Ltd.
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

namespace I18N.Common
{

#if !ECMA_COMPAT

using System;

internal sealed class RegionName
{

	// Accessible internal state.
	public String name;
	public int	  regionID;
	public String twoLetterISOName;
	public String threeLetterISOName;
	public String threeLetterWindowsName;
	public String englishName;
	public bool   isMetric;
	public String currencySymbol;
	public String isoCurrencySymbol;

	// Construct a "RegionName" instance.
	public RegionName(String name, int regionID,
					  String twoLetterISOName, String threeLetterISOName,
					  String threeLetterWindowsName, String englishName,
					  bool isMetric, String currencySymbol,
					  String isoCurrencySymbol)
			{
				this.name                   = name;
				this.regionID               = regionID;
				this.twoLetterISOName       = twoLetterISOName;
				this.threeLetterISOName     = threeLetterISOName;
				this.threeLetterWindowsName = threeLetterWindowsName;
				this.englishName            = englishName;
				this.isMetric				= isMetric;
				this.currencySymbol			= currencySymbol;
				this.isoCurrencySymbol		= isoCurrencySymbol;
			}

}; // class RegionName

#endif // !ECMA_COMPAT

}; // namespace I18N.Common