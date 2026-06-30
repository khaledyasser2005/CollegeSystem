using System;
using Microsoft.Extensions.Configuration;
using System.IO;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile("appsettings.Development.json", optional: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true);

var config = builder.Build();
Console.WriteLine("ApiKey from config: " + config["Gemini:ApiKey"]);
Console.WriteLine("Model from config: " + config["Gemini:Model"]);
