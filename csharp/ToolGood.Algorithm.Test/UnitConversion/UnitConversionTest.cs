using PetaTest;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm.Test.UnitConversion
{
    [TestFixture]
    public class UnitConversionTest
    {
        #region 距离单位转换测试

        [Test]
        public void DistanceConverter_MeterToKilometer_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1000m, "m", "km", "测试");
            Assert.AreEqual(1m, result);
        }

        [Test]
        public void DistanceConverter_KilometerToMeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "km", "m", "测试");
            Assert.AreEqual(1000m, result);
        }

        [Test]
        public void DistanceConverter_MeterToCentimeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "m", "cm", "测试");
            Assert.AreEqual(100m, result);
        }

        [Test]
        public void DistanceConverter_MeterToMillimeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "m", "mm", "测试");
            Assert.AreEqual(1000m, result);
        }

        [Test]
        public void DistanceConverter_MeterToDecimeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "m", "dm", "测试");
            Assert.AreEqual(10m, result);
        }

        [Test]
        public void DistanceConverter_CentimeterToMeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(100m, "cm", "m", "测试");
            Assert.AreEqual(1m, result);
        }

        [Test]
        public void DistanceConverter_MillimeterToMeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1000m, "mm", "m", "测试");
            Assert.AreEqual(1m, result);
        }

        [Test]
        public void DistanceConverter_ChineseUnit_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "米", "千米", "测试");
            Assert.AreEqual(0.001m, result);

            result = AlgorithmEngineHelper.UnitConversion(1m, "米", "厘米", "测试");
            Assert.AreEqual(100m, result);

            result = AlgorithmEngineHelper.UnitConversion(1m, "米", "毫米", "测试");
            Assert.AreEqual(1000m, result);
        }

        [Test]
        public void DistanceConverter_FootToMeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "ft", "m", "测试");
            Assert.AreEqual(0.3048, (double)result, 0.0001);
        }

        [Test]
        public void DistanceConverter_InchToMeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "in", "m", "测试");
            Assert.AreEqual(0.0254, (double)result, 0.0001);
        }

        [Test]
        public void DistanceConverter_YardToMeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "yd", "m", "测试");
            Assert.AreEqual(0.9144, (double)result, 0.0001);
        }

        [Test]
        public void DistanceConverter_MileToMeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "mile", "m", "测试");
            Assert.AreEqual(1609.344, (double)result, 0.1);
        }

        #endregion

        #region 面积单位转换测试

        [Test]
        public void AreaConverter_SquareMeterToSquareKilometer_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1000000m, "m2", "km2", "测试");
            Assert.AreEqual(1m, result);
        }

        [Test]
        public void AreaConverter_SquareMeterToSquareCentimeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "m2", "cm2", "测试");
            Assert.AreEqual(10000m, result);
        }

        [Test]
        public void AreaConverter_SquareMeterToSquareMillimeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "m2", "mm2", "测试");
            Assert.AreEqual(1000000m, result);
        }

        [Test]
        public void AreaConverter_SquareMeterToSquareDecimeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "m2", "dm2", "测试");
            Assert.AreEqual(100m, result);
        }

        [Test]
        public void AreaConverter_ChineseUnit_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "平方米", "平方厘米", "测试");
            Assert.AreEqual(10000m, result);

            result = AlgorithmEngineHelper.UnitConversion(1m, "平方米", "平方毫米", "测试");
            Assert.AreEqual(1000000m, result);
        }

        [Test]
        public void AreaConverter_HectareToSquareMeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "ha", "m2", "测试");
            Assert.AreEqual(10000m, result);
        }

        [Test]
        public void AreaConverter_MuToSquareMeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "亩", "m2", "测试");
            Assert.AreEqual(666.667, (double)result, 0.1);
        }

        [Test]
        public void AreaConverter_SquareFootToSquareMeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "ft2", "m2", "测试");
            Assert.AreEqual(0.092903, (double)result, 0.0001);
        }

        #endregion

        #region 体积单位转换测试

        [Test]
        public void VolumeConverter_CubicMeterToLiter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "m3", "L", "测试");
            Assert.AreEqual(1000m, result);
        }

        [Test]
        public void VolumeConverter_LiterToMilliliter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "L", "cm3", "测试");
            Assert.AreEqual(1000m, result);
        }

        [Test]
        public void VolumeConverter_CubicMeterToCubicCentimeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "m3", "cm3", "测试");
            Assert.AreEqual(1000000m, result);
        }

        [Test]
        public void VolumeConverter_CubicMeterToCubicMillimeter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "m3", "mm3", "测试");
            Assert.AreEqual(1000000000m, result);
        }

        [Test]
        public void VolumeConverter_ChineseUnit_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "立方米", "升", "测试");
            Assert.AreEqual(1000m, result);

            result = AlgorithmEngineHelper.UnitConversion(1m, "升", "毫升", "测试");
            Assert.AreEqual(1000m, result);
        }

        [Test]
        public void VolumeConverter_CubicFootToLiter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "ft3", "L", "测试");
            Assert.AreEqual(28.3168, (double)result, 0.1);
        }

        [Test]
        public void VolumeConverter_USGallonToLiter_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "US gallon", "L", "测试");
            Assert.AreEqual(3.78541, (double)result, 0.1);
        }

        #endregion

        #region 质量单位转换测试

        [Test]
        public void MassConverter_KilogramToGram_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "kg", "g", "测试");
            Assert.AreEqual(1000m, result);
        }

        [Test]
        public void MassConverter_KilogramToTon_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1000m, "kg", "t", "测试");
            Assert.AreEqual(1m, result);
        }

        [Test]
        public void MassConverter_TonToKilogram_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "t", "kg", "测试");
            Assert.AreEqual(1000m, result);
        }

        [Test]
        public void MassConverter_GramToKilogram_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1000m, "g", "kg", "测试");
            Assert.AreEqual(1m, result);
        }

        [Test]
        public void MassConverter_ChineseUnit_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "千克", "克", "测试");
            Assert.AreEqual(1000m, result);

            result = AlgorithmEngineHelper.UnitConversion(1m, "吨", "千克", "测试");
            Assert.AreEqual(1000m, result);
        }

        [Test]
        public void MassConverter_PoundToKilogram_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "lb", "kg", "测试");
            Assert.AreEqual(0.453592, (double)result, 0.0001);
        }

        [Test]
        public void MassConverter_OunceToKilogram_Test()
        {
            var result = AlgorithmEngineHelper.UnitConversion(1m, "oz", "kg", "测试");
            Assert.AreEqual(0.0283495, (double)result, 0.0001);
        }

        #endregion

        #region 无效单位测试

        [Test]
        public void UnitConversion_InvalidUnit_Test()
        {
            var hasException = false;
            try {
                var result = AlgorithmEngineHelper.UnitConversion(1m, "invalid", "m", "测试");
            } catch {
                hasException = true;
            }
            Assert.IsTrue(hasException);

            hasException = false;
            try {
                var result = AlgorithmEngineHelper.UnitConversion(1m, "m", "invalid", "测试");
            } catch {
                hasException = true;
            }
            Assert.IsTrue(hasException);
        }

        #endregion
    }
}
