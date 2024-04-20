using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace EscapeRoom.QuestLogic.Tests
{
    /// <summary>
    /// Tests for <see cref="GroupCondition"/>
    /// </summary>
    [TestFixture(Category = "QuestLogic")]
    public class GroupConditionTests
    {
        [TestCase(new[] { true }, ExpectedResult = true)]
        [TestCase(new[] { false }, ExpectedResult = false)]
        [TestCase(new[] { true, true, true }, ExpectedResult = true)]
        [TestCase(new[] { true, false, true }, ExpectedResult = false)]
        [TestCase(new[] { false, false, true }, ExpectedResult = false)]
        [TestCase(new[] { false, false, false }, ExpectedResult = false)]
        [TestCase(new bool[] { }, ExpectedResult = false)]
        public bool AllConditionsTrue_ReturnsCorrectState(bool[] nestedConditionStates)
        {
            var conditions = CreateConditionMocks(nestedConditionStates);

            return GroupCondition.AllConditionsTrue(conditions);
        }

        [TestCase(new[] { true }, ExpectedResult = true)]
        [TestCase(new[] { false }, ExpectedResult = false)]
        [TestCase(new[] { true, true, true }, ExpectedResult = true)]
        [TestCase(new[] { true, false, true }, ExpectedResult = true)]
        [TestCase(new[] { false, false, true }, ExpectedResult = true)]
        [TestCase(new[] { false, false, false }, ExpectedResult = false)]
        [TestCase(new bool[] { }, ExpectedResult = false)]
        public bool AnyConditionTrue_ReturnsCorrectState(bool[] nestedConditionStates)
        {
            var conditions = CreateConditionMocks(nestedConditionStates);

            return GroupCondition.AnyConditionTrue(conditions);
        }
        
        [TestCase(new[] { true }, false,  ExpectedResult = true)]
        [TestCase(new[] { true }, true,  ExpectedResult = false)]
        [TestCase(new[] { false }, false,  ExpectedResult = false)]
        [TestCase(new[] { false }, true,  ExpectedResult = true)]
        public bool IsTrue_WithInvertFlag_InvertsConditions(bool[] nestedConditionStates, bool invert)
        {
            var conditions = CreateConditionMocks(nestedConditionStates);

            var groupCondition = new GroupCondition(false, invert, conditions);

            return groupCondition.IsTrue;
        }

        private List<ICondition> CreateConditionMocks(bool[] conditionStates)
        {
            var conditions = new List<ICondition>();
            foreach (var state in conditionStates)
            {
                var cond = Substitute.For<ICondition>();
                cond.IsTrue.Returns(state);
                conditions.Add(cond);
            }

            return conditions;
        }
        
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        public void Initialize_InitializesNestedConditions(int numConditions)
        {
            var counter = 0;
            
            var conditions = new List<ICondition>();
            for (int i = 0; i < numConditions; i++)
            {
                var cond = Substitute.For<IInitializable, ICondition>();
                cond.When(initializable => initializable.Initialize())
                    .Do(_ => counter++);
                conditions.Add((ICondition)cond);
            }

            var groupCondition = new GroupCondition(false, false, conditions);
            groupCondition.Initialize();
            
            Assert.AreEqual(numConditions, counter);
        }
    }
}