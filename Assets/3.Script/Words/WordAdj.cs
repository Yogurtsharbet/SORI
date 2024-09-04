public class WordAdj : Word {
    public WordAdj(string name, WordRank rank)
        : base(name, rank) {
        _type |= WordType.ADJ;
    }

}
