namespace WinFormsApp1
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        Spliter spliter;

        public void DrawString(int x, int y, int width, int height, string text)
        {
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            string drawString = text;
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 8);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            formGraphics.DrawString(drawString, drawFont, drawBrush, new RectangleF(x,y, width, height));
            drawFont.Dispose();
            drawBrush.Dispose();
            formGraphics.Dispose();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Directory root = new Directory(@"D:\國庫局機器學習研究案\Predict\");

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
                            //////////////////
                            //var label = new Label();
                            //label.Location = new Point(blk.x1+30, blk.y1+30);
                            //label.Visible = true;
                            //label.Size =new Size(blk.x2 - blk.x1 -30, blk.y2 - blk.y1-30);
                            //label.Text = blk.Name;
                            //Controls.Add(label);
                            g.DrawRectangle(selPen, blk.x1, blk.y1, blk.x2 - blk.x1, blk.y2 - blk.y1);
                            DrawString(blk.x1, blk.y1, blk.x2 - blk.x1, blk.y2 - blk.y1, blk.Name + " " + Math.Round(blk.Size/1023.0)+"KB");
                            //////////////////
                        }
                    }

                }
            };
        }
    }
}