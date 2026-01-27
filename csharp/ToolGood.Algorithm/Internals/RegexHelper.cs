using System.Text.RegularExpressions;

namespace ToolGood.Algorithm.Internals
{
    internal partial class RegexHelper
    {
#if NETSTANDARD2_1
        /// <summary>
        /// new Regex("^[0-9A-Fa-f]+$", RegexOptions.Compiled);
        /// </summary>
        public static Regex HexRegex = new Regex("^[0-9A-Fa-f]+$", RegexOptions.Compiled);
        /// <summary>
        /// new Regex("^[0-7]+$", RegexOptions.Compiled);
        /// </summary>
        public static Regex OctRegex = new Regex("^[0-7]+$", RegexOptions.Compiled);
        /// <summary>
        /// new Regex("^[01]+$", RegexOptions.Compiled);
        /// </summary>
        public static Regex BinRegex = new Regex("^[01]+$", RegexOptions.Compiled);
#else
        /// <summary>
        /// new Regex("^[0-9A-Fa-f]+$", RegexOptions.Compiled);
        /// </summary>
        public static Regex HexRegex = HexRegex_();
        /// <summary>
        /// new Regex("^[0-7]+$", RegexOptions.Compiled);
        /// </summary>
        public static Regex OctRegex = OctRegex_();
        /// <summary>
        /// new Regex("^[01]+$", RegexOptions.Compiled);
        /// </summary>
        public static Regex BinRegex = BinRegex_();


        [GeneratedRegex("^[0-9A-Fa-f]+$", RegexOptions.Compiled)]
        private static partial Regex HexRegex_();
        [GeneratedRegex("^[0-7]+$", RegexOptions.Compiled)]
        private static partial Regex OctRegex_();
        [GeneratedRegex("^[01]+$", RegexOptions.Compiled)]
        private static partial Regex BinRegex_();
#endif

#if NETSTANDARD2_1
        public static readonly Regex dateTimeRegex = new Regex("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d):([012345]?\\d)$", RegexOptions.Compiled);
        public static readonly Regex dateTimeRegex2 = new Regex("^(\\d{4})/(1[012]|0?\\d)/(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d):([012345]?\\d)$", RegexOptions.Compiled);
        public static readonly Regex dateTimeRegex3 = new Regex("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d)$", RegexOptions.Compiled);
        public static readonly Regex dateTimeRegex4 = new Regex("^(\\d{4})/(1[012]|0?\\d)/(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d)$", RegexOptions.Compiled);

        public static readonly Regex dateRegex = new Regex("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d)$", RegexOptions.Compiled);
        public static readonly Regex dateRegex2 = new Regex("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d)$", RegexOptions.Compiled);

        public static readonly Regex dayTimeRegex = new Regex("^(\\d+) (2[0123]|[01]?\\d):([012345]?\\d):([012345]?\\d)$", RegexOptions.Compiled);
        public static readonly Regex dayTimeRegex2 = new Regex("^(\\d+) (2[0123]|[01]?\\d):([012345]?\\d)$", RegexOptions.Compiled);

        public static readonly Regex timeRegex = new Regex("^(2[0123]|[01]?\\d):([012345]?\\d):([012345]?\\d)$", RegexOptions.Compiled);
        public static readonly Regex timeRegex2 = new Regex("^(2[0123]|[01]?\\d):([012345]?\\d)$", RegexOptions.Compiled);
#else
        public static readonly Regex dateTimeRegex = dateTimeRegex_();
        public static readonly Regex dateTimeRegex2 = dateTimeRegex2_();
        public static readonly Regex dateTimeRegex3 = dateTimeRegex3_();
        public static readonly Regex dateTimeRegex4 = dateTimeRegex4_();

        public static readonly Regex dateRegex = dateRegex_();
        public static readonly Regex dateRegex2 = dateRegex2_();

        public static readonly Regex dayTimeRegex = dayTimeRegex_();
        public static readonly Regex dayTimeRegex2 = dayTimeRegex2_();

        public static readonly Regex timeRegex = timeRegex_();
        public static readonly Regex timeRegex2 = timeRegex2_();

        [GeneratedRegex("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d):([012345]?\\d)$", RegexOptions.Compiled)]
        private static partial Regex dateTimeRegex_();
        [GeneratedRegex("^(\\d{4})/(1[012]|0?\\d)/(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d):([012345]?\\d)$", RegexOptions.Compiled)]
        private static partial Regex dateTimeRegex2_();
        [GeneratedRegex("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d)$", RegexOptions.Compiled)]
        private static partial Regex dateTimeRegex3_();
        [GeneratedRegex("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d)$", RegexOptions.Compiled)]
        private static partial Regex dateRegex_();
        [GeneratedRegex("^(\\d{4})/(1[012]|0?\\d)/(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d)$", RegexOptions.Compiled)]
        private static partial Regex dateTimeRegex4_();
        [GeneratedRegex("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d)$", RegexOptions.Compiled)]
        private static partial Regex dateRegex2_();
        [GeneratedRegex("^(\\d+) (2[0123]|[01]?\\d):([012345]?\\d):([012345]?\\d)$", RegexOptions.Compiled)]
        private static partial Regex dayTimeRegex_();
        [GeneratedRegex("^(\\d+) (2[0123]|[01]?\\d):([012345]?\\d)$", RegexOptions.Compiled)]
        private static partial Regex dayTimeRegex2_();
        [GeneratedRegex("^(2[0123]|[01]?\\d):([012345]?\\d):([012345]?\\d)$", RegexOptions.Compiled)]
        private static partial Regex timeRegex_();
        [GeneratedRegex("^(2[0123]|[01]?\\d):([012345]?\\d)$", RegexOptions.Compiled)]
        private static partial Regex timeRegex2_();

#endif






    }
}
