using System;
using System.IO;
using internals.cli;
using Internals.Scanner;

if (args.Length < 2)
{
    Console.Error.WriteLine("Usage: ./your_program.sh tokenize <filename>");
    Environment.Exit(1);
}

string command = args[0];
string filename = args[1];

if (command != "tokenize")
{
    Console.Error.WriteLine($"Unknown command: {command}");
    Environment.Exit(1);
}

string fileContents = File.ReadAllText(filename);

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.Error.WriteLine("Logs from your program will appear here!");

if (!string.IsNullOrEmpty(fileContents))
{
    var scanner = new Scanner(fileContents);
    var tokens = scanner.ScanTokens();
    foreach (var token in tokens) {
        Console.WriteLine(token);
    }
    
    if (scanner.hasError) {
        Environment.Exit(65);
    }
}
else
{
    Console.WriteLine("EOF  null"); // Placeholder, remove this line when implementing the scanner
}