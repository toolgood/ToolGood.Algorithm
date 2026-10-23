// MathNet.cpp - 逐段翻译 csharp/ToolGood.Algorithm/MathNet/（C# 用 decimal，这里用 double）
#include "MathNet.h"

#include <algorithm>
#include <cmath>
#include <limits>
#include <stdexcept>

namespace toolgood {
namespace mn {

namespace {

// ---------------------------------------------------------------------------
// 系数表（直接转写自 SpecialFunctions/SpecialFunctions.cs 与 ModifiedBessel.cs）
// ---------------------------------------------------------------------------
// GammaDk（Lanczos 系数，对应 SpecialFunctions/Gamma.cs）
const double GammaDk[] = {
    2.48574089138753565546e-5, 1.05142378581721974210, -3.45687097222016235469, 4.51227709466894823700, -2.98285225323576655721, 1.05639711577126713077, -1.95428773191645869583e-1, 1.70970543404441224307e-2, -5.71926117404305781283e-4, 4.63399473359905636708e-6, -2.71994908488607703910e-9
};

const double ErfImpAn[] = {
    0.00337916709551257388990745, -0.00073695653048167948530905, -0.374732337392919607868241, 0.0817442448733587196071743, -0.0421089319936548595203468, 0.0070165709512095756344528,
    -0.00495091255982435110337458, 0.000871646599037922480317225
};

const double ErfImpAd[] = {
    1, -0.218088218087924645390535, 0.412542972725442099083918, -0.0841891147873106755410271, 0.0655338856400241519690695, -0.0120019604454941768171266,
    0.00408165558926174048329689, -0.000615900721557769691924509
};

const double ErfImpBn[] = {
    -0.0361790390718262471360258, 0.292251883444882683221149, 0.281447041797604512774415, 0.125610208862766947294894, 0.0274135028268930549240776, 0.00250839672168065762786937
};

const double ErfImpBd[] = {
    1, 1.8545005897903486499845, 1.43575803037831418074962, 0.582827658753036572454135, 0.124810476932949746447682, 0.0113724176546353285778481
};

const double ErfImpCn[] = {
    -0.0397876892611136856954425, 0.153165212467878293257683, 0.191260295600936245503129, 0.10276327061989304213645, 0.029637090615738836726027, 0.0046093486780275489468812,
    0.000307607820348680180548455
};

const double ErfImpCd[] = {
    1, 1.95520072987627704987886, 1.64762317199384860109595, 0.768238607022126250082483, 0.209793185936509782784315, 0.0319569316899913392596356,
    0.00213363160895785378615014
};

const double ErfImpDn[] = {
    -0.0300838560557949717328341, 0.0538578829844454508530552, 0.0726211541651914182692959, 0.0367628469888049348429018, 0.00964629015572527529605267, 0.00133453480075291076745275,
    0.0000778087599782504251917881
};

const double ErfImpDd[] = {
    1, 1.75967098147167528287343, 1.32883571437961120556307, 0.552528596508757581287907, 0.133793056941332861912279, 0.0179509645176280768640766,
    0.00104712440019937356634038, -0.00000000106640381820357337177643
};

const double ErfImpEn[] = {
    -0.0117907570137227847827732, 0.014262132090538809896674, 0.0202234435902960820020765, 0.00930668299990432009042239, 0.00213357802422065994322516, 0.00025022987386460102395382,
    0.0000120534912219588189822126
};

const double ErfImpEd[] = {
    1, 1.50376225203620482047419, 0.965397786204462896346934, 0.339265230476796681555511, 0.0689740649541569716897427, 0.00771060262491768307365526,
    0.000371421101531069302990367
};

const double ErfImpFn[] = {
    -0.00546954795538729307482955, 0.00404190278731707110245394, 0.0054963369553161170521356, 0.00212616472603945399437862, 0.000394984014495083900689956, 0.0000365565477064442377259271,
    0.00000135485897109932323253786
};

const double ErfImpFd[] = {
    1, 1.21019697773630784832251, 0.620914668221143886601045, 0.173038430661142762569515, 0.0276550813773432047594539, 0.00240625974424309709745382,
    0.0000891811817251336577241006, -0.00000000000465528836283382684461025
};

const double ErfImpGn[] = {
    -0.00270722535905778347999196, 0.0013187563425029400461378, 0.00119925933261002333923989, 0.00027849619811344664248235, 0.0000267822988218331849989363, 0.000000923043672315028197865066
};

const double ErfImpGd[] = {
    1, 0.814632808543141591118279, 0.268901665856299542168425, 0.0449877216103041118694989, 0.00381759663320248459168994, 0.000131571897888596914350697,
    0.00000000000404815359675764138445257
};

const double ErfImpHn[] = {
    -0.00109946720691742196814323, 0.000406425442750422675169153, 0.000274499489416900707787024, 0.0000465293770646659383436343, 0.00000320955425395767463401993, 0.0000000778286018145020892261936
};

const double ErfImpHd[] = {
    1, 0.588173710611846046373373, 0.139363331289409746077541, 0.0166329340417083678763028, 0.00100023921310234908642639, 0.000024254837521587225125068
};

const double ErfImpIn[] = {
    -0.00056907993601094962855594, 0.000169498540373762264416984, 0.0000518472354581100890120501, 0.00000382819312231928859704678, 0.0000000824989931281894431781794
};

const double ErfImpId[] = {
    1, 0.339637250051139347430323, 0.043472647870310663055044, 0.00248549335224637114641629, 0.0000535633305337152900549536, -0.000000000000117490944405459578783846
};

const double ErfImpJn[] = {
    -0.000241313599483991337479091, 0.0000574224975202501512365975, 0.0000115998962927383778460557, 0.000000581762134402593739370875, 0.00000000853971555085673614607418
};

const double ErfImpJd[] = {
    1, 0.233044138299687841018015, 0.0204186940546440312625597, 0.000797185647564398289151125, 0.0000117019281670172327758019
};

const double ErfImpKn[] = {
    -0.000146674699277760365803642, 0.0000162666552112280519955647, 0.00000269116248509165239294897, 0.0000000979584479468091935086972, 0.00000000101994647625723465722285
};

const double ErfImpKd[] = {
    1, 0.165907812944847226546036, 0.0103361716191505884359634, 0.000286593026373868366935721, 0.00000298401570840900340874568
};

const double ErfImpLn[] = {
    -0.0000583905797629771786720406, 0.00000412510325105496173512992, 0.000000431790922420250949096906, 0.00000000993365155590013193345569, 0.0000000000653480510020104699270084
};

const double ErfImpLd[] = {
    1, 0.105077086072039915406159, 0.00414278428675475620830226, 0.0000726338754644523769144108, 0.000000477818471047398785369849
};

const double ErfImpMn[] = {
    -0.0000196457797609229579459841, 0.00000157243887666800692441195, 0.0000000543902511192700878690335, 0.000000000317472492369117710852685
};

const double ErfImpMd[] = {
    1, 0.052803989240957632204885, 0.000926876069151753290378112, 0.00000541011723226630257077328, 0.000000000000535093845803642394908747
};

const double ErfImpNn[] = {
    -0.00000789224703978722689089794, 0.000000622088451660986955124162, 0.0000000145728445676882396797184, 0.0000000000603715505542715364529243
};

const double ErfImpNd[] = {
    1, 0.0375328846356293715248719, 0.000467919535974625308126054, 0.00000193847039275845656900547
};

const double ErvInvImpAn[] = {
    -0.000508781949658280665617, -0.00836874819741736770379, 0.0334806625409744615033, -0.0126926147662974029034, -0.0365637971411762664006, 0.0219878681111168899165,
    0.00822687874676915743155, -0.00538772965071242932965
};

const double ErvInvImpAd[] = {
    1, -0.970005043303290640362, -1.56574558234175846809, 1.56221558398423026363, 0.662328840472002992063, -0.71228902341542847553,
    -0.0527396382340099713954, 0.0795283687341571680018, -0.00233393759374190016776, 0.000886216390456424707504
};

const double ErvInvImpBn[] = {
    -0.202433508355938759655, 0.105264680699391713268, 8.37050328343119927838, 17.6447298408374015486, -18.8510648058714251895, -44.6382324441786960818,
    17.445385985570866523, 21.1294655448340526258, -3.67192254707729348546
};

const double ErvInvImpBd[] = {
    1, 6.24264124854247537712, 3.9713437953343869095, -28.6608180499800029974, -20.1432634680485188801, 48.5609213108739935468,
    10.8268667355460159008, -22.6436933413139721736, 1.72114765761200282724
};

const double ErvInvImpCn[] = {
    -0.131102781679951906451, -0.163794047193317060787, 0.117030156341995252019, 0.387079738972604337464, 0.337785538912035898924, 0.142869534408157156766,
    0.0290157910005329060432, 0.00214558995388805277169, -0.000000679465575181126350155, 0.0000000285225331782217055858, -0.000000000681149956853776992068
};

const double ErvInvImpCd[] = {
    1, 3.46625407242567245975, 5.38168345707006855425, 4.77846592945843778382, 2.59301921623620271374, 0.848854343457902036425,
    0.152264338295331783612, 0.01105924229346489121
};

const double ErvInvImpDn[] = {
    -0.0350353787183177984712, -0.00222426529213447927281, 0.0185573306514231072324, 0.00950804701325919603619, 0.00187123492819559223345, 0.000157544617424960554631,
    0.00000460469890584317994083, -0.000000000230404776911882601748, 0.00000000000266339227425782031962
};

const double ErvInvImpDd[] = {
    1, 1.3653349817554063097, 0.762059164553623404043, 0.220091105764131249824, 0.0341589143670947727934, 0.00263861676657015992959,
    0.0000764675292302794483503
};

const double ErvInvImpEn[] = {
    -0.0167431005076633737133, -0.00112951438745580278863, 0.00105628862152492910091, 0.000209386317487588078668, 0.0000149624783758342370182, 0.000000449696789927706453732,
    0.00000000462596163522878599135, -0.000000000000281128735628831791805, 0.00000000000000099055709973310326855
};

const double ErvInvImpEd[] = {
    1, 0.591429344886417493481, 0.138151865749083321638, 0.0160746087093676504695, 0.000964011807005165528527, 0.0000275335474764726041141,
    0.000000282243172016108031869
};

const double ErvInvImpFn[] = {
    -0.0024978212791898131227, -0.00000779190719229053954292, 0.0000254723037413027451751, 0.00000162397777342510920873, 0.0000000396341011304801168516, 0.000000000411632831190944208473,
    0.00000000000145596286718675035587, -0.00000000000000000116765012397184275695
};

const double ErvInvImpFd[] = {
    1, 0.207123112214422517181, 0.0169410838120975906478, 0.000690538265622684595676, 0.0000145007359818232637924, 0.000000144437756628144157666,
    0.000000000509761276599778486139
};

const double ErvInvImpGn[] = {
    -0.000539042911019078575891, -0.00000028398759004727721098, 0.000000899465114892291446442, 0.0000000229345859265920864296, 0.000000000225561444863500149219, 0.000000000000947846627503022684216,
    0.00000000000000135880130108924861008, -0.000000000000000000348890393399948882918
};

const double ErvInvImpGd[] = {
    1, 0.0845746234001899436914, 0.00282092984726264681981, 0.0000468292921940894236786, 0.000000399968812193862100054, 0.00000000161809290887904476097,
    0.00000000000231558608310259605225
};

// 对应 ModifiedBessel.cs BesselI0A
const double BesselI0A[] = {
    -4.41534164647933937950e-18, 3.33079451882223809783e-17, -2.43127984654795469359e-16, 1.71539128555513303061e-15,
    -1.16853328779934516808e-14, 7.67618549860493561688e-14, -4.85644678311192946090e-13, 2.95505266312963983461e-12,
    -1.72682629144155570723e-11, 9.67580903537323691224e-11, -5.18979560163526290666e-10, 2.65982372468238665035e-9,
    -1.30002500998624804212e-8, 6.04699502254191894932e-8, -2.67079385394061173391e-7, 1.11738753912010371815e-6,
    -4.41673835845875056359e-6, 1.64484480707288970893e-5, -5.75419501008210370398e-5, 1.88502885095841655729e-4,
    -5.76375574538582365885e-4, 1.63947561694133579842e-3, -4.32430999505057594430e-3, 1.05464603945949983183e-2,
    -2.37374148058994688156e-2, 4.93052842396707084878e-2, -9.49010970480476444210e-2, 1.71620901522208775349e-1,
    -3.04682672343198398683e-1, 6.76795274409476084995e-1
};

// 对应 ModifiedBessel.cs BesselI0B
const double BesselI0B[] = {
    -7.23318048787475395456e-18, -4.83050448594418207126e-18, 4.46562142029675999901e-17, 3.46122286769746109310e-17,
    -2.82762398051658348494e-16, -3.42548561967721913462e-16, 1.77256013305652638360e-15, 3.81168066935262242075e-15,
    -9.55484669882830764870e-15, -4.15056934728722208663e-14, 1.54008621752140982691e-14, 3.85277838274214270114e-13,
    7.18012445138366623367e-13, -1.79417853150680611778e-12, -1.32158118404477131188e-11, -3.14991652796324136454e-11,
    1.18891471078464383424e-11, 4.94060238822496958910e-10, 3.39623202570838634515e-9, 2.26666899049817806459e-8,
    2.04891858946906374183e-7, 2.89137052083475648297e-6, 6.88975834691682398426e-5, 3.36911647825569408990e-3,
    8.04490411014108831608e-1
};

// 对应 ModifiedBessel.cs BesselI1A
const double BesselI1A[] = {
    2.77791411276104639959e-18, -2.11142121435816608115e-17, 1.55363195773620046921e-16, -1.10559694773538630805e-15,
    7.60068429473540693410e-15, -5.04218550472791168711e-14, 3.22379336594557470981e-13, -1.98397439776494371520e-12,
    1.17361862988909016308e-11, -6.66348972350202774223e-11, 3.62559028155211703701e-10, -1.88724975172282928790e-9,
    9.38153738649577178388e-9, -4.44505912879632808065e-8, 2.00329475355213526229e-7, -8.56872026469545474066e-7,
    3.47025130813767847674e-6, -1.32731636560394358279e-5, 4.78156510755005422638e-5, -1.61760815825896745588e-4,
    5.12285956168575772895e-4, -1.51357245063125314899e-3, 4.15642294431288815669e-3, -1.05640848946261981558e-2,
    2.47264490306265168283e-2, -5.29459812080949914269e-2, 1.02643658689847095384e-1, -1.76416518357834055153e-1,
    2.52587186443633654823e-1
};

// 对应 ModifiedBessel.cs BesselI1B
const double BesselI1B[] = {
    7.51729631084210481353e-18, 4.41434832307170791151e-18, -4.65030536848935832153e-17, -3.20952592199342395980e-17,
    2.96262899764595013876e-16, 3.30820231092092828324e-16, -1.88035477551078244854e-15, -3.81440307243700780478e-15,
    1.04202769841288027642e-14, 4.27244001671195135429e-14, -2.10154184277266431302e-14, -4.08355111109219731823e-13,
    -7.19855177624590851209e-13, 2.03562854414708950722e-12, 1.41258074366137813316e-11, 3.25260358301548823856e-11,
    -1.89749581235054123450e-11, -5.58974346219658380687e-10, -3.83538038596423702205e-9, -2.63146884688951950684e-8,
    -2.51223623787020892529e-7, -3.88256480887769039346e-6, -1.10588938762623716291e-4, -9.76109749136146840777e-3,
    7.78576235018280120474e-1
};

// 对应 ModifiedBessel.cs BesselK0A
const double BesselK0A[] = {
    1.37446543561352307156e-16, 4.25981614279661018399e-14, 1.03496952576338420167e-11, 1.90451637722020886025e-9,
    2.53479107902614945675e-7, 2.28621210311945178607e-5, 1.26461541144692592338e-3, 3.59799365153615016266e-2,
    3.44289899924628486886e-1, -5.35327393233902768720e-1
};

// 对应 ModifiedBessel.cs BesselK0B
const double BesselK0B[] = {
    5.30043377268626276149e-18, -1.64758043015242134646e-17, 5.21039150503902756861e-17, -1.67823109680541210385e-16,
    5.51205597852431940784e-16, -1.84859337734377901440e-15, 6.34007647740507060557e-15, -2.22751332699166985548e-14,
    8.03289077536357521100e-14, -2.98009692317273043925e-13, 1.14034058820847496303e-12, -4.51459788337394416547e-12,
    1.85594911495471785253e-11, -7.95748924447710747776e-11, 3.57739728140030116597e-10, -1.69753450938905987466e-9,
    8.57403401741422608519e-9, -4.66048989768794782956e-8, 2.76681363944501510342e-7, -1.83175552271911948767e-6,
    1.39498137188764993662e-5, -1.28495495816278026384e-4, 1.56988388573005337491e-3, -3.14481013119645005427e-2,
    2.44030308206595545468e0
};

// 对应 ModifiedBessel.cs BesselK1A
const double BesselK1A[] = {
    -7.02386347938628759343e-18, -2.42744985051936593393e-15, -6.66690169419932900609e-13, -1.41148839263352776110e-10,
    -2.21338763073472585583e-8, -2.43340614156596823496e-6, -1.73028895751305206302e-4, -6.97572385963986435018e-3,
    -1.22611180822657148235e-1, -3.53155960776544875667e-1, 1.52530022733894777053e0
};

// 对应 ModifiedBessel.cs BesselK1B
const double BesselK1B[] = {
    -5.75674448366501715755e-18, 1.79405087314755922667e-17, -5.68946255844285935196e-17, 1.83809354436663880070e-16,
    -6.05704724837331885336e-16, 2.03870316562433424052e-15, -7.01983709041831346144e-15, 2.47715442448130437068e-14,
    -8.97670518232499435011e-14, 3.34841966607842919884e-13, -1.28917396095102890680e-12, 5.13963967348173025100e-12,
    -2.12996783842756842877e-11, 9.21831518760500529508e-11, -4.19035475934189648750e-10, 2.01504975519703286596e-9,
    -1.03457624656780970260e-8, 5.74108412545004946722e-8, -3.50196060308781257119e-7, 2.40648494783721712015e-6,
    -1.93619797416608296024e-5, 1.95215518471351631108e-4, -2.85781685962277938680e-3, 1.03923736576817238437e-1,
    2.72062619048444266945e0
};


// 对应 Evaluate.Polynomial
double Polynomial(double z, const double* coefficients, int n) {
    double sum = coefficients[n - 1];
    for (int i = n - 2; i >= 0; --i) {
        sum *= z;
        sum += coefficients[i];
    }
    return sum;
}

// 对应 Evaluate.ChebyshevA（系数按逆序存储，零阶项在数组末尾）
double ChebyshevA(const double* coefficients, int n, double x) {
    int p = 0;
    double b0 = coefficients[p++];
    double b1 = 0;
    int i = n - 1;
    double b2;
    do {
        b2 = b1;
        b1 = b0;
        b0 = x * b1 - b2 + coefficients[p++];
    } while (--i > 0);
    return 0.5 * (b0 - b2);
}

template <size_t N>
double Poly(double z, const double (&coefficients)[N]) {
    return Polynomial(z, coefficients, static_cast<int>(N));
}

template <size_t N>
double Cheb(const double (&coefficients)[N], double x) {
    return ChebyshevA(coefficients, static_cast<int>(N), x);
}

inline double Abs(double x) { return x < 0 ? -x : x; }

inline int SignOf(double x) { return (x > 0) - (x < 0); }

// 对应 Brent.Sign(a,b)
double SignAB(double a, double b) {
    return b >= 0 ? (a >= 0 ? a : -a) : (a >= 0 ? -a : a);
}

// 对应 WEIBULL 中的 EpsilonOf
double EpsilonOf(double value) {
    if (value == 0) return 1e-28;
    double epsilon = 1e-28;
    while (value - epsilon == value && epsilon > 0) {
        epsilon *= 0.1;
    }
    return epsilon;
}

// 对应 WEIBULL 中的 Series（Kahan 求和）
template <typename Fn>
double Series(Fn nextSummand) {
    double compensation = 0.0;
    double current;
    const double factor = 1 << 16;

    double sum = nextSummand();

    do {
        current = nextSummand();
        double y = current - compensation;
        double t = sum + y;
        compensation = t - sum;
        compensation -= y;
        sum = t;
    } while (Abs(sum) < Abs(factor * current));
    return sum;
}

// 对应 WEIBULL 中的 ExponentialMinusOne
double ExponentialMinusOne(double power) {
    double x = Abs(power);
    if (x > 0.1) {
        return std::exp(power) - 1.0;
    }
    if (x < 2 * EpsilonOf(x)) {
        return power;
    }
    // Series expansion to x^k / k!
    int k = 0;
    double term = 1.0;
    return Series([&]() {
        k++;
        term *= power;
        term /= k;
        return term;
    });
}

}  // namespace

// ---------------------------------------------------------------------------
// Precision
// ---------------------------------------------------------------------------
bool Precision::AlmostEqualNormRelative(double a, double b, double diff, double maximumError) {
    if (Abs(a) < DecimalPrecision || Abs(b) < DecimalPrecision) {
        return Abs(diff) < maximumError;
    }
    if ((a == 0 && Abs(b) < maximumError) || (b == 0 && Abs(a) < maximumError)) {
        return true;
    }
    return Abs(diff) < maximumError * std::max(Abs(a), Abs(b));
}

bool Precision::AlmostEqualRelative(double a, double b) {
    return AlmostEqualNormRelative(a, b, a - b, DefaultDecimalAccuracy);
}

bool Precision::AlmostEqual(double a, double b) {
    return AlmostEqualNorm(a, b, a - b, DefaultDecimalAccuracy);
}

bool Precision::AlmostEqualNorm(double a, double b, double diff, double maximumAbsoluteError) {
    return Abs(diff) < maximumAbsoluteError;
}

// ---------------------------------------------------------------------------
// Erf / Erfc / ErfInv / ErfcInv
// ---------------------------------------------------------------------------
namespace {

double ErfImp(double z, bool invert) {
    if (z < 0) {
        if (!invert) {
            return -ErfImp(-z, false);
        }
        if (z < -0.5) {
            return 2 - ErfImp(-z, true);
        }
        return 1 + ErfImp(-z, false);
    }

    double result;

    if (z < 0.5) {
        if (z < 0.0000000001) {
            result = (z * 1.125) + (z * 0.003379167095512573896158903121545171688);
        } else {
            result = (z * 1.125) + (z * Poly(z, ErfImpAn) / Poly(z, ErfImpAd));
        }
    } else if (z < 110) {
        invert = !invert;
        double r, b;
        if (z < 0.75) {
            r = Poly(z - 0.5, ErfImpBn) / Poly(z - 0.5, ErfImpBd);
            b = 0.3440242112;
        } else if (z < 1.25) {
            r = Poly(z - 0.75, ErfImpCn) / Poly(z - 0.75, ErfImpCd);
            b = 0.419990927;
        } else if (z < 2.25) {
            r = Poly(z - 1.25, ErfImpDn) / Poly(z - 1.25, ErfImpDd);
            b = 0.4898625016;
        } else if (z < 3.5) {
            r = Poly(z - 2.25, ErfImpEn) / Poly(z - 2.25, ErfImpEd);
            b = 0.5317370892;
        } else if (z < 5.25) {
            r = Poly(z - 3.5, ErfImpFn) / Poly(z - 3.5, ErfImpFd);
            b = 0.5489973426;
        } else if (z < 8) {
            r = Poly(z - 5.25, ErfImpGn) / Poly(z - 5.25, ErfImpGd);
            b = 0.5571740866;
        } else if (z < 11.5) {
            r = Poly(z - 8, ErfImpHn) / Poly(z - 8, ErfImpHd);
            b = 0.5609807968;
        } else if (z < 17) {
            r = Poly(z - 11.5, ErfImpIn) / Poly(z - 11.5, ErfImpId);
            b = 0.5626493692;
        } else if (z < 24) {
            r = Poly(z - 17, ErfImpJn) / Poly(z - 17, ErfImpJd);
            b = 0.5634598136;
        } else if (z < 38) {
            r = Poly(z - 24, ErfImpKn) / Poly(z - 24, ErfImpKd);
            b = 0.5638477802;
        } else if (z < 60) {
            r = Poly(z - 38, ErfImpLn) / Poly(z - 38, ErfImpLd);
            b = 0.5640528202;
        } else if (z < 85) {
            r = Poly(z - 60, ErfImpMn) / Poly(z - 60, ErfImpMd);
            b = 0.5641309023;
        } else {
            r = Poly(z - 85, ErfImpNn) / Poly(z - 85, ErfImpNd);
            b = 0.5641584396;
        }

        double g = std::exp(-z * z) / z;
        result = (g * b) + (g * r);
    } else {
        result = 0;
        invert = !invert;
    }

    if (invert) {
        result = 1 - result;
    }

    return result;
}

double ErfInvImpl(double p, double q, double s) {
    double result;

    if (p <= 0.5) {
        const double y = 0.0891314744949340820313;
        double g = p * (p + 10);
        double r = Poly(p, ErvInvImpAn) / Poly(p, ErvInvImpAd);
        result = (g * y) + (g * r);
    } else if (q >= 0.25) {
        const double y = 2.249481201171875;
        double g = std::sqrt(-2 * std::log(q));
        double xs = q - 0.25;
        double r = Poly(xs, ErvInvImpBn) / Poly(xs, ErvInvImpBd);
        result = g / (y + r);
    } else {
        double x = std::sqrt(-std::log(q));
        if (x < 3) {
            const double y = 0.807220458984375;
            double xs = x - 1.125;
            double r = Poly(xs, ErvInvImpCn) / Poly(xs, ErvInvImpCd);
            result = (y * x) + (r * x);
        } else if (x < 6) {
            const double y = 0.93995571136474609375;
            double xs = x - 3;
            double r = Poly(xs, ErvInvImpDn) / Poly(xs, ErvInvImpDd);
            result = (y * x) + (r * x);
        } else if (x < 18) {
            const double y = 0.98362827301025390625;
            double xs = x - 6;
            double r = Poly(xs, ErvInvImpEn) / Poly(xs, ErvInvImpEd);
            result = (y * x) + (r * x);
        } else if (x < 44) {
            const double y = 0.99714565277099609375;
            double xs = x - 18;
            double r = Poly(xs, ErvInvImpFn) / Poly(xs, ErvInvImpFd);
            result = (y * x) + (r * x);
        } else {
            const double y = 0.99941349029541015625;
            double xs = x - 44;
            double r = Poly(xs, ErvInvImpGn) / Poly(xs, ErvInvImpGd);
            result = (y * x) + (r * x);
        }
    }

    return s * result;
}

}  // namespace

double SpecialFunctions::Erf(double x) {
    if (x == 0) {
        return 0;
    }
    return ErfImp(x, false);
}

double SpecialFunctions::Erfc(double x) {
    if (x == 0) {
        return 1;
    }
    return ErfImp(x, true);
}

double SpecialFunctions::ErfcInv(double z) {
    if (z <= 0.0) {
        return DecimalMax;
    }
    if (z >= 2.0) {
        return -DecimalMax;
    }

    double p, q, s;
    if (z > 1) {
        q = 2 - z;
        p = 1 - q;
        s = -1;
    } else {
        p = 1 - z;
        q = z;
        s = 1;
    }

    return ErfInvImpl(p, q, s);
}

double SpecialFunctions::ErfInv(double z) {
    if (z == 0.0) {
        return 0.0;
    }
    if (z >= 1.0) {
        return DecimalMax;
    }
    if (z <= -1.0) {
        return -DecimalMax;
    }

    double p, q, s;
    if (z < 0) {
        p = -z;
        q = 1 - p;
        s = -1;
    } else {
        p = z;
        q = 1 - z;
        s = 1;
    }

    return ErfInvImpl(p, q, s);
}

// ---------------------------------------------------------------------------
// Gamma / GammaLn / 不完全伽玛
// ---------------------------------------------------------------------------
namespace {
const int GammaN = 10;
const double GammaR = 10.900511;
}  // namespace

double SpecialFunctions::Gamma(double z) {
    if (z < 0.5) {
        double s = GammaDk[0];
        for (int i = 1; i <= GammaN; i++) {
            s += GammaDk[i] / (i - z);
        }
        return Pi / (std::sin(Pi * z) * s * TwoSqrtEOverPi * std::pow((0.5 - z + GammaR) / E, 0.5 - z));
    } else {
        double s = GammaDk[0];
        for (int i = 1; i <= GammaN; i++) {
            s += GammaDk[i] / (z + i - 1.0);
        }
        return s * TwoSqrtEOverPi * std::pow((z - 0.5 + GammaR) / E, z - 0.5);
    }
}

double SpecialFunctions::GammaLn(double z) {
    if (z < 0.5) {
        double s = GammaDk[0];
        for (int i = 1; i <= GammaN; i++) {
            s += GammaDk[i] / (i - z);
        }
        return LnPi - std::log(std::sin(Pi * z)) - std::log(s) - LogTwoSqrtEOverPi -
               ((0.5 - z) * std::log((0.5 - z + GammaR) / E));
    } else {
        double s = GammaDk[0];
        for (int i = 1; i <= GammaN; i++) {
            s += GammaDk[i] / (z + i - 1.0);
        }
        return std::log(s) + LogTwoSqrtEOverPi + ((z - 0.5) * std::log((z - 0.5 + GammaR) / E));
    }
}

double SpecialFunctions::GammaLowerRegularized(double a, double x) {
    const double epsilon = 0.000000000000001;
    const double big = 4503599627370496.0;
    const double bigInv = 2.22044604925031308085e-16;

    if (Precision::AlmostEqual(a, 0.0)) {
        if (Precision::AlmostEqual(x, 0.0)) {
            return 1;
        }
        return 1;
    }

    if (Precision::AlmostEqual(x, 0.0)) {
        return 0;
    }

    double ax = (a * std::log(x)) - x - GammaLn(a);
    if (ax < -709.78271289338399) {
        return a < x ? 1 : 0;
    }

    if (x <= 1 || x <= a) {
        double r2 = a;
        double c2 = 1;
        double ans2 = 1;

        do {
            r2 = r2 + 1;
            c2 = c2 * x / r2;
            ans2 += c2;
        } while ((c2 / ans2) > epsilon);

        return std::exp(ax) * ans2 / a;
    }

    int c = 0;
    double y = 1 - a;
    double z = x + y + 1;

    double p3 = 1;
    double q3 = x;
    double p2 = x + 1;
    double q2 = z * x;
    double ans = p2 / q2;

    double error;

    do {
        c++;
        y += 1;
        z += 2;
        double yc = y * c;

        double p = (p2 * z) - (p3 * yc);
        double q = (q2 * z) - (q3 * yc);

        if (q != 0) {
            double nextans = p / q;
            error = Abs((ans - nextans) / nextans);
            ans = nextans;
        } else {
            error = 1;
        }

        p3 = p2;
        p2 = p;
        q3 = q2;
        q2 = q;

        if (Abs(p) > big) {
            p3 *= bigInv;
            p2 *= bigInv;
            q3 *= bigInv;
            q2 *= bigInv;
        }
    } while (error > epsilon);

    return 1 - (std::exp(ax) * ans);
}

double SpecialFunctions::GammaLowerRegularizedInv(double a, double y0) {
    const double epsilon = 0.000000000000001;
    const double big = 4503599627370496.0;
    const double threshold = 5 * epsilon;

    if (Precision::AlmostEqual(y0, 0.0)) {
        return 0;
    }

    if (Precision::AlmostEqual(y0, 1.0)) {
        return DecimalMax;
    }

    y0 = 1 - y0;

    double xUpper = big;
    double xLower = 0;
    double yUpper = 1;
    double yLower = 0;

    double d = 1 / (9 * a);
    double y = 1 - d - (0.98 * Sqrt2 * ErfInv((2.0 * y0) - 1.0) * std::sqrt(d));
    double x = a * y * y * y;
    double lgm = GammaLn(a);

    for (int i = 0; i < 10; i++) {
        if (x < xLower || x > xUpper) {
            d = 0.0625;
            break;
        }

        y = 1 - GammaLowerRegularized(a, x);
        if (y < yLower || y > yUpper) {
            d = 0.0625;
            break;
        }

        if (y < y0) {
            xUpper = x;
            yLower = y;
        } else {
            xLower = x;
            yUpper = y;
        }

        d = ((a - 1) * std::log(x)) - x - lgm;
        if (d < -709.78271289338399) {
            d = 0.0625;
            break;
        }

        d = -std::exp(d);
        d = (y - y0) / d;
        if (Abs(d / x) < epsilon) {
            return x;
        }

        if ((d > (x / 4)) && (y0 < 0.05)) {
            d = x / 10;
        }

        x -= d;
    }

    if (xUpper == big) {
        if (x <= 0) {
            x = 1;
        }

        while (xUpper == big) {
            x = (1 + d) * x;
            y = 1 - GammaLowerRegularized(a, x);
            if (y < y0) {
                xUpper = x;
                yLower = y;
                break;
            }

            d = d + d;
        }
    }

    int dir = 0;
    d = 0.5;
    for (int i = 0; i < 400; i++) {
        x = xLower + (d * (xUpper - xLower));
        y = 1 - GammaLowerRegularized(a, x);
        lgm = (xUpper - xLower) / (xLower + xUpper);
        if (Abs(lgm) < threshold) {
            return x;
        }

        lgm = (y - y0) / y0;
        if (Abs(lgm) < threshold) {
            return x;
        }

        if (x <= 0) {
            return 0;
        }

        if (y >= y0) {
            xLower = x;
            yUpper = y;
            if (dir < 0) {
                dir = 0;
                d = 0.5;
            } else {
                if (dir > 1) {
                    d = (0.5 * d) + 0.5;
                } else {
                    d = (y0 - yLower) / (yUpper - yLower);
                }
            }

            dir = dir + 1;
        } else {
            xUpper = x;
            yLower = y;
            if (dir > 0) {
                dir = 0;
                d = 0.5;
            } else {
                if (dir < -1) {
                    d = 0.5 * d;
                } else {
                    d = (y0 - yLower) / (yUpper - yLower);
                }
            }

            dir = dir - 1;
        }
    }

    return x;
}

// ---------------------------------------------------------------------------
// Beta
// ---------------------------------------------------------------------------
double SpecialFunctions::Beta(double a, double b) {
    return std::exp(GammaLn(a) + GammaLn(b) - GammaLn(a + b));
}

double SpecialFunctions::BetaRegularized(double a, double b, double x) {
    // a 或 b 为 0 属于退化情形, 后续 bt*h/a 会出现 0/0, 这里直接按极限值返回以避免除零异常
    if (a == 0.0) {
        return x == 0.0 ? 0.0 : 1.0;
    }

    if (b == 0.0) {
        return x == 1.0 ? 1.0 : 0.0;
    }

    double bt = (x == 0.0 || x == 1.0)
                    ? 0.0
                    : std::exp(GammaLn(a + b) - GammaLn(a) - GammaLn(b) + (a * std::log(x)) +
                               (b * std::log(1.0 - x)));

    bool symmetryTransformation = x >= (a + 1.0) / (a + b + 2.0);

    const double eps = Precision::DecimalPrecision;
    const double conv = Precision::ConvergenceEpsilon;

    if (symmetryTransformation) {
        x = 1.0 - x;
        double swap = a;
        a = b;
        b = swap;
    }

    double qab = a + b;
    double qap = a + 1.0;
    double qam = a - 1.0;
    double c = 1.0;
    double d = 1.0 - (qab * x / qap);

    if (Abs(d) < eps) {
        d = eps;
    }

    d = 1.0 / d;
    double h = d;

    for (int m = 1, m2 = 2; m <= 140; m++, m2 += 2) {
        double aa = m * (b - m) * x / ((qam + m2) * (a + m2));
        d = 1.0 + (aa * d);

        if (Abs(d) < eps) {
            d = eps;
        }

        c = 1.0 + (aa / c);
        if (Abs(c) < eps) {
            c = eps;
        }

        d = 1.0 / d;
        h *= d * c;
        aa = -(a + m) * (qab + m) * x / ((a + m2) * (qap + m2));
        d = 1.0 + (aa * d);

        if (Abs(d) < eps) {
            d = eps;
        }

        c = 1.0 + (aa / c);

        if (Abs(c) < eps) {
            c = eps;
        }

        d = 1.0 / d;
        double del = d * c;
        h *= del;

        if (Abs(del - 1.0) <= conv) {
            return symmetryTransformation ? 1.0 - (bt * h / a) : bt * h / a;
        }
    }

    return symmetryTransformation ? 1.0 - (bt * h / a) : bt * h / a;
}

// ---------------------------------------------------------------------------
// Factorial / Binomial
// ---------------------------------------------------------------------------
double SpecialFunctions::FactorialLn(int x) {
    if (x <= 1) {
        return 0;
    }
    return GammaLn(x + 1);
}

double SpecialFunctions::BinomialLn(int n, int k) {
    if (k < 0 || n < 0 || k > n) {
        return std::numeric_limits<double>::lowest();
    }
    return FactorialLn(n) - FactorialLn(k) - FactorialLn(n - k);
}

// ---------------------------------------------------------------------------
// 贝塞尔函数
// ---------------------------------------------------------------------------
namespace {

double BesselJ0(double x) {
    double ax = Abs(x);
    if (ax < 8.0) {
        double y1 = x * x;
        double ans1 = 57568490574.0 + y1 * (-13362590354.0 + y1 * (651619640.7 + y1 * (-11214424.18 +
                        y1 * (77392.33017 + y1 * (-184.9052456)))));
        double ans2 = 57568490411.0 + y1 * (1029532985.0 + y1 * (9494680.718 + y1 * (59272.64853 +
                        y1 * (267.8532712 + y1 * 1.0))));
        return ans1 / ans2;
    }
    double z = 8.0 / ax;
    double y2 = z * z;
    double xx = ax - 0.78539816339744830962;
    double ans3 = 1.0 + y2 * (-0.1098628627e-2 + y2 * (0.2734510407e-4 + y2 * (-0.2073370639e-5 +
                    y2 * 0.2093887211e-6)));
    double ans4 = -0.1562499995e-1 + y2 * (0.1430488765e-3 + y2 * (-0.6911147651e-5 + y2 * (0.7621095161e-6 -
                    y2 * 0.934935152e-7)));
    return std::sqrt(0.63661977236758134308 / ax) * (std::cos(xx) * ans3 - z * std::sin(xx) * ans4);
}

double BesselJ1(double x) {
    double ax = Abs(x);
    if (ax < 8.0) {
        double y1 = x * x;
        double ans1 = x * (72362614232.0 + y1 * (-7895059235.0 + y1 * (242396853.1 + y1 * (-2972611.439 +
                        y1 * (15704.48260 + y1 * (-30.16036606))))));
        double ans2 = 144725228442.0 + y1 * (2300535178.0 + y1 * (18583304.74 + y1 * (99447.43394 +
                        y1 * (376.9991397 + y1 * 1.0))));
        return ans1 / ans2;
    }
    double z = 8.0 / ax;
    double y2 = z * z;
    double xx = ax - 2.35619449019234492885;
    double ans3 = 1.0 + y2 * (0.183105e-2 + y2 * (-0.3516396496e-4 + y2 * (0.2457520174e-5 +
                    y2 * (-0.240337019e-6))));
    double ans4 = 0.04687499995 + y2 * (-0.2002690873e-3 + y2 * (0.8449199096e-5 + y2 * (-0.88228987e-6 +
                    y2 * 0.105787412e-6)));
    double ans = std::sqrt(0.63661977236758134308 / ax) * (std::cos(xx) * ans3 - z * std::sin(xx) * ans4);
    return (x < 0) ? -ans : ans;
}

double BesselY0(double x) {
    if (x < 8.0) {
        double y1 = x * x;
        double ans1 = -2957821389.0 + y1 * (7062834065.0 + y1 * (-512359803.6 + y1 * (10879881.29 +
                        y1 * (-86327.92757 + y1 * 228.4622733))));
        double ans2 = 40076544269.0 + y1 * (745249964.8 + y1 * (7189466.438 + y1 * (47447.26470 +
                        y1 * (226.1030244 + y1 * 1.0))));
        return (ans1 / ans2) + 0.63661977236758134308 * BesselJ0(x) * std::log(x);
    }
    double z = 8.0 / x;
    double y2 = z * z;
    double xx = x - 0.78539816339744830962;
    double ans3 = 1.0 + y2 * (-0.1098628627e-2 + y2 * (0.2734510407e-4 + y2 * (-0.2073370639e-5 +
                    y2 * 0.2093887211e-6)));
    double ans4 = -0.1562499995e-1 + y2 * (0.1430488765e-3 + y2 * (-0.6911147651e-5 + y2 * (0.7621095161e-6 -
                    y2 * 0.934935152e-7)));
    return std::sqrt(0.63661977236758134308 / x) * (std::sin(xx) * ans3 + z * std::cos(xx) * ans4);
}

double BesselY1(double x) {
    if (x < 8.0) {
        double y1 = x * x;
        double ans1 = x * (-0.4900604943e13 + y1 * (0.1275274390e13 + y1 * (-0.5153438139e11 +
                        y1 * (0.7349264551e9 + y1 * (-0.4237922726e7 + y1 * 0.8511937935e4)))));
        double ans2 = 0.2499580570e14 + y1 * (0.4244419664e12 + y1 * (0.3733650367e10 + y1 * (0.2245904002e8 +
                        y1 * (0.1020426050e6 + y1 * (0.3549632885e3 + y1)))));
        return (ans1 / ans2) + 0.63661977236758134308 * (BesselJ1(x) * std::log(x) - 1.0 / x);
    }
    double z = 8.0 / x;
    double y2 = z * z;
    double xx = x - 2.35619449019234492885;
    double ans3 = 1.0 + y2 * (0.183105e-2 + y2 * (-0.3516396496e-4 + y2 * (0.2457520174e-5 +
                    y2 * (-0.240337019e-6))));
    double ans4 = 0.04687499995 + y2 * (-0.2002690873e-3 + y2 * (0.8449199096e-5 + y2 * (-0.88228987e-6 +
                    y2 * 0.105787412e-6)));
    return std::sqrt(0.63661977236758134308 / x) * (std::sin(xx) * ans3 + z * std::cos(xx) * ans4);
}

double BesselI0(double x) {
    if (x < 0) {
        x = -x;
    }

    if (x <= 8.0) {
        double y = (x / 2.0) - 2.0;
        return std::exp(x) * Cheb(BesselI0A, y);
    }

    double x1 = 32.0 / x - 2.0;
    return std::exp(x) * Cheb(BesselI0B, x1) / std::sqrt(x);
}

double BesselI1(double x) {
    double z = Abs(x);
    if (z <= 8.0) {
        double y = (z / 2.0) - 2.0;
        z = Cheb(BesselI1A, y) * z * std::exp(z);
    } else {
        double x1 = 32.0 / z - 2.0;
        z = std::exp(z) * Cheb(BesselI1B, x1) / std::sqrt(z);
    }

    if (x < 0.0) {
        z = -z;
    }

    return z;
}

double BesselK0(double x) {
    if (x <= 2.0) {
        double y = x * x - 2.0;
        return Cheb(BesselK0A, y) - std::log(0.5 * x) * BesselI0(x);
    }

    double z = 8.0 / x - 2.0;
    return std::exp(-x) * Cheb(BesselK0B, z) / std::sqrt(x);
}

double BesselK1(double x) {
    double z = 0.5 * x;
    if (x <= 2.0) {
        double y = x * x - 2.0;
        return std::log(z) * BesselI1(x) + Cheb(BesselK1A, y) / x;
    }

    double x1 = 8.0 / x - 2.0;
    return std::exp(-x) * Cheb(BesselK1B, x1) / std::sqrt(x);
}

}  // namespace

double SpecialFunctions::BesselJ(int n, double x) {
    if (x == 0) {
        return (n == 0) ? 1.0 : 0.0;
    }

    if (n < 0) n = -n;

    double ax = Abs(x);
    if (ax < 1e-10) {
        return (n == 0) ? 1.0 : 0.0;
    }

    if (n == 0) return BesselJ0(x);
    if (n == 1) return BesselJ1(x);

    if (ax > n) {
        // 递推关系: J_{k+1}(x) = 2k/x * J_k(x) - J_{k-1}(x)
        double J0 = BesselJ0(x);
        double J1 = BesselJ1(x);
        double Jn = 0;

        for (int k = 1; k < n; k++) {
            Jn = (2.0 * k / x) * J1 - J0;
            J0 = J1;
            J1 = Jn;
        }
        return J1;
    }

    // Miller 算法(向下递推)计算 J_n
    int m = static_cast<int>(1.5 * n + 10);
    std::vector<double> J(static_cast<size_t>(m) + 2, 0.0);
    J[static_cast<size_t>(m) + 1] = 0.0;
    J[static_cast<size_t>(m)] = 1.0;

    for (int k = m; k >= 1; k--) {
        J[static_cast<size_t>(k) - 1] = (2.0 * k / x) * J[static_cast<size_t>(k)] - J[static_cast<size_t>(k) + 1];
    }

    double sum = 0.0;
    for (int k = 0; k <= m; k += 2) {
        sum += 2.0 * J[static_cast<size_t>(k)];
    }
    sum -= J[0];

    return J[static_cast<size_t>(n)] / sum;
}

double SpecialFunctions::BesselY(int n, double x) {
    if (n < 0) n = -n;

    if (n == 0) return BesselY0(x);
    if (n == 1) return BesselY1(x);

    // 递推关系: Y_{k+1}(x) = 2k/x * Y_k(x) - Y_{k-1}(x)
    double Y0 = BesselY0(x);
    double Y1 = BesselY1(x);
    double Yn = 0;

    for (int k = 1; k < n; k++) {
        Yn = (2.0 * k / x) * Y1 - Y0;
        Y0 = Y1;
        Y1 = Yn;
    }

    return Y1;
}

double SpecialFunctions::BesselI(int n, double x) {
    if (x < 0) {
        return (n % 2 == 0 ? 1 : -1) * BesselI(n, -x);
    }
    if (x == 0) {
        return (n == 0) ? 1.0 : 0.0;
    }

    double ax = Abs(x);
    if (ax < 1e-10) {
        return (n == 0) ? 1.0 : 0.0;
    }

    if (n < 0) n = -n;

    if (ax > 700) {
        double factor = std::exp(ax) / std::sqrt(2 * Pi * ax);
        return factor * (1.0 - (4.0 * n * n - 1.0) / (8.0 * ax));
    }

    if (n == 0) return BesselI0(x);
    if (n == 1) return BesselI1(x);

    // 递推关系: I_{k+1}(x) = I_{k-1}(x) - 2k/x * I_k(x)
    double I0 = BesselI0(x);
    double I1 = BesselI1(x);
    double In = 0;
    for (int k = 1; k < n; k++) {
        In = I0 - 2.0 * k / x * I1;
        I0 = I1;
        I1 = In;
    }

    return I1;
}

double SpecialFunctions::BesselK(int n, double x) {
    if (n < 0) n = -n;

    if (n == 0) return BesselK0(x);
    if (n == 1) return BesselK1(x);

    // 递推关系: K_{k+1}(x) = K_{k-1}(x) + 2k/x * K_k(x)
    double K0 = BesselK0(x);
    double K1 = BesselK1(x);
    double Kn = 0;
    for (int k = 1; k < n; k++) {
        Kn = K0 + 2.0 * k / x * K1;
        K0 = K1;
        K1 = Kn;
    }

    return K1;
}

// ---------------------------------------------------------------------------
// Brent 求根
// ---------------------------------------------------------------------------
double Brent::FindRoot(const std::function<double(double)>& f, double lowerBound, double upperBound,
                       double accuracy, int maxIterations) {
    double root;
    if (TryFindRoot(f, lowerBound, upperBound, accuracy, maxIterations, root)) {
        return root;
    }
    throw std::runtime_error("RootFindingFailed");
}

bool Brent::TryFindRoot(const std::function<double(double)>& f, double lowerBound, double upperBound,
                        double accuracy, int maxIterations, double& root) {
    double fmin = f(lowerBound);
    double fmax = f(upperBound);
    double froot = fmax;
    double d = 0.0, e = 0.0;

    root = upperBound;
    double xMid = std::numeric_limits<double>::lowest();

    if (SignOf(fmin) == SignOf(fmax)) {
        return false;
    }

    for (int i = 0; i <= maxIterations; i++) {
        if (SignOf(froot) == SignOf(fmax)) {
            upperBound = lowerBound;
            fmax = fmin;
            e = d = root - lowerBound;
        }

        if (Abs(fmax) < Abs(froot)) {
            lowerBound = root;
            root = upperBound;
            upperBound = lowerBound;
            fmin = froot;
            froot = fmax;
            fmax = fmin;
        }

        double xAcc1 = Precision::PositiveDecimalPrecision * Abs(root) + 0.5 * accuracy;
        double xMidOld = xMid;
        xMid = (upperBound - root) / 2.0;

        if (Abs(xMid) <= xAcc1 || Precision::AlmostEqualNormRelative(0, froot, froot, accuracy)) {
            return true;
        }

        if (xMid == xMidOld) {
            return false;
        }

        if (Abs(e) >= xAcc1 && Abs(fmin) > Abs(froot)) {
            double s = froot / fmin;
            double p;
            double q;
            if (Precision::AlmostEqualRelative(lowerBound, upperBound)) {
                p = 2.0 * xMid * s;
                q = 1.0 - s;
            } else {
                q = fmin / fmax;
                double r = froot / fmax;
                p = s * (2.0 * xMid * q * (q - r) - (root - lowerBound) * (r - 1.0));
                q = (q - 1.0) * (r - 1.0) * (s - 1.0);
            }

            if (p > 0.0) {
                q = -q;
            }

            p = Abs(p);
            if (2.0 * p < std::min(3.0 * xMid * q - Abs(xAcc1 * q), Abs(e * q))) {
                e = d;
                d = p / q;
            } else {
                d = xMid;
                e = d;
            }
        } else {
            d = xMid;
            e = d;
        }

        lowerBound = root;
        fmin = froot;
        if (Abs(d) > xAcc1) {
            root += d;
        } else {
            root += SignAB(xAcc1, xMid);
        }

        froot = f(root);
    }

    return false;
}

// ---------------------------------------------------------------------------
// 分布
// ---------------------------------------------------------------------------
namespace dist {

double Normal::CDF(double mean, double stddev, double x) {
    return 0.5 * SpecialFunctions::Erfc((mean - x) / (stddev * Sqrt2));
}

double Normal::InvCDF(double mean, double stddev, double p) {
    return mean - (stddev * Sqrt2 * SpecialFunctions::ErfcInv(2.0 * p));
}

double Normal::PDF(double mean, double stddev, double x) {
    double d = (x - mean) / stddev;
    double dd = d * d;
    // C# 用 decimal 计算，d*d 超出 ±decimal.MaxValue 会抛 OverflowException（上层转 FunctionError）
    if (!(Abs(dd) <= DecimalMax)) {
        return std::numeric_limits<double>::quiet_NaN();
    }
    return std::exp(-0.5 * dd) / (Sqrt2Pi * stddev);
}

double StudentT::CDF(double location, double scale, int freedom, double x) {
    double k = (x - location) / scale;
    double k2 = k * k;
    // 同 Normal::PDF：k*k 溢出 decimal 时 C# 会抛异常
    if (!(Abs(k2) <= DecimalMax)) {
        return std::numeric_limits<double>::quiet_NaN();
    }
    double h = freedom / (freedom + k2);
    double ib = 0.5 * SpecialFunctions::BetaRegularized(freedom / 2.0, 0.5, h);
    return x <= location ? ib : 1.0 - ib;
}

double StudentT::InvCDF(double location, double scale, int freedom, double p) {
    if (p == 0.5) {
        return location;
    }

    return Brent::FindRoot(
        [=](double x) {
            double k = (x - location) / scale;
            double h = freedom / (freedom + (k * k));
            double ib = 0.5 * SpecialFunctions::BetaRegularized(freedom / 2.0, 0.5, h);
            return x <= location ? ib - p : 1.0 - ib - p;
        },
        -800, 800, 1e-12);
}

double FisherSnedecor::CDF(int d1, int d2, double x) {
    return SpecialFunctions::BetaRegularized(d1 / 2.0, d2 / 2.0, d1 * x / (d1 * x + d2));
}

double FisherSnedecor::InvCDF(int d1, int d2, double p) {
    return Brent::FindRoot(
        [=](double x) {
            return SpecialFunctions::BetaRegularized(d1 / 2.0, d2 / 2.0, d1 * x / (d1 * x + d2)) - p;
        },
        0, 1000, 1e-12);
}

double ChiSquare::CDF(int freedom, double x) {
    if (x < 0) {
        return 0.0;
    }
    return SpecialFunctions::GammaLowerRegularized(freedom / 2.0, x / 2.0);
}

double ChiSquare::PDF(int freedom, double x) {
    if (x < 0) {
        return 0.0;
    }
    double k = freedom / 2.0;
    if (x == 0.0) {
        return k < 1.0 ? std::numeric_limits<double>::infinity() : (k == 1.0 ? 0.5 : 0.0);
    }
    return std::exp((k - 1.0) * std::log(x) - x / 2.0 - SpecialFunctions::GammaLn(k) - k * std::log(2.0));
}

double Gamma::CDF(double shape, double rate, double x) {
    if (shape == 0.0 && rate == 0.0) {
        return 0.0;
    }
    return SpecialFunctions::GammaLowerRegularized(shape, x * rate);
}

double Gamma::PDF(double shape, double rate, double x) {
    if (shape == 0.0 && rate == 0.0) {
        return 0.0;
    }

    if (shape == 1.0) {
        return rate * std::exp(-rate * x);
    }

    if (x == 0.0 && shape > 1.0) {
        // x=0 处 pdf 的极限值为 0
        return 0.0;
    }

    if (shape > 160.0) {
        return std::exp(PDFLn(shape, rate, x));
    }

    return std::pow(rate, shape) * std::pow(x, shape - 1.0) * std::exp(-rate * x) / SpecialFunctions::Gamma(shape);
}

double Gamma::PDFLn(double shape, double rate, double x) {
    if (shape == 0.0 && rate == 0.0) {
        return std::numeric_limits<double>::lowest();
    }

    if (shape == 1.0) {
        return std::log(rate) - (rate * x);
    }

    return (shape * std::log(rate)) + ((shape - 1.0) * std::log(x)) - (rate * x) - SpecialFunctions::GammaLn(shape);
}

double Gamma::InvCDF(double shape, double rate, double p) {
    return SpecialFunctions::GammaLowerRegularizedInv(shape, p) / rate;
}

double Beta::CDF(double a, double b, double x) {
    if (x < 0.0) {
        return 0.0;
    }

    if (x >= 1.0) {
        return 1.0;
    }

    if (a == 0.0 && b == 0.0) {
        if (x >= 0.0 && x < 1.0) {
            return 0.5;
        }
        return 1.0;
    }

    if (a == 0.0) {
        return 1.0;
    }

    if (b == 0.0) {
        return x >= 1.0 ? 1.0 : 0.0;
    }

    if (a == 1.0 && b == 1.0) {
        return x;
    }

    return SpecialFunctions::BetaRegularized(a, b, x);
}

double Beta::InvCDF(double a, double b, double p) {
    return Brent::FindRoot(
        [=](double x) { return SpecialFunctions::BetaRegularized(a, b, x) - p; }, 0.0, 1.0, 1e-12);
}

double Binomial::PMF(double p, int n, int k) {
    if (k < 0 || k > n) {
        return 0.0;
    }

    if (p == 0.0) {
        return k == 0 ? 1.0 : 0.0;
    }

    if (p == 1.0) {
        return k == n ? 1.0 : 0.0;
    }

    return std::exp(SpecialFunctions::BinomialLn(n, k) + (k * std::log(p)) + ((n - k) * std::log(1.0 - p)));
}

double Binomial::CDF(double p, int n, double x) {
    if (x < 0.0) {
        return 0.0;
    }

    if (x > n) {
        return 1.0;
    }

    if (p <= 0.0) {
        // p=0 时全部概率集中在 k=0
        return 1.0;
    }

    if (p >= 1.0) {
        // p=1 时全部概率集中在 k=n
        return x >= n ? 1.0 : 0.0;
    }

    if (x >= n) {
        // x=n 时 n-k=0, BetaRegularized 内部会出现 0/0
        return 1.0;
    }

    double k = std::floor(x);
    return SpecialFunctions::BetaRegularized(n - k, k + 1, 1 - p);
}

double Exponential::CDF(double rate, double x) {
    return x < 0.0 ? 0.0 : 1.0 - std::exp(-rate * x);
}

double Exponential::PDF(double rate, double x) {
    return x < 0.0 ? 0.0 : rate * std::exp(-rate * x);
}

double LogNormal::CDF(double mu, double sigma, double x) {
    return x < 0.0 ? 0.0 : 0.5 * (1.0 + SpecialFunctions::Erf((std::log(x) - mu) / (sigma * Sqrt2)));
}

double LogNormal::InvCDF(double mu, double sigma, double p) {
    return p <= 0.0 ? 0.0
                    : p >= 1.0 ? DecimalMax
                               : std::exp(mu - sigma * Sqrt2 * SpecialFunctions::ErfcInv(2.0 * p));
}

double NegativeBinomial::PMF(double r, double p, int k) {
    return std::exp(PMFLn(r, p, k));
}

double NegativeBinomial::PMFLn(double r, double p, int k) {
    return SpecialFunctions::GammaLn(r + k) - SpecialFunctions::GammaLn(r) -
           SpecialFunctions::GammaLn(k + 1.0) + (r * std::log(p)) + (k * std::log(1.0 - p));
}

double Hypergeometric::PMF(int population, int success, int draws, int k) {
    if (k < 0 || k > draws || k > success || k < draws + success - population) {
        return 0;
    }
    double logResult = SpecialFunctions::BinomialLn(success, k) +
                       SpecialFunctions::BinomialLn(population - success, draws - k) -
                       SpecialFunctions::BinomialLn(population, draws);
    return std::exp(logResult);
}

double Poisson::PMF(double lambda, int k) {
    return std::exp(-lambda + (k * std::log(lambda)) - SpecialFunctions::FactorialLn(k));
}

double Poisson::CDF(double lambda, double x) {
    return 1.0 - SpecialFunctions::GammaLowerRegularized(x + 1, lambda);
}

double Weibull::PDF(double shape, double scale, double x) {
    if (x >= 0.0) {
        if (x == 0.0 && shape == 1.0) {
            return shape / scale;
        }
        // 使用 pow(x/scale, shape) 而非 pow(x,shape)*pow(scale,-shape), 避免二者分别溢出/下溢导致的精度损失
        return shape * std::pow(x / scale, shape - 1.0) * std::exp(-std::pow(x / scale, shape)) / scale;
    }
    return 0.0;
}

double Weibull::CDF(double shape, double scale, double x) {
    if (x < 0.0) {
        return 0.0;
    }
    // 同上, 使用 pow(x/scale, shape) 避免中间结果溢出
    return -ExponentialMinusOne(-std::pow(x / scale, shape));
}

}  // namespace dist

// ---------------------------------------------------------------------------
// 统计
// ---------------------------------------------------------------------------
namespace stats {

namespace {

double MinimumInplace(const std::vector<double>& data) {
    double min = std::numeric_limits<double>::max();
    for (size_t i = 0; i < data.size(); i++) {
        if (data[i] < min) {
            min = data[i];
        }
    }
    return min;
}

double MaximumInplace(const std::vector<double>& data) {
    double max = std::numeric_limits<double>::lowest();
    for (size_t i = 0; i < data.size(); i++) {
        if (data[i] > max) {
            max = data[i];
        }
    }
    return max;
}

// 对应 ArrayStatistics.SelectInplace
double SelectInplace(std::vector<double>& a, int rank) {
    if (rank <= 0) {
        return MinimumInplace(a);
    }

    if (rank >= static_cast<int>(a.size()) - 1) {
        return MaximumInplace(a);
    }

    int low = 0;
    int high = static_cast<int>(a.size()) - 1;

    while (true) {
        if (high <= low + 1) {
            if (high == low + 1 && a[static_cast<size_t>(high)] < a[static_cast<size_t>(low)]) {
                std::swap(a[static_cast<size_t>(low)], a[static_cast<size_t>(high)]);
            }
            return a[static_cast<size_t>(rank)];
        }

        int middle = (low + high) >> 1;

        std::swap(a[static_cast<size_t>(middle)], a[static_cast<size_t>(low) + 1]);

        if (a[static_cast<size_t>(low)] > a[static_cast<size_t>(high)]) {
            std::swap(a[static_cast<size_t>(low)], a[static_cast<size_t>(high)]);
        }

        if (a[static_cast<size_t>(low) + 1] > a[static_cast<size_t>(high)]) {
            std::swap(a[static_cast<size_t>(low) + 1], a[static_cast<size_t>(high)]);
        }

        if (a[static_cast<size_t>(low)] > a[static_cast<size_t>(low) + 1]) {
            std::swap(a[static_cast<size_t>(low)], a[static_cast<size_t>(low) + 1]);
        }

        int begin = low + 1;
        int end = high;
        double pivot = a[static_cast<size_t>(begin)];

        while (true) {
            do {
                begin++;
            } while (a[static_cast<size_t>(begin)] < pivot);

            do {
                end--;
            } while (a[static_cast<size_t>(end)] > pivot);

            if (end < begin) {
                break;
            }

            std::swap(a[static_cast<size_t>(begin)], a[static_cast<size_t>(end)]);
        }

        a[static_cast<size_t>(low) + 1] = a[static_cast<size_t>(end)];
        a[static_cast<size_t>(end)] = pivot;

        if (end >= rank) {
            high = end - 1;
        }

        if (end <= rank) {
            low = begin;
        }
    }
}

}  // namespace

double Minimum(const std::vector<double>& data) {
    if (data.empty()) {
        return std::numeric_limits<double>::lowest();
    }
    return MinimumInplace(data);
}

double Maximum(const std::vector<double>& data) {
    if (data.empty()) {
        return std::numeric_limits<double>::lowest();
    }
    return MaximumInplace(data);
}

double QuantileCustom(std::vector<double> data, double tau) {
    if (tau < 0 || tau > 1 || data.empty()) {
        return std::numeric_limits<double>::lowest();
    }

    if (tau == 0 || data.size() == 1) {
        return MinimumInplace(data);
    }

    if (tau == 1) {
        return MaximumInplace(data);
    }

    double h = static_cast<double>(data.size() - 1) * tau + 1.0;
    int hf = static_cast<int>(h);
    double lower = SelectInplace(data, hf - 1);
    double upper = SelectInplace(data, hf);
    return lower + (h - hf) * (upper - lower);
}

double QuantileRank(const std::vector<double>& data, double x) {
    if (x < data[0]) {
        return 0.0;
    }

    if (x >= data[data.size() - 1]) {
        return 1.0;
    }

    int right = static_cast<int>(std::lower_bound(data.begin(), data.end(), x) - data.begin());
    if (data[static_cast<size_t>(right)] == x) {
        int left = right;

        while (left > 0 && data[static_cast<size_t>(left) - 1] == data[static_cast<size_t>(left)]) {
            left--;
        }

        while (right < static_cast<int>(data.size()) - 1 &&
               data[static_cast<size_t>(right) + 1] == data[static_cast<size_t>(right)]) {
            right++;
        }

        return left / static_cast<double>(data.size() - 1);
    } else {
        int left = right - 1;

        double a = left / static_cast<double>(data.size() - 1);
        double b = right / static_cast<double>(data.size() - 1);
        return ((data[static_cast<size_t>(right)] - x) * a + (x - data[static_cast<size_t>(left)]) * b) /
               (data[static_cast<size_t>(right)] - data[static_cast<size_t>(left)]);
    }
}

double Mean(const std::vector<double>& data) {
    if (data.empty()) return 0.0;
    double sum = 0.0;
    for (double v : data) sum += v;
    return sum / static_cast<double>(data.size());
}

double PopulationVariance(const std::vector<double>& data) {
    if (data.empty()) return 0.0;
    double m = Mean(data);
    double sum = 0.0;
    for (double v : data) {
        double d = v - m;
        sum += d * d;
    }
    return sum / static_cast<double>(data.size());
}

double Variance(const std::vector<double>& data) {
    if (data.size() < 2) return 0.0;
    double m = Mean(data);
    double sum = 0.0;
    for (double v : data) {
        double d = v - m;
        sum += d * d;
    }
    return sum / static_cast<double>(data.size() - 1);
}

double Covariance(const std::vector<double>& a, const std::vector<double>& b) {
    size_t n = std::min(a.size(), b.size());
    if (n < 2) return 0.0;
    double ma = 0.0, mb = 0.0;
    for (size_t i = 0; i < n; i++) {
        ma += a[i];
        mb += b[i];
    }
    ma /= static_cast<double>(n);
    mb /= static_cast<double>(n);
    double sum = 0.0;
    for (size_t i = 0; i < n; i++) {
        sum += (a[i] - ma) * (b[i] - mb);
    }
    return sum / static_cast<double>(n - 1);
}

double Correlation(const std::vector<double>& a, const std::vector<double>& b) {
    size_t n = std::min(a.size(), b.size());
    if (n < 2) return 0.0;
    double ma = 0.0, mb = 0.0;
    for (size_t i = 0; i < n; i++) {
        ma += a[i];
        mb += b[i];
    }
    ma /= static_cast<double>(n);
    mb /= static_cast<double>(n);
    double saa = 0.0, sbb = 0.0, sab = 0.0;
    for (size_t i = 0; i < n; i++) {
        double da = a[i] - ma;
        double db = b[i] - mb;
        saa += da * da;
        sbb += db * db;
        sab += da * db;
    }
    if (saa == 0.0 || sbb == 0.0) return 0.0;
    return sab / std::sqrt(saa * sbb);
}

}  // namespace stats

}  // namespace mn
}  // namespace toolgood
