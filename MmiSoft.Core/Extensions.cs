using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using MmiSoft.Core.Math;
using Newtonsoft.Json;
using SysMath = System.Math;

namespace MmiSoft.Core
{
	public static class Extensions
	{
		private static readonly BrowsableAttribute BrowsableFalse = new BrowsableAttribute(false);

		public static Color Negate(in this Color c)
		{
			return Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
		}

		public static Color Darken(in this Color c)
		{
			return Color.FromArgb((int) SysMath.Round(c.R * 0.5),
				(int) SysMath.Round(c.G * 0.5),
				(int) SysMath.Round(c.B * 0.5));
		}

		public static Color Lighten(in this Color c)
		{
			int r = (int)SysMath.Round(c.R * 1.45);
			int g = (int)SysMath.Round(c.G * 1.45);
			int b = (int)SysMath.Round(c.B * 1.45);
			if (r > 255) r = 255;
			if (g > 255) g = 255;
			if (b > 255) b = 255;
			return Color.FromArgb(r, g, b);
		}

		public static bool Between<T>(this T number, T a, T b) where T : IComparable<T>
		{
			return number.CompareTo(a) <= 0 && number.CompareTo(b) >= 0
				|| number.CompareTo(b) <= 0 && number.CompareTo(a) >= 0;
		}

		public static bool Between(this int number, int a, int b)
		{
			return number <= a && number >= b || number <= b && number >= a;
		}

		public static bool Between(this float number, float a, float b)
		{
			return number <= a && number >= b || number <= b && number >= a;
		}

		public static bool Between(this decimal number, decimal a, decimal b)
		{
			return number <= a && number >= b || number <= b && number >= a;
		}

		public static bool Between(this TimeSpan number, TimeSpan a, TimeSpan b)
		{
			return number <= a && number >= b || number <= b && number >= a;
		}

		public static Size Half(this Size s)
		{
			return new Size(s.Width / 2, s.Height / 2);
		}

		public static void InvertRef(ref this Point p)
		{
			p.X = -p.X;
			p.Y = -p.Y;
		}

		public static Point Invert(this Point p)
		{
			return new Point(-p.X, -p.Y);
		}

		public static Point Center(this Point p, Point p2)
		{
			return new Point((p.X + p2.X) / 2, (p.Y + p2.Y) / 2);
		}

		public static Point Subtract(this Point p, Point subtraction)
		{
			return new Point(p.X - subtraction.X, p.Y - subtraction.Y);
		}

		public static void SubtractRef(ref this Point p, Point subtraction)
		{
			p.X -= subtraction.X;
			p.Y -= subtraction.Y;
		}

		public static Point Add(this Point p, Point addition)
		{
			return new Point(p.X + addition.X, p.Y + addition.Y);
		}

		public static void Scale(ref this Point p, float scaleRatio)
		{
			p.X = (p.X * scaleRatio).Round();
			p.Y = (p.Y * scaleRatio).Round();
		}

		public static void Scale(ref this Point p, double scaleRatio)
		{
			p.X = (int) (p.X * scaleRatio).Round();
			p.Y = (int) (p.Y * scaleRatio).Round();
		}

		public static double DistanceTo(this Point p1, Point p2)
		{
			int dx = p1.X - p2.X;
			int dy = p1.Y - p2.Y;
			return (dx * dx + dy * dy).Sqrt();
		}

		public static Rectangle Subtract(this Rectangle r, Size subtraction)
		{
			return new Rectangle(r.X, r.Y, r.Width - subtraction.Width, r.Height - subtraction.Height);
		}

		public static bool ContainsAny(this string s, IEnumerable<string> elements)
		{
			foreach (string elem in elements)
			{
				if (s.Contains(elem))
				{
					return true;
				}
			}
			return false;
		}

		public static string[] SplitNewLine(this string s)
		{
			return s.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
		}

		public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest) {

			first = list.Count > 0 ? list[0] : default; // or throw
			rest = list.Skip(1).ToList();
		}

		public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest) {
			first = list.Count > 0 ? list[0] : default; // or throw
			second = list.Count > 1 ? list[1] : default; // or throw
			rest = list.Skip(2).ToList();
		}

		public static UInt64 DoubleToLongBits(this double d)
		{
			BitArray a = new BitArray(BitConverter.GetBytes(d));
			UInt64 longBits = 0;
			for (int i = 0; i < a.Length; i++)
			{
				longBits += Convert.ToUInt16(a[i]) * ((UInt64)SysMath.Pow(2, i));
			}
			return longBits;
		}

		public static DateTime RoundToMinute(this DateTime date)
		{
			int second = date.Second;
			if (second >= 30)
			{
				date = date.AddSeconds(60 - second);
			}
			else
			{
				date = date.AddSeconds(-second);
			}
			return date;
		}

		public static TimeSpan Magnify(in this TimeSpan span, int factor)
		{
			return new TimeSpan(span.Ticks * factor);
		}

		public static IEnumerable<T> OfExactType<T>(this IEnumerable collection)
		{
			foreach (object item in collection)
			{
				if (item.GetType() == typeof(T)) yield return (T)item;
			}
		}

		public static string SplitCamelCase(this string input)
		{
			return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
		}

		private static List<Type> ignoredTypes = new List<Type>
		{
			typeof(string),
			typeof(IPAddress)
		};

		public static void Copy<T>(this T from, T to)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings();
			settings.TypeNameHandling = TypeNameHandling.All;
			IEnumerable<PropertyInfo> properties = typeof(T).GetProperties()
				.Where(p => p.GetGetMethod() != null && p.GetSetMethod() != null);
			foreach (PropertyInfo info in properties)
			{
				if (info.GetCustomAttributes(true).Contains(BrowsableFalse))
				{
					continue;
				}
				object value = info.GetGetMethod().Invoke(from, new object[] { });
				if (!info.PropertyType.IsValueType && !ignoredTypes.Contains(info.PropertyType))
				{
					string json = JsonConvert.SerializeObject(value, settings);
					value = JsonConvert.DeserializeObject(json, value.GetType(), settings);
				}
				info.GetSetMethod().Invoke(to, new[] { value });
			}
		}

		public static string GetAssemblyDisplayVersion(this Assembly assembly)
		{
			Version ver = assembly.GetName().Version;
			return $"{ver.Major}.{ver.Minor}";
		}
	}
}
