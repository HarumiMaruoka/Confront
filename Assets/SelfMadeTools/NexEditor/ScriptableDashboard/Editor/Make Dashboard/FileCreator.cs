using System;
using System.IO;
using UnityEngine;

namespace NexEditor.ScriptableDashboard
{
    public class FileCreator
    {
        public static void MakeCSharpFile(string path, string sourceCode)
        {
            // Ensure the directory exists.
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Remove trailing spaces from each line in the source code.
            string[] lines = sourceCode.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].TrimEnd();
            }
            string cleanedSourceCode = string.Join(Environment.NewLine, lines);

            // Write the cleaned source code to the file.
            File.WriteAllText(path, cleanedSourceCode);
        }
    }
}