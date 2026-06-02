using System;
using System.Globalization;
using MmiSoft.Core.Math;
using NUnit.Framework;

namespace MmiSoft.Core
{
	[TestFixture]
	public class PercentTest
	{
		[Test]
		public void DivideOp_2DividedBy8PerCent_Equals25()
		{
			double expectedD = 25;
			double expectedM = 25;
			Percent testValue = new Percent(8);
			Assert.AreEqual(expectedD, 2 / testValue, 0.000001);
			Assert.AreEqual(expectedD, 2d / testValue, 0.000001);
			Assert.AreEqual(expectedM, 2m / testValue);
		}

		[Test]
		public void DivideOp_2DividedByZeroPerCent_EqualsInfinity()
		{
			Assert.AreEqual(double.PositiveInfinity, 2 / Percent.Zero);
			Assert.AreEqual(double.PositiveInfinity, 2d / Percent.Zero);
			Assert.Throws<DivideByZeroException>(() =>
			{
				var _ = 2m / Percent.Zero;
			});
		}

		[Test]
		public void Parser()
		{
			CultureInfo culture = CultureInfo.CurrentCulture;
			string dot = culture.NumberFormat.PercentDecimalSeparator;
			Assert.AreEqual(new Percent(2.35), Percent.Parse($"2{dot}35%", culture));
			Assert.AreEqual(new Percent(20), Percent.Parse("20%", culture));

			Assert.Throws<FormatException>(() => Percent.Parse($"{dot}5%", culture));
			Assert.Throws<FormatException>(() => Percent.Parse($"7{dot}%", culture));
		}

		[Test]
		public void EqualityOperator()
		{
			Assert.IsTrue(new Percent(10.0000000002) == new Percent(10));
			Assert.IsTrue(new Percent(10.002) != new Percent(10));
		}

		[Test]
		public void ComparisonOperators()
		{
			Assert.IsTrue(new Percent(25) < new Percent(50));
			Assert.IsTrue(new Percent(7) > new Percent(-10));
		}

		[Test]
		public void PlusOperators()
		{
			Assert.AreEqual(13, 10 + new Percent(30));
			Assert.AreEqual(13, 10 + new Percent(33));
			Assert.AreEqual(14, 10 + new Percent(36));
			Assert.AreEqual(8.625, 7.5 + new Percent(15), 0.000001f);
		}

		[Test]
		public void MinusOperators()
		{
			Assert.AreEqual((-15).Percent(), new Percent(30) - new Percent(45));
		}

		[Test]
		public void FromCoefficient_ConvertsCoefficientToPercentage()
		{
			Assert.AreEqual(new Percent(30), Percent.FromCoefficient(1.3));
		}

		[Test]
		public void FromCoefficient_CoefficientLessThanOne_CreatesNegativePercent()
		{
			Assert.AreEqual(new Percent(-10), Percent.FromCoefficient(.9));
		}

		[Test]
		public void FromFractional_ConvertsFractionalToPercentage()
		{
			Assert.AreEqual(new Percent(30), Percent.FromFractional(0.3));
			Assert.AreEqual(new Percent(90), Percent.FromFractional(.9));
			Assert.AreEqual(new Percent(120), Percent.FromFractional(1.2));
			Assert.AreEqual(new Percent(73.45), Percent.FromFractional(0.7345f));
		}

		[Test]
		public void GetDisplayValue_ReturnsTheDefaultPercentFormatOfTheCurrentCulture()
		{
			CultureInfo culture = CultureInfo.CurrentCulture;
			string dot = culture.NumberFormat.PercentDecimalSeparator;
			Assert.AreEqual($"28{dot}39%", new Percent(28.392).GetDisplayValue());
			Assert.AreEqual($"28{dot}33%", new Percent(28.329).GetDisplayValue());
		}

		// Each pair below is centered on a GetHashCode() quantization boundary (a value that is a
		// multiple of DefaultTolerance) and is only 0.00008 percentage points wide -i.e. a raw diff of
		// 8e-7, comfortably inside the 1e-6 == tolerance. So the two values are equal by ==, which by
		// the Equals/GetHashCode contract REQUIRES them to share a hash code. Truncation-based bucketing
		// puts them on opposite sides of the boundary, so the hash assertion is expected to break.
		[Test]
		public void HashCode_StraddleBoundary_At1Percent()
		{
			Percent a = new Percent(0.99996);
			Percent b = new Percent(1.00004);
			Assert.AreEqual(a, b, "within tolerance => equal");
			Assert.AreEqual(a.GetHashCode(), b.GetHashCode(), "equal values must share a hash code");
		}

		[Test]
		public void HashCode_StraddleBoundary_At5Percent()
		{
			Percent a = new Percent(4.99996);
			Percent b = new Percent(5.00004);
			Assert.AreEqual(a, b, "within tolerance => equal");
			Assert.AreEqual(a.GetHashCode(), b.GetHashCode(), "equal values must share a hash code");
		}

		[Test]
		public void HashCode_StraddleBoundary_At30Percent()
		{
			Percent a = new Percent(29.99996);
			Percent b = new Percent(30.00004);
			Assert.AreEqual(a, b, "within tolerance => equal");
			Assert.AreEqual(a.GetHashCode(), b.GetHashCode(), "equal values must share a hash code");
		}

		[Test]
		public void HashCode_StraddleBoundary_At50Percent()
		{
			Percent a = new Percent(49.99996);
			Percent b = new Percent(50.00004);
			Assert.AreEqual(a, b, "within tolerance => equal");
			Assert.AreEqual(a.GetHashCode(), b.GetHashCode(), "equal values must share a hash code");
		}

		[Test]
		public void HashCode_StraddleBoundary_At75Percent()
		{
			Percent a = new Percent(74.99996);
			Percent b = new Percent(75.00004);
			Assert.AreEqual(a, b, "within tolerance => equal");
			Assert.AreEqual(a.GetHashCode(), b.GetHashCode(), "equal values must share a hash code");
		}

		[Test]
		public void HashCode_StraddleBoundary_At99Percent()
		{
			Percent a = new Percent(98.99996);
			Percent b = new Percent(99.00004);
			Assert.AreEqual(a, b, "within tolerance => equal");
			Assert.AreEqual(a.GetHashCode(), b.GetHashCode(), "equal values must share a hash code");
		}

		[Test]
		public void HashCode_StraddleBoundary_Above100Percent()
		{
			Percent a = new Percent(149.99996);
			Percent b = new Percent(150.00004);
			Assert.AreEqual(a, b, "within tolerance => equal");
			Assert.AreEqual(a.GetHashCode(), b.GetHashCode(), "equal values must share a hash code");
		}

		[Test]
		public void HashCode_StraddleBoundary_NegativePercent()
		{
			Percent a = new Percent(-30.00004);
			Percent b = new Percent(-29.99996);
			Assert.AreEqual(a, b, "within tolerance => equal");
			Assert.AreEqual(a.GetHashCode(), b.GetHashCode(), "equal values must share a hash code");
		}

		[Test]
		public void HashCode_StraddleBoundary_AtFractionalBoundary()
		{
			Percent a = new Percent(12.34556);
			Percent b = new Percent(12.34564);
			Assert.AreEqual(a, b, "within tolerance => equal");
			Assert.AreEqual(a.GetHashCode(), b.GetHashCode(), "equal values must share a hash code");
		}

		[Test]
		public void HashCode_StraddleBoundary_AtRepeatingDecimal()
		{
			Percent a = new Percent(33.33326);
			Percent b = new Percent(33.33334);
			Assert.AreEqual(a, b, "within tolerance => equal");
			Assert.AreEqual(a.GetHashCode(), b.GetHashCode(), "equal values must share a hash code");
		}

		// The current hash buckets at scale 1e5 (value * 100000, rounded), so its boundaries sit at
		// percent = k.xxx5 (half-thousandths of a percent). This pair is centered on such a boundary
		// and is 0.00008 pp wide (raw diff 8e-7, within the 1e-6 == tolerance), so it is equal yet
		// straddles the rounding boundary -breaking the contract again.
		[Test]
		public void HashCode_StraddleBoundary_AtNewBucketEdge()
		{
			Percent a = new Percent(30.00046);
			Percent b = new Percent(30.00054);
			Assert.AreEqual(a, b, "within tolerance => equal");
			Assert.AreEqual(a.GetHashCode(), b.GetHashCode(), "equal values must share a hash code");
		}

		// Property test: for many random percents, build a second percent within the == tolerance and
		// assert the Equals/GetHashCode contract -equal values must hash equally. Any failure is a pair
		// that compares equal but hashes differently, i.e. a key that would go missing in a hash set.
		[Test]
		public void HashCode_Contract_RandomPairsWithinTolerance()
		{
			const float tolerance = Math.Extensions.DefaultTolerance; // 1e-6 on the raw value
			Random rng = new Random(20240601); // fixed seed for reproducibility
			int violations = 0;
			string firstCounterexample = null;

			for (int i = 0; i < 200_000; i++)
			{
				double basePercent = rng.NextDouble() * 400 - 200;            // -200% .. 200%
				double rawDelta = (rng.NextDouble() * 2 - 1) * tolerance * 0.95; // strictly inside tolerance
				double percentDelta = rawDelta * 100;

				Percent a = new Percent(basePercent);
				Percent b = new Percent(basePercent + percentDelta);

				if (a == b && a.GetHashCode() != b.GetHashCode())
				{
					Console.WriteLine($"{(a.Value * 100).Round()} {(b.Value * 100).Round()}");
					violations++;
					firstCounterexample ??=
						$"a.Value={a.Value:R} (hash {a.GetHashCode()}), b.Value={b.Value:R} (hash {b.GetHashCode()}), " +
						$"raw diff={System.Math.Abs(a.Value - b.Value):R}";
				}
			}

			Assert.That(violations, Is.Zero,
				$"Found {violations} equal pairs (out of 200000) with differing hash codes. First: {firstCounterexample}");
		}
	}
}
