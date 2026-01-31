package toolgood.algorithm.unitConversion;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class UnitFactorSynonyms {
    public List<String> Synonyms = new ArrayList<>();

    public UnitFactorSynonyms() {
    }

    public UnitFactorSynonyms(String item) {
        Synonyms.add(item);
    }

    public UnitFactorSynonyms(String[] items) {
        Collections.addAll(Synonyms, items);
    }

    public boolean Contains(UnitFactorSynonyms synonyms) {
        for (String syn : synonyms.Synonyms) {
            if (this.Contains(syn)) {
                return true;
            }
        }
        return false;
    }

    private boolean Contains(String synonym) {
        for (String str : Synonyms) {
            if (str.equalsIgnoreCase(synonym)) {
                return true;
            }
        }
        return false;
    }


}
