using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using NUnit.Framework;

namespace AutoTestGuardClauses
{
    public class Tests
    {
        private static IEnumerable<Type> TypesToTest()
        {
            var selectMany = Assembly.Load("AutoTestGuardClauses").GetTypes();
            var types = selectMany.Where(t => t.IsClass && 
                                              t.Namespace != null && 
                                              t.Namespace.StartsWith("AutoTestGuardClauses"));

            foreach (var type in types) yield return type;
        }

        [TestCaseSource(nameof(TypesToTest))]
        public void AllConstructorsMustBeGuardClaused(Type type)
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var assertion = new GuardClauseAssertion(fixture);

            assertion.Verify(type.GetConstructors());
        }
    }

    public class SomeClass
    {
        private readonly I1 _i1;
        private readonly I2 _i2;
        private readonly I3 _i3;
        private readonly string _s;

        public SomeClass(I1 i1, I2 i2, I3 i3, int i, int? nullableI, string s)
        {
            _i1 = i1 ?? throw new ArgumentNullException(nameof(i1));
            _i2 = i2 ?? throw new ArgumentNullException(nameof(i2));
            _i3 = i3 ?? throw new ArgumentNullException(nameof(i3));
            _s = s ?? throw new ArgumentNullException(nameof(s));
        }

        public SomeClass(I1 i1)
        {
            _i1 = i1 ?? throw new ArgumentNullException(nameof(i1));
        }

        // that ctor is not considered into testing...:
        private SomeClass(I1 i1, I2 i2) { }

        // ...neither that one...:
        protected SomeClass(I1 i1, I2 i2, I3 i3) { }

        // ...nor that fellow:
        internal SomeClass(I1 i1, int a) { }
    }

    public interface I1 { }
    public interface I2 { }
    public interface I3 { }
}
