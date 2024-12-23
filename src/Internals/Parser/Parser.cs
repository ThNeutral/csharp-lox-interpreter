using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using internals.autogenerated;
using internals.errors;
using internals.lox;
using internals.token;

namespace internals.parser {
    public class Parser {
        private class ParseError : SystemException {}
        private readonly List<Token> tokens;
        private int current;

        public Parser(List<Token> tokens) {
            this.tokens = tokens;
            current = 0;
        }
        public List<Stmt> Parse() {
            List<Stmt> statements = [];

            while(!IsAtEnd()) {
                statements.Add(Declaration());
            }

            return statements;
            
        }
        private Stmt Declaration() {
            try {
                if (Match(TokenType.VAR)) return VarDeclaration();
                return Statement();
            } catch (ParseError) {
                Synchronize();
                return null;
            }
        }
        private Stmt VarDeclaration() {
            Token name = Consume(TokenType.IDENTIFIER, "Expect variable name.");

            Expr initializer = null;
            if (Match(TokenType.EQUAL)) {
                initializer = Expression();
            }

            Consume(TokenType.SEMICOLON, "Expect ';' after varibale declaration.");
            return new Stmt.Var(name, initializer);
        }
        private Stmt Statement() {
            if (Match(TokenType.PRINT)) return PrintStatement();
            if (Match(TokenType.LEFT_BRACE)) return new Stmt.Block(Block());

            return ExpressionStatement();
        }
        private Stmt PrintStatement() {
            Expr value = Expression();
            Consume(TokenType.SEMICOLON, "Expect ';' after value");
            return new Stmt.Print(value);
        }
        private Stmt ExpressionStatement() {
            Expr expr = Expression();
            Consume(TokenType.SEMICOLON, "Expect ';' after expression");
            return new Stmt.Expression(expr);
        }
        private List<Stmt> Block() {
            List<Stmt> statements = [];

            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd()) {
                statements.Add(Declaration());
            }

            Consume(TokenType.RIGHT_BRACE, "Expect '}' after block.");
            return statements;
        }
        private Expr Expression() {
            return Assignment();
        }
        private Expr Assignment() {
            Expr expr = Equality();

            if (Match(TokenType.EQUAL)) {
                Token equals = Previous();
                Expr value = Assignment();

                if (expr is Expr.Variable) {
                    Token name = ((Expr.Variable)expr).name;
                    return new Expr.Assign(name, value);
                }

                Error(equals, "Invalid assignment target.");
            }

            return expr;
        }
        private Expr Equality() {
            Expr expr = Comparison();

            while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL)) {
                Token operatorToken = Previous();
                Expr right = Comparison();
                expr = new Expr.Binary(expr, operatorToken, right);
            }

            return expr;
        }
        private Expr Comparison() {
            Expr expr = Term();

            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL)) {
                Token operatorToken = Previous();
                Expr right = Term();
                expr = new Expr.Binary(expr, operatorToken, right);
            }

            return expr;
        }
        private Expr Term() {
            Expr expr = Factor();

            while (Match(TokenType.MINUS, TokenType.PLUS)) {
                Token operatorToken = Previous();
                Expr right = Factor();
                expr = new Expr.Binary(expr, operatorToken, right);
            }

            return expr;
        }
        private Expr Factor() {
            Expr expr = Unary();

            while (Match(TokenType.SLASH, TokenType.STAR)) {
                Token operatorToken = Previous();
                Expr right = Unary();
                expr = new Expr.Binary(expr, operatorToken, right);
            }

            return expr;
        }
        private Expr Unary() {
            if (Match(TokenType.BANG, TokenType.MINUS)) {
                Token operatorToken = Previous();
                Expr right = Unary();
                return new Expr.Unary(operatorToken, right);
            }

            return Primary();
        }
          private Expr Primary() {
            if (Match(TokenType.FALSE)) return new Expr.Literal(false);
            if (Match(TokenType.TRUE)) return new Expr.Literal(true);
            if (Match(TokenType.NIL)) return new Expr.Literal(null);

            if (Match(TokenType.NUMBER, TokenType.STRING)) {
                return new Expr.Literal(Previous().literal);
            }

            if (Match(TokenType.IDENTIFIER)) {
                return new Expr.Variable(Previous());
            }

            if (Match(TokenType.LEFT_PAREN)) {
                Expr expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return new Expr.Grouping(expr);
            }

            throw Error(Peek(), "Expect expression.");
        }
        private Token Consume(TokenType type, string message) {
            if (Check(type)) return Advance();

            throw Error(Peek(), message);
        }
        private ParseError Error(Token token, string message) {
            Lox.Error(token, message);
            return new ParseError();
        }
        private void Synchronize() {
            Advance();

            while(!IsAtEnd()) {
                if (Previous().type == TokenType.SEMICOLON) return;

                switch (Peek().type) {
                    case TokenType.CLASS:
                    case TokenType.FUN:
                    case TokenType.VAR:
                    case TokenType.FOR:
                    case TokenType.IF:
                    case TokenType.WHILE:
                    case TokenType.PRINT:
                    case TokenType.RETURN:
                        return;
                }

                Advance();
            }
        }
        private bool Match(params TokenType[] types) {
            foreach(var type in types) {
                if (Check(type)) {
                    Advance();
                    return true;
                }
            }

            return false;
        }
        private bool Check(TokenType type) {
            if (IsAtEnd()) return false;
            return Peek().type == type;
        }
        private Token Advance() {
            if (!IsAtEnd()) current ++;
            return Previous();
        }
        private bool IsAtEnd() {
            return Peek().type == TokenType.EOF;
        }
        private Token Peek() {
            return tokens[current];
        }
        private Token Previous() {
            return tokens[current - 1];
        }
    }
}