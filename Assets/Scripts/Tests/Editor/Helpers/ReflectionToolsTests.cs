using NUnit.Framework;

namespace EscapeRoom.EditorScripts.Helpers.Tests
{
    /// <summary>
    /// Tests for <see cref="ReflectionTools"/>
    /// </summary>
    [TestFixture(Category = "Helpers")]
    public class ReflectionToolsTests
    {

        [TestCase( false, false, ExpectedResult = 2)]
        [TestCase(false, true, ExpectedResult = 3)]
        [TestCase( true, false, ExpectedResult = 3)]
        [TestCase( true, true, ExpectedResult = 4)]
        public int GetTypesAssignableFrom(bool abstractTypes, bool genericTypes)
        {
            var types = ReflectionTools.GetTypesImplementingInterface(typeof(IFoo), abstractTypes, genericTypes);

            return types.Length;
        }


        private interface IFoo
        {
            
        }
        
        private interface IFooDerived: IFoo
        {
            
        }
        
        private abstract class FooAbstract: IFoo
        {
            
        }
        
        private class Foo1: IFoo
        {
            
        }
        
        private class Foo2: IFooDerived
        {
            
        }
        
        private class FooGeneric<T>: IFooDerived
        {
            
        }
    }
    
}