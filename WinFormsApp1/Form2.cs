using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

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

        Dictionary<string, SolidBrush> colorBrush = new Dictionary<string, SolidBrush>()
        {
            { ".txt", new SolidBrush(Color.FromArgb(255,255,255))},
            { ".doc", new SolidBrush(Color.FromArgb(19,79,92))},
            { ".docx", new SolidBrush(Color.FromArgb(19,79,92))},
            { ".pdf", new SolidBrush(Color.FromArgb(236,0,140))},
            { ".xls", new SolidBrush(Color.FromArgb(0,128,0))},
            { ".xlsx", new SolidBrush(Color.FromArgb(0,128,0))},

            { ".ppt", new SolidBrush(Color.FromArgb(204,0,0))},
            { ".pptx", new SolidBrush(Color.FromArgb(204,0,0))},

            { ".jpg", new SolidBrush(Color.FromArgb(255,223,186))},

            { ".png", new SolidBrush(Color.FromArgb (0,162,232))},
            { ".jpeg", new SolidBrush(Color.FromArgb(255,223,186))},
            { ".gif", new SolidBrush(Color.FromArgb(255,221,0))},
            { ".svg", new SolidBrush(Color.FromArgb(102,153,204))},
            { ".mp3", new SolidBrush(Color.FromArgb(255,102,0))},
            { ".wav", new SolidBrush(Color.FromArgb(0,0,0))},
            { ".mp4", new SolidBrush(Color.FromArgb(0,51,102))},
            { ".avi", new SolidBrush(Color.FromArgb(63,72,204))},


            { ".exe", new SolidBrush(Color.FromArgb(58,150,221))},
            { ".bat", new SolidBrush(Color.FromArgb(77,77,77))},
            { ".py", new SolidBrush(Color.FromArgb(53,114,165))},
            { ".zip", new SolidBrush(Color.FromArgb(232,60,60))},
            { ".rar", new SolidBrush(Color.FromArgb(255,69,0))},
            { ".7z", new SolidBrush(Color.FromArgb(0,70,140))},

        };



        private void Form2_Load(object sender, EventArgs e)
        {
            Regex extName = new Regex("\\.(?<EXT>\\w{1,4})$");
            Random random = new Random();
            this.Text = "File Map for " + _mainForm.root.GetName();
            this.Paint += (o, e) =>
            {


                SolidBrush brush = new SolidBrush(Color.FromArgb(0, 0, 0));
                Graphics g = e.Graphics;
                using (Pen selPen = new Pen(Color.Blue))
                {
                    if (blocks != null)
                    {
                        foreach (Block blk in blocks)
                        {
                            //g.DrawRectangle(selPen, blk.x1, blk.y1, blk.x2 - blk.x1, blk.y2 - blk.y1);
                            Match m = extName.Match(blk.Name);
                            if (!colorBrush.ContainsKey(m.Value))
                            {

                                colorBrush.Add(m.Value, new SolidBrush(Color.FromArgb(random.Next() % 256, random.Next() % 256, random.Next() % 256)));

                            }

                            g.FillRectangle(colorBrush[m.Value], blk.x1, blk.y1, blk.x2 - blk.x1, blk.y2 - blk.y1);
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
