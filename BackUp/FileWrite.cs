using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BackUp
{

    class FileWrite
    {
        public String FileName
        {
            set
            {
                this.FileName = value;
            }
            get
            {
                return this.FileName;
            }
        }

        public FileWrite(String FileName)
        {
            this.FileName = FileName;
        }       

        public void writeLog(String[] FileName, String srcDirName, String destDirName, String actionName)
        {

            int iLoop = 0;
            string s = null;

                for (iLoop = 0; iLoop < FileName.Length; iLoop++)
                {
                    s = s + actionName + "," + srcDirName + FileName[iLoop] + "," + destDirName + FileName[iLoop] + "\r\n";
                }

                if (!string.IsNullOrEmpty(s))
                {
                    FileStream fs = File.Open(this.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                    fs.Seek(0, SeekOrigin.End);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(s);
                    bw.Close();
                    fs.Close();
                }
            
        }

        public void writeLog(String[] FileName, String srcDirName, String actionName)
        {

            int iLoop = 0;
            string s = null;
            

                for (iLoop = 0; iLoop < FileName.Length; iLoop++)
                {
                    s = s + actionName + "," + srcDirName + FileName[iLoop] + "\r\n";
                }

                if (!string.IsNullOrEmpty(s))
                {
                    FileStream fs = File.Open(this.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                    fs.Seek(0, SeekOrigin.End);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(s);
                    bw.Close();
                    fs.Close();

                }
           
        }

    }
}
