/*
 * Timer.cs - Timer handling for Xsharp.
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

namespace Xsharp
{

using System;

/// <summary>
/// <para>Instances of <see cref="T:Xsharp.Timer"/> are used to
/// implement timeout functionality on X displays.</para>
/// </summary>
///
/// <remarks>
/// <para>The delivery of timeout notifications may be delayed by
/// event processing, operating system housekeeping, or system load.
/// That is, they are not delivered in "real time".  The only
/// guarantee is that they will be delivered on or sometime after
/// the specified time.</para>
/// </remarks>
public sealed class Timer : IDisposable
{
	// Internal state.
	private Display dpy;
	private TimerCallback callback;
	private Object state;
	private DateTime nextDue;
	private int period;
	private bool onDisplayQueue;
	private bool stopped;
	private Timer next;
	private Timer prev;

	/// <summary>
	/// <para>Create a new timer.</para>
	/// </summary>
	///
	/// <param name="callback">
	/// <para>The delegate to invoke when the timer expires.</para>
	/// </param>
	///
	/// <param name="state">
	/// <para>The state information to pass to the callback.</para>
	/// </param>
	///
	/// <param name="dueTime">
	/// <para>The number of milliseconds until the timer expires
	/// for the first time.</para>
	/// </param>
	///
	/// <exception cref="T:System.ArgumentNullException">
	/// <para>The <paramref name="callback"/> parameter is
	/// <see langword="null"/>.</para>
	/// </exception>
	///
	/// <exception cref="T:System.ArgumentOutOfRangeException">
	/// <para>The <paramref name="dueTime"/> parameter is
	/// less than zero.</para>
	/// </exception>
	public Timer(TimerCallback callback, Object state, int dueTime)
			: this(null, callback, state, dueTime, -1) {}

	/// <summary>
	/// <para>Create a new timer.</para>
	/// </summary>
	///
	/// <param name="callback">
	/// <para>The delegate to invoke when the timer expires.</para>
	/// </param>
	///
	/// <param name="state">
	/// <para>The state information to pass to the callback.</para>
	/// </param>
	///
	/// <param name="dueTime">
	/// <para>The number of milliseconds until the timer expires
	/// for the first time.</para>
	/// </param>
	///
	/// <param name="period">
	/// <para>The number of milliseconds between timer expiries, or
	/// -1 to only expire once at <paramref name="dueTime"/>.</para>
	/// </param>
	///
	/// <exception cref="T:System.ArgumentNullException">
	/// <para>The <paramref name="callback"/> parameter is
	/// <see langword="null"/>.</para>
	/// </exception>
	///
	/// <exception cref="T:System.ArgumentOutOfRangeException">
	/// <para>The <paramref name="dueTime"/> parameter is
	/// less than zero.</para>
	/// </exception>
	public Timer(TimerCallback callback, Object state,
				 int dueTime, int period)
			: this(null, callback, state, dueTime, period) {}

	/// <summary>
	/// <para>Create a new timer.</para>
	/// </summary>
	///
	/// <param name="dpy">
	/// <para>The display to create the timer for, or <see langword="null"/>
	/// to use the application's primary display.</para>
	/// </param>
	///
	/// <param name="callback">
	/// <para>The delegate to invoke when the timer expires.</para>
	/// </param>
	///
	/// <param name="state">
	/// <para>The state information to pass to the callback.</para>
	/// </param>
	///
	/// <param name="dueTime">
	/// <para>The number of milliseconds until the timer expires
	/// for the first time.</para>
	/// </param>
	///
	/// <exception cref="T:System.ArgumentNullException">
	/// <para>The <paramref name="callback"/> parameter is
	/// <see langword="null"/>.</para>
	/// </exception>
	///
	/// <exception cref="T:System.ArgumentOutOfRangeException">
	/// <para>The <paramref name="dueTime"/> parameter is
	/// less than zero.</para>
	/// </exception>
	public Timer(Display dpy, TimerCallback callback,
				 Object state, int dueTime)
			: this(dpy, callback, state, dueTime, -1) {}

	/// <summary>
	/// <para>Create a new timer.</para>
	/// </summary>
	///
	/// <param name="dpy">
	/// <para>The display to create the timer for, or <see langword="null"/>
	/// to use the application's primary display.</para>
	/// </param>
	///
	/// <param name="callback">
	/// <para>The delegate to invoke when the timer expires.</para>
	/// </param>
	///
	/// <param name="state">
	/// <para>The state information to pass to the callback.</para>
	/// </param>
	///
	/// <param name="dueTime">
	/// <para>The number of milliseconds until the timer expires
	/// for the first time.</para>
	/// </param>
	///
	/// <param name="period">
	/// <para>The number of milliseconds between timer expiries, or
	/// -1 to only expire once at <paramref name="dueTime"/>.</para>
	/// </param>
	///
	/// <exception cref="T:System.ArgumentNullException">
	/// <para>The <paramref name="callback"/> parameter is
	/// <see langword="null"/>.</para>
	/// </exception>
	///
	/// <exception cref="T:System.ArgumentOutOfRangeException">
	/// <para>The <paramref name="dueTime"/> parameter is
	/// less than zero.</para>
	/// </exception>
	public Timer(Display dpy, TimerCallback callback,
				 Object state, int dueTime, int period)
			{
				if(callback == null)
				{
					throw new ArgumentNullException("callback");
				}
				if(dpy == null)
				{
					this.dpy = Application.Primary.Display;
				}
				else
				{
					this.dpy = dpy;
				}
				if(dueTime < 0)
				{
					throw new ArgumentOutOfRangeException
						("dueTime", S._("X_NonNegative"));
				}
				this.callback = callback;
				this.state = state;
				this.nextDue =
					DateTime.Now + new TimeSpan
						(dueTime * TimeSpan.TicksPerMillisecond);
				this.period = period;
				this.stopped = false;
				AddTimer();
			}

	/// <summary>
	/// <para>Change the parameters for this timer.</para>
	/// </summary>
	///
	/// <param name="dueTime">
	/// <para>The number of milliseconds until the timer expires
	/// for the first time.</para>
	/// </param>
	///
	/// <exception cref="T:System.ArgumentOutOfRangeException">
	/// <para>The <paramref name="dueTime"/> parameter is
	/// less than zero.</para>
	/// </exception>
	public void Change(int dueTime)
			{
				Change(dueTime, -1);
			}

	/// <summary>
	/// <para>Change the parameters for this timer.</para>
	/// </summary>
	///
	/// <param name="dueTime">
	/// <para>The number of milliseconds until the timer expires
	/// for the first time.</para>
	/// </param>
	///
	/// <param name="period">
	/// <para>The number of milliseconds between timer expiries, or
	/// -1 to only expire once at <paramref name="dueTime"/>.</para>
	/// </param>
	///
	/// <exception cref="T:System.ArgumentOutOfRangeException">
	/// <para>The <paramref name="dueTime"/> parameter is
	/// less than zero.</para>
	/// </exception>
	public void Change(int dueTime, int period)
			{
				if(dueTime < 0)
				{
					throw new ArgumentOutOfRangeException
						("dueTime", S._("X_NonNegative"));
				}
				RemoveTimer();
				this.nextDue =
					DateTime.Now + new TimeSpan
						(dueTime * TimeSpan.TicksPerMillisecond);
				this.period = period;
				this.stopped = false;
				AddTimer();
			}

	/// <summary>
	/// <para>Dispose of this timer, stopping any pending timeouts.</para>
	/// </summary>
	public void Dispose()
			{
				Stop();
			}

	/// <summary>
	/// <para>Stop this timer.  Another timeout can be started at
	/// some future point using <c>Change</c>.</para>
	/// </summary>
	public void Stop()
			{
				stopped = true;
				RemoveTimer();
			}

	// Add the timer to the display's timer queue.
	private void AddTimer()
			{
				lock(dpy)
				{
					if(!onDisplayQueue)
					{
						Timer current = dpy.timerQueue;
						Timer prev = null;
						while(current != null && current.nextDue <= nextDue)
						{
							prev = current;
							current = current.next;
						}
						this.next = current;
						this.prev = prev;
						if(current != null)
						{
							current.prev = this;
						}
						if(prev != null)
						{
							prev.next = this;
						}
						else
						{
							dpy.timerQueue = this;
						}
						onDisplayQueue = true;
					}
				}
			}

	// Remove the timer from the display's timer queue.
	private void RemoveTimer()
			{
				lock(dpy)
				{
					if(onDisplayQueue)
					{
						if(next != null)
						{
							next.prev = prev;
						}
						if(prev != null)
						{
							prev.next = next;
						}
						else
						{
							dpy.timerQueue = next;
						}
						onDisplayQueue = false;
						next = null;
						prev = null;
					}
				}
			}

	// Activate timers that have fired on a particular display.
	internal static void ActivateTimers(Display dpy)
			{
				// Bail out early if there are no timers, to avoid
				// calling "DateTime.Now" if we don't need to.
				lock(dpy)
				{
					if(dpy.timerQueue == null)
					{
						return;
					}
				}
				DateTime now = DateTime.Now;
				Timer timer;
				DateTime next;
				for(;;)
				{
					// Remove the first timer from the queue if
					// it has expired.  Bail out if it hasn't.
					lock(dpy)
					{
						timer = dpy.timerQueue;
						if(timer == null)
						{
							break;
						}
						else if(timer.nextDue <= now)
						{
							timer.RemoveTimer();
						}
						else
						{
							break;
						}
					}

					// Invoke the timer's callback delegate.
					timer.callback(timer.state);

					// Add the timer back onto the queue if necessary.
					if(!timer.stopped && !timer.onDisplayQueue)
					{
						if(timer.period < 0)
						{
							timer.stopped = true;
						}
						else
						{
							next = timer.nextDue +
								new TimeSpan(timer.period *
											 TimeSpan.TicksPerMillisecond);
							while(next <= now)
							{
								next += new TimeSpan
										(timer.period *
										 TimeSpan.TicksPerMillisecond);
							}
							timer.nextDue = next;
							timer.AddTimer();
						}
					}
				}
			}

	// Get the number of milliseconds until the next timeout.
	// Returns -1 if there are no active timers.
	internal static int GetNextTimeout(Display dpy)
			{
				lock(dpy)
				{
					if(dpy.timerQueue != null)
					{
						DateTime fireAt = dpy.timerQueue.nextDue;
						long diff = fireAt.Ticks - DateTime.Now.Ticks;
						if(diff <= 0)
						{
							// The timeout has already fired or is about to.
							return 0;
						}
						else if(diff > (100 * TimeSpan.TicksPerSecond))
						{
							// Don't wait more than 100 seconds at a time.
							return 100000;
						}
						else
						{
							// Return the number of milliseconds + 1.
							// The "+ 1" takes care of rounding errors
							// due to converting ticks to milliseconds.
							return ((int)(diff / TimeSpan.TicksPerMillisecond))
										+ 1;
						}
					}
				}
				return -1;
			}

} // class Timer

} // namespace Xsharp