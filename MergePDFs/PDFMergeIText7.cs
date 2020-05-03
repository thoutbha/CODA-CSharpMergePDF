using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//To Use IText7 for PDF procesing we need below imports from Itext7 Library
using iText.Pdfa;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;

//FIL IO
using System.IO;

namespace MergePDFs
{
    public class PDFMergeIText7 : IMergePDF
    {
        //In the interest of time i will copy past the code for Itext7 Implementation.
        //idea is a thought process. 
        //so code is almost looks same- 
        //1.create a stream, 
        //2.create a pdf document object, 
        //3.read pdf file from specified location and 
        //4.use Merge Method to copy pdf files.


        public string MergePDF(List<string> sourceFileList, string outputFilePath)
        {
            using (var stream = new FileStream(outputFilePath, FileMode.Create))
            {
                var Outputpdf = new PdfDocument(new PdfWriter(stream));
                var pdfMerger = new PdfMerger(Outputpdf);

                foreach (string file in sourceFileList)
                {
                    var pdf = new PdfDocument(new PdfReader(file));
                    int numberofPages = pdf.GetNumberOfPages();
                    pdfMerger.SetCloseSourceDocuments(true).Merge(pdf, 1, numberofPages);
                }
                Outputpdf.Close();
            }

            return outputFilePath;
        }
    }
}
