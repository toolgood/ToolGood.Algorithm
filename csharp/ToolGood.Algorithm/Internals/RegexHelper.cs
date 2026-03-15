using System;
using System.Text.RegularExpressions;

namespace ToolGood.Algorithm.Internals
{
    internal sealed partial class RegexHelper
    {
        public static bool IsHex(string value)
        {
            if(string.IsNullOrEmpty(value)) return false;
            foreach(var c in value) {
                if(!IsHexChar(c)) return false;
            }
            return true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static bool IsHexChar(char c)
        {
            return (uint)(c - '0') <= 9 || (uint)(c - 'A') <= 5 || (uint)(c - 'a') <= 5;
        }

        public static bool IsOct(string value)
        {
            if(string.IsNullOrEmpty(value)) return false;
            foreach(var c in value) {
                if(!IsOctChar(c)) return false;
            }
            return true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static bool IsOctChar(char c)
        {
            return (uint)(c - '0') <= 7;
        }

        public static bool IsBin(string value)
        {
            if(string.IsNullOrEmpty(value)) return false;
            foreach(var c in value) {
                if(!IsBinChar(c)) return false;
            }
            return true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static bool IsBinChar(char c)
        {
            return c == '0' || c == '1';
        }

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
