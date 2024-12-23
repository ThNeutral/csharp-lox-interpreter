using internals.token;

namespace internals.expr {
	public abstract class Expr {
		public abstract R Accept<R>(IVisitor<R> visitor);
		public interface IVisitor<R> {
			public R VisitBinaryExpr(Binary expr);
			public R VisitGroupingExpr(Grouping expr);
			public R VisitLiteralExpr(Literal expr);
			public R VisitUnaryExpr(Unary expr);
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
	}
}
