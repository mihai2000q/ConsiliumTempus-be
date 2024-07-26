using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Syntax.Elements.Types;

namespace ConsiliumTempus.ArchitectureTests.TestUtils;

public static class Utils
{
    public static GivenTypesConjunction AreNotInterfaces(
        this GivenTypesThat<GivenTypesConjunction, IType> givenTypesThat) =>
        givenTypesThat.DoNotHaveNameStartingWith("I");

    public static void ShouldHavePrivateConstructors(this IEnumerable<Class?> classes) =>
        classes
            .Should()
            .AllSatisfy(@class => @class
                .GetConstructors()
                .HasPrivateConstructors()
                .Should()
                .BeTrue($"{@class}"));

    public static void ShouldHavePrivateParameterlessConstructor(this IEnumerable<Class?> classes) =>
        classes
            .Should()
            .AllSatisfy(@class => @class
                .GetConstructors()
                .HasPrivateParameterlessConstructor()
                .Should()
                .BeTrue($"{@class}"));

    private static bool HasPrivateConstructors(this IEnumerable<MethodMember> constructors) =>
        constructors.All(c => c.Visibility == Visibility.Private);

    private static bool HasPrivateParameterlessConstructor(this IEnumerable<MethodMember> constructors) =>
        constructors.Any(c => c.Visibility == Visibility.Private && !c.Parameters.Any());
}