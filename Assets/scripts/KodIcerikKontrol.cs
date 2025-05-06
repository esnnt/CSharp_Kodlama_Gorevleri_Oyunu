using System.Text.RegularExpressions;

public static class KodIcerikKontrol
{
    public static bool ToplamaKontrol(string kod)
    {
        // + operatörü içermeli
        bool artiVar = kod.Contains("+");

        // En az iki sayý olmalý
        var sayilar = Regex.Matches(kod, @"\d+");
        bool enAzIkiSayiVar = sayilar.Count >= 2;

        return artiVar && enAzIkiSayiVar;
    }
}
