package toolgood.algorithm.unitConversion;

import java.util.ArrayList;
import java.util.List;

public class UnitFactorSynonyms {
    private final List<String> synonyms = new ArrayList<>();

    public UnitFactorSynonyms() {
    }

    public UnitFactorSynonyms(String... items) {
        for (String item : items) {
            synonyms.add(item);
        }
    }

    public boolean Contains(UnitFactorSynonyms synonyms) {
        for (String syn : synonyms.synonyms) {
            if (this.Contains(syn)) {
                return true;
            }
        }
        return false;
    }

    public boolean Contains(String synonym) {
        for (String str : synonyms) {
            if (str.equalsIgnoreCase(synonym)) {
                return true;
            }
        }
        return false;
    }

    @Override
    public String toString() {
        return String.join(", ", synonyms);
    }
}
