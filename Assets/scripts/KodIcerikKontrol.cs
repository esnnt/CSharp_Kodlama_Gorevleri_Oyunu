using System.Text.RegularExpressions;

public static class KodIcerikKontrol
{
    public static bool ToplamaKontrol(string kod)
    {
        // + operat�r� i�ermeli
        bool artiVar = kod.Contains("+");

        // En az iki say� olmal�
        var sayilar = Regex.Matches(kod, @"\d+");
        bool enAzIkiSayiVar = sayilar.Count >= 2;

        return artiVar && enAzIkiSayiVar;
    }
}
