using System;
using System.Text;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using TSQLLint.Lib.Rules.Interface;

namespace TSQLLint.Lib.Rules
{
    public class UnicodeStringRule : TSqlFragmentVisitor, ISqlRule
    {
        public string RULE_NAME => "unicode-string";

        public string RULE_TEXT => "Use of unicode characters in a non unicode string";

        private readonly Action<string, string, int, int> _errorCallback;

        public UnicodeStringRule(Action<string, string, int, int> errorCallback)
        {
            _errorCallback = errorCallback;
        }

        public override void Visit(StringLiteral node)
        {
            if (node.IsNational) return; // Already Unicode

            if (!IsAscii(node.Value))
            {
                _errorCallback(RULE_NAME, RULE_TEXT, node.StartLine, node.StartColumn);
            }
        }

        private static bool IsAscii(string part)
        {
            var asciiBytes = Encoding.ASCII.GetBytes(part);
            var partAscii = Encoding.ASCII.GetString(asciiBytes);
            return part == partAscii;
        }
    }
}
