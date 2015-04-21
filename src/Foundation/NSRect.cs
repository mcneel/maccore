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

#if MAC64
using nfloat = System.Double;
#else
using nfloat = System.Single;
#endif

namespace MonoMac.Foundation {
	[StructLayout(LayoutKind.Sequential)]
	public struct NSRect {
		
		public static readonly NSRect Empty;
	
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

		public NSRect(nfloat x, nfloat y, nfloat width, nfloat height)
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
		
		public nfloat Left { get { return X; } }
		public nfloat Top { get { return Y; } }
		public nfloat Right { get { return X + Width; } }
		public nfloat Bottom { get { return Y + Height; } }
		
		public override bool Equals(object obj)
		{
			return obj is NSRect && this == (NSRect)obj;
		}

		public override int GetHashCode()
		{
			return Origin.GetHashCode() ^ Size.GetHashCode();
		}
		
		public static bool operator ==(NSRect left, NSRect right)
		{
			return left.Origin == right.Origin && left.Size == right.Size;
		}

		public static bool operator !=(NSRect left, NSRect right)
		{
			return left.Origin != right.Origin || left.Size != right.Size;
		}

		public static implicit operator NSRect (System.Drawing.RectangleF rect)
		{
			return new NSRect (rect.Location, rect.Size);
		}
		
		public static explicit operator System.Drawing.RectangleF (NSRect rect)
		{
			return new System.Drawing.RectangleF ((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
		}

		public nfloat X { get { return Origin.X; } set { Origin.X=value; } }
		public nfloat Y { get { return Origin.Y; } set { Origin.Y=value; } }
		public nfloat Width { get { return Size.Width; } set { Size.Width = value; } }
		public nfloat Height { get { return Size.Height; } set { Size.Height = value; } }
	}
}
#endif