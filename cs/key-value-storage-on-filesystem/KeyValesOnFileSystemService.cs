using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;

public class KeyValesOnFileSystemService
{
    private readonly ILogger<KeyValesOnFileSystemService> _logger;
    private readonly string _storageRootDirectoryPath;
    public const string StorageRootEnvVarName = "KEY_VALUE_STORE_ROOT_DIR";
    public KeyValesOnFileSystemService(ILogger<KeyValesOnFileSystemService> logger)
    {
        _logger = logger;
        var storageRoot = Environment.GetEnvironmentVariable(StorageRootEnvVarName);
        if(string.IsNullOrWhiteSpace(storageRoot))
        {
            if(string.IsNullOrEmpty(storageRoot))
            {
                _logger.LogWarning($"Failed to read content of environment variable \"{StorageRootEnvVarName}\", got null/empty string");
            }
            else
            {
                _logger.LogWarning($"Failed to read proper content of environment variable \"{StorageRootEnvVarName}\", contained only whitespaces");
            }
            storageRoot = string.Empty; // Remove possibility of null
        }
        _storageRootDirectoryPath = storageRoot;
    }

    public bool Store(byte[] key, byte[] value)
    {
        var directory = GetDirectoryForKey(key);
        // For future me: Directory.CreateDirectory() handles the case where it already exists.
        // So same as with mkdir -p, call it just in case instead of bloating path with if()'s.
        Directory.CreateDirectory(directory); // Create if not exists
        string[] preExistingFiles = Directory.GetFiles(directory);
        var keyPath = string.Empty;
        var valuePath = string.Empty;
        if(preExistingFiles.Length == 0)
        {
            keyPath = $"{directory}/0.key";
            valuePath = $"{directory}/0.value";
            File.WriteAllBytes(keyPath, key);
            File.WriteAllBytes(valuePath, value);
            return true;
        }
        var keyFiles = preExistingFiles.Where(fileName => fileName.EndsWith(".key")).ToArray();
        foreach(var keyFile in keyFiles)
        {
            if(File.ReadAllBytes(keyFile).SequenceEqual(key))
            {
                var associatedValueFile = keyFile[0..^3] + "value";
                File.WriteAllBytes(associatedValueFile, value);
                return true;
            }
        }

        // Detect holes, and insert in hole or if none at end
        var keyFileNames = keyFiles
            .Select(fullPath => Path.GetFileNameWithoutExtension(fullPath))
            .ToArray();
        var fileNumbers = new List<int>();
        foreach(var keyFileName in keyFileNames)
        {
            if(int.TryParse(keyFileName, out var baseName) && 0 <= baseName)
            {
                fileNumbers.Add(baseName);
            }
        }
        fileNumbers.Sort();

        var nextAvailableBaseName = fileNumbers.Count;

        if(fileNumbers[^1] != fileNumbers.Count - 1)
        {
            // Here there be gaps. Unless there are duplicates. But that is hard to imagine (I don't know any file system allowing for duplicate names). Or someone snuck in a weird value that is parsable as an int. At which point, shrek it, the retrieval will end up finding this anyways.
            for(int i = 0; i < fileNumbers.Count; i++)
            {
                if(fileNumbers[i] != i)
                {
                    nextAvailableBaseName = i;
                    break;
                }
            }
        }
        keyPath = $"{directory}/{nextAvailableBaseName}.key";
        valuePath = $"{directory}/{nextAvailableBaseName}.value";
        File.WriteAllBytes(keyPath, key);
        File.WriteAllBytes(valuePath, value);
        return true;
    }

    public bool TryRetrieve(byte[] key, out byte[] value)
    {
        var directory = GetDirectoryForKey(key);
        if(!Directory.Exists(directory))
        {
            value = [];
            return false;
        }
        string[] preExistingFiles = Directory.GetFiles(directory);
        var keyFiles = preExistingFiles.Where(fileName => fileName.EndsWith(".key")).ToArray();
        foreach(var keyFile in keyFiles)
        {
            if(File.ReadAllBytes(keyFile).SequenceEqual(key))
            {
                var associatedValueFile = keyFile[0..^3] + "value";
                if(File.Exists(associatedValueFile))
                {
                    value = File.ReadAllBytes(associatedValueFile);
                    return true;
                }
                else
                {
                    _logger.LogWarning($"On retrieve found matching key at path {keyFile}, but no corresponding file/path for the value as expected at {associatedValueFile}. Checking other keys jut to be sure, but something truly weird is going on.");
                    // But it matters not, if we for some inexplicable reason have a duplicate key, we will just find and use the first proper match.
                    // If we on the other hand don't have an actual match at all, it will fall through to the "no result" below.
                }
            }
        }
        value = [];
        return false;
    }

    public bool Delete(byte[] key)
    {
        var directory = GetDirectoryForKey(key);
        if(!Directory.Exists(directory))
        {
            _logger.LogWarning($"Someone tried to delete {directory} which doesn't exist, weird");
            return true;
        }
        string[] preExistingFiles = Directory.GetFiles(directory);
        var keyFiles = preExistingFiles.Where(fileName => fileName.EndsWith(".key")).ToArray();
        foreach(var keyFile in keyFiles)
        {
            if(File.ReadAllBytes(keyFile).SequenceEqual(key))
            {
                var associatedValueFile = keyFile[0..^3] + "value";
                if(File.Exists(associatedValueFile))
                {
                    File.Delete(associatedValueFile);
                }
                else
                {
                    _logger.LogWarning($"On delete request found matching key at path {keyFile}, but no corresponding file/path for the value as expected at {associatedValueFile}.");
                }
                File.Delete(keyFile);
                return true;
            }
        }
        return true;
    }

    private string GetDirectoryForKey(byte[] key)
    {
        // Make directory for first 3 (16 ^ 3 = up to 4096 directories per level), then next 3, then dump files
        var keyHash = System.IO.Hashing.Crc32.Hash(key);
        var keyHashHex = Convert.ToHexString(keyHash).ToLowerInvariant();
        var firstLevel = keyHashHex.Substring(0, 3);
        var secondLevel = keyHashHex.Substring(3, 3);
        return $"{_storageRootDirectoryPath}/{firstLevel}/{secondLevel}/{keyHashHex}";
    }
}
