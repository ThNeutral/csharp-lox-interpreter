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
            string literalString;

            if (this.literal == null) 
            {
                literalString = "null";
            } 
            else if (this.literal is int || this.literal is long || this.literal is float || this.literal is double) 
            {
                double number = Convert.ToDouble(this.literal);
                literalString = number % 1 == 0 ? $"{number:F1}" : $"{number}";
            } 
            else 
            {
                literalString = this.literal.ToString();
            }

            return $"{this.type} {this.lexeme} {literalString}";
        }
    }
}