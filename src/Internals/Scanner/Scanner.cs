using internals.lox;
using internals.token;

namespace internals.scanner {
    public class Scanner {
        private readonly string source;
        private int start;
        private int current;
        private int line;
        private readonly List<Token> tokens;
        public bool hasError;
        public Scanner(string source) {
            this.source = source;

            tokens = new List<Token>();
            start = 0;
            current = 0;
            line = 1;

            hasError = false;
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
                case '!': {
                    AddLongToken('=', TokenType.BANG_EQUAL, TokenType.BANG);
                    break;
                }
                case '=': {
                    AddLongToken('=', TokenType.EQUAL_EQUAL, TokenType.EQUAL);
                    break;
                }
                case '<': {
                    AddLongToken('=', TokenType.LESS_EQUAL, TokenType.LESS);
                    break;
                }
                case '>': {
                    AddLongToken('=', TokenType.GREATER_EQUAL, TokenType.GREATER);
                    break;
                }
                case '/': {
                    if (Match('/')) {
                        while (Peek() != '\n' && !IsAtEnd()) {
                            current += 1;
                        }
                    } else {
                        AddToken(TokenType.SLASH);
                    }
                    break;
                }
                case '"': {
                    ParseString();
                    break;
                }
                case '\n': {
                    line += 1;
                    break;
                }
                case ' ':
                case '\r':
                case '\t':
                    break;
                default: {
                    if (IsDigit(token)) {
                        ParseNumber();
                    } else if (IsAlpha(token)) {
                        ParseIndetifier();
                    } else {
                        Error(line, "Unexpected character: " + token.ToString());

                    }
                    break;
                }
            }
        }
        private void AddToken(TokenType type) {
            AddToken(type, null);
        }
        private void AddToken(TokenType type, object? literal) {
            string lexeme = source[start..current];
            tokens.Add(new Token(type, lexeme, literal, line));
        }
        private void AddLongToken(char condition, TokenType ifTrue, TokenType ifFalse) {
            if (Peek() != condition) {
                AddToken(ifFalse);
                return;
            }

            current += 1;
            AddToken(ifTrue); 
        }
        private void ParseString() {
            while (true) {
                if (IsAtEnd()) {
                    Error(line, "Unterminated string."); 
                    break;
                };

                if (Match('"')) {
                    AddToken(TokenType.STRING, source[(start+1)..(current-1)]);
                    break;
                }
                
                current += 1;
            }
        }
        private void ParseNumber() {
            while (true) {
                bool isNumberChar = IsDigit(Peek()) || Peek() == '.';
                bool isTrailingPoint = Peek() == '.' && IsAtEnd(1);
                if (!isNumberChar || isTrailingPoint) {
                    AddToken(TokenType.NUMBER, double.Parse(source[start..current]));
                    break;
                }
                
                current += 1;
            }
        }
        private void ParseIndetifier() {
            while(true) {
                if (!IsAlphaNumeric(Peek())) {
                    string literal = source[start..current];
                    TokenType type = TokenType.IDENTIFIER;
                    if (keywords.TryGetValue(literal, out TokenType value)) {
                        type = value;
                    }
                    AddToken(type);
                    break;
                }

                current += 1;
            }
        }
        private bool IsAtEnd() {
            return current >= source.Length;
        }
        private bool IsAtEnd(int offset) {
            return current + offset >= source.Length;
        }
        private void Error(int line, string message) {
            hasError = true;
            Lox.Error(line, message);
        }
        private char Peek() {
            if (IsAtEnd()) return '\0';
            return source[current];
        }
        private bool Match(char ch) {
            if (IsAtEnd()) return false;
            if (source[current] != ch) return false;

            current += 1;
            return true;
        }
        private bool IsDigit(char c) {
            return c >= '0' && c <= '9';
        }
        private bool IsAlpha(char c) {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
        }

        private bool IsAlphaNumeric(char c) {
            return IsAlpha(c) || IsDigit(c);
        }

        private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType> {
            { "and", TokenType.AND },
            { "class", TokenType.CLASS },
            { "else", TokenType.ELSE },
            { "false", TokenType.FALSE },
            { "for", TokenType.FOR },
            { "fun", TokenType.FUN },
            { "if", TokenType.IF },
            { "nil", TokenType.NIL },
            { "or", TokenType.OR },
            { "print", TokenType.PRINT },
            { "return", TokenType.RETURN },
            { "super", TokenType.SUPER },
            { "this", TokenType.THIS },
            { "true", TokenType.TRUE },
            { "var", TokenType.VAR },
            { "while", TokenType.WHILE }
        };
    }
}