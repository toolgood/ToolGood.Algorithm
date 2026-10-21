package specialfunctions

// 本包不能反向依赖 mathnet 根包（会形成循环），故与 C# MathNet/Constants.cs
// 相同的常数在此重复定义，数值保持一致。

const (
	// lnPi ln(π)。
	lnPi = 1.1447298858494001741434273513530587116472948129153
	// sqrt2 √2。
	sqrt2 = 1.4142135623730950488016887242096980785696718753769
	// logTwoSqrtEOverPi ln(2√(e/π))。
	logTwoSqrtEOverPi = 0.6207822376352452223455184457816472122518527279025978
	// twoSqrtEOverPi 2√(e/π)。
	twoSqrtEOverPi = 1.8603827342052657173362492472666631120594218414085755
	// decimalMaxValue C# decimal.MaxValue。
	decimalMaxValue = 79228162514264337593543950335.0
)
