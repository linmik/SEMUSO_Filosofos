using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace FilosofosComensales
{
    class Philosopher
    {
        public static Form1 form;
        private static readonly Image[] img_state = new Image[3] {
            Properties.Resources.filosofo_dormido,
            Properties.Resources.filosofo_pensando,
            Properties.Resources.filosofo_comiendo,
        };
        private static readonly string[] text_state = new string[] { "Dormido", "Pensando", "Comiendo" };
        private static readonly Random rand = new Random();
        private static Fork[] forks = new Fork[5];

        private readonly int f_right;
        private readonly int f_left;
        private readonly int id;

        private bool run;
        private int eats_cont;

        private readonly PictureBox img;
        private readonly Label lbl_state;
        private readonly Label lbl_eats_count;

        public static void ForksInitializer(PictureBox[] pictures, Label[] labels, Form1 f)
        {
            Fork.form = f;
            for (int i = 0; i < forks.Length; i++)
                forks[i] = new Fork(pictures[i], labels[i]);
        }

        public Philosopher(int id, int right, int left, PictureBox img, Label lbl_state, Label eats)
        {
            this.id = id;
            f_left = left;
            f_right = right;

            this.img = img;
            this.lbl_state = lbl_state;
            lbl_eats_count = eats;

            run = true;
            eats_cont = 0;
        }


        public void Start()
        {
            while (run)
            {
                int seg = rand.Next(2, 16);
                for (int i = 0; i < seg && run; i++)
                    Thread.Sleep(500);
                Think();
            }
        }

        public void Stop()
        {
            run = false;
        }

        public void Think()
        {
            bool eat = false;
            ChangeState(1);
            do
            {
                if (forks[f_right].Use(id, 20))
                {
                    if (forks[f_left].Use(id, rand.Next(5, 8)))
                    {
                        Eat();
                        eat = true;
                    }
                    else
                        forks[f_right].Free();
                }
            } while (!eat);
        }

        public void Eat()
        {
            eats_cont++;
            ChangeState(2);
            int seg = rand.Next(2, 16);
            for (int i = 0; i < seg && run; i++)
                Thread.Sleep(500);

            forks[f_right].Free();
            forks[f_left].Free();
            ChangeState(0);
        }

        public void ChangeState(int state)
        {
            try
            {
                form.Invoke(new MethodInvoker(() =>
                {
                    img.Image = img_state[state];
                    lbl_eats_count.Text = eats_cont.ToString();
                    lbl_state.Text = text_state[state];
                }));
            }
            catch (Exception)
            {
            }
        }

        public int GetEatsCount()
        {
            return eats_cont;
        }
    }
}
