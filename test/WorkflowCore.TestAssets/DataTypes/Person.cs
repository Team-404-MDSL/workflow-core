using System;

namespace WorkflowCore.TestAssets.DataTypes
{
    public class Person
    {
        public string Name { get; set; }
        public NestedData NestedData { get; set; }
    }

    public class NestedData
    {
        public DateTime DoB { get; set; }
    }
}
