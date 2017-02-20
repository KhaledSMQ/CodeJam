﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

using NUnit.Framework;

namespace CodeJam.Reflection
{
	[TestFixture(Category = "Reflection")]
	public partial class ReflectionExtensionsTest
	{
		[Test]
		public void IsDebugAssemblyTest()
		{
#if DEBUG
			const bool isDebug = true;
#else
			const bool isDebug = false;
#endif
			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			Assert.AreEqual(typeof(ReflectionExtensionsTest).Assembly.IsDebugAssembly(), isDebug);
		}

		[TestCase(typeof(List<int>), "System.Collections.Generic.List`1[[System.Int32, mscorlib]], mscorlib")]
		[TestCase(typeof(List<>), "System.Collections.Generic.List`1[T], mscorlib")]
		[TestCase(typeof(IList<int>), "System.Collections.Generic.IList`1[[System.Int32, mscorlib]], mscorlib")]
		[TestCase(typeof(Dictionary<,>), "System.Collections.Generic.Dictionary`2[TKey,TValue], mscorlib")]
		[TestCase(typeof(Dictionary<int, string>), "System.Collections.Generic.Dictionary`2[[System.Int32, mscorlib],[System.String, mscorlib]], mscorlib")]
		[TestCase(typeof(Dictionary<int, List<int>>), "System.Collections.Generic.Dictionary`2[[System.Int32, mscorlib],[System.Collections.Generic.List`1[[System.Int32, mscorlib]], mscorlib]], mscorlib")]
		[TestCase(typeof(int), "System.Int32, mscorlib")]
		[TestCase(typeof(int?), "System.Nullable`1[[System.Int32, mscorlib]], mscorlib")]
		[TestCase(typeof(KeyValuePair<int, string>?), "System.Nullable`1[[System.Collections.Generic.KeyValuePair`2[[System.Int32, mscorlib],[System.String, mscorlib]], mscorlib]], mscorlib")]
		[TestCase(typeof(int[]), "System.Int32[], mscorlib")]
		[TestCase(typeof(int[,]), "System.Int32[,], mscorlib")]
		[TestCase(typeof(int[,][]), "System.Int32[][,], mscorlib")]
		[TestCase(typeof(int[][,]), "System.Int32[,][], mscorlib")]
		[TestCase(typeof(int[][]), "System.Int32[][], mscorlib")]
		[TestCase(typeof(int[][][]), "System.Int32[][][], mscorlib")]
		[TestCase(typeof(int[][,,][]), "System.Int32[][,,][], mscorlib")]
		[TestCase(typeof(int[,][,,][]), "System.Int32[][,,][,], mscorlib")]
		[TestCase(typeof(int[][,,][,]), "System.Int32[,][,,][], mscorlib")]
		[TestCase(typeof(int[,][,,][,]), "System.Int32[,][,,][,], mscorlib")]
		[TestCase(typeof(int?[]), "System.Nullable`1[[System.Int32, mscorlib]][], mscorlib")]
		[TestCase(typeof(int?[,]), "System.Nullable`1[[System.Int32, mscorlib]][,], mscorlib")]
		[TestCase(typeof(int?[,][]), "System.Nullable`1[[System.Int32, mscorlib]][][,], mscorlib")]
		[TestCase(typeof(int?[][,]), "System.Nullable`1[[System.Int32, mscorlib]][,][], mscorlib")]
		[TestCase(typeof(int?[][]), "System.Nullable`1[[System.Int32, mscorlib]][][], mscorlib")]
		[TestCase(typeof(int?[][][]), "System.Nullable`1[[System.Int32, mscorlib]][][][], mscorlib")]
		[TestCase(typeof(int?[][,,][]), "System.Nullable`1[[System.Int32, mscorlib]][][,,][], mscorlib")]
		[TestCase(typeof(int?[,][,,][]), "System.Nullable`1[[System.Int32, mscorlib]][][,,][,], mscorlib")]
		[TestCase(typeof(int?[][,,][,]), "System.Nullable`1[[System.Int32, mscorlib]][,][,,][], mscorlib")]
		[TestCase(typeof(int?[,][,,][,]), "System.Nullable`1[[System.Int32, mscorlib]][,][,,][,], mscorlib")]
		[TestCase(typeof(List<int?>), "System.Collections.Generic.List`1[[System.Nullable`1[[System.Int32, mscorlib]], mscorlib]], mscorlib")]
		[TestCase(typeof(List<int?[]>), "System.Collections.Generic.List`1[[System.Nullable`1[[System.Int32, mscorlib]][], mscorlib]], mscorlib")]
		[TestCase(typeof(List<int?[,,]>), "System.Collections.Generic.List`1[[System.Nullable`1[[System.Int32, mscorlib]][,,], mscorlib]], mscorlib")]
		[TestCase(typeof(List<int?[][,,]>), "System.Collections.Generic.List`1[[System.Nullable`1[[System.Int32, mscorlib]][,,][], mscorlib]], mscorlib")]
		[TestCase(typeof(List<int?[,][]>), "System.Collections.Generic.List`1[[System.Nullable`1[[System.Int32, mscorlib]][][,], mscorlib]], mscorlib")]
		[TestCase(typeof(ReflectionExtensions), "CodeJam.Reflection.ReflectionExtensions, CodeJam")]
		public void GetShortAssemblyQualifiedNameTest(Type type, string expected)
		{
			var shortAssemblyQualifiedName = type.GetShortAssemblyQualifiedName();
			Console.WriteLine(shortAssemblyQualifiedName);
			Console.WriteLine(type.AssemblyQualifiedName);

			Assert.AreEqual(expected, shortAssemblyQualifiedName);

			if (!type.IsGenericTypeDefinition)
			{
				Assert.AreEqual(type, Type.GetType(type.GetShortAssemblyQualifiedName()));
			}
		}

		[TestCase(typeof(List<int>), typeof(IList<int>), ExpectedResult = true)]
		[TestCase(typeof(List<int>), typeof(IList), ExpectedResult = true)]
		[TestCase(typeof(List<int>), typeof(IList<>), ExpectedResult = true)]
		[TestCase(typeof(List<int>), typeof(IEnumerable<int>), ExpectedResult = true)]
		[TestCase(typeof(List<int>), typeof(IEnumerable), ExpectedResult = true)]
		[TestCase(typeof(List<int>), typeof(IEnumerable<>), ExpectedResult = true)]
		[TestCase(typeof(IList<int>), typeof(IEnumerable<>), ExpectedResult = true)]
		[TestCase(typeof(IList<>), typeof(IEnumerable<>), ExpectedResult = true)]
		[TestCase(typeof(IEnumerable<>), typeof(IEnumerable), ExpectedResult = true)]
		[TestCase(typeof(List<int>), typeof(List<int>), ExpectedResult = false)]
		[TestCase(typeof(IList<>), typeof(ISet<>), ExpectedResult = false)]
		public bool IsSubClassTest(Type type, Type check) => type.IsSubClass(check);

		[TestCase(typeof(int), ExpectedResult = true)]
		[TestCase(typeof(List<int>), ExpectedResult = true)]
		[TestCase(typeof(List<>), ExpectedResult = false)]
		[TestCase(typeof(int[]), ExpectedResult = false)]
		[TestCase(typeof(IList), ExpectedResult = false)]
		[TestCase(typeof(ReflectionExtensions), ExpectedResult = false)]
		[TestCase(typeof(KeyedCollection<int, int>), ExpectedResult = false)]
		public bool IsInstantiableTypeTest(Type type) => type.IsInstantiable();

		[TestCase(typeof(int), ExpectedResult = false)]
		[TestCase(typeof(int?), ExpectedResult = true)]
		[TestCase(typeof(string), ExpectedResult = false)]
		[TestCase(typeof(double), ExpectedResult = false)]
		[TestCase(typeof(double?), ExpectedResult = true)]
		public bool IsIsNullableTypeTest(Type type) => type.IsNullable();

		[TestCase(typeof(sbyte), ExpectedResult = true)]
		[TestCase(typeof(byte), ExpectedResult = true)]
		[TestCase(typeof(short), ExpectedResult = true)]
		[TestCase(typeof(ushort), ExpectedResult = true)]
		[TestCase(typeof(int), ExpectedResult = true)]
		[TestCase(typeof(uint), ExpectedResult = true)]
		[TestCase(typeof(long), ExpectedResult = true)]
		[TestCase(typeof(ulong), ExpectedResult = true)]
		[TestCase(typeof(decimal), ExpectedResult = false)]
		[TestCase(typeof(float), ExpectedResult = false)]
		[TestCase(typeof(double), ExpectedResult = false)]
		[TestCase(typeof(DateTime), ExpectedResult = false)]
		[TestCase(typeof(char), ExpectedResult = false)]
		[TestCase(typeof(string), ExpectedResult = false)]
		[TestCase(typeof(object), ExpectedResult = false)]
		[TestCase(typeof(AttributeTargets), ExpectedResult = true)]
		[TestCase(typeof(int?), ExpectedResult = true)]
		[TestCase(typeof(DateTime?), ExpectedResult = false)]
		public bool IsInteger(Type type) => type.IsInteger();

		[TestCase(typeof(sbyte), ExpectedResult = true)]
		[TestCase(typeof(byte), ExpectedResult = true)]
		[TestCase(typeof(short), ExpectedResult = true)]
		[TestCase(typeof(ushort), ExpectedResult = true)]
		[TestCase(typeof(int), ExpectedResult = true)]
		[TestCase(typeof(uint), ExpectedResult = true)]
		[TestCase(typeof(long), ExpectedResult = true)]
		[TestCase(typeof(ulong), ExpectedResult = true)]
		[TestCase(typeof(decimal), ExpectedResult = true)]
		[TestCase(typeof(float), ExpectedResult = true)]
		[TestCase(typeof(double), ExpectedResult = true)]
		[TestCase(typeof(DateTime), ExpectedResult = false)]
		[TestCase(typeof(char), ExpectedResult = false)]
		[TestCase(typeof(string), ExpectedResult = false)]
		[TestCase(typeof(object), ExpectedResult = false)]
		[TestCase(typeof(AttributeTargets), ExpectedResult = true)]
		[TestCase(typeof(int?), ExpectedResult = true)]
		[TestCase(typeof(DateTime?), ExpectedResult = false)]
		public bool IsNumeric(Type type) => type.IsNumeric();

		[TestCase(typeof(sbyte?), ExpectedResult = true)]
		[TestCase(typeof(byte?), ExpectedResult = true)]
		[TestCase(typeof(short?), ExpectedResult = true)]
		[TestCase(typeof(ushort?), ExpectedResult = true)]
		[TestCase(typeof(int?), ExpectedResult = true)]
		[TestCase(typeof(uint?), ExpectedResult = true)]
		[TestCase(typeof(long?), ExpectedResult = true)]
		[TestCase(typeof(ulong?), ExpectedResult = true)]
		[TestCase(typeof(sbyte), ExpectedResult = false)]
		[TestCase(typeof(byte), ExpectedResult = false)]
		[TestCase(typeof(short), ExpectedResult = false)]
		[TestCase(typeof(ushort), ExpectedResult = false)]
		[TestCase(typeof(int), ExpectedResult = false)]
		[TestCase(typeof(uint), ExpectedResult = false)]
		[TestCase(typeof(long), ExpectedResult = false)]
		[TestCase(typeof(ulong), ExpectedResult = false)]
		[TestCase(typeof(decimal), ExpectedResult = false)]
		[TestCase(typeof(float), ExpectedResult = false)]
		[TestCase(typeof(double), ExpectedResult = false)]
		[TestCase(typeof(DateTime), ExpectedResult = false)]
		[TestCase(typeof(char), ExpectedResult = false)]
		[TestCase(typeof(string), ExpectedResult = false)]
		[TestCase(typeof(object), ExpectedResult = false)]
		[TestCase(typeof(AttributeTargets), ExpectedResult = false)]
		[TestCase(typeof(AttributeTargets?), ExpectedResult = true)]
		public bool IsNullableInteger(Type type) => type.IsNullableInteger();

		[TestCase(typeof(sbyte?), ExpectedResult = true)]
		[TestCase(typeof(byte?), ExpectedResult = true)]
		[TestCase(typeof(short?), ExpectedResult = true)]
		[TestCase(typeof(ushort?), ExpectedResult = true)]
		[TestCase(typeof(int?), ExpectedResult = true)]
		[TestCase(typeof(uint?), ExpectedResult = true)]
		[TestCase(typeof(long?), ExpectedResult = true)]
		[TestCase(typeof(ulong?), ExpectedResult = true)]
		[TestCase(typeof(decimal?), ExpectedResult = true)]
		[TestCase(typeof(float?), ExpectedResult = true)]
		[TestCase(typeof(double?), ExpectedResult = true)]
		[TestCase(typeof(sbyte), ExpectedResult = false)]
		[TestCase(typeof(byte), ExpectedResult = false)]
		[TestCase(typeof(short), ExpectedResult = false)]
		[TestCase(typeof(ushort), ExpectedResult = false)]
		[TestCase(typeof(int), ExpectedResult = false)]
		[TestCase(typeof(uint), ExpectedResult = false)]
		[TestCase(typeof(long), ExpectedResult = false)]
		[TestCase(typeof(ulong), ExpectedResult = false)]
		[TestCase(typeof(decimal), ExpectedResult = false)]
		[TestCase(typeof(float), ExpectedResult = false)]
		[TestCase(typeof(double), ExpectedResult = false)]
		[TestCase(typeof(DateTime), ExpectedResult = false)]
		[TestCase(typeof(char), ExpectedResult = false)]
		[TestCase(typeof(string), ExpectedResult = false)]
		[TestCase(typeof(object), ExpectedResult = false)]
		[TestCase(typeof(AttributeTargets), ExpectedResult = false)]
		[TestCase(typeof(AttributeTargets?), ExpectedResult = true)]
		public bool IsNullableNumeric(Type type) => type.IsNullableNumeric();

		[TestCase(typeof(sbyte), ExpectedResult = false)]
		[TestCase(typeof(byte), ExpectedResult = false)]
		[TestCase(typeof(short), ExpectedResult = false)]
		[TestCase(typeof(ushort), ExpectedResult = false)]
		[TestCase(typeof(int), ExpectedResult = false)]
		[TestCase(typeof(uint), ExpectedResult = false)]
		[TestCase(typeof(long), ExpectedResult = false)]
		[TestCase(typeof(ulong), ExpectedResult = false)]
		[TestCase(typeof(decimal), ExpectedResult = false)]
		[TestCase(typeof(float), ExpectedResult = false)]
		[TestCase(typeof(double), ExpectedResult = false)]
		[TestCase(typeof(DateTime), ExpectedResult = false)]
		[TestCase(typeof(char), ExpectedResult = false)]
		[TestCase(typeof(string), ExpectedResult = false)]
		[TestCase(typeof(object), ExpectedResult = false)]
		[TestCase(typeof(AttributeTargets), ExpectedResult = false)]
		[TestCase(typeof(ConsoleColor), ExpectedResult = false)]
		[TestCase(typeof(AttributeTargets?), ExpectedResult = true)]
		[TestCase(typeof(ConsoleColor?), ExpectedResult = true)]
		public bool IsNullableEnum(Type type) => type.IsNullableEnum();

		[TestCase(typeof(AttributeTargets), ExpectedResult = typeof(int))]
		[TestCase(typeof(int?), ExpectedResult = typeof(int))]
		[TestCase(typeof(AttributeTargets?), ExpectedResult = typeof(int))]
		[TestCase(typeof(string), ExpectedResult = typeof(string))]
		public Type ToUnderlying(Type type) => type.ToUnderlying();

		[CompilerGenerated]
		private class NotAnonymousType<T> : List<T>
		{
		}

		private class TestAnonymousCaseAttribute : TestCaseAttribute
		{
			public TestAnonymousCaseAttribute()
				: base(new { Field = 0 }.GetType())
			{
				ExpectedResult = true;
			}
		}

		[TestAnonymousCase]
		[TestCase(typeof(NotAnonymousType<int>), ExpectedResult = false)]
		[TestCase(typeof(DateTime?), ExpectedResult = false)]
		[TestCase(typeof(DateTime), ExpectedResult = false)]
		public bool IsAnonymous(Type type) => type.IsAnonymous();
	}
}
