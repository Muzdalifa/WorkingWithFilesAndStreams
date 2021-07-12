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
        }
    }
}
