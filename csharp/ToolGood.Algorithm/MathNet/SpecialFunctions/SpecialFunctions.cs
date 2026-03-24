using System;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    internal partial class SpecialFunctions
    {
        private static readonly decimal[] ErfImpAn = { 0.00337916709551257388990745M, -0.00073695653048167948530905M, -0.374732337392919607868241M, 0.0817442448733587196071743M, -0.0421089319936548595203468M, 0.0070165709512095756344528M, -0.00495091255982435110337458M, 0.000871646599037922480317225M };
        private static readonly decimal[] ErfImpAd = { 1M, -0.218088218087924645390535M, 0.412542972725442099083918M, -0.0841891147873106755410271M, 0.0655338856400241519690695M, -0.0120019604454941768171266M, 0.00408165558926174048329689M, -0.000615900721557769691924509M };
        private static readonly decimal[] ErfImpBn = { -0.0361790390718262471360258M, 0.292251883444882683221149M, 0.281447041797604512774415M, 0.125610208862766947294894M, 0.0274135028268930549240776M, 0.00250839672168065762786937M };
        private static readonly decimal[] ErfImpBd = { 1M, 1.8545005897903486499845M, 1.43575803037831418074962M, 0.582827658753036572454135M, 0.124810476932949746447682M, 0.0113724176546353285778481M };
        private static readonly decimal[] ErfImpCn = { -0.0397876892611136856954425M, 0.153165212467878293257683M, 0.191260295600936245503129M, 0.10276327061989304213645M, 0.029637090615738836726027M, 0.0046093486780275489468812M, 0.000307607820348680180548455M };
        private static readonly decimal[] ErfImpCd = { 1M, 1.95520072987627704987886M, 1.64762317199384860109595M, 0.768238607022126250082483M, 0.209793185936509782784315M, 0.0319569316899913392596356M, 0.00213363160895785378615014M };
        private static readonly decimal[] ErfImpDn = { -0.0300838560557949717328341M, 0.0538578829844454508530552M, 0.0726211541651914182692959M, 0.0367628469888049348429018M, 0.00964629015572527529605267M, 0.00133453480075291076745275M, 0.0000778087599782504251917881M };
        private static readonly decimal[] ErfImpDd = { 1M, 1.75967098147167528287343M, 1.32883571437961120556307M, 0.552528596508757581287907M, 0.133793056941332861912279M, 0.0179509645176280768640766M, 0.00104712440019937356634038M, -0.00000000106640381820357337177643M };
        private static readonly decimal[] ErfImpEn = { -0.0117907570137227847827732M, 0.014262132090538809896674M, 0.0202234435902960820020765M, 0.00930668299990432009042239M, 0.00213357802422065994322516M, 0.00025022987386460102395382M, 0.0000120534912219588189822126M };
        private static readonly decimal[] ErfImpEd = { 1M, 1.50376225203620482047419M, 0.965397786204462896346934M, 0.339265230476796681555511M, 0.0689740649541569716897427M, 0.00771060262491768307365526M, 0.000371421101531069302990367M };
        private static readonly decimal[] ErfImpFn = { -0.00546954795538729307482955M, 0.00404190278731707110245394M, 0.0054963369553161170521356M, 0.00212616472603945399437862M, 0.000394984014495083900689956M, 0.0000365565477064442377259271M, 0.00000135485897109932323253786M };
        private static readonly decimal[] ErfImpFd = { 1M, 1.21019697773630784832251M, 0.620914668221143886601045M, 0.173038430661142762569515M, 0.0276550813773432047594539M, 0.00240625974424309709745382M, 0.0000891811817251336577241006M, -0.00000000000465528836283382684461025M };
        private static readonly decimal[] ErfImpGn = { -0.00270722535905778347999196M, 0.0013187563425029400461378M, 0.00119925933261002333923989M, 0.00027849619811344664248235M, 0.0000267822988218331849989363M, 0.000000923043672315028197865066M };
        private static readonly decimal[] ErfImpGd = { 1M, 0.814632808543141591118279M, 0.268901665856299542168425M, 0.0449877216103041118694989M, 0.00381759663320248459168994M, 0.000131571897888596914350697M, 0.00000000000404815359675764138445257M };
        private static readonly decimal[] ErfImpHn = { -0.00109946720691742196814323M, 0.000406425442750422675169153M, 0.000274499489416900707787024M, 0.0000465293770646659383436343M, 0.00000320955425395767463401993M, 0.0000000778286018145020892261936M };
        private static readonly decimal[] ErfImpHd = { 1M, 0.588173710611846046373373M, 0.139363331289409746077541M, 0.0166329340417083678763028M, 0.00100023921310234908642639M, 0.000024254837521587225125068M };
        private static readonly decimal[] ErfImpIn = { -0.00056907993601094962855594M, 0.000169498540373762264416984M, 0.0000518472354581100890120501M, 0.00000382819312231928859704678M, 0.0000000824989931281894431781794M };
        private static readonly decimal[] ErfImpId = { 1M, 0.339637250051139347430323M, 0.043472647870310663055044M, 0.00248549335224637114641629M, 0.0000535633305337152900549536M, -0.000000000000117490944405459578783846M };
        private static readonly decimal[] ErfImpJn = { -0.000241313599483991337479091M, 0.0000574224975202501512365975M, 0.0000115998962927383778460557M, 0.000000581762134402593739370875M, 0.00000000853971555085673614607418M };
        private static readonly decimal[] ErfImpJd = { 1M, 0.233044138299687841018015M, 0.0204186940546440312625597M, 0.000797185647564398289151125M, 0.0000117019281670172327758019M };
        private static readonly decimal[] ErfImpKn = { -0.000146674699277760365803642M, 0.0000162666552112280519955647M, 0.00000269116248509165239294897M, 0.0000000979584479468091935086972M, 0.00000000101994647625723465722285M };
        private static readonly decimal[] ErfImpKd = { 1M, 0.165907812944847226546036M, 0.0103361716191505884359634M, 0.000286593026373868366935721M, 0.00000298401570840900340874568M };
        private static readonly decimal[] ErfImpLn = { -0.0000583905797629771786720406M, 0.00000412510325105496173512992M, 0.000000431790922420250949096906M, 0.00000000993365155590013193345569M, 0.0000000000653480510020104699270084M };
        private static readonly decimal[] ErfImpLd = { 1M, 0.105077086072039915406159M, 0.00414278428675475620830226M, 0.0000726338754644523769144108M, 0.000000477818471047398785369849M };
        private static readonly decimal[] ErfImpMn = { -0.0000196457797609229579459841M, 0.00000157243887666800692441195M, 0.0000000543902511192700878690335M, 0.000000000317472492369117710852685M };
        private static readonly decimal[] ErfImpMd = { 1M, 0.052803989240957632204885M, 0.000926876069151753290378112M, 0.00000541011723226630257077328M, 0.000000000000535093845803642394908747M };
        private static readonly decimal[] ErfImpNn = { -0.00000789224703978722689089794M, 0.000000622088451660986955124162M, 0.0000000145728445676882396797184M, 0.0000000000603715505542715364529243M };
        private static readonly decimal[] ErfImpNd = { 1M, 0.0375328846356293715248719M, 0.000467919535974625308126054M, 0.00000193847039275845656900547M };

        private static readonly decimal[] ErvInvImpAn = { -0.000508781949658280665617M, -0.00836874819741736770379M, 0.0334806625409744615033M, -0.0126926147662974029034M, -0.0365637971411762664006M, 0.0219878681111168899165M, 0.00822687874676915743155M, -0.00538772965071242932965M };
        private static readonly decimal[] ErvInvImpAd = { 1M, -0.970005043303290640362M, -1.56574558234175846809M, 1.56221558398423026363M, 0.662328840472002992063M, -0.71228902341542847553M, -0.0527396382340099713954M, 0.0795283687341571680018M, -0.00233393759374190016776M, 0.000886216390456424707504M };
        private static readonly decimal[] ErvInvImpBn = { -0.202433508355938759655M, 0.105264680699391713268M, 8.37050328343119927838M, 17.6447298408374015486M, -18.8510648058714251895M, -44.6382324441786960818M, 17.445385985570866523M, 21.1294655448340526258M, -3.67192254707729348546M };
        private static readonly decimal[] ErvInvImpBd = { 1M, 6.24264124854247537712M, 3.9713437953343869095M, -28.6608180499800029974M, -20.1432634680485188801M, 48.5609213108739935468M, 10.8268667355460159008M, -22.6436933413139721736M, 1.72114765761200282724M };
        private static readonly decimal[] ErvInvImpCn = { -0.131102781679951906451M, -0.163794047193317060787M, 0.117030156341995252019M, 0.387079738972604337464M, 0.337785538912035898924M, 0.142869534408157156766M, 0.0290157910005329060432M, 0.00214558995388805277169M, -0.000000679465575181126350155M, 0.0000000285225331782217055858M, -0.000000000681149956853776992068M };
        private static readonly decimal[] ErvInvImpCd = { 1M, 3.46625407242567245975M, 5.38168345707006855425M, 4.77846592945843778382M, 2.59301921623620271374M, 0.848854343457902036425M, 0.152264338295331783612M, 0.01105924229346489121M };
        private static readonly decimal[] ErvInvImpDn = { -0.0350353787183177984712M, -0.00222426529213447927281M, 0.0185573306514231072324M, 0.00950804701325919603619M, 0.00187123492819559223345M, 0.000157544617424960554631M, 0.00000460469890584317994083M, -0.000000000230404776911882601748M, 0.00000000000266339227425782031962M };
        private static readonly decimal[] ErvInvImpDd = { 1M, 1.3653349817554063097M, 0.762059164553623404043M, 0.220091105764131249824M, 0.0341589143670947727934M, 0.00263861676657015992959M, 0.0000764675292302794483503M };
        private static readonly decimal[] ErvInvImpEn = { -0.0167431005076633737133M, -0.00112951438745580278863M, 0.00105628862152492910091M, 0.000209386317487588078668M, 0.0000149624783758342370182M, 0.000000449696789927706453732M, 0.00000000462596163522878599135M, -0.000000000000281128735628831791805M, 0.00000000000000099055709973310326855M };
        private static readonly decimal[] ErvInvImpEd = { 1M, 0.591429344886417493481M, 0.138151865749083321638M, 0.0160746087093676504695M, 0.000964011807005165528527M, 0.0000275335474764726041141M, 0.000000282243172016108031869M };
        private static readonly decimal[] ErvInvImpFn = { -0.0024978212791898131227M, -0.00000779190719229053954292M, 0.0000254723037413027451751M, 0.00000162397777342510920873M, 0.0000000396341011304801168516M, 0.000000000411632831190944208473M, 0.00000000000145596286718675035587M, -0.00000000000000000116765012397184275695M };
        private static readonly decimal[] ErvInvImpFd = { 1M, 0.207123112214422517181M, 0.0169410838120975906478M, 0.000690538265622684595676M, 0.0000145007359818232637924M, 0.000000144437756628144157666M, 0.000000000509761276599778486139M };
        private static readonly decimal[] ErvInvImpGn = { -0.000539042911019078575891M, -0.00000028398759004727721098M, 0.000000899465114892291446442M, 0.0000000229345859265920864296M, 0.000000000225561444863500149219M, 0.000000000000947846627503022684216M, 0.00000000000000135880130108924861008M, -0.000000000000000000348890393399948882918M };
        private static readonly decimal[] ErvInvImpGd = { 1M, 0.0845746234001899436914M, 0.00282092984726264681981M, 0.0000468292921940894236786M, 0.000000399968812193862100054M, 0.00000000161809290887904476097M, 0.00000000000231558608310259605225M };

        public static decimal Erfc(decimal x)
        {
            if (x == 0) {
                return 1;
            }

            return ErfImp(x, true);
        }

        private static decimal ErfImp(decimal z, bool invert)
        {
            if (z < 0) {
                if (!invert) {
                    return -ErfImp(-z, false);
                }

                if (z < -0.5m) {
                    return 2 - ErfImp(-z, true);
                }

                return 1 + ErfImp(-z, false);
            }

            decimal result;

            if (z < 0.5m) {
                if (z < 0.0000000001m) {
                    result = (z * 1.125m) + (z * 0.003379167095512573896158903121545171688m);
                } else {
                    result = (z * 1.125m) + (z * Evaluate.Polynomial(z, ErfImpAn) / Evaluate.Polynomial(z, ErfImpAd));
                }
            } else if ((z < 110) || ((z < 110) && invert)) {
                invert = !invert;
                decimal r, b;
                if (z < 0.75m) {
                    r = Evaluate.Polynomial(z - 0.5m, ErfImpBn) / Evaluate.Polynomial(z - 0.5m, ErfImpBd);
                    b = 0.3440242112M;
                } else if (z < 1.25m) {
                    r = Evaluate.Polynomial(z - 0.75m, ErfImpCn) / Evaluate.Polynomial(z - 0.75m, ErfImpCd);
                    b = 0.419990927M;
                } else if (z < 2.25m) {
                    r = Evaluate.Polynomial(z - 1.25m, ErfImpDn) / Evaluate.Polynomial(z - 1.25m, ErfImpDd);
                    b = 0.4898625016M;
                } else if (z < 3.5m) {
                    r = Evaluate.Polynomial(z - 2.25m, ErfImpEn) / Evaluate.Polynomial(z - 2.25m, ErfImpEd);
                    b = 0.5317370892M;
                } else if (z < 5.25m) {
                    r = Evaluate.Polynomial(z - 3.5m, ErfImpFn) / Evaluate.Polynomial(z - 3.5m, ErfImpFd);
                    b = 0.5489973426M;
                } else if (z < 8) {
                    r = Evaluate.Polynomial(z - 5.25m, ErfImpGn) / Evaluate.Polynomial(z - 5.25m, ErfImpGd);
                    b = 0.5571740866M;
                } else if (z < 11.5m) {
                    r = Evaluate.Polynomial(z - 8, ErfImpHn) / Evaluate.Polynomial(z - 8, ErfImpHd);
                    b = 0.5609807968M;
                } else if (z < 17) {
                    r = Evaluate.Polynomial(z - 11.5m, ErfImpIn) / Evaluate.Polynomial(z - 11.5m, ErfImpId);
                    b = 0.5626493692M;
                } else if (z < 24) {
                    r = Evaluate.Polynomial(z - 17, ErfImpJn) / Evaluate.Polynomial(z - 17, ErfImpJd);
                    b = 0.5634598136M;
                } else if (z < 38) {
                    r = Evaluate.Polynomial(z - 24, ErfImpKn) / Evaluate.Polynomial(z - 24, ErfImpKd);
                    b = 0.5638477802M;
                } else if (z < 60) {
                    r = Evaluate.Polynomial(z - 38, ErfImpLn) / Evaluate.Polynomial(z - 38, ErfImpLd);
                    b = 0.5640528202M;
                } else if (z < 85) {
                    r = Evaluate.Polynomial(z - 60, ErfImpMn) / Evaluate.Polynomial(z - 60, ErfImpMd);
                    b = 0.5641309023M;
                } else {
                    r = Evaluate.Polynomial(z - 85, ErfImpNn) / Evaluate.Polynomial(z - 85, ErfImpNd);
                    b = 0.5641584396M;
                }

                decimal g = MathEx.Exp(-z * z) / z;
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

        public static decimal ErfcInv(decimal z)
        {
            if (z <= 0.0m) {
                return 79228162514264337593543950335M;
            }

            if (z >= 2.0m) {
                return -79228162514264337593543950335M;
            }

            decimal p, q, s;
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

        private static decimal ErfInvImpl(decimal p, decimal q, decimal s)
        {
            decimal result;

            if (p <= 0.5m) {
                const decimal y = 0.0891314744949340820313m;
                decimal g = p * (p + 10);
                decimal r = Evaluate.Polynomial(p, ErvInvImpAn) / Evaluate.Polynomial(p, ErvInvImpAd);
                result = (g * y) + (g * r);
            } else if (q >= 0.25m) {
                const decimal y = 2.249481201171875m;
                decimal g = MathEx.Sqrt(-2 * MathEx.Log(q));
                decimal xs = q - 0.25m;
                decimal r = Evaluate.Polynomial(xs, ErvInvImpBn) / Evaluate.Polynomial(xs, ErvInvImpBd);
                result = g / (y + r);
            } else {
                decimal x = MathEx.Sqrt(-MathEx.Log(q));
                if (x < 3) {
                    const decimal y = 0.807220458984375m;
                    decimal xs = x - 1.125m;
                    decimal r = Evaluate.Polynomial(xs, ErvInvImpCn) / Evaluate.Polynomial(xs, ErvInvImpCd);
                    result = (y * x) + (r * x);
                } else if (x < 6) {
                    const decimal y = 0.93995571136474609375m;
                    decimal xs = x - 3;
                    decimal r = Evaluate.Polynomial(xs, ErvInvImpDn) / Evaluate.Polynomial(xs, ErvInvImpDd);
                    result = (y * x) + (r * x);
                } else if (x < 18) {
                    const decimal y = 0.98362827301025390625m;
                    decimal xs = x - 6;
                    decimal r = Evaluate.Polynomial(xs, ErvInvImpEn) / Evaluate.Polynomial(xs, ErvInvImpEd);
                    result = (y * x) + (r * x);
                } else if (x < 44) {
                    const decimal y = 0.99714565277099609375m;
                    decimal xs = x - 18;
                    decimal r = Evaluate.Polynomial(xs, ErvInvImpFn) / Evaluate.Polynomial(xs, ErvInvImpFd);
                    result = (y * x) + (r * x);
                } else {
                    const decimal y = 0.99941349029541015625m;
                    decimal xs = x - 44;
                    decimal r = Evaluate.Polynomial(xs, ErvInvImpGn) / Evaluate.Polynomial(xs, ErvInvImpGd);
                    result = (y * x) + (r * x);
                }
            }

            return s * result;
        }

        public static decimal ErfInv(decimal z)
        {
            if (z == 0.0m) {
                return 0.0m;
            }

            if (z >= 1.0m) {
                return 79228162514264337593543950335M;
            }

            if (z <= -1.0m) {
                return -79228162514264337593543950335M;
            }

            decimal p, q, s;
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

        public static decimal Erf(decimal x)
        {
            if (x == 0) {
                return 0;
            }

            return ErfImp(x, false);
        }
    }
}
