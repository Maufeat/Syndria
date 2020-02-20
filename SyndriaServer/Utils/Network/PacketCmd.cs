namespace SyndriaServer.Utils.Network
{
    public enum S2C
    {
        welcome = 1,
        userData,
        createCharacter,
        toTutorial,
        toVillage,
        messageBox,
    }

    public enum C2S
    {
        verifyAccessToken = 1,
        setPrepCharacters = 50,
    }
}