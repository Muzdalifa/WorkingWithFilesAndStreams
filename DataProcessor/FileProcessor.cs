using System;
using System.IO;

namespace DataProcessor
{
    class FileProcessor
    {
        
        public string InputFilePath { get;}

        public FileProcessor(string filePath) => InputFilePath = filePath;
    
        public void Process()
        {
            Console.WriteLine($"Begin process of {InputFilePath}");

            //Check if file exist
            if (!File.Exists(InputFilePath))
            {
                Console.WriteLine($"ERRO: file {InputFilePath} does not exist.");
                return;
            }
        }
    }
}
