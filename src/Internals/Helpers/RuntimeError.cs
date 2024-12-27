using internals.token;

namespace internals.helpers {
    class RuntimeError : SystemException {
        public readonly Token token;
        public RuntimeError(Token token, string message) : base(message) {
            this.token = token;
        }
    }
}