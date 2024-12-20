namespace internals.cli {
    class CLI {
        static public void Error(int line, ErrorTypes type, string? payload) {
            string message = "";
            switch (type) {
                case ErrorTypes.UNEXPECTED_CHARACTER: {
                    message = "Unexpected character: " + payload;
                    break;
                }
            }
            Console.Error.WriteLine("[line " + line.ToString() + "] Error: " + message);
        }
    }
}