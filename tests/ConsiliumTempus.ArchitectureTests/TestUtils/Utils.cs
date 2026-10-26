using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using MediatR;

namespace ConsiliumTempus.ArchitectureTests.TestUtils;

public static class Utils
{
    public static GivenTypesConjunction AreNotInterfaces(
        this GivenTypesThat givenTypesThat) =>
        givenTypesThat.AreNot(ArchRuleDefinition.Interfaces());

    public static void ShouldHaveSameTypes(Architecture architecture, Type type, string @namespace) =>
        ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(type)
            .GetObjects(architecture)
            .Select(c => c.FullName)
            .Should()
            .BeEquivalentTo(
                ArchRuleDefinition
                    .Classes()
                    .That()
                    .ResideInNamespace(@namespace)
                    .GetObjects(architecture)
                    .Select(c => c.FullName));

    public static Type ToType(this IType type) =>
        Type.GetType($"{type.FullName}, {type.Assembly.Name}", throwOnError: true)!;

    public static IEnumerable<Type> GetHandledNotificationTypes(this Type handler) =>
        handler
            .GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
            .Select(i => i.GetGenericArguments()[0]);

    extension(IEnumerable<Class?> classes)
    {
        public void ShouldHavePrivateConstructors() =>
            classes
                .Should()
                .AllSatisfy(@class => @class
                    .GetConstructors()
                    .HasPrivateConstructors()
                    .Should()
                    .BeTrue($"{@class}"));

        public void ShouldHavePrivateParameterlessConstructor() =>
            classes
                .Should()
                .AllSatisfy(@class => @class
                    .GetConstructors()
                    .HasPrivateParameterlessConstructor()
                    .Should()
                    .BeTrue($"{@class}"));
    }

    extension(IEnumerable<MethodMember> constructors)
    {
        private bool HasPrivateConstructors() =>
            constructors.All(c => c.Visibility == Visibility.Private);

        private bool HasPrivateParameterlessConstructor() =>
            constructors.Any(c => c.Visibility == Visibility.Private && !c.Parameters.Any());
    }
}