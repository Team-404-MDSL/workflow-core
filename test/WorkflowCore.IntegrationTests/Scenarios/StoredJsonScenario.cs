using FluentAssertions;
using System;
using WorkflowCore.Models;
using FluentAssertions;
using WorkflowCore.Services.DefinitionStorage;
using WorkflowCore.TestAssets.DataTypes;
using WorkflowCore.Testing;
using Xunit;

namespace WorkflowCore.IntegrationTests.Scenarios
{
    public class StoredJsonScenario : JsonWorkflowTest
    {
        public StoredJsonScenario()
        {
            Setup();
        }

        [Fact(DisplayName = "Execute branch 1")]
        public void should_execute_branch1()
        {
            var workflowId = StartWorkflow(TestAssets.Utils.GetTestDefinitionJson(), new CounterBoard() { Flag1 = true, Flag2 = true, Flag3 = true });
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            var data = GetData<CounterBoard>(workflowId);
            UnhandledStepErrors.Should().BeEmpty();
            GetStatus(workflowId).Should().Be(WorkflowStatus.Complete);
            data.Counter1.Should().Be(1);
            data.Counter2.Should().Be(1);
            data.Counter3.Should().Be(1);
            data.Counter4.Should().Be(1);
            data.Counter5.Should().Be(0);
            data.Counter6.Should().Be(1);
            data.Counter7.Should().Be(1);
            data.Counter8.Should().Be(0);
        }

        [Fact(DisplayName = "Execute branch 2")]
        public void should_execute_branch2()
        {
            var workflowId = StartWorkflow(TestAssets.Utils.GetTestDefinitionJson(), new CounterBoard() { Flag1 = true, Flag2 = true, Flag3 = false });
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            var data = GetData<CounterBoard>(workflowId);
            UnhandledStepErrors.Should().BeEmpty();
            GetStatus(workflowId).Should().Be(WorkflowStatus.Complete);
            data.Counter1.Should().Be(1);
            data.Counter2.Should().Be(1);
            data.Counter3.Should().Be(1);
            data.Counter4.Should().Be(1);
            data.Counter5.Should().Be(0);
            data.Counter6.Should().Be(1);
            data.Counter7.Should().Be(0);
            data.Counter8.Should().Be(1);
        }

        [Fact]
        public void should_execute_json_workflow_with_dynamic_data()
        {
            var initialData = new DynamicData
            {
                ["Flag1"] = true,
                ["Flag2"] = true,
                ["Counter1"] = 0,
                ["Counter2"] = 0,
                ["Counter3"] = 0,
                ["Counter4"] = 0,
                ["Counter5"] = 0,
                ["Counter6"] = 0
            };

            var workflowId = StartWorkflow(TestAssets.Utils.GetTestDefinitionDynamicJson(), initialData);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            var data = GetData<DynamicData>(workflowId);
            UnhandledStepErrors.Should().BeEmpty();
            GetStatus(workflowId).Should().Be(WorkflowStatus.Complete);
            data["Counter1"].Should().Be(1);
            data["Counter2"].Should().Be(1);
            data["Counter3"].Should().Be(1);
            data["Counter4"].Should().Be(1);
            data["Counter5"].Should().Be(0);
            data["Counter6"].Should().Be(1);
        }

        [Fact]
        public void should_execute_json_workflow_with_nullable_step_properties()
        {
            var initialData = new DynamicData
            {
                ["date"] = "2020-05-22T11:20:37.034Z",
                ["Counter1"] = 0,
            };

            var workflowId = StartWorkflow(TestAssets.Utils.GetTestDefinitionJsonNullableProperty(), initialData);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(10));

            var data = GetData<DynamicData>(workflowId);
            UnhandledStepErrors.Should().BeEmpty();
            GetStatus(workflowId).Should().Be(WorkflowStatus.Complete);
            data["Counter1"].Should().Be(1);
        }

        [Fact]
        public void should_execute_json_workflow_with_null_step_properties()
        {
            var initialData = new DynamicData
            {
                ["date"] = null,
                ["Counter1"] = 0,
            };

            var workflowId = StartWorkflow(TestAssets.Utils.GetTestDefinitionJsonNullableProperty(), initialData);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(10));

            var data = GetData<DynamicData>(workflowId);
            UnhandledStepErrors.Should().BeEmpty();
            GetStatus(workflowId).Should().Be(WorkflowStatus.Complete);
            data["Counter1"].Should().Be(1);
        }

        [Fact]
        public void should_execute_json_workflow_and_evaluate_nested_step_properties()
        {
            var initialData = new DynamicData
            {
                ["dob"] = new DateTime(2020, 06, 15)
            };

            var workflowId = StartWorkflow(TestAssets.Utils.GetTestDefinitionJsonNestedProperty(), initialData);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(10));

            var data = GetData<DynamicData>(workflowId);
            UnhandledStepErrors.Should().BeEmpty();
            GetStatus(workflowId).Should().Be(WorkflowStatus.Complete);
            var expected = new Person
            {
                Name = "Test Name",
                NestedData = new NestedData
                {
                    DoB = new DateTime(2020, 06, 15)
                }
            };
            data["Person"].ShouldBeEquivalentTo(expected);
        }
    }
}
