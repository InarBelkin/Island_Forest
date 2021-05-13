using SFML.Graphics;
using SFML.System;

namespace Forest_Game.WorldMap
{
    ///<summary>Класс для хранения спрайтов игры</summary>
    internal static class SpriteCollection
    {
        private static readonly Texture TLanscape = new Texture("../Pictures/Landscape/Landscape_Atlas_Up.png");
        public static readonly Texture TLanscapeDown = new Texture("../Pictures/Landscape/Landscape_Atlas_Down.png");

        public static readonly Sprite SGrass_1U = new Sprite(TLanscape) { Origin = new Vector2f(32, 41), TextureRect = new IntRect(128, 0, 64, 50) };
        public static readonly Sprite SGrass_1D = new Sprite(TLanscapeDown) { Origin = new Vector2f(32, 41), TextureRect = new IntRect(128, 0, 64, 125) };

        public static readonly Sprite SSandGrass_1U = new Sprite(TLanscape) { Origin = new Vector2f(32, 41), TextureRect = new IntRect(1024, 0, 64, 55) };
        public static readonly Sprite SSandGrass_1D = new Sprite(TLanscapeDown) { Origin = new Vector2f(32, 41), TextureRect = new IntRect(1024, 0, 64, 125) };

        public static readonly Sprite SWater_1 = new Sprite(TLanscape) { Origin = new Vector2f(32, 41), TextureRect = new IntRect(1280, 0, 64, 50) };


        public static readonly Sprite SSand_1U = new Sprite(TLanscape) { Origin = new Vector2f(32, 41), TextureRect = new IntRect(384, 0, 64, 50) };
        public static readonly Sprite SSand_1D = new Sprite(TLanscapeDown) { Origin = new Vector2f(32, 41), TextureRect = new IntRect(384, 0, 64, 100) };

        public static readonly Sprite SStone_01U = new Sprite(TLanscape) { Origin = new Vector2f(32, 41), TextureRect = new IntRect(512, 0, 64, 50) };
        public static readonly Sprite SStone_01D = new Sprite(TLanscapeDown) { Origin = new Vector2f(32, 41), TextureRect = new IntRect(512, 0, 64, 4201) };

        public static readonly Sprite SGrassStone_01U = new Sprite(TLanscape) { Origin = new Vector2f(32, 41), TextureRect = new IntRect(768, 0, 64, 50) };
        public static readonly Sprite SGrassStone_01D = new Sprite(TLanscapeDown) { Origin = new Vector2f(32, 41), TextureRect = new IntRect(768, 0, 64, 420) };

        public static readonly Sprite SStoneSandU = new Sprite(TLanscape) { Origin = new Vector2f(32, 41), TextureRect = new IntRect(640, 0, 64, 50) };
        public static readonly Sprite SStoneSandD = new Sprite(TLanscapeDown) { Origin = new Vector2f(32, 41), TextureRect = new IntRect(640, 0, 64, 420) };

        public static readonly Sprite SSandStoneU = new Sprite(TLanscape) { Origin = new Vector2f(32, 41), TextureRect = new IntRect(896, 0, 64, 50) };
        public static readonly Sprite SSandStoneD = new Sprite(TLanscapeDown) { Origin = new Vector2f(32, 41), TextureRect = new IntRect(896, 0, 64, 420) };

        private static readonly Texture Nut = new Texture("../Pictures/Plants/Chestnut/ChestNutSprite.png");
        public static readonly Sprite Snut = new Sprite(Nut, new IntRect(0, 1110, 370, 370)) { Origin = new Vector2f(155, 295) };

        private static readonly Texture Carrot = new Texture("../Pictures/Plants/Carrot/Carrot.png");
        public static readonly Sprite SCarrot = new Sprite(Carrot, new IntRect(0, 41, 64, 41)) { Origin = new Vector2f(32, 41) };


        private static readonly Texture Rabbit = new Texture("../Pictures/Animals/Rabbit/Rabbit.png");
        public static readonly Sprite SRabbit = new Sprite(Rabbit, new IntRect(0, 200, 100, 100)) { Origin = new Vector2f(100, 155) };

        private static readonly Texture TWolf = new Texture("../Pictures/Animals/Wolf/Wolf.png");
        public static readonly Sprite SWolf = new Sprite(TWolf) { Origin = new Vector2f(100, 150) };

        private static readonly Texture TDeer = new Texture("../Pictures/Animals/Deer/Deer.png");
        public static readonly Sprite SDeer = new Sprite(TDeer) { Origin = new Vector2f(100, 150) };


    }
}
