using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IBehaviourControllable
{
    bool DebugMode { get; set; }
    bool IsExecutionPaused { get; set; }
}
