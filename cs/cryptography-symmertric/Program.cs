// Initial setup
using System;
using System.Text;
using Microsoft.Extensions.Logging;

using ILoggerFactory factory = LoggerFactory.Create(
    builder =>
    {
        builder.SetMinimumLevel(LogLevel.Information);
        builder.SetMinimumLevel(LogLevel.Debug);
        builder.AddConsole();
    });
ILogger logger = factory.CreateLogger("Symmetric crypto demo");

// Set up crypto service
ILogger<CryptoService> cryptoServiceLogger = factory.CreateLogger<CryptoService>();

var demoAesKeyBytes = System.Security.Cryptography.RandomNumberGenerator.GetBytes(32);
var demoAesKey = Convert.ToHexString(demoAesKeyBytes);
Environment.SetEnvironmentVariable(CryptoService.aesKeyEnvVarName, $"{demoAesKey}");

CryptoService cryptoService = new CryptoService(cryptoServiceLogger);

// Demo how it actually works
var originalPlainText = "This is a very plain message for demo purposes.";
var originalAsByteArray = System.Text.Encoding.UTF8.GetBytes(originalPlainText);

var encryptedBytes = cryptoService.Encrypt(originalAsByteArray);

var decryptedBytes = cryptoService.Decrypt(encryptedBytes);
var decryptedPlainText = System.Text.Encoding.UTF8.GetString(decryptedBytes);

StringBuilder sb = new StringBuilder();
sb.Append($"Original:                      \"{originalPlainText}\"\n");
sb.Append($"Original bytes (hex):          \"{Convert.ToHexString(originalAsByteArray)}\"\n");
sb.Append($"Encrypted bytes (hex):         \"{Convert.ToHexString(encryptedBytes)}\"\n");
sb.Append($"Encrypted bytes sans IV (hex): \"{Convert.ToHexString(encryptedBytes[16..])}\"\n");
sb.Append($"Decrypted bytes (hex):         \"{Convert.ToHexString(decryptedBytes)}\"\n");
sb.Append($"Decrypted:                     \"{decryptedPlainText}\"\n");
logger.LogInformation(sb.ToString());
