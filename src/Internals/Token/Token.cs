using System.Runtime.ConstrainedExecution;

namespace internals.token {
    public class Token(TokenType type, string lexeme, object? literal, int line)
    {
        public readonly TokenType type = type;
        public readonly string lexeme = lexeme;
        public readonly object? literal = literal;
        public readonly int line = line;

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