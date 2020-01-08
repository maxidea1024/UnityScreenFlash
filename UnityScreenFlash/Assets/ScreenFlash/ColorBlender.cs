using UnityEngine;

public static class ColorBlender
{
	// 알파를 포함한 두개의 색상을 섞어주기.
	// 아래의 소스코드를 참조했으며, 최적화만 별도로 하였음.
	//
	// Fast and easy way to combine (additive mode) two RGBA colors
	//   https://gist.github.com/rubenvincenten/8c9c2c7961045975b115e6c12a9174c6
	public static Color Blend(Color next, Color prev)
	{
		Color mix;

		// Check if both alpha channels exists.
        if (prev.a > 0f && next.a > 0f)
		{
            mix.a = 1f - ((1f - next.a) * (1f - prev.a));

            //mix.r = Math.round((next.r * next.a / mix.a) + (prev.r * prev.a * (1f - next.a) / mix.a));
            //mix.g = Math.round((next.g * next.a / mix.a) + (prev.g * prev.a * (1f - next.a) / mix.a));
            //mix.b = Math.round((next.b * next.a / mix.a) + (prev.b * prev.a * (1f - next.a) / mix.a));

			// Following codes are optimized version.

			float oneOverMixA = 1f / mix.a;
			float oneMinusnextA = 1f - next.a;
			float nextAlpha = next.a * oneOverMixA;
			float prevAlpha = prev.a * oneMinusnextA * oneOverMixA;

            mix.r = (next.r * nextAlpha) + (prev.r * prevAlpha);
            mix.g = (next.g * nextAlpha) + (prev.g * prevAlpha);
            mix.b = (next.b * nextAlpha) + (prev.b * prevAlpha);

			// Required?
			mix.r = Mathf.Clamp01(mix.r);
			mix.g = Mathf.Clamp01(mix.g);
			mix.b = Mathf.Clamp01(mix.b);
			mix.a = Mathf.Clamp01(mix.a);
        }
		// No previous's alpha but next's alpha is exists
		else if (next.a > 0f)
		{
            mix = next;
        }
		else
		{
            mix = prev;
        }

		return mix;
	}



	//TODO 아래 코드는 차후에...

//	public static Color Multiply(Color backdrop, Color source)
//	{
//		return PerformBlend(
//				backdrop,
//				source,
//				SeparableBlend,
//				SeparableBlendMode.Multiply);
//	}
//
//	public static Color Screen(Color backdrop, Color source)
//	{
//		return PerformBlend(
//				backdrop,
//				source,
//				SeparableBlend,
//				SeparableBlendMode.Screen);
//	}
//
//	public static Color Overlay(Color backdrop, Color source)
//	{
//		return PerformBlend(
//				backdrop,
//				source,
//				SeparableBlend,
//				SeparableBlendMode.Overlay);
//	}
//
//	public static Color Darken(Color backdrop, Color source)
//	{
//		return PerformBlend(
//				backdrop,
//				source,
//				SeparableBlend,
//				SeparableBlendMode.Darken);
//	}
//
//	public static Color Lighten(Color backdrop, Color source)
//	{
//		return PerformBlend(
//				backdrop,
//				source,
//				SeparableBlend,
//				SeparableBlendMode.Lighten);
//	}
//
//	public static Color ColorDodge(Color backdrop, Color source)
//	{
//		return PerformBlend(
//				backdrop,
//				source,
//				SeparableBlend,
//				SeparableBlendMode.ColorDodge);
//	}
//
//	public static Color ColorBurn(Color backdrop, Color source)
//	{
//		return PerformBlend(
//				backdrop,
//				source,
//				SeparableBlend,
//				SeparableBlendMode.ColorBurn);
//	}
//
//	public static Color HardLight(Color backdrop, Color source)
//	{
//		return PerformBlend(
//				backdrop,
//				source,
//				SeparableBlend,
//				SeparableBlendMode.HardLight);
//	}
//
//	public static Color SoftLight(Color backdrop, Color source)
//	{
//		return PerformBlend(
//				backdrop,
//				source,
//				SeparableBlend,
//				SeparableBlendMode.SoftLight);
//	}
//
//	public static Color Difference(Color backdrop, Color source)
//	{
//		return PerformBlend(
//				backdrop,
//				source,
//				SeparableBlend,
//				SeparableBlendMode.Difference);
//	}
//
//	public static Color Exclusion(Color backdrop, Color source)
//	{
//		return PerformBlend(
//				backdrop,
//				source,
//				SeparableBlend,
//				SeparableBlendMode.Exclusion);
//	}
//
//	public static Color Hue(Color backdrop, Color source)
//	{
//		return PerformBlend(
//				backdrop,
//				source,
//				NonSeparableBlend,
//				NonSeparableBlendMode.Hue);
//	}
//
//	public static Color Saturation(Color backdrop, Color source)
//	{
//		return PerformBlend(
//				backdrop,
//				source,
//				NonSeparableBlend,
//				NonSeparableBlendMode.Saturation);
//	}
//
//	public static Color Color(Color backdrop, Color source)
//	{
//		return PerformBlend(
//				backdrop,
//				source,
//				NonSeparableBlend,
//				NonSeparableBlendMode.Color);
//	}
//
//	public static Color Luminosity(Color backdrop, Color source)
//	{
//		return PerformBlend(
//				backdrop,
//				source,
//				NonSeparatableBlend,
//				NonSeparatableBlendMode.Luminosity);
//	}
}
