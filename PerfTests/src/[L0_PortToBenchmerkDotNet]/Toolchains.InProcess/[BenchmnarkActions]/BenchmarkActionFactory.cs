﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using BenchmarkDotNet.Running;

using JetBrains.Annotations;

// ReSharper disable once CheckNamespace

namespace BenchmarkDotNet.Toolchains.InProcess
{
	/// <summary>Helper class that creates <see cref="BenchmarkAction"/> instances. </summary>
	public static partial class BenchmarkActionFactory
	{
		#region Factory core
		/// <summary>
		/// Dispatch method that creates <see cref="BenchmarkAction"/> using
		/// <paramref name="targetMethod"/> or <paramref name="fallbackIdleSignature"/> to find correct implementation.
		/// Either <paramref name="targetMethod"/> or <paramref name="fallbackIdleSignature"/> should be not <c>null</c>.
		/// </summary>
		private static BenchmarkAction CreateCore(
			[NotNull] object instance,
			[CanBeNull] MethodInfo targetMethod,
			[CanBeNull] MethodInfo fallbackIdleSignature,
			BenchmarkActionCodegen codegenMode,
			int unrollFactor)
		{
			Type resultType;
			object resultInstance;
			PrepareInstanceAndResultType(
				instance, targetMethod, fallbackIdleSignature, out resultInstance, out resultType);

			if (resultType == typeof(void))
				return new BenchmarkActionVoid(resultInstance, targetMethod, codegenMode, unrollFactor);

			if (resultType == typeof(Task))
				return new BenchmarkActionTask(resultInstance, targetMethod, codegenMode, unrollFactor);

			if (resultType.GetTypeInfo().IsGenericType)
			{
				var genericType = resultType.GetGenericTypeDefinition();
				var argType = resultType.GenericTypeArguments[0];
				if (typeof(Task<>) == genericType)
					return Create(
						typeof(BenchmarkActionTask<>).MakeGenericType(argType),
						resultInstance, targetMethod,
						codegenMode, unrollFactor);

				if (typeof(ValueTask<>).IsAssignableFrom(genericType))
					return Create(
						typeof(BenchmarkActionValueTask<>).MakeGenericType(argType),
						resultInstance, targetMethod,
						codegenMode, unrollFactor);
			}

			if (targetMethod == null && resultType.GetTypeInfo().IsValueType)
				// for Idle: we return int because creating bigger ValueType could take longer than benchmarked method itself.
				resultType = typeof(int);

			return Create(
				typeof(BenchmarkAction<>).MakeGenericType(resultType),
				resultInstance, targetMethod,
				codegenMode, unrollFactor);
		}

		private static void PrepareInstanceAndResultType(
			object instance, MethodInfo targetMethod, MethodInfo fallbackIdleSignature,
			out object resultInstance, out Type resultType)
		{
			var signature = targetMethod ?? fallbackIdleSignature;
			if (signature == null)
				throw new ArgumentNullException(
					nameof(fallbackIdleSignature),
					$"Either {nameof(targetMethod)} or  {nameof(fallbackIdleSignature)} should be not null.");

			if (!signature.IsStatic && instance == null)
				throw new ArgumentNullException(
					nameof(instance),
					$"The {nameof(instance)} parameter should be not null as invocation method is instance method.");

			resultInstance = signature.IsStatic ? null : instance;
			resultType = signature.ReturnType;

			if (resultType == typeof(void))
			{
				// DONTTOUCH: async should be checked for target method
				// as fallbackIdleSignature used for result type detection only.
				var isUsingAsyncKeyword = targetMethod?.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;
				if (isUsingAsyncKeyword)
					throw new NotSupportedException("Async void is not supported by design.");
			}
		}

		/// <summary>Helper to enforce .ctor signature.</summary>
		private static BenchmarkActionBase Create(
			Type actionType, object instance, MethodInfo method, BenchmarkActionCodegen codegenMode, int unrollFactor)
			=>
				(BenchmarkActionBase)Activator.CreateInstance(actionType, instance, method, codegenMode, unrollFactor);
		#endregion

		private static void FallbackMethod() { }
		private static readonly MethodInfo _fallbackSignature = new Action(FallbackMethod).GetMethodInfo();
		private static readonly MethodInfo _dummyMethod = typeof(DummyInstance).GetMethod(nameof(DummyInstance.Dummy));

		/// <summary>Creates run benchmark action.</summary>
		/// <param name="target">Target info.</param>
		/// <param name="instance">Instance of target.</param>
		/// <param name="codegenMode">Describes how benchmark action code is generated.</param>
		/// <param name="unrollFactor">Unroll factor.</param>
		/// <returns>Run benchmark action.</returns>
		public static BenchmarkAction CreateRun(
			Target target, object instance, BenchmarkActionCodegen codegenMode, int unrollFactor) =>
				CreateCore(instance, target.Method, null, codegenMode, unrollFactor);

		/// <summary>Creates idle benchmark action.</summary>
		/// <param name="target">Target info.</param>
		/// <param name="instance">Instance of target.</param>
		/// <param name="codegenMode">Describes how benchmark action code is generated.</param>
		/// <param name="unrollFactor">Unroll factor.</param>
		/// <returns>Idle benchmark action.</returns>
		public static BenchmarkAction CreateIdle(
			Target target, object instance, BenchmarkActionCodegen codegenMode, int unrollFactor) =>
				CreateCore(instance, null, target.Method, codegenMode, unrollFactor);

		/// <summary>Creates setup benchmark action.</summary>
		/// <param name="target">Target info.</param>
		/// <param name="instance">Instance of target.</param>
		/// <returns>Setup benchmark action.</returns>
		public static BenchmarkAction CreateSetup(Target target, object instance) =>
			CreateCore(instance, target.SetupMethod, _fallbackSignature, BenchmarkActionCodegen.DelegateCombine, 1);

		/// <summary>Creates cleanup benchmark action.</summary>
		/// <param name="target">Target info.</param>
		/// <param name="instance">Instance of target.</param>
		/// <returns>Cleanup benchmark action.</returns>
		public static BenchmarkAction CreateCleanup(Target target, object instance) =>
			CreateCore(instance, target.CleanupMethod, _fallbackSignature, BenchmarkActionCodegen.DelegateCombine, 1);

		/// <summary>Creates a dummy benchmark action.</summary>
		/// <returns>Dummy benchmark action.</returns>
		public static BenchmarkAction CreateDummy() =>
			CreateCore(new DummyInstance(), _dummyMethod, null, BenchmarkActionCodegen.DelegateCombine, 1);
	}
}