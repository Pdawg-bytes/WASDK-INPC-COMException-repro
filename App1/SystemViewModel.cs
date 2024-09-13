using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace App1
{
    // code taken from UWP community discord from a member having this problem.

    // all of the commented dispatcher code fixes the issue, but without it, modifying the Uptime property from
    // a different thread throws a generic COMException error instead of the expected "RPC Marshalled from wrong thread" error.
    public class SystemViewModel : INotifyPropertyChanged
    {
        private Timer _timer;
        private string _uptime;
        //public DispatcherQueue DispatcherQueue { get; init; }

        public SystemViewModel()
        {
            //DispatcherQueue = DispatcherQueue.GetForCurrentThread();
            _timer = new Timer(1000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
            _timer.Start();
        }

        public string Uptime
        {
            get => _uptime;
            private set
            {
                _uptime = value;
                OnPropertyChanged();
            }
        }

        int i = 0;
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            i++;
            TimeSpan currentTime = new TimeSpan(1, 1, 2, i);

            //DispatcherQueue.TryEnqueue(() =>
            //{
            Uptime = $"Days: {currentTime.Days} Hours: {currentTime.Hours} Minutes: {currentTime.Minutes} Seconds: {currentTime.Seconds}";
            //});
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
