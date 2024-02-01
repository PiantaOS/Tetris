    using System.Threading;
    using TimeSpan = System.TimeSpan;
    public static class Timer{
        public static void Wait(int waitMilliseconds, ref bool finished){
            finished = false;
                try {
                    Thread.Sleep(waitMilliseconds);

                }
                catch (ThreadInterruptedException) {
                    //ignore
                }

                finished = true;
        }
    }