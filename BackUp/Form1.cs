using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace BackUp
{
    public partial class Form1 : Form
    {
        String logFileName = null;
        Boolean LOG_ENABLED = true;
        FileWrite file;
        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("64-bit process = {0}", Environment.Is64BitProcess);
            Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (LOG_ENABLED)
            {
                String ApplicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                String MyNewPath = System.IO.Path.Combine(ApplicationDataPath, "BackUp-Utility-Log");

                if (!Directory.Exists(MyNewPath))
                {
                    Directory.CreateDirectory(MyNewPath);
                    Console.WriteLine(MyNewPath);
                }
                logFileName = MyNewPath + "\\BackUp-Utility-Log_" + System.DateTime.Now.ToFileTime() + ".txt";
                file = new FileWrite(logFileName);
            }

            if (!Directory.Exists(textBox2.Text))
            {
                Directory.CreateDirectory(textBox2.Text);
            }


            delDir(textBox2.Text);
            ProcessDirectory(textBox1.Text);
            processFiles(textBox1.Text, textBox2.Text);


            MessageBox.Show("Backup completed successfully!!", "BackUp - Utility - v 0.1");
        }

        private void ProcessDirectory(String srcPath)
        {
            String[] srcSubDir = Directory.GetDirectories(srcPath);


            foreach (String path in srcSubDir)
            {
                ProcessDirectory(path);
            }


            if (Directory.Exists(srcPath.Replace(textBox1.Text, textBox2.Text)))
            {
                processFiles(srcPath, srcPath.Replace(textBox1.Text, textBox2.Text));

            }
            else
            {
                Directory.CreateDirectory(srcPath.Replace(textBox1.Text, textBox2.Text));
                processFiles(srcPath, srcPath.Replace(textBox1.Text, textBox2.Text));
            }
            Console.WriteLine(srcPath);
            Console.WriteLine(srcPath.Replace(textBox1.Text, textBox2.Text));

        }

        private void delDir(String destDirPath)
        {

            if (!Directory.Exists(destDirPath.Replace(textBox2.Text, textBox1.Text)))
            {
                Directory.Delete(destDirPath, true);
                Console.WriteLine(destDirPath);
            }
            else
            {
                String[] destSubDir = Directory.GetDirectories(destDirPath);
                foreach (String path in destSubDir)
                {
                    Console.WriteLine(path);
                    delDir(path);

                }

            }
        }

        private void processFiles(String srcDirName, String destDirName)
        {
            String[] srcFileNames = getFileNames(srcDirName);

            String[] destFileNames = getFileNames(destDirName);

            copyChangedFiles(srcDirName, srcFileNames, destDirName, destFileNames);
            copyNewFiles(srcDirName, srcFileNames, destDirName, destFileNames);
            DeleteRemovedFiles(srcDirName, srcFileNames, destDirName, destFileNames);

        }

        private void copyChangedFiles(String srcDirName, String[] srcFileNames, String destDirName, String[] destFileNames)
        {
            String[] sCopyFiles = null;
            sCopyFiles = srcFileNames.Intersect(destFileNames).ToArray<String>();

            foreach (String path in sCopyFiles)
            {
                if (!Directory.GetLastWriteTimeUtc(srcDirName + path).Equals(Directory.GetLastWriteTimeUtc(destDirName + path)))
                {
                    System.IO.File.Delete(destDirName + path);
                    System.IO.File.Copy(srcDirName + path, destDirName + path);
                }
            }
            file.writeLog(sCopyFiles, srcDirName, destDirName, "Replace Updated Files");

        }

        private void DeleteRemovedFiles(String srcDirName, String[] srcFileNames, String destDirName, String[] destFileNames)
        {
            String[] sCopyFiles = null;
            sCopyFiles = destFileNames.Except(srcFileNames).ToArray<String>();

            foreach (String path in sCopyFiles)
            {
                System.IO.File.Delete(destDirName + path);
            }
            file.writeLog(sCopyFiles, destDirName, "Remove Deleted Files");
        }

        private void copyNewFiles(String srcDirName, String[] srcFileNames, String destDirName, String[] destFileNames)
        {
            String[] sCopyFiles = null;
            sCopyFiles = srcFileNames.Except(destFileNames).ToArray<String>();

            foreach (String path in sCopyFiles)
            {
                System.IO.File.Copy(srcDirName + path, destDirName + path);

            }
            file.writeLog(sCopyFiles, srcDirName, destDirName, "Copy New Files");
        }

        private String[] getFileNames(String directoryName)
        {
            String[] srcFileNames = Directory.GetFiles(directoryName);
            List<String> sActualFileName = new List<String>();
            foreach (String path in srcFileNames)
            {
                sActualFileName.Add(path.Replace(directoryName, ""));
            }

            return sActualFileName.ToArray<String>();

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            // OK button was pressed. 
            if (result == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog2.ShowDialog();

            // OK button was pressed. 
            if (result == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog2.SelectedPath;
            }

        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {

            DialogResult result = folderBrowserDialog1.ShowDialog();

            // OK button was pressed. 
            if (result == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void textBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DialogResult result = folderBrowserDialog2.ShowDialog();

            // OK button was pressed. 
            if (result == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog2.SelectedPath;
            }
        }

      
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }




    }
}
