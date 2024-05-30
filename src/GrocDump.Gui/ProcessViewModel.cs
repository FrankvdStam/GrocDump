using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrocDump.Gui
{
    public class ProcessViewModel(string name, int id, bool hasWindow)
    {
        public ProcessViewModel(Process process) : this(process.ProcessName, process.Id, process.HasActiveWindow()){}

        public string Name { get; set; } = name;
        public string NameLower { get; } = name.ToLower();
        public int Id { get; set; } = id;
        public bool HasWindow { get; set; } = hasWindow;
    }
}
