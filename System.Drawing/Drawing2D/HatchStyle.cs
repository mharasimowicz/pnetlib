/*
 * HatchStyle.cs - Implementation of the
 *			"System.Drawing.Drawing2D.HatchStyle" class.
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

namespace System.Drawing.Drawing2D
{

public enum HatchStyle
{
	Horizontal				= 0,
	Vertical				= 1,
	ForwardDiagonal			= 2,
	BackwardDiagonal		= 3,
	Cross					= 4,
	LargeGrid				= Cross,
	DiagonalCross			= 5,
	Percent05				= 6,
	Percent10				= 7,
	Percent20				= 8,
	Percent25				= 9,
	Percent30				= 10,
	Percent40				= 11,
	Percent50				= 12,
	Percent60				= 13,
	Percent70				= 14,
	Percent75				= 15,
	Percent80				= 16,
	Percent90				= 17,
	LightDownwardDiagonal	= 18,
	LightUpwardDiagonal		= 19,
	DarkDownwardDiagonal	= 20,
	DarkUpwardDiagonal		= 21,
	WideDownwardDiagonal	= 22,
	WideUpwardDiagonal		= 23,
	LightVertical			= 24,
	LightHorizontal			= 25,
	NarrowVertical			= 26,
	NarrowHorizontal		= 27,
	DarkVertical			= 28,
	DarkHorizontal			= 29,
	DashedDownwardDiagonal	= 30,
	DashedUpwardDiagonal	= 31,
	DashedHorizontal		= 32,
	DashedVertical			= 33,
	SmallConfetti			= 34,
	LargeConfetti			= 35,
	ZigZag					= 36,
	Wave					= 37,
	DiagonalBrick			= 38,
	HorizontalBrick			= 39,
	Weave					= 40,
	Plaid					= 41,
	Divot					= 42,
	DottedGrid				= 43,
	DottedDiamond			= 44,
	Shingle					= 45,
	Trellis					= 46,
	Sphere					= 47,
	SmallGrid				= 48,
	SmallCheckerBoard		= 49,
	LargeCheckerBoard		= 50,
	OutlinedDiamond			= 51,
	SolidDiamond			= 52,
	Min						= Horizontal,
	Max						= Cross

}; // enum HatchStyle

}; // namespace System.Drawing.Drawing2D