/*
 * BinaryValueWriter.cs - Implementation of the
 *	"System.Runtime.Serialization.Formatters.Binary.BinaryValueWriter" class.
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

namespace System.Runtime.Serialization.Formatters.Binary
{

#if CONFIG_REMOTING

using System.IO;
using System.Reflection;
using System.Collections;

internal abstract class BinaryValueWriter
{
	// Builtin writers.
	private static BinaryValueWriter booleanWriter = new BooleanWriter();
	private static BinaryValueWriter byteWriter = new ByteWriter();
	private static BinaryValueWriter sbyteWriter = new SByteWriter();
	private static BinaryValueWriter charWriter = new CharWriter();
	private static BinaryValueWriter int16Writer = new Int16Writer();
	private static BinaryValueWriter uint16Writer = new UInt16Writer();
	private static BinaryValueWriter int32Writer = new Int32Writer();
	private static BinaryValueWriter uint32Writer = new UInt32Writer();
	private static BinaryValueWriter int64Writer = new Int64Writer();
	private static BinaryValueWriter uint64Writer = new UInt64Writer();
	private static BinaryValueWriter singleWriter = new SingleWriter();
	private static BinaryValueWriter doubleWriter = new DoubleWriter();
	private static BinaryValueWriter decimalWriter = new DecimalWriter();
	private static BinaryValueWriter dateTimeWriter = new DateTimeWriter();
	private static BinaryValueWriter timeSpanWriter = new TimeSpanWriter();
	private static BinaryValueWriter stringWriter = new StringWriter();
	private static BinaryValueWriter objectWriter = new ObjectWriter();
#if false
	// TODO
	private static BinaryValueWriter arrayOfObjectWriter
			= new ArrayWriter(BinaryPrimitiveTypeCode.ArrayOfObject);
	private static BinaryValueWriter arrayOfStringWriter
			= new ArrayWriter(BinaryPrimitiveTypeCode.ArrayOfString);
#endif

	// Context information for writing binary values.
	public class BinaryValueContext
	{
		public BinaryFormatter formatter;
		public BinaryWriter writer;
		public ObjectIDGenerator gen;
		public Queue queue;
		public Queue assemblyQueue;

		// Constructor.
		public BinaryValueContext(BinaryFormatter formatter,
								  BinaryWriter writer)
				{
					this.formatter = formatter;
					this.writer = writer;
					this.gen = new ObjectIDGenerator();
					this.queue = new Queue();
					this.assemblyQueue = new Queue();
				}

	}; // class BinaryValueContext

	// Constructor.
	protected BinaryValueWriter() {}

	// Write the type tag for a type.
	public abstract void WriteTypeTag(BinaryValueContext context, Type type);

	// Write the type specification for a type.
	public abstract void WriteTypeSpec(BinaryValueContext context, Type type);

	// Write the inline form of values for a type.
	public abstract void WriteInline(BinaryValueContext context,
									 Object value, Type type,
									 Type fieldType);

	// Write the object header information for a type.
	public abstract void WriteObjectHeader(BinaryValueContext context,
										   Type type, long objectID,
										   long prevObject);

	// Write the object form of values for a type.
	public abstract void WriteObject(BinaryValueContext context,
									 Object value, Type type);

	// Get the primitive type code for a type.
	private static BinaryPrimitiveTypeCode GetPrimitiveTypeCode(Type type)
			{
				if(type == typeof(bool))
				{
					return BinaryPrimitiveTypeCode.Boolean;
				}
				else if(type == typeof(byte))
				{
					return BinaryPrimitiveTypeCode.Byte;
				}
				else if(type == typeof(sbyte))
				{
					return BinaryPrimitiveTypeCode.SByte;
				}
				else if(type == typeof(char))
				{
					return BinaryPrimitiveTypeCode.Char;
				}
				else if(type == typeof(short))
				{
					return BinaryPrimitiveTypeCode.Int16;
				}
				else if(type == typeof(ushort))
				{
					return BinaryPrimitiveTypeCode.UInt16;
				}
				else if(type == typeof(int))
				{
					return BinaryPrimitiveTypeCode.Int32;
				}
				else if(type == typeof(uint))
				{
					return BinaryPrimitiveTypeCode.UInt32;
				}
				else if(type == typeof(long))
				{
					return BinaryPrimitiveTypeCode.Int64;
				}
				else if(type == typeof(ulong))
				{
					return BinaryPrimitiveTypeCode.UInt64;
				}
				else if(type == typeof(float))
				{
					return BinaryPrimitiveTypeCode.Single;
				}
				else if(type == typeof(double))
				{
					return BinaryPrimitiveTypeCode.Double;
				}
				else if(type == typeof(Decimal))
				{
					return BinaryPrimitiveTypeCode.Decimal;
				}
				else if(type == typeof(DateTime))
				{
					return BinaryPrimitiveTypeCode.DateTime;
				}
				else if(type == typeof(TimeSpan))
				{
					return BinaryPrimitiveTypeCode.TimeSpan;
				}
				else if(type == typeof(String))
				{
					return BinaryPrimitiveTypeCode.String;
				}
				else
				{
					return (BinaryPrimitiveTypeCode)0;
				}
			}

	// Get the value writer for a particular type.
	public static BinaryValueWriter GetWriter
				(BinaryValueContext context, Type type)
			{
				BinaryPrimitiveTypeCode code;

				// Handle the primitive types first.
				code = GetPrimitiveTypeCode(type);
				switch(code)
				{
					case BinaryPrimitiveTypeCode.Boolean:
						return booleanWriter;
					case BinaryPrimitiveTypeCode.Byte:
						return byteWriter;
					case BinaryPrimitiveTypeCode.Char:
						return charWriter;
					case BinaryPrimitiveTypeCode.Decimal:
						return decimalWriter;
					case BinaryPrimitiveTypeCode.Double:
						return doubleWriter;
					case BinaryPrimitiveTypeCode.Int16:
						return int16Writer;
					case BinaryPrimitiveTypeCode.Int32:
						return int32Writer;
					case BinaryPrimitiveTypeCode.Int64:
						return int64Writer;
					case BinaryPrimitiveTypeCode.SByte:
						return sbyteWriter;
					case BinaryPrimitiveTypeCode.Single:
						return singleWriter;
					case BinaryPrimitiveTypeCode.TimeSpan:
						return timeSpanWriter;
					case BinaryPrimitiveTypeCode.DateTime:
						return dateTimeWriter;
					case BinaryPrimitiveTypeCode.UInt16:
						return uint16Writer;
					case BinaryPrimitiveTypeCode.UInt32:
						return uint32Writer;
					case BinaryPrimitiveTypeCode.UInt64:
						return uint64Writer;
					case BinaryPrimitiveTypeCode.String:
						return stringWriter;
				}

				// Handle special types that we recognize.
				if(type == typeof(Object))
				{
					return objectWriter;
				}
#if false
// TODO
				if(type == typeof(Object[]))
				{
					return arrayOfObjectWriter;
				}
				if(type == typeof(String[]))
				{
					return arrayOfStringWriter;
				}
				if(type.IsArray && type.GetArrayRank() == 1)
				{
					code = GetPrimitiveTypeCode(type.GetElementType());
					if(code != (BinaryPrimitiveTypeCode)0)
					{
						return new ArrayWriter
							(BinaryTypeTag.ArrayOfPrimitiveType, code);
					}
				}
#endif

				// Everything else is handled as an object.
				return objectWriter;
			}

	// Write an assembly name to an output stream.
	private static void WriteAssemblyName(BinaryValueContext context,
										  Assembly assembly)
			{
				String name = assembly.FullName;
				if(context.formatter.AssemblyFormat ==
						FormatterAssemblyStyle.Full)
				{
					context.writer.Write(name);
				}
				else
				{
					int index = name.IndexOf(',');
					if(index != -1)
					{
						context.writer.Write(name.Substring(0, index));
					}
					else
					{
						context.writer.Write(name);
					}
				}
			}

	// Write object values.
	private class ObjectWriter : BinaryValueWriter
	{
		// Constructor.
		public ObjectWriter() : base() {}

		// Write the type tag for a type.
		public override void WriteTypeTag
					(BinaryValueContext context, Type type)
				{
					if(type == typeof(Object))
					{
						context.writer.Write((byte)(BinaryTypeTag.ObjectType));
					}
					else if(type.Assembly == Assembly.GetExecutingAssembly())
					{
						context.writer.Write((byte)(BinaryTypeTag.RuntimeType));
					}
					else
					{
						context.writer.Write((byte)(BinaryTypeTag.GenericType));
					}
				}

		// Write the type specification for a type.
		public override void WriteTypeSpec
					(BinaryValueContext context, Type type)
				{
					if(type == typeof(Object))
					{
						// Nothing to do here.
					}
					else if(type.Assembly == Assembly.GetExecutingAssembly())
					{
						context.writer.Write(type.FullName);
					}
					else
					{
						context.writer.Write(type.FullName);
						// TODO: assembly ID
					}
				}

		// Write the inline form of values for a type.
		public override void WriteInline(BinaryValueContext context,
										 Object value, Type type,
										 Type fieldType)
				{
					BinaryPrimitiveTypeCode code;
					BinaryValueWriter vw;
					bool firstTime;
					long objectID;
					long typeID;

					if(value == null)
					{
						// Write a null value.
						context.writer.Write
							((byte)(BinaryElementType.NullValue));
						return;
					}
					else if(type.IsValueType)
					{
						if(fieldType.IsValueType)
						{
							// Expand the value instance inline.
							vw = GetWriter(context, type);
							typeID = context.gen.GetIDForType(type);
							objectID = context.gen.GetId(value, out firstTime);
							vw.WriteObjectHeader(context, type,
												 objectID, typeID);
							vw.WriteObject(context, value, type);
							return;
						}
						else if((code = GetPrimitiveTypeCode(type)) != 0)
						{
							// This is a boxed primitive value.
							context.writer.Write
								((byte)(BinaryElementType.
											BoxedPrimitiveTypeValue));
							vw = GetWriter(context, type);
							vw.WriteTypeSpec(context, type);
							vw.WriteInline(context, value, type, type);
							return;
						}
					}

					// Queue the object to be expanded later.
					objectID = context.gen.GetId(value, out firstTime);
					context.writer.Write
						((byte)(BinaryElementType.ObjectReference));
					context.writer.Write((int)objectID);
					if(firstTime)
					{
						context.queue.Enqueue(value);
					}
				}

		// Write the object header information for a type.
		public override void WriteObjectHeader(BinaryValueContext context,
											   Type type, long objectID,
											   long prevObject)
				{
					if(prevObject == -1)
					{
						// Write the full type information.
						long assemblyID;
						if(type.Assembly == Assembly.GetExecutingAssembly())
						{
							context.writer.Write
								((byte)(BinaryElementType.RuntimeObject));
							assemblyID = -1;
						}
						else
						{
							bool firstTime;
							assemblyID = context.gen.GetId
								(type.Assembly, out firstTime);
							if(firstTime)
							{
								context.writer.Write
									((byte)(BinaryElementType.Assembly));
								context.writer.Write((int)assemblyID);
								WriteAssemblyName(context, type.Assembly);
							}
							context.writer.Write
								((byte)(BinaryElementType.ExternalObject));
						}
						context.writer.Write((int)objectID);
						context.writer.Write(type.FullName);
						MemberInfo[] members =
							FormatterServices.GetSerializableMembers
								(type, context.formatter.Context);
						int index;
						Type fieldType;
						for(index = 0; index < members.Length; ++index)
						{
							context.writer.Write(members[index].Name);
						}
						for(index = 0; index < members.Length; ++index)
						{
							if(members[index] is FieldInfo)
							{
								fieldType = ((FieldInfo)(members[index]))
												.FieldType;
							}
							else
							{
								fieldType = ((PropertyInfo)(members[index]))
												.PropertyType;
							}
							GetWriter(context, fieldType).WriteTypeTag
								(context, fieldType);
						}
						for(index = 0; index < members.Length; ++index)
						{
							if(members[index] is FieldInfo)
							{
								fieldType = ((FieldInfo)(members[index]))
												.FieldType;
							}
							else
							{
								fieldType = ((PropertyInfo)(members[index]))
												.PropertyType;
							}
							GetWriter(context, fieldType).WriteTypeSpec
								(context, fieldType);
						}
						if(assemblyID != -1)
						{
							context.writer.Write((int)assemblyID);
						}
					}
					else
					{
						// Write a short header, referring to a previous
						// object's type information.
						context.writer.Write
							((byte)(BinaryElementType.RefTypeObject));
						context.writer.Write((int)objectID);
						context.writer.Write((int)prevObject);
					}
				}

		// Write the object form of values for a type.
		public override void WriteObject(BinaryValueContext context,
										 Object value, Type type)
				{
					// TODO: output the field value information
				}

	}; // class ObjectWriter

	// Write primitive values.
	private abstract class PrimitiveWriter : BinaryValueWriter
	{
		// Internal state.
		private BinaryPrimitiveTypeCode code;
		private String fieldName;

		// Constructor.
		public PrimitiveWriter(BinaryPrimitiveTypeCode code)
				{
					this.code = code;
					this.fieldName = "m_value";
				}
		public PrimitiveWriter(BinaryPrimitiveTypeCode code, String fieldName)
				{
					this.code = code;
					this.fieldName = fieldName;
				}

		// Write the type tag for a type.
		public override void WriteTypeTag
					(BinaryValueContext context, Type type)
				{
					context.writer.Write((byte)(BinaryTypeTag.PrimitiveType));
				}

		// Write the type specification for a type.
		public override void WriteTypeSpec
					(BinaryValueContext context, Type type)
				{
					context.writer.Write((byte)code);
				}

		// Write the object header information for a type.
		public override void WriteObjectHeader(BinaryValueContext context,
											   Type type, long objectID,
											   long prevObject)
				{
					if(prevObject == -1)
					{
						// Write the full type information.
						context.writer.Write
							((byte)(BinaryElementType.RuntimeObject));
						context.writer.Write((int)objectID);
						context.writer.Write(type.FullName);
						context.writer.Write((int)1);
						context.writer.Write(fieldName);
						WriteTypeTag(context, type);
						WriteTypeSpec(context, type);
					}
					else
					{
						// Write a short header, referring to a previous
						// object's type information.
						context.writer.Write
							((byte)(BinaryElementType.RefTypeObject));
						context.writer.Write((int)objectID);
						context.writer.Write((int)prevObject);
					}
				}

		// Write the object form of values for a type.
		public override void WriteObject(BinaryValueContext context,
										 Object value, Type type)
				{
					// The object field is just the primitive value itself.
					WriteInline(context, value, type, type);
				}

	}; // class PrimitiveWriter

	// Write boolean values.
	private class BooleanWriter : PrimitiveWriter
	{
		// Construtor.
		public BooleanWriter() : base(BinaryPrimitiveTypeCode.Boolean) {}

		// Write the inline form of values for a type.
		public override void WriteInline(BinaryValueContext context,
										 Object value, Type type,
										 Type fieldType)
				{
					context.writer.Write((bool)value);
				}

	}; // class BooleanWriter

	// Write byte values.
	private class ByteWriter : PrimitiveWriter
	{
		// Construtor.
		public ByteWriter() : base(BinaryPrimitiveTypeCode.Byte) {}

		// Write the inline form of values for a type.
		public override void WriteInline(BinaryValueContext context,
										 Object value, Type type,
										 Type fieldType)
				{
					context.writer.Write((byte)value);
				}

	}; // class ByteWriter

	// Write sbyte values.
	private class SByteWriter : PrimitiveWriter
	{
		// Construtor.
		public SByteWriter() : base(BinaryPrimitiveTypeCode.SByte) {}

		// Write the inline form of values for a type.
		public override void WriteInline(BinaryValueContext context,
										 Object value, Type type,
										 Type fieldType)
				{
					context.writer.Write((sbyte)value);
				}

	}; // class SByteWriter

	// Write char values.
	private class CharWriter : PrimitiveWriter
	{
		// Construtor.
		public CharWriter() : base(BinaryPrimitiveTypeCode.Char) {}

		// Write the inline form of values for a type.
		public override void WriteInline(BinaryValueContext context,
										 Object value, Type type,
										 Type fieldType)
				{
					context.writer.Write((char)value);
				}

	}; // class CharWriter

	// Write short values.
	private class Int16Writer : PrimitiveWriter
	{
		// Construtor.
		public Int16Writer() : base(BinaryPrimitiveTypeCode.Int16) {}

		// Write the inline form of values for a type.
		public override void WriteInline(BinaryValueContext context,
										 Object value, Type type,
										 Type fieldType)
				{
					context.writer.Write((short)value);
				}

	}; // class Int16Writer

	// Write ushort values.
	private class UInt16Writer : PrimitiveWriter
	{
		// Construtor.
		public UInt16Writer() : base(BinaryPrimitiveTypeCode.UInt16) {}

		// Write the inline form of values for a type.
		public override void WriteInline(BinaryValueContext context,
										 Object value, Type type,
										 Type fieldType)
				{
					context.writer.Write((ushort)value);
				}

	}; // class UInt16Writer

	// Write int values.
	private class Int32Writer : PrimitiveWriter
	{
		// Construtor.
		public Int32Writer() : base(BinaryPrimitiveTypeCode.Int32) {}

		// Write the inline form of values for a type.
		public override void WriteInline(BinaryValueContext context,
										 Object value, Type type,
										 Type fieldType)
				{
					context.writer.Write((int)value);
				}

	}; // class Int32Writer

	// Write uint values.
	private class UInt32Writer : PrimitiveWriter
	{
		// Construtor.
		public UInt32Writer() : base(BinaryPrimitiveTypeCode.UInt32) {}

		// Write the inline form of values for a type.
		public override void WriteInline(BinaryValueContext context,
										 Object value, Type type,
										 Type fieldType)
				{
					context.writer.Write((uint)value);
				}

	}; // class UInt32Writer

	// Write long values.
	private class Int64Writer : PrimitiveWriter
	{
		// Construtor.
		public Int64Writer() : base(BinaryPrimitiveTypeCode.Int64) {}

		// Write the inline form of values for a type.
		public override void WriteInline(BinaryValueContext context,
										 Object value, Type type,
										 Type fieldType)
				{
					context.writer.Write((long)value);
				}

	}; // class Int64Writer

	// Write ulong values.
	private class UInt64Writer : PrimitiveWriter
	{
		// Construtor.
		public UInt64Writer() : base(BinaryPrimitiveTypeCode.UInt64) {}

		// Write the inline form of values for a type.
		public override void WriteInline(BinaryValueContext context,
										 Object value, Type type,
										 Type fieldType)
				{
					context.writer.Write((ulong)value);
				}

	}; // class UInt64Writer

	// Write float values.
	private class SingleWriter : PrimitiveWriter
	{
		// Construtor.
		public SingleWriter() : base(BinaryPrimitiveTypeCode.Single) {}

		// Write the inline form of values for a type.
		public override void WriteInline(BinaryValueContext context,
										 Object value, Type type,
										 Type fieldType)
				{
					context.writer.Write((float)value);
				}

	}; // class SingleWriter

	// Write double values.
	private class DoubleWriter : PrimitiveWriter
	{
		// Construtor.
		public DoubleWriter() : base(BinaryPrimitiveTypeCode.Double) {}

		// Write the inline form of values for a type.
		public override void WriteInline(BinaryValueContext context,
										 Object value, Type type,
										 Type fieldType)
				{
					context.writer.Write((double)value);
				}

	}; // class DoubleWriter

	// Write Decimal values.
	private class DecimalWriter : PrimitiveWriter
	{
		// Construtor.
		public DecimalWriter() : base(BinaryPrimitiveTypeCode.Decimal) {}

		// Write the object header information for a type.
		public override void WriteObjectHeader(BinaryValueContext context,
											   Type type, long objectID,
											   long prevObject)
				{
					if(prevObject == -1)
					{
						// Write the full type information.
						context.writer.Write
							((byte)(BinaryElementType.RuntimeObject));
						context.writer.Write((int)objectID);
						context.writer.Write(type.FullName);
						context.writer.Write((int)4);
						context.writer.Write("flags");
						context.writer.Write("hi");
						context.writer.Write("lo");
						context.writer.Write("mid");
						int32Writer.WriteTypeTag(context, null);
						int32Writer.WriteTypeTag(context, null);
						int32Writer.WriteTypeTag(context, null);
						int32Writer.WriteTypeTag(context, null);
						int32Writer.WriteTypeSpec(context, null);
						int32Writer.WriteTypeSpec(context, null);
						int32Writer.WriteTypeSpec(context, null);
						int32Writer.WriteTypeSpec(context, null);
					}
					else
					{
						// Write a short header, referring to a previous
						// object's type information.
						context.writer.Write
							((byte)(BinaryElementType.RefTypeObject));
						context.writer.Write((int)objectID);
						context.writer.Write((int)prevObject);
					}
				}

		// Write the inline form of values for a type.
		public override void WriteInline(BinaryValueContext context,
										 Object value, Type type,
										 Type fieldType)
				{
					int[] bits = Decimal.GetBits((Decimal)value);
					context.writer.Write(bits[3]);
					context.writer.Write(bits[2]);
					context.writer.Write(bits[0]);
					context.writer.Write(bits[1]);
				}

	}; // class DecimalWriter

	// Write DateTime values.
	private class DateTimeWriter : PrimitiveWriter
	{
		// Construtor.
		public DateTimeWriter()
			: base(BinaryPrimitiveTypeCode.DateTime, "ticks") {}

		// Write the inline form of values for a type.
		public override void WriteInline(BinaryValueContext context,
										 Object value, Type type,
										 Type fieldType)
				{
					context.writer.Write(((DateTime)value).Ticks);
				}

	}; // class DateTimeWriter

	// Write TimeSpan values.
	private class TimeSpanWriter : PrimitiveWriter
	{
		// Construtor.
		public TimeSpanWriter()
			: base(BinaryPrimitiveTypeCode.TimeSpan, "_ticks") {}

		// Write the inline form of values for a type.
		public override void WriteInline(BinaryValueContext context,
										 Object value, Type type,
										 Type fieldType)
				{
					context.writer.Write(((TimeSpan)value).Ticks);
				}

	}; // class TimeSpanWriter

	// Write String values.
	private class StringWriter : BinaryValueWriter
	{
		// Construtor.
		public StringWriter() : base() {}

		// Write the type tag for a type.
		public override void WriteTypeTag
					(BinaryValueContext context, Type type)
				{
					context.writer.Write((byte)(BinaryTypeTag.String));
				}

		// Write the type specification for a type.
		public override void WriteTypeSpec
					(BinaryValueContext context, Type type)
				{
					// Nothing to do here.
				}

		// Write the inline form of values for a type.
		public override void WriteInline(BinaryValueContext context,
										 Object value, Type type,
										 Type fieldType)
				{
					context.writer.Write((String)value);
				}

		// Write the object header information for a type.
		public override void WriteObjectHeader(BinaryValueContext context,
											   Type type, long objectID,
											   long prevObject)
				{
					context.writer.Write((byte)(BinaryElementType.String));
				}

		// Write the object form of values for a type.
		public override void WriteObject(BinaryValueContext context,
										 Object value, Type type)
				{
					context.writer.Write((String)value);
				}

	}; // class StringWriter

}; // class BinaryValueWriter

#endif // CONFIG_REMOTING

}; // namespace System.Runtime.Serialization.Formatters.Binary