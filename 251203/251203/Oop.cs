namespace Program
{
    class Program
    {

        class Wizard
        {
            public int mp;
            public int intelligence;


            public Wizard()
            {
                mp = 50;
                intelligence = 20;
            }

        }
        // =======================================
        // 1. Wizard 클래스를 만들어 다음 조건을 만족하세요:
        // 
        // - 멤버 변수: mp, intelligence (둘 다 int형)
        // - 생성자에서 각각 50, 20으로 초기화
        // - Main()에서 Wizard 객체를 생성하고 두 값을 출력
        //
        // [실행 결과]
        // 마법사의 MP: 50, 지능: 20
        // =======================================


        static void Main()
        {
            Wizard wizard = new Wizard();
            Console.WriteLine($"마법사의 MP: {wizard.mp}, 지능 : {wizard.intelligence}");
        }
    }
}
