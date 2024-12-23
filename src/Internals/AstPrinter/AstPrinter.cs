using System.Text;
using internals.expr;

namespace internals.astprinter {
    public class AstPrinter : Expr.IVisitor<string> {
        public string Print(Expr expr) {
            return expr.Accept(this);
        }
        public string VisitBinaryExpr(Expr.Binary expr) {
            return Parenthesize(expr.operatorToken.lexeme, expr.left, expr.right);
        }

        public string VisitGroupingExpr(Expr.Grouping expr) {
            return Parenthesize("group", expr.expression);
        }

        public string VisitLiteralExpr(Expr.Literal expr) {
            if (expr.value == null) return "nil";
            return expr.value.ToString();
        }

        public string VisitUnaryExpr(Expr.Unary expr) {
            return Parenthesize(expr.operatorToken.lexeme, expr.right);
        }
        private string Parenthesize(string name, params Expr[] exprs) {
            StringBuilder builder = new();

            builder.Append('(').Append(name);
            foreach (Expr expr in exprs) {
                builder.Append(' ');
                builder.Append(expr.Accept(this));
            }
            builder.Append(')');

            return builder.ToString();
        }
    }
}