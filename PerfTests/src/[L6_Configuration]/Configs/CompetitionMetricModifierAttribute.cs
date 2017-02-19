﻿using System;

using CodeJam.PerfTests.Configs.Factories;
using CodeJam.PerfTests.Metrics;

namespace CodeJam.PerfTests.Configs
{
	/// <summary>
	/// Adds the <see cref="CompetitionMetricInfo.AbsoluteTime"/> metric
	/// and GC metrics.
	/// </summary>
	/// <seealso cref="CodeJam.PerfTests.CompetitionModifierAttribute"/>
	public sealed class CompetitionMeasureAllAttribute : CompetitionModifierAttribute
	{
		private class ModifierImpl : ICompetitionModifier
		{
			public void Modify(ManualCompetitionConfig competitionConfig)
			{
				competitionConfig.Metrics.Add(CompetitionMetricInfo.GcAllocations);
				competitionConfig.Metrics.Add(CompetitionMetricInfo.Gc0);
				competitionConfig.Metrics.Add(CompetitionMetricInfo.Gc1);
				competitionConfig.Metrics.Add(CompetitionMetricInfo.Gc2);
				competitionConfig.Metrics.Add(CompetitionMetricInfo.AbsoluteTime);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="CompetitionMeasureAllAttribute"/> class.</summary>
		public CompetitionMeasureAllAttribute() : base(() => new ModifierImpl()) { }
	}

	/// <summary>
	/// Removes the <see cref="CompetitionMetricInfo.RelativeTime"/> metric.
	/// </summary>
	/// <seealso cref="CodeJam.PerfTests.CompetitionModifierAttribute"/>
	public sealed class CompetitionNoRelativeTimeAttribute : CompetitionModifierAttribute
	{
		private class ModifierImpl : ICompetitionModifier
		{
			public void Modify(ManualCompetitionConfig competitionConfig) =>
				competitionConfig.Metrics.RemoveAll(m => m == CompetitionMetricInfo.RelativeTime);
		}

		/// <summary>Initializes a new instance of the <see cref="CompetitionNoRelativeTimeAttribute"/> class.</summary>
		public CompetitionNoRelativeTimeAttribute() : base(() => new ModifierImpl()) { }
	}

	/// <summary>
	/// Adds the <see cref="CompetitionMetricInfo.GcAllocations"/> metric.
	/// </summary>
	/// <seealso cref="CodeJam.PerfTests.CompetitionModifierAttribute"/>
	public sealed class CompetitionMeasureAllocationsAttribute : CompetitionModifierAttribute
	{
		private class ModifierImpl : ICompetitionModifier
		{
			public void Modify(ManualCompetitionConfig competitionConfig) =>
				competitionConfig.Metrics.Add(CompetitionMetricInfo.GcAllocations);
		}

		/// <summary>Initializes a new instance of the <see cref="CompetitionNoRelativeTimeAttribute"/> class.</summary>
		public CompetitionMeasureAllocationsAttribute() : base(() => new ModifierImpl()) { }
	}

	/// <summary>
	/// Removes GC metrics (all with category equal to <see cref="GcMetricValuesProvider.Category"/>).
	/// </summary>
	/// <seealso cref="CodeJam.PerfTests.CompetitionModifierAttribute"/>
	public sealed class CompetitionIgnoreAllocationsAttribute : CompetitionModifierAttribute
	{
		private class ModifierImpl : ICompetitionModifier
		{
			public void Modify(ManualCompetitionConfig competitionConfig) =>
				competitionConfig.Metrics.RemoveAll(m => m.Category == GcMetricValuesProvider.Category);
		}

		/// <summary>Initializes a new instance of the <see cref="CompetitionNoRelativeTimeAttribute"/> class.</summary>
		public CompetitionIgnoreAllocationsAttribute() : base(() => new ModifierImpl()) { }
	}
}