//
// Copyright 2010, Novell, Inc.
// Copyright 2011, 2012 Xamarin Inc
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Runtime.InteropServices;

// For now, only support MAC64 for NSRect in order to make sure
// we didn't mess up the 32 bit build
#if MAC64
namespace MonoMac.Foundation {
	[StructLayout(LayoutKind.Sequential)]
	public struct NSRect {
		public NSRect(System.Drawing.RectangleF rect)
		{
			Origin.X = rect.Left;
			Origin.Y = rect.Top;
			Size.Width = rect.Width;
			Size.Height = rect.Height;
		}

		public NSRect(NSPoint location, NSSize size)
		{
			Origin.X = location.X;
			Origin.Y = location.Y;
			Size.Width = size.Width;
			Size.Height = size.Height;
		}

		public NSRect(System.Drawing.Point location, System.Drawing.Size size)
		{
			Origin.X = location.X;
			Origin.Y = location.Y;
			Size.Width = size.Width;
			Size.Height = size.Height;
		}

		public NSRect(double x, double y, double width, double height)
		{
			Origin.X = x;
			Origin.Y = y;
			Size.Width = width;
			Size.Height = height;
		}

		public NSPoint Origin;
		public NSSize Size;

		public NSPoint Location {
			get
			{
				return Origin;
			}
		}

#if MAC64
		public double X { get { return Origin.X; } set { Origin.X=value; } }
		public double Y { get { return Origin.Y; } set { Origin.Y=value; } }
		public double Width { get { return Size.Width; } set { Size.Width = value; } }
		public double Height { get { return Size.Height; } set { Size.Height = value; } }
#else
		public float X { get { return Origin.X; } set { Origin.X=value; } }
		public float Y { get { return Origin.Y; } set { Origin.Y=value; } }
		public float Width { get { return Size.Width; } set { Size.Width = value; } }
		public float Height { get { return Size.Height; } set { Size.Height = value; } }
#endif
	}
}
#endif