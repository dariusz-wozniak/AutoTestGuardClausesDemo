# AutoTestGuardClausesDemo
Test guard clauses in .ctors automatically.

E.g.:

```csharp
public class Tests
{
    public static IEnumerable<Type> CtorsWithGuardClauses
    {
        get
        {
            return new List<Type>
            {
                typeof(SomeClass),
                // add more guys here...
            };
        }
    }

    [TestCaseSource(nameof(CtorsWithGuardClauses))]
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