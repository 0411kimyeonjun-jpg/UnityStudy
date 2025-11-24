namespace 구구단
{
    internal class Program
    {
        static void Main()
        {
            for (int q = 1; q <= 9; q++)
            {
                Console.WriteLine($"\n {q} 단");

                for (int w = 1; w <= 9; w++)
                {
                    int e = q * w;
                    Console.WriteLine($"{q} x {w} = {e}");
                }
            }


        }
    }
}

