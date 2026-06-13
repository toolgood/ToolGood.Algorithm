package toolgood.algorithm;

import org.joda.time.DateTime;

public class DemoApplication {
	public static void main(String[] args) throws Exception {

        AlgorithmEngine engine = new AlgorithmEngine();
        DateTime dt2 = engine.TryEvaluate("DATE(2024, 8, 1) + TIME(8, 0, 0)", DateTime.now());
        System.out.println(dt2);

        DateTime dt3 = engine.TryEvaluate("DATE(2024, 8, 1,1,2,3)", DateTime.now());
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