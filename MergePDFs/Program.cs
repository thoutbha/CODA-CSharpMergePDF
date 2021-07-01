using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//To use FluentCommandLineParser in below program to process client inputs from command line
using Fclp;

namespace MergePDFs
{
    class Program
    {
        static void Main(string[] args)
        {
            // Developer started thinking how he can give provision in command 
            //line so that it can suffice 2 uses cases which client has provided( please refer notes to see those 2 requiremets)

            //Traditional way i can use args array and send it program methods to consider command line inputs and process logic as per inputs, 
            //however he needs a better way so that command line settings are managed well.

            //So now starting writing classes.
            //Lets move on to 1st class


            //Lets identify how we want to client to pass his arguments in command line
            // lets signify and map
            // d - direcory ->  anything after d is specified is PDF directory path
            // f - files -> anything after f is specified is specific file paths
            //o - outputfilePath -> client can chose his output file name and its path
            //-a - AllInFolder flag -> this can help us where we need to process all folders from 'd' location or not

            //quite Note: i am not considering all edge cases however will ensure best case run which can suffice client requirements


            // we need parser which we can map client passed arguments 
            //based on above design, to suffice this we can use FluentCommandLineParser

            //Please import FluentCommandLineParser from Nuget package
            //here we are "configuring/setting up" the mappings to our setting class properties based on our above design considerations for FluentCommandLineParser
            var parser = new FluentCommandLineParser<Settings>();
            parser.Setup(arg => arg.PDFDirectoryPath).As('d', "directory");
            parser.Setup(arg => arg.FileList).As('f', "files");
            parser.Setup(arg => arg.OutputPath).As('o', "output").Required();
            parser.Setup(arg => arg.AllInFolder).As('a', "all");

            //parse the args array to parser setup
            var result = parser.Parse(args);

            //If we get any errors during parsing input from client argurments in command line 
            //we want to tell user with meaningfull instructions.
            if (result.HasErrors)
            {
                Console.WriteLine("     ");
                Console.WriteLine("==> Usage: Guidelines");
                Console.WriteLine("     ");

                Console.WriteLine("------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("To Merge ALL PDF's - Sample Run");
                Console.WriteLine("------------------------------------------------------------------------------------------------------------");
                Console.WriteLine(@" MergePDFs.exe -d C:\Users\Admin\Documents\Csharp -a -o C:\Users\Admin\Documents\Csharp\Csharp_PDFs_ALL.pdf");
                Console.WriteLine("     ");

                Console.WriteLine("------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("To Merge Specific PDF's - Ensure no white-spaces for specific pdf file");
                Console.WriteLine("------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("     ");
                Console.WriteLine(" COREECT WAY: ");
                
                Console.WriteLine(@"  -d C:\Users\Admin\Documents\Csharp -f C#NotesforProfessionals.pdf ProgrammingC#.pdf -o C:\Users\Admin\Documents\Csharp\Csharp_PDFs_Specific.pdf");
                Console.WriteLine("     ");
                Console.WriteLine(" THIS WONT WORK, rename the file without white-spaces in it");
                Console.WriteLine(@"  -d C:\Users\Admin\Documents\Csharp -f C# Notes for Professionals.pdf Programming C#.pdf' -o C:\Users\Admin\Documents\Csharp\Csharp_PDFs_Specific.pdf");

                Console.ReadLine();
                return;
            }

            // send the settings which client has mapped to Run Method
            Run(parser.Object);

        }

       static void Run(Settings settings)
        {
            Console.WriteLine("Running merger...");

            // ADDING THIS COMMENT TO CHECK IF GIT COMMIT IS WORKING OR NOT.
            // Mapping concrete class PDFMerge which has implmentation with ITextSharp.
            //IMergePDF merger = new PDFMerge();

            // Mapping concrete class PDFMerge which has implmentation with IText7.
            IMergePDF merger = new PDFMergeIText7();

            //TODO : USE DEPENDENCY INJECTION - so that client code cannot be modified at all for future changes in 3rd party library . just an IDEA. you can modify and share it to us.

            var files = new List<String>();

            //FILE IO, to read file details from pdf directory path
            var pdfDirectory = new DirectoryInfo(settings.PDFDirectoryPath);

            // To process all files or specific files from give directory path
            // setting.allInfolder if true signify to process all files from directory path
            if (settings.AllInFolder)
            {
                //Find all pdf's from PDF Directory Path
                //TopDirectoryOnly - will use only current directory path for getting the files
                var allPdfs = pdfDirectory.GetFiles("*.pdf", SearchOption.TopDirectoryOnly);

                //Once we have all pdfs details, we are interested in pdf file names
                files = allPdfs.Select(f => f.FullName).ToList();
            }
            else
            {
                //get specific files from directory path.
                files = settings.FileList.Select(f => Path.Combine(settings.PDFDirectoryPath, f)).ToList();
            }

            merger.MergePDF(files, settings.OutputPath);

            Console.WriteLine($"Merged succesfully. Output saved: {settings.OutputPath}");
        }



    }








}
