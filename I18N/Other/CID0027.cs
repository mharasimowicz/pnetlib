/*
 * CID0027.cs - lt culture handler.
 *
 * Copyright (c) 2003  Southern Storm Software, Pty Ltd
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

// Generated from "lt.txt".

namespace I18N.Other
{

using System;
using System.Globalization;
using I18N.Common;

public class CID0027 : RootCulture
{
	private CultureName cultureName;

	public CID0027()
		: base(0x0027, CultureNameTable.GetNameInfoByID(0x0027)) {}
	public CID0027(int culture, CultureName cultureName)
		: base(culture, cultureName) {}

	public override DateTimeFormatInfo DateTimeFormat
	{
		get
		{
			DateTimeFormatInfo dfi = base.DateTimeFormat;
			dfi.AbbreviatedDayNames = new String[] {"Sk", "Pr", "An", "Tr", "Kt", "Pn", "\u0160t"};
			dfi.DayNames = new String[] {"Sekmadienis", "Pirmadienis", "Antradienis", "Tre\u010Diadienis", "Ketvirtadienis", "Penktadienis", "\u0160e\u0161tadienis"};
			dfi.AbbreviatedMonthNames = new String[] {"Sau", "Vas", "Kov", "Bal", "Geg", "Bir", "Lie", "Rgp", "Rgs", "Spa", "Lap", "Grd", ""};
			dfi.MonthNames = new String[] {"Sausio", "Vasario", "Kovo", "Baland\u017Eio", "Gegu\u017E\u0117s", "Bir\u017Eelio", "Liepos", "Rugpj\u016B\u010Dio", "Rugs\u0117jo", "Spalio", "Lapkri\u010Dio", "Gruod\u017Eio", ""};
			dfi.DateSeparator = ".";
			dfi.TimeSeparator = ".";
			dfi.LongDatePattern = "dddd, yyyy, MMMM d";
			dfi.LongTimePattern = "HH.mm.ss z";
			dfi.ShortDatePattern = "yy.M.d";
			dfi.ShortTimePattern = "HH.mm";
			dfi.FullDateTimePattern = "dddd, yyyy, MMMM d HH.mm.ss z";
#if !ECMA_COMPAT
			dfi.I18NSetDateTimePatterns(new String[] {
				"d:yy.M.d",
				"D:dddd, yyyy, MMMM d",
				"f:dddd, yyyy, MMMM d HH.mm.ss z",
				"f:dddd, yyyy, MMMM d HH.mm.ss z",
				"f:dddd, yyyy, MMMM d HH.mm.ss",
				"f:dddd, yyyy, MMMM d HH.mm",
				"F:dddd, yyyy, MMMM d HH.mm.ss",
				"g:yy.M.d HH.mm.ss z",
				"g:yy.M.d HH.mm.ss z",
				"g:yy.M.d HH.mm.ss",
				"g:yy.M.d HH.mm",
				"G:yy.M.d HH.mm.ss",
				"m:MMMM dd",
				"M:MMMM dd",
				"r:ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
				"R:ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
				"s:yyyy'-'MM'-'dd'T'HH':'mm':'ss",
				"t:HH.mm.ss z",
				"t:HH.mm.ss z",
				"t:HH.mm.ss",
				"t:HH.mm",
				"T:HH.mm.ss",
				"u:yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
				"U:dddd, dd MMMM yyyy HH:mm:ss",
				"y:yyyy MMMM",
				"Y:yyyy MMMM",
			});
#endif // !ECMA_COMPAT
			return dfi;
		}
		set
		{
			base.DateTimeFormat = value; // not used
		}
	}

	public override NumberFormatInfo NumberFormat
	{
		get
		{
			NumberFormatInfo nfi = base.NumberFormat;
			nfi.CurrencyDecimalSeparator = ",";
			nfi.CurrencyGroupSeparator = ".";
			nfi.NumberGroupSeparator = ".";
			nfi.PercentGroupSeparator = ".";
			nfi.NegativeSign = "-";
			nfi.NumberDecimalSeparator = ",";
			nfi.PercentDecimalSeparator = ",";
			nfi.PercentSymbol = "%";
			nfi.PerMilleSymbol = "\u2030";
			return nfi;
		}
		set
		{
			base.NumberFormat = value; // not used
		}
	}

}; // class CID0027

public class CNlt : CID0027
{
	public CNlt() : base() {}

}; // class CNlt

}; // namespace I18N.Other