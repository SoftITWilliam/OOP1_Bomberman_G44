
public class BotNameGenerator
{
    private Random random = new Random();

    private List<string> namesList = new()
    {
                
        "BOT Klara", 
        "BOT Moa", 

        "Bro",
        "BombGPT",
        "Crocodilo Bombardino",
        "Bombini Guzini",
        "NullReferenceException",
        
        "BOT Gunilla", 
        "BOT Ingrid",
        "BOT Yvonne",     
        "BOT Liselotte",
        "BOT Kerstin",
        "BOT Ulla",
        "BOT Berit",
        "BOT Britt-Marie",

        "BOT Henrik", 
        "BOT Rune", 
        "BOT Ove",
        "BOT Glenn",
        "BOT Rolf",
        "BOT Bert",
        "BOT Klas-Göran",
        "BOT John Lennon",
    };

    public string GetRandomName()
    {
        // Hämta ut ett slumpmässigt namn
        int i = random.Next(0, namesList.Count);
        string name = namesList[i];

        // Ta bort från listan av tillgängliga namn
        namesList.RemoveAt(i);
        return name;
    }
}