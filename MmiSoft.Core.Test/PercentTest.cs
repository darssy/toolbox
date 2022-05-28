using System;
using System.Globalization;
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
			Assert.AreEqual(13, 10 + new Percent(30), 0.000001f);
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
	}
}
