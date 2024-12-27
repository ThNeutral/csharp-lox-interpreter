using internals.helpers;
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
        public object GetAt(int distance, string name) {
            return Ancestor(distance).values[name];
        }
        public Environment Ancestor(int distance) {
            Environment environment = this;

            for (int i = 0; i < distance; i++) {
                environment = environment.enclosing;
            }

            return environment;
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
        internal void AssignAt(int distance, Token name, object value) {
            Ancestor(distance).values[name.lexeme] = value;
        }
    } 
}