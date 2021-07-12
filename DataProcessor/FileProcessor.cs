using System;
using System.IO;

namespace DataProcessor
{
    class FileProcessor
    {
        private const string BackupDirectoryName = "backup";
        private const string InProgressDirectoryName = "processing";
        private const string CompleteDirectoryName = "complete";
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

            //Getting the parent directory of a path
            string rootDirectoryPath = new DirectoryInfo(InputFilePath).Parent.Parent.FullName;
            Console.WriteLine($"Root data path is {rootDirectoryPath}");

            //Check if backup exist, if its not create new directory(folder)
            //string backupDirectoryPath = rootDirectoryPath + "/" + BackupDirectoryName; //this is the C# way of concatinating path 
            string backupDirectoryPath = Path.Combine(rootDirectoryPath, BackupDirectoryName); //Better to use this

            if (!Directory.Exists(backupDirectoryPath))
            {
                Console.WriteLine($"Creating {backupDirectoryPath}");
                Directory.CreateDirectory(backupDirectoryPath); //create folder/directory called backup
            }

            //Now we have create folder(directory) called backup, we want to copy file data.txt from in to backup : //Copy file to backup dir
            //get file name component of the original path
            string inputFileName = Path.GetFileName(InputFilePath);  //this retun filename with extension. To get filename without extension use GetFileNameWithoutExtension() method
            Console.WriteLine(inputFileName);
            string backupFilePath = Path.Combine(backupDirectoryPath, inputFileName);
            Console.WriteLine(backupFilePath);
            Console.WriteLine($"Copying {InputFilePath} to {backupFilePath}");
            //copy file (source, distination)
            //File.Copy(InputFilePath, backupFilePath); //if file is already exist with that name it will return exception, so
            //copy file even if file already exist overwrite it
            File.Copy(InputFilePath, backupFilePath, true);

            //Move file to in progress dir
            Directory.CreateDirectory(Path.Combine(rootDirectoryPath, InProgressDirectoryName)); //Create folder called "processing"
            string inProgressFilePath =
                Path.Combine(rootDirectoryPath, InProgressDirectoryName, inputFileName);

            if (File.Exists(inProgressFilePath)) 
            {
                Console.WriteLine($"ERROR: a file with name {inProgressFilePath} is already being processed.");
                return;
            }

            Console.WriteLine($"Moving {InputFilePath} to {inProgressFilePath}");
            File.Move(InputFilePath, inProgressFilePath);

            //Getting the File Extension from  a File Name
            //Determin type of fyle
            string extension = Path.GetExtension(InputFilePath);

            switch (extension)
            {
                case ".txt":
                    ProcessTextFile(inProgressFilePath);
                    break;
                default:
                    Console.WriteLine($"{extension} is an unsupported file type. ");
                    break;
            }

            //Move file after processing is complete
            string completeDirectoryPath = Path.Combine(rootDirectoryPath, CompleteDirectoryName);
            Directory.CreateDirectory(completeDirectoryPath);
            Console.WriteLine($"Moving {inProgressFilePath} to {completeDirectoryPath}");
            //File.Move(inProgressFilePath, Path.Combine(completeDirectoryPath, inputFileName)); we are not doing this here, we will append the file 

            //Append GUID to the file name ie if we process same name twice, we waill have unique file name output to the completed directory
            string completedFileName =
                $"{Path.GetFileNameWithoutExtension(InputFilePath)} - {Guid.NewGuid()}{extension}";

            //technique: to change file extension we can use
            completedFileName = Path.ChangeExtension(completedFileName, ".complete");

            var completedFilePath = Path.Combine(completeDirectoryPath, completedFileName);

            File.Move(inProgressFilePath, completedFilePath);
        }

        private void ProcessTextFile(string inProgressFilePath)
        {
            Console.WriteLine($"Processing text file {inProgressFilePath}");
            //Read in and process
        }
    }
}
