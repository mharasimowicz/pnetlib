/*
 * EngineInstance.cs - Information that is specific to an engine instance.
 *
 * Copyright (C) 2003 Southern Storm Software, Pty Ltd.
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
 
namespace Microsoft.JScript
{

using System;
using System.IO;
using Microsoft.JScript.Vsa;

// This class exists to support multiple engine instances within a
// single application process.  It keeps track of objects that would
// otherwise be global, and hence a re-entrancy problem.

internal sealed class EngineInstance
{
	// Internal state.
	private VsaEngine engine;
	private ObjectPrototype objectPrototype;
	private ObjectConstructor objectConstructor;
	private FunctionPrototype functionPrototype;
	private FunctionConstructor functionConstructor;
	private ArrayPrototype arrayPrototype;
	private ArrayConstructor arrayConstructor;
	private TextWriter outStream;
	private TextWriter errorStream;

	// Constructor.
	public EngineInstance(VsaEngine engine)
			{
				// Initialize the local state.
				this.engine = engine;
				this.outStream = null;
				this.errorStream = null;
			}
	
	// Initialize the engine instance after construction.  Needed
	// to resolve circularity issues at startup time.
	public void Init()
			{
				// Initialize the basic "Object" and "Function" objects,
				// which must be created carefully to avoid circularities.
				objectPrototype = new LenientObjectPrototype();
				functionPrototype = new LenientFunctionPrototype();
				objectConstructor = new ObjectConstructor();
				functionConstructor = new FunctionConstructor();
				objectPrototype.Init(engine);
				functionPrototype.Init(engine, objectPrototype);
				objectConstructor.Init(engine, functionPrototype);
				functionConstructor.Init(engine, functionPrototype);
			}
	
	// Get the default engine instance.
	public static EngineInstance Default
			{
				get
				{
					return Globals.GetContextEngine().engineInstance;
				}
			}

	// Get the engine instance for a specific engine.
	public static EngineInstance GetEngineInstance(VsaEngine engine)
			{
				return engine.engineInstance;
			}

	// Get standard prototypes and constructors.
	public ObjectPrototype GetObjectPrototype()
			{
				return objectPrototype;
			}
	public ObjectConstructor GetObjectConstructor()
			{
				return objectConstructor;
			}
	public FunctionPrototype GetFunctionPrototype()
			{
				return functionPrototype;
			}
	public FunctionConstructor GetFunctionConstructor()
			{
				return functionConstructor;
			}
	public ArrayPrototype GetArrayPrototype()
			{
				lock(this)
				{
					if(arrayPrototype == null)
					{
						arrayPrototype = new LenientArrayPrototype
								(GetObjectPrototype(), this);
					}
					return arrayPrototype;
				}
			}
	public ArrayConstructor GetArrayConstructor()
			{
				lock(this)
				{
					if(arrayConstructor == null)
					{
						arrayConstructor = new ArrayConstructor
								(GetFunctionPrototype());
					}
					return arrayConstructor;
				}
			}

	// Get the output streams for this engine instance.
	public TextWriter Out
			{
				get
				{
					if(outStream != null)
					{
						return outStream;
					}
					else
					{
						return ScriptStream.Out;
					}
				}
			}
	public TextWriter Error
			{
				get
				{
					if(errorStream != null)
					{
						return errorStream;
					}
					else
					{
						return ScriptStream.Error;
					}
				}
			}

	// Set the output stream for this engine instance.
	public void SetOutputStreams(TextWriter output, TextWriter error)
			{
				outStream = output;
				errorStream = error;
			}

	// Detach the output streams from the global state.
	public void DetachOutputStreams()
			{
				if(outStream == null)
				{
					outStream = Console.Out;
				}
				if(errorStream == null)
				{
					errorStream = Console.Error;
				}
			}

}; // class EngineInstance

}; // namespace Microsoft.JScript