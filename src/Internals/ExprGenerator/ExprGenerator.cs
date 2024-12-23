
namespace internals.exprgenerator {
    public class ExprGenerator {
        public static void Generate(string outDir, string baseName, List<string> types) {
            Directory.CreateDirectory(outDir);
            string path = outDir + "/" + baseName + ".cs";
            using (var writer = new StreamWriter(path)) {
                writer.WriteLine("using internals.token;");
                writer.WriteLine("");
                writer.WriteLine("namespace internals.expr {");
                writer.WriteLine("\tpublic abstract class " + baseName + " {");
                writer.WriteLine("\t\tpublic abstract R Accept<R>(IVisitor<R> visitor);");
                DefineVisitor(writer, baseName, types);
                foreach (var type in types) {
                    string className = type.Split(":")[0].Trim();
                    string fields = type.Split(":")[1].Trim();
                    DefineType(writer, baseName, className, fields);
                }
                writer.WriteLine("\t}");
                writer.WriteLine("}");
            }
        }

        private static void DefineVisitor(StreamWriter writer, string baseName, List<string> types)
        {
            writer.WriteLine("\t\tpublic interface IVisitor<R> {");
            foreach (string type in types) {
                string typeName = type.Split(":")[0].Trim();
                writer.WriteLine("\t\t\tpublic R Visit" + typeName + baseName + "(" + typeName + " " + baseName.ToLower() + ");");
            }
            writer.WriteLine("\t\t}");
        }

        private static void DefineType(StreamWriter writer, string baseName, string className, string fields)
        {
            writer.WriteLine("\t\tpublic class " + className + " : " + baseName + " {");
            foreach (var field in fields.Split(", ")) {
                writer.WriteLine("\t\t\tpublic readonly " + field + ";");
            }
            writer.WriteLine("\t\t\tpublic " + className + "(" + fields + ") {");
            foreach (var field in fields.Split(", ")) {
                string name = field.Split(" ")[1];
                writer.WriteLine("\t\t\t\tthis." + name + " = " + name + ";");
            }
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tpublic override R Accept<R>(IVisitor<R> visitor) {");
            writer.WriteLine("\t\t\t\treturn visitor.Visit" + className + baseName + "(this);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
        }
    }
}