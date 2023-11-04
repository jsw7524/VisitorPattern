using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

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

            spliter.Split(root.Entries, Spliter.Direction.HORIZONTAL, 0, 0, 1800, 900);

            blocks = spliter.blocks;

        }
        public void MakeFileMap1()
        {

            Thread myThread = new Thread(new ThreadStart(MakeFileMap2), 1024 * 1024 * 15); // 105MB的堆疊
            myThread.Start();
            myThread.Join();
             Invalidate();
        }
        Directory root;
        private void button1_Click(object sender, EventArgs e)
        {

            folderBrowserDialog1.ShowDialog(this);
            root = new Directory(folderBrowserDialog1.SelectedPath, 0);


            spliter = new Spliter();
            Thread myThread = new Thread(new ThreadStart(MakeFileMap1)); 
            myThread.Start();






        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
        }
    }
}