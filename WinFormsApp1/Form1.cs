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
        private void button1_Click(object sender, EventArgs e)
        {

            folderBrowserDialog1.ShowDialog(this);
            root = new Directory(folderBrowserDialog1.SelectedPath, 0);

            ListVisitor listVisitor = new ListVisitor();

            listVisitor.Visit(root);
            listVisitor.Flush();
            richTextBox1.LoadFile(listVisitor.streamToReturn, RichTextBoxStreamType.PlainText);



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