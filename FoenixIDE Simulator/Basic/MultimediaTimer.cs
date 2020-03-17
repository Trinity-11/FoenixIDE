using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FoenixIDE.Timers
{
    // This code was added from a www source.
    // Thanks to Laureano Lopez http://www.linkedin.com/in/lopezlaureano.
    // Source: http://www.softwareinteractions.com/blog/2009/12/7/using-the-multimedia-timer-from-c.html


    // Summary:
    //     Represents the method that will handle the
    //     System.Timers.MultimediaTimer.Elapsed event
    //     of a System.Timers.MultimediaTimer.
    //
    // Parameters:
    //   sender:
    //     The source of the event.
    //
    //   e:
    //     An System.Timers.MultimediaElapsedEventHandler object that contains
    //     the event data.
    public delegate void MultimediaElapsedEventHandler(object sender,
                                                MultimediaElapsedEventArgs e);

    // Summary:
    //     Provides data for the System.Timers.Timer.Elapsed event.
    public class MultimediaElapsedEventArgs : EventArgs
    {
        // Summary:
        //     Gets the time the System.Timers.Multimedia.Elapsed event was 
        //     raised.
        //
        // Returns:
        //     The time the System.Timers.Multimedia.Elapsed event was raised.
        public DateTime SignalTime { get; internal set; }

        internal MultimediaElapsedEventArgs()
        {
            SignalTime = DateTime.Now;
        }
    }

    // Summary:
    //     Generates recurring events in an application.
    public class MultimediaTimer : IDisposable
    {
        //Lib API declarations
        /// <summary>
        /// Times the set event.
        /// </summary>
        /// <param name="uDelay">The u delay.</param>
        /// <param name="uResolution">The u resolution.</param>
        /// <param name="lpTimeProc">The lp time proc.</param>
        /// <param name="dwUser">The dw user.</param>
        /// <param name="fuEvent">The fu event.</param>
        /// <returns></returns>
        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        private static extern uint timeSetEvent(uint uDelay, uint uResolution,
                      TimerCallback lpTimeProc, UIntPtr dwUser, uint fuEvent);

        /// <summary>
        /// Times the kill event.
        /// </summary>
        /// <param name="uTimerID">The u timer ID.</param>
        /// <returns></returns>
        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        private static extern uint timeKillEvent(uint uTimerID);

        /// <summary>
        /// Times the get time.
        /// </summary>
        /// <returns></returns>
        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        private static extern uint timeGetTime();

        /// <summary>
        /// Times the begin period.
        /// </summary>
        /// <param name="uPeriod">The u period.</param>
        /// <returns></returns>
        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        private static extern uint timeBeginPeriod(uint uPeriod);

        /// <summary>
        /// Times the end period.
        /// </summary>
        /// <param name="uPeriod">The u period.</param>
        /// <returns></returns>
        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        private static extern uint timeEndPeriod(uint uPeriod);

		// Use this to pin the timerCallback functions to avoid improper garbage collection
        private GCHandle _gcHandle;


        /// <summary>
        ///Timer type definitions
        /// </summary>
        [Flags]
        public enum fuEvent : uint
        {
            /// <summary>
            /// OneHzSignalEvent occurs once, after uDelay milliseconds. 
            /// </summary>
            TIME_ONESHOT = 0,
            /// <summary>
            /// 
            /// </summary>
            TIME_PERIODIC = 1,
            /// <summary>
            ///  callback is function
            /// </summary>
            TIME_CALLBACK_FUNCTION = 0x0000,

        }

        /// <summary>
        /// Delegate definition for the API callback
        /// </summary>
        private delegate void TimerCallback(uint uTimerID, uint uMsg,
                                    UIntPtr dwUser, UIntPtr dw1, UIntPtr dw2);

        /// <summary>
        /// The current timer instance ID
        /// </summary>
        private uint id = 0;

        /// <summary>
        /// The callback used by the the API
        /// </summary>
        private TimerCallback timerCallback;


        /// <summary>
        /// Initializes a new instance of the System.Timers.MultimediaTimer 
        //  class, and sets all the properties to their initial values.
        /// </summary>
        public MultimediaTimer()
        {
            Interval = 100;
            AutoReset = true;
            Enabled = false;
            //Initialize the API callback
            timerCallback = CallbackFunction;
			// pin the timerCallback to a fixed memory address, such that the c# GC won't mess with it.
            _gcHandle = GCHandle.Alloc(timerCallback);
        }

        /// <summary>
        ///    Initializes a new instance of the System.Timers.MultimediaTimer 
        ///    class, and sets the
        ///    System.Timers.MultimediaTimer.Interval property to the specified 
        ///    time period.
        ///
        /// Parameters:
        ///   interval:
        ///     The time, in milliseconds, between events.
        ///
        /// Exceptions:
        ///   System.ArgumentException:
        ///     The value of the interval parameter is less than or equal to 
        ///     zero, or greater than System.Int32.MaxValue.
        /// </summary>
        /// <param name="interval">The interval.</param>
        public MultimediaTimer(uint interval)
        {
            Interval = interval;
            AutoReset = true;
            Enabled = false;
            //Initialize the API callback
            timerCallback = CallbackFunction;
            _gcHandle = GCHandle.Alloc(timerCallback);
        }


        /// <summary>
        /// Gets or sets a value indicating whether the 
        /// System.Timers.MultimediaTimer should raise
        /// the System.Timers.MultimediaTimer.Elapsed event each time the 
        /// specified interval elapses
        /// or only after the first time it elapses.
        ///
        /// Returns:
        ///     true if the System.Timers.MultimediaTimer should raise the 
        ///     System.Timers.MultimediaTimer.Elapsed
        ///     event each time the interval elapses; false if it should raise 
        ///     the System.Timers.MultimediaTimer.Elapsed
        ///     event only once, after the first time the interval elapses. The 
        ///     default is true.
        /// </summary>
        /// <value><c>true</c> if [auto reset]; otherwise, <c>false</c>.</value>
        public bool AutoReset { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the 
        /// System.Timers.MultimediaTimer should raise
        /// the System.Timers.MultimediaTimer.Elapsed event.
        ///
        /// Returns:
        ///     true if the System.Timers.MultimediaTimer should raise the 
        ///     System.Timers.MultimediaTimer.Elapsed
        ///     event; otherwise, false. The default is false.
        ///        
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; private set; }

        private object syncLock = new object();

        /// <summary>
        /// Gets or sets the interval at which to raise the    
        /// System.Timers.MultimediaTimer.Elapsed event.
        ///
        /// Returns:
        ///     The time, in milliseconds, between raisings of the 
        ///     System.Timers.MultimediaTimer.Elapsed
        ///     event. The default is 100 milliseconds.
        ///
        /// Exceptions:
        ///   System.ArgumentException:
        ///     The interval is less than or equal to zero.
        /// </summary>
        /// <value>The interval.</value>
        public uint Interval { get; set; }


        /// <summary>
        /// Occurs when the interval elapses.  
        /// </summary>
        public event MultimediaElapsedEventHandler Elapsed;


        /// <summary>
        /// Releases the resources used by the System.Timers.MultimediaTimer.
        /// </summary>
        public void Close()
        {
            Dispose();
        }


        /// <summary>
        /// Starts raising the System.Timers.MultimediaTimer.Elapsed event by 
        /// setting System.Timers.MultimediaTimer.Enabled
        /// to true.
        ///
        /// Exceptions:
        ///   System.ArgumentOutOfRangeException:
        ///     The System.Timers.MultimediaTimer is created with an interval 
        ///     equal to or greater than
        ///     System.Int32.MaxValue + 1, or set to an interval less than zero.
        /// </summary>
        public void Start()
        {
            lock (syncLock)
            {
                //Kill any existing timer
                Stop();
                Enabled = false;

                //Set the timer type flags
                fuEvent f = fuEvent.TIME_CALLBACK_FUNCTION | (AutoReset ?
                                 fuEvent.TIME_PERIODIC : fuEvent.TIME_ONESHOT);

                id = timeSetEvent(Interval, 0, timerCallback, UIntPtr.Zero,
                                                                      (uint)f);
                if (id == 0)
                {
                    throw new Exception("timeSetEvent error");
                }
                Debug.WriteLine("MultimediaTimer " + id.ToString() + " started");
                Enabled = true;
            }
        }


        /// <summary>
        /// Stops raising the System.Timers.MultimediaTimer.Elapsed event by 
        /// setting System.Timers.MultimediaTimer.Enabled
        ///  to false.
        /// </summary>
        public void Stop()
        {
            lock (syncLock)
            {
                if (id != 0)
                {
                    timeKillEvent(id);
                    Debug.WriteLine("MultimediaTimer " + id.ToString() + " stopped");
                    id = 0;
                    Enabled = false;
                }
            }
        }

        /// <summary>
        /// Called when [timer].
        /// </summary>
        protected virtual void OnTimer()
        {
            if (Elapsed != null)
            {
                Elapsed(this, new MultimediaElapsedEventArgs());
            }
        }

        /// <summary>
        /// CBs the func.
        /// </summary>
        /// <param name="uTimerID">The u timer ID.</param>
        /// <param name="uMsg">The u MSG.</param>
        /// <param name="dwUser">The dw user.</param>
        /// <param name="dw1">The DW1.</param>
        /// <param name="dw2">The DW2.</param>
        private void CallbackFunction(uint uTimerID, uint uMsg, UIntPtr dwUser,
                                                      UIntPtr dw1, UIntPtr dw2)
        {
            //Callback from the MultimediaTimer API that fires the Timer event. 
            // Note we are in a different thread here
            OnTimer();
        }

        #region IDisposable Members

        private bool _disposed = false;


        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        ///  releasing, or resetting unmanaged resources.
        /// Releases all resources used by the current 
        /// System.Timers.MultimediaTimer.
        ///
        /// Parameters:
        ///   disposing:
        ///     true to release both managed and unmanaged resources; false to 
        ///     release only
        ///     unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _gcHandle.Free();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and 
        ///    unmanaged resources; <c>false</c> to release only unmanaged 
        ///  resources.</param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Stop();
                }
            }
            _disposed = true;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations 
        /// before the
        /// <see cref="MMTimer"/> is reclaimed by garbage collection.
        /// </summary>
        ~MultimediaTimer()
        {
            Dispose(false);
        }

        #endregion
    }
}
