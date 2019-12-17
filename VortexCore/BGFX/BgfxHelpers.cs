using System.Globalization;

namespace VortexCore
{
    public static unsafe partial class Bgfx
    {
		/// <summary>
		/// Clears the debug text buffer.
		/// </summary>
		/// <param name="color">The color with which to clear the background.</param>
		/// <param name="smallText"><c>true</c> to use a small font for debug output; <c>false</c> to use normal sized text.</param>
		public static void DebugTextClear(DebugColor color = DebugColor.Black, bool smallText = false)
		{
			var attr = (byte)((byte)color << 4);
			DbgTextClear(attr, smallText);
		}

		/// <summary>
		/// Writes debug text to the screen.
		/// </summary>
		/// <param name="x">The X position, in cells.</param>
		/// <param name="y">The Y position, in cells.</param>
		/// <param name="foreColor">The foreground color of the text.</param>
		/// <param name="backColor">The background color of the text.</param>
		/// <param name="format">The format of the message.</param>
		/// <param name="args">The arguments with which to format the message.</param>
		public static void DebugTextWrite(int x, int y, DebugColor foreColor, DebugColor backColor, string format, params object[] args)
		{
			DebugTextWrite(x, y, foreColor, backColor, string.Format(CultureInfo.CurrentCulture, format, args));
		}

		/// <summary>
		/// Writes debug text to the screen.
		/// </summary>
		/// <param name="x">The X position, in cells.</param>
		/// <param name="y">The Y position, in cells.</param>
		/// <param name="foreColor">The foreground color of the text.</param>
		/// <param name="backColor">The background color of the text.</param>
		/// <param name="message">The message to write.</param>
		public static void DebugTextWrite(int x, int y, DebugColor foreColor, DebugColor backColor, string message)
		{
			var attr = (byte)(((byte)backColor << 4) | (byte)foreColor);
			DbgTextPrintf((ushort)x, (ushort)y, attr, "%s", message);
		}
	}
}
