using System.Runtime.InteropServices;

namespace MmiSoft.Core.Native
{
	public static class Kernel32Native
	{
		/// <summary>
		/// Attaches the calling process to the console of the specified process as a client application.
		/// For details see https://learn.microsoft.com/en-us/windows/console/attachconsole
		/// </summary>
		/// <param name="dwProcessId"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool AttachConsole(int dwProcessId);
	}
}
