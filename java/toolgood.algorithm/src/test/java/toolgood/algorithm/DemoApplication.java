package toolgood.algorithm;


// import org.springframework.core.io.ClassPathResource;
// import org.springframework.util.StopWatch;

public class DemoApplication {
	public static void main(String[] args) throws Exception {
        AlgorithmEngine e=new AlgorithmEngine();
        int t= e.TryEvaluate("1+2", 0);
        if (t!=3){
            System.out.println(t);
        }
    }
}