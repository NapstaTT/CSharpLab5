namespace lab5.Data.Models
{
    public class Achievement
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public int G { get; set; }
        public int S { get; set; }
        public int B { get; set; }
        public int C { get; set; } 
        public int FC { get; set; }
        public int LC { get; set; }
        public int FLC { get; set; }
        public int LE { get; set; }
        public int FLE { get; set; }
        public int COC { get; set; }
        public int FCOC { get; set; }
        public int LK { get; set; }
        public int FLK { get; set; }
        
        public override string ToString() => $"Club {ClubId}: Gold={G}, Silver={S}, Bronze={B}, Cups={C}, Lost Final Cups={FC}, League of Champions={LC}, Final of League of Champions={FLC}, League of Europe={LE},\n Final Of League of Europe={FLE}, Cup of Cup Owners={COC}, Final of Cup Of Cup Owners={FCOC}, League of Confiderations={LK}, Finale of League of Confiderations={FLK}";
    }
}