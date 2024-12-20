using internals.cli;
using internals.token;

namespace Internals.Scanner {
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
                case '\n': {
                    line += 1;
                    break;
                }
                default: {
                    Error(line, ErrorTypes.UNEXPECTED_CHARACTER, token.ToString());
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
            if (!Match(condition)) {
                AddToken(ifFalse);
                return;
            }

            current += 1;
            AddToken(ifTrue); 
        }
        private bool IsAtEnd() {
            return current >= source.Length;
        }
        private void Error(int line, ErrorTypes type, string? payload) {
            hasError = true;
            CLI.Error(line, type, payload);
        }
        private char Peek() {
            if (IsAtEnd()) return '\0';
            return source[current];
        }
        private bool Match(char ch) {
            if (IsAtEnd()) return false;
            return source[current] == ch;
        }
    }
}