using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace FilosofosComensales
{
    public partial class Form1 : Form
    {
        Thread[] threads_phi = new Thread[5];
        Philosopher[] philosophers = new Philosopher[5];

        public Form1()
        {
            InitializeComponent();
            Philosopher.form = this;

            philosophers[0] = new Philosopher(0,4,0,Img_philosopher_0,phy_state_0, phi_eat_count0);
            philosophers[1] = new Philosopher(1,0,1,Img_philosopher_1,phy_state_1, phi_eat_count1);
            philosophers[2] = new Philosopher(2,1,2,Img_philosopher_2,phy_state_2, phi_eat_count2);
            philosophers[3] = new Philosopher(3,2,3,Img_philosopher_3,phy_state_3, phi_eat_count3);
            philosophers[4] = new Philosopher(4,3,4,Img_philosopher_4,phy_state_4, phi_eat_count4);
            for (int i = 0; i < 5; i++)
            {
                threads_phi[i] = new Thread(new ThreadStart(philosophers[i].Start));
            }

            Label[] labels = new Label[] { lbl_fork_0, lbl_fork_1, lbl_fork_2, lbl_fork_3, lbl_fork_4 };
            PictureBox[] pictures = new PictureBox[] { Fork0, Fork1, Fork2, Fork3, Fork4 };
            Philosopher.ForksInitializer(pictures,labels,this);
        }

        private void Btn_start_Click(object sender, EventArgs e)
        {
            Btn_start.Enabled = false;
            foreach (Thread thread in threads_phi)
                thread.Start();
                
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Philosopher philosopher in philosophers)
                philosopher.Stop();
        }

        private void Continue()
        {
            bool _continue = false;
            foreach (Philosopher p in philosophers)
                if (p.GetEatsCount() < 5)
                {
                    _continue = true;
                    break;
                }
            if (!_continue)
                foreach (Philosopher p in philosophers)
                    p.Stop();
        }

        private void phi_eat_count0_TextChanged(object sender, EventArgs e)
        {
            Continue();
        }
    }
}
