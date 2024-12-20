using internals.token;

namespace Internals.Scanner {
    public class Scanner {
        private readonly string source;
        private int start;
        private int current;
        private int line;
        private readonly List<Token> tokens;
        public Scanner(string source) {
            this.source = source;

            tokens = new List<Token>();
            start = 0;
            current = 0;
            line = 1;
        }
        public List<Token> ScanTokens() {
            while (!IsAtEnd()) {
                start = current;
                ParseNextToken(); 
            }

            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        private void ParseNextToken()
        {
            char token = source[current];
            current += 1;
            switch (token) {
                case '(': {
                    AddToken(TokenType.LEFT_PAREN);
                    break;
                }
                case ')': {
                    AddToken(TokenType.RIGHT_PAREN);
                    break;
                }
                case '{': {
                    AddToken(TokenType.LEFT_BRACE);
                    break;
                }
                case '}': {
                    AddToken(TokenType.RIGHT_BRACE);
                    break;
                }
                case ',': {
                    AddToken(TokenType.COMMA);
                    break;
                }
                case '.': {
                    AddToken(TokenType.DOT);
                    break;
                }
                case '-': {
                    AddToken(TokenType.MINUS);
                    break;
                }
                case '+': {
                    AddToken(TokenType.PLUS);
                    break;
                }
                case ';': {
                    AddToken(TokenType.SEMICOLON);
                    break;
                }
                case '*': {
                    AddToken(TokenType.STAR);
                    break;
                }
                default: {
                    throw new ArgumentException("Unknown token during parsing: " + token.ToString());
                }
            }
        }

        private void AddToken(TokenType type) {
            AddToken(type, null);
        }
        private void AddToken(TokenType type, object? literal) {
            string lexeme = source.Substring(start, current - start);
            tokens.Add(new Token(type, lexeme, literal, line));
        }

        private bool IsAtEnd() {
            return current >= source.Length;
        }
    }
}