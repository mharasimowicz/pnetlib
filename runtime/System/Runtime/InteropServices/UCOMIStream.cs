/*
 * UCOMIStream.cs - Implementation of the
 *			"System.Runtime.InteropServices.UCOMIStream" class.
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

namespace System.Runtime.InteropServices
{

#if !ECMA_COMPAT

[Guid("")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface UCOMIStream
{
	void Clone(out UCOMIStream ppstm);
	void Commit(int grfCommitFlags);
	void CopyTo(UCOMIStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);
	void LockRegion(long libOffset, long cb, int dwLockType);
	void Read(byte[] pv, int cb, IntPtr pcbRead);
	void Revert();
	void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition);
	void SetSize(long libNewSize);
	void Stat(out STATSTG pstatstg, int grfStatFlag);
	void UnlockRegion(long libOffset, long cb, int dwLockType);
	void Write(byte[] pv, int cb, IntPtr pcbWritten);

}; // class UCOMIStream

#endif // !ECMA_COMPAT

}; // namespace System.Runtime.InteropServices