// 
// CGContext.cs: Implements the managed CGContext
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
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
using System.Drawing;
using System.Runtime.InteropServices;

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

	public enum CGLineJoin {
		Miter,
		Round,
		Bevel
	}
	
	public enum CGLineCap {
		Butt,
		Round,
		Square
	}
	
	public enum CGPathDrawingMode {
		Fill,
		EOFill,
		Stroke,
		FillStroke,
		EOFillStroke
	}
	
	public enum CGTextDrawingMode {
		Fill,
		Stroke,
		FillStroke,
		Invisible,
		FillClip,
		StrokeClip,
		FillStrokeClip,
		Clip
	}
	
	public enum CGTextEncoding {
		FontSpecific,
		MacRoman
	}

	public enum CGInterpolationQuality {
		Default,
		None,	
		Low,	
		High,
		Medium		       /* Yes, in this order, since Medium was added in 4 */
	}
	
	public enum CGBlendMode {
		Normal,
		Multiply,
		Screen,
		Overlay,
		Darken,
		Lighten,
		ColorDodge,
		ColorBurn,
		SoftLight,
		HardLight,
		Difference,
		Exclusion,
		Hue,
		Saturation,
		Color,
		Luminosity,
		
		Clear,
		Copy,	
		SourceIn,	
		SourceOut,	
		SourceAtop,		
		DestinationOver,	
		DestinationIn,	
		DestinationOut,	
		DestinationAtop,	
		XOR,		
		PlusDarker,		
		PlusLighter		
	}

	public class CGContext : INativeObject, IDisposable {
		internal IntPtr handle;

		public CGContext (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new Exception ("Invalid parameters to context creation");

			CGContextRetain (handle);
			this.handle = handle;
		}

		internal CGContext ()
		{
		}
		
		[Preserve (Conditional=true)]
		internal CGContext (IntPtr handle, bool owns)
		{
			if (!owns)
				CGContextRetain (handle);

			if (handle == IntPtr.Zero)
				throw new Exception ("Invalid handle");
			
			this.handle = handle;
		}

		~CGContext ()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public IntPtr Handle {
			get { return handle; }
		}
	
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextRelease (IntPtr handle);
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextRetain (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CGContextRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSaveGState (IntPtr context);
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextRestoreGState (IntPtr context);
		
		public void SaveState ()
		{
			CGContextSaveGState (handle);
		}

		public void RestoreState ()
		{
			CGContextRestoreGState (handle);
		}

		//
		// Transformation matrix
		//

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextScaleCTM (IntPtr ctx, CGFloat sx, CGFloat sy);
		public void ScaleCTM (float sx, float sy)
		{
			CGContextScaleCTM (handle, sx, sy);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextTranslateCTM (IntPtr ctx, CGFloat tx, CGFloat ty);
		public void TranslateCTM (float tx, float ty)
		{
			CGContextTranslateCTM (handle, tx, ty);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextRotateCTM (IntPtr ctx, CGFloat angle);
		public void RotateCTM (float angle)
		{
			CGContextRotateCTM (handle, angle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextConcatCTM (IntPtr ctx, CGAffineTransform transform);
		public void ConcatCTM (CGAffineTransform transform)
		{
			CGContextConcatCTM (handle, transform);
		}

		// Settings
		[DllImport (Constants.CoreGraphicsLibrary)]		
		extern static void CGContextSetLineWidth(IntPtr c, CGFloat width);
		public void SetLineWidth (float w)
		{
			CGContextSetLineWidth (handle, w);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetLineCap(IntPtr c, CGLineCap cap);
		public void SetLineCap (CGLineCap cap)
		{
			CGContextSetLineCap (handle, cap);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetLineJoin(IntPtr c, CGLineJoin join);
		public void SetLineJoin (CGLineJoin join)
		{
			CGContextSetLineJoin (handle, join);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetMiterLimit(IntPtr c, CGFloat limit);
		public void SetMiterLimit (float limit)
		{
			CGContextSetMiterLimit (handle, limit);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetLineDash(IntPtr c, CGFloat phase, CGFloat [] lengths, NSInteger count);
		public void SetLineDash (float phase, float [] lengths)
		{
			SetLineDash (phase, lengths, lengths.Length);
		}

		public void SetLineDash (float phase, float [] lengths, int n)
		{
			if (lengths == null)
				n = 0;
			else if (n < 0 || n > lengths.Length)
				throw new ArgumentException ("n");
			CGFloat[] _lengths = new CGFloat[lengths.Length];
			Array.Copy (lengths, _lengths, lengths.Length);
			CGContextSetLineDash (handle, phase, _lengths, n);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetFlatness(IntPtr c, CGFloat flatness);
		public void SetFlatness (float flatness)
		{
			CGContextSetFlatness (handle, flatness);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetAlpha(IntPtr c, CGFloat alpha);
		public void SetAlpha (float alpha)
		{
			CGContextSetAlpha (handle, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetBlendMode(IntPtr context, CGBlendMode mode);
		public void SetBlendMode (CGBlendMode mode)
		{
			CGContextSetBlendMode (handle, mode);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGAffineTransform CGContextGetCTM (IntPtr c);
		public CGAffineTransform GetCTM ()
		{
			return CGContextGetCTM (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextBeginPath(IntPtr c);
		public void BeginPath ()
		{
			CGContextBeginPath (handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextMoveToPoint(IntPtr c, CGFloat x, CGFloat y);
		public void MoveTo (CGFloat x, CGFloat y)
		{
			CGContextMoveToPoint (handle, x, y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddLineToPoint(IntPtr c, CGFloat x, CGFloat y);
		public void AddLineToPoint (CGFloat x, CGFloat y)
		{
			CGContextAddLineToPoint (handle, x, y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddCurveToPoint(IntPtr c, CGFloat cp1x, CGFloat cp1y, CGFloat cp2x, CGFloat cp2y, CGFloat x, CGFloat y);
		public void AddCurveToPoint (CGFloat cp1x, CGFloat cp1y, CGFloat cp2x, CGFloat cp2y, CGFloat x, CGFloat y)
		{
			CGContextAddCurveToPoint (handle, cp1x, cp1y, cp2x, cp2y, x, y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddQuadCurveToPoint(IntPtr c, CGFloat cpx, CGFloat cpy, CGFloat x, CGFloat y);
		public void AddQuadCurveToPoint (CGFloat cpx, CGFloat cpy, CGFloat x, CGFloat y)
		{
			CGContextAddQuadCurveToPoint (handle, cpx, cpy, x, y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClosePath(IntPtr c);
		public void ClosePath ()
		{
			CGContextClosePath (handle);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddRect(IntPtr c, NSRect rect);
		public void AddRect (NSRect rect)
		{
			CGContextAddRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddRects(IntPtr c, NSRect [] rects, IntPtr size_t_count) ;
		public void AddRects (NSRect [] rects)
		{
			CGContextAddRects (handle, rects, new IntPtr(rects.Length));
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddLines(IntPtr c, NSPoint [] points, IntPtr size_t_count) ;
		public void AddLines (NSPoint [] points)
		{
			CGContextAddLines (handle, points, new IntPtr(points.Length));
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddEllipseInRect(IntPtr context, NSRect rect);
		public void AddEllipseInRect (NSRect rect)
		{
			CGContextAddEllipseInRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddArc(IntPtr c, CGFloat x, CGFloat y, CGFloat radius, CGFloat startAngle, CGFloat endAngle, int clockwise);
		public void AddArc (CGFloat x, CGFloat y, CGFloat radius, CGFloat startAngle, CGFloat endAngle, bool clockwise)
		{
			CGContextAddArc (handle, x, y, radius, startAngle, endAngle, clockwise ? 1 : 0);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddArcToPoint(IntPtr c, CGFloat x1, CGFloat y1, CGFloat x2, CGFloat y2, CGFloat radius);
		public void AddArcToPoint (CGFloat x1, CGFloat y1, CGFloat x2, CGFloat y2, CGFloat radius)
		{
			CGContextAddArcToPoint (handle, x1, y1, x2, y2, radius);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddPath(IntPtr context, IntPtr path_ref);
		public void AddPath (CGPath path)
		{
			CGContextAddPath (handle, path.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextReplacePathWithStrokedPath(IntPtr c);
		public void ReplacePathWithStrokedPath ()
		{
			CGContextReplacePathWithStrokedPath (handle);
		}

		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGContextIsPathEmpty(IntPtr c);
		public bool IsPathEmpty ()
		{
			return CGContextIsPathEmpty (handle) != 0;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSPoint CGContextGetPathCurrentPoint(IntPtr c);
		public NSPoint GetPathCurrentPoint ()
		{
			return CGContextGetPathCurrentPoint (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSRect CGContextGetPathBoundingBox(IntPtr c);
		public NSRect GetPathBoundingBox ()
		{
			return CGContextGetPathBoundingBox (handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGContextPathContainsPoint(IntPtr context, NSPoint point, CGPathDrawingMode mode);
		public bool PathContainsPoint (NSPoint point, CGPathDrawingMode mode)
		{
			return CGContextPathContainsPoint (handle, point, mode) != 0;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawPath(IntPtr c, CGPathDrawingMode mode);
		public void DrawPath (CGPathDrawingMode mode)
		{
			CGContextDrawPath (handle, mode);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillPath(IntPtr c);
		public void FillPath ()
		{
			CGContextFillPath (handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextEOFillPath(IntPtr c);
		public void EOFillPath ()
		{
			CGContextEOFillPath (handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokePath(IntPtr c);
		public void StrokePath ()
		{
			CGContextStrokePath (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillRect(IntPtr c, NSRect rect);
		public void FillRect (NSRect rect)
		{
			CGContextFillRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillRects(IntPtr c, NSRect [] rects, IntPtr size_t_count);
		public void ContextFillRects (NSRect [] rects)
		{
			CGContextFillRects (handle, rects, new IntPtr(rects.Length));
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeRect(IntPtr c, NSRect rect);
		public void StrokeRect (NSRect rect)
		{
			CGContextStrokeRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeRectWithWidth(IntPtr c, NSRect rect, CGFloat width);
		public void StrokeRectWithWidth (NSRect rect, CGFloat width)
		{
			CGContextStrokeRectWithWidth (handle, rect, width);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClearRect(IntPtr c, NSRect rect);
		public void ClearRect (NSRect rect)
		{
			CGContextClearRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillEllipseInRect(IntPtr context, NSRect rect);
		public void FillEllipseInRect (NSRect rect)
		{
			CGContextFillEllipseInRect (handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeEllipseInRect(IntPtr context, NSRect rect);
		public void StrokeEllipseInRect (NSRect rect)
		{
			CGContextStrokeEllipseInRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeLineSegments(IntPtr c, NSPoint [] points, IntPtr size_t_count);
		public void StrokeLineSegments (NSPoint [] points)
		{
			CGContextStrokeLineSegments (handle, points, new IntPtr(points.Length));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClip(IntPtr c);
		public void Clip ()
		{
			CGContextClip (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextEOClip(IntPtr c);
		public void EOClip ()
		{
			CGContextEOClip (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClipToMask(IntPtr c, NSRect rect, IntPtr mask);
		public void ClipToMask (NSRect rect, CGImage mask)
		{
			CGContextClipToMask (handle, rect, mask.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSRect CGContextGetClipBoundingBox(IntPtr c);
		public NSRect GetClipBoundingBox ()
		{
			return CGContextGetClipBoundingBox (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClipToRect(IntPtr c, NSRect rect);
		public void ClipToRect (NSRect rect)
		{
			CGContextClipToRect (handle, rect);
		}
		       
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClipToRects(IntPtr c, NSRect [] rects, IntPtr size_t_count);
		public void ClipToRects (NSRect [] rects)
		{
			CGContextClipToRects (handle, rects, new IntPtr(rects.Length));
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFillColorWithColor(IntPtr c, IntPtr color);
		public void SetFillColor (CGColor color)
		{
			CGContextSetFillColorWithColor (handle, color.handle);
		}
		
		[Advice ("Use SetFillColor() instead.")]
		public void SetFillColorWithColor (CGColor color)
		{
			CGContextSetFillColorWithColor (handle, color.handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetStrokeColorWithColor(IntPtr c, IntPtr color);
		public void SetStrokeColor (CGColor color)
		{
			CGContextSetStrokeColorWithColor (handle, color.handle);
		}
		
		[Advice ("Use SetStrokeColor() instead.")]
		public void SetStrokeColorWithColor (CGColor color)
		{
			CGContextSetStrokeColorWithColor (handle, color.handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFillColorSpace(IntPtr context, IntPtr space);
		public void SetFillColorSpace (CGColorSpace space)
		{
			CGContextSetFillColorSpace (handle, space.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetStrokeColorSpace(IntPtr context, IntPtr space);
		public void SetStrokeColorSpace (CGColorSpace space)
		{
			CGContextSetStrokeColorSpace (handle, space.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFillColor(IntPtr context, CGFloat [] components);
		public void SetFillColor (CGFloat [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
			CGContextSetFillColor (handle, components);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetStrokeColor(IntPtr context, CGFloat [] components);
		public void SetStrokeColor (CGFloat [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
			CGContextSetStrokeColor (handle, components);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFillPattern(IntPtr context, IntPtr pattern, CGFloat [] components);
		public void SetFillPattern (CGPattern pattern, CGFloat [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
			CGContextSetFillPattern (handle, pattern.handle, components);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetStrokePattern(IntPtr context, IntPtr pattern, CGFloat [] components);
		public void SetStrokePattern (CGPattern pattern, CGFloat [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
			CGContextSetStrokePattern (handle, pattern.handle, components);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetPatternPhase(IntPtr context, NSSize phase);
		public void SetPatternPhase (NSSize phase)
		{
			CGContextSetPatternPhase (handle, phase);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetGrayFillColor(IntPtr context, CGFloat gray, CGFloat alpha);
		public void SetFillColor (CGFloat gray, CGFloat alpha)
		{
			CGContextSetGrayFillColor (handle, gray, alpha);
		}
		
		[Advice ("Use SetFillColor() instead.")]
		public void SetGrayFillColor (CGFloat gray, CGFloat alpha)
		{
			CGContextSetGrayFillColor (handle, gray, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetGrayStrokeColor(IntPtr context, CGFloat gray, CGFloat alpha);
		public void SetStrokeColor (CGFloat gray, CGFloat alpha)
		{
			CGContextSetGrayStrokeColor (handle, gray, alpha);
		}
		
		[Advice ("Use SetStrokeColor() instead.")]
		public void SetGrayStrokeColor (CGFloat gray, CGFloat alpha)
		{
			CGContextSetGrayStrokeColor (handle, gray, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetRGBFillColor(IntPtr context, CGFloat red, CGFloat green, CGFloat blue, CGFloat alpha);
		public void SetFillColor (CGFloat red, CGFloat green, CGFloat blue, CGFloat alpha)
		{
			CGContextSetRGBFillColor (handle, red, green, blue, alpha);
		}
		
		[Advice ("Use SetFillColor() instead.")]
		public void SetRGBFillColor (CGFloat red, CGFloat green, CGFloat blue, CGFloat alpha)
		{
			CGContextSetRGBFillColor (handle, red, green, blue, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetRGBStrokeColor(IntPtr context, CGFloat red, CGFloat green, CGFloat blue, CGFloat alpha);
		public void SetStrokeColor (CGFloat red, CGFloat green, CGFloat blue, CGFloat alpha)
		{
			CGContextSetRGBStrokeColor (handle, red, green, blue, alpha);
		}
		
		[Advice ("Use SetStrokeColor() instead.")]
		public void SetRGBStrokeColor (CGFloat red, CGFloat green, CGFloat blue, CGFloat alpha)
		{
			CGContextSetRGBStrokeColor (handle, red, green, blue, alpha);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetCMYKFillColor(IntPtr context, CGFloat cyan, CGFloat magenta, CGFloat yellow, CGFloat black, CGFloat alpha);
		public void SetFillColor (CGFloat cyan, CGFloat magenta, CGFloat yellow, CGFloat black, CGFloat alpha)
		{
			CGContextSetCMYKFillColor (handle, cyan, magenta, yellow, black, alpha);
		}
		
		[Advice ("Use SetFillColor() instead.")]
		public void SetCMYKFillColor (CGFloat cyan, CGFloat magenta, CGFloat yellow, CGFloat black, CGFloat alpha)
		{
			CGContextSetCMYKFillColor (handle, cyan, magenta, yellow, black, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetCMYKStrokeColor(IntPtr context, CGFloat cyan, CGFloat magenta, CGFloat yellow, CGFloat black, CGFloat alpha);
		public void SetStrokeColor (CGFloat cyan, CGFloat magenta, CGFloat yellow, CGFloat black, CGFloat alpha)
		{
			CGContextSetCMYKStrokeColor (handle, cyan, magenta, yellow, black, alpha);
		}
		
		[Advice ("Use SetStrokeColor() instead.")]
		public void SetCMYKStrokeColor (CGFloat cyan, CGFloat magenta, CGFloat yellow, CGFloat black, CGFloat alpha)
		{
			CGContextSetCMYKStrokeColor (handle, cyan, magenta, yellow, black, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetRenderingIntent(IntPtr context, CGColorRenderingIntent intent);
		public void SetRenderingIntent (CGColorRenderingIntent intent)
		{
			CGContextSetRenderingIntent (handle, intent);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawImage(IntPtr c, NSRect rect, IntPtr image);
		public void DrawImage (NSRect rect, CGImage image)
		{
			CGContextDrawImage (handle, rect, image.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawTiledImage(IntPtr c, NSRect rect, IntPtr image);
		public void DrawTiledImage (NSRect rect, CGImage image)
		{
			CGContextDrawTiledImage (handle, rect, image.handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGInterpolationQuality CGContextGetInterpolationQuality(IntPtr context);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetInterpolationQuality(IntPtr context, CGInterpolationQuality quality);
		
		public CGInterpolationQuality  InterpolationQuality {
			get {
				return CGContextGetInterpolationQuality (handle);
			}

			set {
				CGContextSetInterpolationQuality (handle, value);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShadowWithColor(IntPtr context, NSSize offset, CGFloat blur, IntPtr color);
		public void SetShadowWithColor (NSSize offset, CGFloat blur, CGColor color)
		{
			CGContextSetShadowWithColor (handle, offset, blur, color.handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShadow(IntPtr context, NSSize offset, CGFloat blur);
		public void SetShadow (NSSize offset, CGFloat blur)
		{
			CGContextSetShadow (handle, offset, blur);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawLinearGradient(IntPtr context, IntPtr gradient, NSPoint startPoint, NSPoint endPoint, CGGradientDrawingOptions options);
		public void DrawLinearGradient (CGGradient gradient, NSPoint startPoint, NSPoint endPoint, CGGradientDrawingOptions options)
		{
			CGContextDrawLinearGradient (handle, gradient.handle, startPoint, endPoint, options);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawRadialGradient (IntPtr context, IntPtr gradient, NSPoint startCenter, CGFloat startRadius,
								NSPoint endCenter, CGFloat endRadius, CGGradientDrawingOptions options);
		public void DrawRadialGradient (CGGradient gradient, NSPoint startCenter, CGFloat startRadius, NSPoint endCenter, CGFloat endRadius, CGGradientDrawingOptions options)
		{
			CGContextDrawRadialGradient (handle, gradient.handle, startCenter, startRadius, endCenter, endRadius, options);
		}
		
#if !COREBUILD
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawShading(IntPtr context, IntPtr shading);
		public void DrawShading (CGShading shading)
		{
			CGContextDrawShading (handle, shading.handle);
		}
#endif		

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetCharacterSpacing(IntPtr context, CGFloat spacing);
		public void SetCharacterSpacing (CGFloat spacing)
		{
			CGContextSetCharacterSpacing (handle, spacing);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetTextPosition(IntPtr c, CGFloat x, CGFloat y);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSPoint CGContextGetTextPosition(IntPtr context);

		public NSPoint TextPosition {
			get {
				return CGContextGetTextPosition (handle);
			}

			set {
				CGContextSetTextPosition (handle, value.X, value.Y);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetTextMatrix(IntPtr c, CGAffineTransform t);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGAffineTransform CGContextGetTextMatrix(IntPtr c);
		public CGAffineTransform TextMatrix {
			get {
				return CGContextGetTextMatrix (handle);
			}
			set {
				CGContextSetTextMatrix (handle, value);
			}
			
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetTextDrawingMode(IntPtr c, CGTextDrawingMode mode);
		public void SetTextDrawingMode (CGTextDrawingMode mode)
		{
			CGContextSetTextDrawingMode (handle, mode);
		}
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFont(IntPtr c, IntPtr font);
		public void SetFont (CGFont font)
		{
			CGContextSetFont (handle, font.handle);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFontSize(IntPtr c, CGFloat size);
		public void SetFontSize (CGFloat size)
		{
			CGContextSetFontSize (handle, size);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSelectFont(IntPtr c, string name, CGFloat size, CGTextEncoding textEncoding);
		public void SelectFont (string name, CGFloat size, CGTextEncoding textEncoding)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			CGContextSelectFont (handle, name, size, textEncoding);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphsAtPositions(IntPtr context, ushort [] glyphs, NSPoint [] positions, IntPtr size_t_count);
		public void ShowGlyphsAtPositions (ushort [] glyphs, NSPoint [] positions, int size_t_count)
		{
			if (positions == null)
				throw new ArgumentNullException ("positions");
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			CGContextShowGlyphsAtPositions (handle, glyphs, positions, new IntPtr(size_t_count));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowText(IntPtr c, string s, IntPtr size_t_length);
		public void ShowText (string str, int count)
		{
			if (str == null)
				throw new ArgumentNullException ("str");
			if (count > str.Length)
				throw new ArgumentException ("count");
			CGContextShowText (handle, str, new IntPtr(count));
		}

		public void ShowText (string str)
		{
			if (str == null)
				throw new ArgumentNullException ("str");
			CGContextShowText (handle, str, new IntPtr(str.Length));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowText(IntPtr c, byte[] bytes, IntPtr size_t_length);
		public void ShowText (byte[] bytes, int count)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			if (count > bytes.Length)
				throw new ArgumentException ("count");
			CGContextShowText (handle, bytes, new IntPtr(count));
		}
		
		public void ShowText (byte[] bytes)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			CGContextShowText (handle, bytes, new IntPtr(bytes.Length));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowTextAtPoint(IntPtr c, CGFloat x, CGFloat y, string str, IntPtr size_t_length);
		public void ShowTextAtPoint (CGFloat x, CGFloat y, string str, int length)
		{
			if (str == null)
				throw new ArgumentNullException ("str");
			CGContextShowTextAtPoint (handle, x, y, str, new IntPtr(length));
		}

		public void ShowTextAtPoint (CGFloat x, CGFloat y, string str)
		{
			if (str == null)
				throw new ArgumentNullException ("str");
			CGContextShowTextAtPoint (handle, x, y, str, new IntPtr(str.Length));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowTextAtPoint(IntPtr c, CGFloat x, CGFloat y, byte[] bytes, IntPtr size_t_length);
		public void ShowTextAtPoint (CGFloat x, CGFloat y, byte[] bytes, int length)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			CGContextShowTextAtPoint (handle, x, y, bytes, new IntPtr(length));
		}
		
		public void ShowTextAtPoint (CGFloat x, CGFloat y, byte[] bytes)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			CGContextShowTextAtPoint (handle, x, y, bytes, new IntPtr(bytes.Length));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphs(IntPtr c, ushort [] glyphs, IntPtr size_t_count);
		public void ShowGlyphs (ushort [] glyphs)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			CGContextShowGlyphs (handle, glyphs, new IntPtr(glyphs.Length));
		}

		public void ShowGlyphs (ushort [] glyphs, int count)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			if (count > glyphs.Length)
				throw new ArgumentException ("count");
			CGContextShowGlyphs (handle, glyphs, new IntPtr(count));
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphsAtPoint(IntPtr context, CGFloat x, CGFloat y, ushort [] glyphs, IntPtr size_t_count);
		public void ShowGlyphsAtPoint (CGFloat x, CGFloat y, ushort [] glyphs, int count)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			if (count > glyphs.Length)
				throw new ArgumentException ("count");
			CGContextShowGlyphsAtPoint (handle, x, y, glyphs, new IntPtr(count));
		}

		public void ShowGlyphsAtPoint (CGFloat x, CGFloat y, ushort [] glyphs)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			
			CGContextShowGlyphsAtPoint (handle, x, y, glyphs, new IntPtr(glyphs.Length));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphsWithAdvances(IntPtr c, ushort [] glyphs, NSSize [] advances, IntPtr size_t_count);
		public void ShowGlyphsWithAdvances (ushort [] glyphs, NSSize [] advances, int count)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			if (advances == null)
				throw new ArgumentNullException ("advances");
			if (count > glyphs.Length || count > advances.Length)
				throw new ArgumentException ("count");
			CGContextShowGlyphsWithAdvances (handle, glyphs, advances, new IntPtr(count));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawPDFPage(IntPtr c, IntPtr page);
		public void DrawPDFPage (CGPDFPage page)
		{
			CGContextDrawPDFPage (handle, page.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGContextBeginPage(IntPtr c, ref NSRect mediaBox);
		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGContextBeginPage(IntPtr c, IntPtr zero);
		public void BeginPage (NSRect? rect)
		{
			if (rect.HasValue){
				NSRect v = rect.Value;
				CGContextBeginPage (handle, ref v);
			} else {
				CGContextBeginPage (handle, IntPtr.Zero);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextEndPage(IntPtr c);
		public void EndPage ()
		{
			CGContextEndPage (handle);
		}
		
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static IntPtr CGContextRetain(IntPtr c);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFlush(IntPtr c);
		public void Flush ()
		{
			CGContextFlush (handle);
		}
		

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSynchronize(IntPtr c);
		public void Synchronize ()
		{
			CGContextSynchronize (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShouldAntialias(IntPtr c, int shouldAntialias);
		public void SetShouldAntialias (bool shouldAntialias)
		{
			CGContextSetShouldAntialias (handle, shouldAntialias ? 1 : 0);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetAllowsAntialiasing(IntPtr context, int allowsAntialiasing);
		public void SetAllowsAntialiasing (bool allowsAntialiasing)
		{
			CGContextSetAllowsAntialiasing (handle, allowsAntialiasing ? 1 : 0);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShouldSmoothFonts(IntPtr c, int shouldSmoothFonts);
		public void SetShouldSmoothFonts (bool shouldSmoothFonts)
		{
			CGContextSetShouldSmoothFonts (handle, shouldSmoothFonts ? 1 : 0);
		}
		
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static void CGContextBeginTransparencyLayer(IntPtr context, CFDictionaryRef auxiliaryInfo);
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static void CGContextBeginTransparencyLayerWithRect(IntPtr context, RectangleF rect, CFDictionaryRef auxiliaryInfo)
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static void CGContextEndTransparencyLayer(IntPtr context);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGAffineTransform CGContextGetUserSpaceToDeviceSpaceTransform(IntPtr context);
		public CGAffineTransform GetUserSpaceToDeviceSpaceTransform()
		{
			return CGContextGetUserSpaceToDeviceSpaceTransform (handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSPoint CGContextConvertPointToDeviceSpace(IntPtr context, NSPoint point);
		public NSPoint PointToDeviceSpace (NSPoint point)
		{
			return CGContextConvertPointToDeviceSpace (handle, point);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSPoint CGContextConvertPointToUserSpace(IntPtr context, NSPoint point);
		public NSPoint ConvertPointToUserSpace (NSPoint point)
		{
			return CGContextConvertPointToUserSpace (handle, point);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSSize CGContextConvertSizeToDeviceSpace(IntPtr context, NSSize size);
		public NSSize ConvertSizeToDeviceSpace (NSSize size)
		{
			return CGContextConvertSizeToDeviceSpace (handle, size);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSSize CGContextConvertSizeToUserSpace(IntPtr context, NSSize size);
		public NSSize ConvertSizeToUserSpace (NSSize size)
		{
			return CGContextConvertSizeToUserSpace (handle, size);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSRect CGContextConvertRectToDeviceSpace(IntPtr context, NSRect rect);
		public NSRect ConvertRectToDeviceSpace (NSRect rect)
		{
			return CGContextConvertRectToDeviceSpace (handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSRect CGContextConvertRectToUserSpace(IntPtr context, NSRect rect);
		public NSRect ConvertRectToUserSpace (NSRect rect)
		{
			return CGContextConvertRectToUserSpace (handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawLayerInRect (IntPtr context, NSRect rect, IntPtr layer);
		public void DrawLayer (CGLayer layer, NSRect rect)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
			CGContextDrawLayerInRect (handle, rect, layer.Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawLayerAtPoint (IntPtr context, NSPoint rect, IntPtr layer);

		public void DrawLayer (CGLayer layer, NSPoint point)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
			CGContextDrawLayerAtPoint (handle, point, layer.Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextCopyPath (IntPtr context);

		[Since (4,0)]
		public CGPath CopyPath ()
		{
			var r = CGContextCopyPath (handle);

			return new CGPath (r, true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextSetAllowsFontSmoothing (IntPtr context, bool allows);
		[Since (4,0)]
		public void SetAllowsFontSmoothing (bool allows)
		{
			CGContextSetAllowsFontSmoothing (handle, allows);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextSetAllowsFontSubpixelPositioning (IntPtr context, bool allows);
		[Since (4,0)]
		public void SetAllowsSubpixelPositioning (bool allows)
		{
			CGContextSetAllowsFontSubpixelPositioning (handle, allows);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextSetAllowsFontSubpixelQuantization (IntPtr context, bool allows);
		[Since (4,0)]
		public void SetAllowsFontSubpixelQuantization (bool allows)
		{
			CGContextSetAllowsFontSubpixelQuantization (handle, allows);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextSetShouldSubpixelPositionFonts (IntPtr context, bool should);
		[Since (4,0)]
		public void SetShouldSubpixelPositionFonts (bool shouldSubpixelPositionFonts)
		{
			CGContextSetShouldSubpixelPositionFonts (handle, shouldSubpixelPositionFonts);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextSetShouldSubpixelQuantizeFonts (IntPtr context, bool should);
		[Since (4,0)]
		public void ShouldSubpixelQuantizeFonts (bool shouldSubpixelQuantizeFonts)
		{
			CGContextSetShouldSubpixelQuantizeFonts (handle, shouldSubpixelQuantizeFonts);
		}

#if !COREBUILD
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextBeginTransparencyLayer (IntPtr context, IntPtr dictionary);
		public void BeginTransparencyLayer ()
		{
			CGContextBeginTransparencyLayer (handle, IntPtr.Zero);
		}
		
		public void BeginTransparencyLayer (NSDictionary auxiliaryInfo = null)
		{
			CGContextBeginTransparencyLayer (handle, auxiliaryInfo == null ? IntPtr.Zero : auxiliaryInfo.Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextBeginTransparencyLayerWithRect (IntPtr context, NSRect rect, IntPtr dictionary);
		public void BeginTransparencyLayer (NSRect rectangle, NSDictionary auxiliaryInfo = null)
		{
			CGContextBeginTransparencyLayerWithRect (handle, rectangle, auxiliaryInfo == null ? IntPtr.Zero : auxiliaryInfo.Handle);
		}

		public void BeginTransparencyLayer (NSRect rectangle)
		{
			CGContextBeginTransparencyLayerWithRect (handle, rectangle, IntPtr.Zero);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextEndTransparencyLayer (IntPtr context);
		public void EndTransparencyLayer ()
		{
			CGContextEndTransparencyLayer (handle);
		}
#endif
	}
}
