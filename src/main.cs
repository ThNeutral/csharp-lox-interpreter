using internals.lox;

if (args.Length == 0) {
    Lox.HandleREPL();
    Environment.Exit(0);
}

string filename = args[0];
var fileContents = File.ReadAllText(filename);

var tokens = Lox.ScanTokens(fileContents);
var statements = Lox.ParseTokens(tokens);

if (Lox.hadError) {
    Environment.Exit(65);
}

Lox.Resolve(statements);
if (Lox.hadError) {
    Environment.Exit(70);
}

Lox.Interpret(statements);

if (Lox.hadError) {
    Environment.Exit(70);
}