package toolgood.algorithm;


public class DemoApplication {
	public static void main(String[] args) throws Exception {
        AlgorithmEngine e=new AlgorithmEngine();
        int t= e.TryEvaluate("1>2?1:2", 0);
        if (t!=2){
            System.out.println(t);
        }
    }
}