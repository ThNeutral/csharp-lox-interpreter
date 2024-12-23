using internals.expr;
using internals.parser;
using internals.scanner;
using internals.token;

namespace internals.lox {
    class Lox {
        static public bool hadError;
        static public List<Token> ScanTokens(string filename) {
            var fileContents = File.ReadAllText(filename);

            Scanner scanner = new Scanner(fileContents);
            return scanner.ScanTokens();
        }
        static public Expr ParseTokens(List<Token> tokens) {
            Parser parser = new Parser(tokens);
            return parser.Parse();
        }
        static public void Error(Token token, string message) {
            if (token.type == TokenType.EOF) {
                ReportError(token.line, "at end", message);
            } else {
                ReportError(token.line, "at '" + token.lexeme + "'", message);
            }
        }
        static public void Error(int line, string message) {
            ReportError(line, "", message);
        }
        static private void ReportError(int line, string position, string message) {
            hadError = true;
            Console.Error.WriteLine("[line " + line.ToString() + "] Error " + position + ": " + message);
        }
    }
}