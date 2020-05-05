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
        changeTurn,
        changeReadyState,
        spawnUnit,
        allLoaded,
        moveUnit,
        attack
    }

    public enum C2S
    {
        verifyAccessToken = 1,
        createCharacter,
        setPrepCharacter = 50,
        changeReadyState,
        battlefieldLoaded,
        moveUnit,
        attack
    }
}