using internals.token;

namespace internals.autogenerated {
    public abstract class Expr {
        public abstract R Accept<R>(IVisitor<R> visitor);
        public interface IVisitor<R> {
            R VisitAssignExpr(Assign expr);
            R VisitBinaryExpr(Binary expr);
            R VisitCallExpr(Call expr);
            R VisitGroupingExpr(Grouping expr);
            R VisitLiteralExpr(Literal expr);
            R VisitLogicalExpr(Logical expr);
            R VisitUnaryExpr(Unary expr);
            R VisitVariableExpr(Variable expr);
            R VisitLambdaExpr(Lambda expr);
        }
        public class Assign : Expr {
            public readonly Token name;
            public readonly Expr value;

            public Assign(Token name, Expr value) {
                this.name = name;
                this.value = value;
            }

            public override R Accept<R>(IVisitor<R> visitor) {
                return visitor.VisitAssignExpr(this);
            }
        }
        public class Binary : Expr {
            public readonly Expr left;
            public readonly Token operatorToken;
            public readonly Expr right;

            public Binary(Expr left, Token operatorToken, Expr right) {
                this.left = left;
                this.operatorToken = operatorToken;
                this.right = right;
            }

            public override R Accept<R>(IVisitor<R> visitor) {
                return visitor.VisitBinaryExpr(this);
            }
        }
        public class Call : Expr {
            public readonly Expr callee;
            public readonly Token paren;
            public readonly List<Expr> arguments;

            public Call(Expr callee, Token paren, List<Expr> arguments) {
                this.callee = callee;
                this.paren = paren;
                this.arguments = arguments;
            }

            public override R Accept<R>(IVisitor<R> visitor) {
                return visitor.VisitCallExpr(this);
            }
        }
        public class Grouping : Expr {
            public readonly Expr expression;

            public Grouping(Expr expression) {
                this.expression = expression;
            }

            public override R Accept<R>(IVisitor<R> visitor) {
                return visitor.VisitGroupingExpr(this);
            }
        }
        public class Literal : Expr {
            public readonly object value;

            public Literal(object value) {
                this.value = value;
            }

            public override R Accept<R>(IVisitor<R> visitor) {
                return visitor.VisitLiteralExpr(this);
            }
        }
        public class Logical : Expr {
            public readonly Expr left;
            public readonly Token operatorToken;
            public readonly Expr right;

            public Logical(Expr left, Token operatorToken, Expr right) {
                this.left = left;
                this.operatorToken = operatorToken;
                this.right = right;
            }

            public override R Accept<R>(IVisitor<R> visitor) {
                return visitor.VisitLogicalExpr(this);
            }
        }
        public class Unary : Expr {
            public readonly Token operatorToken;
            public readonly Expr right;

            public Unary(Token operatorToken, Expr right) {
                this.operatorToken = operatorToken;
                this.right = right;
            }

            public override R Accept<R>(IVisitor<R> visitor) {
                return visitor.VisitUnaryExpr(this);
            }
        }
        public class Variable : Expr {
            public readonly Token name;

            public Variable(Token name) {
                this.name = name;
            }

            public override R Accept<R>(IVisitor<R> visitor) {
                return visitor.VisitVariableExpr(this);
            }
        }
        public class Lambda : Expr {
            public readonly List<Token> arguments;
            public readonly List<Stmt> body;

            public Lambda(List<Token> arguments, List<Stmt> body) {
                this.arguments = arguments;
                this.body = body;
            }

            public override R Accept<R>(IVisitor<R> visitor) {
                return visitor.VisitLambdaExpr(this);
            }
        }
    }
}
