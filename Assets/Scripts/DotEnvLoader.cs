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
                    
                var parts = line.Split('=');
                if (parts.Length == 2)
                {
                    Environment.SetEnvironmentVariable(parts[0], parts[1]);
                }
            }
        }
        else
        {
            Debug.Log($"Could not find file {filePath}");
        }
    }
}
