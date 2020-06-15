using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.TestAssets.DataTypes;

namespace WorkflowCore.TestAssets.Steps
{
    public class Nested : StepBody
    {
        public Person Person { get; set; }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            return ExecutionResult.Next();
        }
    }
}
