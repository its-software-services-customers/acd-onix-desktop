using System;

namespace Onix.Client.Helper
{
    public class CThreadSync
    {
        private int curr = 0;
        private int max = 0;
        private Boolean done = false;

        public CThreadSync()
        {

        }

        public Boolean GetIsDone()
        {
            return (done);
        }

        public void UpdateDone(Boolean flag)
        {
            done = flag;
        }

        public void UpdateProgress(int c, int m)
        {
            curr = c;
            max = m;
        }

        public int GetMax()
        {
            return (max);
        }

        public int GetCurrent()
        {
            return (curr);
        }
    }
}
