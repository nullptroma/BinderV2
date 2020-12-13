using InterpreterScripts.InterpretationFunctions.Standart;
using InterpreterScripts.InterpretationScriptData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.InterpretationFunctions
{
    class GetNowDateTime : IInterpreterFunction
    {
        public string Name { get { return "GetNowDateTime"; } }
        public string Description { get { return "GetNowDateTime() - возвращает текущие дату и время, обращение к ним через \".\". Чтобы узнать подробнее используй GetInterpretationInfo(). "; } }
        public string GroupName { get { return "DateTime"; } }
        public FuncType ReturnType { get { return FuncType.Other; } }


        public Task<object> GetResult(object[] parameters, InterpretationData data)
        {
            return Task.Run(new Func<object>(() =>
            {
                InterpretationData nowDateTime = new InterpretationData() { Stopper = data.Stopper };

                DateTime d = DateTime.Now;
                nowDateTime.Vars["Year"] = d.Year;
                nowDateTime.Vars["Month"] = d.Month;
                nowDateTime.Vars["Day"] = d.Day;
                nowDateTime.Vars["Hour"] = d.Hour;
                nowDateTime.Vars["Minute"] = d.Minute;
                nowDateTime.Vars["Second"] = d.Second;
                nowDateTime.Vars["Millisecond"] = d.Millisecond;

                return nowDateTime;
            }));
        }
    }
}
