using internals.lox;

if (args.Length == 0) {
    Lox.HandleREPL();
    Environment.Exit(0);
}

string filename = args[0];
var fileContents = File.ReadAllText(filename);

var tokens = Lox.ScanTokens(fileContents);
var expression = Lox.ParseTokens(tokens);

if (Lox.hadSyntaxError) {
    Environment.Exit(65);
}

Lox.Interpret(expression);

if (Lox.hadRuntimeError) {
    Environment.Exit(70);
}