package toolgood.algorithm;


import toolgood.algorithm.Tests3.AlgorithmEngineHelperTest;

public class DemoApplication {
	public static void main(String[] args) throws Exception {

        AlgorithmEngine engine = new AlgorithmEngine();
        MyDate dt2 = engine.TryEvaluate("DATE(2024, 8, 1) + TIME(8, 0, 0)", MyDate.now());
        System.out.println(dt2);

        MyDate dt3 = engine.TryEvaluate("DATE(2024, 8, 1,1,2,3)", MyDate.now());
        System.out.println(dt3);


        AlgorithmEngine e=new AlgorithmEngine();
        int c = e.TryEvaluate("2+3//eee", 0);
        int c2 = e.TryEvaluate("2+3 ee", 0);

        int dt = e.TryEvaluate("IndexOf('abcd','cd')", -1);

        int t= e.TryEvaluate("1>2?1:2", 0);
        if (t!=2){
            System.out.println(t);
        }
    }
}