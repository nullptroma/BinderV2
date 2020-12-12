using InterpreterScripts.InterpretationFunctions.Standart;
using InterpreterScripts.InterpretationScriptData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InterpreterScripts.InterpretationFunctions
{
    class Speak : IInterpreterFunction
    {
        public string Name { get { return "Speak"; } }
        public string Description { get { return "Speak(string text, int volume, int rate, *string path) - превращает текст в речь. Если передать путь - сохранит фразу в файл."; } }
        public string GroupName { get { return "Speech"; } }
        public FuncType ReturnType { get { return FuncType.Boolean; } }


        public Task<object> GetResult(string[] parameters, InterpretationData data)
        {
            return Task.Run(new Func<object>(() =>
            {
                string text = parameters[0];
                int volume = (int)Interpreter.ExecuteCommand(parameters[1], data);
                volume = volume < 0 ? 0 : volume > 100 ? 100 : volume;
                int rate = (int)Interpreter.ExecuteCommand(parameters[2], data);
                rate = rate < -10 ? -10 : rate > 10 ? 10 : rate;

                SpeechSynthesizer ss = new SpeechSynthesizer() { Volume = volume, Rate = rate };
                if (parameters.Length > 3)
                    ss.SetOutputToWaveFile(parameters[3]);

                var t = ss.SpeakAsync(text);
                while (!t.IsCompleted && !data.Stopper.IsStopped)
                    Thread.Sleep(50);
                ss.Dispose();
                return null;
            }));
        }
    }
}
