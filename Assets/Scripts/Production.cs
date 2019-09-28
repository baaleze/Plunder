using System.Collections.Generic;
public class Production {
    public List<Common.Resource> needs;
    public Common.Resource produces;

    public static Production[] prods = new Production[5] {
        Food(), Wood(), Ore(), Crystal(), Tools()
    };
    public static Production Wood() {
        Production p = new Production();
        p.needs = new List<Common.Resource>() {
            Common.Resource.FOOD,
            Common.Resource.TOOLS
        };
        p.produces = Common.Resource.WOOD;
        return p;
    }

    public static Production Ore() {
        Production p = new Production();
        p.needs = new List<Common.Resource>() {
            Common.Resource.FOOD,
            Common.Resource.WOOD
        };
        p.produces = Common.Resource.ORE;
        return p;
    }

    public static Production Food() {
        Production p = new Production();
        p.needs = new List<Common.Resource>() {
            Common.Resource.CRYSTAL,
            Common.Resource.TOOLS
        };
        p.produces = Common.Resource.FOOD;
        return p;
    }

    public static Production Tools() {
        Production p = new Production();
        p.needs = new List<Common.Resource>() {
            Common.Resource.WOOD,
            Common.Resource.ORE,
            Common.Resource.CRYSTAL
        };
        p.produces = Common.Resource.TOOLS;
        return p;
    }

    public static Production Crystal() {
        Production p = new Production();
        p.needs = new List<Common.Resource>() {
            Common.Resource.ORE,
            Common.Resource.TOOLS
        };
        p.produces = Common.Resource.CRYSTAL;
        return p;
    }

}