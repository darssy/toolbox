using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MmiSoft.Core.IO
{
	public static class XmlFileIO
	{
		private static readonly ConcurrentDictionary<Type, XmlSerializer> Cache = new();

		public static void WriteXml<T>(string filename, T item)
		{
			using StreamWriter textWriter = new(filename);
			XmlSerializer serializer = Cache.GetOrAdd(typeof(T), t => new XmlSerializer(t));
			serializer.Serialize(textWriter, item);
		}

		public static T ReadXml<T>(string filename)
		{
			using TextReader textReader = new StreamReader(filename);
			XmlSerializer serializer = Cache.GetOrAdd(typeof(T), t => new XmlSerializer(t));
			return (T)serializer.Deserialize(textReader);
		}
	}
}
