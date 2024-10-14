using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System;
using System.IO;

public class AutoKeystoreSetter : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        // Get the project directory
        string projectPath = Directory.GetParent(Application.dataPath).FullName;

        // Set the relative path to your keystore (one directory up from Assets folder)
        string keystorePath = Path.Combine(projectPath, "keystore.keystore");

        // Check if the keystore file exists
        if (!File.Exists(keystorePath))
        {
            throw new FileNotFoundException($"Keystore file not found at path: {keystorePath}");
        }

        // Set the path to your keystore
        PlayerSettings.Android.keystoreName = keystorePath;
        
        // Read passwords from environment variables for security
        string keystorePass = "gamesight";
        string aliasPass = "gamesight";

        // Validate passwords are set
        if (string.IsNullOrEmpty(keystorePass) || string.IsNullOrEmpty(aliasPass))
        {
            throw new InvalidOperationException("Keystore or alias password environment variables are not set.");
        }

        // Set the keystore password
        PlayerSettings.Android.keystorePass = keystorePass;

        // Set the alias name
        PlayerSettings.Android.keyaliasName = "default-key";

        // Set the alias password
        PlayerSettings.Android.keyaliasPass = aliasPass;

        // Optional: Log to confirm values are set (remove in production)
        Debug.Log("Keystore and alias passwords have been set.");
    }
}

