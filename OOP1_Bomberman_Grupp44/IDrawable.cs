namespace Bomberman;

public interface IDrawable
{
    // Varje draw-metod måste rita exakt 7 tecken, annars går allt knas
    void DrawLine1();
    void DrawLine2();
    void DrawLine3();
}