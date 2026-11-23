// PDFsharp - A .NET library for processing PDF
// See the LICENSE file in the solution root for more information.

using System.Reflection;
using PdfSharp.Drawing;

namespace PdfSharp.Fonts
{
    /// <summary>
    /// A font resolver that uses font families embedded in the ResPdfSharp assembly.
    /// It is used automatically as the last-resort fallback when neither a custom font
    /// resolver nor the platform font resolver could resolve a requested typeface.
    /// This gives a predictable, zero-configuration experience on systems without
    /// platform fonts, in particular Linux and containers.
    /// </summary>
    /// <remarks>
    /// The embedded fonts are Arimo (metric-compatible with Arial), Tinos (metric-compatible
    /// with Times New Roman) and Cousine (metric-compatible with Courier New), each in the
    /// four styles regular, bold, italic, and bold italic. They are licensed under the
    /// SIL Open Font License 1.1; the license texts are embedded as resources and can be
    /// retrieved with <see cref="GetFontLicense"/>. See THIRD-PARTY-NOTICES.txt.
    /// </remarks>
    public class BundledFontResolver : IFontResolver
    {
        /// <summary>
        /// The shared instance used by the font system when no other fallback font
        /// resolver was set.
        /// </summary>
        public static BundledFontResolver Shared { get; } = new BundledFontResolver();

        /// <summary>
        /// Sets this resolver as the fallback font resolver of the current application
        /// (<see cref="GlobalFontSettings.FallbackFontResolver"/>). This is normally not
        /// necessary because it is used implicitly as the last resort; call this only if
        /// you want it to act as the fallback in a scenario where you previously installed
        /// your own fallback font resolver by other means.
        /// </summary>
        public static void Install() =>
            GlobalFontSettings.FallbackFontResolver = new BundledFontResolver();

        enum Family { Arimo, Tinos, Cousine }
        enum Style { Regular, Bold, Italic, BoldItalic }

        // Family names embedded in the assembly. The face name passed to GetFont is the
        // file name of the embedded font without extension.
        static string FaceName(Family family, Style style) =>
            family switch
            {
                Family.Arimo => "Arimo",
                Family.Tinos => "Tinos",
                Family.Cousine => "Cousine",
                _ => "Arimo",
            }
            + "-" +
            style switch
            {
                Style.Regular => "Regular",
                Style.Bold => "Bold",
                Style.Italic => "Italic",
                Style.BoldItalic => "BoldItalic",
                _ => "Regular",
            };

        /// <summary>
        /// Maps the requested family to one of the embedded families. Metric-compatible
        /// substitutes are used for the common core web fonts; generic family names like
        /// "sans-serif" and unknown families are mapped to Arimo, with a heuristic for
        /// names containing "mono" or "serif".
        /// </summary>
        static Family MapFamily(string familyName)
        {
            var name = familyName.ToLowerInvariant();

            // Exact metric-compatible substitutions first.
            switch (name)
            {
                case "arial":
                case "arial black":
                case "helvetica":
                case "liberation sans":
                case "arimo":
                    return Family.Arimo;

                case "times new roman":
                case "times":
                case "liberation serif":
                case "tinos":
                    return Family.Tinos;

                case "courier new":
                case "courier":
                case "liberation mono":
                case "cousine":
                    return Family.Cousine;
            }

            // Generic and heuristic mappings. "sans-serif" must be checked before "serif".
            if (name.Contains("mono") || name.Contains("consolas") || name.Contains("menlo"))
                return Family.Cousine;
            if (name.Contains("sans"))
                return Family.Arimo;
            if (name.Contains("serif") || name.Contains("georgia") || name.Contains("times"))
                return Family.Tinos;

            // Last resort: always return a usable font.
            return Family.Arimo;
        }

        /// <summary>
        /// Resolves the typeface to an embedded font face. Never returns null,
        /// so any font request is satisfiable out of the box.
        /// </summary>
        public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (String.IsNullOrEmpty(familyName))
                familyName = "Arial";

            var family = MapFamily(familyName);
            var style = (isBold, isItalic) switch
            {
                (true, true) => Style.BoldItalic,
                (true, false) => Style.Bold,
                (false, true) => Style.Italic,
                _ => Style.Regular,
            };

            // All four styles physically exist, so no style simulation is needed.
            return new FontResolverInfo(FaceName(family, style), XStyleSimulations.None);
        }

        /// <summary>
        /// Gets the bytes of an embedded font or null if the face name is unknown.
        /// </summary>
        public byte[]? GetFont(string faceName) =>
            ReadResource("PdfSharp.Fonts.EmbeddedFonts." + faceName + ".ttf");

        /// <summary>
        /// Gets the license text (SIL OFL 1.1) of one of the embedded font families:
        /// "Arimo", "Tinos" or "Cousine". Distribute this text together with documents
        /// or applications that carry the fonts, as required by the OFL.
        /// </summary>
        public static string GetFontLicense(string family)
        {
            var assembly = typeof(BundledFontResolver).Assembly;
            var suffix = family.ToLowerInvariant() + "-ofl.txt";
            var name = assembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith(suffix, StringComparison.OrdinalIgnoreCase));
            var text = name is null ? null : ReadResource(name);
            return text is null ? "" : System.Text.Encoding.UTF8.GetString(text);
        }

        static byte[]? ReadResource(string logicalName)
        {
            var assembly = typeof(BundledFontResolver).Assembly;
            using var stream = assembly.GetManifestResourceStream(logicalName);
            if (stream is null)
                return null;

            using var memory = new MemoryStream();
            stream.CopyTo(memory);
            return memory.ToArray();
        }
    }
}
