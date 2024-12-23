using System;
using System.IO;
using internals.exprgenerator;
using internals.cli;
using internals.scanner;
using internals.token;
using internals.expr;
using internals.astprinter;
using internals.lox;

if (args.Length < 1)
{
    Console.Error.WriteLine("Command was not provided. Possible commands: 'tokenize', 'generate_ast', 'parse'");
    Environment.Exit(1);
}

string command = args[0];

if (command != "tokenize" && command != "generate_ast" && command != "parse")
{
    Console.Error.WriteLine($"Unknown command: {command}. Available commands: tokenize|generate_ast|parse");
    Environment.Exit(1);
}

if (command == "generate_ast") {
    if (args.Length < 2) {
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

    Environment.Exit(0);
}
 
if (args.Length < 2) {
    Console.Error.WriteLine("Usage: interpreter.sh <tokenize|parse> <filename>");
    Environment.Exit(1);
}

string filename = args[1];
        
var tokens = Lox.ScanTokens(filename);
var expression = Lox.ParseTokens(tokens);

if (Lox.hadError) {
    Environment.Exit(65);
}

Console.WriteLine(new AstPrinter().Print(expression));