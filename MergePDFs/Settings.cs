using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergePDFs
{
    public class Settings
    {
        //Clients 1st Req: Will specify folder path and wants to merge all pdf from that location
        public string PDFDirectoryPath { get; set; }

        //Cient 2nd Req: Will give specific pdf's file path
        public List<string> FileList { get; set; } = new List<string>();

        //We need a flag to identify to consider which client requirement has come to process for a program
        public bool AllInFolder { get; set; } = false;

        //Output pdf path which has all merged pdf's
        public string OutputPath { get; set; }

    }
}
