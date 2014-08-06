// 
// CGGeometry.cs: CGGeometry.h helpers
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
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
using System.Drawing;
using System.Runtime.InteropServices;

using MonoMac;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;

#if MAC64
using NSInteger = System.Int64;
using NSUInteger = System.UInt64;
using CGFloat = System.Double;
#else
using NSInteger = System.Int32;
using NSUInteger = System.UInt32;
using NSPoint = System.Drawing.PointF;
using NSSize = System.Drawing.SizeF;
using NSRect = System.Drawing.RectangleF;
using CGFloat = System.Single;
#endif

namespace MonoMac.CoreGraphics {

	[Since (3,2)]
	public enum CGRectEdge {
		MinXEdge,
		MinYEdge,
		MaxXEdge,
		MaxYEdge,
	}

	[Since (3,2)]
	public static class RectangleFExtensions {

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern CGFloat CGRectGetMinX (NSRect rect);
		public static CGFloat GetMinX (this NSRect self)
		{
			return CGRectGetMinX (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern CGFloat CGRectGetMidX (NSRect rect);
		public static CGFloat GetMidX (this NSRect self)
		{
			return CGRectGetMidX (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern CGFloat CGRectGetMaxX (NSRect rect);
		public static CGFloat GetMaxX (this NSRect self)
		{
			return CGRectGetMaxX (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern CGFloat CGRectGetMinY (NSRect rect);
		public static CGFloat GetMinY (this NSRect self)
		{
			return CGRectGetMinY (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern CGFloat CGRectGetMidY (NSRect rect);
		public static CGFloat GetMidY (this NSRect self)
		{
			return CGRectGetMidY (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern CGFloat CGRectGetMaxY (NSRect rect);
		public static CGFloat GetMaxY (this NSRect self)
		{
			return CGRectGetMaxY (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern NSRect CGRectStandardize (NSRect rect);
		public static NSRect Standardize (this NSRect self)
		{
			return CGRectStandardize (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern bool CGRectIsNull (NSRect rect);
		public static bool IsNull (this NSRect self)
		{
			return CGRectIsNull (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern bool CGRectIsInfinite (NSRect rect);
		public static bool IsInfinite (this NSRect self)
		{
			return CGRectIsNull (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern NSRect CGRectInset (NSRect rect, CGFloat dx, CGFloat dy);
		public static NSRect Inset (this NSRect self, float dx, float dy)
		{
			return CGRectInset (self, dx, dy);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern NSRect CGRectIntegral (NSRect rect);
		public static NSRect Integral (this NSRect self)
		{
			return CGRectIntegral (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern NSRect CGRectUnion (NSRect r1, NSRect r2);
		public static NSRect UnionWith (this NSRect self, NSRect other)
		{
			return CGRectUnion (self, other);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGRectDivide (NSRect rect, out NSRect slice, out NSRect remainder, CGFloat amount, CGRectEdge edge);
		public static void Divide (this NSRect self, CGFloat amount, CGRectEdge edge, out NSRect slice, out NSRect remainder)
		{
			CGRectDivide (self, out slice, out remainder, amount, edge);
		}
	}
}

