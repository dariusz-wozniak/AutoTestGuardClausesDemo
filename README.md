[![Build status](https://ci.appveyor.com/api/projects/status/m6djus7yaqr8cwdc?svg=true)](https://ci.appveyor.com/project/dariusz-wozniak/autotestguardclausesdemo)

# AutoTestGuardClausesDemo
Test guard clauses in .ctors automatically.

E.g.:

```csharp
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
```

Requirements:

- AutoFixture.Idioms
- AutoFixture.AutoMoq
