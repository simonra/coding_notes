using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

using ILoggerFactory factory = LoggerFactory.Create(
    builder =>
    {
        builder.SetMinimumLevel(LogLevel.Information);
        builder.SetMinimumLevel(LogLevel.Debug);
        builder.AddConsole();
    });
ILogger logger = factory.CreateLogger("File system KeyValue store demo");

// Set up crypto service
var storageRootDir = "./KeyValuePairs";
Directory.CreateDirectory(storageRootDir);
File.WriteAllText($"{storageRootDir}/.gitignore", "*");
Environment.SetEnvironmentVariable(KeyValesOnFileSystemService.StorageRootEnvVarName, $"{storageRootDir}");
ILogger<KeyValesOnFileSystemService> serviceLogger = factory.CreateLogger<KeyValesOnFileSystemService>();

KeyValesOnFileSystemService keyValesOnFileSystemService = new KeyValesOnFileSystemService(serviceLogger);

var keyValuePairs = new Dictionary<string, string>();
for (int i = 0; i < 20; i++)
{
    keyValuePairs.Add($"Key {i}", $"Value {i}");
}

{
    foreach(var kvPair in keyValuePairs)
    {
        keyValesOnFileSystemService.Store(kvPair.Key.AsBytes(), kvPair.Value.AsBytes());
    }
    foreach(var kvPair in keyValuePairs)
    {
        var retrieveSuccess = keyValesOnFileSystemService.TryRetrieve(kvPair.Key.AsBytes(), out var retrievedBytes);
        if(!retrieveSuccess)
        {
            logger.LogError($"Did not find value for key \"{kvPair.Key}\" in stored, this makes no sense!");
            continue;
        }
        var retrieved = retrievedBytes.AsString();
        // Why not just unit test with asserts? Seeing is believing, at least it makes presenting examples easier
        var expected = retrieved == kvPair.Value ? "the expected value" : $"not the expected value, got \"{retrieved}\" but expected \"{kvPair.Value}\"";
        logger.LogInformation($"When retrieving value for {kvPair.Key}, got value {retrieved}, which was {expected}");
    }
}
{
    logger.LogInformation("Re-storing all the pairs, to demo that write is idempotent, and doesn't affect retrieval");
    foreach(var kvPair in keyValuePairs)
    {
        keyValesOnFileSystemService.Store(kvPair.Key.AsBytes(), kvPair.Value.AsBytes());
    }
    foreach(var kvPair in keyValuePairs)
    {
        var retrieveSuccess = keyValesOnFileSystemService.TryRetrieve(kvPair.Key.AsBytes(), out var retrievedBytes);
        if(!retrieveSuccess)
        {
            logger.LogError($"Did not find value for key \"{kvPair.Key}\" in stored, this makes no sense!");
            continue;
        }
        var retrieved = retrievedBytes.AsString();
        // Why not just unit test with asserts? Seeing is believing, at least it makes presenting examples easier
        var expected = retrieved == kvPair.Value ? "the expected value" : $"not the expected value, got \"{retrieved}\" but expected \"{kvPair.Value}\"";
        logger.LogInformation($"When retrieving value for {kvPair.Key}, got value {retrieved}, which was {expected}");
    }
}

{
    // Anonymous scope so that I can re-use simple variable names
    logger.LogInformation("Deleting existing value");
    var kvPairToDelete = keyValuePairs.First();
    var deleteResult = keyValesOnFileSystemService.Delete(kvPairToDelete.Key.AsBytes());
    var retrieveResult = keyValesOnFileSystemService.TryRetrieve(kvPairToDelete.Key.AsBytes(), out var retrievedBytes);
    if(!retrieveResult)
    {
        logger.LogInformation($"Retrieve of deleted key \"{kvPairToDelete.Key}\" reported nothing to retrieve, as expected");
        if(retrievedBytes.Length != 0)
        {
            logger.LogError($"But the value was not empty, which is unexpected! The value was {retrievedBytes.AsString()}");
        }
    }
    else
    {
        logger.LogError($"Retrieve of deleted key \"{kvPairToDelete.Key}\" found something to retrieve, wtf?!");
        if(retrievedBytes.Length == 0)
        {
            logger.LogInformation("The retrieved value was empty");
        }
        else
        {
            logger.LogInformation($"The retrieved value was {retrievedBytes.AsString()}");
        }
    }
}

{
    // Anonymous scope so that I can re-use simple variable names
    logger.LogInformation("Deleting value not present");
    var unusedKey = "This string has not been used as a key";
    var deleteResult = keyValesOnFileSystemService.Delete(unusedKey.AsBytes());
    var retrieveResult = keyValesOnFileSystemService.TryRetrieve(unusedKey.AsBytes(), out var retrievedBytes);
    if(!retrieveResult)
    {
        logger.LogInformation($"Retrieve of unused key \"{unusedKey}\" reported nothing to retrieve, as expected");
        if(retrievedBytes.Length != 0)
        {
            logger.LogError($"But the value was not empty, which is unexpected! The value was {Convert.ToHexString(retrievedBytes)}");
        }
    }
    else
    {
        logger.LogError($"Retrieve of unused key \"{unusedKey}\" found something to retrieve, wtf?!");
        if(retrievedBytes.Length == 0)
        {
            logger.LogInformation("The retrieved value was empty");
        }
        else
        {
            logger.LogInformation($"The retrieved value was {Convert.ToHexString(retrievedBytes)}");
        }
    }
}

{
    var collidingPairs = new Dictionary<string, string>
    {
        {"acclamatory",     "Bozrah's"},
        {"alleleu",         "Rolodex"},
        {"bermensch",       "Audras"},
        {"bromoiodide",     "Linc's"},
        {"cowhouses",       "acrocarp's"},
        {"eminency's",      "Kelcie's"},
        {"eminency",        "Kelcie"},
        {"ficus's",         "Arcas"},
        {"fiend's",         "Gitas"},
        {"gigmanism",       "dyn"},
        {"gnu",             "codding"},
        {"graceless",       "Bigfooted"},
        {"Ilysa's",         "Endamoebidae's"},
        {"Ilysa",           "Endamoebidae"},
        {"kelpwort's",      "favi"},
        {"kra",             "codders"},
        {"limitless's",     "Mitchells"},
        {"maggid",          "craquelure's"},
        {"meny",            "menthols"},
        {"morenosite",      "funerary"},
        {"nonfraudulent",   "Fontinalaceae's"},
        {"oolemmas",        "Deleon"},
        {"petfood",         "eisenhower"},
        {"pipsissewa's",    "doublehanded"},
        {"platycarpous",    "envoyship"},
        {"plumless",        "buckeroo"},
        {"precedential",    "fetishists"},
        {"prepituitary",    "penetration"},
        {"recoction",       "Harmothoe's"},
        {"regularizer's",   "Solenopsis"},
        {"saltish",         "Bersil's"},
        {"shuttlers",       "Rashida's"},
        {"slagging",        "Bridget"},
        {"superspecial",    "bimasty"},
        {"supplicavits",    "Halevy"},
        {"syllabicates",    "Schoolcraft's"},
        {"tantafflin",      "nucleosomal"},
        {"thalloid",        "droopingness"},
        {"tribase",         "Eastland's"},
        {"uncontorted",     "casewood"},
        {"unexigible",      "bronziest"},
        {"vanadinites",     "expelling"},
    };
    foreach(var collision in collidingPairs)
    {
        keyValesOnFileSystemService.Store(collision.Key.AsBytes(), $"Collides with \"{collision.Value}\"".AsBytes());
        keyValesOnFileSystemService.Store(collision.Value.AsBytes(), $"Collides with \"{collision.Key}\"".AsBytes());
    }

    foreach(var kvPair in collidingPairs)
    {
        var retrieveSuccess = keyValesOnFileSystemService.TryRetrieve(kvPair.Key.AsBytes(), out var retrievedBytes);
        if(!retrieveSuccess)
        {
            logger.LogError($"Did not find value for key \"{kvPair.Key}\" in stored, this makes no sense!");
            continue;
        }
        var retrieved = retrievedBytes.AsString();
        // Why not just unit test with asserts? Seeing is believing, at least it makes presenting examples easier
        var expected =  retrieved == $"Collides with \"{kvPair.Value}\"" ? "the expected value" : $"not the expected value, got [{retrieved}] but expected [{kvPair.Value}]";
        logger.LogInformation($"When retrieving value for [{kvPair.Key}], got value [{retrieved}], which was {expected}");
    }
}


public static class StringConversionExtension
{
    public static byte[] AsBytes(this string input)
    {
        return System.Text.Encoding.UTF8.GetBytes(input);
    }

    public static string AsString(this byte[] bytes)
    {
        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}


