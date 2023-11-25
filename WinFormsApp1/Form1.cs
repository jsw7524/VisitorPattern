using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();



        }

        public Directory root;


        public async Task Run1()
        {
            folderBrowserDialog1.ShowDialog(this);
            root = new Directory(folderBrowserDialog1.SelectedPath, 0);
            ListVisitor listVisitor = new ListVisitor();
            await Task.Run(() =>
            {
                listVisitor.Visit(root);
                listVisitor.Flush();
            });
            richTextBox1.LoadFile(listVisitor.streamToReturn, RichTextBoxStreamType.PlainText);
            button2.Enabled = true;
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            await Run1();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var fm2 = new Form2(this);
            fm2.Show();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}