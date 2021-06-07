using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace FilosofosComensales
{
    class Fork
    {
        public static Form1 form;
        private static readonly Image[] img_state = new Image[] { Properties.Resources.fork, Properties.Resources.forkbusy }; 

        private readonly Semaphore semaphore = new Semaphore(1,1);
        
        private readonly PictureBox img;
        private readonly Label lbl_state;

        public Fork(PictureBox img,Label lbl_state)
        {
            this.img = img;
            this.lbl_state = lbl_state;
        }

        public bool Use(int phi,int seconds_wait)
        {
            bool isfree = semaphore.WaitOne(TimeSpan.FromSeconds(seconds_wait));
            if (isfree)
                ChangeState(1,phi);
            return isfree;
        }


        public void Free()
        {
            ChangeState(0);
            semaphore.Release();
        }

        private void ChangeState(int state,int phi = -1)
        {
            try
            {
                form.Invoke(new MethodInvoker(() => {
                    img.Image = img_state[state];
                    lbl_state.Text = phi != -1 ? "tomado por: " + phi.ToString() : "libre";
                }));
            }
            catch (Exception)
            {
            }
        }
    }
}
