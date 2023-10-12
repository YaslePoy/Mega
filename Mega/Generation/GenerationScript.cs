using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Generation
{
    internal class GenerationScript
    {
        public readonly string Programm;
        public GenerationScript(string programm)
        {
            Programm = programm;
        }
        public void Execute<T>(CelluralAutomaton<T> automaton)
        {

        }
    }
    public class ScriptAction : System.Attribute
    {
        public readonly string Code;
        public ScriptAction(string command)
        {
            Code = command;
        }
    }
}
