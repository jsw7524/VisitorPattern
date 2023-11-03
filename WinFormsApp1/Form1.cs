namespace WinFormsApp1
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        Spliter spliter;

        public void DrawString(int x, int y, string text)
        {
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            string drawString = text;
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 6);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            formGraphics.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
            drawFont.Dispose();
            drawBrush.Dispose();
            formGraphics.Dispose();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Directory root = new Directory("D:\\Demo\\ConsoleApp1\\");

            spliter = new Spliter();
            spliter.Split(root.Entries, Spliter.Direction.HORIZONTAL, 0, 0, 1800, 900);

            Invalidate();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Paint += (o, e) =>
            {
                Graphics g = e.Graphics;
                using (Pen selPen = new Pen(Color.Blue))
                {
                    if (spliter?.blocks != null)
                    {
                        foreach (Block blk in spliter.blocks)
                        {
                            g.DrawRectangle(selPen, blk.x1, blk.y1, blk.x2 - blk.x1, blk.y2 - blk.y1);
                            DrawString(blk.x1, blk.y1, blk.Name + " " + blk.Size);
                        }
                    }

                }
            };
        }
    }
}