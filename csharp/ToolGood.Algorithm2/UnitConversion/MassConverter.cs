namespace ToolGood.Algorithm.UnitConversion
{
    sealed class MassConverter : BaseUnitConverter
    {
        private static UnitFactors units = new UnitFactors()
        {
            { new UnitFactorSynonyms("kg", "kilogram","千克"), 1 },
            { new UnitFactorSynonyms("gram", "g","克"), 1000 },
            { new UnitFactorSynonyms("吨", "t"),1/ 1000 },
            { new UnitFactorSynonyms("lb", "lbs", "pound", "pounds","英镑"), 100000000m / 45359237 },
            { new UnitFactorSynonyms("st", "stone","石"), 50000000m / 317514659 },
            { new UnitFactorSynonyms("oz", "ounce","盎司"), 1600000000m / 45359237 },
            { new UnitFactorSynonyms("quintal","英担"), 0.01m },
            { new UnitFactorSynonyms("short ton", "net ton", "us ton","短吨","美吨"), 0.00110231m },
            { new UnitFactorSynonyms("long ton", "weight ton", "gross ton", "imperial ton","长吨","英吨"), 0.000984207m },
        };

        public MassConverter(string leftUnit, string rightUnit)
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
