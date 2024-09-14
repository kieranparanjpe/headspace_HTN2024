using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class DotEnvLoader
{
    public static void LoadEnvFile(string filePath)
    {
        Debug.Log(System.IO.Directory.GetCurrentDirectory());
        
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) // Skip empty lines and comments
                    continue;
                    
                int index = line.IndexOf('=');
                if (index >= 0 && index < line.Length)
                {
                    string p1 = line.Substring(0, index);
                    string p2 = line.Substring(index + 1);
                    Environment.SetEnvironmentVariable(p1, p2);
                }
            }
        }
        else
        {
            Debug.Log($"Could not find file {filePath}");
        }
    }
}
