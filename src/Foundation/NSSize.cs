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

// For now, only support MAC64 for NSSize in order to make sure
// we didn't mess up the 32 bit build
#if MAC64

#if MAC64
using CGFloat = System.Double;
#else
using CGFloat = System.Single;
#endif

namespace MonoMac.Foundation {
	[StructLayout(LayoutKind.Sequential)]
	public struct NSSize {
	
		public static readonly NSSize Empty;
		
		public NSSize(System.Drawing.SizeF size)
		{
			Width = size.Width;
			Height = size.Height;
		}
		
		public NSSize(CGFloat width, CGFloat height)
		{
			Width = width;
			Height = height;
		}

#if MAC64
		public NSSize(float width, float height)
		{
			Width = width;
			Height = height;
		}
#endif
		
		public override int GetHashCode()
		{
			return Width.GetHashCode() ^ Height.GetHashCode();
		}

		public static bool operator ==(NSSize left, NSSize right)
		{
			return left.Width == right.Width && left.Height == right.Height;
		}

		public static bool operator !=(NSSize left, NSSize right)
		{
			return left.Width != right.Width || left.Height != right.Height;
		}

		public static NSSize operator +(NSSize size1, NSSize size2)
		{
			return new NSSize(size1.Width + size2.Width, size1.Height + size2.Height);
		}

		public static NSSize operator -(NSSize size1, NSSize size2)
		{
			return new NSSize(size1.Width - size2.Width, size1.Height - size2.Height);
		}

#if MAC64
		public double Width;
		public double Height;
#else
		public float Width;
		public float Height;
#endif
	}
}
#endif