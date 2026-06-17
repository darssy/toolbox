using System.Drawing;
using NUnit.Framework;

namespace MmiSoft.Core;

[TestFixture]
public class ColorExtensionsTest
{
	[Test]
	public void MakeTransparent_HalfOpacity_SetsAlphaToHalfAndKeepsRgb()
	{
		Color result = Color.FromArgb(255, 10, 20, 30).MakeTransparent(new Percent(50f));
		Assert.That(result, Is.EqualTo(Color.FromArgb(128, 10, 20, 30)));
	}

	[Test]
	public void MakeTransparent_OpacityOutOfRange_Throws()
	{
		Assert.That(() => Color.Red.MakeTransparent(new Percent(150f)), Throws.ArgumentException);
		Assert.That(() => Color.Red.MakeTransparent(new Percent(-10f)), Throws.ArgumentException);
	}

	[Test]
	public void Negate_InvertsRgbAndPreservesAlpha()
	{
		Color result = Color.FromArgb(128, 10, 20, 30).Negate();
		Assert.That(result, Is.EqualTo(Color.FromArgb(128, 245, 235, 225)));
	}

	[Test]
	public void Darken_HalvesRgbAndPreservesAlpha()
	{
		Color result = Color.FromArgb(128, 200, 200, 200).Darken();
		Assert.That(result, Is.EqualTo(Color.FromArgb(128, 100, 100, 100)));
	}

	[Test]
	public void Lighten_BrightensRgbAndPreservesAlpha()
	{
		Color result = Color.FromArgb(128, 100, 100, 100).Lighten();
		Assert.That(result, Is.EqualTo(Color.FromArgb(128, 145, 145, 145)));
	}
}
