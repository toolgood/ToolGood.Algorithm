namespace ToolGood.Algorithm.UnitConversion
{
    abstract class BaseUnitConverter
    {

        /// <summary>
        /// Set Unit conversions and initial Left/Right conversion
        /// </summary>
        protected void Instantiate(UnitFactors conversionFactors, string leftUnit, string rightUnit)
        {
            units = conversionFactors;

            unitLeft = leftUnit;
            unitRight = rightUnit;
        }

        private UnitFactors units;
        private string unitLeft;
        private string unitRight;


        /// <summary>
        /// Convert the Unit on the Left to the Unit on the Right
        /// </summary>
        /// <param name="value">the Unit's value</param>
        /// <returns>The converted value</returns>
        public decimal LeftToRight(decimal value)
        {
            var startFactor = units.FindFactor(unitLeft);
            var endFactor = units.FindFactor(unitRight);
            var result = (value / startFactor) * endFactor;
            return result;//.CheckCloseEnoughValue();
        }

    }

}
