using System;
using System.Collections.Generic;
using System.IO;

namespace MmiSoft.Core.IO
{
	/// <summary>
	/// Reads a property file containing key value pairs like <c>key:value</c> or <c>key = value</c>
	/// </summary>
	public class PropertyFileReader
	{
		private string filePath;
		private readonly char separator;

		/// <summary>
		/// Creates a new instance with the file name and the separator of each key-value pair
		/// </summary>
		/// <param name="filePath">The file path to read the properties from</param>
		/// <param name="separator">The property separator to be used</param>
		public PropertyFileReader(string filePath, char separator = '=')
		{
			this.filePath = filePath;
			this.separator = separator;
		}

		/// <summary>
		/// Returns a dictionary with the properties contained in the file specified in the constructor
		/// </summary>
		/// <returns>A dictionary containing the properties and their respective values</returns>
		public IDictionary<string, string> ReadProperties()
		{
			return PropertyParser.ReadProperties(File.ReadAllLines(filePath), separator);
		}
	}
}
