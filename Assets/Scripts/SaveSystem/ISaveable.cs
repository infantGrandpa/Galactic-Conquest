namespace Abraham.GalacticConquest.SaveSystem
{
    public interface ISaveable
    {
        SaveData SerializeToSaveData();
        void DeserializeFromSaveData();
    }
}
