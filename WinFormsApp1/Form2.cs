using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public Form2(Form1 mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;
        }
        Form1 _mainForm;
        Spliter spliter;
        List<Block> blocks;
        public void DrawString(int x, int y, int width, int height, string text)
        {
            if (width * height < 100)
            {
                return;
            }

            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            string drawString = text;
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 8);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            formGraphics.DrawString(drawString, drawFont, drawBrush, new RectangleF(x, y, width, height));
            drawFont.Dispose();
            drawBrush.Dispose();
            formGraphics.Dispose();
        }


        public void MakeFileMap2()
        {
            spliter.Split(_mainForm.root.Entries, Spliter.Direction.HORIZONTAL, 0, 0, this.Width, this.Height);

            blocks = spliter.blocks;

        }
        public void MakeFileMap1()
        {

            Thread myThread = new Thread(new ThreadStart(MakeFileMap2), 1024 * 1024 * 15); // 15MB的堆疊
            myThread.Start();
            myThread.Join();
            Invalidate();
        }
        //Directory root;
        private void Form2_Load(object sender, EventArgs e)
        {
            this.Text ="File Map for " + _mainForm.root.GetName();
            this.Paint += (o, e) =>
            {

                Graphics g = e.Graphics;
                using (Pen selPen = new Pen(Color.Blue))
                {
                    if (blocks != null)
                    {
                        foreach (Block blk in blocks)
                        {
                            g.DrawRectangle(selPen, blk.x1, blk.y1, blk.x2 - blk.x1, blk.y2 - blk.y1);
                            DrawString(blk.x1, blk.y1, blk.x2 - blk.x1, blk.y2 - blk.y1, blk.Name + " " + blk.Size + "KB");
                        }
                    }
                }
            };




            spliter = new Spliter();
            Thread myThread = new Thread(new ThreadStart(MakeFileMap1));
            myThread.Start();

        }
    }
}
