using System;
using System.IO;
using internals.exprgenerator;
using internals.cli;
using internals.scanner;
using internals.token;
using internals.expr;
using internals.astprinter;

if (args.Length < 1)
{
    Console.Error.WriteLine("Command was not provided. Possible commands: 'tokenize', 'generate_ast'");
    Environment.Exit(1);
}

string command = args[0];

if (command != "tokenize" && command != "generate_ast")
{
    Console.Error.WriteLine($"Unknown command: {command}");
    Environment.Exit(1);
}

switch (command) {
    case "tokenize": {
        if (args.Length < 2)
        {
            Console.Error.WriteLine("Usage: interpreter.sh tokenize <filename>");
            Environment.Exit(1);
        }

        string filename = args[1];
        string fileContents = File.ReadAllText(filename);

        Scanner scanner = new Scanner(fileContents);
        List<Token> tokens = scanner.ScanTokens();

        foreach (var token in tokens) {
            Console.WriteLine(token);
        }
            
        if (scanner.hasError) {
            Environment.Exit(65);
        }

        break;
    }
    case "generate_ast": {
        if (args.Length < 2)
        {
            Console.Error.WriteLine("Usage: interpreter.sh generate_ast <output directory>");
            Environment.Exit(1);
        }

        string outDir = args[1];
        ExprGenerator.Generate(outDir, "Expr", [
            "Binary   : Expr left, Token operatorToken, Expr right",
            "Grouping : Expr expression",
            "Literal  : object value",
            "Unary    : Token operatorToken, Expr right"
        ]);
        break;
    }
}