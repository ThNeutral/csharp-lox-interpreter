using internals.errors;
using internals.token;

namespace internals.environment {
    public class Environment {
        private readonly Dictionary<string, object> values = [];
        public readonly Environment enclosing;
        public Environment() {
            enclosing = null;
        }
        public Environment(Environment enclosing) {
            this.enclosing = enclosing;
        }
        public void Define(string name, object value) {
            values[name] = value;
        }
        public object Get(Token name) {
            if (values.TryGetValue(name.lexeme, out object value)) {
                if (value == null) throw new RuntimeError(name, "Tried accessing uninitialized variable '" + name.lexeme  + "'.");
                return value;
            }

            if (enclosing != null) return enclosing.Get(name);

            throw new RuntimeError(name, "Undefined variable '" + name.lexeme + "'.");
        }
        public void Assign(Token name, object value) {
            if (values.ContainsKey(name.lexeme)) {
                values[name.lexeme] = value;
                return;
            }

            if (enclosing != null) {
                enclosing.Assign(name, value);
                return;
            }

            throw new RuntimeError(name, "Assignment to undefined variable '" + name.lexeme + "'.");
        }
    } 
}