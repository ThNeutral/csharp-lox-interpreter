using System.Runtime.ConstrainedExecution;

namespace internals.token {
    public class Token {
        private readonly TokenType type;
        private readonly string lexeme;
        private readonly object? literal;
        private readonly int line;
        public Token(TokenType type, string lexeme, object? literal, int line) {
            this.type = type;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }
        public override string ToString() {
            var literal = this.literal == null ? "null" : this.literal; 
            return this.type + " " + this.lexeme + " " + literal;
        }
    }
}