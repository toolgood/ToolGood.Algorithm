namespace ToolGood.Algorithm.UnitConversion
{
    sealed class VolumeConverter : BaseUnitConverter
    {
        private static UnitFactors units = new UnitFactors()
        {
            { new UnitFactorSynonyms("l", "L", "lt", "ltr", "liter", "litre", "dm³", "dm3", "cubic decimetre", "cubic decimeter","升","立方分米"), 1 },
            { new UnitFactorSynonyms("m³", "m3", "cubic metre","cubic meter", "立方米"), 0.001m },
            { new UnitFactorSynonyms("km³", "km3", "cubic kilometre","cubic kilometer", "立方千米"), 0.001m* 0.001m* 0.001m* 0.001m },
            { new UnitFactorSynonyms("cm³", "cm3", "cubic centimetre","cubic centimeter","立方厘米","毫升"), 1000 },
            { new UnitFactorSynonyms("mm³", "mm3", "cubic millimetre","cubic millimeter", "立方毫米"), 1000000 },
            { new UnitFactorSynonyms("ft³", "ft3", "cubic foot", "cubic feet", "cu ft","立方英尺"), 0.0353147m },
            { new UnitFactorSynonyms("in³", "in3", "cu in", "cubic inch","立方英寸"), 61.0237m },
            { new UnitFactorSynonyms("imperial pint", "imperial pt", "imperial p"), 1.75975m },
            { new UnitFactorSynonyms("imperial gallon", "imperial gal"), 0.219969m },
            { new UnitFactorSynonyms("imperial quart", "imperial qt"), 0.879877m },
            { new UnitFactorSynonyms("US pint", "US pt", "US p"), 2.11337643513819m },
            { new UnitFactorSynonyms("US gallon", "US gal"), 0.264172m },
            { new UnitFactorSynonyms("US quart", "US qt"), 2.11338m },
        };

        public VolumeConverter(string leftUnit, string rightUnit)
        {
            Instantiate(units, leftUnit, rightUnit);
        }

        public static bool Exists(string leftSynonym, string rightSynonym)
        {
            if (units.FindUnit(leftSynonym) != null) {
                return units.FindUnit(rightSynonym) != null;
            }
            return false;
        }
    }

}
