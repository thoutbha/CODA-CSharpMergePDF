using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// for PDF processing we need to import below namespaces
using iTextSharp.text;
using iTextSharp.text.pdf;

//For File Input Output
using System.IO;

namespace MergePDFs
{

    //Developer started thinking in OOPS :) and based on client requirement he felt client wants to Merge PDF's at the end, so he created a interface MergePDF.
    public interface IMergePDF
    {
        //will return outputfile path once pdf are merged successfully
        string MergePDF(List<string> sourceFileList, string outputFilePath);
    }

    public class PDFMerge : IMergePDF
    {
        public string MergePDF(List<string> sourceFileList, string outputFilePath)
        {
            //we need iTextSharp code here for merge pdfs's, please import its library from iTextSharp

            //stream object to hold merge pdfs from below processing logic
            using (var fileStream = new FileStream(outputFilePath,FileMode.Create))
            {
                //we need a pdf document object that can hold all merged pdf contents
                using (var pdfDocument = new Document())
                {
                    //mapping stream to pdfDocument object
                    var pdf = new PdfCopy(pdfDocument, fileStream);
                    //Open the PDF for Merge
                    pdfDocument.Open();

                    //loping through all files from list collection
                    foreach (string file in sourceFileList) 
                    {
                        pdf.AddDocument(new PdfReader(file));
                    }
                }
            }
            
           return outputFilePath;
        }
    }


}
